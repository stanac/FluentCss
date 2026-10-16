namespace FluentCss.Core.Tests;

/// <summary>
/// The named <c>Min*</c> / <c>Max*</c> helpers in <see cref="MediaFluentCssBuilderExtensions"/>.
/// </summary>
public class BreakpointHelperTests
{
    [Fact]
    public void Min_Helpers_Should_Produce_The_Expected_Prefix_And_Query()
    {
        AssertMedia(b => b.MinXs(x => x.Flex()), "min-xs:flex", "min-xs", "(width >= 20rem)");
        AssertMedia(b => b.MinSm(x => x.Flex()), "min-sm:flex", "min-sm", "(width >= 40rem)");
        AssertMedia(b => b.MinMd(x => x.Flex()), "min-md:flex", "min-md", "(width >= 48rem)");
        AssertMedia(b => b.MinLg(x => x.Flex()), "min-lg:flex", "min-lg", "(width >= 64rem)");
        AssertMedia(b => b.MinXl(x => x.Flex()), "min-xl:flex", "min-xl", "(width >= 80rem)");
        AssertMedia(b => b.Min2Xl(x => x.Flex()), "min-2xl:flex", "min-2xl", "(width >= 96rem)");
    }

    [Fact]
    public void Max_Helpers_Should_Produce_The_Expected_Prefix_And_Query()
    {
        AssertMedia(b => b.MaxXs(x => x.Hidden()), "max-xs:hidden", "max-xs", "(width < 20rem)");
        AssertMedia(b => b.MaxSm(x => x.Hidden()), "max-sm:hidden", "max-sm", "(width < 40rem)");
        AssertMedia(b => b.MaxMd(x => x.Hidden()), "max-md:hidden", "max-md", "(width < 48rem)");
        AssertMedia(b => b.MaxLg(x => x.Hidden()), "max-lg:hidden", "max-lg", "(width < 64rem)");
        AssertMedia(b => b.MaxXl(x => x.Hidden()), "max-xl:hidden", "max-xl", "(width < 80rem)");
        AssertMedia(b => b.Max2Xl(x => x.Hidden()), "max-2xl:hidden", "max-2xl", "(width < 96rem)");
    }

    private static void AssertMedia(
        Action<InternalRootFluentCssBuilder> apply,
        string className,
        string prefix,
        string query)
    {
        ClassRegistry registry = new();
        var sut = new InternalRootFluentCssBuilder(registry, null, null, null);

        apply(sut);

        var added = registry.Single();
        added.ClassName.Should().Be(className);
        added.MediaPrefix.Should().Be(prefix);
        added.MediaQuery.Should().Be(query);
    }
}
