using System.Globalization;

namespace FluentCss.Core;

public abstract class BaseStyleBuilder<T> : IImportantStyleBuilder<T>
{
    private readonly List<string> _classNames = [];
    private string? _cachedToString;

    protected internal string? MediaQuery { get; }
    protected internal string? MediaPrefix { get; }
    protected internal string? Theme { get; }

    private protected BaseStyleBuilder(string? theme, string? mediaQuery, string? mediaPrefix)
    {
        MediaQuery = mediaQuery;
        MediaPrefix = mediaPrefix;
        Theme = theme;
    }

    protected internal abstract void AddClass(UtilityClass c);

    protected abstract T ReturnSelf();

    private T AddInternal(UtilityClass c)
    {
        c = c.AddTheme(Theme).AddMedia(MediaQuery, MediaPrefix);
        AddClass(c);
        _classNames.Add(c.ClassName);
        _cachedToString = null;
        return ReturnSelf();
    }

    protected string GetSpacingValue(int scale)
    {
        if (scale == 0) return "0px";
        return (scale * 0.25d).ToString("0.##", CultureInfo.InvariantCulture) + "rem";
    }

    public T Background(Color color) => AddInternal(new UtilityClass($"bg-{color.Name}", "background-color", color.Value));
    public T Color(Color color) => AddInternal(new UtilityClass($"text-{color.Name}", "color", color.Value));
    public T BorderColor(Color color) => AddInternal(new UtilityClass($"border-{color.Name}", "border-color", color.Value));

    public T P(int scale) => AddInternal(new UtilityClass($"p-{scale}", "padding", GetSpacingValue(scale)));
    public T Px(int scale) => AddInternal(new UtilityClass($"px-{scale}", "padding-inline", GetSpacingValue(scale)));
    public T Py(int scale) => AddInternal(new UtilityClass($"py-{scale}", "padding-block", GetSpacingValue(scale)));
    public T Pt(int scale) => AddInternal(new UtilityClass($"pt-{scale}", "padding-top", GetSpacingValue(scale)));
    public T Pr(int scale) => AddInternal(new UtilityClass($"pr-{scale}", "padding-right", GetSpacingValue(scale)));
    public T Pb(int scale) => AddInternal(new UtilityClass($"pb-{scale}", "padding-bottom", GetSpacingValue(scale)));
    public T Pl(int scale) => AddInternal(new UtilityClass($"pl-{scale}", "padding-left", GetSpacingValue(scale)));

    public T M(int scale) => AddInternal(new UtilityClass($"m-{scale}", "margin", GetSpacingValue(scale)));
    public T Mx(int scale) => AddInternal(new UtilityClass($"mx-{scale}", "margin-inline", GetSpacingValue(scale)));
    public T My(int scale) => AddInternal(new UtilityClass($"my-{scale}", "margin-block", GetSpacingValue(scale)));
    public T Mt(int scale) => AddInternal(new UtilityClass($"mt-{scale}", "margin-top", GetSpacingValue(scale)));
    public T Mr(int scale) => AddInternal(new UtilityClass($"mr-{scale}", "margin-right", GetSpacingValue(scale)));
    public T Mb(int scale) => AddInternal(new UtilityClass($"mb-{scale}", "margin-bottom", GetSpacingValue(scale)));
    public T Ml(int scale) => AddInternal(new UtilityClass($"ml-{scale}", "margin-left", GetSpacingValue(scale)));

    public T MxAuto() => AddInternal(new UtilityClass("mx-auto", "margin-inline", "auto"));
    public T MyAuto() => AddInternal(new UtilityClass("my-auto", "margin-block", "auto"));

    public T Block() => AddInternal(new UtilityClass("block", "display", "block"));
    public T InlineBlock() => AddInternal(new UtilityClass("inline-block", "display", "inline-block"));
    public T Inline() => AddInternal(new UtilityClass("inline", "display", "inline"));
    public T Flex() => AddInternal(new UtilityClass("flex", "display", "flex"));
    public T Grid() => AddInternal(new UtilityClass("grid", "display", "grid"));
    public T Hidden() => AddInternal(new UtilityClass("hidden", "display", "none"));

    public T Relative() => AddInternal(new UtilityClass("relative", "position", "relative"));
    public T Absolute() => AddInternal(new UtilityClass("absolute", "position", "absolute"));
    public T Fixed() => AddInternal(new UtilityClass("fixed", "position", "fixed"));
    public T Sticky() => AddInternal(new UtilityClass("sticky", "position", "sticky"));

    public T Inset(int scale) => AddInternal(new UtilityClass($"inset-{scale}", "inset", GetSpacingValue(scale)));
    public T Top(int scale) => AddInternal(new UtilityClass($"top-{scale}", "top", GetSpacingValue(scale)));
    public T Bottom(int scale) => AddInternal(new UtilityClass($"bottom-{scale}", "bottom", GetSpacingValue(scale)));
    public T Left(int scale) => AddInternal(new UtilityClass($"left-{scale}", "left", GetSpacingValue(scale)));
    public T Right(int scale) => AddInternal(new UtilityClass($"right-{scale}", "right", GetSpacingValue(scale)));

    public T Z(int index) => AddInternal(new UtilityClass($"z-{index}", "z-index", index.ToString()));

    public T W(int scale) => AddInternal(new UtilityClass($"w-{scale}", "width", GetSpacingValue(scale)));
    public T WFull() => AddInternal(new UtilityClass("w-full", "width", "100%"));
    public T WScreen() => AddInternal(new UtilityClass("w-screen", "width", "100vw"));
    public T WAuto() => AddInternal(new UtilityClass("w-auto", "width", "auto"));

