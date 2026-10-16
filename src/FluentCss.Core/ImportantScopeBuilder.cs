namespace FluentCss.Core;

/// <summary>
/// The builder used as the scope of an <c>!important</c> block. It behaves like
/// <see cref="OnlyStyleBuilder"/> but every utility it registers is marked
/// important, and it exposes no <c>Important</c> method (see
/// <see cref="IStyleScopeBuilder"/>), so override blocks cannot nest.
/// </summary>
internal sealed class ImportantScopeBuilder : BaseStyleBuilder<IStyleScopeBuilder>, IStyleScopeBuilder
{
    private readonly ClassRegistry _registry;
    private readonly string? _pseudo;
    private readonly List<string> _classNames = [];

    internal ImportantScopeBuilder(ClassRegistry registry, string? theme, string? mediaQuery, string? mediaPrefix, string? pseudo)
        : base(theme, mediaQuery, mediaPrefix)
    {
        _registry = registry;
        _pseudo = pseudo;
    }

    protected internal override void AddClass(UtilityClass c)
    {
        c = c.AddTheme(Theme).AddMedia(MediaQuery, MediaPrefix).AddPseudo(_pseudo).AddImportant();
        _classNames.Add(c.ClassName);
        _registry.Add(c);
    }

    protected override IStyleScopeBuilder ReturnSelf() => this;

    // Unreachable through IStyleScopeBuilder (which has no Important), but the base
    // class requires it.
    protected override IStyleScopeBuilder CreateImportantScope() =>
        new ImportantScopeBuilder(_registry, Theme, MediaQuery, MediaPrefix, _pseudo);

    public override string ToString() => string.Join(" ", _classNames);
}
