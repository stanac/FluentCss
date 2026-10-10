using System.Globalization;

namespace FluentCss.Core;

public abstract class BaseStyleBuilder<T> : IStyleBuilder<T>
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

    public override string ToString()
    {
        if (_cachedToString != null) return _cachedToString;
        _cachedToString = string.Join(" ", _classNames);
        return _cachedToString;
    }

    protected void AddClassName(string className)
    {
        _classNames.Add(className);
        _cachedToString = null;
    }
}