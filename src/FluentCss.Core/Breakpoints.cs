namespace FluentCss.Core;

/// <summary>
/// Named <see cref="Length"/> values for the common breakpoints, mirroring the
/// values already used by <see cref="Size"/>. Use them with
/// <see cref="RootFluentCssBuilderExtensions.Min"/> and
/// <see cref="RootFluentCssBuilderExtensions.Max"/> (or the named helpers such as
/// <see cref="RootFluentCssBuilderExtensions.MinMd"/>), or define your own
/// <see cref="Length"/> for a project-specific breakpoint.
/// </summary>
public static class Breakpoints
{
    /// <summary>20rem.</summary>
    public static Length XSmall { get; } = new("20rem", "xs");

    /// <summary>40rem, matches <see cref="Size.Sm"/>.</summary>
    public static Length Small { get; } = new("40rem", "sm");

    /// <summary>48rem, matches <see cref="Size.Md"/> and the default MinMd/MaxMd helpers.</summary>
    public static Length Medium { get; } = new("48rem", "md");

    /// <summary>64rem, matches <see cref="Size.Lg"/>.</summary>
    public static Length Large { get; } = new("64rem", "lg");

    /// <summary>80rem, matches <see cref="Size.Xl"/>.</summary>
    public static Length XLarge { get; } = new("80rem", "xl");

    /// <summary>96rem, matches <see cref="Size._2Xl"/>.</summary>
    // ReSharper disable once InconsistentNaming
    public static Length XXLarge { get; } = new("96rem", "2xl");
}
