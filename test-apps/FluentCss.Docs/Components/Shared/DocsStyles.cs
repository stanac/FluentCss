using FluentCss.Core;
using static FluentCss.Core.FluentCssBuilder;

namespace FluentCss.Docs.Components.Shared;

/// <summary>
/// Every class string used by the documentation site is produced by the FluentCss
/// builder itself. Keeping them here makes the Razor markup readable and proves
/// that the framework is capable of styling a real application.
/// </summary>
public static class DocsStyles
{
    // ---------------------------------------------------------------------
    // Themes
    //
    // The docs support the built-in light/dark pair plus two transient custom
    // themes used by the /docs/custom-themes playground. A theme is nothing more
    // than an ancestor class: when an element carries "theme-ocean" the
    // "theme-ocean:" utilities below start to apply. The custom themes are never
    // written to a cookie, so they cannot leak to other pages.
    // ---------------------------------------------------------------------

    /// <summary>Ancestor class that activates the ocean custom theme.</summary>
    public const string OceanThemeClass = "theme-ocean";

    /// <summary>Ancestor class that activates the sunset custom theme.</summary>
    public const string SunsetThemeClass = "theme-sunset";

    /// <summary>
    /// Applied to <c>&lt;body&gt;</c> by every docs host. The custom theme variants
    /// only take effect while the page opts in by adding the ancestor class.
    /// </summary>
    public static string BodyClasses { get; } =
        $"{Css.Background(Color.White).Color(Color.Slate900)} " +
        $"{Dark.Background(Color.Slate950).Color(Color.Slate100)} " +
        $"{Theme("ocean").Background(Color.Sky50).Color(Color.Slate900)} " +
        $"{Theme("sunset").Background(Color.Orange50).Color(Color.Slate900)}";

    /// <summary>
    /// A component class documented on the /docs/selectors page. The base rule is
    /// registered with <c>ApplyClass</c> and the hover rule with the fluent selector
    /// builder, once, in the static constructor below.
    /// </summary>
    public const string PillClass = "docs-pill";

    static DocsStyles()
    {
        DocsAppliedStyles.Initialize();

        Css.ApplyClass(PillClass, css => css
            .InlineBlock()
            .Px(3)
            .Py(1)
            .RoundedFull()
            .TextXs()
            .FontBold()
            .Background(Color.Blue100)
            .Color(Color.Blue700));

        Css.ApplySelector(s => s.Class(PillClass).Hover(), css => css
            .Background(Color.Blue200)
            .Color(Color.Blue800));
    }

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
        $"{Theme("ocean").Background(Color.Sky100)} " +
        $"{Theme("sunset").Background(Color.Orange100)} " +
        $"{Css.MinMd(md => md.W(64).Flex(FlexDirection.Col).HScreen().Sticky().Top(0).OverflowAuto())}";

    public static string ContentWrap { get; } =
        $"{Css.Grow().MinW(Size.Zero).Flex(FlexDirection.Col)}";

    public static string Header { get; } =
        $"{Css.Sticky().Top(0).Z(10).H(14).Px(4).ShadowSm()} " +
        $"{Css.Flex(FlexDirection.Row, Justify.Between, Align.Center)} " +
        $"{Css.Background(Color.White)} " +
        $"{Dark.Background(Color.Slate900)} " +
        $"{Theme("ocean").Background(Color.Sky100)} " +
        $"{Theme("sunset").Background(Color.Orange100)}";

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

    public static string ContentRow { get; } =
        $"{Css.WFull().Flex(FlexDirection.Row, Justify.Center, align: Align.Start, gap: 8)}";

    // "On this page" navigation. The container starts hidden and its list is built by
    // docs.js from the headings that are actually rendered, so no page hard-codes it.
    public static string Toc { get; } =
        $"{Css.Hidden().W(56).Sticky().Top(20).P(4).RoundedLg()} " +
        $"{Css.Border().BorderColor(Color.Slate200)} " +
        $"{Dark.BorderColor(Color.Slate800)} " +
        $"{Css.Media("width > 80rem", "min-xl", xl => xl.Block())}";

