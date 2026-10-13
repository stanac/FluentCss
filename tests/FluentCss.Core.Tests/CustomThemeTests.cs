using static FluentCss.Core.FluentCssBuilder;

namespace FluentCss.Core.Tests;

/// <summary>
/// A "custom theme" is any value passed to <see cref="FluentCssBuilder.Theme"/>.
/// The theme name becomes an ancestor class (<c>theme-ocean</c>) and every utility
/// built from that root is prefixed (<c>theme-ocean:bg-sky-500</c>) and scoped by a
/// descendant selector (<c>.theme-ocean .theme-ocean\:bg-sky-500</c>). These tests
/// cover that contract independently of the built-in dark theme.
/// </summary>
public class CustomThemeTests
{
    private readonly ClassRegistry _registry = new();

    [Fact]
    public void Custom_Theme_Should_Prefix_Class_And_Scope_Css()
    {
        // Arrange
        var sut = new InternalRootFluentCssBuilder(_registry, "theme-ocean", null, null);

        // Act
        sut.Background(Color.Sky500);

        // Assert
        var added = _registry.Single();
        added.Theme.Should().Be("theme-ocean");
        added.ClassName.Should().Be("theme-ocean:bg-sky-500");
        added.CssString.Should().Be(".theme-ocean .theme-ocean\\:bg-sky-500 { background-color: oklch(0.685 0.169 237.323) } ");
        _registry.Css.Should().Contain(".theme-ocean .theme-ocean\\:bg-sky-500");
    }

    [Fact]
    public void Custom_Theme_Should_Not_Be_The_Same_As_Dark()
    {
        // Arrange
        var ocean = new InternalRootFluentCssBuilder(_registry, "theme-ocean", null, null);
        var dark = new InternalRootFluentCssBuilder(_registry, "theme-dark", null, null);

        // Act
        ocean.Background(Color.Sky100);
        dark.Background(Color.Slate900);

        // Assert
        _registry.Should().Contain(x => x.ClassName == "theme-ocean:bg-sky-100");
        _registry.Should().Contain(x => x.ClassName == "theme-dark:bg-slate-900");
        _registry.Should().NotContain(x => x.ClassName == "dark:bg-sky-100");
    }

    [Fact]
    public void Pseudo_Inside_Custom_Theme_Should_Combine_In_Correct_Order()
    {
        // Arrange
        var sut = new InternalFluentCssBuilder(_registry, "theme-ocean", null, null);

        // Act
        sut.Hover(h => h.Background(Color.Sky600));

        // Assert
        var added = _registry.Single();
        added.Theme.Should().Be("theme-ocean");
        added.Pseudo.Should().Be("hover");
        added.ClassName.Should().Be("theme-ocean:hover:bg-sky-600");
        added.CssString.Should().Contain(".theme-ocean .theme-ocean\\:hover\\:bg-sky-600:hover");
    }

    [Fact]
    public void Media_Inside_Custom_Theme_Should_Combine_In_Correct_Order()
    {
        // Arrange
        var sut = new InternalRootFluentCssBuilder(_registry, "theme-ocean", null, null);

        // Act
        sut.MinMd(md => md.Background(Color.Sky200));

        // Assert
        var added = _registry.Single();
        added.Theme.Should().Be("theme-ocean");
        added.MediaPrefix.Should().Be("min-md");
        added.ClassName.Should().Be("min-md:theme-ocean:bg-sky-200");
        added.CssString.Should().StartWith("@media (width >= 48rem) { ")
            .And.Contain(".theme-ocean .min-md\\:theme-ocean\\:bg-sky-200");
    }

    [Fact]
    public void Multiple_Custom_Themes_Should_Coexist_Without_Colliding()
    {
        // Arrange
        var ocean = new InternalFluentCssBuilder(_registry, "theme-ocean", null, null);
        var sunset = new InternalFluentCssBuilder(_registry, "theme-sunset", null, null);

        // Act
        ocean.Background(Color.Sky100);
        sunset.Background(Color.Orange100);

        // Assert
        _registry.Should().HaveCount(2);
        _registry.Should().Contain(x => x.ClassName == "theme-ocean:bg-sky-100");
        _registry.Should().Contain(x => x.ClassName == "theme-sunset:bg-orange-100");

        // The two themes emit separate scopes so they never get in each other's way.
        _registry.Css.Should().Contain(".theme-ocean .theme-ocean\\:bg-sky-100");
        _registry.Css.Should().Contain(".theme-sunset .theme-sunset\\:bg-orange-100");
    }

    [Fact]
    public void Custom_Theme_Root_ToString_Should_Aggregate_All_Utilities()
    {
        // Arrange
        var sut = new InternalRootFluentCssBuilder(_registry, "theme-ocean", null, null);

        // Act
        sut.Background(Color.White).Color(Color.Slate900);
        sut.Hover(h => h.Background(Color.Sky100));

        // Assert
        sut.ToString().Should().Be("theme-ocean:bg-white theme-ocean:text-slate-900 theme-ocean:hover:bg-sky-100");
    }

    [Fact]
    public void Theme_Factory_Should_Map_Name_To_Ancestor_Class()
    {
        // FluentCssBuilder.Theme("ocean") -> theme-ocean:...
        var ocean = Theme("ocean");
        ocean.Background(Color.Sky500);
        ocean.ToString().Should().Be("theme-ocean:bg-sky-500");

        // Dark is a shortcut for Theme("dark") -> theme-dark:...
        var dark = Dark;
        dark.Background(Color.Slate900);
        dark.ToString().Should().Be("theme-dark:bg-slate-900");
    }

    [Fact]
    public void Custom_Theme_Should_Not_Be_Persisted_As_A_Cookie_Theme()
    {
        // Only "dark" is persisted/read from the cookie. Custom themes are transient
        // by design: they are applied to an ancestor element at runtime and are never
        // written to the cookie, so they cannot leak to other pages.
        FluentCssTheme.IsDark("ocean").Should().BeFalse();
        FluentCssTheme.IsDark("sunset").Should().BeFalse();
        FluentCssTheme.GetHtmlClass("ocean").Should().BeEmpty();
        FluentCssTheme.GetHtmlClass("sunset").Should().BeEmpty();
    }
}
