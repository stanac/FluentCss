# Missing features (Tailwind parity)

FluentCss implements a focused subset of Tailwind CSS as a fluent C# builder.
This document is a current-state checklist of where FluentCss still trails
Tailwind. It lives outside the documentation pages on purpose: the docs only
describe features that exist today, while everything here is a gap or a note
about parity.

## Parity, not a clone

Tailwind is a reference point, not a specification. FluentCss does not have to
mirror Tailwind one-to-one. Where a different shape makes more sense for a
strongly-typed C# API, FluentCss deliberately does it differently, and a missing
capability only matters when the capability itself is useful.

- **Class names are an implementation detail.** The builder generates and
  deduplicates class tokens; their spelling is not part of the public contract.
  `mt--4` instead of Tailwind's `-mt-4` is fine. Matching Tailwind's class
  strings is not a goal.
- **The C# API may be shaped differently.** A separate, typed call is often
  clearer than Tailwind's modifier syntax. Example: opacity is a standalone
  `Opacity(n)` utility rather than Tailwind's per-color `bg-red-500/50` modifier.
  Such a difference is a design choice, not automatically a gap.
- **Capability is what counts.** Treat an entry as a real gap when the CSS
  effect cannot be expressed at all through the typed API (setting `Arbitrary` /
  `ApplySelector` aside), not merely when it is spelled differently from
  Tailwind.

The families and names below still follow Tailwind because that makes a
convenient index. Read the tags with the above in mind.

Legend:

- **[missing]** – the whole category or utility family is absent.
- **[partial]** – something exists, but the coverage is a subset of Tailwind.
- **[not a bug]** – works today; only the class-name spelling differs from Tailwind.
- **[runtime]** / **[conflict]** / **[mutable]** – architecture notes, not utilities.

---

## Not gaps (often assumed missing)

These work today and should not be listed as gaps. They are called out because
the surrounding gaps are easy to misread as "unsupported".

- **Important modifier.** `Css.Important(c => c.P(4))` yields class `!p-4` and
  `.\!p-4 { padding: 1rem !important }`. It composes with theme, media, hover and
  `Arbitrary`. Only the `!` *prefix* form is implemented; the Tailwind v4 `!`
  *suffix* form (`p-4!`) is not. That is a cosmetic parity choice, not a
  functional gap.
- **[not a bug] Negative values.** Pass a negative `int` (`Mt(-4)` →
  `margin-top: -1rem`) or a `Length` (`Length.Rem(-1)`). The only difference from
  Tailwind is the generated class token (`mt--4` vs `-mt-4`); class names are an
  internal detail.
- **Base layer customization.** `IRootFluentCssBuilder.ApplySelector(...)` (and
  `ApplyClass` / `ApplyId`) adds global rules. The raw-string overload can even
  emit an at-rule such as `@layer base { ... }`.
- **Selector builder.** `Selector.New()` builds selectors that are not tied to a
  utility class: attribute selectors (presence, `=`, `~=`, `|=`, `^=`, `$=`,
  `*=`, plus `i` / `s` case flags), `Data(...)` / `Aria(...)` sugar,
  `:not` / `:is` / `:where` / `:has`, pseudo-elements, all four combinators, and
  typed pseudo-class helpers (`Hover`, `FocusVisible`, `Checked`, `Disabled`,
  `First`, `Odd`, `NthChild(n|formula)`, `NthLastChild(...)`, `NthOfType(...)`,
  `Lang(...)`, …). `Pseudo(...)` remains the escape hatch. It is reachable from
  the main builder with
  `Css.ApplySelector(s => s.Class("btn").Hover(), css => css.Px(4).Py(2))`.
- **Typed pseudo-class variants.** `Css.FocusVisible(x => x.Outline(2))`,
  `Css.Checked(...)`, `Css.Disabled(...)`, `Css.Odd(...)`,
  `Css.NthChild(2, ...)`, `Css.Lang("en", ...)` and the rest of the set generate
  class variants (`focus-visible:outline-2`) through the single-pseudo model and
  compose with theme, media and `!important`. `Pseudo(...)` remains the escape
  hatch for pseudo-classes without a helper.
- **Container / supports selectors.** `Selector.New().Container(query)` and
  `.Supports(query)` are implemented; they wrap whatever selector follows in
  `@container` / `@supports`.