    public T H(int scale) => AddInternal(new UtilityClass($"h-{scale}", "height", GetSpacingValue(scale)));
    public T HFull() => AddInternal(new UtilityClass("h-full", "height", "100%"));
    public T HScreen() => AddInternal(new UtilityClass("h-screen", "height", "100vh"));

    public T MaxW(Size size) => AddInternal(new UtilityClass($"max-w-{size.Name}", "max-width", size.Value));
    public T MinH(Size size) => AddInternal(new UtilityClass($"min-h-{size.Name}", "min-height", size.Value));
    public T MinW(Size size) => AddInternal(new UtilityClass($"min-w-{size.Name}", "min-width", size.Value));
    public T MaxH(Size size) => AddInternal(new UtilityClass($"max-h-{size.Name}", "max-height", size.Value));

    public T FlexRow() => AddInternal(new UtilityClass("flex-row", "flex-direction", "row"));
    public T FlexCol() => AddInternal(new UtilityClass("flex-col", "flex-direction", "column"));
    public T FlexWrap() => AddInternal(new UtilityClass("flex-wrap", "flex-wrap", "wrap"));

    public T ItemsCenter() => AddInternal(new UtilityClass("items-center", "align-items", "center"));
    public T ItemsStart() => AddInternal(new UtilityClass("items-start", "align-items", "flex-start"));
    public T ItemsEnd() => AddInternal(new UtilityClass("items-end", "align-items", "flex-end"));

    public T JustifyCenter() => AddInternal(new UtilityClass("justify-center", "justify-content", "center"));
    public T JustifyBetween() => AddInternal(new UtilityClass("justify-between", "justify-content", "space-between"));
    public T JustifyStart() => AddInternal(new UtilityClass("justify-start", "justify-content", "flex-start"));
    public T JustifyEnd() => AddInternal(new UtilityClass("justify-end", "justify-content", "flex-end"));

    public T Gap(int scale) => AddInternal(new UtilityClass($"gap-{scale}", "gap", GetSpacingValue(scale)));
    public T GapX(int scale) => AddInternal(new UtilityClass($"gap-x-{scale}", "column-gap", GetSpacingValue(scale)));
    public T GapY(int scale) => AddInternal(new UtilityClass($"gap-y-{scale}", "row-gap", GetSpacingValue(scale)));

    public T Grow() => AddInternal(new UtilityClass("grow", "flex-grow", "1"));
    public T Shrink() => AddInternal(new UtilityClass("shrink", "flex-shrink", "1"));

    public T TextXs() => AddInternal(new UtilityClass("text-xs", "font-size", "0.75rem"));
    public T TextSm() => AddInternal(new UtilityClass("text-sm", "font-size", "0.875rem"));
    public T TextBase() => AddInternal(new UtilityClass("text-base", "font-size", "1rem"));
    public T TextLg() => AddInternal(new UtilityClass("text-lg", "font-size", "1.125rem"));
    public T TextXl() => AddInternal(new UtilityClass("text-xl", "font-size", "1.25rem"));
    public T Text2Xl() => AddInternal(new UtilityClass("text-2xl", "font-size", "1.5rem"));
    public T Text3Xl() => AddInternal(new UtilityClass("text-3xl", "font-size", "1.875rem"));

    public T FontThin() => AddInternal(new UtilityClass("font-thin", "font-weight", "100"));
    public T FontNormal() => AddInternal(new UtilityClass("font-normal", "font-weight", "400"));
    public T FontBold() => AddInternal(new UtilityClass("font-bold", "font-weight", "700"));

    public T FontSans() => Font(FontFamily.Sans);
    public T FontSerif() => Font(FontFamily.Serif);
    public T FontMono() => Font(FontFamily.Mono);

    public T Font(FontFamily family) =>
        AddInternal(new UtilityClass($"font-{family.Token}", "font-family", family.Value));

    public T Sans(params string[] families) =>
        Font(FontFamily.Prepend("sans", FontFamily.SansStack, families));

    public T Serif(params string[] families) =>
        Font(FontFamily.Prepend("serif", FontFamily.SerifStack, families));

    public T Mono(params string[] families) =>
        Font(FontFamily.Prepend("mono", FontFamily.MonoStack, families));

    public T TextCenter() => AddInternal(new UtilityClass("text-center", "text-align", "center"));
    public T TextLeft() => AddInternal(new UtilityClass("text-left", "text-align", "left"));
    public T TextRight() => AddInternal(new UtilityClass("text-right", "text-align", "right"));

    public T Underline() => AddInternal(new UtilityClass("underline", "text-decoration-line", "underline"));
    public T NoUnderline() => AddInternal(new UtilityClass("no-underline", "text-decoration-line", "none"));

    public T Border(int width) => AddInternal(new UtilityClass($"border-{width}", "border-width", $"{width}px"));
    public T Border() => AddInternal(new UtilityClass("border", "border-width", "1px"));

    public T Rounded() => AddInternal(new UtilityClass("rounded", "border-radius", "0.25rem"));
    public T RoundedSm() => AddInternal(new UtilityClass("rounded-sm", "border-radius", "0.125rem"));
    public T RoundedMd() => AddInternal(new UtilityClass("rounded-md", "border-radius", "0.375rem"));
    public T RoundedLg() => AddInternal(new UtilityClass("rounded-lg", "border-radius", "0.5rem"));
    public T RoundedFull() => AddInternal(new UtilityClass("rounded-full", "border-radius", "9999px"));

