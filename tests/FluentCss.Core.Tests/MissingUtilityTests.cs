namespace FluentCss.Core.Tests;

/// <summary>
/// Coverage for the utility families that were added to close the gaps listed in
/// <c>missing-features.md</c>. Each case asserts the generated class name, the CSS
/// property and the CSS value, which is the contract the registry emits.
/// </summary>
public class MissingUtilityTests
{
    private static void AssertUtility(
        Action<InternalRootFluentCssBuilder> apply,
        string className,
        string property,
        string value)
    {
        ClassRegistry registry = new();
        var sut = new InternalRootFluentCssBuilder(registry, null, null, null);

        apply(sut);

        var added = registry.Single();
        added.ClassName.Should().Be(className);
        added.Property.Should().Be(property);
        added.Value.Should().Be(value);
    }

    [Fact]
    public void Layout_Display_Position_Float_Should_Emit_Expected_Utilities()
    {
        AssertUtility(b => b.InlineFlex(), "inline-flex", "display", "inline-flex");
        AssertUtility(b => b.InlineGrid(), "inline-grid", "display", "inline-grid");
        AssertUtility(b => b.FlowRoot(), "flow-root", "display", "flow-root");
        AssertUtility(b => b.Contents(), "contents", "display", "contents");
        AssertUtility(b => b.ListItem(), "list-item", "display", "list-item");

        AssertUtility(b => b.Static(), "static", "position", "static");
        AssertUtility(b => b.ZAuto(), "z-auto", "z-index", "auto");

        AssertUtility(b => b.FloatLeft(), "float-left", "float", "left");
        AssertUtility(b => b.FloatRight(), "float-right", "float", "right");
        AssertUtility(b => b.FloatNone(), "float-none", "float", "none");

        AssertUtility(b => b.ClearLeft(), "clear-left", "clear", "left");
        AssertUtility(b => b.ClearRight(), "clear-right", "clear", "right");
        AssertUtility(b => b.ClearBoth(), "clear-both", "clear", "both");
        AssertUtility(b => b.ClearNone(), "clear-none", "clear", "none");

        AssertUtility(b => b.Isolate(), "isolate", "isolation", "isolate");
        AssertUtility(b => b.IsolationAuto(), "isolation-auto", "isolation", "auto");

        AssertUtility(b => b.BoxBorder(), "box-border", "box-sizing", "border-box");
        AssertUtility(b => b.BoxContent(), "box-content", "box-sizing", "content-box");

        AssertUtility(b => b.Visible(), "visible", "visibility", "visible");
        AssertUtility(b => b.Invisible(), "invisible", "visibility", "hidden");
    }

    [Fact]
    public void ObjectFit_Should_Emit_Expected_Utilities()
    {
        AssertUtility(b => b.ObjectFill(), "object-fill", "object-fit", "fill");
        AssertUtility(b => b.ObjectScaleDown(), "object-scale-down", "object-fit", "scale-down");
        AssertUtility(b => b.ObjectNone(), "object-none", "object-fit", "none");
    }

    [Fact]
    public void Flex_And_Order_Should_Emit_Expected_Utilities()
    {
        AssertUtility(b => b.Basis(4), "basis-4", "flex-basis", "1rem");
        AssertUtility(b => b.Basis(Length.Rem(50)), "basis-50rem", "flex-basis", "50rem");
        AssertUtility(b => b.BasisAuto(), "basis-auto", "flex-basis", "auto");
        AssertUtility(b => b.BasisFull(), "basis-full", "flex-basis", "100%");

        AssertUtility(b => b.Order(3), "order-3", "order", "3");
        AssertUtility(b => b.OrderFirst(), "order-first", "order", "-9999");
        AssertUtility(b => b.OrderLast(), "order-last", "order", "9999");
        AssertUtility(b => b.OrderNone(), "order-none", "order", "0");

        AssertUtility(b => b.Grow(2), "grow-2", "flex-grow", "2");
        AssertUtility(b => b.Shrink(0), "shrink-0", "flex-shrink", "0");
    }

    [Fact]
    public void Grid_Placement_And_Flow_Should_Emit_Expected_Utilities()
    {
        AssertUtility(b => b.GridAutoFlowRow(), "grid-flow-row", "grid-auto-flow", "row");
        AssertUtility(b => b.GridAutoFlowColumn(), "grid-flow-col", "grid-auto-flow", "column");
        AssertUtility(b => b.GridAutoFlowRowDense(), "grid-flow-row-dense", "grid-auto-flow", "row dense");
        AssertUtility(b => b.GridAutoFlowColumnDense(), "grid-flow-col-dense", "grid-auto-flow", "column dense");

        AssertUtility(b => b.ColEnd(4), "col-end-4", "grid-column-end", "4");
        AssertUtility(b => b.RowStart(2), "row-start-2", "grid-row-start", "2");
        AssertUtility(b => b.RowEnd(3), "row-end-3", "grid-row-end", "3");
    }

