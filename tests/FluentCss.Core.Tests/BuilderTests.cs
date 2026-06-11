namespace FluentCss.Core.Tests;

public class BuilderTests
{
    private readonly ClassRegistry _registry = new();

    [Fact]
    public void Builder_Should_Add_Simple_Class_To_Registry()
    {
        // Arrange
        var sut = new InternalFluentCssBuilder(_registry, null, null, null);

        // Act
        sut.Background(Color.Red50);

        // Assert
        var added = _registry.Single();
        added.ClassName.Should().Be("bg-red-50");
        added.Value.Should().Be(Color.Red50.Value);
    }

    [Fact]
    public void Builder_Chaining_Should_Add_Multiple_Classes()
    {
        // Arrange
        var sut = new InternalFluentCssBuilder(_registry, null, null, null);

        // Act
        // .Background returns 'this', allowing chaining
        sut.Background(Color.Black).Color(Color.White);

        // Assert
        _registry.Count().Should().Be(2);
        _registry.Should().Contain(x => x.ClassName == "bg-black");
        _registry.Should().Contain(x => x.ClassName == "text-white");

        // Check the ToString output of the builder (used for class="" attributes)
        sut.ToString().Should().Contain("bg-black").And.Contain("text-white");
    }

    [Fact]
    public void Hover_Should_Add_Pseudo_Class_To_Registry()
    {
        // Arrange
        var sut = new InternalFluentCssBuilder(_registry, null, null, null);

        // Act
        sut.Hover(x => x.Background(Color.Red100));

        // Assert
        var added = _registry.Single();
        added.Pseudo.Should().Be("hover");
        added.ClassName.Should().Be("hover:bg-red-100");
    }

    [Fact]
    public void Theme_Builder_Should_Prefix_Classes()
    {
        // Arrange
        // Simulate FluentCssBuilder.Theme("dark")
        var sut = new InternalFluentCssBuilder(_registry, "dark", null, null);

        // Act
        sut.BorderColor(Color.Black);

        // Assert
        var added = _registry.Single();
        added.Theme.Should().Be("dark");
        added.ClassName.Should().Be("dark:border-black");
    }

    [Fact]
    public void Nested_Pseudo_Inside_Theme_Should_Work()
    {
        // Arrange
        var sut = new InternalFluentCssBuilder(_registry, "dark", null, null);

        // Act
        sut.Focus(x => x.Color(Color.Red200));

        // Assert
        var added = _registry.Single();
        added.Theme.Should().Be("dark");
        added.Pseudo.Should().Be("focus");
        added.ClassName.Should().Be("dark:focus:text-red-200");
    }

    [Fact]
    public void Custom_Pseudo_Should_Work()
    {
        // Arrange
        var sut = new InternalFluentCssBuilder(_registry, null, null, null);

        // Act
        sut.Pseudo("nth-child(2)", x => x.Background(Color.White));

        // Assert
        var added = _registry.Single();
        added.Pseudo.Should().Be("nth-child(2)");
        // The builder passes it raw.
        added.ClassName.Should().Be("nth-child(2):bg-white");
    }
}