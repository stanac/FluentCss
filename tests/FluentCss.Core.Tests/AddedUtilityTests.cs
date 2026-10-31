namespace FluentCss.Core.Tests;

/// <summary>
/// Coverage for the utilities and pseudo-element support added after the
/// <c>missing-features.md</c> gaps were triaged. Each case asserts the generated
/// class name, the CSS property and the CSS value.
/// </summary>
public class AddedUtilityTests
{
    private static UtilityClass Single(Action<InternalRootFluentCssBuilder> apply)
    {
        ClassRegistry registry = new();
        var sut = new InternalRootFluentCssBuilder(registry, null, null, null);

        apply(sut);

        return registry.Single();
    }

    private static void AssertUtility(
        Action<InternalRootFluentCssBuilder> apply,
        string className,
        string property,
        string value)
    {
        var added = Single(apply);
        added.ClassName.Should().Be(className);
        added.Property.Should().Be(property);
        added.Value.Should().Be(value);
    }

    [Fact]
    public void Sizing_Keywords_Should_Emit_Expected_Utilities()
    {
        AssertUtility(b => b.WFit(), "w-fit", "width", "fit-content");
        AssertUtility(b => b.HFit(), "h-fit", "height", "fit-content");
        AssertUtility(b => b.WMin(), "w-min", "width", "min-content");
        AssertUtility(b => b.WMax(), "w-max", "width", "max-content");
        AssertUtility(b => b.HMin(), "h-min", "height", "min-content");
        AssertUtility(b => b.HMax(), "h-max", "height", "max-content");
    }

    [Fact]
    public void Flex_Shorthands_Should_Emit_Expected_Utilities()
    {
        AssertUtility(b => b.Flex1(), "flex-1", "flex", "1 1 0%");
        AssertUtility(b => b.FlexAuto(), "flex-auto", "flex", "1 1 auto");
        AssertUtility(b => b.FlexInitial(), "flex-initial", "flex", "0 1 auto");
        AssertUtility(b => b.FlexNone(), "flex-none", "flex", "none");
    }

    [Fact]
    public void SpaceX_And_SpaceY_Should_Target_Siblings()
    {
        var spaceX = Single(b => b.SpaceX(4));
        spaceX.ClassName.Should().Be("space-x-4");
        spaceX.Property.Should().Be("margin-inline-start");
        spaceX.Value.Should().Be("1rem");
        spaceX.CssString.Should().Be(".space-x-4 > :not([hidden]) ~ :not([hidden]) { margin-inline-start: 1rem } ");

        AssertUtility(b => b.SpaceY(Length.Rem(1.5)), "space-y-1.5rem", "margin-block-start", "1.5rem");
    }

    [Fact]
    public void Divide_Should_Target_Siblings()
    {
        var divide = Single(b => b.DivideX(2));
        divide.ClassName.Should().Be("divide-x-2");
        divide.Property.Should().Be("border-inline-start-width");
        divide.Value.Should().Be("2px");
        divide.CssString.Should().Contain(".divide-x-2 > :not([hidden]) ~ :not([hidden])");

        AssertUtility(b => b.DivideY(), "divide-y-1", "border-block-start-width", "1px");
        AssertUtility(b => b.DivideColor(Color.Red500), "divide-red-500", "border-color", Color.Red500.Value);
    }

    [Fact]
    public void Vertical_Align_And_TextEnd_Should_Emit_Expected_Utilities()
    {
        AssertUtility(b => b.AlignBaseline(), "align-baseline", "vertical-align", "baseline");
        AssertUtility(b => b.AlignTop(), "align-top", "vertical-align", "top");
        AssertUtility(b => b.AlignMiddle(), "align-middle", "vertical-align", "middle");
        AssertUtility(b => b.AlignBottom(), "align-bottom", "vertical-align", "bottom");
        AssertUtility(b => b.AlignTextTop(), "align-text-top", "vertical-align", "text-top");
        AssertUtility(b => b.AlignTextBottom(), "align-text-bottom", "vertical-align", "text-bottom");
        AssertUtility(b => b.AlignSub(), "align-sub", "vertical-align", "sub");
        AssertUtility(b => b.AlignSuper(), "align-super", "vertical-align", "super");
        AssertUtility(b => b.TextEnd(), "text-end", "text-align", "end");
    }

    [Fact]
    public void Text_Decoration_Should_Emit_Expected_Utilities()
    {
        AssertUtility(b => b.DecorationColor(Color.Indigo500), "decoration-indigo-500", "text-decoration-color", Color.Indigo500.Value);
        AssertUtility(b => b.Decoration(2), "decoration-2", "text-decoration-thickness", "2px");
        AssertUtility(b => b.DecorationSolid(), "decoration-solid", "text-decoration-style", "solid");
        AssertUtility(b => b.DecorationDouble(), "decoration-double", "text-decoration-style", "double");
        AssertUtility(b => b.DecorationDotted(), "decoration-dotted", "text-decoration-style", "dotted");
        AssertUtility(b => b.DecorationDashed(), "decoration-dashed", "text-decoration-style", "dashed");
        AssertUtility(b => b.DecorationWavy(), "decoration-wavy", "text-decoration-style", "wavy");
        AssertUtility(b => b.UnderlineOffset(4), "underline-offset-4", "text-underline-offset", "4px");
    }

