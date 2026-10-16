namespace FluentCss.Core.Tests;

/// <summary>
/// The named <see cref="Size"/> values and the sizing utilities that consume them.
/// </summary>
public class SizeTests
{
    [Fact]
    public void Named_Sizes_Should_Carry_The_Expected_Value_And_Name()
    {
        Size.Sm.Value.Should().Be("40rem");
        Size.Sm.Name.Should().Be("sm");
        Size.Md.Value.Should().Be("48rem");
        Size.Md.Name.Should().Be("md");
        Size.Lg.Value.Should().Be("64rem");
        Size.Lg.Name.Should().Be("lg");
        Size.Xl.Value.Should().Be("80rem");
        Size.Xl.Name.Should().Be("xl");
        Size.Xxl.Value.Should().Be("96rem");
        Size.Xxl.Name.Should().Be("2xl");
        Size.Xxxl.Value.Should().Be("128rem");
        Size.Xxxl.Name.Should().Be("3xl");

        Size.Full.Value.Should().Be("100%");
        Size.Full.Name.Should().Be("full");
        Size.ScreenH.Value.Should().Be("100vh");
        Size.ScreenW.Value.Should().Be("100vw");
        Size.Zero.Value.Should().Be("0px");
        Size.Min.Value.Should().Be("min-content");
        Size.Max.Value.Should().Be("max-content");
        Size.Fit.Value.Should().Be("fit-content");
    }

    [Fact]
    public void Sizing_Utilities_Should_Use_The_Size_Name_And_Value()
    {
        AssertSizing(b => b.MaxW(Size.Lg), "max-w-lg", "max-width", "64rem");
        AssertSizing(b => b.MaxW(Size.Xxl), "max-w-2xl", "max-width", "96rem");
        AssertSizing(b => b.MaxW(Size.Xxxl), "max-w-3xl", "max-width", "128rem");
        AssertSizing(b => b.MinW(Size.Sm), "min-w-sm", "min-width", "40rem");
        AssertSizing(b => b.MaxH(Size.Full), "max-h-full", "max-height", "100%");
        AssertSizing(b => b.MinH(Size.Zero), "min-h-0", "min-height", "0px");
    }

    private static void AssertSizing(
        Action<InternalRootFluentCssBuilder> apply,
        string className,
        string property,
        string value)
    {
        ClassRegistry registry = new();
        var sut = new InternalRootFluentCssBuilder(registry, null, null, null);

        apply(sut);

        var added = registry.Single();
        added.ClassName.Should().Be(className);
        added.Property.Should().Be(property);
        added.Value.Should().Be(value);
    }
}