    [Fact]
    public void Justify_And_Align_Should_Emit_Expected_Utilities()
    {
        AssertUtility(b => b.JustifyItemsStart(), "justify-items-start", "justify-items", "flex-start");
        AssertUtility(b => b.JustifyItemsCenter(), "justify-items-center", "justify-items", "center");
        AssertUtility(b => b.JustifyItemsEnd(), "justify-items-end", "justify-items", "flex-end");
        AssertUtility(b => b.JustifyItemsStretch(), "justify-items-stretch", "justify-items", "stretch");

        AssertUtility(b => b.JustifySelfAuto(), "justify-self-auto", "justify-self", "auto");
        AssertUtility(b => b.JustifySelfStart(), "justify-self-start", "justify-self", "flex-start");
        AssertUtility(b => b.JustifySelfCenter(), "justify-self-center", "justify-self", "center");
        AssertUtility(b => b.JustifySelfEnd(), "justify-self-end", "justify-self", "flex-end");
        AssertUtility(b => b.JustifySelfStretch(), "justify-self-stretch", "justify-self", "stretch");

        AssertUtility(b => b.AlignContentStart(), "content-start", "align-content", "flex-start");
        AssertUtility(b => b.AlignContentCenter(), "content-center", "align-content", "center");
        AssertUtility(b => b.AlignContentEnd(), "content-end", "align-content", "flex-end");
        AssertUtility(b => b.AlignContentBetween(), "content-between", "align-content", "space-between");
        AssertUtility(b => b.AlignContentAround(), "content-around", "align-content", "space-around");
        AssertUtility(b => b.AlignContentEvenly(), "content-evenly", "align-content", "space-evenly");
        AssertUtility(b => b.AlignContentBaseline(), "content-baseline", "align-content", "baseline");
        AssertUtility(b => b.AlignContentStretch(), "content-stretch", "align-content", "stretch");

        AssertUtility(b => b.AlignSelfAuto(), "self-auto", "align-self", "auto");
        AssertUtility(b => b.AlignSelfStart(), "self-start", "align-self", "flex-start");
        AssertUtility(b => b.AlignSelfCenter(), "self-center", "align-self", "center");
        AssertUtility(b => b.AlignSelfEnd(), "self-end", "align-self", "flex-end");
        AssertUtility(b => b.AlignSelfBaseline(), "self-baseline", "align-self", "baseline");
        AssertUtility(b => b.AlignSelfStretch(), "self-stretch", "align-self", "stretch");

        AssertUtility(b => b.JustifyAround(), "justify-around", "justify-content", "space-around");
        AssertUtility(b => b.JustifyEvenly(), "justify-evenly", "justify-content", "space-evenly");
        AssertUtility(b => b.ItemsBaseline(), "items-baseline", "align-items", "baseline");
        AssertUtility(b => b.ItemsStretch(), "items-stretch", "align-items", "stretch");
    }

    [Fact]
    public void PerSide_Borders_Should_Emit_Expected_Utilities()
    {
        AssertUtility(b => b.BorderT(), "border-t", "border-top-width", "1px");
        AssertUtility(b => b.BorderT(2), "border-t-2", "border-top-width", "2px");
        AssertUtility(b => b.BorderR(), "border-r", "border-right-width", "1px");
        AssertUtility(b => b.BorderR(4), "border-r-4", "border-right-width", "4px");
        AssertUtility(b => b.BorderB(), "border-b", "border-bottom-width", "1px");
        AssertUtility(b => b.BorderB(8), "border-b-8", "border-bottom-width", "8px");
        AssertUtility(b => b.BorderL(), "border-l", "border-left-width", "1px");
        AssertUtility(b => b.BorderL(1), "border-l-1", "border-left-width", "1px");
        AssertUtility(b => b.BorderX(), "border-x", "border-inline-width", "1px");
        AssertUtility(b => b.BorderX(2), "border-x-2", "border-inline-width", "2px");
        AssertUtility(b => b.BorderY(), "border-y", "border-block-width", "1px");
        AssertUtility(b => b.BorderY(2), "border-y-2", "border-block-width", "2px");

        AssertUtility(b => b.BorderDouble(), "border-double", "border-style", "double");

        AssertUtility(b => b.OutlineColor(Color.Blue500), "outline-blue-500", "outline-color", Color.Blue500.Value);
        AssertUtility(b => b.OutlineOffset(4), "outline-offset-4", "outline-offset", "4px");
    }