    [Fact]
    public void Transitions_Should_Emit_Expected_Utilities()
    {
        var transition = Single(b => b.Transition());
        transition.ClassName.Should().Be("transition");
        transition.Property.Should().Be("transition");
        transition.Value.Should().Contain("opacity 150ms cubic-bezier(0.4, 0, 0.2, 1)");
        transition.Value.Should().Contain("background-color 150ms cubic-bezier(0.4, 0, 0.2, 1)");

        AssertUtility(b => b.TransitionNone(), "transition-none", "transition-property", "none");
        AssertUtility(b => b.TransitionAll(), "transition-all", "transition", "all 150ms cubic-bezier(0.4, 0, 0.2, 1)");
        AssertUtility(b => b.TransitionColors(), "transition-colors", "transition", "color 150ms cubic-bezier(0.4, 0, 0.2, 1), background-color 150ms cubic-bezier(0.4, 0, 0.2, 1), border-color 150ms cubic-bezier(0.4, 0, 0.2, 1), text-decoration-color 150ms cubic-bezier(0.4, 0, 0.2, 1), fill 150ms cubic-bezier(0.4, 0, 0.2, 1), stroke 150ms cubic-bezier(0.4, 0, 0.2, 1)");
        AssertUtility(b => b.TransitionOpacity(), "transition-opacity", "transition", "opacity 150ms cubic-bezier(0.4, 0, 0.2, 1)");
        AssertUtility(b => b.TransitionShadow(), "transition-shadow", "transition", "box-shadow 150ms cubic-bezier(0.4, 0, 0.2, 1)");
        AssertUtility(b => b.TransitionTransform(), "transition-transform", "transition", "transform 150ms cubic-bezier(0.4, 0, 0.2, 1), translate 150ms cubic-bezier(0.4, 0, 0.2, 1), scale 150ms cubic-bezier(0.4, 0, 0.2, 1), rotate 150ms cubic-bezier(0.4, 0, 0.2, 1)");
        AssertUtility(b => b.Duration(300), "duration-300", "transition-duration", "300ms");
        AssertUtility(b => b.Delay(75), "delay-75", "transition-delay", "75ms");
        AssertUtility(b => b.EaseLinear(), "ease-linear", "transition-timing-function", "linear");
        AssertUtility(b => b.EaseIn(), "ease-in", "transition-timing-function", "cubic-bezier(0.4, 0, 1, 1)");
        AssertUtility(b => b.EaseOut(), "ease-out", "transition-timing-function", "cubic-bezier(0, 0, 0.2, 1)");
        AssertUtility(b => b.EaseInOut(), "ease-in-out", "transition-timing-function", "cubic-bezier(0.4, 0, 0.2, 1)");
    }

    [Fact]
    public void Transforms_Should_Emit_Expected_Utilities()
    {
        AssertUtility(b => b.TransformNone(), "transform-none", "transform", "none");
        AssertUtility(b => b.TransformGpu(), "transform-gpu", "transform", "translateZ(0)");

        AssertUtility(b => b.OriginCenter(), "origin-center", "transform-origin", "center");
        AssertUtility(b => b.OriginTop(), "origin-top", "transform-origin", "top");
        AssertUtility(b => b.OriginTopRight(), "origin-top-right", "transform-origin", "top right");
        AssertUtility(b => b.OriginRight(), "origin-right", "transform-origin", "right");
        AssertUtility(b => b.OriginBottomRight(), "origin-bottom-right", "transform-origin", "bottom right");
        AssertUtility(b => b.OriginBottom(), "origin-bottom", "transform-origin", "bottom");
        AssertUtility(b => b.OriginBottomLeft(), "origin-bottom-left", "transform-origin", "bottom left");
        AssertUtility(b => b.OriginLeft(), "origin-left", "transform-origin", "left");
        AssertUtility(b => b.OriginTopLeft(), "origin-top-left", "transform-origin", "top left");

        AssertUtility(b => b.Rotate(45), "rotate-45", "transform", "rotate(45deg)");
        AssertUtility(b => b.Scale(50), "scale-50", "transform", "scale(0.5)");
        AssertUtility(b => b.ScaleX(75), "scale-x-75", "transform", "scaleX(0.75)");
        AssertUtility(b => b.ScaleY(150), "scale-y-150", "transform", "scaleY(1.5)");
        AssertUtility(b => b.TranslateX(4), "translate-x-4", "transform", "translateX(1rem)");
        AssertUtility(b => b.TranslateX(Length.Px(-8)), "translate-x--8px", "transform", "translateX(-8px)");
        AssertUtility(b => b.TranslateY(2), "translate-y-2", "transform", "translateY(0.5rem)");
        AssertUtility(b => b.TranslateY(Length.Percent(50)), "translate-y-50%", "transform", "translateY(50%)");
        AssertUtility(b => b.SkewX(6), "skew-x-6", "transform", "skewX(6deg)");
        AssertUtility(b => b.SkewY(-6), "skew-y--6", "transform", "skewY(-6deg)");
    }

