namespace FluentCss.Core.Tests;

/// <summary>
/// The <c>ApplyClass</c> / <c>ApplyId</c> sugar over
/// <see cref="IRootFluentCssBuilder.ApplySelector"/>.
/// </summary>
public class SelectorBuilderExtensionsTests
{
    private static InternalRootFluentCssBuilder CreateSut(out ClassRegistry registry)
    {
        registry = new ClassRegistry();
        return new InternalRootFluentCssBuilder(registry, null, null, null);
    }

    [Fact]
    public void ApplyClass_Should_Prefix_The_Class_With_A_Dot()
    {
        var sut = CreateSut(out ClassRegistry registry);

        sut.ApplyClass("btn", css => css.P(4));

        registry.Css.Should().Contain(".btn { padding: 1rem }");
    }

    [Fact]
    public void ApplyClass_Should_Not_Double_Prefix_An_Already_Prefixed_Name()
    {
        var sut = CreateSut(out ClassRegistry registry);

        sut.ApplyClass(".btn", css => css.P(4));

        registry.Css.Should().Contain(".btn { padding: 1rem }");
        registry.Css.Should().NotContain("..btn");
    }

    [Fact]
    public void ApplyId_Should_Prefix_The_Id_With_A_Hash()
    {
        var sut = CreateSut(out ClassRegistry registry);

        sut.ApplyId("hero", css => css.Py(12));

        registry.Css.Should().Contain("#hero { padding-block: 3rem }");
    }

    [Fact]
    public void ApplyId_Should_Not_Double_Prefix_An_Already_Prefixed_Id()
    {
        var sut = CreateSut(out ClassRegistry registry);

        sut.ApplyId("#hero", css => css.Py(12));

        registry.Css.Should().Contain("#hero { padding-block: 3rem }");
        registry.Css.Should().NotContain("##hero");
    }

    [Fact]
    public void ApplyClass_Should_Return_The_Same_Root_Builder_For_Chaining()
    {
        var sut = CreateSut(out ClassRegistry registry);

        IRootFluentCssBuilder result = sut.ApplyClass("btn", css => css.P(4));

        result.Should().BeSameAs(sut);

        result.ApplyClass("card", css => css.M(2));

        registry.Css.Should().Contain(".btn { padding: 1rem }");
        registry.Css.Should().Contain(".card { margin: 0.5rem }");
    }
}