- **Content-hashed stylesheet.** `ClassRegistry.Sha256Hex` backs
  `/fluentcss/{hash}.css`. `FluentCssMiddleware` serves it with an `ETag` and,
  when the hash matches, `public, max-age=31536000, immutable`;
  `FluentCssTagHelper` emits that hashed link. The stylesheet is still generated
  at runtime (not a build artifact), but long-lived caching is done.
- **Named sizes.** `Size.Xxl` is 96rem (`2xl`) and `Size.Xxxl` is 128rem
  (`3xl`); they are correct.

---

## Core concepts and variants

- **[missing] Variant groups.** Compound-selector variants are not supported:
  no `group-hover`, `peer-focus`, `group-*`, `peer-*`.
- **[partial] Data / ARIA / attribute variants.** The underlying *selectors* are
  supported by `SelectorBuilder` (`Attribute(...)` with all operators and the
  `i` / `s` flag, plus `Data(...)` / `Aria(...)` sugar), but there is no
  utility-level variant surface: `IFluentCssBuilder` only offers pseudo-class
  variants, so you cannot write a `data-*` / `aria-*` / `supports-*` variant that
  generates a class. They are reachable only as standalone rules through
  `ApplySelector`, for example:
  ```csharp
  Css.ApplySelector(
      s => s.Attribute("data-state", "open").Class("bg-red-500"),
      css => css.Background(Color.Red500));
  // => [data-state="open"].bg-red-500 { background-color: … }
  ```
- **[partial] Functional selector-list pseudo variants.** `:not` / `:is` /
  `:where` / `:has` are available as selector-builder methods (`Not(...)`,
  `Is(...)`, `Where(...)`, `Has(...)`) and through `ApplySelector`, but there are
  no matching class variants on `IFluentCssBuilder`. (The simple structural
  pseudos — `First`, `Last`, `Odd`, `Even`, `NthChild(...)`, `NthOfType(...)` —
  *are* available as class variants.)
- **[partial] Pseudo-class coverage.** The typed helpers cover the commonly used
  pseudo-classes on both `IFluentCssBuilder` (class variants) and
  `ICompoundSelectorBuilder` (selectors): `hover`, `focus`, `active`,
  `focus-visible`, `focus-within`, `visited`, `target`, `open`, `popover-open`,
  `modal`, `fullscreen`, `picture-in-picture`, the form states (`disabled`,
  `enabled`, `checked`, `indeterminate`, `default`, `required`, `optional`,
  `valid`, `invalid`, `in-range`, `out-of-range`, `read-only`, `read-write`,
  `placeholder-shown`, `autofill`, `user-valid`, `user-invalid`) and the
  structural / functional set (`root`, `empty`, `first`, `last`, `only`, `odd`,
  `even`, `first-of-type`, `last-of-type`, `only-of-type`, `defined`,
  `nth-child(...)`, `nth-last-child(...)`, `nth-of-type(...)`,
  `nth-last-of-type(...)`, `lang(...)`, `dir(...)`). Missing typed helpers (all
  still reachable through `Pseudo(...)`, so a convenience gap rather than a
  capability gap): `:link` / `:any-link` (notably absent next to `Visited`),
  `:scope`, the shadow-DOM / custom-element pair `:host` / `:host(...)` /
  `:host-context(...)` and `:state(...)`, and the draft / low-support
  `:local-link`, `:blank`, `:target-within`, `:nth-col()` / `:nth-last-col()`,
  `:current` / `:past` / `:future`, the media-element states (`:playing`,
  `:paused`, `:seeking`, `:buffering`, `:stalled`, `:muted`,
  `:volume-locked`) and `:active-view-transition` /
  `:active-view-transition-type(...)` / `:has-slotted`. The nth `of S` form
  (`:nth-child(2 of .item)`) has no typed overload; the `string` overload only
  passes it through. `Lang(...)` / `Dir(...)` take a raw string without
  validating the argument (`:dir` should be `ltr` / `rtl`).
