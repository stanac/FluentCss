namespace FluentCss.Core.Tests;

/// <summary>
/// The <see cref="Color"/> value type. The full palette is data, so these tests
/// cover the struct itself plus a representative sample of the named values.
/// </summary>
public class ColorTests
{
    [Fact]
    public void Constructor_Should_Set_Name_And_Value()
    {
        var sut = new Color("brand", "#123456");

        sut.Name.Should().Be("brand");
        sut.Value.Should().Be("#123456");
    }

    [Fact]
    public void Black_And_White_Should_Have_The_Expected_Values()
    {
        Color.Black.Name.Should().Be("black");
        Color.Black.Value.Should().Be("#000");
        Color.White.Name.Should().Be("white");
        Color.White.Value.Should().Be("#fff");
    }

    [Fact]
    public void Palette_Entries_Should_Carry_Family_And_Shade_In_The_Name()
    {
        Color.Red50.Name.Should().Be("red-50");
        Color.Red500.Name.Should().Be("red-500");
        Color.Red950.Name.Should().Be("red-950");
        Color.Blue500.Name.Should().Be("blue-500");
        Color.Slate300.Name.Should().Be("slate-300");
        Color.Emerald500.Name.Should().Be("emerald-500");

        Color.Red500.Value.Should().NotBeNullOrWhiteSpace();
    }
}
