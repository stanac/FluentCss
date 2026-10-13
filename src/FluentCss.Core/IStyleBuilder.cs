namespace FluentCss.Core;

/// <summary>
/// Base style builder interface
/// </summary>
public interface IStyleBuilder<out T>
{
    /// <summary>
    /// Sets background-color property
    /// </summary>
    /// <param name="color">Color</param>
    T Background(Color color);

    /// <summary>
    /// Sets color property
    /// </summary>
    /// <param name="color">Color</param>
    T Color(Color color);

    /// <summary>
    /// Sets border color-property
    /// </summary>
    /// <param name="color">Color</param>
    T BorderColor(Color color);

    /// <summary>
    /// Sets padding on all sides
    /// </summary>
    /// <param name="scale">Spacing scale, scale is 0.25rem</param>
    T P(int scale);

    /// <summary>
    /// Sets padding on all sides from a <see cref="Length"/>.
    /// </summary>
    T P(Length length);

    /// <summary>
    /// Sets padding on left and right sides
    /// </summary>
    /// <param name="scale">Spacing scale, scale is 0.25rem</param>
    T Px(int scale);

    /// <summary>
    /// Sets padding on left and right sides from a <see cref="Length"/>.
    /// </summary>
    T Px(Length length);

    /// <summary>
    /// Sets padding on top and bottom sides
    /// </summary>
    /// <param name="scale">Spacing scale, scale is 0.25rem</param>
    T Py(int scale);

    /// <summary>
    /// Sets padding on top and bottom sides from a <see cref="Length"/>.
    /// </summary>
    T Py(Length length);

    /// <summary>
    /// Sets padding on top side
    /// </summary>
    /// <param name="scale">Spacing scale, scale is 0.25rem</param>
    T Pt(int scale);

    /// <summary>
    /// Sets padding on top side from a <see cref="Length"/>.
    /// </summary>
    T Pt(Length length);

    /// <summary>
    /// Sets padding on right side
    /// </summary>
    /// <param name="scale">Spacing scale, scale is 0.25rem</param>
    T Pr(int scale);

    /// <summary>
    /// Sets padding on right side from a <see cref="Length"/>.
    /// </summary>
    T Pr(Length length);

    /// <summary>
    /// Sets padding on bottom side
    /// </summary>
    /// <param name="scale">Spacing scale, scale is 0.25rem</param>
    T Pb(int scale);

    /// <summary>
    /// Sets padding on bottom side from a <see cref="Length"/>.
    /// </summary>
    T Pb(Length length);

    /// <summary>
    /// Sets padding on left side
    /// </summary>
    /// <param name="scale">Spacing scale, scale is 0.25rem</param>
    T Pl(int scale);

    /// <summary>
    /// Sets padding on left side from a <see cref="Length"/>.
    /// </summary>
    T Pl(Length length);

    /// <summary>
    /// Sets margin on all sides
    /// </summary>
    /// <param name="scale">Spacing scale, scale is 0.25rem</param>
    T M(int scale);

    /// <summary>
    /// Sets margin on all sides from a <see cref="Length"/>.
    /// </summary>
    T M(Length length);

    /// <summary>
    /// Sets margin on left and right sides
    /// </summary>
    /// <param name="scale">Spacing scale, scale is 0.25rem</param>
    T Mx(int scale);

    /// <summary>
    /// Sets margin on left and right sides from a <see cref="Length"/>.
    /// </summary>
    T Mx(Length length);

    /// <summary>
    /// Sets margin on top and bottom sides
    /// </summary>
    /// <param name="scale">Spacing scale, scale is 0.25rem</param>
    T My(int scale);

    /// <summary>
    /// Sets margin on top and bottom sides from a <see cref="Length"/>.
    /// </summary>
    T My(Length length);

    /// <summary>
    /// Sets margin on top side
    /// </summary>
    /// <param name="scale">Spacing scale, scale is 0.25rem</param>
    T Mt(int scale);

    /// <summary>
    /// Sets margin on top side from a <see cref="Length"/>.
    /// </summary>
    T Mt(Length length);

    /// <summary>
    /// Sets margin on right side
    /// </summary>
    /// <param name="scale">Spacing scale, scale is 0.25rem</param>
    T Mr(int scale);

    /// <summary>
    /// Sets margin on right side from a <see cref="Length"/>.
    /// </summary>
    T Mr(Length length);