    public T Shadow() => AddInternal(new UtilityClass("shadow", "box-shadow", "0 1px 3px 0 rgb(0 0 0 / 0.1), 0 1px 2px -1px rgb(0 0 0 / 0.1)"));
    public T ShadowSm() => AddInternal(new UtilityClass("shadow-sm", "box-shadow", "0 1px 2px 0 rgb(0 0 0 / 0.05)"));
    public T ShadowMd() => AddInternal(new UtilityClass("shadow-md", "box-shadow", "0 4px 6px -1px rgb(0 0 0 / 0.1), 0 2px 4px -2px rgb(0 0 0 / 0.1)"));
    public T ShadowLg() => AddInternal(new UtilityClass("shadow-lg", "box-shadow", "0 10px 15px -3px rgb(0 0 0 / 0.1), 0 4px 6px -4px rgb(0 0 0 / 0.1)"));

    public T Opacity(int val) => AddInternal(new UtilityClass($"opacity-{val}", "opacity", (val / 100.0).ToString("0.##", CultureInfo.InvariantCulture)));
    public T CursorPointer() => AddInternal(new UtilityClass("cursor-pointer", "cursor", "pointer"));

    
    public T GridCols(int n) => AddInternal(new UtilityClass($"grid-cols-{n}", "grid-template-columns", $"repeat({n}, minmax(0, 1fr))"));
    public T GridRows(int n) => AddInternal(new UtilityClass($"grid-rows-{n}", "grid-template-rows", $"repeat({n}, minmax(0, 1fr))"));
    public T ColSpan(int n) => AddInternal(new UtilityClass($"col-span-{n}", "grid-column", $"span {n} / span {n}"));
    public T RowSpan(int n) => AddInternal(new UtilityClass($"row-span-{n}", "grid-row", $"span {n} / span {n}"));
    public T ColStart(int n) => AddInternal(new UtilityClass($"col-start-{n}", "grid-column-start", n.ToString()));

    public T OverflowAuto() => AddInternal(new UtilityClass("overflow-auto", "overflow", "auto"));
    public T OverflowHidden() => AddInternal(new UtilityClass("overflow-hidden", "overflow", "hidden"));
    public T OverflowScroll() => AddInternal(new UtilityClass("overflow-scroll", "overflow", "scroll"));

    public T ObjectCover() => AddInternal(new UtilityClass("object-cover", "object-fit", "cover"));
    public T ObjectContain() => AddInternal(new UtilityClass("object-contain", "object-fit", "contain"));

    public T AspectSquare() => AddInternal(new UtilityClass("aspect-square", "aspect-ratio", "1 / 1"));
    public T AspectVideo() => AddInternal(new UtilityClass("aspect-video", "aspect-ratio", "16 / 9"));

    public T Italic() => AddInternal(new UtilityClass("italic", "font-style", "italic"));
    public T NotItalic() => AddInternal(new UtilityClass("not-italic", "font-style", "normal"));

    public T Uppercase() => AddInternal(new UtilityClass("uppercase", "text-transform", "uppercase"));
    public T Lowercase() => AddInternal(new UtilityClass("lowercase", "text-transform", "lowercase"));
    public T Capitalize() => AddInternal(new UtilityClass("capitalize", "text-transform", "capitalize"));

    public T WhitespaceNoWrap() => AddInternal(new UtilityClass("whitespace-nowrap", "white-space", "nowrap"));
    public T Leading(int scale) => AddInternal(new UtilityClass($"leading-{scale}", "line-height", GetSpacingValue(scale)));

    public T BorderSolid() => AddInternal(new UtilityClass("border-solid", "border-style", "solid"));
    public T BorderDashed() => AddInternal(new UtilityClass("border-dashed", "border-style", "dashed"));
    public T BorderDotted() => AddInternal(new UtilityClass("border-dotted", "border-style", "dotted"));
    public T Ring(int width, Color c) => AddInternal(new UtilityClass($"ring-{width}", "box-shadow", $"0 0 0 {width}px {c.Value}"));
    public T Outline(int width) => AddInternal(new UtilityClass($"outline-{width}", "outline-width", $"{width}px"));
    public T OutlineHidden() => AddInternal(new UtilityClass("outline-hidden", "outline", "2px solid transparent"));
    public T Border(int width, Color c, BorderStyle style = BorderStyle.Solid)
    {
        Border(width);
        BorderColor(c);

        switch (style)
        {
            case BorderStyle.Solid: BorderSolid(); break;
            case BorderStyle.Dashed: BorderDashed(); break;
            case BorderStyle.Dotted: BorderDotted(); break;
            case BorderStyle.None: AddInternal(new UtilityClass("border-none", "border-style", "none")); break;
        }

        return ReturnSelf();
    }