- **Pseudo-element variants.** `IFluentCssBuilder.Pseudo(name, ...)` decides the
  colon count from the name: known pseudo-elements get a double colon
  (`::placeholder`), everything else a single colon (`:hover`); a leading
  `:` / `::` forces the shape. The typed `Before`, `After`, `Placeholder`,
  `Marker`, `Selection`, `File`, `Backdrop`, `FirstLetter` and `FirstLine`
  helpers wrap it. `SelectorBuilder` keeps its explicit `PseudoElement(...)` /
  `FunctionalPseudoElement(...)` methods.
- **[partial] Container queries.** The `@container` selector wrapper exists
  (`Selector.New().Container(query)`). There is no utility-level `@container` /
  `@min-*` / `@max-*` variant family.
- **[partial] Dark mode strategies.** Only an ancestor class theme
  (`theme-dark`, `FluentCssBuilder.Dark`, `FluentCssTheme.DarkClass`) is built in.
  A `prefers-color-scheme` strategy is expressible with
  `Media("(prefers-color-scheme: dark)", …)`, but there is no built-in media or
  data-attribute dark strategy.
- **[missing] Motion and print variants.** No `motion-safe`, `motion-reduce`,
  `print`.
- **[missing] Direction variants.** No `rtl` / `ltr`, and no logical-property
  variants.
- **[missing] Direct-child class variants.** No `*` / `**` child combinator
  variants. (`Child()`, `Descendant()`, `AdjacentSibling()` and
  `GeneralSibling()` exist on the selector builder.)
- **[missing] Combinable variants.** Only one media, one theme and one pseudo can
  be combined, and there are no arbitrary/custom variants. In particular two
  pseudo-classes cannot be chained (`hover:focus:`) through the typed helpers,
  because `UtilityClass` holds a single `Pseudo` string and the pseudo scope
  passed to `Pseudo(string, Action<IOnlyStyleBuilder>)` cannot nest another
  variant. The raw escape hatch does produce a working selector today:
  `Css.Pseudo("hover:focus", c => c.Outline(2))` yields the class
  `hover:focus:outline-2` and `.hover\:focus\:outline-2:hover:focus { … }`, but
  there is no typed surface for it and no test locking it in.

---

## Base styles

- **[partial] Preflight.** The bundled reset is Tailwind CSS v3.2.4, not the v4
  preflight, and it is not wired to theme variables.

---

## Layout

- **`container`.** `Container()` (extension on `IMediaFluentCssBuilder`) applies
  full width plus the Tailwind max-widths at each breakpoint. Add `MxAuto()` to
  center it.
- **[missing]** `columns`
- **[missing]** `break-after`, `break-before`, `break-inside`
- **[missing]** `box-decoration-break`
- **[missing]** `object-position`
- **[missing]** `overscroll-behavior`
- **[partial] display.** `block`, `inline-block`, `inline`, `flex`,
  `inline-flex`, `grid`, `inline-grid`, `flow-root`, `contents`, `list-item`
  and `none` are implemented. Missing the `table` family.
- **[partial] position.** `relative`, `absolute`, `fixed`, `sticky` and
  `static`; missing inset logical variants.
- **[partial] inset / top / right / bottom / left.** Numeric spacing scale plus
  arbitrary lengths via `Length` (negatives included); missing dedicated `auto`,
  `full` and fraction helpers (all still expressible with a `Length`).
- **[partial] aspect-ratio.** Only `square` (1/1) and `video` (16/9); missing
  `3/2`, `4/3`, `21/9`, arbitrary values and custom properties.

---

## Flexbox and grid

- **`flex` shorthand.** `Flex1()`, `FlexAuto()`, `FlexInitial()` and
  `FlexNone()` emit the `flex` shorthand.
- **[partial] grid-auto-flow / grid-auto-columns / grid-auto-rows.**
  `GridAutoFlowRow`, `GridAutoFlowColumn`, `GridAutoFlowRowDense` and
  `GridAutoFlowColumnDense` are implemented; missing `grid-auto-columns`,
  `grid-auto-rows` and arbitrary tracks.
- **[missing]** `place-content`, `place-items`, `place-self`
- **[partial] flex-grow / flex-shrink.** `Grow()` / `Shrink()` (value `1`) plus
  `Grow(n)` / `Shrink(n)`; the `flex` shorthands are covered by `Flex1()` /
  `FlexAuto()` / `FlexInitial()` / `FlexNone()`.