    /// <summary>
    /// Sets margin on bottom side
    /// </summary>
    /// <param name="scale">Spacing scale, scale is 0.25rem</param>
    T Mb(int scale);

    /// <summary>
    /// Sets margin on bottom side from a <see cref="Length"/>.
    /// </summary>
    T Mb(Length length);

    /// <summary>
    /// Sets margin on left side
    /// </summary>
    /// <param name="scale">Spacing scale, scale is 0.25rem</param>
    T Ml(int scale);

    /// <summary>
    /// Sets margin on left side from a <see cref="Length"/>.
    /// </summary>
    T Ml(Length length);

    /// <summary>
    /// Sets margin left and right to auto
    /// </summary>
    T MxAuto();

    /// <summary>
    /// Sets margin top and bottom to auto
    /// </summary>
    T MyAuto();

    /// <summary>
    /// Sets display to block
    /// </summary>
    T Block();

    /// <summary>
    /// Sets display to inline-block
    /// </summary>
    T InlineBlock();

    /// <summary>
    /// Sets display to inline
    /// </summary>
    T Inline();

    /// <summary>
    /// Sets display to flex
    /// </summary>
    T Flex();

    /// <summary>
    /// Sets display to grid
    /// </summary>
    T Grid();

    /// <summary>
    /// Sets display to none
    /// </summary>
    T Hidden();

    /// <summary>
    /// Sets position to relative
    /// </summary>
    T Relative();

    /// <summary>
    /// Sets position to absolute
    /// </summary>
    T Absolute();

    /// <summary>
    /// Sets position to fixed
    /// </summary>
    T Fixed();

    /// <summary>
    /// Sets position to sticky
    /// </summary>
    T Sticky();

    /// <summary>
    /// Sets top, right, bottom, and left properties
    /// </summary>
    /// <param name="scale">Position scale, scale is 0.25rem</param>
    T Inset(int scale);

    /// <summary>
    /// Sets top, right, bottom, and left properties from a <see cref="Length"/>.
    /// </summary>
    T Inset(Length length);

    /// <summary>
    /// Sets top property
    /// </summary>
    /// <param name="scale">Position scale, scale is 0.25rem</param>
    T Top(int scale);

    /// <summary>
    /// Sets top property from a <see cref="Length"/>.
    /// </summary>
    T Top(Length length);

    /// <summary>
    /// Sets bottom property
    /// </summary>
    /// <param name="scale">Position scale, scale is 0.25rem</param>
    T Bottom(int scale);

    /// <summary>
    /// Sets bottom property from a <see cref="Length"/>.
    /// </summary>
    T Bottom(Length length);

    /// <summary>
    /// Sets left property
    /// </summary>
    /// <param name="scale">Position scale, scale is 0.25rem</param>
    T Left(int scale);

    /// <summary>
    /// Sets left property from a <see cref="Length"/>.
    /// </summary>
    T Left(Length length);

    /// <summary>
    /// Sets right property
    /// </summary>
    /// <param name="scale">Position scale, scale is 0.25rem</param>
    T Right(int scale);

    /// <summary>
    /// Sets right property from a <see cref="Length"/>.
    /// </summary>
    T Right(Length length);

    /// <summary>
    /// Sets z-index
    /// </summary>
    /// <param name="index">Stack order index</param>
    T Z(int index);

    /// <summary>
    /// Sets width based on scale
    /// </summary>
    /// <param name="scale">Width scale, scale is 0.25rem</param>
    T W(int scale);

    /// <summary>
    /// Sets width from a <see cref="Length"/>.
    /// </summary>
    T W(Length length);

    /// <summary>
    /// Sets width to 100%
    /// </summary>
    T WFull();

    /// <summary>
    /// Sets width to 100vw
    /// </summary>
    T WScreen();

    /// <summary>
    /// Sets width to auto
    /// </summary>
    T WAuto();

    /// <summary>
    /// Sets height based on scale
    /// </summary>
    /// <param name="scale">Height scale, scale is 0.25rem</param>
    T H(int scale);

    /// <summary>
    /// Sets height from a <see cref="Length"/>.
    /// </summary>
    T H(Length length);

    /// <summary>
    /// Sets height to 100%
    /// </summary>
    T HFull();

    /// <summary>
    /// Sets height to 100vh
    /// </summary>
    T HScreen();

