# FluentCss

**Utility-first CSS builder for .NET** — generate Tailwind-inspired utility CSS classes on-the-fly from C# code.

---

## Overview

FluentCss lets you build CSS utility classes directly in `.razor`/`.cshtml` files using a fluent, chainable API. Instead of writing CSS by hand, you compose styles with type-safe method calls — and FluentCss automatically generates the CSS and class names.

- **Fluent builder API** — chain methods like `.Background(Color.Red500).Color(Color.White).P(4)`
- **Full color palette** — 22 color families (red, blue, green, slate, etc.), 11 shades each, plus black/white (taken from TailwindCss)
- **Pseudo-classes** — `Hover(...)`, `Focus(...)`, `Active(...)`, and custom `Pseudo("...", ...)`
- **Theming / dark mode** — `FluentCssBuilder.Dark` or `FluentCssBuilder.Theme("dark")`
- **Responsive design** — `MinMd(...)`, `MaxMd(...)`, and custom `Media(...)` queries
- **Automatic CSS output** — classes are deduplicated and the final CSS is available as Css, build artifact or Razor component
- **Blazor/Razor integration** — drop `<CssStyles />` into your layout and get live CSS injection

## Packages

| Package | Target | Description |
|---------|--------|-------------|
| **FluentCss.Core** | `netstandard2.0` | Core builder, registry, and all CSS utilities |
| **FluentCss** | `net10.0` | Blazor Razor component `<CssStyles>` for auto-rendering CSS |

## Installation

```bash
dotnet add package FluentCss.Core
dotnet add package FluentCss         # if using Blazor
```

## Quick Start

### 1. Blazor (Razor Components)


Add the following directives to your `_Imports.razor` for a cleaner syntax:

```razor
@using static FluentCss.Core.FluentCssBuilder
@using FluentCss.Core
```

Add `<CssStyles />` to your layout (e.g., `App.razor` or `MainLayout.razor`):

```razor
<CssStyles />
```

**Important!!: during development place `<CssStyles />` at the end of the layout file!**

Optionally, include the Tailwind CSS reset (preflight) to normalize browser defaults:

```razor
<link rel="stylesheet" href="_content/FluentCss/preflight.css" />
```
Preflight is licensed by Tailwind under MIT license.

Then build classes anywhere in your components using the shorthand `@Css` and `@Dark`:

```razor
<p class="
    @Css.Background(Color.Slate400).Color(Color.Green400)
        .Hover(h => h.Background(Color.Black).Color(Color.White))
    @Css.MinMd(md => md.Color(Color.Green800)
        .Hover(h => h.Color(Color.Gray400)))
    @Dark.Background(Color.Black).Color(Color.White)
        .Hover(h => h.Background(Color.Red300).Color(Color.Red800))">
    Hello, world!
</p>
```

The generated HTML class attribute will be something like:
```
bg-slate-400 text-green-400 hover:bg-black hover:text-white max-md:text-green-800 max-md:hover:text-gray-400 dark:bg-black dark:text-white dark:hover:bg-red-300 dark:hover:text-red-800
```

And FluentCss automatically emits the corresponding CSS into the page.

Alternativly you can place styles inside a class:

```csharp
using FluentCss.Core;
using static FluentCss.Core.FluentCssBuilder;

public static class MyStyles
{
    public static string MyElement { get; } =
        $"""
         {Css.Background(Color.Slate400).Color(Color.Green400).Hover(h => h.Background(Color.Black).Color(Color.White))}
         {Css.MinMd(md => md.Color(Color.Green800).Hover(h => h.Color(Color.Gray400)))}
         {Dark.Background(Color.Black).Color(Color.White).Hover(h => h.Background(Color.Red300).Color(Color.Red800))}
         """;
}
```

and use it as 

```razor:
<p class="@MyStyles.MyElement">
    Test
</p>
```

> **⚠️ Important:** The inline `@Css.Chain().Method()` syntax only works on **HTML elements** (`<div>`, `<p>`, `<a>`, etc.).
> **Blazor components** (`<NavLink>`, `<InputText>`, etc.) do not support complex C# expressions in their attributes.
> For component attributes, precompute the class string in a `@code` block or a static class (as shown above) and pass a simple variable:
>
> ```razor
> @* ❌ Does NOT work on components *@
> <NavLink class="@Css.Background(Color.Blue500).Color(Color.White)">...</NavLink>
>
> @* ✅ Works on components *@
> <NavLink class="@NavLinkClass">...</NavLink>
>
> @code {
>     private static string NavLinkClass = Css.Background(Color.Blue500).Color(Color.White).ToString();
> }
> ```

