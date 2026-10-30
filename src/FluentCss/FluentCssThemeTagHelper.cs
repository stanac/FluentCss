using FluentCss.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;

namespace FluentCss;

/// <summary>
/// Applies the persisted theme class to the <c>html</c> element, so a dark theme
/// is already active in the server-rendered markup instead of being added by
/// script after the first paint.
/// </summary>
/// <remarks>
/// <para>
/// The class is read from the <see cref="FluentCssTheme.CookieName"/> cookie that
/// the theme toggle script writes. Registered by the
/// <c>@addTagHelper *, FluentCss</c> directive already used by the layouts; no
/// markup change is required.
/// </para>
/// <para>
/// The HTTP context is obtained through <see cref="IHttpContextAccessor"/> rather
/// than an MVC <c>ViewContext</c> so this assembly keeps no reference to
/// <c>Microsoft.AspNetCore.Mvc.*</c>. That keeps Blazor WebAssembly consumers
/// working: the MVC application-parts discovery would otherwise treat this
/// assembly as an MVC application part.
/// </para>
/// <para>
/// Register the accessor with <c>services.AddFluentCss()</c> (from
/// <c>FluentCss.Middleware</c>) or <c>services.AddHttpContextAccessor()</c>. When
/// it is not registered the tag helper does nothing and the client-side theme
/// script still applies the class.
/// </para>
/// </remarks>
[HtmlTargetElement("html")]
public sealed class FluentCssThemeTagHelper(IServiceProvider services) : TagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        HttpContext? httpContext = services.GetService<IHttpContextAccessor>()?.HttpContext;

        if (httpContext is null)
        {
            return;
        }

        string htmlClass = httpContext.GetFluentCssThemeClass();

        if (htmlClass.Length == 0)
        {
            return;
        }

        string? existing = output.Attributes["class"]?.Value?.ToString();

        output.Attributes.SetAttribute(
            "class",
            string.IsNullOrWhiteSpace(existing) ? htmlClass : existing + " " + htmlClass);
    }
}

/// <summary>HttpContext helpers for the persisted FluentCss theme cookie.</summary>
public static class FluentCssThemeHttpContextExtensions
{
    /// <summary>Returns the theme stored in the request cookie, or null.</summary>
    public static string? GetFluentCssTheme(this HttpContext context) =>
        context.Request.Cookies.TryGetValue(FluentCssTheme.CookieName, out string? value) ? value : null;

    /// <summary>
    /// Returns the html theme class for the request cookie, or an empty string.
    /// </summary>
    public static string GetFluentCssThemeClass(this HttpContext context) =>
        FluentCssTheme.GetHtmlClass(context.GetFluentCssTheme());
}