    [Fact]
    public void Filters_Should_Emit_Expected_Utilities()
    {
        AssertUtility(b => b.FilterNone(), "filter-none", "filter", "none");

        AssertUtility(b => b.Blur(), "blur", "filter", "blur(8px)");
        AssertUtility(b => b.BlurSm(), "blur-sm", "filter", "blur(4px)");
        AssertUtility(b => b.BlurMd(), "blur-md", "filter", "blur(12px)");
        AssertUtility(b => b.BlurLg(), "blur-lg", "filter", "blur(16px)");
        AssertUtility(b => b.BlurXl(), "blur-xl", "filter", "blur(24px)");
        AssertUtility(b => b.Blur2Xl(), "blur-2xl", "filter", "blur(40px)");
        AssertUtility(b => b.Blur3Xl(), "blur-3xl", "filter", "blur(64px)");

        AssertUtility(b => b.Brightness(50), "brightness-50", "filter", "brightness(0.5)");
        AssertUtility(b => b.Contrast(200), "contrast-200", "filter", "contrast(2)");

        AssertUtility(b => b.DropShadow(), "drop-shadow", "filter", "drop-shadow(0 1px 1px rgb(0 0 0 / 0.05))");
        AssertUtility(b => b.DropShadowSm(), "drop-shadow-sm", "filter", "drop-shadow(0 1px 0 rgb(0 0 0 / 0.05))");
        AssertUtility(b => b.DropShadowMd(), "drop-shadow-md", "filter", "drop-shadow(0 3px 3px rgb(0 0 0 / 0.12))");
        AssertUtility(b => b.DropShadowLg(), "drop-shadow-lg", "filter", "drop-shadow(0 4px 4px rgb(0 0 0 / 0.15))");
        AssertUtility(b => b.DropShadowXl(), "drop-shadow-xl", "filter", "drop-shadow(0 20px 13px rgb(0 0 0 / 0.03)) drop-shadow(0 8px 5px rgb(0 0 0 / 0.08))");
        AssertUtility(b => b.DropShadow2Xl(), "drop-shadow-2xl", "filter", "drop-shadow(0 25px 25px rgb(0 0 0 / 0.15))");

        AssertUtility(b => b.Grayscale(), "grayscale", "filter", "grayscale(100%)");
        AssertUtility(b => b.Grayscale(50), "grayscale-50", "filter", "grayscale(50%)");
        AssertUtility(b => b.HueRotate(90), "hue-rotate-90", "filter", "hue-rotate(90deg)");
        AssertUtility(b => b.Invert(), "invert", "filter", "invert(100%)");
        AssertUtility(b => b.Invert(25), "invert-25", "filter", "invert(25%)");
        AssertUtility(b => b.Saturate(150), "saturate-150", "filter", "saturate(1.5)");
        AssertUtility(b => b.Sepia(), "sepia", "filter", "sepia(100%)");
        AssertUtility(b => b.Sepia(50), "sepia-50", "filter", "sepia(50%)");
    }

    [Fact]
    public void New_Utilities_Should_Compose_With_Pseudo_And_Media_Variants()
    {
        ClassRegistry registry = new();
        var sut = new InternalRootFluentCssBuilder(registry, null, null, null);

        sut.Hover(x => x.SpaceX(2));
        sut.Media("min-width: 30rem", "sm", x => x.Rotate(45));

        registry.Select(x => x.ClassName).Should().Equal("hover:space-x-2", "sm:rotate-45");
    }

    [Fact]
    public void Named_Radii_Should_Emit_Expected_Utilities()
    {
        AssertUtility(b => b.RoundedXl(), "rounded-xl", "border-radius", "0.75rem");
        AssertUtility(b => b.Rounded2Xl(), "rounded-2xl", "border-radius", "1rem");
        AssertUtility(b => b.Rounded3Xl(), "rounded-3xl", "border-radius", "1.5rem");
    }