    public static string TocTitle { get; } =
        $"{Css.TextXs().FontBold().Uppercase().Color(Color.Slate400).Mb(3)} " +
        $"{Dark.Color(Color.Slate500)}";

    public static string TocNav { get; } =
        $"{Css.Flex(FlexDirection.Col, gap: 1)}";

    public static string TocLink { get; } =
        $"{Css.Block().TextSm().NoUnderline().Color(Color.Slate500).Py(1).Leading(5)} " +
        $"{Css.Hover(h => h.Color(Color.Slate900))} " +
        $"{Dark.Color(Color.Slate400)} {Dark.Hover(h => h.Color(Color.White))}";

    public static string TocLinkActive { get; } =
        $"{Css.Block().TextSm().NoUnderline().FontBold().Color(Color.Blue600).Py(1).Leading(5)} " +
        $"{Dark.Color(Color.Blue400)}";

    // ---------------------------------------------------------------------
    // Branding / navigation
    // ---------------------------------------------------------------------

    public static string Brand { get; } =
        $"{Css.Flex(FlexDirection.Row, align: Align.Center, gap: 3)} " +
        $"{Css.NoUnderline().FontBold().TextXl().Color(Color.Slate900)} " +
        $"{Dark.Color(Color.White)}";

    public static string BrandMark { get; } =
        $"{Css.W(8).H(8).Block().RoundedMd()}";

    public static string SectionTitle { get; } =
        $"{Css.TextXs().FontBold().Uppercase().Mt(6).Mb(2).Color(Color.Slate400)}";

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
    // Previous / next pager
    // ---------------------------------------------------------------------

    public static string Pager { get; } =
        $"{Css.Mt(12).Pt(6).Flex(FlexDirection.Row, Justify.Between, gap: 4)} " +
        $"{Css.Border(0)} " +
        $"{Dark.BorderColor(Color.Slate800)}";

    public static string PagerLink { get; } =
        $"{Css.Flex(FlexDirection.Col, gap: 1).Px(4).Py(3).RoundedLg().NoUnderline()} " +
        $"{Css.Border().BorderColor(Color.Slate200).Background(Color.White)} " +
        $"{Css.Hover(h => h.BorderColor(Color.Blue400).Background(Color.Blue50))} " +
        $"{Dark.BorderColor(Color.Slate800).Background(Color.Slate900)} " +
        $"{Dark.Hover(h => h.BorderColor(Color.Blue700).Background(Color.Slate800))}";

    public static string PagerLinkNext { get; } =
        $"{PagerLink} {Css.Flex(FlexDirection.Col, align: Align.End).TextRight()}";

    public static string PagerHint { get; } =
        $"{Css.TextXs().FontBold().Uppercase().Color(Color.Slate400)}";

    public static string PagerTitle { get; } =
        $"{Css.FontBold().Color(Color.Slate900)} {Dark.Color(Color.White)}";

    // ---------------------------------------------------------------------
    // Examples / cards
    // ---------------------------------------------------------------------

    public static string Example { get; } =
        $"{Css.Mb(8)}";

    public static string ExamplePreview { get; } =
        $"{Css.P(6).RoundedLg().Border().BorderColor(Color.Slate200).Mb(4)} " +
        $"{Css.Background(Color.Slate50)} " +
        $"{Dark.BorderColor(Color.Slate800).Background(Color.Slate900)} " +
        $"{Theme("ocean").Background(Color.Sky50).BorderColor(Color.Sky300)} " +
        $"{Theme("sunset").Background(Color.Orange50).BorderColor(Color.Orange300)}";

    public static string Card { get; } =
        $"{Css.P(6).RoundedLg().Border().BorderColor(Color.Slate200)} " +
        $"{Css.Background(Color.White)} " +
        $"{Dark.BorderColor(Color.Slate800).Background(Color.Slate900)} " +
        $"{Theme("ocean").Background(Color.Sky50).BorderColor(Color.Sky300)} " +
        $"{Theme("sunset").Background(Color.Orange50).BorderColor(Color.Orange300)}";

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
    // Colors (palette swatches)
    // ---------------------------------------------------------------------

