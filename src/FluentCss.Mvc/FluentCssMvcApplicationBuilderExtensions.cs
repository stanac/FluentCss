using Microsoft.AspNetCore.Builder;

namespace FluentCss.Mvc;

public static class FluentCssMvcApplicationBuilderExtensions
{
    /// <summary>
    /// Adds the middleware that serves the stylesheet and its current hash.
    /// </summary>
    public static IApplicationBuilder UseFluentCssMvc(this IApplicationBuilder app) =>
        app.UseMiddleware<FluentCssMiddleware>();
}