- **[partial] grid-template-columns / grid-template-rows.** `GridCols(n)` and
  `GridRows(n)` only emit `repeat(n, minmax(0, 1fr))`; missing named lines,
  subgrid, arbitrary tracks and `none`.
- **[partial] grid-column / grid-row.** `ColSpan`, `RowSpan`, `ColStart`,
  `ColEnd`, `RowStart` and `RowEnd`; missing `auto` and arbitrary placement.
- **[partial] gap.** Numeric spacing scale plus arbitrary lengths via
  `Gap(Length)` / `GapX` / `GapY`; missing a `gap-px` shorthand (use
  `Gap(Length.Px(1))`).

---

## Spacing

- **`space-x` / `space-y`.** `SpaceX(int|Length)` / `SpaceY(int|Length)` style
  the siblings through a `> :not([hidden]) ~ :not([hidden])` selector suffix.
  There is no `space-*-reverse`.
- **`divide-x` / `divide-y`.** `DivideX()` / `DivideX(int)` / `DivideY()` /
  `DivideY(int)` plus `DivideColor(Color)`. Divide styles (`divide-dashed`, ...)
  are still missing.
- **[missing]** logical properties: `ps`, `pe`, `pbs`, `pbe`, `ms`, `me`,
  `mbs`, `mbe`
- Padding (`p`, `px`, `py`, `pt`, `pr`, `pb`, `pl`) and margin
  (`m`, `mx`, `my`, `mt`, `mr`, `mb`, `ml`, auto) are implemented.

---

## Sizing

- **[missing]** `size-*`
- **[missing]** logical properties: `inline-size`, `min-inline-size`,
  `max-inline-size`, `block-size`, `min-block-size`, `max-block-size`
- **[partial] width / height.** Numeric scale plus `full`, `screen`, `auto`,
  `fit` (`WFit` / `HFit`), `min-content` (`WMin` / `HMin`), `max-content`
  (`WMax` / `HMax`) and arbitrary lengths via `W(Length)` / `H(Length)`; missing
  dedicated fraction and `svh` / `lvh` / `dvh` helpers (still expressible with a
  `Length`).
- **[partial] min/max width & height.** The predefined `Size` set plus any
  `Length` (arbitrary values/units, including `none`/`auto`/`min`/`max`/`fit`
  passed as a `Length`); missing a dedicated numeric scale and per-axis keyword
  helpers.

---

## Typography

- **[partial] font-family.** Built-in `font-sans` / `font-serif` / `font-mono`
  (Tailwind default stacks), custom stacks via `Font(FontFamily)` and prepend
  helpers `Sans(...)` / `Serif(...)` / `Mono(...)` (the class token includes a
  short hash of the custom families); missing the theme-variable form
  (`--font-*`).
- **[missing]** `font-smoothing`
- **[missing]** `font-stretch`
- **[missing]** `font-variant-numeric`
- **[missing]** `font-feature-settings`
- **[missing]** `line-clamp`
- **[partial] list-style.** `list-style-type` (`ListStyleNone`, `ListDisc`,
  `ListDecimal`) and `list-style-position` (`ListInside`, `ListOutside`) are
  implemented; missing `list-style-image`.
- **text-decoration.** `DecorationColor(Color)`, `Decoration(int thickness)`,
  `DecorationSolid` / `DecorationDouble` / `DecorationDotted` /
  `DecorationDashed` / `DecorationWavy` and `UnderlineOffset(int)`.
- **[missing]** `text-wrap` (`balance`, `pretty`, `nowrap`, `wrap`)
- **[missing]** `tab-size`
- **`vertical-align`.** `AlignBaseline`, `AlignTop`, `AlignMiddle`,
  `AlignBottom`, `AlignTextTop`, `AlignTextBottom`, `AlignSub`, `AlignSuper`.
- **[missing]** `content`
- **[partial] font-size.** Named scale `text-xs`…`text-9xl` plus arbitrary
  sizes via `Text(Length)`; missing per-size line heights (the `Text*` helpers
  set `font-size` only).
- **[partial] font-weight.** All standard weights (`100`–`900`) are implemented;
  missing arbitrary values (use `Arbitrary`).
- **[partial] text-transform / font-style / text-align / decoration.** Common
  values plus `LineThrough`, `Overline` and `TextEnd`; missing `normal` variants
  for every axis and `text-align: justify` / `start`.

