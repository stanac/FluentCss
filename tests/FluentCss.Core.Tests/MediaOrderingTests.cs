namespace FluentCss.Core.Tests;

/// <summary>
/// Media rules are emitted in a deterministic order (Tailwind mobile-first) rather
/// than the order classes happened to be registered, so a larger breakpoint reliably
/// overrides a smaller one.
/// </summary>
public class MediaOrderingTests
{
    private static ClassRegistry Build(Action<InternalRootFluentCssBuilder> configure)
    {
        var registry = new ClassRegistry();
        var sut = new InternalRootFluentCssBuilder(registry, null, null, null);
        configure(sut);
        return registry;
    }

    [Fact]
    public void Min_Media_Queries_Should_Be_Emitted_Ascending()
    {
        // Large is registered first on purpose; it must still be emitted last.
        ClassRegistry registry = Build(sut =>
        {
            sut.Min(Breakpoints.Large, x => x.GridCols(4));
            sut.Min(Breakpoints.Small, x => x.GridCols(1));
        });

        string css = registry.Css;
        int small = css.IndexOf(".min-sm", StringComparison.Ordinal);
        int large = css.IndexOf(".min-lg", StringComparison.Ordinal);

        small.Should().BeGreaterThanOrEqualTo(0);
        large.Should().BeGreaterThanOrEqualTo(0);
        small.Should().BeLessThan(large);
    }

    [Fact]
    public void Max_Media_Queries_Should_Be_Emitted_Descending()
    {
        // Max-lg before max-sm so the smaller range wins when both match.
        ClassRegistry registry = Build(sut =>
        {
            sut.Max(Breakpoints.Small, x => x.Hidden());
            sut.Max(Breakpoints.Large, x => x.Block());
        });

        string css = registry.Css;
        int large = css.IndexOf(".max-lg", StringComparison.Ordinal);
        int small = css.IndexOf(".max-sm", StringComparison.Ordinal);

        large.Should().BeGreaterThanOrEqualTo(0);
        small.Should().BeGreaterThanOrEqualTo(0);
        large.Should().BeLessThan(small);
    }

    [Fact]
    public void Min_Rules_Should_Be_Emitted_After_Max_Rules()
    {
        ClassRegistry registry = Build(sut =>
        {
            sut.Min(Breakpoints.Medium, x => x.Block());
            sut.Max(Breakpoints.Medium, x => x.Hidden());
        });

        string css = registry.Css;
        int max = css.IndexOf(".max-md", StringComparison.Ordinal);
        int min = css.IndexOf(".min-md", StringComparison.Ordinal);

        max.Should().BeGreaterThanOrEqualTo(0);
        min.Should().BeGreaterThanOrEqualTo(0);
        max.Should().BeLessThan(min);
    }

    [Fact]
    public void Unclassified_Media_Queries_Should_Be_Emitted_Last()
    {
        ClassRegistry registry = Build(sut =>
        {
            sut.Media("(prefers-color-scheme: dark)", "dark-scheme", x => x.Hidden());
            sut.Min(Breakpoints.Medium, x => x.Block());
        });

        string css = registry.Css;
        int min = css.IndexOf(".min-md", StringComparison.Ordinal);
        int scheme = css.IndexOf("prefers-color-scheme", StringComparison.Ordinal);

        min.Should().BeGreaterThanOrEqualTo(0);
        scheme.Should().BeGreaterThanOrEqualTo(0);
        min.Should().BeLessThan(scheme);
    }

    [Fact]
    public void Same_Breakpoint_Should_Keep_Registration_Order()
    {
        ClassRegistry registry = Build(sut =>
        {
            sut.Min(Breakpoints.Medium, x => x.GridCols(1));
            sut.Min(Breakpoints.Medium, x => x.GridCols(2));
        });

        string css = registry.Css;
        int first = css.IndexOf(".min-md\\:grid-cols-1", StringComparison.Ordinal);
        int second = css.IndexOf(".min-md\\:grid-cols-2", StringComparison.Ordinal);

        first.Should().BeGreaterThanOrEqualTo(0);
        second.Should().BeGreaterThanOrEqualTo(0);
        first.Should().BeLessThan(second);
    }

    [Fact]
    public void Px_And_Rem_Breakpoints_Should_Sort_Together()
    {
        // 24px == 1.5rem, so px sorts before the 40rem breakpoint.
        ClassRegistry registry = Build(sut =>
        {
            sut.Min(Breakpoints.Small, x => x.GridCols(1));
            sut.Min(Length.Px(24), x => x.GridCols(2));
        });

        string css = registry.Css;
        int px = css.IndexOf(".min-24px", StringComparison.Ordinal);
        int rem = css.IndexOf(".min-sm", StringComparison.Ordinal);

        px.Should().BeGreaterThanOrEqualTo(0);
        rem.Should().BeGreaterThanOrEqualTo(0);
        px.Should().BeLessThan(rem);
    }
}