    public T Flex(FlexDirection direction, Justify? justify = null, Align? align = null, int? gap = null, FlexWrapValue? wrap = null)
    {
        Flex();

        switch (direction)
        {
            case FlexDirection.Row: FlexRow(); break;
            case FlexDirection.RowReverse: AddInternal(new UtilityClass("flex-row-reverse", "flex-direction", "row-reverse")); break;
            case FlexDirection.Col: FlexCol(); break;
            case FlexDirection.ColReverse: AddInternal(new UtilityClass("flex-col-reverse", "flex-direction", "column-reverse")); break;
        }

        if (justify.HasValue)
        {
            switch (justify.Value)
            {
                case Justify.Start: JustifyStart(); break;
                case Justify.End: JustifyEnd(); break;
                case Justify.Center: JustifyCenter(); break;
                case Justify.Between: JustifyBetween(); break;
                case Justify.Around: AddInternal(new UtilityClass("justify-around", "justify-content", "space-around")); break;
                case Justify.Evenly: AddInternal(new UtilityClass("justify-evenly", "justify-content", "space-evenly")); break;
            }
        }

        if (align.HasValue)
        {
            switch (align.Value)
            {
                case Align.Start: ItemsStart(); break;
                case Align.End: ItemsEnd(); break;
                case Align.Center: ItemsCenter(); break;
                case Align.Baseline: AddInternal(new UtilityClass("items-baseline", "align-items", "baseline")); break;
                case Align.Stretch: AddInternal(new UtilityClass("items-stretch", "align-items", "stretch")); break;
            }
        }

        if (wrap.HasValue)
        {
            switch (wrap.Value)
            {
                case FlexWrapValue.Wrap: FlexWrap(); break;
                case FlexWrapValue.NoWrap: AddInternal(new UtilityClass("flex-nowrap", "flex-wrap", "nowrap")); break;
                case FlexWrapValue.WrapReverse: AddInternal(new UtilityClass("flex-wrap-reverse", "flex-wrap", "wrap-reverse")); break;
            }
        }

        if (gap.HasValue)
        {
            Gap(gap.Value);
        }

        return ReturnSelf();
    }

    // ---------------------------------------------------------------------
    // Length overloads
    //
    // Each int-scale method has a Length overload so arbitrary values and units
    // can be used, and so named Length values work (for example
    // P(Spacing.Medium)). The class name comes from Length.Token.
    // ---------------------------------------------------------------------

    protected T LengthUtility(string classPrefix, string property, Length length) =>
        AddInternal(new UtilityClass($"{classPrefix}-{length.Token}", property, length.Value));

    public T P(Length length) => LengthUtility("p", "padding", length);
    public T Px(Length length) => LengthUtility("px", "padding-inline", length);
    public T Py(Length length) => LengthUtility("py", "padding-block", length);
    public T Pt(Length length) => LengthUtility("pt", "padding-top", length);
    public T Pr(Length length) => LengthUtility("pr", "padding-right", length);
    public T Pb(Length length) => LengthUtility("pb", "padding-bottom", length);
    public T Pl(Length length) => LengthUtility("pl", "padding-left", length);

    public T M(Length length) => LengthUtility("m", "margin", length);
    public T Mx(Length length) => LengthUtility("mx", "margin-inline", length);
    public T My(Length length) => LengthUtility("my", "margin-block", length);
    public T Mt(Length length) => LengthUtility("mt", "margin-top", length);
    public T Mr(Length length) => LengthUtility("mr", "margin-right", length);
    public T Mb(Length length) => LengthUtility("mb", "margin-bottom", length);
    public T Ml(Length length) => LengthUtility("ml", "margin-left", length);

    public T Inset(Length length) => LengthUtility("inset", "inset", length);
    public T Top(Length length) => LengthUtility("top", "top", length);
    public T Bottom(Length length) => LengthUtility("bottom", "bottom", length);
    public T Left(Length length) => LengthUtility("left", "left", length);
    public T Right(Length length) => LengthUtility("right", "right", length);

    public T W(Length length) => LengthUtility("w", "width", length);
    public T H(Length length) => LengthUtility("h", "height", length);

    public T MaxW(Length length) => LengthUtility("max-w", "max-width", length);
    public T MinW(Length length) => LengthUtility("min-w", "min-width", length);
    public T MaxH(Length length) => LengthUtility("max-h", "max-height", length);
    public T MinH(Length length) => LengthUtility("min-h", "min-height", length);

    public T Gap(Length length) => LengthUtility("gap", "gap", length);
    public T GapX(Length length) => LengthUtility("gap-x", "column-gap", length);
    public T GapY(Length length) => LengthUtility("gap-y", "row-gap", length);

    public T Leading(Length length) => LengthUtility("leading", "line-height", length);

    public T Rounded(Length radius) => LengthUtility("rounded", "border-radius", radius);
    public T Text(Length size) => LengthUtility("text", "font-size", size);

    public T InlineFlex() => AddInternal(new UtilityClass("inline-flex", "display", "inline-flex"));
    public T InlineGrid() => AddInternal(new UtilityClass("inline-grid", "display", "inline-grid"));
    public T FlowRoot() => AddInternal(new UtilityClass("flow-root", "display", "flow-root"));
    public T Contents() => AddInternal(new UtilityClass("contents", "display", "contents"));
    public T ListItem() => AddInternal(new UtilityClass("list-item", "display", "list-item"));

    public T Static() => AddInternal(new UtilityClass("static", "position", "static"));

    public T ZAuto() => AddInternal(new UtilityClass("z-auto", "z-index", "auto"));

    public T FloatLeft() => AddInternal(new UtilityClass("float-left", "float", "left"));
    public T FloatRight() => AddInternal(new UtilityClass("float-right", "float", "right"));
    public T FloatNone() => AddInternal(new UtilityClass("float-none", "float", "none"));

    public T ClearLeft() => AddInternal(new UtilityClass("clear-left", "clear", "left"));
    public T ClearRight() => AddInternal(new UtilityClass("clear-right", "clear", "right"));
    public T ClearBoth() => AddInternal(new UtilityClass("clear-both", "clear", "both"));
    public T ClearNone() => AddInternal(new UtilityClass("clear-none", "clear", "none"));

    public T Isolate() => AddInternal(new UtilityClass("isolate", "isolation", "isolate"));
    public T IsolationAuto() => AddInternal(new UtilityClass("isolation-auto", "isolation", "auto"));