    [Fact]
    public void PerCorner_Radius_Should_Emit_Expected_Utilities()
    {
        AssertUtility(b => b.RoundedTL(), "rounded-tl", "border-top-left-radius", "0.25rem");
        AssertUtility(b => b.RoundedTR(Length.Rem(0.5)), "rounded-tr-0.5rem", "border-top-right-radius", "0.5rem");
        AssertUtility(b => b.RoundedBR(), "rounded-br", "border-bottom-right-radius", "0.25rem");
        AssertUtility(b => b.RoundedBL(Length.Px(8)), "rounded-bl-8px", "border-bottom-left-radius", "8px");
    }

    [Fact]
    public void PerSide_Radius_Should_Emit_Both_Corner_Declarations()
    {
        var left = Single(b => b.RoundedL(Length.Rem(0.375)));
        left.ClassName.Should().Be("rounded-l-0.375rem");
        left.Property.Should().Be("border-top-left-radius");
        left.AdditionalProperty.Should().Be("border-bottom-left-radius");
        left.CssString.Should().Be(".rounded-l-0\\.375rem { border-top-left-radius: 0.375rem; border-bottom-left-radius: 0.375rem } ");

        var top = Single(b => b.RoundedT());
        top.ClassName.Should().Be("rounded-t");
        top.CssString.Should().Be(".rounded-t { border-top-left-radius: 0.25rem; border-top-right-radius: 0.25rem } ");

        Single(b => b.RoundedR()).CssString.Should().Contain("border-top-right-radius: 0.25rem; border-bottom-right-radius: 0.25rem");
        Single(b => b.RoundedB()).CssString.Should().Contain("border-bottom-right-radius: 0.25rem; border-bottom-left-radius: 0.25rem");
    }

    [Fact]
    public void Larger_Text_Sizes_Should_Emit_Expected_Utilities()
    {
        AssertUtility(b => b.Text4Xl(), "text-4xl", "font-size", "2.25rem");
        AssertUtility(b => b.Text5Xl(), "text-5xl", "font-size", "3rem");
        AssertUtility(b => b.Text6Xl(), "text-6xl", "font-size", "3.75rem");
        AssertUtility(b => b.Text7Xl(), "text-7xl", "font-size", "4.5rem");
        AssertUtility(b => b.Text8Xl(), "text-8xl", "font-size", "6rem");
        AssertUtility(b => b.Text9Xl(), "text-9xl", "font-size", "8rem");
    }

    [Fact]
    public void Axis_Overflow_Should_Emit_Expected_Utilities()
    {
        AssertUtility(b => b.OverflowXAuto(), "overflow-x-auto", "overflow-x", "auto");
        AssertUtility(b => b.OverflowXHidden(), "overflow-x-hidden", "overflow-x", "hidden");
        AssertUtility(b => b.OverflowXScroll(), "overflow-x-scroll", "overflow-x", "scroll");
        AssertUtility(b => b.OverflowYAuto(), "overflow-y-auto", "overflow-y", "auto");
        AssertUtility(b => b.OverflowYHidden(), "overflow-y-hidden", "overflow-y", "hidden");
        AssertUtility(b => b.OverflowYScroll(), "overflow-y-scroll", "overflow-y", "scroll");
    }

    [Fact]
    public void Outline_Style_And_Colorless_Ring_Should_Emit_Expected_Utilities()
    {
        AssertUtility(b => b.OutlineSolid(), "outline-solid", "outline-style", "solid");
        AssertUtility(b => b.OutlineDashed(), "outline-dashed", "outline-style", "dashed");
        AssertUtility(b => b.OutlineDotted(), "outline-dotted", "outline-style", "dotted");
        AssertUtility(b => b.OutlineDouble(), "outline-double", "outline-style", "double");
        AssertUtility(b => b.OutlineNone(), "outline-none", "outline-style", "none");
        AssertUtility(b => b.Ring(0), "ring-0", "box-shadow", "0 0 0 0px");
    }

    [Fact]
    public void Radius_Should_Compose_With_Pseudo_Variants()
    {
        var added = Single(b => b.Hover(h => h.RoundedTL(Length.Rem(0.5))));
        added.ClassName.Should().Be("hover:rounded-tl-0.5rem");
        added.CssString.Should().Contain(":hover { border-top-left-radius");
    }

    [Fact]
    public void Container_Should_Emit_Full_Width_And_Breakpoint_Max_Widths()
    {
        ClassRegistry registry = new();
        var sut = new InternalRootFluentCssBuilder(registry, null, null, null);

        sut.Container();

        var classes = registry.Select(x => x.ClassName).ToList();
        classes.Should().Contain("w-full");
        classes.Should().Contain("min-sm:max-w-640px");
        classes.Should().Contain("min-md:max-w-768px");
        classes.Should().Contain("min-lg:max-w-1024px");
        classes.Should().Contain("min-xl:max-w-1280px");
        classes.Should().Contain("min-2xl:max-w-1536px");
        classes.Should().HaveCount(6);
    }
}
