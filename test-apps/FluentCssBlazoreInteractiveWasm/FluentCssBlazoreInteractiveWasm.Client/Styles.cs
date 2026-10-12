using FluentCss.Core;
using static FluentCss.Core.FluentCssBuilder;

namespace FluentCssBlazoreInteractiveWasm.Client;

/// <summary>
/// Class strings for the app shell and pages, built once at startup.
/// Precomputing them keeps the fluent builder off the render path.
/// </summary>
public static class Styles
{
    // Shell
    public static string Shell { get; } =
        $"{Css.MinH(Size.ScreenH).Flex(FlexDirection.Col).Background(Color.Slate50)} " +
        $"{Dark.Background(Color.Slate950)}";

    public static string Header { get; } =
        $"{Css.WFull().Border().BorderColor(Color.Slate200).Background(Color.White)} " +
        $"{Dark.BorderColor(Color.Slate800)} " +
        $"{Dark.Background(Color.Slate900)}";

    public static string Nav { get; } =
        $"{Css.MaxW(Size.Lg).MxAuto().Px(6).H(16).Flex(FlexDirection.Row, Justify.Between, Align.Center)}";

    public static string Brand { get; } =
        $"{Css.TextLg().FontBold().NoUnderline().Color(Color.Blue600)} " +
        $"{Dark.Color(Color.Blue400)}";

    public static string NavLinks { get; } =
        $"{Css.Flex(FlexDirection.Row, align: Align.Center, gap: 2)}";

    public static string NavLink { get; } =
        $"{Css.Px(3).Py(2).RoundedMd().NoUnderline().Color(Color.Slate600).Hover(h => h.Background(Color.Slate100))}";

    public static string NavLinkActive { get; } =
        $"{Css.Px(3).Py(2).RoundedMd().NoUnderline().FontBold().Background(Color.Blue100).Color(Color.Blue700)}";

    // Theme toggle
    public static string ThemeButton { get; } =
        $"{Css.Px(3).Py(2).RoundedMd().CursorPointer().TextSm().FontBold().Color(Color.Slate600).Background(Color.Slate100).Hover(h => h.Background(Color.Slate200))} " +
        $"{Dark.Background(Color.Slate800).Color(Color.Slate200)} " +
        $"{Dark.Hover(h => h.Background(Color.Slate700))}";

    public static string ThemeLightIcon { get; } =
        $"{Css.W(5).H(5).Block()} {Dark.Hidden()}";

    public static string ThemeDarkIcon { get; } =
        $"{Css.W(5).H(5).Hidden()} {Dark.Block()}";

    public static string Main { get; } =
        $"{Css.Grow().WFull().MaxW(Size.Lg).MxAuto().P(6)}";

    public static string Footer { get; } =
        $"{Css.Border().BorderColor(Color.Slate200).P(6).TextCenter().TextSm().Color(Color.Slate500)} " +
        $"{Dark.BorderColor(Color.Slate800)} " +
        $"{Dark.Color(Color.Slate400)}";

    // Typography
    public static string PageTitle { get; } =
        $"{Css.Text2Xl().FontBold().Mb(4)} {Dark.Color(Color.White)}";

    public static string Lead { get; } =
        $"{Css.Color(Color.Slate600).Leading(7).Mb(6)} {Dark.Color(Color.Slate300)}";

    public static string Subtitle { get; } =
        $"{Css.Color(Color.Slate600).Mb(6)} {Dark.Color(Color.Slate300)}";

    public static string BodyText { get; } =
        $"{Css.Color(Color.Slate600)} {Dark.Color(Color.Slate300)}";

    // About
    public static string FactGrid { get; } =
        $"{Css.Grid().GridCols(1).Gap(4)} {Css.MinMd(md => md.GridCols(3))}";

    public static string FactCard { get; } =
        $"{Css.P(5).RoundedLg().Border().BorderColor(Color.Slate200).Background(Color.White)} " +
        $"{Dark.BorderColor(Color.Slate800)} {Dark.Background(Color.Slate900)}";

    public static string FactTitle { get; } =
        $"{Css.FontBold().Color(Color.Slate900).Mb(1)} {Dark.Color(Color.White)}";

    public static string FactBody { get; } =
        $"{Css.TextSm().Color(Color.Slate500)} {Dark.Color(Color.Slate400)}";

    // Contact
    public static string FormPanel { get; } =
        $"{Css.MaxW(Size.Md).P(6).RoundedLg().Border().BorderColor(Color.Slate200).Background(Color.White)} " +
        $"{Dark.BorderColor(Color.Slate800)} {Dark.Background(Color.Slate900)}";

    public static string Field { get; } = $"{Css.Mb(4)}";

    public static string Label { get; } =
        $"{Css.Block().TextSm().FontBold().Color(Color.Slate700).Mb(1)} {Dark.Color(Color.Slate300)}";

    public static string Input { get; } =
        $"{Css.WFull().Px(3).Py(2).RoundedMd().Border().BorderColor(Color.Slate300)} " +
        $"{Dark.Background(Color.Slate800)} {Dark.BorderColor(Color.Slate700)}";

    public static string ButtonPrimary { get; } =
        $"{Css.Px(6).Py(3).RoundedLg().Background(Color.Blue600).Color(Color.White).FontBold().CursorPointer().Hover(h => h.Background(Color.Blue700))}";

    // Home
    public static string Hero { get; } = $"{Css.Py(12).TextCenter()}";

    public static string HeroTitle { get; } =
        $"{Css.Text3Xl().FontBold().Color(Color.Slate900).Mb(4)} {Dark.Color(Color.White)}";

    public static string HeroLead { get; } =
        $"{Css.TextLg().Color(Color.Slate600).MaxW(Size.Md).MxAuto()} {Dark.Color(Color.Slate400)}";

    public static string ButtonCta { get; } =
        $"{Css.InlineBlock().Mt(6).Px(6).Py(3).RoundedLg().Background(Color.Blue600).Color(Color.White).FontBold().NoUnderline().Hover(h => h.Background(Color.Blue700))}";

    // Error / not found
    public static string CenteredSection { get; } = $"{Css.TextCenter().Py(12)}";

    public static string ErrorCode { get; } =
        $"{Css.Text3Xl().FontBold().Color(Color.Blue600).Mb(2)} {Dark.Color(Color.Blue400)}";

    // Load-component demo. The slot lives here, but the classes inside the dynamically
    // loaded component live in <see cref="PartialStyles"/> so they are registered only
    // when the component itself renders on the client.
    public static string PartialSlot { get; } = $"{Css.Mt(6)}";
}
