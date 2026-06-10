using System;

namespace FluentCss.Core;

internal class InternalFluentCssuilder : BaseStyleBuilder<IFluentCssuilder>, IFluentCssuilder
{
    private readonly ClassRegistry _registry;

    internal InternalFluentCssuilder(ClassRegistry registry, string? theme, string? mediaQuery, string? mediaPrefix) : base(theme, mediaQuery, mediaPrefix)
    {
        _registry = registry;
    }

    protected internal override void AddClass(UtilityClass c)
    {
        c = c.AddTheme(Theme).AddMedia(MediaQuery, MediaPrefix);
        _registry.Add(c);
    }

    protected override IFluentCssuilder ReturnSelf() => this;

    public IFluentCssuilder Hover(Action<IOnlyStyleBuilder> pseudoBuilderAction) => Pseudo("hover", pseudoBuilderAction);

    public IFluentCssuilder Active(Action<IOnlyStyleBuilder> pseudoBuilderAction) => Pseudo("active", pseudoBuilderAction);

    public IFluentCssuilder Focus(Action<IOnlyStyleBuilder> pseudoBuilderAction) => Pseudo("focus", pseudoBuilderAction);

    public IFluentCssuilder Pseudo(string pseudoPrefix, Action<IOnlyStyleBuilder> pseudoBuilderAction)
    {
        IOnlyStyleBuilder builder = new OnlyStyleBuilder(_registry, Theme, MediaQuery, MediaPrefix, pseudoPrefix);
        pseudoBuilderAction(builder);
        base.AddClassName(builder.ToString());
        return ReturnSelf();
    }
}