    /// <summary>
    /// Sets max-width
    /// </summary>
    /// <param name="size">Size identifier (e.g., md, lg, full)</param>
    T MaxW(Size size);

    /// <summary>
    /// Sets max-width from a <see cref="Length"/>.
    /// </summary>
    T MaxW(Length length);

    /// <summary>
    /// Sets min-height
    /// </summary>
    /// <param name="size">Size identifier</param>
    T MinH(Size size);

    /// <summary>
    /// Sets min-height from a <see cref="Length"/>.
    /// </summary>
    T MinH(Length length);

    /// <summary>
    /// Sets flex-direction to row
    /// </summary>
    T FlexRow();

    /// <summary>
    /// Sets flex-direction to column
    /// </summary>
    T FlexCol();

    /// <summary>
    /// Sets flex-wrap to wrap
    /// </summary>
    T FlexWrap();

    /// <summary>
    /// Sets align-items to center
    /// </summary>
    T ItemsCenter();

    /// <summary>
    /// Sets align-items to flex-start
    /// </summary>
    T ItemsStart();

    /// <summary>
    /// Sets align-items to flex-end
    /// </summary>
    T ItemsEnd();

    /// <summary>
    /// Sets justify-content to center
    /// </summary>
    T JustifyCenter();

    /// <summary>
    /// Sets justify-content to space-between
    /// </summary>
    T JustifyBetween();

    /// <summary>
    /// Sets justify-content to flex-start
    /// </summary>
    T JustifyStart();

    /// <summary>
    /// Sets justify-content to flex-end
    /// </summary>
    T JustifyEnd();

    /// <summary>
    /// Sets gap between rows and columns
    /// </summary>
    /// <param name="scale">Spacing scale, scale is 0.25rem</param>
    T Gap(int scale);

    /// <summary>
    /// Sets gap between rows and columns from a <see cref="Length"/>.
    /// </summary>
    T Gap(Length length);

    /// <summary>
    /// Sets column gap
    /// </summary>
    /// <param name="scale">Spacing scale, scale is 0.25rem</param>
    T GapX(int scale);

    /// <summary>
    /// Sets column gap from a <see cref="Length"/>.
    /// </summary>
    T GapX(Length length);

    /// <summary>
    /// Sets row gap
    /// </summary>
    /// <param name="scale">Spacing scale, scale is 0.25rem</param>
    T GapY(int scale);

    /// <summary>
    /// Sets row gap from a <see cref="Length"/>.
    /// </summary>
    T GapY(Length length);

    /// <summary>
    /// Sets flex-grow to 1
    /// </summary>
    T Grow();

    /// <summary>
    /// Sets flex-shrink to 1
    /// </summary>
    T Shrink();

    /// <summary>
    /// Sets font-size to 0.75rem and line-height to 1rem
    /// </summary>
    T TextXs();

    /// <summary>
    /// Sets font-size to 0.875rem and line-height to 1.25rem
    /// </summary>
    T TextSm();

    /// <summary>
    /// Sets font-size to 1rem and line-height to 1.5rem
    /// </summary>
    T TextBase();

    /// <summary>
    /// Sets font-size to 1.125rem and line-height to 1.75rem
    /// </summary>
    T TextLg();

    /// <summary>
    /// Sets font-size to 1.25rem and line-height to 1.75rem
    /// </summary>
    T TextXl();

    /// <summary>
    /// Sets font-size to 1.5rem and line-height to 2rem
    /// </summary>
    T Text2Xl();

    /// <summary>
    /// Sets font-size to 1.875rem and line-height to 2.25rem
    /// </summary>
    T Text3Xl();

    /// <summary>
    /// Sets font-size from a <see cref="Length"/>, for example
    /// <c>Text(Length.Rem(3))</c> or <c>Text(new Length("clamp(1rem,2vw,2rem)", "fluid"))</c>.
    /// </summary>
    T Text(Length size);

    /// <summary>
    /// Sets font-weight to 100
    /// </summary>
    T FontThin();

    /// <summary>
    /// Sets font-weight to 400
    /// </summary>
    T FontNormal();

    /// <summary>
    /// Sets font-weight to 700
    /// </summary>
    T FontBold();

    /// <summary>
    /// Sets font-family to the default <c>sans</c> stack (<c>font-sans</c>)
    /// </summary>
    T FontSans();

