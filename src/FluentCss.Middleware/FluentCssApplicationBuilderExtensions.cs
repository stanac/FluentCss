using Microsoft.AspNetCore.Builder;

namespace FluentCss.Middleware;

public static class FluentCssApplicationBuilderExtensions
{
    /// <summary>
    /// Adds the middleware that serves the stylesheet and its current hash.
    /// </summary>
    public static IApplicationBuilder UseFluentCss(this IApplicationBuilder app) =>
        app.UseMiddleware<FluentCssMiddleware>();
}