    [Fact]
    public void FontWeight_And_Leading_Should_Emit_Expected_Utilities()
    {
        AssertUtility(b => b.FontExtralight(), "font-extralight", "font-weight", "200");
        AssertUtility(b => b.FontLight(), "font-light", "font-weight", "300");
        AssertUtility(b => b.FontMedium(), "font-medium", "font-weight", "500");
        AssertUtility(b => b.FontSemibold(), "font-semibold", "font-weight", "600");
        AssertUtility(b => b.FontExtrabold(), "font-extrabold", "font-weight", "800");
        AssertUtility(b => b.FontBlack(), "font-black", "font-weight", "900");

        AssertUtility(b => b.LeadingNone(), "leading-none", "line-height", "1");
        AssertUtility(b => b.LeadingTight(), "leading-tight", "line-height", "1.25");
        AssertUtility(b => b.LeadingSnug(), "leading-snug", "line-height", "1.375");
        AssertUtility(b => b.LeadingNormal(), "leading-normal", "line-height", "1.5");
        AssertUtility(b => b.LeadingRelaxed(), "leading-relaxed", "line-height", "1.625");
        AssertUtility(b => b.LeadingLoose(), "leading-loose", "line-height", "2");
    }

    [Fact]
    public void Tracking_And_Whitespace_Should_Emit_Expected_Utilities()
    {
        AssertUtility(b => b.TrackingTighter(), "tracking-tighter", "letter-spacing", "-0.05em");
        AssertUtility(b => b.TrackingTight(), "tracking-tight", "letter-spacing", "-0.025em");
        AssertUtility(b => b.TrackingNormal(), "tracking-normal", "letter-spacing", "0em");
        AssertUtility(b => b.TrackingWide(), "tracking-wide", "letter-spacing", "0.025em");
        AssertUtility(b => b.TrackingWider(), "tracking-wider", "letter-spacing", "0.05em");
        AssertUtility(b => b.TrackingWidest(), "tracking-widest", "letter-spacing", "0.1em");
        AssertUtility(b => b.Tracking(Length.Em(0.2)), "tracking-0.2em", "letter-spacing", "0.2em");

        AssertUtility(b => b.WhitespaceNormal(), "whitespace-normal", "white-space", "normal");
        AssertUtility(b => b.WhitespacePre(), "whitespace-pre", "white-space", "pre");
        AssertUtility(b => b.WhitespacePreLine(), "whitespace-pre-line", "white-space", "pre-line");
        AssertUtility(b => b.WhitespacePreWrap(), "whitespace-pre-wrap", "white-space", "pre-wrap");
        AssertUtility(b => b.WhitespaceBreakSpaces(), "whitespace-break-spaces", "white-space", "break-spaces");
    }

    [Fact]
    public void Text_Decoration_Overflow_And_Indent_Should_Emit_Expected_Utilities()
    {
        AssertUtility(b => b.LineThrough(), "line-through", "text-decoration-line", "line-through");
        AssertUtility(b => b.Overline(), "overline", "text-decoration-line", "overline");
        AssertUtility(b => b.TextEllipsis(), "text-ellipsis", "text-overflow", "ellipsis");
        AssertUtility(b => b.TextClip(), "text-clip", "text-overflow", "clip");

        AssertUtility(b => b.TextIndent(4), "indent-4", "text-indent", "1rem");
        AssertUtility(b => b.TextIndent(Length.Rem(2)), "indent-2rem", "text-indent", "2rem");
    }

    [Fact]
    public void Truncate_Should_Emit_Overflow_Ellipsis_And_NoWrap()
    {
        ClassRegistry registry = new();
        var sut = new InternalRootFluentCssBuilder(registry, null, null, null);

        sut.Truncate();

        registry.Select(x => x.ClassName).Should().Equal("overflow-hidden", "text-ellipsis", "whitespace-nowrap");
    }