    /// <summary>
    /// Sets font-family to the default <c>serif</c> stack (<c>font-serif</c>)
    /// </summary>
    T FontSerif();

    /// <summary>
    /// Sets font-family to the default monospace stack (<c>font-mono</c>)
    /// </summary>
    T FontMono();

    /// <summary>
    /// Sets font-family from a <see cref="FontFamily"/>, for example
    /// <c>Font(FontFamily.Sans)</c> or <c>Font(new FontFamily("Inter, sans-serif", "brand"))</c>.
    /// </summary>
    T Font(FontFamily family);

    /// <summary>
    /// Prepends custom families to the default <c>sans</c> stack. The class token is
    /// derived from the families and suffixed with a short hash, for example
    /// <c>Sans("Inter")</c> emits <c>font-family: Inter, ui-sans-serif, …</c> with the
    /// class <c>font-sans-inter-&lt;hash&gt;</c>.
    /// </summary>
    T Sans(params string[] families);

    /// <summary>
    /// Prepends custom families to the default <c>serif</c> stack.
    /// </summary>
    T Serif(params string[] families);

    /// <summary>
    /// Prepends custom families to the default monospace stack.
    /// </summary>
    T Mono(params string[] families);

    /// <summary>
    /// Sets text-align to center
    /// </summary>
    T TextCenter();

    /// <summary>
    /// Sets text-align to left
    /// </summary>
    T TextLeft();

    /// <summary>
    /// Sets text-align to right
    /// </summary>
    T TextRight();

    /// <summary>
    /// Sets text-decoration to underline
    /// </summary>
    T Underline();

    /// <summary>
    /// Sets text-decoration to none
    /// </summary>
    T NoUnderline();

    /// <summary>
    /// Sets border-width
    /// </summary>
    /// <param name="width">Border width in pixels</param>
    T Border(int width);

    /// <summary>
    /// Sets border-width to 1px
    /// </summary>
    T Border();

    /// <summary>
    /// Sets border-radius to 0.25rem
    /// </summary>
    T Rounded();

    /// <summary>
    /// Sets border-radius from a <see cref="Length"/>, for example
    /// <c>Rounded(Length.Percent(50))</c> or <c>Rounded(new Length("1.25rem", "card"))</c>.
    /// </summary>
    T Rounded(Length radius);

    /// <summary>
    /// Sets border-radius to 0.125rem
    /// </summary>
    T RoundedSm();

    /// <summary>
    /// Sets border-radius to 0.375rem
    /// </summary>
    T RoundedMd();

    /// <summary>
    /// Sets border-radius to 0.5rem
    /// </summary>
    T RoundedLg();

    /// <summary>
    /// Sets border-radius to 9999px
    /// </summary>
    T RoundedFull();

    /// <summary>
    /// Sets box-shadow to default
    /// </summary>
    T Shadow();

    /// <summary>
    /// Sets box-shadow to small
    /// </summary>
    T ShadowSm();

    /// <summary>
    /// Sets box-shadow to medium
    /// </summary>
    T ShadowMd();

    /// <summary>
    /// Sets box-shadow to large
    /// </summary>
    T ShadowLg();

    /// <summary>
    /// Sets opacity
    /// </summary>
    /// <param name="val">Opacity value (0-100)</param>
    T Opacity(int val);

    /// <summary>
    /// Sets cursor to pointer
    /// </summary>
    T CursorPointer();

    /// <summary>
    /// Sets min-width
    /// </summary>
    /// <param name="size">Size identifier</param>
    T MinW(Size size);

    /// <summary>
    /// Sets min-width from a <see cref="Length"/>.
    /// </summary>
    T MinW(Length length);

    /// <summary>
    /// Sets max-height
    /// </summary>
    /// <param name="size">Size identifier</param>
    T MaxH(Size size);

    /// <summary>
    /// Sets max-height from a <see cref="Length"/>.
    /// </summary>
    T MaxH(Length length);

    /// <summary>
    /// Creates a grid with n equal-width columns
    /// </summary>
    /// <param name="n">Number of columns</param>
    T GridCols(int n);

    /// <summary>
    /// Creates a grid with n equal-height rows
    /// </summary>
    /// <param name="n">Number of rows</param>
    T GridRows(int n);

    /// <summary>
    /// Makes element span n columns
    /// </summary>
    /// <param name="n">Number of columns to span</param>
    T ColSpan(int n);

