namespace FluentCss.Core.Tests;

/// <summary>
/// The <c>Important(...)</c> override blocks. The lambda receives an
/// <see cref="IStyleScopeBuilder"/>, which has no <c>Important</c>, so blocks cannot
/// nest; several declarations can still be added in one block.
/// </summary>
public class ImportantStyleBuilderTests
{
    private static InternalRootFluentCssBuilder CreateSut(
        out ClassRegistry registry,
        string? theme = null,
        string? mediaQuery = null,
        string? mediaPrefix = null)
    {
        registry = new ClassRegistry();
        return new InternalRootFluentCssBuilder(registry, theme, mediaQuery, mediaPrefix);
    }

    [Fact]
    public void Important_Should_Emit_The_Important_Flag()
    {
        var sut = CreateSut(out ClassRegistry registry);

        sut.Important(c => c.P(4));

        registry.Css.Should().Contain(".\\!p-4 { padding: 1rem !important }");
    }

    [Fact]
    public void Important_Should_Return_An_Important_Class_Name()
    {
        var sut = CreateSut(out _);

        string classes = sut.Important(c => c.P(4)).ToString() ?? string.Empty;

        classes.Should().Be("!p-4");
    }

    [Fact]
    public void Important_Should_Accept_Multiple_Declarations()
    {
        var sut = CreateSut(out ClassRegistry registry);

        string classes = sut.Important(c => c.P(4).M(2)).ToString() ?? string.Empty;

        classes.Should().Be("!p-4 !m-2");
        registry.Css.Should().Contain(".\\!p-4 { padding: 1rem !important }");
        registry.Css.Should().Contain(".\\!m-2 { margin: 0.5rem !important }");
    }

    [Fact]
    public void Important_Should_Not_Collide_With_The_Normal_Utility()
    {
        var sut = CreateSut(out ClassRegistry registry);

        sut.P(4);
        sut.Important(c => c.P(4));

        registry.Css.Should().Contain(".p-4 { padding: 1rem }");
        registry.Css.Should().Contain(".\\!p-4 { padding: 1rem !important }");
    }

    [Fact]
    public void Important_Should_Work_Inside_Hover()
    {
        var sut = CreateSut(out ClassRegistry registry);

        sut.Hover(h => h.Important(c => c.Background(Color.Blue600)));

        registry.Css.Should().Contain(
            $".hover\\:\\!bg-blue-600:hover {{ background-color: {Color.Blue600.Value} !important }}");
    }

    [Fact]
    public void Important_Should_Apply_The_Theme()
    {
        var sut = CreateSut(out ClassRegistry registry, theme: "theme-dark");

        sut.Important(c => c.Background(Color.Slate800));

        registry.Css.Should().Contain(
            $".theme-dark .theme-dark\\:\\!bg-slate-800 {{ background-color: {Color.Slate800.Value} !important }}");
    }

    [Fact]
    public void Important_Should_Wrap_The_Declaration_In_Media()
    {
        var sut = CreateSut(out ClassRegistry registry, mediaQuery: "(min-width: 48rem)", mediaPrefix: "min-md");

        sut.Important(c => c.P(4));

        registry.Css.Should().Contain(
            "@media (min-width: 48rem) { .min-md\\:\\!p-4 { padding: 1rem !important } }");
    }

    [Fact]
    public void Important_Should_Do_Nothing_For_An_Empty_Scope()
    {
        var sut = CreateSut(out ClassRegistry registry);

        string classes = sut.Important(_ => { }).ToString() ?? string.Empty;

        classes.Should().BeEmpty();
        registry.Css.Should().NotContain("!important");
    }

    [Fact]
    public void Important_Should_Work_With_Arbitrary()
    {
        var sut = CreateSut(out ClassRegistry registry);

        sut.Important(c => c.Arbitrary("font-350", "font-weight", "350"));

        registry.Css.Should().Contain(".\\!font-350 { font-weight: 350 !important }");
    }

    [Fact]
    public void The_Important_Scope_Should_Not_Expose_Important()
    {
        // Compile-time prevention of Important-inside-Important is the point of the
        // dedicated scope interface; assert it stays that way.
        typeof(IImportantStyleBuilder<IOnlyStyleBuilder>).IsAssignableFrom(typeof(IOnlyStyleBuilder))
            .Should().BeTrue();
        typeof(IImportantStyleBuilder<IStyleScopeBuilder>).IsAssignableFrom(typeof(IStyleScopeBuilder))
            .Should().BeFalse();
    }
}