    [Fact]
    public void WordBreak_OverflowWrap_And_Hyphens_Should_Emit_Expected_Utilities()
    {
        AssertUtility(b => b.WordBreakNormal(), "break-normal", "word-break", "normal");
        AssertUtility(b => b.WordBreakAll(), "break-all", "word-break", "break-all");
        AssertUtility(b => b.WordBreakKeepAll(), "break-keep", "word-break", "keep-all");

        AssertUtility(b => b.OverflowWrapNormal(), "wrap-normal", "overflow-wrap", "normal");
        AssertUtility(b => b.OverflowWrapBreakWord(), "wrap-break-word", "overflow-wrap", "break-word");
        AssertUtility(b => b.OverflowWrapAnywhere(), "wrap-anywhere", "overflow-wrap", "anywhere");

        AssertUtility(b => b.HyphensNone(), "hyphens-none", "hyphens", "none");
        AssertUtility(b => b.HyphensManual(), "hyphens-manual", "hyphens", "manual");
        AssertUtility(b => b.HyphensAuto(), "hyphens-auto", "hyphens", "auto");
    }

    [Fact]
    public void List_Style_Should_Emit_Expected_Utilities()
    {
        AssertUtility(b => b.ListStyleNone(), "list-none", "list-style-type", "none");
        AssertUtility(b => b.ListDisc(), "list-disc", "list-style-type", "disc");
        AssertUtility(b => b.ListDecimal(), "list-decimal", "list-style-type", "decimal");
        AssertUtility(b => b.ListInside(), "list-inside", "list-style-position", "inside");
        AssertUtility(b => b.ListOutside(), "list-outside", "list-style-position", "outside");
    }

    [Fact]
    public void Background_Should_Emit_Expected_Utilities()
    {
        AssertUtility(b => b.BgAttachmentFixed(), "bg-fixed", "background-attachment", "fixed");
        AssertUtility(b => b.BgAttachmentLocal(), "bg-local", "background-attachment", "local");
        AssertUtility(b => b.BgAttachmentScroll(), "bg-scroll", "background-attachment", "scroll");

        AssertUtility(b => b.BgClipBorder(), "bg-clip-border", "background-clip", "border-box");
        AssertUtility(b => b.BgClipPadding(), "bg-clip-padding", "background-clip", "padding-box");
        AssertUtility(b => b.BgClipContent(), "bg-clip-content", "background-clip", "content-box");
        AssertUtility(b => b.BgClipText(), "bg-clip-text", "background-clip", "text");

        AssertUtility(b => b.BgOriginBorder(), "bg-origin-border", "background-origin", "border-box");
        AssertUtility(b => b.BgOriginPadding(), "bg-origin-padding", "background-origin", "padding-box");
        AssertUtility(b => b.BgOriginContent(), "bg-origin-content", "background-origin", "content-box");

        AssertUtility(b => b.BgRepeat(), "bg-repeat", "background-repeat", "repeat");
        AssertUtility(b => b.BgRepeatX(), "bg-repeat-x", "background-repeat", "repeat-x");
        AssertUtility(b => b.BgRepeatY(), "bg-repeat-y", "background-repeat", "repeat-y");
        AssertUtility(b => b.BgNoRepeat(), "bg-no-repeat", "background-repeat", "no-repeat");
        AssertUtility(b => b.BgRepeatRound(), "bg-repeat-round", "background-repeat", "round");
        AssertUtility(b => b.BgRepeatSpace(), "bg-repeat-space", "background-repeat", "space");

        AssertUtility(b => b.BgCover(), "bg-cover", "background-size", "cover");
        AssertUtility(b => b.BgContain(), "bg-contain", "background-size", "contain");
        AssertUtility(b => b.BgAuto(), "bg-auto", "background-size", "auto");
        AssertUtility(b => b.BgNone(), "bg-none", "background-image", "none");
    }

    [Fact]
    public void Table_Should_Emit_Expected_Utilities()
    {
        AssertUtility(b => b.BorderCollapse(), "border-collapse", "border-collapse", "collapse");
        AssertUtility(b => b.BorderSeparate(), "border-separate", "border-collapse", "separate");
        AssertUtility(b => b.TableLayoutAuto(), "table-auto", "table-layout", "auto");
        AssertUtility(b => b.TableLayoutFixed(), "table-fixed", "table-layout", "fixed");
        AssertUtility(b => b.CaptionTop(), "caption-top", "caption-side", "top");
        AssertUtility(b => b.CaptionBottom(), "caption-bottom", "caption-side", "bottom");
    }

