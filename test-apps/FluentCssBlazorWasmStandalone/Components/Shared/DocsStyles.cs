using FluentCss.Core;
using static FluentCss.Core.FluentCssBuilder;

namespace FluentCssBlazorWasmStandalone.Components.Shared;

/// <summary>
/// Every class string used by the documentation site is produced by the FluentCss
/// builder itself. Keeping them here makes the Razor markup readable and proves
/// that the framework is capable of styling a real application.
/// </summary>
public static class DocsStyles
{
    // ---------------------------------------------------------------------
    // Layout shell
    // ---------------------------------------------------------------------

    public static string Shell { get; } =
        $"{Css.MinH(Size.ScreenH).Flex(FlexDirection.Col)} " +
        $"{Css.MinMd(md => md.Flex(FlexDirection.Row))}";

    public static string Sidebar { get; } =
        $"{Css.WFull().Flex(FlexDirection.Col).Hidden()} " +
        $"{Css.P(4).Background(Color.Slate50)} " +
        $"{Dark.Background(Color.Slate900)} " +
        $"{Css.MinMd(md => md.W(64).Flex(FlexDirection.Col).HScreen().Sticky().Top(0).OverflowAuto())}";

    public static string ContentWrap { get; } =
        $"{Css.Grow().MinW(Size.Zero).Flex(FlexDirection.Col)}";

    public static string Header { get; } =
        $"{Css.Sticky().Top(0).Z(10).H(14).Px(4).ShadowSm()} " +
        $"{Css.Flex(FlexDirection.Row, Justify.Between, Align.Center)} " +
        $"{Css.Background(Color.White)} " +
        $"{Dark.Background(Color.Slate900)}";

    public static string HeaderLeft { get; } =
        $"{Css.Flex(FlexDirection.Row, align: Align.Center, gap: 3)}";

    public static string HeaderLink { get; } =
        $"{Css.TextSm().FontBold().NoUnderline().Color(Color.Slate600)} " +
        $"{Dark.Color(Color.Slate300)} " +
        $"{Css.Hover(h => h.Color(Color.Blue600))} " +
        $"{Dark.Hover(h => h.Color(Color.Blue400))}";

    public static string ThemeButton { get; } =
        $"{Css.Px(3).Py(2).RoundedMd().CursorPointer().TextSm().FontBold().Color(Color.Slate600)} " +
        $"{Css.Background(Color.Slate100)} " +
        $"{Css.Hover(h => h.Background(Color.Slate200))} " +
        $"{Dark.Background(Color.Slate800).Color(Color.Slate200)} " +
        $"{Dark.Hover(h => h.Background(Color.Slate700))}";

    public static string ThemeLightIcon { get; } =
        $"{Css.W(5).H(5).Block()} {Dark.Hidden()}";

    public static string ThemeDarkIcon { get; } =
        $"{Css.W(5).H(5).Hidden()} {Dark.Block()}";

    public static string MenuButton { get; } =
        $"{Css.InlineBlock().Px(3).Py(2).RoundedMd().CursorPointer().FontBold().Color(Color.Slate600)} " +
        $"{Css.Background(Color.Slate100)} " +
        $"{Css.Hover(h => h.Background(Color.Slate200))} " +
        $"{Dark.Background(Color.Slate800).Color(Color.Slate200)} " +
        $"{Css.MinMd(md => md.Hidden())}";

    public static string Content { get; } =
        $"{Css.Grow().P(6)} " +
        $"{Css.MinMd(md => md.P(10))}";

    public static string Article { get; } =
        $"{Css.MaxW(Size.Lg).MxAuto().WFull()}";

    // ---------------------------------------------------------------------
    // Branding / navigation
    // ---------------------------------------------------------------------

    public static string Brand { get; } =
        $"{Css.Flex(FlexDirection.Row, align: Align.Center, gap: 3)} " +
        $"{Css.NoUnderline().FontBold().TextXl().Color(Color.Slate900)} " +
        $"{Dark.Color(Color.White)}";

    public static string BrandMark { get; } =
        $"{Css.W(8).H(8).RoundedMd().TextCenter().TextSm().FontBold()} " +
        $"{Css.Background(Color.Blue600).Color(Color.White)} " +
        $"{Css.Flex(FlexDirection.Row, Justify.Center, Align.Center, gap: 0)}";

    public static string SectionTitle { get; } =
        $"{Css.TextXs().FontBold().Uppercase().Mt(6).Mb(2).Color(Color.Slate400)}";

    public static string NavList { get; } = "";

