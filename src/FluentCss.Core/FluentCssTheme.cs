namespace FluentCss.Core;

/// <summary>
/// Names and helpers shared by the server and the browser when the selected
/// color theme is persisted in a cookie.
/// </summary>
/// <remarks>
/// The cookie is written by the theme toggle script and read on the server so
/// the theme class can be part of the rendered markup. That avoids the flash of
/// the light theme that happens when the class is only added by script after the
/// first paint.
/// </remarks>
public static class FluentCssTheme
{
    /// <summary>Cookie that stores the selected theme ("dark" or "light").</summary>
    public const string CookieName = "fluentcss-theme";

    /// <summary>Theme class applied to an ancestor (usually html) for dark mode.</summary>
    public const string DarkClass = "theme-dark";

    /// <summary>Returns true when the supplied theme value selects dark mode.</summary>
    public static bool IsDark(string? theme) =>
        string.Equals(theme, "dark", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Returns <see cref="DarkClass"/> when the supplied cookie value selects
    /// dark mode, otherwise an empty string.
    /// </summary>
    public static string GetHtmlClass(string? theme) => IsDark(theme) ? DarkClass : string.Empty;
}
