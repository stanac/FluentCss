using AwesomeAssertions;
using Xunit;

namespace CssB.Core.Tests;

public class UtilityClassTests
{
    [Fact]
    public void Constructor_Should_Initialize_Properties()
    {
        // Arrange
        var name = "bg-red-500";
        var prop = "background-color";
        var val = "#ef4444";

        // Act
        var sut = new UtilityClass(name, prop, val);

        // Assert
        sut.ClassName.Should().Be(name);
        sut.Property.Should().Be(prop);
        sut.Value.Should().Be(val);
        sut.Pseudo.Should().BeNull();
        sut.Theme.Should().BeNull();
    }

    [Fact]
    public void AddPseudo_Should_ReturnNewInstance_And_UpdateClassName()
    {
        // Arrange
        var sut = new UtilityClass("p-4", "padding", "1rem");

        // Act
        var result = sut.AddPseudo("hover");

        // Assert
        result.Should().NotBeSameAs(sut); // Immutability check
        result.Pseudo.Should().Be("hover");
        result.ClassName.Should().Be("hover:p-4");

        // Original object should stay untouched
        sut.Pseudo.Should().BeNull();
        sut.ClassName.Should().Be("p-4");
    }

    [Fact]
    public void AddTheme_Should_ReturnNewInstance_And_UpdateClassName()
    {
        // Arrange
        var sut = new UtilityClass("text-white", "color", "white");

        // Act
        var result = sut.AddTheme("dark");

        // Assert
        result.Should().NotBeSameAs(sut);
        result.Theme.Should().Be("dark");
        result.ClassName.Should().Be("dark:text-white");
    }

    [Fact]
    public void AddTheme_Then_AddPseudo_Should_Combine_In_CssString()
    {
        // Arrange
        var sut = new UtilityClass("bg-black", "background-color", "black")
            .AddTheme("dark")
            .AddPseudo("focus");

        // Act
        var css = sut.CssString;

        // Assert
        css.Should().Be(".dark .dark\\:focus\\:bg-black:focus { background-color: black } ");
    }

    [Fact]
    public void AddPseudo_Then_AddTheme_Should_Combine_In_CssString()
    {
        // Arrange
        var sut = new UtilityClass("bg-black", "background-color", "black")
            .AddPseudo("focus")
            .AddTheme("dark");

        // Act
        var css = sut.CssString;

        // Assert
        css.Should().Be(".dark .dark\\:focus\\:bg-black:focus { background-color: black } ");
    }

    [Fact]
    public void BuildCssString_Should_Escape_Colons_For_Standard_Utility()
    {
        // Arrange
        var sut = new UtilityClass("w-1/2", "width", "50%");

        // Act
        var css = sut.CssString;

        // Assert
        css.Should().Be(".w-1/2 { width: 50% } ");
    }

    [Fact]
    public void BuildCssString_Should_Handle_Simple_Pseudo()
    {
        // Arrange
        var sut = new UtilityClass("opacity-50", "opacity", "0.5")
            .AddPseudo("hover");

        // Act
        var css = sut.CssString;

        // Assert
        // Logic: .pseudo\:name:pseudo
        css.Should().Be(".hover\\:opacity-50:hover { opacity: 0.5 } ");
    }

    [Fact]
    public void BuildCssString_Should_Handle_Simple_Theme()
    {
        // Arrange
        var sut = new UtilityClass("flex", "display", "flex")
            .AddTheme("dark");

        // Act
        var css = sut.CssString;

        // Assert
        // Logic: .theme .theme\:name
        css.Should().Be(".dark .dark\\:flex { display: flex } ");
    }

    [Fact]
    public void Equals_Should_ReturnTrue_For_Identical_Definitions()
    {
        // Arrange
        var u1 = new UtilityClass("a", "b", "c").AddPseudo("hover");
        var u2 = new UtilityClass("a", "b", "c").AddPseudo("hover");

        // Act & Assert
        u1.Should().Be(u2);
        (u1 == u2).Should().BeTrue();
        (u1 != u2).Should().BeFalse();
        u1.GetHashCode().Should().Be(u2.GetHashCode());
    }

    [Fact]
    public void Equals_Should_ReturnFalse_For_Different_Definitions()
    {
        // Arrange
        var u1 = new UtilityClass("a", "b", "c");
        var u2 = new UtilityClass("a", "b", "c").AddTheme("dark");

        // Act & Assert
        u1.Should().NotBe(u2);
        (u1 == u2).Should().BeFalse();
    }

    [Fact]
    public void EqualityOperators_Should_Handle_Nulls()
    {
        // Arrange
        UtilityClass? u1 = null;
        UtilityClass? u2 = null;
        UtilityClass u3 = new UtilityClass("a", "b", "c");

        // Act & Assert
        (u1 == u2).Should().BeTrue();
        (u1 == u3).Should().BeFalse();
        (u3 == u1).Should().BeFalse();
    }
}