    public T BoxBorder() => AddInternal(new UtilityClass("box-border", "box-sizing", "border-box"));
    public T BoxContent() => AddInternal(new UtilityClass("box-content", "box-sizing", "content-box"));

    public T Visible() => AddInternal(new UtilityClass("visible", "visibility", "visible"));
    public T Invisible() => AddInternal(new UtilityClass("invisible", "visibility", "hidden"));

    public T ObjectFill() => AddInternal(new UtilityClass("object-fill", "object-fit", "fill"));
    public T ObjectScaleDown() => AddInternal(new UtilityClass("object-scale-down", "object-fit", "scale-down"));
    public T ObjectNone() => AddInternal(new UtilityClass("object-none", "object-fit", "none"));

    public T Basis(int scale) => AddInternal(new UtilityClass($"basis-{scale}", "flex-basis", GetSpacingValue(scale)));
    public T Basis(Length length) => LengthUtility("basis", "flex-basis", length);
    public T BasisAuto() => AddInternal(new UtilityClass("basis-auto", "flex-basis", "auto"));
    public T BasisFull() => AddInternal(new UtilityClass("basis-full", "flex-basis", "100%"));

    public T Order(int order) => AddInternal(new UtilityClass($"order-{order}", "order", order.ToString(CultureInfo.InvariantCulture)));
    public T OrderFirst() => AddInternal(new UtilityClass("order-first", "order", "-9999"));
    public T OrderLast() => AddInternal(new UtilityClass("order-last", "order", "9999"));
    public T OrderNone() => AddInternal(new UtilityClass("order-none", "order", "0"));

    public T Grow(int value) => AddInternal(new UtilityClass($"grow-{value}", "flex-grow", value.ToString(CultureInfo.InvariantCulture)));
    public T Shrink(int value) => AddInternal(new UtilityClass($"shrink-{value}", "flex-shrink", value.ToString(CultureInfo.InvariantCulture)));

    public T GridAutoFlowRow() => AddInternal(new UtilityClass("grid-flow-row", "grid-auto-flow", "row"));
    public T GridAutoFlowColumn() => AddInternal(new UtilityClass("grid-flow-col", "grid-auto-flow", "column"));
    public T GridAutoFlowRowDense() => AddInternal(new UtilityClass("grid-flow-row-dense", "grid-auto-flow", "row dense"));
    public T GridAutoFlowColumnDense() => AddInternal(new UtilityClass("grid-flow-col-dense", "grid-auto-flow", "column dense"));

    public T ColEnd(int n) => AddInternal(new UtilityClass($"col-end-{n}", "grid-column-end", n.ToString(CultureInfo.InvariantCulture)));
    public T RowStart(int n) => AddInternal(new UtilityClass($"row-start-{n}", "grid-row-start", n.ToString(CultureInfo.InvariantCulture)));
    public T RowEnd(int n) => AddInternal(new UtilityClass($"row-end-{n}", "grid-row-end", n.ToString(CultureInfo.InvariantCulture)));

    public T JustifyItemsStart() => AddInternal(new UtilityClass("justify-items-start", "justify-items", "flex-start"));
    public T JustifyItemsCenter() => AddInternal(new UtilityClass("justify-items-center", "justify-items", "center"));
    public T JustifyItemsEnd() => AddInternal(new UtilityClass("justify-items-end", "justify-items", "flex-end"));
    public T JustifyItemsStretch() => AddInternal(new UtilityClass("justify-items-stretch", "justify-items", "stretch"));

    public T JustifySelfAuto() => AddInternal(new UtilityClass("justify-self-auto", "justify-self", "auto"));
    public T JustifySelfStart() => AddInternal(new UtilityClass("justify-self-start", "justify-self", "flex-start"));
    public T JustifySelfCenter() => AddInternal(new UtilityClass("justify-self-center", "justify-self", "center"));
    public T JustifySelfEnd() => AddInternal(new UtilityClass("justify-self-end", "justify-self", "flex-end"));
    public T JustifySelfStretch() => AddInternal(new UtilityClass("justify-self-stretch", "justify-self", "stretch"));

    public T AlignContentStart() => AddInternal(new UtilityClass("content-start", "align-content", "flex-start"));
    public T AlignContentCenter() => AddInternal(new UtilityClass("content-center", "align-content", "center"));
    public T AlignContentEnd() => AddInternal(new UtilityClass("content-end", "align-content", "flex-end"));
    public T AlignContentBetween() => AddInternal(new UtilityClass("content-between", "align-content", "space-between"));
    public T AlignContentAround() => AddInternal(new UtilityClass("content-around", "align-content", "space-around"));
    public T AlignContentEvenly() => AddInternal(new UtilityClass("content-evenly", "align-content", "space-evenly"));
    public T AlignContentBaseline() => AddInternal(new UtilityClass("content-baseline", "align-content", "baseline"));
    public T AlignContentStretch() => AddInternal(new UtilityClass("content-stretch", "align-content", "stretch"));

    public T AlignSelfAuto() => AddInternal(new UtilityClass("self-auto", "align-self", "auto"));
    public T AlignSelfStart() => AddInternal(new UtilityClass("self-start", "align-self", "flex-start"));
    public T AlignSelfCenter() => AddInternal(new UtilityClass("self-center", "align-self", "center"));
    public T AlignSelfEnd() => AddInternal(new UtilityClass("self-end", "align-self", "flex-end"));
    public T AlignSelfBaseline() => AddInternal(new UtilityClass("self-baseline", "align-self", "baseline"));
    public T AlignSelfStretch() => AddInternal(new UtilityClass("self-stretch", "align-self", "stretch"));