    /// <summary>
    /// Makes element span n rows
    /// </summary>
    /// <param name="n">Number of rows to span</param>
    T RowSpan(int n);

    /// <summary>
    /// Makes element start at grid column line n
    /// </summary>
    /// <param name="n">Start line</param>
    T ColStart(int n);

    /// <summary>
    /// Sets overflow to auto (adds scrollbars only if content overflows)
    /// </summary>
    T OverflowAuto();

    /// <summary>
    /// Sets overflow to hidden (clips overflowing content)
    /// </summary>
    T OverflowHidden();

    /// <summary>
    /// Sets overflow to scroll (always adds scrollbars)
    /// </summary>
    T OverflowScroll();

    /// <summary>
    /// Resizes content to cover container, preserving aspect ratio
    /// </summary>
    T ObjectCover();

    /// <summary>
    /// Resizes content to fit within container, preserving aspect ratio
    /// </summary>
    T ObjectContain();

    /// <summary>
    /// Sets aspect-ratio to 1 / 1
    /// </summary>
    T AspectSquare();

    /// <summary>
    /// Sets aspect-ratio to 16 / 9
    /// </summary>
    T AspectVideo();

    /// <summary>
    /// Sets font-style to italic
    /// </summary>
    T Italic();

    /// <summary>
    /// Sets font-style to normal
    /// </summary>
    T NotItalic();

    /// <summary>
    /// Transforms text to uppercase
    /// </summary>
    T Uppercase();

    /// <summary>
    /// Transforms text to lowercase
    /// </summary>
    T Lowercase();

    /// <summary>
    /// Capitalizes the first letter of each word
    /// </summary>
    T Capitalize();

    /// <summary>
    /// Prevents text wrapping
    /// </summary>
    T WhitespaceNoWrap();

    /// <summary>
    /// Sets line-height based on scale
    /// </summary>
    /// <param name="scale">Spacing scale, scale is 0.25rem</param>
    T Leading(int scale);

    /// <summary>
    /// Sets line-height from a <see cref="Length"/>.
    /// </summary>
    T Leading(Length length);

    /// <summary>
    /// Sets border-style to solid
    /// </summary>
    T BorderSolid();

    /// <summary>
    /// Sets border-style to dashed
    /// </summary>
    T BorderDashed();

    /// <summary>
    /// Sets border-style to dotted
    /// </summary>
    T BorderDotted();

    /// <summary>
    /// Sets width and color of the ring (box-shadow)
    /// </summary>
    /// <param name="width">Width in pixels</param>
    /// <param name="c">Color</param>
    T Ring(int width, Color c);
    
    /// <summary>
    /// Sets the width of the outline
    /// </summary>
    /// <param name="width">Outline width in pixels</param>
    T Outline(int width);

    /// <summary>
    /// Hides the outline
    /// </summary>
    T OutlineHidden();

    /// <summary>
    /// Composite method for setting border properties at once
    /// </summary>
    /// <param name="width">Width in pixels</param>
    /// <param name="c">Color</param>
    /// <param name="style">Border Style (default Solid)</param>
    T Border(int width, Color c, BorderStyle style = BorderStyle.Solid);

    /// <summary>
    /// Composite method for Flex container layout
    /// </summary>
    /// <param name="direction">Flex direction</param>
    /// <param name="justify">Justify content (optional)</param>
    /// <param name="align">Align items (optional)</param>
    /// <param name="gap">Gap scale (optional)</param>
    /// <param name="wrap">Flex wrap (optional)</param>
    T Flex(FlexDirection direction, Justify? justify = null, Align? align = null, int? gap = null, FlexWrapValue? wrap = null);

    /// <summary>
    /// Escape hatch for any CSS declaration the typed helpers do not cover. Emits a
    /// single utility with the given class name, property and value.
    /// </summary>
    /// <remarks>
    /// The class name is the identity used to deduplicate the generated CSS, so it must
    /// be unique and stable. Use the typed overloads (<see cref="Length"/>, <see cref="Color"/>,
    /// <see cref="Size"/>) when they exist; reach for this only for the gaps.
    /// </remarks>
    /// <param name="className">Class name / token for the utility, for example <c>font-350</c></param>
    /// <param name="property">CSS property, for example <c>font-weight</c></param>
    /// <param name="value">CSS value, for example <c>350</c></param>
    T Arbitrary(string className, string property, string value);
}