using Microsoft.AspNetCore.Builder;

namespace FluentCss.MvcRazor;

public static class FluentCssMvcRazorApplicationBuilderExtensions
{
    /// <summary>
    /// Adds the middleware that serves the stylesheet and its current hash.
    /// </summary>
    public static IApplicationBuilder UseFluentCssMvcRazor(this IApplicationBuilder app) =>
        app.UseMiddleware<FluentCssMiddleware>();
}