    public T JustifyAround() => AddInternal(new UtilityClass("justify-around", "justify-content", "space-around"));
    public T JustifyEvenly() => AddInternal(new UtilityClass("justify-evenly", "justify-content", "space-evenly"));
    public T ItemsBaseline() => AddInternal(new UtilityClass("items-baseline", "align-items", "baseline"));
    public T ItemsStretch() => AddInternal(new UtilityClass("items-stretch", "align-items", "stretch"));

    public T BorderT() => AddInternal(new UtilityClass("border-t", "border-top-width", "1px"));
    public T BorderT(int width) => AddInternal(new UtilityClass($"border-t-{width}", "border-top-width", $"{width}px"));
    public T BorderR() => AddInternal(new UtilityClass("border-r", "border-right-width", "1px"));
    public T BorderR(int width) => AddInternal(new UtilityClass($"border-r-{width}", "border-right-width", $"{width}px"));
    public T BorderB() => AddInternal(new UtilityClass("border-b", "border-bottom-width", "1px"));
    public T BorderB(int width) => AddInternal(new UtilityClass($"border-b-{width}", "border-bottom-width", $"{width}px"));
    public T BorderL() => AddInternal(new UtilityClass("border-l", "border-left-width", "1px"));
    public T BorderL(int width) => AddInternal(new UtilityClass($"border-l-{width}", "border-left-width", $"{width}px"));
    public T BorderX() => AddInternal(new UtilityClass("border-x", "border-inline-width", "1px"));
    public T BorderX(int width) => AddInternal(new UtilityClass($"border-x-{width}", "border-inline-width", $"{width}px"));
    public T BorderY() => AddInternal(new UtilityClass("border-y", "border-block-width", "1px"));
    public T BorderY(int width) => AddInternal(new UtilityClass($"border-y-{width}", "border-block-width", $"{width}px"));

    public T BorderDouble() => AddInternal(new UtilityClass("border-double", "border-style", "double"));

    public T OutlineColor(Color color) => AddInternal(new UtilityClass($"outline-{color.Name}", "outline-color", color.Value));
    public T OutlineOffset(int offset) => AddInternal(new UtilityClass($"outline-offset-{offset}", "outline-offset", $"{offset}px"));

    public T FontExtralight() => AddInternal(new UtilityClass("font-extralight", "font-weight", "200"));
    public T FontLight() => AddInternal(new UtilityClass("font-light", "font-weight", "300"));
    public T FontMedium() => AddInternal(new UtilityClass("font-medium", "font-weight", "500"));
    public T FontSemibold() => AddInternal(new UtilityClass("font-semibold", "font-weight", "600"));
    public T FontExtrabold() => AddInternal(new UtilityClass("font-extrabold", "font-weight", "800"));
    public T FontBlack() => AddInternal(new UtilityClass("font-black", "font-weight", "900"));

    public T LeadingNone() => AddInternal(new UtilityClass("leading-none", "line-height", "1"));
    public T LeadingTight() => AddInternal(new UtilityClass("leading-tight", "line-height", "1.25"));
    public T LeadingSnug() => AddInternal(new UtilityClass("leading-snug", "line-height", "1.375"));
    public T LeadingNormal() => AddInternal(new UtilityClass("leading-normal", "line-height", "1.5"));
    public T LeadingRelaxed() => AddInternal(new UtilityClass("leading-relaxed", "line-height", "1.625"));
    public T LeadingLoose() => AddInternal(new UtilityClass("leading-loose", "line-height", "2"));

    public T TrackingTighter() => AddInternal(new UtilityClass("tracking-tighter", "letter-spacing", "-0.05em"));
    public T TrackingTight() => AddInternal(new UtilityClass("tracking-tight", "letter-spacing", "-0.025em"));
    public T TrackingNormal() => AddInternal(new UtilityClass("tracking-normal", "letter-spacing", "0em"));
    public T TrackingWide() => AddInternal(new UtilityClass("tracking-wide", "letter-spacing", "0.025em"));
    public T TrackingWider() => AddInternal(new UtilityClass("tracking-wider", "letter-spacing", "0.05em"));
    public T TrackingWidest() => AddInternal(new UtilityClass("tracking-widest", "letter-spacing", "0.1em"));
    public T Tracking(Length length) => LengthUtility("tracking", "letter-spacing", length);

    public T WhitespaceNormal() => AddInternal(new UtilityClass("whitespace-normal", "white-space", "normal"));
    public T WhitespacePre() => AddInternal(new UtilityClass("whitespace-pre", "white-space", "pre"));
    public T WhitespacePreLine() => AddInternal(new UtilityClass("whitespace-pre-line", "white-space", "pre-line"));
    public T WhitespacePreWrap() => AddInternal(new UtilityClass("whitespace-pre-wrap", "white-space", "pre-wrap"));
    public T WhitespaceBreakSpaces() => AddInternal(new UtilityClass("whitespace-break-spaces", "white-space", "break-spaces"));

    public T LineThrough() => AddInternal(new UtilityClass("line-through", "text-decoration-line", "line-through"));
    public T Overline() => AddInternal(new UtilityClass("overline", "text-decoration-line", "overline"));

    public T Truncate()
    {
        OverflowHidden();
        TextEllipsis();
        WhitespaceNoWrap();
        return ReturnSelf();
    }

