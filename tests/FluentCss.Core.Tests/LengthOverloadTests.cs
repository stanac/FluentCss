namespace FluentCss.Core.Tests;

/// <summary>
/// Every int-scale spacing/sizing method also has a <see cref="Length"/> overload so
/// arbitrary values, other units and named tokens can be used.
/// </summary>
public class LengthOverloadTests
{
    private readonly ClassRegistry _registry = new();

    private InternalFluentCssBuilder Sut => new(_registry, null, null, null);

    [Fact]
    public void P_With_Length_Should_Use_Value_And_Class_Token()
    {
        Sut.P(Length.Px(13));

        var added = _registry.Single();
        added.ClassName.Should().Be("p-13px");
        added.Property.Should().Be("padding");
        added.Value.Should().Be("13px");
    }

    [Fact]
    public void M_With_Decimal_Rem_Should_Escape_The_Selector()
    {
        Sut.M(Length.Rem(1.5));

        var added = _registry.Single();
        added.ClassName.Should().Be("m-1.5rem");
        added.CssString.Should().Contain(".m-1\\.5rem");
    }

    [Fact]
    public void Named_Length_Should_Use_Name_As_Class_Token()
    {
        var card = new Length("1.5rem", "card");

        Sut.P(card);

        _registry.Single().ClassName.Should().Be("p-card");
    }

    [Fact]
    public void Int_Overload_Should_Still_Use_The_Scale()
    {
        Sut.P(4);

        var added = _registry.Single();
        added.ClassName.Should().Be("p-4");
        added.Value.Should().Be("1rem");
    }

    [Fact]
    public void All_Length_Overloads_Should_Produce_The_Expected_Class()
    {
        Sut.Mx(Length.Px(8));
        Sut.Gap(Length.Px(6));
        Sut.W(Length.Percent(80));
        Sut.Inset(Length.Px(0));
        Sut.Leading(Length.Em(2));

        _registry.Should().Contain(x => x.ClassName == "mx-8px");
        _registry.Should().Contain(x => x.ClassName == "gap-6px");
        _registry.Should().Contain(x => x.ClassName == "w-80%");
        _registry.Should().Contain(x => x.ClassName == "inset-0px");
        _registry.Should().Contain(x => x.ClassName == "leading-2em");
    }

    [Fact]
    public void Percent_Length_Selector_Should_Escape_The_Percent()
    {
        Sut.W(Length.Percent(80));

        _registry.Single().CssString.Should().Contain(".w-80\\%");
    }

    [Fact]
    public void MinMax_Length_Overloads_Should_Produce_The_Expected_Class()
    {
        Sut.MaxW(Length.Px(720));
        Sut.MinW(Length.Percent(50));
        Sut.MaxH(new Length("min-content", "min"));
        Sut.MinH(Length.Vh(100));

        _registry.Should().Contain(x => x.ClassName == "max-w-720px");
        _registry.Should().Contain(x => x.ClassName == "min-w-50%");
        _registry.Should().Contain(x => x.ClassName == "max-h-min");
        _registry.Should().Contain(x => x.ClassName == "min-h-100vh");
    }

    [Fact]
    public void Length_Overload_Should_Work_Inside_A_Pseudo_Builder()
    {
        Sut.Hover(h => h.P(Length.Px(4)));

        _registry.Single().ClassName.Should().Be("hover:p-4px");
    }
}
