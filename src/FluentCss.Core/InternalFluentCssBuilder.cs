using System;

namespace FluentCss.Core;

internal class InternalFluentCssBuilder : BaseStyleBuilder<IFluentCssBuilder>, IFluentCssBuilder
{
    private readonly ClassRegistry _registry;

    internal InternalFluentCssBuilder(ClassRegistry registry, string? theme, string? mediaQuery, string? mediaPrefix) : base(theme, mediaQuery, mediaPrefix)
    {
        _registry = registry;
    }

    protected internal override void AddClass(UtilityClass c)
    {
        c = c.AddTheme(Theme).AddMedia(MediaQuery, MediaPrefix);
        _registry.Add(c);
    }

    protected override IFluentCssBuilder ReturnSelf() => this;

    protected override IStyleScopeBuilder CreateImportantScope() =>
        new ImportantScopeBuilder(_registry, Theme, MediaQuery, MediaPrefix, null);

    public IFluentCssBuilder Pseudo(string pseudoPrefix, Action<IOnlyStyleBuilder> pseudoBuilderAction)
    {
        IOnlyStyleBuilder builder = new OnlyStyleBuilder(_registry, Theme, MediaQuery, MediaPrefix, pseudoPrefix);
        pseudoBuilderAction(builder);
        base.AddClassName(builder.ToString());
        return ReturnSelf();
    }
}