    public static string NavLink { get; } =
        $"{Css.Block().Px(3).Py(2).RoundedMd().TextSm().NoUnderline().Color(Color.Slate600)} " +
        $"{Css.Hover(h => h.Background(Color.Slate200).Color(Color.Slate900))} " +
        $"{Dark.Color(Color.Slate400)} " +
        $"{Dark.Hover(h => h.Background(Color.Slate800).Color(Color.White))}";

    public static string NavActive { get; } =
        $"{Css.Block().Px(3).Py(2).RoundedMd().TextSm().NoUnderline().FontBold()} " +
        $"{Css.Background(Color.Blue100).Color(Color.Blue700)} " +
        $"{Dark.Background(Color.Blue950).Color(Color.Blue300)}";

    // ---------------------------------------------------------------------
    // Typography
    // ---------------------------------------------------------------------

    public static string H1 { get; } =
        $"{Css.Text3Xl().FontBold().Color(Color.Slate900).Mb(3)} " +
        $"{Dark.Color(Color.White)}";

    public static string Lead { get; } =
        $"{Css.TextLg().Color(Color.Slate600).Mb(8)} " +
        $"{Dark.Color(Color.Slate400)}";

    public static string H2 { get; } =
        $"{Css.Text2Xl().FontBold().Color(Color.Slate900).Mt(10).Mb(3)} " +
        $"{Dark.Color(Color.White)}";

    public static string H3 { get; } =
        $"{Css.TextLg().FontBold().Color(Color.Slate900).Mt(8).Mb(2)} " +
        $"{Dark.Color(Color.White)}";

    public static string H4 { get; } =
        $"{Css.TextBase().FontBold().Color(Color.Slate900).Mt(6).Mb(2)} " +
        $"{Dark.Color(Color.White)}";

    public static string P { get; } =
        $"{Css.Color(Color.Slate600).Leading(7).Mb(4)} " +
        $"{Dark.Color(Color.Slate300)}";

    public static string Small { get; } =
        $"{Css.TextSm().Color(Color.Slate500)} " +
        $"{Dark.Color(Color.Slate400)}";

    public static string Strong { get; } =
        $"{Css.FontBold().Color(Color.Slate900)} " +
        $"{Dark.Color(Color.White)}";

    public static string Link { get; } =
        $"{Css.FontBold().Color(Color.Blue600).NoUnderline()} " +
        $"{Css.Hover(h => h.Underline())} " +
        $"{Dark.Color(Color.Blue400)}";

    public static string InlineCode { get; } =
        $"{Css.Background(Color.Slate100).Color(Color.Pink700).Px(1).RoundedSm().TextSm()} " +
        $"{Dark.Background(Color.Slate800).Color(Color.Pink300)}";

    public static string CodeBlock { get; } =
        $"{Css.Background(Color.Slate100).Color(Color.Slate800).P(4).RoundedLg()} " +
        $"{Css.OverflowAuto().TextSm().Leading(6).Mb(6)} " +
        $"{Dark.Background(Color.Black).Color(Color.Slate100)}";

    public static string Ul { get; } =
        $"{Css.Mb(4)}";

    public static string Li { get; } =
        $"{Css.Flex(FlexDirection.Row, gap: 2).Mb(2).Color(Color.Slate600).Leading(7)} " +
        $"{Dark.Color(Color.Slate300)}";

    public static string Bullet { get; } =
        $"{Css.Color(Color.Blue500).FontBold()}";

    // ---------------------------------------------------------------------
    // Buttons / badges
    // ---------------------------------------------------------------------

    public static string ButtonPrimary { get; } =
        $"{Css.InlineBlock().Px(6).Py(3).RoundedLg().FontBold().NoUnderline().TextCenter().CursorPointer()} " +
        $"{Css.Background(Color.Blue600).Color(Color.White)} " +
        $"{Css.Hover(h => h.Background(Color.Blue700))}";

    public static string ButtonSecondary { get; } =
        $"{Css.InlineBlock().Px(6).Py(3).RoundedLg().FontBold().NoUnderline().TextCenter().CursorPointer()} " +
        $"{Css.Background(Color.White).Color(Color.Slate700).Border().BorderColor(Color.Slate200)} " +
        $"{Css.Hover(h => h.Background(Color.Slate50))} " +
        $"{Dark.Background(Color.Slate900).Color(Color.Slate200).BorderColor(Color.Slate700)} " +
        $"{Dark.Hover(h => h.Background(Color.Slate800))}";

