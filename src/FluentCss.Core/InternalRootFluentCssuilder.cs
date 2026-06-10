using System;

namespace FluentCss.Core;

internal class InternalRootFluentCssuilder : InternalFluentCssuilder, IRootFluentCssuilder
{
    private readonly ClassRegistry _registry;

    internal InternalRootFluentCssuilder(ClassRegistry registry, string? theme, string? mediaQuery, string? mediaPrefix) 
        : base(registry, theme, mediaQuery, mediaPrefix)
    {
        _registry = registry;
    }

    public IRootFluentCssuilder ContainerInlineSizeName(string name)
    {
        throw new NotImplementedException();
    }

    public IRootFluentCssuilder ContainerSizeName(string name)
    {
        throw new NotImplementedException();
    }

    public IRootFluentCssuilder MinMd(Action<IFluentCssuilder> cssBuilderAction) => Media("width > 48rem", "min-md", cssBuilderAction);

    public IRootFluentCssuilder MaxMd(Action<IFluentCssuilder> cssBuilderAction) => Media("width < 48rem", "max-md", cssBuilderAction);

    public IRootFluentCssuilder Media(string query, string classPrefix, Action<IRootFluentCssuilder> cssBuilderAction)
    {
        InternalRootFluentCssuilder builder = new InternalRootFluentCssuilder(_registry, Theme, query, classPrefix);
        cssBuilderAction(builder);
        AddClassName(builder.ToString());
        return this;
    }
}