namespace FluentCss.Core.Tests;

/// <summary>
/// Broad coverage of the typed utility methods on
/// <see cref="BaseStyleBuilder{T}"/>. Each case asserts the generated class name,
/// the CSS property and the CSS value, which is the contract the registry emits.
/// </summary>
public class StyleUtilitiesTests
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
    public void Display_Should_Emit_Expected_Utilities()
    {
        AssertUtility(b => b.Block(), "block", "display", "block");
        AssertUtility(b => b.InlineBlock(), "inline-block", "display", "inline-block");
        AssertUtility(b => b.Inline(), "inline", "display", "inline");
        AssertUtility(b => b.Flex(), "flex", "display", "flex");
        AssertUtility(b => b.Grid(), "grid", "display", "grid");
        AssertUtility(b => b.Hidden(), "hidden", "display", "none");
    }

    [Fact]
    public void Position_Should_Emit_Expected_Utilities()
    {
        AssertUtility(b => b.Relative(), "relative", "position", "relative");
        AssertUtility(b => b.Absolute(), "absolute", "position", "absolute");
        AssertUtility(b => b.Fixed(), "fixed", "position", "fixed");
        AssertUtility(b => b.Sticky(), "sticky", "position", "sticky");
    }

    [Fact]
    public void Offsets_And_ZIndex_Should_Emit_Expected_Utilities()
    {
        AssertUtility(b => b.Inset(2), "inset-2", "inset", "0.5rem");
        AssertUtility(b => b.Top(1), "top-1", "top", "0.25rem");
        AssertUtility(b => b.Bottom(4), "bottom-4", "bottom", "1rem");
        AssertUtility(b => b.Left(0), "left-0", "left", "0px");
        AssertUtility(b => b.Right(3), "right-3", "right", "0.75rem");
        AssertUtility(b => b.Z(10), "z-10", "z-index", "10");
    }

    [Fact]
    public void Sizing_Should_Emit_Expected_Utilities()
    {
        AssertUtility(b => b.W(4), "w-4", "width", "1rem");
        AssertUtility(b => b.WFull(), "w-full", "width", "100%");
        AssertUtility(b => b.WScreen(), "w-screen", "width", "100vw");
        AssertUtility(b => b.WAuto(), "w-auto", "width", "auto");
        AssertUtility(b => b.H(8), "h-8", "height", "2rem");
        AssertUtility(b => b.HFull(), "h-full", "height", "100%");
        AssertUtility(b => b.HScreen(), "h-screen", "height", "100vh");
    }

    [Fact]
    public void Flexbox_Should_Emit_Expected_Utilities()
    {
        AssertUtility(b => b.FlexRow(), "flex-row", "flex-direction", "row");
        AssertUtility(b => b.FlexCol(), "flex-col", "flex-direction", "column");
        AssertUtility(b => b.FlexWrap(), "flex-wrap", "flex-wrap", "wrap");
        AssertUtility(b => b.ItemsCenter(), "items-center", "align-items", "center");
        AssertUtility(b => b.ItemsStart(), "items-start", "align-items", "flex-start");
        AssertUtility(b => b.ItemsEnd(), "items-end", "align-items", "flex-end");
        AssertUtility(b => b.JustifyCenter(), "justify-center", "justify-content", "center");
        AssertUtility(b => b.JustifyBetween(), "justify-between", "justify-content", "space-between");
        AssertUtility(b => b.JustifyStart(), "justify-start", "justify-content", "flex-start");
        AssertUtility(b => b.JustifyEnd(), "justify-end", "justify-content", "flex-end");
        AssertUtility(b => b.Gap(2), "gap-2", "gap", "0.5rem");
        AssertUtility(b => b.GapX(3), "gap-x-3", "column-gap", "0.75rem");
        AssertUtility(b => b.GapY(4), "gap-y-4", "row-gap", "1rem");
        AssertUtility(b => b.Grow(), "grow", "flex-grow", "1");
        AssertUtility(b => b.Shrink(), "shrink", "flex-shrink", "1");
    }

    [Fact]
    public void Grid_Should_Emit_Expected_Utilities()
    {
        AssertUtility(b => b.GridCols(3), "grid-cols-3", "grid-template-columns", "repeat(3, minmax(0, 1fr))");
        AssertUtility(b => b.GridRows(2), "grid-rows-2", "grid-template-rows", "repeat(2, minmax(0, 1fr))");
        AssertUtility(b => b.ColSpan(2), "col-span-2", "grid-column", "span 2 / span 2");
        AssertUtility(b => b.RowSpan(3), "row-span-3", "grid-row", "span 3 / span 3");
        AssertUtility(b => b.ColStart(4), "col-start-4", "grid-column-start", "4");
    }

    [Fact]
    public void Typography_Should_Emit_Expected_Utilities()
    {
        AssertUtility(b => b.TextXs(), "text-xs", "font-size", "0.75rem");
        AssertUtility(b => b.TextSm(), "text-sm", "font-size", "0.875rem");
        AssertUtility(b => b.TextBase(), "text-base", "font-size", "1rem");
        AssertUtility(b => b.TextLg(), "text-lg", "font-size", "1.125rem");
        AssertUtility(b => b.TextXl(), "text-xl", "font-size", "1.25rem");
        AssertUtility(b => b.Text2Xl(), "text-2xl", "font-size", "1.5rem");
        AssertUtility(b => b.Text3Xl(), "text-3xl", "font-size", "1.875rem");

        AssertUtility(b => b.FontThin(), "font-thin", "font-weight", "100");
        AssertUtility(b => b.FontNormal(), "font-normal", "font-weight", "400");
        AssertUtility(b => b.FontBold(), "font-bold", "font-weight", "700");

        AssertUtility(b => b.TextCenter(), "text-center", "text-align", "center");
        AssertUtility(b => b.TextLeft(), "text-left", "text-align", "left");
        AssertUtility(b => b.TextRight(), "text-right", "text-align", "right");

        AssertUtility(b => b.Underline(), "underline", "text-decoration-line", "underline");
        AssertUtility(b => b.NoUnderline(), "no-underline", "text-decoration-line", "none");

        AssertUtility(b => b.Italic(), "italic", "font-style", "italic");
        AssertUtility(b => b.NotItalic(), "not-italic", "font-style", "normal");

        AssertUtility(b => b.Uppercase(), "uppercase", "text-transform", "uppercase");
        AssertUtility(b => b.Lowercase(), "lowercase", "text-transform", "lowercase");
        AssertUtility(b => b.Capitalize(), "capitalize", "text-transform", "capitalize");

        AssertUtility(b => b.WhitespaceNoWrap(), "whitespace-nowrap", "white-space", "nowrap");
        AssertUtility(b => b.Leading(6), "leading-6", "line-height", "1.5rem");
    }

    [Fact]
    public void Borders_Should_Emit_Expected_Utilities()
    {
        AssertUtility(b => b.Border(), "border", "border-width", "1px");
        AssertUtility(b => b.Border(2), "border-2", "border-width", "2px");

        AssertUtility(b => b.Rounded(), "rounded", "border-radius", "0.25rem");
        AssertUtility(b => b.RoundedSm(), "rounded-sm", "border-radius", "0.125rem");
        AssertUtility(b => b.RoundedMd(), "rounded-md", "border-radius", "0.375rem");
        AssertUtility(b => b.RoundedLg(), "rounded-lg", "border-radius", "0.5rem");
        AssertUtility(b => b.RoundedFull(), "rounded-full", "border-radius", "9999px");

        AssertUtility(b => b.BorderSolid(), "border-solid", "border-style", "solid");
        AssertUtility(b => b.BorderDashed(), "border-dashed", "border-style", "dashed");
        AssertUtility(b => b.BorderDotted(), "border-dotted", "border-style", "dotted");

        AssertUtility(b => b.Outline(2), "outline-2", "outline-width", "2px");
        AssertUtility(b => b.OutlineHidden(), "outline-hidden", "outline", "2px solid transparent");
    }

    [Fact]
    public void Effects_Should_Emit_Expected_Utilities()
    {
        AssertUtility(b => b.Shadow(), "shadow", "box-shadow",
            "0 1px 3px 0 rgb(0 0 0 / 0.1), 0 1px 2px -1px rgb(0 0 0 / 0.1)");
        AssertUtility(b => b.ShadowSm(), "shadow-sm", "box-shadow",
            "0 1px 2px 0 rgb(0 0 0 / 0.05)");
        AssertUtility(b => b.ShadowMd(), "shadow-md", "box-shadow",
            "0 4px 6px -1px rgb(0 0 0 / 0.1), 0 2px 4px -2px rgb(0 0 0 / 0.1)");
        AssertUtility(b => b.ShadowLg(), "shadow-lg", "box-shadow",
            "0 10px 15px -3px rgb(0 0 0 / 0.1), 0 4px 6px -4px rgb(0 0 0 / 0.1)");

        AssertUtility(b => b.Opacity(50), "opacity-50", "opacity", "0.5");
        AssertUtility(b => b.Opacity(75), "opacity-75", "opacity", "0.75");
        AssertUtility(b => b.Opacity(100), "opacity-100", "opacity", "1");
        AssertUtility(b => b.Opacity(0), "opacity-0", "opacity", "0");

        AssertUtility(b => b.CursorPointer(), "cursor-pointer", "cursor", "pointer");
        AssertUtility(b => b.Ring(2, Color.Blue500), "ring-2", "box-shadow",
            "0 0 0 2px " + Color.Blue500.Value);
    }

    [Fact]
    public void Misc_Utilities_Should_Emit_Expected_Utilities()
    {
        AssertUtility(b => b.OverflowAuto(), "overflow-auto", "overflow", "auto");
        AssertUtility(b => b.OverflowHidden(), "overflow-hidden", "overflow", "hidden");
        AssertUtility(b => b.OverflowScroll(), "overflow-scroll", "overflow", "scroll");

        AssertUtility(b => b.ObjectCover(), "object-cover", "object-fit", "cover");
        AssertUtility(b => b.ObjectContain(), "object-contain", "object-fit", "contain");

        AssertUtility(b => b.AspectSquare(), "aspect-square", "aspect-ratio", "1 / 1");
        AssertUtility(b => b.AspectVideo(), "aspect-video", "aspect-ratio", "16 / 9");
    }

    [Fact]
    public void Flex_Composite_Should_Emit_All_Requested_Utilities()
    {
        ClassRegistry registry = new();
        var sut = new InternalRootFluentCssBuilder(registry, null, null, null);

        sut.Flex(FlexDirection.Row, Justify.Center, Align.Center, gap: 4);

        registry.Select(x => x.ClassName).Should().Equal(
            "flex", "flex-row", "justify-center", "items-center", "gap-4");
    }

    [Fact]
    public void Flex_Composite_Should_Cover_Reverse_And_Wrap_Variants()
    {
        ClassRegistry registry = new();
        var sut = new InternalRootFluentCssBuilder(registry, null, null, null);

        sut.Flex(FlexDirection.ColReverse, Justify.Between, Align.Baseline, wrap: FlexWrapValue.WrapReverse);

        registry.Select(x => x.ClassName).Should().Equal(
            "flex", "flex-col-reverse", "justify-between", "items-baseline", "flex-wrap-reverse");
    }

    [Fact]
    public void Border_Composite_Should_Emit_Width_Color_And_Style()
    {
        ClassRegistry registry = new();
        var sut = new InternalRootFluentCssBuilder(registry, null, null, null);

        sut.Border(2, Color.Slate300, BorderStyle.Dashed);

        registry.Select(x => x.ClassName).Should().Equal("border-2", "border-slate-300", "border-dashed");
    }

    [Fact]
    public void Border_Composite_With_None_Should_Emit_Border_None()
    {
        ClassRegistry registry = new();
        var sut = new InternalRootFluentCssBuilder(registry, null, null, null);

        sut.Border(1, Color.Black, BorderStyle.None);

        registry.Select(x => x.ClassName).Should().Equal("border-1", "border-black", "border-none");
    }

    [Fact]
    public void Font_Helpers_Should_Emit_The_Family_Utilities()
    {
        AssertUtility(b => b.FontSerif(), "font-serif", "font-family", FontFamily.Serif.Value);
        AssertUtility(b => b.FontMono(), "font-mono", "font-family", FontFamily.Mono.Value);
    }

    [Fact]
    public void ToString_Should_Be_Cached_And_Reflect_All_Classes()
    {
        ClassRegistry registry = new();
        var sut = new InternalRootFluentCssBuilder(registry, null, null, null);

        sut.Background(Color.White).P(4);

        string first = sut.ToString();
        string second = sut.ToString();

        first.Should().Be("bg-white p-4");
        second.Should().BeSameAs(first);
    }
}
