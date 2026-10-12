using FluentCss.Core;
using static FluentCss.Core.FluentCssBuilder;

namespace FluentCssBlazoreInteractiveWasm.Client;

/// <summary>
/// Classes used only by the dynamically loaded component. Kept separate from
/// <see cref="Styles"/> so they are not registered until the component actually
/// renders on the client, after the initial stylesheet has been emitted.
/// </summary>
public static class PartialStyles
{
    public static string Card { get; } =
        $"{Css.P(6).RoundedLg().Border().BorderDashed().BorderColor(Color.Amber400).Background(Color.Amber50).ShadowLg()} " +
        $"{Dark.BorderColor(Color.Amber700)} {Dark.Background(Color.Amber950)}";

    public static string Title { get; } =
        $"{Css.FontBold().Color(Color.Amber800).Mb(1)} {Dark.Color(Color.Amber200)}";

    public static string Body { get; } =
        $"{Css.TextSm().Color(Color.Amber700)} {Dark.Color(Color.Amber300)}";

    public static string Button { get; } =
        $"{Css.Mt(4).Px(5).Py(2).RoundedLg().Background(Color.Emerald600).Color(Color.White).FontBold().CursorPointer().Hover(h => h.Background(Color.Emerald700))}";

    // Registered even later, the first time the card's button is clicked.
    public static string Highlight { get; } =
        $"{Css.Ring(4, Color.Emerald300)} {Dark.Ring(4, Color.Emerald700)}";
}
