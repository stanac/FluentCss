using System.Globalization;

namespace FluentCss.Core;

/// <summary>
/// A CSS length that can be used by several utilities (sizing, spacing, font
/// size, media queries, ...). Like <see cref="Color"/>, it carries both the raw
/// CSS <see cref="Value"/> and an optional <see cref="Name"/> that is used as the
/// class-name fragment.
/// </summary>
/// <remarks>
/// When <see cref="Name"/> is not set the class fragment falls back to the raw
/// value (for example <c>1.5rem</c>). Characters that are meaningful in a CSS
/// selector are escaped when the stylesheet is emitted, so the class attribute
/// stays readable (<c>min-1.5rem:flex</c>) while the selector is valid
/// (<c>.min-1\.5rem\:flex</c>).
/// </remarks>
public readonly struct Length
{
    /// <summary>Raw CSS value, for example <c>1.5rem</c> or <c>400px</c>.</summary>
    public string Value { get; }

    /// <summary>
    /// Optional name used in class names and media prefixes. Must be unique and
    /// stable: it is the identity used to deduplicate generated CSS.
    /// </summary>
    public string? Name { get; }

    public Length(string value, string? name = null)
    {
        Value = value;
        Name = name;
    }

    /// <summary>
    /// The fragment used in class names and media prefixes: the <see cref="Name"/>
    /// when set, otherwise the raw <see cref="Value"/>.
    /// </summary>
    public string Token => string.IsNullOrWhiteSpace(Name) ? Value : Name!;

    /// <summary>A length expressed in pixels, for example <c>Length.Px(400)</c>.</summary>
    public static Length Px(int px) => new($"{px}px");

    /// <summary>A length expressed in rem, for example <c>Length.Rem(1.5)</c>.</summary>
    public static Length Rem(double rem) => new($"{Format(rem)}rem");

    /// <summary>A length expressed in em, for example <c>Length.Em(1.25)</c>.</summary>
    public static Length Em(double em) => new($"{Format(em)}em");

    /// <summary>A length expressed as a percentage, for example <c>Length.Percent(80)</c>.</summary>
    public static Length Percent(double percent) => new($"{Format(percent)}%");

    /// <summary>A length expressed in viewport height units, for example <c>Length.Vh(100)</c>.</summary>
    public static Length Vh(double vh) => new($"{Format(vh)}vh");

    /// <summary>A length expressed in viewport width units, for example <c>Length.Vw(100)</c>.</summary>
    public static Length Vw(double vw) => new($"{Format(vw)}vw");

    private static string Format(double value) => value.ToString("0.####", CultureInfo.InvariantCulture);

    public override string ToString() => Token;
}
