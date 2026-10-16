namespace FluentCss.Core.Tests;

/// <summary>
/// The cookie theme helpers shared by the server and the browser script.
/// </summary>
public class FluentCssThemeTests
{
    [Fact]
    public void Constants_Should_Have_The_Expected_Values()
    {
        FluentCssTheme.CookieName.Should().Be("fluentcss-theme");
        FluentCssTheme.DarkClass.Should().Be("theme-dark");
    }

    [Theory]
    [InlineData("dark", true)]
    [InlineData("DARK", true)]
    [InlineData("Dark", true)]
    [InlineData("light", false)]
    [InlineData("ocean", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsDark_Should_Match_Dark_Case_Insensitively(string? theme, bool expected)
    {
        FluentCssTheme.IsDark(theme).Should().Be(expected);
    }

    [Fact]
    public void GetHtmlClass_Should_Only_Return_The_Dark_Class_For_Dark()
    {
        FluentCssTheme.GetHtmlClass("dark").Should().Be("theme-dark");
        FluentCssTheme.GetHtmlClass("DARK").Should().Be("theme-dark");
        FluentCssTheme.GetHtmlClass("light").Should().BeEmpty();
        FluentCssTheme.GetHtmlClass(null).Should().BeEmpty();
    }
}