    public T TextEllipsis() => AddInternal(new UtilityClass("text-ellipsis", "text-overflow", "ellipsis"));
    public T TextClip() => AddInternal(new UtilityClass("text-clip", "text-overflow", "clip"));

    public T TextIndent(int scale) => AddInternal(new UtilityClass($"indent-{scale}", "text-indent", GetSpacingValue(scale)));
    public T TextIndent(Length length) => LengthUtility("indent", "text-indent", length);

    public T WordBreakNormal() => AddInternal(new UtilityClass("break-normal", "word-break", "normal"));
    public T WordBreakAll() => AddInternal(new UtilityClass("break-all", "word-break", "break-all"));
    public T WordBreakKeepAll() => AddInternal(new UtilityClass("break-keep", "word-break", "keep-all"));

    public T OverflowWrapNormal() => AddInternal(new UtilityClass("wrap-normal", "overflow-wrap", "normal"));
    public T OverflowWrapBreakWord() => AddInternal(new UtilityClass("wrap-break-word", "overflow-wrap", "break-word"));
    public T OverflowWrapAnywhere() => AddInternal(new UtilityClass("wrap-anywhere", "overflow-wrap", "anywhere"));

    public T HyphensNone() => AddInternal(new UtilityClass("hyphens-none", "hyphens", "none"));
    public T HyphensManual() => AddInternal(new UtilityClass("hyphens-manual", "hyphens", "manual"));
    public T HyphensAuto() => AddInternal(new UtilityClass("hyphens-auto", "hyphens", "auto"));

    public T ListStyleNone() => AddInternal(new UtilityClass("list-none", "list-style-type", "none"));
    public T ListDisc() => AddInternal(new UtilityClass("list-disc", "list-style-type", "disc"));
    public T ListDecimal() => AddInternal(new UtilityClass("list-decimal", "list-style-type", "decimal"));
    public T ListInside() => AddInternal(new UtilityClass("list-inside", "list-style-position", "inside"));
    public T ListOutside() => AddInternal(new UtilityClass("list-outside", "list-style-position", "outside"));

    public T BgAttachmentFixed() => AddInternal(new UtilityClass("bg-fixed", "background-attachment", "fixed"));
    public T BgAttachmentLocal() => AddInternal(new UtilityClass("bg-local", "background-attachment", "local"));
    public T BgAttachmentScroll() => AddInternal(new UtilityClass("bg-scroll", "background-attachment", "scroll"));

    public T BgClipBorder() => AddInternal(new UtilityClass("bg-clip-border", "background-clip", "border-box"));
    public T BgClipPadding() => AddInternal(new UtilityClass("bg-clip-padding", "background-clip", "padding-box"));
    public T BgClipContent() => AddInternal(new UtilityClass("bg-clip-content", "background-clip", "content-box"));
    public T BgClipText() => AddInternal(new UtilityClass("bg-clip-text", "background-clip", "text"));

    public T BgOriginBorder() => AddInternal(new UtilityClass("bg-origin-border", "background-origin", "border-box"));
    public T BgOriginPadding() => AddInternal(new UtilityClass("bg-origin-padding", "background-origin", "padding-box"));
    public T BgOriginContent() => AddInternal(new UtilityClass("bg-origin-content", "background-origin", "content-box"));

    public T BgRepeat() => AddInternal(new UtilityClass("bg-repeat", "background-repeat", "repeat"));
    public T BgRepeatX() => AddInternal(new UtilityClass("bg-repeat-x", "background-repeat", "repeat-x"));
    public T BgRepeatY() => AddInternal(new UtilityClass("bg-repeat-y", "background-repeat", "repeat-y"));
    public T BgNoRepeat() => AddInternal(new UtilityClass("bg-no-repeat", "background-repeat", "no-repeat"));
    public T BgRepeatRound() => AddInternal(new UtilityClass("bg-repeat-round", "background-repeat", "round"));
    public T BgRepeatSpace() => AddInternal(new UtilityClass("bg-repeat-space", "background-repeat", "space"));

    public T BgCover() => AddInternal(new UtilityClass("bg-cover", "background-size", "cover"));
    public T BgContain() => AddInternal(new UtilityClass("bg-contain", "background-size", "contain"));
    public T BgAuto() => AddInternal(new UtilityClass("bg-auto", "background-size", "auto"));
    public T BgNone() => AddInternal(new UtilityClass("bg-none", "background-image", "none"));

    public T BorderCollapse() => AddInternal(new UtilityClass("border-collapse", "border-collapse", "collapse"));
    public T BorderSeparate() => AddInternal(new UtilityClass("border-separate", "border-collapse", "separate"));
    public T TableLayoutAuto() => AddInternal(new UtilityClass("table-auto", "table-layout", "auto"));
    public T TableLayoutFixed() => AddInternal(new UtilityClass("table-fixed", "table-layout", "fixed"));
    public T CaptionTop() => AddInternal(new UtilityClass("caption-top", "caption-side", "top"));
    public T CaptionBottom() => AddInternal(new UtilityClass("caption-bottom", "caption-side", "bottom"));

    public T PointerEventsNone() => AddInternal(new UtilityClass("pointer-events-none", "pointer-events", "none"));
    public T PointerEventsAuto() => AddInternal(new UtilityClass("pointer-events-auto", "pointer-events", "auto"));

    public T ResizeNone() => AddInternal(new UtilityClass("resize-none", "resize", "none"));
    public T ResizeY() => AddInternal(new UtilityClass("resize-y", "resize", "vertical"));
    public T ResizeX() => AddInternal(new UtilityClass("resize-x", "resize", "horizontal"));
    public T ResizeBoth() => AddInternal(new UtilityClass("resize", "resize", "both"));

