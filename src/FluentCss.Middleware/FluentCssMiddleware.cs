using System.IO.Compression;
using System.Text;
using FluentCss.Core;
using Microsoft.AspNetCore.Http;

namespace FluentCss.Middleware;

/// <summary>
/// Serves the FluentCss stylesheet and its current hash.
/// </summary>
/// <remarks>
/// <para><c>GET /fluentcss/hash</c> returns the current hash as plain text.</para>
/// <para>
/// <c>GET /fluentcss/{hash}.css</c> returns the generated stylesheet. When the
/// requested hash matches the current one the response is immutable and cached
/// for a year. When it does not match (for example because the page registered
/// more classes after the link was written) the current stylesheet is served
/// instead, marked <c>no-store</c>. The union of classes only ever grows, so
/// serving a newer stylesheet is always safe; we never serve an older one in
/// place of a newer one.
/// </para>
/// <para>
/// The preflight reset is not part of this stylesheet. It is shipped as a static
/// asset in the <c>FluentCss</c> package at <c>_content/FluentCss/preflight.css</c>
/// and linked separately.
/// </para>
/// </remarks>
public sealed class FluentCssMiddleware
{
    private const string PathPrefix = "/fluentcss";

    private readonly RequestDelegate _next;

    private readonly object _cacheLock = new();
    private string? _cachedHash;
    private byte[]? _cachedIdentity;
    private byte[]? _cachedGzip;

    public FluentCssMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Path.StartsWithSegments(PathPrefix, out var remaining))
        {
            await _next(context);
            return;
        }

        var method = context.Request.Method;
        if (!HttpMethods.IsGet(method) && !HttpMethods.IsHead(method))
        {
            await _next(context);
            return;
        }

        var segment = remaining.Value;
        if (!string.IsNullOrEmpty(segment))
        {
            segment = segment.TrimStart('/');
        }

        if (string.Equals(segment, "hash", StringComparison.OrdinalIgnoreCase))
        {
            await WriteHashAsync(context);
            return;
        }

        if (!string.IsNullOrEmpty(segment) &&
            segment.EndsWith(".css", StringComparison.OrdinalIgnoreCase))
        {
            await WriteCssAsync(context, segment[..^4]);
            return;
        }

        await _next(context);
    }

    private static Task WriteHashAsync(HttpContext context)
    {
        var bytes = Encoding.UTF8.GetBytes(ClassRegistry.DefaultRegistry.Sha256Hex);

        context.Response.StatusCode = StatusCodes.Status200OK;
        context.Response.ContentType = "text/plain; charset=utf-8";
        context.Response.Headers.CacheControl = "no-store";
        context.Response.ContentLength = bytes.Length;

        return HttpMethods.IsHead(context.Request.Method)
            ? Task.CompletedTask
            : context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
    }

    private Task WriteCssAsync(HttpContext context, string requestedHash)
    {
        var registry = ClassRegistry.DefaultRegistry;
        var currentHash = registry.Sha256Hex;
        var isMatch = string.Equals(requestedHash, currentHash, StringComparison.OrdinalIgnoreCase);
        var etag = "\"" + currentHash + "\"";

        context.Response.Headers.ETag = etag;
        context.Response.Headers.CacheControl = isMatch
            ? "public, max-age=31536000, immutable"
            : "no-store";
        context.Response.Headers.Vary = "Accept-Encoding";

        if (isMatch && IsNotModified(context, etag))
        {
            context.Response.StatusCode = StatusCodes.Status304NotModified;
            return Task.CompletedTask;
        }

        var useGzip = AcceptsGzip(context.Request);
        var payload = GetPayload(currentHash, useGzip);

        if (useGzip)
        {
            context.Response.Headers.ContentEncoding = "gzip";
        }

        context.Response.StatusCode = StatusCodes.Status200OK;
        context.Response.ContentType = "text/css; charset=utf-8";
        context.Response.ContentLength = payload.Length;

        return HttpMethods.IsHead(context.Request.Method)
            ? Task.CompletedTask
            : context.Response.Body.WriteAsync(payload, 0, payload.Length);
    }

    private byte[] GetPayload(string hash, bool useGzip)
    {
        lock (_cacheLock)
        {
            if (_cachedHash != hash || _cachedIdentity is null || _cachedGzip is null)
            {
                _cachedIdentity = Encoding.UTF8.GetBytes(ClassRegistry.DefaultRegistry.Css);
                _cachedGzip = Compress(_cachedIdentity);
                _cachedHash = hash;
            }

            return useGzip ? _cachedGzip : _cachedIdentity;
        }
    }

    private static byte[] Compress(byte[] bytes)
    {
        using var output = new MemoryStream();

        using (var gzip = new GZipStream(output, CompressionLevel.Optimal, leaveOpen: true))
        {
            gzip.Write(bytes, 0, bytes.Length);
        }

        return output.ToArray();
    }

    private static bool IsNotModified(HttpContext context, string etag)
    {
        foreach (var value in context.Request.Headers.IfNoneMatch)
        {
            if (string.Equals(value, etag, StringComparison.Ordinal) ||
                string.Equals(value, "W/" + etag, StringComparison.Ordinal))
            {
                return true;
            }
        }

        return false;
    }

    private static bool AcceptsGzip(HttpRequest request)
    {
        foreach (var value in request.Headers.AcceptEncoding)
        {
            if (string.IsNullOrEmpty(value))
            {
                continue;
            }

            foreach (var part in value.Split(','))
            {
                var token = part.Trim();
                var semicolon = token.IndexOf(';');

                if (semicolon >= 0)
                {
                    token = token[..semicolon].Trim();
                }

                if (token.Equals("gzip", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
