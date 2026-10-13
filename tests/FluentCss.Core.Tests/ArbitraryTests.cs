namespace FluentCss.Core.Tests;

/// <summary>
/// The generic <see cref="IStyleBuilder{T}.Arbitrary"/> escape hatch for CSS
/// declarations that do not have a typed helper.
/// </summary>
public class ArbitraryTests
{
    private readonly ClassRegistry _registry = new();

    private InternalRootFluentCssBuilder Sut => new(_registry, null, null, null);

    [Fact]
    public void Arbitrary_Should_Emit_The_Class_Property_And_Value()
    {
        Sut.Arbitrary("font-350", "font-weight", "350");

        var added = _registry.Single();
        added.ClassName.Should().Be("font-350");
        added.Property.Should().Be("font-weight");
        added.Value.Should().Be("350");
        added.CssString.Should().Contain(".font-350 { font-weight: 350 }");
    }

    [Fact]
    public void Arbitrary_Should_Support_Complex_Values()
    {
        Sut.Arbitrary("aspect-golden", "aspect-ratio", "1.618 / 1");
        Sut.Arbitrary("grid-dashboard", "grid-template-columns", "240px 1fr");

        _registry.Should().Contain(x => x.ClassName == "aspect-golden" && x.Value == "1.618 / 1");
        _registry.Should().Contain(x => x.ClassName == "grid-dashboard" && x.Value == "240px 1fr");
    }

    [Fact]
    public void Arbitrary_Should_Compose_With_Pseudo_And_Media()
    {
        Sut.Hover(h => h.Arbitrary("font-350", "font-weight", "350"));
        Sut.Min(Breakpoints.Medium, md => md.Arbitrary("font-900", "font-weight", "900"));

        _registry.Should().Contain(x => x.ClassName == "hover:font-350");
        _registry.Should().Contain(x => x.ClassName == "min-md:font-900");
    }

    [Fact]
    public void Rounded_With_Length_Should_Use_The_Radius()
    {
        Sut.Rounded(Length.Percent(50));
        Sut.Rounded(new Length("1.25rem", "card"));

        var percent = _registry.Single(x => x.ClassName == "rounded-50%");
        percent.Value.Should().Be("50%");
        percent.Property.Should().Be("border-radius");
        _registry.Should().Contain(x => x.ClassName == "rounded-card" && x.Value == "1.25rem");
    }

    [Fact]
    public void Text_With_Length_Should_Set_Font_Size()
    {
        Sut.Text(Length.Rem(3));
        Sut.Text(new Length("clamp(1rem,2vw,2rem)", "fluid"));

        var rem = _registry.Single(x => x.ClassName == "text-3rem");
        rem.Value.Should().Be("3rem");
        rem.Property.Should().Be("font-size");
        _registry.Should().Contain(x => x.ClassName == "text-fluid");
    }
}
