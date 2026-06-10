using System;

namespace FluentCss.Core;

internal class InternalRootFluentCssBuilder : InternalFluentCssBuilder, IRootFluentCssBuilder
{
    private readonly ClassRegistry _registry;

    internal InternalRootFluentCssBuilder(ClassRegistry registry, string? theme, string? mediaQuery, string? mediaPrefix) 
        : base(registry, theme, mediaQuery, mediaPrefix)
    {
        _registry = registry;
    }

    public IRootFluentCssBuilder ContainerInlineSizeName(string name)
    {
        throw new NotImplementedException();
    }

    public IRootFluentCssBuilder ContainerSizeName(string name)
    {
        throw new NotImplementedException();
    }

    public IRootFluentCssBuilder MinMd(Action<IFluentCssBuilder> cssBuilderAction) => Media("width > 48rem", "min-md", cssBuilderAction);

    public IRootFluentCssBuilder MaxMd(Action<IFluentCssBuilder> cssBuilderAction) => Media("width < 48rem", "max-md", cssBuilderAction);

    public IRootFluentCssBuilder Media(string query, string classPrefix, Action<IRootFluentCssBuilder> cssBuilderAction)
    {
        InternalRootFluentCssBuilder builder = new InternalRootFluentCssBuilder(_registry, Theme, query, classPrefix);
        cssBuilderAction(builder);
        AddClassName(builder.ToString());
        return this;
    }
}