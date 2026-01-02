namespace CssB.Core;

public static class CssBuilder
{
    public static IRootCssBuilder Css => new InternalRootCssBuilder(ClassRegistry.DefaultRegistry, null, null, null);

    public static IRootCssBuilder Dark => Theme("dark");

    public static IRootCssBuilder Theme(string theme) => new InternalRootCssBuilder(ClassRegistry.DefaultRegistry, $"theme-{theme}", null, null);
}