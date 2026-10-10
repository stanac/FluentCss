namespace FluentCssWeb.Components;

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
            new("Installation", "/installation"),
            new("How it works", "/how-it-works"),
        ]),
        new("Core concepts",
        [
            new("Utility classes", "/utility-classes"),
            new("Hover & focus", "/states"),
            new("Responsive design", "/responsive"),
            new("Dark mode", "/dark-mode"),
            new("Colors", "/colors"),
        ]),
        new("Utilities",
        [
            new("Layout", "/layout"),
            new("Flexbox & grid", "/flexbox-grid"),
            new("Spacing", "/spacing"),
            new("Sizing", "/sizing"),
            new("Typography", "/typography"),
            new("Borders & effects", "/borders-effects"),
        ]),
    ];
}
