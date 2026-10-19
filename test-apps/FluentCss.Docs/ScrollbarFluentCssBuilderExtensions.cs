using FluentCss.Core;

namespace FluentCss.Docs;

/// <summary>
/// Registers a slim, trackless scrollbar that only reveals its thumb while the
/// scrollable area is hovered.
/// </summary>
/// <remarks>
/// The rules target the WebKit/Blink scrollbar pseudo-elements
/// (<c>::-webkit-scrollbar</c> and friends). Chrome, Edge and Safari implement
/// them, so they get the same "thin grey, hidden until hover" look Firefox already
/// uses, while Firefox ignores the unknown selectors and keeps its native overlay
/// scrollbars untouched. This is why the helper deliberately does not use the
/// standard <c>scrollbar-width</c> / <c>scrollbar-color</c> properties: those would
/// also change (and can disable the auto-hiding of) Firefox's scrollbars.
/// </remarks>
public static class ScrollbarFluentCssBuilderExtensions
{
    /// <summary>Default thumb colour while the scrollable area is hovered.</summary>
    public const string DefaultThumbColor = "rgb(100 116 139 / 0.45)";

    /// <summary>Default thumb colour while the pointer is directly over the scrollbar.</summary>
    public const string DefaultThumbHoverColor = "rgb(100 116 139 / 0.7)";

    /// <summary>Default scrollbar thickness (both axes).</summary>
    public const string DefaultThickness = "10px";

    /// <summary>
    /// Applies the slim scrollbar rules to every scrollable element in the document.
    /// Call once while configuring the stylesheet, for example from the same static
    /// constructor that registers the other global selectors.
    /// </summary>
    /// <param name="cssBuilder">The root builder to register the rules on.</param>
    /// <param name="thumbColor">Colour used while the scrollable area is hovered.</param>
    /// <param name="thumbHoverColor">Colour used while the pointer is over the scrollbar.</param>
    /// <param name="thickness">Width (and height) of the scrollbar.</param>
    /// <returns>The same builder so calls can keep chaining.</returns>
    public static IRootFluentCssBuilder ApplySlimScrollbars(
        this IRootFluentCssBuilder cssBuilder,
        string thumbColor = DefaultThumbColor,
        string thumbHoverColor = DefaultThumbHoverColor,
        string thickness = DefaultThickness)
    {
        if (cssBuilder is null)
        {
            throw new ArgumentNullException(nameof(cssBuilder));
        }

        if (string.IsNullOrWhiteSpace(thumbColor))
        {
            throw new ArgumentException("A thumb colour is required.", nameof(thumbColor));
        }

        if (string.IsNullOrWhiteSpace(thumbHoverColor))
        {
            throw new ArgumentException("A hover thumb colour is required.", nameof(thumbHoverColor));
        }

        if (string.IsNullOrWhiteSpace(thickness))
        {
            throw new ArgumentException("A thickness is required.", nameof(thickness));
        }

        // Thin scrollbar on both axes.
        _ = cssBuilder.ApplySelector("::-webkit-scrollbar", css => css
            .Arbitrary("fc-scrollbar-width", "width", thickness)
            .Arbitrary("fc-scrollbar-height", "height", thickness));

        // No white gutter: the track stays transparent so the element background shows.
        _ = cssBuilder.ApplySelector("::-webkit-scrollbar-track", css => css
            .Arbitrary("fc-scrollbar-track", "background-color", "transparent"));

        // Hidden by default. The transparent border insets the background so the
        // rounded thumb does not touch the edge of the scrollbar.
        _ = cssBuilder.ApplySelector("::-webkit-scrollbar-thumb", css => css
            .Arbitrary("fc-scrollbar-thumb-color", "background-color", "transparent")
            .Arbitrary("fc-scrollbar-thumb-radius", "border-radius", "9999px")
            .Arbitrary("fc-scrollbar-thumb-border", "border", "2px solid transparent")
            .Arbitrary("fc-scrollbar-thumb-clip", "background-clip", "content-box"));

        // Reveal it while the scrollable area (or one of its descendants) is hovered.
        _ = cssBuilder.ApplySelector(":hover::-webkit-scrollbar-thumb", css => css
            .Arbitrary("fc-scrollbar-thumb-shown", "background-color", thumbColor));

        // Darken it further while the pointer is over the scrollbar itself.
        // Registered last so it wins over the rule above at equal specificity.
        _ = cssBuilder.ApplySelector("::-webkit-scrollbar-thumb:hover", css => css
            .Arbitrary("fc-scrollbar-thumb-hover", "background-color", thumbHoverColor));

        return cssBuilder;
    }
}