---

## Backgrounds

- **[missing]** `background-image` (linear/radial/conic gradients)
- **[missing]** `background-position`
- `background-color` via `Background(Color)`, `background-attachment`,
  `background-clip`, `background-origin`, `background-repeat` and
  `background-size` are implemented (`BgAttachment*`, `BgClip*`, `BgOrigin*`,
  `BgRepeat*`, `BgCover` / `BgContain` / `BgAuto`), plus `BgNone`. No opacity
  modifier or gradients.

---

## Borders

- **[missing]** per-side border styles and colors
- **[partial] border radius.** Named `RoundedSm`…`Rounded3Xl` and `RoundedFull`,
  arbitrary `Rounded(Length)`, plus per-side (`RoundedT/R/B/L`) and per-corner
  (`RoundedTL/TR/BR/BL`) with default and `Length` overloads. Logical corners
  (`rounded-ss`, …) are missing.
- **`outline-style`.** `OutlineSolid`, `OutlineDashed`, `OutlineDotted`,
  `OutlineDouble`, `OutlineNone` (plus the existing `Outline(width)`,
  `OutlineHidden()`, `OutlineColor`, `OutlineOffset`).
- **`divide-*`.** `DivideX()` / `DivideX(int)` / `DivideY()` / `DivideY(int)`
  plus `DivideColor(Color)`. Divide styles (`divide-dashed`, ...) are still
  missing.
- **[partial] border-style.** `solid`, `dashed`, `dotted` and `double` (and
  `none` through the `Border(width, color, BorderStyle)` composite overload).
- **[partial] outline.** `Outline(width)`, `OutlineHidden()`, `OutlineColor`,
  `OutlineOffset` and the `outline-style` helpers.
- **[partial] ring.** `Ring(width, Color)` and `Ring(width)`; missing
  `ring-inset`, `ring-offset-*`, ring color-only utilities and composable
  shadows. See also the box-shadow conflict note below.

---

## Effects

- **[missing]** `text-shadow`
- **[missing]** `mix-blend-mode`
- **[missing]** `background-blend-mode`
- **[missing]** all `mask-*` utilities (`mask-clip`, `mask-composite`,
  `mask-image`, `mask-mode`, `mask-origin`, `mask-position`, `mask-repeat`,
  `mask-size`, `mask-type`)
- **[partial] box-shadow.** A small fixed set (`shadow-sm`, `shadow`,
  `shadow-md`, `shadow-lg`); missing the named color/shadow scales and shadow
  colors.
- **[partial] opacity.** Only global `Opacity(n)`; no per-color opacity.

---

## Filters

- **[partial] filter utilities.** `Blur` (all sizes), `Brightness`, `Contrast`,
  `DropShadow` (all sizes), `Grayscale`, `HueRotate`, `Invert`, `Saturate`,
  `Sepia` and `FilterNone` are implemented. They each set the `filter` property
  directly, so they do not compose (stacking two filters keeps the globally
  first-registered rule).
- **[missing]** all `backdrop-filter` equivalents.

---

## Tables

- **[missing]** `border-spacing`

---

## Transitions and animation

- **[partial] transition utilities.** `Transition`, `TransitionNone`,
  `TransitionAll`, `TransitionColors`, `TransitionOpacity`, `TransitionShadow`,
  `TransitionTransform`, `Duration`, `Delay` and `EaseLinear` / `EaseIn` /
  `EaseOut` / `EaseInOut`. The `Transition*` helpers use the `transition`
  shorthand (property + 150ms + timing) in one class.
- **[missing]** `transition-behavior`
- **[missing]** `animation` (`animate-*`) and `@keyframes` support

---

## Transforms

- **[partial] transforms.** `Rotate`, `Scale`, `ScaleX` / `ScaleY`,
  `TranslateX` / `TranslateY`, `SkewX` / `SkewY`, `TransformNone`,
  `TransformGpu` and the `Origin*` helpers (including `OriginTop`). Each sets
  the `transform` property directly, so multiple transforms do not compose
  (stacking keeps the globally first-registered rule).
- **[missing]** `backface-visibility`
- **[missing]** `perspective`, `perspective-origin`
- **[missing]** `transform-style`
- **[missing]** `zoom`

