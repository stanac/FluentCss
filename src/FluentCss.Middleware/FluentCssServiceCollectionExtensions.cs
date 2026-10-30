using Microsoft.Extensions.DependencyInjection;

namespace FluentCss.Middleware;

public static class FluentCssServiceCollectionExtensions
{
    /// <summary>
    /// Registers the services FluentCss needs on the server.
    /// </summary>
    public static IServiceCollection AddFluentCss(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        return services;
    }
}
