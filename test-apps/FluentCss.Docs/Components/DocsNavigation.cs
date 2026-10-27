namespace FluentCss.Docs.Components;

public sealed record NavItem(string Title, string Href);

public sealed record NavSection(string Title, IReadOnlyList<NavItem> Items);

/// <summary>
/// Single source of truth for the documentation navigation. Both the sidebar
/// and the landing page use this list.
/// </summary>
public static class DocsNavigation
{
    public static IReadOnlyList<NavSection> Sections { get; } =
    [
        new("Getting started",
        [
            new("Overview", "/"),
            new("Installation", "/docs/installation"),
            new("How it works", "/docs/how-it-works"),
            new("Performance", "/docs/performance"),
        ]),
        new("Core concepts",
        [
            new("Utility classes", "/docs/utility-classes"),
            new("Extensibility", "/docs/extensibility"),
            new("Custom selectors", "/docs/selectors"),
            new("!important", "/docs/important"),
            new("Hover & focus", "/docs/states"),
            new("Responsive design", "/docs/responsive"),
            new("Dark mode", "/docs/dark-mode"),
            new("Custom themes", "/docs/custom-themes"),
            new("Colors", "/docs/colors"),
        ]),
        new("Utilities",
        [
            new("Layout", "/docs/layout"),
            new("Flexbox & grid", "/docs/flexbox-grid"),
            new("Spacing", "/docs/spacing"),
            new("Sizing", "/docs/sizing"),
            new("Typography", "/docs/typography"),
            new("Backgrounds", "/docs/backgrounds"),
            new("Borders & effects", "/docs/borders-effects"),
            new("Interactivity", "/docs/interactivity"),
            new("Tables & SVG", "/docs/tables-svg"),
        ]),
    ];
}