---

## Interactivity

- **[missing]** `field-sizing`
- **[missing]** `scrollbar-color`, `scrollbar-width`, `scrollbar-gutter`
- **[missing]** `scroll-margin`, `scroll-padding`
- **[missing]** `scroll-snap-align`, `scroll-snap-stop`, `scroll-snap-type`

---

## SVG

`fill`, `stroke` and `stroke-width` are implemented (`Fill(Color)`,
`Stroke(Color)`, `StrokeWidth(int)`).

---

## Architecture and runtime gaps

These are not Tailwind utilities, but they limit how closely FluentCss can match
Tailwind's behavior in practice.

- **[runtime] Global static registry.** `ClassRegistry.DefaultRegistry` is
  process-wide and never resets. `ClassRegistry` is a public, instantiable type,
  but every public builder entry point (`FluentCssBuilder.Css` / `Dark` /
  `Theme`) still hard-wires `DefaultRegistry`, and `InternalRootFluentCssBuilder`
  is internal. Per-request isolation is therefore not reachable from the public
  API, and the default registry grows unbounded in a long-running server.
- **[runtime] Runtime-generated stylesheet.** CSS is assembled per process and
  served from the registry instead of being emitted as a static asset. The
  content-hashed `/fluentcss/{hash}.css` URL, `ETag` and immutable caching are
  implemented, but the bytes are still produced at runtime rather than as a build
  artifact.
- **[runtime] No class name prefixing / scoping.** You cannot namespace the
  generated classes for embedding in an existing design system.
- **[conflict] `box-shadow` collisions.** `Shadow*` and `Ring` both set the
  `box-shadow` property. They cannot be composed; whichever utility was
  registered first (globally) wins for every element using either class.
- **[conflict] Duplicate base utilities.** Two base utilities that target the
  same CSS property (for example two `Background(...)` calls on one element) are
  emitted as separate rules, and the globally first-registered one wins instead
  of the value written last on the element. Variant rules (pseudo, theme, media)
  are emitted after base rules, so one base value plus one variant value behaves
  as expected.
- **[partial] Canonical ordering.** `ClassRegistry` emits base → pseudo → theme →
  theme+pseudo buckets and sorts media rules (max descending, min ascending,
  unknowns last). Property/category ordering *within* a bucket is still missing,
  which is why the duplicate base-utility and `Ring` / `Shadow` conflicts remain.
- **[mutable] Cached class name.** `UtilityClass.ClassName` lazily caches into a
  mutable field, so a utility instance is not purely immutable on first read.
- **[partial] Raw CSS escape hatch.** `Arbitrary(className, property, value)`
  attaches arbitrary CSS declarations, and `ApplySelector` attaches arbitrary
  rules/at-rules one call at a time; there is still no way to attach a full
  stylesheet through the builder.

---

## Highest-value remaining gaps

1. **Non-pseudo selector variants** (data/ARIA/attribute, `supports-*`,
   pseudo-elements, functional `:not` / `:is` / `:where` / `:has`). Needs the
   `UtilityClass` selector-model change so `[attr]`, `::before`, at-rules and
   compound selectors can be class variants, plus variant stacking.
2. **Canonical property ordering within `ClassRegistry` buckets.** Unblocks
   transforms / filters / transitions and fixes the `Ring` / `Shadow` conflict
   and duplicate base-utility behavior.
3. **Rename projects.** Done: `FluentCss.Blazor` → `FluentCss` (Blazor
   component, MVC/Razor Pages tag helpers and static assets) and
   `FluentCss.MvcRazor` → `FluentCss.Middleware` (middleware and the
   `AddFluentCss()` / `UseFluentCss()` extensions). The tag helpers live in
   `FluentCss` next to `CssStyles`. The ASP.NET Core framework reference they
   need is marked `PrivateAssets="all"`, and the theme tag helper reads the
   request through `IHttpContextAccessor` instead of an MVC `ViewContext`, so the
   assembly keeps no `Microsoft.AspNetCore.Mvc.*` reference. That stops the MVC
   application-parts discovery from flagging it and breaking Blazor WebAssembly
   consumers. Users can still choose the middleware and script-link path in
   server-hosted Blazor projects.