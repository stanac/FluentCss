namespace FluentCss.Core;

public static class FluentCssBuilder
{
    public static IRootFluentCssBuilder Css => new InternalRootFluentCssBuilder(ClassRegistry.DefaultRegistry, null, null, null);

    public static IRootFluentCssBuilder Dark => Theme("dark");

    public static IRootFluentCssBuilder Theme(string theme) => new InternalRootFluentCssBuilder(ClassRegistry.DefaultRegistry, $"theme-{theme}", null, null);
}