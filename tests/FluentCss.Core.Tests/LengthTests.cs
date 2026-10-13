namespace FluentCss.Core.Tests;

public class LengthTests
{
    [Fact]
    public void Px_Should_Expose_Value_And_Use_It_As_Token()
    {
        var length = Length.Px(400);

        length.Value.Should().Be("400px");
        length.Name.Should().BeNull();
        length.Token.Should().Be("400px");
    }

    [Fact]
    public void Rem_With_Decimal_Should_Use_Invariant_Culture()
    {
        var length = Length.Rem(1.5);

        length.Value.Should().Be("1.5rem");
        length.Token.Should().Be("1.5rem");
    }

    [Fact]
    public void Unit_Factories_Should_Produce_The_Expected_Values()
    {
        Length.Em(1.25).Value.Should().Be("1.25em");
        Length.Percent(80).Value.Should().Be("80%");
        Length.Vh(100).Value.Should().Be("100vh");
        Length.Vw(50).Value.Should().Be("50vw");
    }

    [Fact]
    public void Percent_Breakpoint_Should_Escape_The_Percent_In_The_Selector()
    {
        var registry = new ClassRegistry();
        var sut = new InternalRootFluentCssBuilder(registry, null, null, null);

        sut.Min(Length.Percent(80), x => x.Flex());

        var added = registry.Single();

        // The HTML class stays readable...
        added.ClassName.Should().Be("min-80%:flex");

        // ...while the CSS selector escapes the percent and the separators.
        added.CssString.Should().Contain(".min-80\\%\\:flex");
    }

    [Fact]
    public void Name_Should_Take_Precedence_Over_Value_For_The_Token()
    {
        var length = new Length("4.5rem", "header");

        length.Value.Should().Be("4.5rem");
        length.Token.Should().Be("header");
    }

    [Fact]
    public void Min_Should_Build_Media_Query_And_Class_From_A_Length()
    {
        var registry = new ClassRegistry();
        var sut = new InternalRootFluentCssBuilder(registry, null, null, null);

        sut.Min(Length.Px(400), x => x.Background(Color.Black));

        var added = registry.Single();
        added.ClassName.Should().Be("min-400px:bg-black");
        added.MediaPrefix.Should().Be("min-400px");
        added.MediaQuery.Should().Be("(width >= 400px)");
        added.CssString.Should().Be("@media (width >= 400px) { .min-400px\\:bg-black { background-color: #000 } }");
    }

    [Fact]
    public void Max_Should_Build_Media_Query_And_Class_From_A_Length()
    {
        var registry = new ClassRegistry();
        var sut = new InternalRootFluentCssBuilder(registry, null, null, null);

        sut.Max(Length.Px(640), x => x.Hidden());

        var added = registry.Single();
        added.ClassName.Should().Be("max-640px:hidden");
        added.MediaQuery.Should().Be("(width < 640px)");
    }

    [Fact]
    public void Decimal_Rem_Breakpoint_Should_Escape_The_Dot_In_The_Selector()
    {
        var registry = new ClassRegistry();
        var sut = new InternalRootFluentCssBuilder(registry, null, null, null);

        sut.Min(Length.Rem(1.5), x => x.Flex());

        var added = registry.Single();

        // The HTML class stays readable...
        added.ClassName.Should().Be("min-1.5rem:flex");

        // ...while the CSS selector escapes the dot and the separators.
        added.CssString.Should().Contain(".min-1\\.5rem\\:flex");
    }

    [Fact]
    public void Named_Breakpoint_Should_Use_The_Name_In_The_Class()
    {
        var registry = new ClassRegistry();
        var sut = new InternalRootFluentCssBuilder(registry, null, null, null);

        sut.Min(Breakpoints.Large, x => x.GridCols(4));

        var added = registry.Single();
        added.ClassName.Should().Be("min-lg:grid-cols-4");
        added.MediaQuery.Should().Be("(width >= 64rem)");
    }

    [Fact]
    public void MinMd_Should_Be_Equivalent_To_Min_Medium()
    {
        var registry = new ClassRegistry();
        var sut = new InternalRootFluentCssBuilder(registry, null, null, null);

        sut.MinMd(x => x.Background(Color.Black));

        var added = registry.Single();
        added.ClassName.Should().Be("min-md:bg-black");
        added.MediaQuery.Should().Be("(width >= 48rem)");
    }

    [Fact]
    public void Breakpoints_Should_Carry_The_Expected_Values()
    {
        Breakpoints.XSmall.Value.Should().Be("20rem");
        Breakpoints.Small.Value.Should().Be("40rem");
        Breakpoints.Medium.Value.Should().Be("48rem");
        Breakpoints.Large.Value.Should().Be("64rem");
        Breakpoints.XLarge.Value.Should().Be("80rem");
        Breakpoints.XXLarge.Value.Should().Be("96rem");

        Breakpoints.Medium.Token.Should().Be("md");
    }
}
