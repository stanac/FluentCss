using FluentCss.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace FluentCss.Middleware;

/// <summary>
/// Applies the persisted theme class to the <c>html</c> element, so a dark theme
/// is already active in the server-rendered markup instead of being added by
/// script after the first paint.
/// </summary>
/// <remarks>
/// The class is read from the <see cref="FluentCssTheme.CookieName"/> cookie that
/// the theme toggle script writes. Registered by the
/// <c>@addTagHelper *, FluentCss.Middleware</c> directive already used by the
/// layouts; no markup change is required.
/// </remarks>
[HtmlTargetElement("html")]
public sealed class FluentCssThemeTagHelper : TagHelper
{
    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; } = default!;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var htmlClass = ViewContext.HttpContext.GetFluentCssThemeClass();

        if (htmlClass.Length == 0)
        {
            return;
        }

        var existing = output.Attributes["class"]?.Value?.ToString();

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
        context.Request.Cookies.TryGetValue(FluentCssTheme.CookieName, out var value) ? value : null;

    /// <summary>
    /// Returns the html theme class for the request cookie, or an empty string.
    /// </summary>
    public static string GetFluentCssThemeClass(this HttpContext context) =>
        FluentCssTheme.GetHtmlClass(context.GetFluentCssTheme());
}