    public static string ColorFamilyRow { get; } = $"{Css.Mb(3)}";

    public static string ColorFamilyName { get; } =
        $"{Css.TextXs().FontBold().Color(Color.Slate500).Mb(1)} {Dark.Color(Color.Slate400)}";

    public static string ColorSwatchRow { get; } =
        $"{Css.Flex(FlexDirection.Row).RoundedMd().OverflowHidden()}";

    public static string ColorSwatch(Color shade) =>
        $"{Css.H(8).Grow()} {Css.Background(shade)}";

    // ---------------------------------------------------------------------
    // Code samples (inline helpers for pages)
    // ---------------------------------------------------------------------

    public static string CodeTag { get; } = InlineCode;

    // ---------------------------------------------------------------------
    // Custom theme playground (see /docs/custom-themes)
    //
    // These classes pair a neutral look with theme-ocean / theme-sunset variants so
    // the compact app preview on that page re-skins when the ancestor class changes.
    // ---------------------------------------------------------------------

    public static string CustomThemeToolbar { get; } =
        $"{Css.Flex(FlexDirection.Row, gap: 3).Mb(6)}";

    public static string CustomThemeStage { get; } =
        $"{Css.P(6).RoundedLg().Border().BorderColor(Color.Slate200)} " +
        $"{Css.Background(Color.White)} " +
        $"{Dark.BorderColor(Color.Slate800).Background(Color.Slate900)} " +
        $"{Theme("ocean").Background(Color.Sky50).BorderColor(Color.Sky300)} " +
        $"{Theme("sunset").Background(Color.Orange50).BorderColor(Color.Orange300)}";

    public static string CustomThemeBar { get; } =
        $"{Css.Flex(FlexDirection.Row, Justify.Between, align: Align.Center).P(4).RoundedMd().Mb(4)} " +
        $"{Css.Background(Color.Slate100)} " +
        $"{Dark.Background(Color.Slate800)} " +
        $"{Theme("ocean").Background(Color.Sky200)} " +
        $"{Theme("sunset").Background(Color.Orange200)}";

    public static string CustomThemeAccent { get; } =
        $"{Css.Px(4).Py(2).RoundedMd().FontBold().Color(Color.White)} " +
        $"{Css.Background(Color.Slate700)} " +
        $"{Theme("ocean").Background(Color.Sky600)} " +
        $"{Theme("sunset").Background(Color.Orange600)}";

    public static string CustomThemeCard { get; } =
        $"{Css.P(4).RoundedMd().Border().BorderColor(Color.Slate200).Background(Color.White)} " +
        $"{Dark.BorderColor(Color.Slate700).Background(Color.Slate800)} " +
        $"{Theme("ocean").Background(Color.White).BorderColor(Color.Sky300)} " +
        $"{Theme("sunset").Background(Color.White).BorderColor(Color.Orange300)}";

    public static string CustomThemeChip { get; } =
        $"{Css.InlineBlock().Px(2).Py(1).RoundedFull().TextXs().FontBold()} " +
        $"{Css.Background(Color.Slate200).Color(Color.Slate700)} " +
        $"{Dark.Background(Color.Slate700).Color(Color.Slate200)} " +
        $"{Theme("ocean").Background(Color.Sky300).Color(Color.Sky900)} " +
        $"{Theme("sunset").Background(Color.Orange300).Color(Color.Orange900)}";

    // ---------------------------------------------------------------------
    // Search
    //
    // The markup is rendered here (so it is styled with the builder like the rest
    // of the site) but the behaviour lives in docs-search.js. Dynamic result nodes
    // read their class names from data attributes on the results list, mirroring
    // how the "On this page" nav passes its classes to docs.js.
    // ---------------------------------------------------------------------

    // Centred in the header by the "fc-header-search" applied selector in
    // DocsAppliedStyles. It is positioned without transform so the fixed-position
    // palette inside is not trapped by a transformed containing block.
    public static string HeaderSearch { get; } = "fc-header-search";

