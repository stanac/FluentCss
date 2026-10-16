namespace FluentCss.Core;

internal class OnlyStyleBuilder : BaseStyleBuilder<IOnlyStyleBuilder>, IOnlyStyleBuilder
{
    private readonly ClassRegistry _registry;
    private readonly string? _pseudo;
    private readonly List<string> _classNames = [];

    internal OnlyStyleBuilder(ClassRegistry registry, string? theme, string? mediaQuery, string? mediaPrefix, string? pseudo) 
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

    protected override IStyleScopeBuilder CreateImportantScope() =>
        new ImportantScopeBuilder(_registry, Theme, MediaQuery, MediaPrefix, _pseudo);

    protected override void AddChildClassNames(string classNames) => _classNames.Add(classNames);

    public override string ToString() => string.Join(" ", _classNames);
}