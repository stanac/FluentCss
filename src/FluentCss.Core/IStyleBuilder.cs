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

    /// <summary>Sets display to inline-flex</summary>
    T InlineFlex();

    /// <summary>Sets display to inline-grid</summary>
    T InlineGrid();

    /// <summary>Sets display to flow-root</summary>
    T FlowRoot();

    /// <summary>Sets display to contents</summary>
    T Contents();

    /// <summary>Sets display to list-item</summary>
    T ListItem();

    /// <summary>Sets position to static</summary>
    T Static();

    /// <summary>Sets z-index to auto</summary>
    T ZAuto();

    /// <summary>Sets float to left</summary>
    T FloatLeft();

    /// <summary>Sets float to right</summary>
    T FloatRight();

    /// <summary>Sets float to none</summary>
    T FloatNone();

    /// <summary>Sets clear to left</summary>
    T ClearLeft();

    /// <summary>Sets clear to right</summary>
    T ClearRight();

    /// <summary>Sets clear to both</summary>
    T ClearBoth();

    /// <summary>Sets clear to none</summary>
    T ClearNone();

    /// <summary>Sets isolation to isolate</summary>
    T Isolate();

    /// <summary>Sets isolation to auto</summary>
    T IsolationAuto();

    /// <summary>Sets box-sizing to border-box</summary>
    T BoxBorder();

    /// <summary>Sets box-sizing to content-box</summary>
    T BoxContent();

    /// <summary>Sets visibility to visible</summary>
    T Visible();

    /// <summary>Sets visibility to hidden</summary>
    T Invisible();

    /// <summary>Sets object-fit to fill</summary>
    T ObjectFill();

    /// <summary>Sets object-fit to scale-down</summary>
    T ObjectScaleDown();

    /// <summary>Sets object-fit to none</summary>
    T ObjectNone();

    /// <summary>Sets flex-basis based on scale</summary>
    T Basis(int scale);

    /// <summary>Sets flex-basis from a <see cref="Length"/>.</summary>
    T Basis(Length length);

    /// <summary>Sets flex-basis to auto</summary>
    T BasisAuto();

    /// <summary>Sets flex-basis to 100%</summary>
    T BasisFull();

    /// <summary>Sets order</summary>
    T Order(int order);

    /// <summary>Sets order to -9999</summary>
    T OrderFirst();

    /// <summary>Sets order to 9999</summary>
    T OrderLast();

    /// <summary>Sets order to 0</summary>
    T OrderNone();

    /// <summary>Sets flex-grow</summary>
    T Grow(int value);

    /// <summary>Sets flex-shrink</summary>
    T Shrink(int value);

    /// <summary>Sets grid-auto-flow to row</summary>
    T GridAutoFlowRow();

    /// <summary>Sets grid-auto-flow to column</summary>
    T GridAutoFlowColumn();

    /// <summary>Sets grid-auto-flow to row dense</summary>
    T GridAutoFlowRowDense();

    /// <summary>Sets grid-auto-flow to column dense</summary>
    T GridAutoFlowColumnDense();

    /// <summary>Makes element end at grid column line n</summary>
    T ColEnd(int n);

    /// <summary>Makes element start at grid row line n</summary>
    T RowStart(int n);

    /// <summary>Makes element end at grid row line n</summary>
    T RowEnd(int n);

    /// <summary>Sets justify-items to start</summary>
    T JustifyItemsStart();

    /// <summary>Sets justify-items to center</summary>
    T JustifyItemsCenter();

    /// <summary>Sets justify-items to end</summary>
    T JustifyItemsEnd();

    /// <summary>Sets justify-items to stretch</summary>
    T JustifyItemsStretch();

    /// <summary>Sets justify-self to auto</summary>
    T JustifySelfAuto();

    /// <summary>Sets justify-self to start</summary>
    T JustifySelfStart();

    /// <summary>Sets justify-self to center</summary>
    T JustifySelfCenter();

    /// <summary>Sets justify-self to end</summary>
    T JustifySelfEnd();

    /// <summary>Sets justify-self to stretch</summary>
    T JustifySelfStretch();

    /// <summary>Sets align-content to start</summary>
    T AlignContentStart();

    /// <summary>Sets align-content to center</summary>
    T AlignContentCenter();

    /// <summary>Sets align-content to end</summary>
    T AlignContentEnd();

    /// <summary>Sets align-content to space-between</summary>
    T AlignContentBetween();

    /// <summary>Sets align-content to space-around</summary>
    T AlignContentAround();

    /// <summary>Sets align-content to space-evenly</summary>
    T AlignContentEvenly();

    /// <summary>Sets align-content to baseline</summary>
    T AlignContentBaseline();

    /// <summary>Sets align-content to stretch</summary>
    T AlignContentStretch();

    /// <summary>Sets align-self to auto</summary>
    T AlignSelfAuto();

    /// <summary>Sets align-self to start</summary>
    T AlignSelfStart();

    /// <summary>Sets align-self to center</summary>
    T AlignSelfCenter();

    /// <summary>Sets align-self to end</summary>
    T AlignSelfEnd();

    /// <summary>Sets align-self to baseline</summary>
    T AlignSelfBaseline();

    /// <summary>Sets align-self to stretch</summary>
    T AlignSelfStretch();

    /// <summary>Sets justify-content to space-around</summary>
    T JustifyAround();

    /// <summary>Sets justify-content to space-evenly</summary>
    T JustifyEvenly();

    /// <summary>Sets align-items to baseline</summary>
    T ItemsBaseline();

    /// <summary>Sets align-items to stretch</summary>
    T ItemsStretch();

    /// <summary>Sets the top border width to 1px</summary>
    T BorderT();

    /// <summary>Sets the top border width</summary>
    T BorderT(int width);

    /// <summary>Sets the right border width to 1px</summary>
    T BorderR();

    /// <summary>Sets the right border width</summary>
    T BorderR(int width);

    /// <summary>Sets the bottom border width to 1px</summary>
    T BorderB();

    /// <summary>Sets the bottom border width</summary>
    T BorderB(int width);

    /// <summary>Sets the left border width to 1px</summary>
    T BorderL();

    /// <summary>Sets the left border width</summary>
    T BorderL(int width);

    /// <summary>Sets the horizontal border width to 1px</summary>
    T BorderX();

    /// <summary>Sets the horizontal border width</summary>
    T BorderX(int width);

    /// <summary>Sets the vertical border width to 1px</summary>
    T BorderY();

    /// <summary>Sets the vertical border width</summary>
    T BorderY(int width);

    /// <summary>Sets border-style to double</summary>
    T BorderDouble();

    /// <summary>Sets outline-color</summary>
    T OutlineColor(Color color);

    /// <summary>Sets outline-offset in pixels</summary>
    T OutlineOffset(int offset);

    /// <summary>Sets font-weight to 200</summary>
    T FontExtralight();

    /// <summary>Sets font-weight to 300</summary>
    T FontLight();

    /// <summary>Sets font-weight to 500</summary>
    T FontMedium();

    /// <summary>Sets font-weight to 600</summary>
    T FontSemibold();

    /// <summary>Sets font-weight to 800</summary>
    T FontExtrabold();

    /// <summary>Sets font-weight to 900</summary>
    T FontBlack();

    /// <summary>Sets line-height to 1</summary>
    T LeadingNone();

    /// <summary>Sets line-height to 1.25</summary>
    T LeadingTight();

    /// <summary>Sets line-height to 1.375</summary>
    T LeadingSnug();

    /// <summary>Sets line-height to 1.5</summary>
    T LeadingNormal();

    /// <summary>Sets line-height to 1.625</summary>
    T LeadingRelaxed();

    /// <summary>Sets line-height to 2</summary>
    T LeadingLoose();

    /// <summary>Sets letter-spacing to -0.05em</summary>
    T TrackingTighter();

    /// <summary>Sets letter-spacing to -0.025em</summary>
    T TrackingTight();

    /// <summary>Sets letter-spacing to 0em</summary>
    T TrackingNormal();

    /// <summary>Sets letter-spacing to 0.025em</summary>
    T TrackingWide();

    /// <summary>Sets letter-spacing to 0.05em</summary>
    T TrackingWider();

    /// <summary>Sets letter-spacing to 0.1em</summary>
    T TrackingWidest();

    /// <summary>Sets letter-spacing from a <see cref="Length"/>.</summary>
    T Tracking(Length length);

    /// <summary>Sets white-space to normal</summary>
    T WhitespaceNormal();

    /// <summary>Sets white-space to pre</summary>
    T WhitespacePre();

    /// <summary>Sets white-space to pre-line</summary>
    T WhitespacePreLine();

    /// <summary>Sets white-space to pre-wrap</summary>
    T WhitespacePreWrap();

    /// <summary>Sets white-space to break-spaces</summary>
    T WhitespaceBreakSpaces();

    /// <summary>Sets text-decoration to line-through</summary>
    T LineThrough();

    /// <summary>Sets text-decoration to overline</summary>
    T Overline();

    /// <summary>Truncates overflowing text with an ellipsis</summary>
    T Truncate();

    /// <summary>Sets text-overflow to ellipsis</summary>
    T TextEllipsis();

    /// <summary>Sets text-overflow to clip</summary>
    T TextClip();

    /// <summary>Sets text-indent based on scale</summary>
    T TextIndent(int scale);

    /// <summary>Sets text-indent from a <see cref="Length"/>.</summary>
    T TextIndent(Length length);

    /// <summary>Sets word-break to normal</summary>
    T WordBreakNormal();

    /// <summary>Sets word-break to break-all</summary>
    T WordBreakAll();

    /// <summary>Sets word-break to keep-all</summary>
    T WordBreakKeepAll();

    /// <summary>Sets overflow-wrap to normal</summary>
    T OverflowWrapNormal();

    /// <summary>Sets overflow-wrap to break-word</summary>
    T OverflowWrapBreakWord();

    /// <summary>Sets overflow-wrap to anywhere</summary>
    T OverflowWrapAnywhere();

    /// <summary>Sets hyphens to none</summary>
    T HyphensNone();

    /// <summary>Sets hyphens to manual</summary>
    T HyphensManual();

    /// <summary>Sets hyphens to auto</summary>
    T HyphensAuto();

    /// <summary>Sets list-style-type to none</summary>
    T ListStyleNone();

    /// <summary>Sets list-style-type to disc</summary>
    T ListDisc();

    /// <summary>Sets list-style-type to decimal</summary>
    T ListDecimal();

    /// <summary>Sets list-style-position to inside</summary>
    T ListInside();

    /// <summary>Sets list-style-position to outside</summary>
    T ListOutside();

    /// <summary>Sets background-attachment to fixed</summary>
    T BgAttachmentFixed();

    /// <summary>Sets background-attachment to local</summary>
    T BgAttachmentLocal();

    /// <summary>Sets background-attachment to scroll</summary>
    T BgAttachmentScroll();

    /// <summary>Sets background-clip to border-box</summary>
    T BgClipBorder();

    /// <summary>Sets background-clip to padding-box</summary>
    T BgClipPadding();

    /// <summary>Sets background-clip to content-box</summary>
    T BgClipContent();

    /// <summary>Sets background-clip to text</summary>
    T BgClipText();

    /// <summary>Sets background-origin to border-box</summary>
    T BgOriginBorder();

    /// <summary>Sets background-origin to padding-box</summary>
    T BgOriginPadding();

    /// <summary>Sets background-origin to content-box</summary>
    T BgOriginContent();

    /// <summary>Sets background-repeat to repeat</summary>
    T BgRepeat();

    /// <summary>Sets background-repeat to repeat-x</summary>
    T BgRepeatX();

    /// <summary>Sets background-repeat to repeat-y</summary>
    T BgRepeatY();

    /// <summary>Sets background-repeat to no-repeat</summary>
    T BgNoRepeat();

    /// <summary>Sets background-repeat to round</summary>
    T BgRepeatRound();

    /// <summary>Sets background-repeat to space</summary>
    T BgRepeatSpace();

    /// <summary>Sets background-size to cover</summary>
    T BgCover();

    /// <summary>Sets background-size to contain</summary>
    T BgContain();

    /// <summary>Sets background-size to auto</summary>
    T BgAuto();

    /// <summary>Sets background-image to none</summary>
    T BgNone();

    /// <summary>Sets border-collapse to collapse</summary>
    T BorderCollapse();

    /// <summary>Sets border-collapse to separate</summary>
    T BorderSeparate();

    /// <summary>Sets table-layout to auto</summary>
    T TableLayoutAuto();

    /// <summary>Sets table-layout to fixed</summary>
    T TableLayoutFixed();

    /// <summary>Sets caption-side to top</summary>
    T CaptionTop();

    /// <summary>Sets caption-side to bottom</summary>
    T CaptionBottom();

    /// <summary>Sets pointer-events to none</summary>
    T PointerEventsNone();

    /// <summary>Sets pointer-events to auto</summary>
    T PointerEventsAuto();

    /// <summary>Sets resize to none</summary>
    T ResizeNone();

    /// <summary>Sets resize to vertical</summary>
    T ResizeY();

    /// <summary>Sets resize to horizontal</summary>
    T ResizeX();

    /// <summary>Sets resize to both</summary>
    T ResizeBoth();

    /// <summary>Sets scroll-behavior to smooth</summary>
    T ScrollBehaviorSmooth();

    /// <summary>Sets scroll-behavior to auto</summary>
    T ScrollBehaviorAuto();

    /// <summary>Sets user-select to none</summary>
    T UserSelectNone();

    /// <summary>Sets user-select to text</summary>
    T UserSelectText();

    /// <summary>Sets user-select to all</summary>
    T UserSelectAll();

    /// <summary>Sets user-select to auto</summary>
    T UserSelectAuto();

    /// <summary>Sets appearance to none</summary>
    T AppearanceNone();

    /// <summary>Sets appearance to auto</summary>
    T AppearanceAuto();

    /// <summary>Sets accent-color</summary>
    T AccentColor(Color color);

    /// <summary>Sets caret-color</summary>
    T CaretColor(Color color);

    /// <summary>Sets color-scheme to light</summary>
    T ColorSchemeLight();

    /// <summary>Sets color-scheme to dark</summary>
    T ColorSchemeDark();

    /// <summary>Sets color-scheme to normal</summary>
    T ColorSchemeNormal();

    /// <summary>Sets cursor to default</summary>
    T CursorDefault();

    /// <summary>Sets cursor to text</summary>
    T CursorText();

    /// <summary>Sets cursor to wait</summary>
    T CursorWait();

    /// <summary>Sets cursor to move</summary>
    T CursorMove();

    /// <summary>Sets cursor to help</summary>
    T CursorHelp();

    /// <summary>Sets cursor to not-allowed</summary>
    T CursorNotAllowed();

    /// <summary>Sets cursor to crosshair</summary>
    T CursorCrosshair();

    /// <summary>Sets cursor to grab</summary>
    T CursorGrab();

    /// <summary>Sets cursor to grabbing</summary>
    T CursorGrabbing();

    /// <summary>Sets cursor to none</summary>
    T CursorNone();

    /// <summary>Sets cursor to progress</summary>
    T CursorProgress();

    /// <summary>Sets touch-action to none</summary>
    T TouchActionNone();

    /// <summary>Sets touch-action to auto</summary>
    T TouchActionAuto();

    /// <summary>Sets touch-action to pan-x</summary>
    T TouchActionPanX();

    /// <summary>Sets touch-action to pan-y</summary>
    T TouchActionPanY();

    /// <summary>Sets touch-action to manipulation</summary>
    T TouchActionManipulation();

    /// <summary>Sets will-change to auto</summary>
    T WillChangeAuto();

    /// <summary>Sets will-change to scroll-position</summary>
    T WillChangeScroll();

    /// <summary>Sets will-change to contents</summary>
    T WillChangeContents();

    /// <summary>Sets will-change to transform</summary>
    T WillChangeTransform();

    /// <summary>Sets fill</summary>
    T Fill(Color color);

    /// <summary>Sets stroke</summary>
    T Stroke(Color color);

    /// <summary>Sets stroke-width in pixels</summary>
    T StrokeWidth(int width);

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