    [Fact]
    public void Interactivity_Should_Emit_Expected_Utilities()
    {
        AssertUtility(b => b.PointerEventsNone(), "pointer-events-none", "pointer-events", "none");
        AssertUtility(b => b.PointerEventsAuto(), "pointer-events-auto", "pointer-events", "auto");

        AssertUtility(b => b.ResizeNone(), "resize-none", "resize", "none");
        AssertUtility(b => b.ResizeY(), "resize-y", "resize", "vertical");
        AssertUtility(b => b.ResizeX(), "resize-x", "resize", "horizontal");
        AssertUtility(b => b.ResizeBoth(), "resize", "resize", "both");

        AssertUtility(b => b.ScrollBehaviorSmooth(), "scroll-smooth", "scroll-behavior", "smooth");
        AssertUtility(b => b.ScrollBehaviorAuto(), "scroll-auto", "scroll-behavior", "auto");

        AssertUtility(b => b.UserSelectNone(), "select-none", "user-select", "none");
        AssertUtility(b => b.UserSelectText(), "select-text", "user-select", "text");
        AssertUtility(b => b.UserSelectAll(), "select-all", "user-select", "all");
        AssertUtility(b => b.UserSelectAuto(), "select-auto", "user-select", "auto");

        AssertUtility(b => b.AppearanceNone(), "appearance-none", "appearance", "none");
        AssertUtility(b => b.AppearanceAuto(), "appearance-auto", "appearance", "auto");

        AssertUtility(b => b.AccentColor(Color.Blue500), "accent-blue-500", "accent-color", Color.Blue500.Value);
        AssertUtility(b => b.CaretColor(Color.Red500), "caret-red-500", "caret-color", Color.Red500.Value);

        AssertUtility(b => b.ColorSchemeLight(), "scheme-light", "color-scheme", "light");
        AssertUtility(b => b.ColorSchemeDark(), "scheme-dark", "color-scheme", "dark");
        AssertUtility(b => b.ColorSchemeNormal(), "scheme-normal", "color-scheme", "normal");

        AssertUtility(b => b.CursorDefault(), "cursor-default", "cursor", "default");
        AssertUtility(b => b.CursorText(), "cursor-text", "cursor", "text");
        AssertUtility(b => b.CursorWait(), "cursor-wait", "cursor", "wait");
        AssertUtility(b => b.CursorMove(), "cursor-move", "cursor", "move");
        AssertUtility(b => b.CursorHelp(), "cursor-help", "cursor", "help");
        AssertUtility(b => b.CursorNotAllowed(), "cursor-not-allowed", "cursor", "not-allowed");
        AssertUtility(b => b.CursorCrosshair(), "cursor-crosshair", "cursor", "crosshair");
        AssertUtility(b => b.CursorGrab(), "cursor-grab", "cursor", "grab");
        AssertUtility(b => b.CursorGrabbing(), "cursor-grabbing", "cursor", "grabbing");
        AssertUtility(b => b.CursorNone(), "cursor-none", "cursor", "none");
        AssertUtility(b => b.CursorProgress(), "cursor-progress", "cursor", "progress");

        AssertUtility(b => b.TouchActionNone(), "touch-none", "touch-action", "none");
        AssertUtility(b => b.TouchActionAuto(), "touch-auto", "touch-action", "auto");
        AssertUtility(b => b.TouchActionPanX(), "touch-pan-x", "touch-action", "pan-x");
        AssertUtility(b => b.TouchActionPanY(), "touch-pan-y", "touch-action", "pan-y");
        AssertUtility(b => b.TouchActionManipulation(), "touch-manipulation", "touch-action", "manipulation");

        AssertUtility(b => b.WillChangeAuto(), "will-change-auto", "will-change", "auto");
        AssertUtility(b => b.WillChangeScroll(), "will-change-scroll", "will-change", "scroll-position");
        AssertUtility(b => b.WillChangeContents(), "will-change-contents", "will-change", "contents");
        AssertUtility(b => b.WillChangeTransform(), "will-change-transform", "will-change", "transform");
    }

    [Fact]
    public void Svg_Should_Emit_Expected_Utilities()
    {
        AssertUtility(b => b.Fill(Color.Blue500), "fill-blue-500", "fill", Color.Blue500.Value);
        AssertUtility(b => b.Stroke(Color.Red500), "stroke-red-500", "stroke", Color.Red500.Value);
        AssertUtility(b => b.StrokeWidth(2), "stroke-2", "stroke-width", "2px");
    }

    [Fact]
    public void New_Utilities_Should_Compose_With_Pseudo_And_Media_Variants()
    {
        ClassRegistry registry = new();
        var sut = new InternalRootFluentCssBuilder(registry, null, null, null);

        sut.Hover(x => x.BorderT(2));
        sut.Media("min-width: 30rem", "sm", x => x.InlineFlex());

        registry.Select(x => x.ClassName).Should().Equal("hover:border-t-2", "sm:inline-flex");
    }
}
