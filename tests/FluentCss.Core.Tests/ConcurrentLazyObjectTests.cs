namespace FluentCss.Core.Tests;

/// <summary>
/// <see cref="ConcurrentLazyObject{T}"/> is the cache behind <see cref="ClassRegistry.Css"/>
/// and its hash, so it must build once, cache the value and rebuild after invalidation.
/// </summary>
public class ConcurrentLazyObjectTests
{
    [Fact]
    public void Get_Should_Build_On_First_Access()
    {
        int builds = 0;
        var sut = new ConcurrentLazyObject<string>(() =>
        {
            builds++;
            return "value";
        });

        sut.Get().Should().Be("value");
        builds.Should().Be(1);
    }

    [Fact]
    public void Get_Should_Cache_The_Value()
    {
        int builds = 0;
        var sut = new ConcurrentLazyObject<string>(() =>
        {
            builds++;
            return "value";
        });

        string first = sut.Get();
        string second = sut.Get();

        second.Should().Be("value");
        builds.Should().Be(1);
    }

    [Fact]
    public void Invalidate_Should_Force_A_Rebuild()
    {
        int builds = 0;
        var sut = new ConcurrentLazyObject<string>(() => "value-" + ++builds);

        sut.Get().Should().Be("value-1");
        sut.Invalidate();
        sut.Get().Should().Be("value-2");
        builds.Should().Be(2);
    }

    [Fact]
    public void Get_Should_Only_Build_Once_Under_Concurrency()
    {
        int builds = 0;
        var sut = new ConcurrentLazyObject<string>(() =>
        {
            Interlocked.Increment(ref builds);
            return "value";
        });

        Parallel.For(0, 100, _ => sut.Get());

        builds.Should().Be(1);
    }
}
