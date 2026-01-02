using AwesomeAssertions;

namespace CssB.Core.Tests;

public class StringBuilderBagTests
{
    [Fact]
    public void TakeOrNew_Should_Return_Empty_StringBuilder()
    {
        // Act
        var sb = StringBuilderBag.TakeOrNew();

        // Assert
        sb.Should().NotBeNull();
        sb.Length.Should().Be(0);
    }

    [Fact]
    public void ClearAndReturn_Should_Allow_Reuse()
    {
        // Arrange
        var sb1 = StringBuilderBag.TakeOrNew();
        sb1.Append("Dirty");
        var originalRef = sb1;

        // Act
        StringBuilderBag.ClearAndReturn(sb1);
        var sb2 = StringBuilderBag.TakeOrNew();

        // Assert
        sb2.Should().BeSameAs(originalRef); // Should get the pooled instance
        sb2.Length.Should().Be(0); // Should be cleared
    }

    [Fact]
    public void Bag_Should_Handle_Multiple_Takes()
    {
        // Act
        var sb1 = StringBuilderBag.TakeOrNew();
        var sb2 = StringBuilderBag.TakeOrNew();

        // Assert
        sb1.Should().NotBeSameAs(sb2);
    }
}