    public T ScrollBehaviorSmooth() => AddInternal(new UtilityClass("scroll-smooth", "scroll-behavior", "smooth"));
    public T ScrollBehaviorAuto() => AddInternal(new UtilityClass("scroll-auto", "scroll-behavior", "auto"));

    public T UserSelectNone() => AddInternal(new UtilityClass("select-none", "user-select", "none"));
    public T UserSelectText() => AddInternal(new UtilityClass("select-text", "user-select", "text"));
    public T UserSelectAll() => AddInternal(new UtilityClass("select-all", "user-select", "all"));
    public T UserSelectAuto() => AddInternal(new UtilityClass("select-auto", "user-select", "auto"));

    public T AppearanceNone() => AddInternal(new UtilityClass("appearance-none", "appearance", "none"));
    public T AppearanceAuto() => AddInternal(new UtilityClass("appearance-auto", "appearance", "auto"));

    public T AccentColor(Color color) => AddInternal(new UtilityClass($"accent-{color.Name}", "accent-color", color.Value));
    public T CaretColor(Color color) => AddInternal(new UtilityClass($"caret-{color.Name}", "caret-color", color.Value));

    public T ColorSchemeLight() => AddInternal(new UtilityClass("scheme-light", "color-scheme", "light"));
    public T ColorSchemeDark() => AddInternal(new UtilityClass("scheme-dark", "color-scheme", "dark"));
    public T ColorSchemeNormal() => AddInternal(new UtilityClass("scheme-normal", "color-scheme", "normal"));

    public T CursorDefault() => AddInternal(new UtilityClass("cursor-default", "cursor", "default"));
    public T CursorText() => AddInternal(new UtilityClass("cursor-text", "cursor", "text"));
    public T CursorWait() => AddInternal(new UtilityClass("cursor-wait", "cursor", "wait"));
    public T CursorMove() => AddInternal(new UtilityClass("cursor-move", "cursor", "move"));
    public T CursorHelp() => AddInternal(new UtilityClass("cursor-help", "cursor", "help"));
    public T CursorNotAllowed() => AddInternal(new UtilityClass("cursor-not-allowed", "cursor", "not-allowed"));
    public T CursorCrosshair() => AddInternal(new UtilityClass("cursor-crosshair", "cursor", "crosshair"));
    public T CursorGrab() => AddInternal(new UtilityClass("cursor-grab", "cursor", "grab"));
    public T CursorGrabbing() => AddInternal(new UtilityClass("cursor-grabbing", "cursor", "grabbing"));
    public T CursorNone() => AddInternal(new UtilityClass("cursor-none", "cursor", "none"));
    public T CursorProgress() => AddInternal(new UtilityClass("cursor-progress", "cursor", "progress"));

    public T TouchActionNone() => AddInternal(new UtilityClass("touch-none", "touch-action", "none"));
    public T TouchActionAuto() => AddInternal(new UtilityClass("touch-auto", "touch-action", "auto"));
    public T TouchActionPanX() => AddInternal(new UtilityClass("touch-pan-x", "touch-action", "pan-x"));
    public T TouchActionPanY() => AddInternal(new UtilityClass("touch-pan-y", "touch-action", "pan-y"));
    public T TouchActionManipulation() => AddInternal(new UtilityClass("touch-manipulation", "touch-action", "manipulation"));

    public T WillChangeAuto() => AddInternal(new UtilityClass("will-change-auto", "will-change", "auto"));
    public T WillChangeScroll() => AddInternal(new UtilityClass("will-change-scroll", "will-change", "scroll-position"));
    public T WillChangeContents() => AddInternal(new UtilityClass("will-change-contents", "will-change", "contents"));
    public T WillChangeTransform() => AddInternal(new UtilityClass("will-change-transform", "will-change", "transform"));

    public T Fill(Color color) => AddInternal(new UtilityClass($"fill-{color.Name}", "fill", color.Value));
    public T Stroke(Color color) => AddInternal(new UtilityClass($"stroke-{color.Name}", "stroke", color.Value));
    public T StrokeWidth(int width) => AddInternal(new UtilityClass($"stroke-{width}", "stroke-width", $"{width}px"));

    public T Arbitrary(string className, string property, string value) =>
        AddInternal(new UtilityClass(className, property, value));

    public override string ToString()
    {
        if (_cachedToString != null) return _cachedToString;
        _cachedToString = string.Join(" ", _classNames);
        return _cachedToString;
    }

    /// <summary>
    /// Creates the child builder used as the scope of an
    /// <see cref="IImportantStyleBuilder{T}.Important"/> block. Implemented by the
    /// concrete builders because they own the registry the scope writes to.
    /// </summary>
    protected abstract IStyleScopeBuilder CreateImportantScope();

    /// <summary>
    /// Adds the class names collected by a child scope to this builder. Overridden
    /// by builders that keep their own class-name list.
    /// </summary>
    protected virtual void AddChildClassNames(string classNames) => AddClassName(classNames);

    public T Important(Action<IStyleScopeBuilder> styles)
    {
        if (styles is null)
        {
            throw new ArgumentNullException(nameof(styles));
        }

        IStyleScopeBuilder scope = CreateImportantScope();
        styles(scope);

        string classNames = scope.ToString() ?? string.Empty;

        if (classNames.Length > 0)
        {
            AddChildClassNames(classNames);
        }

        return ReturnSelf();
    }

    protected void AddClassName(string className)
    {
        _classNames.Add(className);
        _cachedToString = null;
    }
}