    public static string Badge { get; } =
        $"{Css.InlineBlock().Px(3).Py(1).RoundedFull().TextXs().FontBold()} " +
        $"{Css.Background(Color.Blue100).Color(Color.Blue700)} " +
        $"{Dark.Background(Color.Blue950).Color(Color.Blue300)}";

    public static string SourceTag { get; } =
        $"{Css.TextXs().Mb(1).FontBold().Uppercase().Color(Color.Blue600)} " +
        $"{Dark.Color(Color.Blue400)}";

    public static string PageLinkCard { get; } =
        $"{Css.Block().P(5).RoundedLg().Border().BorderColor(Color.Slate200).NoUnderline()} " +
        $"{Css.Background(Color.White)} " +
        $"{Css.Hover(h => h.BorderColor(Color.Blue400).Background(Color.Blue50))} " +
        $"{Dark.BorderColor(Color.Slate800).Background(Color.Slate900)} " +
        $"{Dark.Hover(h => h.BorderColor(Color.Blue700).Background(Color.Slate800))}";

    // ---------------------------------------------------------------------
    // Examples / cards
    // ---------------------------------------------------------------------

    public static string Example { get; } =
        $"{Css.Mb(8)}";

    public static string ExamplePreview { get; } =
        $"{Css.P(6).RoundedLg().Border().BorderColor(Color.Slate200).Mb(4)} " +
        $"{Css.Background(Color.Slate50)} " +
        $"{Dark.BorderColor(Color.Slate800).Background(Color.Slate900)}";

    public static string Card { get; } =
        $"{Css.P(6).RoundedLg().Border().BorderColor(Color.Slate200)} " +
        $"{Css.Background(Color.White)} " +
        $"{Dark.BorderColor(Color.Slate800).Background(Color.Slate900)}";

    public static string CardHover { get; } =
        $"{Css.Hover(h => h.ShadowLg())}";

    public static string CardTitle { get; } =
        $"{Css.FontBold().Color(Color.Slate900).Mb(2)} " +
        $"{Dark.Color(Color.White)}";

    // ---------------------------------------------------------------------
    // Tables
    // ---------------------------------------------------------------------

    public static string Table { get; } =
        $"{Css.WFull().TextSm().Mb(8).RoundedLg().Border().BorderColor(Color.Slate200)} " +
        $"{Css.OverflowHidden()} " +
        $"{Dark.BorderColor(Color.Slate800)}";

    public static string Thead { get; } =
        $"{Css.Background(Color.Slate50).Color(Color.Slate500)} " +
        $"{Dark.Background(Color.Slate900).Color(Color.Slate400)}";

    public static string Th { get; } =
        $"{Css.Px(4).Py(3).TextLeft().FontBold().Uppercase().TextXs()}";

    public static string Td { get; } =
        $"{Css.Px(4).Py(3).TextLeft().Color(Color.Slate600)} " +
        $"{Dark.Color(Color.Slate300)}";

    public static string Tr { get; } =
        $"{Css.Border().BorderColor(Color.Slate200)} " +
        $"{Dark.BorderColor(Color.Slate800)}";

    // ---------------------------------------------------------------------
    // Callouts
    // ---------------------------------------------------------------------

    public static string CalloutInfo { get; } =
        $"{Css.P(4).RoundedLg().Border().BorderColor(Color.Blue200).Mb(6)} " +
        $"{Css.Background(Color.Blue50)} " +
        $"{Dark.BorderColor(Color.Blue900).Background(Color.Slate900)}";

    public static string CalloutTip { get; } =
        $"{Css.P(4).RoundedLg().Border().BorderColor(Color.Emerald200).Mb(6)} " +
        $"{Css.Background(Color.Emerald50)} " +
        $"{Dark.BorderColor(Color.Emerald900).Background(Color.Slate900)}";

    public static string CalloutWarning { get; } =
        $"{Css.P(4).RoundedLg().Border().BorderColor(Color.Amber200).Mb(6)} " +
        $"{Css.Background(Color.Amber50)} " +
        $"{Dark.BorderColor(Color.Amber900).Background(Color.Slate900)}";

    public static string CalloutTitle { get; } =
        $"{Css.FontBold().Mb(1).Color(Color.Slate900)} " +
        $"{Dark.Color(Color.White)}";

    public static string CalloutBody { get; } =
        $"{Css.TextSm().Leading(6).Color(Color.Slate700)} " +
        $"{Dark.Color(Color.Slate300)}";

    // ---------------------------------------------------------------------
    // Code samples (inline helpers for pages)
    // ---------------------------------------------------------------------

    public static string CodeTag { get; } = InlineCode;
}
