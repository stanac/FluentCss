using System.Linq;
using AwesomeAssertions;
using Xunit;

namespace FluentCss.Core.Tests;

public class RootBuilderTests
{
    private readonly ClassRegistry _registry;

    public RootBuilderTests()
    {
        _registry = new ClassRegistry();
    }

    [Fact]
    public void MinMd_Should_Apply_Media_Query()
    {
        // Arrange
        var sut = new InternalRootFluentCssuilder(_registry, null, null, null);

        // Act
        sut.MinMd(x => x.Background(Color.Black));

        // Assert
        var added = _registry.Single();
        // Based on implementation: Media("width > 48rem", "max-md", ...)
        added.MediaPrefix.Should().Be("max-md");
        added.MediaQuery.Should().Be("(width > 48rem)");
        added.ClassName.Should().Be("max-md:bg-black");
    }

    [Fact]
    public void Media_Nesting_Should_Work_With_Action_Delegates()
    {
        // Arrange
        var sut = new InternalRootFluentCssuilder(_registry, null, null, null);

        // Act
        // We cannot nest Media inside Media via fluent API, but we can pass a builder action
        sut.Media("(min-width: 100px)", "custom", builder =>
        {
            builder.Color(Color.White);
            builder.Hover(h => h.Background(Color.Black));
        });

        // Assert
        _registry.Count().Should().Be(2);

        var normal = _registry.First(x => x.Pseudo == null);
        normal.ClassName.Should().Be("custom:text-white");
        normal.MediaQuery.Should().Be("(min-width: 100px)");

        var hover = _registry.First(x => x.Pseudo != null);
        hover.ClassName.Should().Be("custom:hover:bg-black");
        hover.MediaQuery.Should().Be("(min-width: 100px)");
    }

    [Fact]
    public void RootBuilder_ToString_Should_Aggregate_Child_Builder_Classes()
    {
        // Arrange
        var sut = new InternalRootFluentCssuilder(_registry, null, null, null);

        // Act
        sut.Background(Color.Red50);
        sut.MinMd(x => x.Color(Color.Black));

        // Assert
        var classes = sut.ToString();

        // Should contain the direct class
        classes.Should().Contain("bg-red-50");

        // Should contain the class generated inside the media action
        classes.Should().Contain("max-md:text-black");
    }
}