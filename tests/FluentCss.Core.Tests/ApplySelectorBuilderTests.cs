namespace FluentCss.Core.Tests;

/// <summary>
/// The <see cref="IRootFluentCssBuilder"/> overload of <c>ApplySelector</c> that
/// takes the fluent selector builder instead of a raw selector string.
/// </summary>
public class ApplySelectorBuilderTests
{
    private static InternalRootFluentCssBuilder CreateSut(out ClassRegistry registry)
    {
        registry = new ClassRegistry();
        return new InternalRootFluentCssBuilder(registry, null, null, null);
    }

    [Fact]
    public void ApplySelector_Should_Apply_Declarations_Under_A_Compound_Selector()
    {
        var sut = CreateSut(out ClassRegistry registry);

        sut.ApplySelector(s => s.Class("btn").Hover(), css => css.P(4));

        registry.Css.Should().Contain(".btn:hover { padding: 1rem }");
    }

    [Fact]
    public void ApplySelector_Should_Prefix_The_Theme()
    {
        var sut = CreateSut(out ClassRegistry registry);

        sut.ApplySelector(s => s.Theme("dark").Class("card"), css => css.P(4));

        registry.Css.Should().Contain(".theme-dark .card { padding: 1rem }");
    }

    [Fact]
    public void ApplySelector_Should_Nest_Declarations_Inside_The_At_Rule()
    {
        var sut = CreateSut(out ClassRegistry registry);

        sut.ApplySelector(s => s.Media("(min-width: 48rem)").Class("card"), css => css.P(4));

        registry.Css.Should().Contain("@media (min-width: 48rem) { .card { padding: 1rem } }");
    }

    [Fact]
    public void ApplySelector_Should_Nest_Theme_And_Media_Together()
    {
        var sut = CreateSut(out ClassRegistry registry);

        sut.ApplySelector(s => s.Media("(min-width: 48rem)").Dark().Class("card").Hover(), css => css.P(4));

        registry.Css.Should().Contain("@media (min-width: 48rem) { .theme-dark .card:hover { padding: 1rem } }");
    }

    [Fact]
    public void ApplySelector_Should_Support_Targets()
    {
        var sut = CreateSut(out ClassRegistry registry);

        sut.ApplySelector(
            s => s.Targets(t => t.Class("a"), t => t.Class("b")),
            css => css.P(4));

        registry.Css.Should().Contain(".a, .b { padding: 1rem }");
    }

    [Fact]
    public void ApplySelector_Should_Support_Relative_Targets()
    {
        var sut = CreateSut(out ClassRegistry registry);

        sut.ApplySelector(
            s => s.Class("card").Has(r => r.Child().Element("img")),
            css => css.P(4));

        registry.Css.Should().Contain(".card:has(> img) { padding: 1rem }");
    }

    [Fact]
    public void ApplySelector_Should_Return_The_Same_Root_Builder_For_Chaining()
    {
        var sut = CreateSut(out ClassRegistry registry);

        IRootFluentCssBuilder result = sut.ApplySelector(s => s.Class("btn"), css => css.P(4));

        result.Should().BeSameAs(sut);

        result.ApplySelector(s => s.Class("card"), css => css.M(2));

        registry.Css.Should().Contain(".btn { padding: 1rem }");
        registry.Css.Should().Contain(".card { margin: 0.5rem }");
    }

    [Fact]
    public void ApplySelector_Should_Keep_Important_Declarations()
    {
        var sut = CreateSut(out ClassRegistry registry);

        sut.ApplySelector(".btn", css => css.Important(c => c.P(4)));

        registry.Css.Should().Contain(".btn { padding: 1rem !important }");
    }

    [Fact]
    public void ApplySelector_Should_Mix_Important_And_Normal_Declarations()
    {
        var sut = CreateSut(out ClassRegistry registry);

        sut.ApplySelector(".btn", css => css.P(4).Important(c => c.M(2)));

        registry.Css.Should().Contain(".btn { padding: 1rem; margin: 0.5rem !important }");
    }

    [Fact]
    public void ApplySelector_Should_Support_Attribute_Selector_With_Important()
    {
        var sut = CreateSut(out ClassRegistry registry);

        sut.ApplySelector(s => s.Attribute("hidden"), css => css.Important(c => c.Hidden()));

        registry.Css.Should().Contain("[hidden] { display: none !important }");
    }

    [Fact]
    public void ApplySelector_Should_Throw_For_An_Empty_Selector()
    {
        var sut = CreateSut(out _);

        Action act = () => sut.ApplySelector(_ => { }, css => css.P(4));

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void ApplySelector_Should_Emit_Additional_Declarations()
    {
        var sut = CreateSut(out ClassRegistry registry);

        sut.ApplySelector(s => s.Class("pill"), css => css.RoundedL(Length.Rem(0.375)));

        registry.Css.Should().Contain(".pill { border-top-left-radius: 0.375rem; border-bottom-left-radius: 0.375rem }");
    }
}
