using System.Collections.Generic;

namespace CssB.Core;

internal class OnlyStyleBuilder : BaseStyleBuilder<IOnlyStyleBuilder>, IOnlyStyleBuilder
{
    private readonly ClassRegistry _registry;
    private readonly string _pseudo;
    private readonly List<string> _classNames = [];

    internal OnlyStyleBuilder(ClassRegistry registry, string? theme, string? mediaQuery, string? mediaPrefix, string pseudo) 
        : base(theme, mediaQuery, mediaPrefix)
    {
        _registry = registry;
        _pseudo = pseudo;
    }

    protected internal override void AddClass(UtilityClass c)
    {
        c = c.AddTheme(Theme).AddMedia(MediaQuery, MediaPrefix).AddPseudo(_pseudo);
        _classNames.Add(c.ClassName);
        _registry.Add(c);
    }

    protected override IOnlyStyleBuilder ReturnSelf() => this;

    public override string ToString() => string.Join(" ", _classNames);
}