    public static string SearchButton { get; } =
        $"{Css.Flex(FlexDirection.Row, Justify.Between, align: Align.Center, gap: 2).WFull()} " +
        $"{Css.Px(3).Py(2).RoundedMd().CursorPointer().TextSm()} " +
        $"{Css.Color(Color.Slate500).Background(Color.Slate100)} " +
        $"{Css.Hover(h => h.Background(Color.Slate200).Color(Color.Slate700))} " +
        $"{Dark.Background(Color.Slate800).Color(Color.Slate400)} " +
        $"{Dark.Hover(h => h.Background(Color.Slate700).Color(Color.Slate200))}";

    public static string SearchButtonMain { get; } =
        $"{Css.Flex(FlexDirection.Row, align: Align.Center, gap: 2)}";

    public static string SearchButtonLabel { get; } =
        $"{Css.Hidden()} {Css.MinMd(md => md.Inline())}";

    public static string SearchKbd { get; } =
        $"{Css.Hidden().RoundedSm().Border().BorderColor(Color.Slate300).Px(1).TextXs()} " +
        $"{Css.Color(Color.Slate400)} " +
        $"{Css.MinMd(md => md.InlineBlock())} " +
        $"{Dark.BorderColor(Color.Slate600).Color(Color.Slate500)}";

    public static string SearchOverlay { get; } =
        $"fc-search-overlay {Css.Fixed().Inset(0).Z(50).P(4).OverflowAuto()} " +
        $"{Css.Flex(FlexDirection.Row, Justify.Center, align: Align.Start)}";

    public static string SearchDialog { get; } =
        $"{Css.WFull().MaxW(Size.Md).MxAuto().Mt(16).RoundedLg().ShadowLg().OverflowHidden()} " +
        $"{Css.Background(Color.White).Border().BorderColor(Color.Slate200)} " +
        $"{Dark.Background(Color.Slate900).BorderColor(Color.Slate700)}";

    public static string SearchInputRow { get; } =
        $"fc-search-input-row {Css.Flex(FlexDirection.Row, align: Align.Center, gap: 3).Px(4).Py(3)}";

    public static string SearchIcon { get; } =
        $"{Css.W(5).H(5).Shrink()} {Css.Color(Color.Slate400)}";

    public static string SearchInput { get; } =
        $"fc-search-input {Css.Grow().WFull().Border(0).Background(Color.White).Color(Color.Slate900).TextBase()} " +
        $"{Dark.Background(Color.Slate900).Color(Color.Slate100)}";

    public static string SearchStatus { get; } =
        $"{Css.Px(4).Pt(2).TextXs().Color(Color.Slate400)} {Dark.Color(Color.Slate500)}";

    public static string SearchResults { get; } =
        $"fc-search-results {Css.M(0).P(0).Mb(2)}";

    public static string SearchResult { get; } =
        $"{Css.Block().Px(4).Py(3).NoUnderline()} " +
        $"{Css.Hover(h => h.Background(Color.Slate100))} " +
        $"{Dark.Hover(h => h.Background(Color.Slate800))}";

    public static string SearchResultActive { get; } =
        $"{Css.Background(Color.Slate100)} {Dark.Background(Color.Slate800)}";

    public static string SearchResultPath { get; } =
        $"{Css.Block().TextXs().FontBold().Uppercase().Color(Color.Slate400).Mb(1)} " +
        $"{Dark.Color(Color.Slate500)}";

    public static string SearchResultTitle { get; } =
        $"{Css.Block().FontBold().Color(Color.Slate900)} {Dark.Color(Color.White)}";

    public static string SearchResultText { get; } =
        $"{Css.Block().TextSm().Color(Color.Slate500).Mt(1)} {Dark.Color(Color.Slate400)}";

    public static string SearchMark { get; } =
        $"{Css.Background(Color.Amber200).Color(Color.Amber900).RoundedSm()} " +
        $"{Dark.Background(Color.Amber900).Color(Color.Amber100)}";

    public static string SearchEmpty { get; } =
        $"{Css.Px(4).Py(6).TextSm().TextCenter().Color(Color.Slate400)} {Dark.Color(Color.Slate500)}";
}
