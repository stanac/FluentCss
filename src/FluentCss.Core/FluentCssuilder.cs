namespace FluentCss.Core;

public static class FluentCssuilder
{
    public static IRootFluentCssuilder Css => new InternalRootFluentCssuilder(ClassRegistry.DefaultRegistry, null, null, null);

    public static IRootFluentCssuilder Dark => Theme("dark");

    public static IRootFluentCssuilder Theme(string theme) => new InternalRootFluentCssuilder(ClassRegistry.DefaultRegistry, $"theme-{theme}", null, null);
}