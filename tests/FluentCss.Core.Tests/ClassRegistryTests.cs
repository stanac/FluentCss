using AwesomeAssertions;

namespace FluentCss.Core.Tests;

public class ClassRegistryTests
{
    [Fact]
    public void Add_Should_ReturnTrue_When_Class_Is_New()
    {
        // Arrange
        ClassRegistry sut = [];
        UtilityClass util = new UtilityClass("bg-red", "background", "red");

        // Act
        bool result = sut.Add(util);

        // Assert
        result.Should().BeTrue();
        sut.Should().ContainSingle();
    }

    [Fact]
    public void Add_Should_ReturnFalse_When_Class_Is_Duplicate()
    {
        // Arrange
        ClassRegistry sut = [];
        UtilityClass util1 = new UtilityClass("bg-red", "background", "red");
        UtilityClass util2 = new UtilityClass("bg-red", "background", "red"); // Identical signature

        // Act
        sut.Add(util1);
        bool result = sut.Add(util2);

        // Assert
        result.Should().BeFalse();
        sut.Should().ContainSingle();
    }

    [Fact]
    public void Css_Property_Should_Accumulate_Strings()
    {
        // Arrange
        ClassRegistry sut =
        [
            new UtilityClass("a", "prop", "val"),
            new UtilityClass("b", "prop", "val")
            // Act
        ];

        // Act
        string css = sut.Css;

        // Assert
        css.Should().Contain(".a { prop: val }");
        css.Should().Contain(".b { prop: val }");
    }

    [Fact]
    public void OnClassAdded_Event_Should_Fire_Only_For_New_Classes()
    {
        // Arrange
        ClassRegistry sut = [];
        UtilityClass util = new UtilityClass("test", "p", "v");
        UtilityClass? captured = null;
        int callCount = 0;

        sut.OnClassAdded += (_, args) =>
        {
            captured = args;
            callCount++;
        };

        // Act
        sut.Add(util);
        sut.Add(util);

        // Assert
        callCount.Should().Be(1);
        captured.Should().Be(util);
    }
}