### 2. Non-Blazor / Any .NET App

```csharp
using FluentCss.Core;

// Create a root builder (registers classes in the default registry)
var classes = FluentCssBuilder.Css
    .Background(Color.Emerald500)
    .Color(Color.White)
    .Flex(FlexDirection.Row, Justify.Between, Align.Center)
    .Px(6).Py(3)
    .RoundedMd();

// Get the class string for your HTML
string classAttr = classes.ToString();
// => "bg-emerald-500 text-white flex flex-row justify-between items-center px-6 py-3 rounded-md"

// Get the auto-generated CSS
string css = ClassRegistry.DefaultRegistry.Css;
```

## Builder API

### Entry Points

```csharp
FluentCssBuilder.Css         // standard builder
FluentCssBuilder.Dark        // dark theme builder (prefix: "dark:")
FluentCssBuilder.Theme(name) // custom theme builder (prefix: "theme-{name}:")
```

### Layout

| Method | CSS |
|--------|-----|
| `.Block()` | `display: block` |
| `.InlineBlock()` | `display: inline-block` |
| `.Inline()` | `display: inline` |
| `.Flex()` | `display: flex` |
| `.Grid()` | `display: grid` |
| `.Hidden()` | `display: none` |
| `.Relative()` | `position: relative` |
| `.Absolute()` | `position: absolute` |
| `.Fixed()` | `position: fixed` |
| `.Sticky()` | `position: sticky` |
| `.Inset(n)` | `inset` (n × 0.25rem) |
| `.Top(n)` / `.Bottom(n)` / `.Left(n)` / `.Right(n)` | positional offsets |
| `.Z(n)` | `z-index` |

### Flexbox

```csharp
// Individual methods
.FlexRow()        .FlexCol()
.FlexWrap()       .ItemsCenter()     .ItemsStart()     .ItemsEnd()
.JustifyCenter()  .JustifyBetween()  .JustifyStart()   .JustifyEnd()
.Gap(n)           .GapX(n)           .GapY(n)
.Grow()           .Shrink()

// Composite method
.Flex(FlexDirection.Row, Justify.Center, Align.Center, gap: 4)
```

`FlexDirection`: `Row`, `RowReverse`, `Col`, `ColReverse`  
`Justify`: `Start`, `End`, `Center`, `Between`, `Around`, `Evenly`  
`Align`: `Start`, `End`, `Center`, `Baseline`, `Stretch`  
`FlexWrapValue`: `Wrap`, `NoWrap`, `WrapReverse`

### Grid

```csharp
.GridCols(n)      .GridRows(n)
.ColSpan(n)       .RowSpan(n)
.ColStart(n)
```

### Spacing

Spacing scale: `n × 0.25rem`. E.g., `P(4)` = `1rem`.

| Method | CSS |
|--------|-----|
| `.P(n)` / `.Px(n)` / `.Py(n)` | `padding` / `padding-inline` / `padding-block` |
| `.Pt(n)` / `.Pr(n)` / `.Pb(n)` / `.Pl(n)` | directional padding |
| `.M(n)` / `.Mx(n)` / `.My(n)` | `margin` / `margin-inline` / `margin-block` |
| `.Mt(n)` / `.Mr(n)` / `.Mb(n)` / `.Ml(n)` | directional margin |
| `.MxAuto()` / `.MyAuto()` | auto margins |

### Sizing

```csharp
.W(n)       .WFull()    .WScreen()    .WAuto()
.H(n)       .HFull()    .HScreen()
.MaxW(Size.Md)    .MinW(Size.Sm)
.MaxH(Size.Lg)    .MinH(Size.Zero)
```

Predefined sizes: `Size.Sm`, `Size.Md`, `Size.Lg`, `Size.Xl`, `Size._2Xl`, `Size._3Xl`, `Size.Full`, `Size.ScreenH`, `Size.ScreenW`, `Size.Zero`, `Size.Min`, `Size.Max`, `Size.Fit`.

### Typography

```csharp
.TextXs()    .TextSm()    .TextBase()
.TextLg()    .TextXl()    .Text2Xl()    .Text3Xl()
.FontThin()  .FontNormal()  .FontBold()
.TextCenter() .TextLeft() .TextRight()
.Underline() .NoUnderline()
.Italic()    .NotItalic()
.Uppercase() .Lowercase() .Capitalize()
.Leading(n)  .WhitespaceNoWrap()
```

