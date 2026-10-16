using System.Collections;

namespace FluentCss.Core.Tests;

/// <summary>
/// <see cref="ConcurrentSet{T}"/> is the deduplication set behind
/// <see cref="ClassRegistry.Add"/>.
/// </summary>
public class ConcurrentSetTests
{
    [Fact]
    public void TryAdd_Should_Return_True_For_A_New_Item()
    {
        var sut = new ConcurrentSet<string>();

        sut.TryAdd("a").Should().BeTrue();
        sut.Should().ContainSingle();
    }

    [Fact]
    public void TryAdd_Should_Return_False_For_A_Duplicate()
    {
        var sut = new ConcurrentSet<string>();

        sut.TryAdd("a").Should().BeTrue();
        sut.TryAdd("a").Should().BeFalse();
        sut.Should().ContainSingle();
    }

    [Fact]
    public void Enumeration_Should_Contain_Every_Added_Item()
    {
        var sut = new ConcurrentSet<string>();

        sut.TryAdd("a");
        sut.TryAdd("b");
        sut.TryAdd("c");

        sut.Should().BeEquivalentTo("a", "b", "c");
    }

    [Fact]
    public void NonGeneric_Enumeration_Should_Work()
    {
        var sut = new ConcurrentSet<string>();
        sut.TryAdd("a");

        IEnumerable enumerable = sut;
        var items = new List<object?>();

        foreach (object? item in enumerable)
        {
            items.Add(item);
        }

        items.Should().ContainSingle().Which.Should().Be("a");
    }
}
