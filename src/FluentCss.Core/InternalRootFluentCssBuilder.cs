namespace FluentCss.Core;

internal class InternalRootFluentCssBuilder : InternalFluentCssBuilder, IRootFluentCssBuilder
{
    private readonly ClassRegistry _registry;
    
    internal InternalRootFluentCssBuilder(ClassRegistry registry, string? theme, string? mediaQuery, string? mediaPrefix)
        : base(registry, theme, mediaQuery, mediaPrefix)
    {
        _registry = registry;
    }

    public IMediaFluentCssBuilder Media(string query, string classPrefix, Action<IMediaFluentCssBuilder> cssBuilderAction)
    {
        InternalRootFluentCssBuilder builder = new InternalRootFluentCssBuilder(_registry, Theme, query, classPrefix);
        cssBuilderAction(builder);
        AddClassName(builder.ToString());
        return this;
    }

    public IRootFluentCssBuilder ApplySelector(string selector, Action<IOnlyStyleBuilder> cssBuilderAction)
    {
        ClassRegistry tempRegistry = new ClassRegistry();
        IOnlyStyleBuilder builder = new OnlyStyleBuilder(tempRegistry, null, null, null, null);
        cssBuilderAction(builder);

        _registry.AddSelector(selector, tempRegistry.ToArray());

        return this;
    }
}