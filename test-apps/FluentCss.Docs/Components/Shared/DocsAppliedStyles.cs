using FluentCss.Core;
using static FluentCss.Core.FluentCssBuilder;

namespace FluentCss.Docs.Components.Shared;

/// <summary>
/// Selectors that used to live in <c>site.css</c>, expressed with the fluent
/// selector builder. Registered once from <see cref="DocsStyles"/>'s static
/// constructor so they are in the registry before <c>CssStyles</c> renders.
/// </summary>
/// <remarks>
/// The remaining <c>site.css</c> rules are Blazor template/boilerplate styles
/// (validation, the error UI and the loading indicator). They must stay in a static
/// stylesheet because they style markup that exists before the component tree — and
/// therefore the generated stylesheet — is available, notably in the WebAssembly host.
/// </remarks>
internal static class DocsAppliedStyles
{
    private static bool _initialized;

    internal static void Initialize()
    {
        if (_initialized)
        {
            return;
        }

        _initialized = true;

        // Global code colour (was "code { color: #c02d76 }").
        Css.ApplySelector("code", css => css.Arbitrary("docs-code", "color", "#c02d76"));

        // docs.js toggles the hidden attribute on elements that must win over
        // responsive display utilities on the same element (was "[hidden]").
        Css.ApplySelector(
            s => s.Attribute("hidden"),
            css => css.Important(c => c.Hidden()));

        // The "On this page" panel is in the DOM from the first paint but stays
        // invisible until docs.js has filled it (was "#fc-toc:not([data-fc-toc-ready])").
        Css.ApplySelector(
            s => s.Id("fc-toc").Not(t => t.Attribute("data-fc-toc-ready")),
            css => css.Arbitrary("docs-toc-hidden", "visibility", "hidden"));

        // Headings get ids from docs.js; keep the anchor jumps clear of the sticky
        // header (was "[data-fc-toc-source] :is(h1, ..., h6)[id]").
        Css.ApplySelector(
            s => s.Attribute("data-fc-toc-source")
                .Descendant()
                .Is(
                    t => t.Element("h1"),
                    t => t.Element("h2"),
                    t => t.Element("h3"),
                    t => t.Element("h4"),
                    t => t.Element("h5"),
                    t => t.Element("h6"))
                .Attribute("id"),
            css => css.Arbitrary("docs-toc-anchor", "scroll-margin-top", "5rem"));

        // Search trigger: centred in the header on wide screens, inline on narrow
        // ones. Centred with insets/margin (not transform) so the fixed-position
        // palette it contains is still positioned against the viewport.
        Css.ApplyClass("fc-header-search", css => css
            .Arbitrary("docs-header-search-position", "position", "absolute")
            .Arbitrary("docs-header-search-inset", "inset", "0")
            .Arbitrary("docs-header-search-margin", "margin", "auto")
            .Arbitrary("docs-header-search-width", "width", "20rem")
            .Arbitrary("docs-header-search-max-width", "max-width", "40vw")
            .Arbitrary("docs-header-search-height", "height", "fit-content"));

        Css.ApplySelector(
            s => s.Media("(width < 64rem)").Class("fc-header-search"),
            css => css
                .Arbitrary("docs-header-search-narrow-position", "position", "static")
                .Arbitrary("docs-header-search-narrow-inset", "inset", "auto")
                .Arbitrary("docs-header-search-narrow-margin", "margin", "0")
                .Arbitrary("docs-header-search-narrow-width", "width", "auto")
                .Arbitrary("docs-header-search-narrow-max-width", "max-width", "none")
                .Arbitrary("docs-header-search-narrow-height", "height", "auto"));

        // Search palette: translucency and list resets the utility builder does not
        // express.
        Css.ApplyClass("fc-search-overlay", css => css
            .Arbitrary("docs-search-overlay-bg", "background-color", "rgb(15 23 42 / 0.45)")
            .Arbitrary("docs-search-overlay-blur", "backdrop-filter", "blur(2px)"));

        Css.ApplySelector(
            s => s.Theme("dark").Class("fc-search-overlay"),
            css => css.Arbitrary("docs-search-overlay-bg-dark", "background-color", "rgb(0 0 0 / 0.6)"));

        Css.ApplyClass("fc-search-input-row", css =>
            css.Arbitrary("docs-search-input-row-border", "border-bottom", "1px solid #e2e8f0"));

        Css.ApplySelector(
            s => s.Theme("dark").Class("fc-search-input-row"),
            css => css.Arbitrary("docs-search-input-row-border-dark", "border-bottom-color", "#334155"));

        Css.ApplyClass("fc-search-results", css =>
            css.Arbitrary("docs-search-results-list-style", "list-style", "none"));

        // Keep the focus ring clear of the text.
        Css.ApplyClass("fc-search-input", css =>
            css.Arbitrary("docs-search-input-padding", "padding", "2px 4px"));
    }
}