### Colors

```csharp
.Background(Color.Red500)    // background-color
.Color(Color.White)           // color
.BorderColor(Color.Slate200) // border-color
```

22 color families × 11 shades (50–950) + `Black` and `White`.  
Examples: `Color.Red50`, `Color.Blue700`, `Color.Emerald400`, `Color.Slate950`.

### Borders

```csharp
.Border()           // 1px solid
.Border(2)          // 2px width
.Border(2, Color.Slate300, BorderStyle.Dashed)
.BorderSolid()  .BorderDashed()  .BorderDotted()
.Rounded()     .RoundedSm()   .RoundedMd()   .RoundedLg()   .RoundedFull()
.Ring(2, Color.Blue200)        // ring (box-shadow based outline)
.Outline(2)    .OutlineHidden()
```

### Effects & Utilities

```csharp
.Shadow()   .ShadowMd()   .ShadowLg()
.Opacity(50)          // 0-100 → opacity: 0.5
.CursorPointer()
.OverflowAuto()  .OverflowHidden()  .OverflowScroll()
.ObjectCover()   .ObjectContain()
.AspectSquare()  .AspectVideo()
```

### Pseudo-classes

```csharp
.Hover(h => h.Background(Color.Blue600))
.Focus(f => f.Ring(2, Color.Blue300))
.Active(a => a.Background(Color.Blue700))
.Pseudo("nth-child(2)", x => x.Background(Color.Gray100))
```

### Media Queries (responsive)

```csharp
FluentCssBuilder.Css
    .Background(Color.Red500)
    .MinMd(m => m.Background(Color.Blue500))    // @media (width > 48rem)
    .MaxMd(m => m.Background(Color.Green500));  // @media (width < 48rem)
```

Custom media queries:

```csharp
.Media("min-width: 1024px", "lg", builder =>
{
    builder.Flex(FlexDirection.Row);
    builder.P(8);
});
```

## Theming / Dark Mode

```csharp
// Use the dark theme entry point
string darkClasses = FluentCssBuilder.Dark
    .Background(Color.Slate800)
    .Color(Color.Slate100)
    .ToString();
// => "dark:bg-slate-800 dark:text-slate-100"

// Or inline theming
string themeClasses = FluentCssBuilder.Theme("halloween")
    .Background(Color.Orange500)
    .ToString();
// => "theme-halloween:bg-orange-500"
```

## Getting the CSS Output

The `ClassRegistry` aggregates all utility classes and generates the final CSS:

```csharp
string css = ClassRegistry.DefaultRegistry.Css;
```

In Blazor, the `<CssStyles />` component subscribes to `ClassRegistry.DefaultRegistry.OnClassAdded` and automatically updates the rendered `<style>` block in real time. It also implements `IDisposable` to clean up the event subscription.

## How It Works

1. You call fluent builder methods (e.g., `.Background(Color.Red500)`)
2. Each call creates a `UtilityClass` instance (CSS property + value + generated class name)
3. The class is added to a `ClassRegistry` — duplicates are ignored
4. The registry builds a CSS string automatically
5. `.ToString()` on the builder returns the space-separated class names for your `class` attribute

### Class Name Format

```
[media-prefix]:[theme]:[pseudo]:{utility}
```

Examples:
- `bg-red-500` — plain utility
- `hover:bg-red-600` — hover pseudo
- `dark:text-white` — dark theme
- `max-md:bg-black` — responsive (max-md media prefix)
- `dark:hover:bg-slate-700` — combined theme + pseudo

## License

[BSD Zero Clause License](https://opensource.org/licenses/0BSD) (0BSD) — a public-domain-equivalent license with no restrictions.

Color values ~~adapted~~ stolen from [Tailwind CSS](https://tailwindcss.com) (MIT licensed).

## Contributing

Issues and pull requests are welcome! By submitting a pull request, you agree that your contribution will be licensed under the [BSD Zero Clause License](https://opensource.org/licenses/0BSD) (0BSD).

The project structure:

```
FluentCss/
├── src/
│   ├── FluentCss.Core/       # Core library (netstandard2.0)
│   └── FluentCss.Blazor/     # Blazor component (net10.0)
├── tests/
│   └── FluentCss.Core.Tests/ # Unit tests
└── test-apps/
    ├── FluentCssBlazorSsr/             # Documentation site (static SSR)
    └── FluentCssBlazorWasmStandalone/  # Documentation site (WASM standalone)
```
