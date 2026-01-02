using System;

namespace CssB.Core;

internal class InternalCssBuilder : BaseStyleBuilder<ICssBuilder>, ICssBuilder
{
    private readonly ClassRegistry _registry;

    internal InternalCssBuilder(ClassRegistry registry, string? theme, string? mediaQuery, string? mediaPrefix) : base(theme, mediaQuery, mediaPrefix)
    {
        _registry = registry;
    }

    protected internal override void AddClass(UtilityClass c)
    {
        c = c.AddTheme(Theme).AddMedia(MediaQuery, MediaPrefix);
        _registry.Add(c);
    }

    protected override ICssBuilder ReturnSelf() => this;

    public ICssBuilder Hover(Action<IOnlyStyleBuilder> pseudoBuilderAction) => Pseudo("hover", pseudoBuilderAction);

    public ICssBuilder Active(Action<IOnlyStyleBuilder> pseudoBuilderAction) => Pseudo("active", pseudoBuilderAction);

    public ICssBuilder Focus(Action<IOnlyStyleBuilder> pseudoBuilderAction) => Pseudo("focus", pseudoBuilderAction);

    public ICssBuilder Pseudo(string pseudoPrefix, Action<IOnlyStyleBuilder> pseudoBuilderAction)
    {
        IOnlyStyleBuilder builder = new OnlyStyleBuilder(_registry, Theme, MediaQuery, MediaPrefix, pseudoPrefix);
        pseudoBuilderAction(builder);
        base.AddClassName(builder.ToString());
        return ReturnSelf();
    }
}