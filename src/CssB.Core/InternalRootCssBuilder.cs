using System;

namespace CssB.Core;

internal class InternalRootCssBuilder : InternalCssBuilder, IRootCssBuilder
{
    private readonly ClassRegistry _registry;

    internal InternalRootCssBuilder(ClassRegistry registry, string? theme, string? mediaQuery, string? mediaPrefix) 
        : base(registry, theme, mediaQuery, mediaPrefix)
    {
        _registry = registry;
    }

    public IRootCssBuilder ContainerInlineSizeName(string name)
    {
        throw new NotImplementedException();
    }

    public IRootCssBuilder ContainerSizeName(string name)
    {
        throw new NotImplementedException();
    }

    public IRootCssBuilder MinMd(Action<ICssBuilder> cssBuilderAction) => Media("width > 48rem", "min-md", cssBuilderAction);

    public IRootCssBuilder MaxMd(Action<ICssBuilder> cssBuilderAction) => Media("width < 48rem", "max-md", cssBuilderAction);

    public IRootCssBuilder Media(string query, string classPrefix, Action<IRootCssBuilder> cssBuilderAction)
    {
        InternalRootCssBuilder builder = new InternalRootCssBuilder(_registry, Theme, query, classPrefix);
        cssBuilderAction(builder);
        AddClassName(builder.ToString());
        return this;
    }
}