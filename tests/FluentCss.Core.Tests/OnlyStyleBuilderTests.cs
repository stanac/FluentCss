namespace FluentCss.Core.Tests;

/// <summary>
/// <see cref="OnlyStyleBuilder"/> is the declaration-only builder used inside a
/// pseudo (<c>Hover</c>, <c>Focus</c>, ...) and now also to capture the declarations
/// of <see cref="IRootFluentCssBuilder.ApplySelector"/>.
/// </summary>
public class OnlyStyleBuilderTests
{
    [Fact]
    public void With_Pseudo_Should_Prefix_The_Generated_Class()
    {
        ClassRegistry registry = new();
        var sut = new OnlyStyleBuilder(registry, null, null, null, "hover");

        sut.Background(Color.Red500);

        var added = registry.Single();
        added.Pseudo.Should().Be("hover");
        added.ClassName.Should().Be("hover:bg-red-500");
    }

    [Fact]
    public void With_Null_Pseudo_Should_Emit_A_Plain_Class()
    {
        ClassRegistry registry = new();
        var sut = new OnlyStyleBuilder(registry, null, null, null, null);

        sut.P(4);

        var added = registry.Single();
        added.Pseudo.Should().BeNull();
        added.ClassName.Should().Be("p-4");
    }

    [Fact]
    public void ToString_Should_Join_The_Class_Names_In_Order()
    {
        ClassRegistry registry = new();
        var sut = new OnlyStyleBuilder(registry, null, null, null, null);

        sut.P(4).M(2).Background(Color.White);

        sut.ToString().Should().Be("p-4 m-2 bg-white");
    }
}
