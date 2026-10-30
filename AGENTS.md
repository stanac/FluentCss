# AGENTS.md

> **Scope:** how to *use* the FluentCss library in an application. This is **not** a
> guide for developing FluentCss itself. Human docs: [README](README.md) and the
> [documentation site](https://fluentcss.stanac.dev).

## Purpose & Overview

FluentCss is a utility-first CSS builder for .NET. You compose Tailwind-inspired
utility classes with a fluent, type-safe C# API; the library generates both the
class-name string and the CSS. It works in Blazor, ASP.NET Core MVC / Razor Pages
and any other .NET app.

- Entry points (namespace `FluentCss.Core`): `FluentCssBuilder.Css`,
  `FluentCssBuilder.Dark`, `FluentCssBuilder.Theme(name)`.
- Generated CSS is deduplicated in a process-wide registry, read from
  `ClassRegistry.DefaultRegistry.Css`.
- Class names are an implementation detail — never hard-code or depend on their spelling.

## Choose the package

| Hosting model | Packages | Ships |
|---|---|---|
| Blazor (any render mode) | `FluentCss` | `<CssStyles>`, `<fluent-css />` + theme tag helpers, `preflight.css` |
| ASP.NET Core MVC / Razor Pages | `FluentCss` + `FluentCss.Middleware` | tag helpers + the stylesheet middleware |
| Console / other .NET | `FluentCss.Core` | the builder and registry |

`FluentCss` and `FluentCss.Middleware` target `net10.0`; `FluentCss.Core` targets
`netstandard2.0` and is pulled in transitively.

## Critical Boundaries

**ALWAYS DO**
- Add the package(s) above before writing builder code.
- Read the stylesheet from `ClassRegistry.DefaultRegistry.Css` (or `CssUtf8` / `Sha256Hex`).
- Precompute class strings in a static `*Styles` class or `@code` field for Blazor components.
- For interactive Blazor, put `<CssStyles />` inside the interactive render boundary (the layout), not the static `App.razor` head.
- For MVC / Razor Pages, call `builder.Services.AddFluentCss()` and `app.UseFluentCss()`.
- Link `preflight.css` if you want the Tailwind reset (optional but recommended).
- Run `dotnet build` after wiring, and `dotnet test` before declaring a task complete.

**ASK FIRST**
- Before using `new ClassRegistry()` instead of the default registry.
- Before using `Arbitrary(...)` — a typed method almost always exists.
- Before using `Important(...)` — fix the cascade when possible.
- Before depending on a generated class name in your own CSS or JavaScript.
- Before adding a third-party dependency to work around a styling problem.

**NEVER DO**
- Never call the builder inline in a Blazor **component** attribute; it does not compile.
- Never hand-write CSS for utilities FluentCss already generates.
- Never assume 1:1 Tailwind parity — see [missing-features.md](missing-features.md).
- Never mutate registry internals or reuse a utility class name for something else.
- Never reference `FluentCss.Core` alone in an MVC / Razor Pages app and expect CSS delivery.

## Commands

```bash
dotnet add package FluentCss              # Blazor
dotnet add package FluentCss.Middleware   # MVC / Razor Pages (alongside FluentCss)
dotnet add package FluentCss.Core         # console / other .NET
dotnet build                              # verify the app compiles
dotnet test                               # run the app's tests
```

## Quick start per hosting model

### Blazor

`_Imports.razor`:
```razor
@using static FluentCss.Core.FluentCssBuilder
@using FluentCss
@using FluentCss.Core
```
Layout — keep `<CssStyles />` at the begining of the layout during development:
```razor
<link rel="stylesheet" href="_content/FluentCss/preflight.css" />
...
<CssStyles />
```

### MVC / Razor Pages

`Program.cs`:
```csharp
builder.Services.AddFluentCss();

var app = builder.Build();

app.UseRouting();
app.UseFluentCss();   // serves /fluentcss/hash and /fluentcss/{hash}.css
app.UseAuthorization();
```
`_ViewImports.cshtml` / `Pages/_ViewImports.cshtml`:
```razor
@using static FluentCss.Core.FluentCssBuilder
@using FluentCss.Core
@addTagHelper *, FluentCss
```
Layout `<head>`:
```html
<link rel="stylesheet" href="/_content/FluentCss/preflight.css" />
<fluent-css />
```

### Console / other .NET

```csharp
using FluentCss.Core;

var classes = FluentCssBuilder.Css.P(4).RoundedMd().Background(Color.Emerald500);
string classAttr = classes.ToString()!;             // class attribute value
string css = ClassRegistry.DefaultRegistry.Css;     // the full stylesheet
File.WriteAllText("styles.css", css);
```

## Composing styles

Chain calls anywhere C# runs. Each call registers its class immediately, so order is free.

```csharp
using static FluentCss.Core.FluentCssBuilder;

public static class MyStyles
{
    public static string PrimaryButton { get; } =
        $"{Css.InlineBlock().Px(6).Py(3).RoundedMd().FontBold()
              .Background(Color.Blue600).Color(Color.White)
              .Hover(h => h.Background(Color.Blue700))
              .Focus(f => f.Ring(3, Color.Blue300).OutlineHidden())} " +
        $"{Dark.Background(Color.Blue500)}";
}
```
Use it: `<button class="@MyStyles.PrimaryButton">Save</button>`.

### Variants

```csharp
Css.Hover(h => h.Background(Color.Blue700));   // pseudo-classes
Css.MinMd(md => md.Flex(FlexDirection.Row));   // responsive (>= breakpoint)
Css.MaxMd(md => md.Hidden());                  // responsive (< breakpoint)
Css.Media("(prefers-reduced-motion: reduce)", "reduce-motion", x => x.Hidden());
Css.Important(x => x.P(4));                    // !important override block
Dark.Background(Color.Slate900);               // theme-dark: variant
Theme("ocean").Background(Color.Sky100);       // theme-ocean: variant
```

### Real selectors

```csharp
Css.ApplyClass("btn", css => css.Px(4).Py(2).RoundedMd());
Css.ApplySelector(s => s.Class("card").Child().Element("h2"), css => css.FontBold());
Css.ApplySelector("input[type=\"text\"]", css => css.Border(1));  // raw escape hatch
```

### Typed tokens (prefer over raw strings)

- `Color.Red500`, `Color.Slate950`, ... — from `FluentCss.Core.Color`.
- `Length.Px(400)`, `Length.Rem(1.5)`, `Length.Percent(80)` — arbitrary values.
- `Size.Sm` / `Size.Md` / `Size.Lg` / `Size.Full` / ... — for `MaxW`, `MinH`, etc.
- `FontFamily`, `Breakpoints.XSmall` ... `Breakpoints.XXLarge`.

## Interfaces

The public surface is a small set of interfaces plus one static entry point and one
abstract base. Pick the narrowest interface that exposes what a helper needs; the
full method list for `IStyleBuilder<T>` is on the
[Interfaces docs page](https://fluentcss.stanac.dev/docs/interfaces).

### Builder interfaces

| Interface | Inherits | Purpose |
|---|---|---|
| `IStyleBuilder<out T>` | — | Declares **every utility method**; implemented by all builders and scopes. `T` is the builder returned by each call. |
| `IImportantStyleBuilder<out T>` | `IStyleBuilder<T>` | Adds `Important(Action<IStyleScopeBuilder>)`. |
| `IStyleScopeBuilder` | `IStyleBuilder<IStyleScopeBuilder>` | Scope passed to `Important`; deliberately has no `Important`, so blocks cannot nest. |
| `IOnlyStyleBuilder` | `IImportantStyleBuilder<IOnlyStyleBuilder>` | Styles-only scope passed to `Pseudo`/`Hover`/`Focus`/`Active`/`ApplySelector`; no variant methods. |
| `IFluentCssBuilder` | `IImportantStyleBuilder<IFluentCssBuilder>` | The normal builder returned by `Css`/`Dark`/`Theme(...)`; adds `Pseudo(...)`. |
| `IMediaFluentCssBuilder` | `IFluentCssBuilder` | Adds `Media(query, prefix, action)`, the primitive behind the responsive helpers. |
| `IRootFluentCssBuilder` | `IMediaFluentCssBuilder` | Root stage; adds `ApplySelector(...)`. Only this stage can start a media query or attach a selector. |

```csharp
public interface IStyleBuilder<out T>                // every utility method
{
    T Background(Color color);
    T Color(Color color);
    T BorderColor(Color color);
    T P(int scale);
    T P(Length length);
    T Px(int scale);
    T Px(Length length);
    T Py(int scale);
    T Py(Length length);
    T Pt(int scale);
    T Pt(Length length);
    T Pr(int scale);
    T Pr(Length length);
    T Pb(int scale);
    T Pb(Length length);
    T Pl(int scale);
    T Pl(Length length);
    T M(int scale);
    T M(Length length);
    T Mx(int scale);
    T Mx(Length length);
    T My(int scale);
    T My(Length length);
    T Mt(int scale);
    T Mt(Length length);
    T Mr(int scale);
    T Mr(Length length);
    T Mb(int scale);
    T Mb(Length length);
    T Ml(int scale);
    T Ml(Length length);
    T MxAuto();
    T MyAuto();
    T Block();
    T InlineBlock();
    T Inline();
    T Flex();
    T Grid();
    T Hidden();
    T Relative();
    T Absolute();
    T Fixed();
    T Sticky();
    T Inset(int scale);
    T Inset(Length length);
    T Top(int scale);
    T Top(Length length);
    T Bottom(int scale);
    T Bottom(Length length);
    T Left(int scale);
    T Left(Length length);
    T Right(int scale);
    T Right(Length length);
    T Z(int index);
    T W(int scale);
    T W(Length length);
    T WFull();
    T WScreen();
    T WAuto();
    T H(int scale);
    T H(Length length);
    T HFull();
    T HScreen();
    T MaxW(Size size);
    T MaxW(Length length);
    T MinH(Size size);
    T MinH(Length length);
    T FlexRow();
    T FlexCol();
    T FlexWrap();
    T ItemsCenter();
    T ItemsStart();
    T ItemsEnd();
    T JustifyCenter();
    T JustifyBetween();
    T JustifyStart();
    T JustifyEnd();
    T Gap(int scale);
    T Gap(Length length);
    T GapX(int scale);
    T GapX(Length length);
    T GapY(int scale);
    T GapY(Length length);
    T Grow();
    T Shrink();
    T TextXs();
    T TextSm();
    T TextBase();
    T TextLg();
    T TextXl();
    T Text2Xl();
    T Text3Xl();
    T Text(Length size);
    T FontThin();
    T FontNormal();
    T FontBold();
    T FontSans();
    T FontSerif();
    T FontMono();
    T Font(FontFamily family);
    T Sans(params string[] families);
    T Serif(params string[] families);
    T Mono(params string[] families);
    T TextCenter();
    T TextLeft();
    T TextRight();
    T Underline();
    T NoUnderline();
    T Border(int width);
    T Border();
    T Rounded();
    T Rounded(Length radius);
    T RoundedSm();
    T RoundedMd();
    T RoundedLg();
    T RoundedFull();
    T Shadow();
    T ShadowSm();
    T ShadowMd();
    T ShadowLg();
    T Opacity(int val);
    T CursorPointer();
    T MinW(Size size);
    T MinW(Length length);
    T MaxH(Size size);
    T MaxH(Length length);
    T GridCols(int n);
    T GridRows(int n);
    T ColSpan(int n);
    T RowSpan(int n);
    T ColStart(int n);
    T OverflowAuto();
    T OverflowHidden();
    T OverflowScroll();
    T ObjectCover();
    T ObjectContain();
    T AspectSquare();
    T AspectVideo();
    T Italic();
    T NotItalic();
    T Uppercase();
    T Lowercase();
    T Capitalize();
    T WhitespaceNoWrap();
    T Leading(int scale);
    T Leading(Length length);
    T BorderSolid();
    T BorderDashed();
    T BorderDotted();
    T Ring(int width, Color c);
    T Outline(int width);
    T OutlineHidden();
    T Border(int width, Color c, BorderStyle style = BorderStyle.Solid);
    T Flex(FlexDirection direction, Justify? justify = null, Align? align = null, int? gap = null, FlexWrapValue? wrap = null);
    T InlineFlex();
    T InlineGrid();
    T FlowRoot();
    T Contents();
    T ListItem();
    T Static();
    T ZAuto();
    T FloatLeft();
    T FloatRight();
    T FloatNone();
    T ClearLeft();
    T ClearRight();
    T ClearBoth();
    T ClearNone();
    T Isolate();
    T IsolationAuto();
    T BoxBorder();
    T BoxContent();
    T Visible();
    T Invisible();
    T ObjectFill();
    T ObjectScaleDown();
    T ObjectNone();
    T Basis(int scale);
    T Basis(Length length);
    T BasisAuto();
    T BasisFull();
    T Order(int order);
    T OrderFirst();
    T OrderLast();
    T OrderNone();
    T Grow(int value);
    T Shrink(int value);
    T GridAutoFlowRow();
    T GridAutoFlowColumn();
    T GridAutoFlowRowDense();
    T GridAutoFlowColumnDense();
    T ColEnd(int n);
    T RowStart(int n);
    T RowEnd(int n);
    T JustifyItemsStart();
    T JustifyItemsCenter();
    T JustifyItemsEnd();
    T JustifyItemsStretch();
    T JustifySelfAuto();
    T JustifySelfStart();
    T JustifySelfCenter();
    T JustifySelfEnd();
    T JustifySelfStretch();
    T AlignContentStart();
    T AlignContentCenter();
    T AlignContentEnd();
    T AlignContentBetween();
    T AlignContentAround();
    T AlignContentEvenly();
    T AlignContentBaseline();
    T AlignContentStretch();
    T AlignSelfAuto();
    T AlignSelfStart();
    T AlignSelfCenter();
    T AlignSelfEnd();
    T AlignSelfBaseline();
    T AlignSelfStretch();
    T JustifyAround();
    T JustifyEvenly();
    T ItemsBaseline();
    T ItemsStretch();
    T BorderT();
    T BorderT(int width);
    T BorderR();
    T BorderR(int width);
    T BorderB();
    T BorderB(int width);
    T BorderL();
    T BorderL(int width);
    T BorderX();
    T BorderX(int width);
    T BorderY();
    T BorderY(int width);
    T BorderDouble();
    T OutlineColor(Color color);
    T OutlineOffset(int offset);
    T FontExtralight();
    T FontLight();
    T FontMedium();
    T FontSemibold();
    T FontExtrabold();
    T FontBlack();
    T LeadingNone();
    T LeadingTight();
    T LeadingSnug();
    T LeadingNormal();
    T LeadingRelaxed();
    T LeadingLoose();
    T TrackingTighter();
    T TrackingTight();
    T TrackingNormal();
    T TrackingWide();
    T TrackingWider();
    T TrackingWidest();
    T Tracking(Length length);
    T WhitespaceNormal();
    T WhitespacePre();
    T WhitespacePreLine();
    T WhitespacePreWrap();
    T WhitespaceBreakSpaces();
    T LineThrough();
    T Overline();
    T Truncate();
    T TextEllipsis();
    T TextClip();
    T TextIndent(int scale);
    T TextIndent(Length length);
    T WordBreakNormal();
    T WordBreakAll();
    T WordBreakKeepAll();
    T OverflowWrapNormal();
    T OverflowWrapBreakWord();
    T OverflowWrapAnywhere();
    T HyphensNone();
    T HyphensManual();
    T HyphensAuto();
    T ListStyleNone();
    T ListDisc();
    T ListDecimal();
    T ListInside();
    T ListOutside();
    T BgAttachmentFixed();
    T BgAttachmentLocal();
    T BgAttachmentScroll();
    T BgClipBorder();
    T BgClipPadding();
    T BgClipContent();
    T BgClipText();
    T BgOriginBorder();
    T BgOriginPadding();
    T BgOriginContent();
    T BgRepeat();
    T BgRepeatX();
    T BgRepeatY();
    T BgNoRepeat();
    T BgRepeatRound();
    T BgRepeatSpace();
    T BgCover();
    T BgContain();
    T BgAuto();
    T BgNone();
    T BorderCollapse();
    T BorderSeparate();
    T TableLayoutAuto();
    T TableLayoutFixed();
    T CaptionTop();
    T CaptionBottom();
    T PointerEventsNone();
    T PointerEventsAuto();
    T ResizeNone();
    T ResizeY();
    T ResizeX();
    T ResizeBoth();
    T ScrollBehaviorSmooth();
    T ScrollBehaviorAuto();
    T UserSelectNone();
    T UserSelectText();
    T UserSelectAll();
    T UserSelectAuto();
    T AppearanceNone();
    T AppearanceAuto();
    T AccentColor(Color color);
    T CaretColor(Color color);
    T ColorSchemeLight();
    T ColorSchemeDark();
    T ColorSchemeNormal();
    T CursorDefault();
    T CursorText();
    T CursorWait();
    T CursorMove();
    T CursorHelp();
    T CursorNotAllowed();
    T CursorCrosshair();
    T CursorGrab();
    T CursorGrabbing();
    T CursorNone();
    T CursorProgress();
    T TouchActionNone();
    T TouchActionAuto();
    T TouchActionPanX();
    T TouchActionPanY();
    T TouchActionManipulation();
    T WillChangeAuto();
    T WillChangeScroll();
    T WillChangeContents();
    T WillChangeTransform();
    T Fill(Color color);
    T Stroke(Color color);
    T StrokeWidth(int width);
    T Arbitrary(string className, string property, string value);
}
public interface IImportantStyleBuilder<out T> : IStyleBuilder<T>
{
    T Important(Action<IStyleScopeBuilder> styles);
}
public interface IStyleScopeBuilder : IStyleBuilder<IStyleScopeBuilder> { }
public interface IOnlyStyleBuilder : IImportantStyleBuilder<IOnlyStyleBuilder> { }
public interface IFluentCssBuilder : IImportantStyleBuilder<IFluentCssBuilder>
{
    IFluentCssBuilder Pseudo(string pseudoPrefix, Action<IOnlyStyleBuilder> pseudoBuilderAction);
}
public interface IMediaFluentCssBuilder : IFluentCssBuilder
{
    IMediaFluentCssBuilder Media(string query, string classPrefix, Action<IMediaFluentCssBuilder> cssBuilderAction);
}
public interface IRootFluentCssBuilder : IMediaFluentCssBuilder
{
    IRootFluentCssBuilder ApplySelector(string selector, Action<IOnlyStyleBuilder> cssBuilderAction);
}
```

### Selector interfaces

Built with `Selector.New()` or the `ApplySelector` selector overload; they produce a
selector instead of a utility class.

| Interface | Inherits | Purpose |
|---|---|---|
| `IBuildableSelector` | — | A finished selector/selector list; `Css` returns the text. |
| `IPseudoElementBuilder` | `IBuildableSelector` | Terminal pseudo-element stage. |
| `ICompoundSelectorBuilder` | `IPseudoElementBuilder` | Classes, ids, attributes, pseudo-classes and combinators. |
| `ITargetSelectorBuilder` | `ICompoundSelectorBuilder` | Adds `Element(tag)`. |
| `IRelativeSelectorBuilder` | `ITargetSelectorBuilder` | Target builder passed to `Has`; may start with a combinator. |
| `ISelectorContextBuilder` | `ITargetSelectorBuilder` | Can build a selector list via `Targets(...)`. |
| `IThemeSelectorBuilder` | `ISelectorContextBuilder` | Adds `Theme(name)` (`.theme-{name}` prefix). |
| `IRootSelectorBuilder` | `IThemeSelectorBuilder` | Entry point; adds `Media`, `Container`, `Supports` wrappers. |

```csharp
public interface IBuildableSelector { string Css { get; } }
public interface IPseudoElementBuilder : IBuildableSelector
{
    IBuildableSelector PseudoElement(string pseudoElement);
    ICompoundSelectorBuilder FunctionalPseudoElement(string name, string argument);
}
public interface ICompoundSelectorBuilder : IPseudoElementBuilder
{
    ICompoundSelectorBuilder Class(string className);
    ICompoundSelectorBuilder Id(string id);
    ICompoundSelectorBuilder Universal();
    ICompoundSelectorBuilder Attribute(string name);
    ICompoundSelectorBuilder Attribute(string name, string value);
    ICompoundSelectorBuilder Attribute(string name, string value, AttributeOperator op, AttributeCase casing = AttributeCase.Default);
    ICompoundSelectorBuilder Pseudo(string pseudo);
    ICompoundSelectorBuilder Pseudo(string pseudo, string argument);
    ICompoundSelectorBuilder Not(params Func<ITargetSelectorBuilder, IBuildableSelector>[] selectors);
    ICompoundSelectorBuilder Is(params Func<ITargetSelectorBuilder, IBuildableSelector>[] selectors);
    ICompoundSelectorBuilder Where(params Func<ITargetSelectorBuilder, IBuildableSelector>[] selectors);
    ICompoundSelectorBuilder Has(params Func<IRelativeSelectorBuilder, ICompoundSelectorBuilder>[] selectors);
    ITargetSelectorBuilder Descendant();
    ITargetSelectorBuilder Child();
    ITargetSelectorBuilder AdjacentSibling();
    ITargetSelectorBuilder GeneralSibling();
}
public interface ITargetSelectorBuilder : ICompoundSelectorBuilder
{
    ICompoundSelectorBuilder Element(string tag);
}
public interface IRelativeSelectorBuilder : ITargetSelectorBuilder { }
public interface ISelectorContextBuilder : ITargetSelectorBuilder
{
    IBuildableSelector Targets(params Func<ITargetSelectorBuilder, IBuildableSelector>[] targets);
}
public interface IThemeSelectorBuilder : ISelectorContextBuilder
{
    ISelectorContextBuilder Theme(string theme);
}
public interface IRootSelectorBuilder : IThemeSelectorBuilder
{
    IThemeSelectorBuilder Media(string query);
    IThemeSelectorBuilder Container(string query);
    IThemeSelectorBuilder Supports(string query);
}
```

### Entry point and abstract base

```csharp
public static class FluentCssBuilder
{
    public static IRootFluentCssBuilder Css { get; }        // no theme prefix
    public static IRootFluentCssBuilder Dark { get; }       // theme-dark: prefix
    public static IRootFluentCssBuilder Theme(string theme); // theme-{theme}: prefix
}

public abstract class BaseStyleBuilder<T> : IImportantStyleBuilder<T>
{
    // Assembly-internal base: implements IStyleBuilder<T> once for all builders;
    // its constructor is private protected, so do not subclass it from an app.
}
```

Extension classes add the typed conveniences on top of these interfaces:
`StyleBuilderPseudoExtensions` (on `IFluentCssBuilder`),
`MediaFluentCssBuilderExtensions` (on `IMediaFluentCssBuilder`),
`RootFluentCssBuilderInterfaceExtensions` (on `IRootFluentCssBuilder`), and
`SelectorBuilderExtensions` (on the selector interfaces).

## Getting the CSS output

```csharp
string css      = ClassRegistry.DefaultRegistry.Css;        // full stylesheet
byte[] utf8     = ClassRegistry.DefaultRegistry.CssUtf8;    // UTF-8 bytes
string hash     = ClassRegistry.DefaultRegistry.Sha256Hex;  // content hash
```
In Blazor, `<CssStyles />` injects this automatically and re-renders when new classes register.

## Where to put style code

- Keep reusable class strings in static `*Styles` classes so they leave the render path.
- Register classes **before** the layout renders when the stylesheet link is written by the tag helper.
- Classes registered later (AJAX partials, client-rendered components) are picked up by the injected script / `CssStyles` subscription — don't refresh manually.

## Common mistakes

| Symptom | Cause / fix |
|---|---|
| `<CssStyles />` not found | Add `@using FluentCss` or use `<FluentCss.CssStyles />`. |
| `<fluent-css />` renders nothing | Register it: `@addTagHelper *, FluentCss`. |
| Theme class not on `<html>` (server-rendered) | Call `builder.Services.AddFluentCss()`; otherwise the client script still applies it. |
| Inline builder chain on a component fails to compile | Precompute the string and pass a variable. |
| CSS missing in a Blazor Web App | Move `<CssStyles />` inside the interactive render boundary. |
| No CSS at all in MVC / Razor Pages | Reference both packages and call `app.UseFluentCss()`. |

## Reference

- [README](README.md) · [missing features](missing-features.md)
- Docs: [Installation](https://fluentcss.stanac.dev/docs/installation) ·
  [Utility classes](https://fluentcss.stanac.dev/docs/utility-classes) ·
  [Interfaces](https://fluentcss.stanac.dev/docs/interfaces) ·
  [Selectors](https://fluentcss.stanac.dev/docs/selectors) ·
  [Responsive](https://fluentcss.stanac.dev/docs/responsive) ·
  [Dark mode](https://fluentcss.stanac.dev/docs/dark-mode) ·
  [Extensibility](https://fluentcss.stanac.dev/docs/extensibility)
