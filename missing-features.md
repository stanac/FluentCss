# Missing features (Tailwind parity)

FluentCss implements a focused subset of Tailwind CSS as a fluent C# builder.
This document is a parity checklist: it lists what Tailwind offers that FluentCss
does **not** yet provide.

It intentionally lives outside the documentation pages. The docs only describe
features that exist today; anything below is a gap.

Legend:

- **[missing]** – the whole category or utility family is absent.
- **[partial]** – something exists, but the coverage is a subset of Tailwind.

---

## Core concepts

- **[missing] Important modifier.** No `!` prefix/suffix.
- **[missing] Negative values.** No `-mt-4`, `-left-2`, etc.
- **[missing] Opacity modifier on colors.** No `bg-red-500/50` or
  `text-white/75`; opacity is only available globally via `.Opacity(n)`.
- **[missing] Variant groups.** No `group-hover`, `peer-focus`, `has-*`.
- **[missing] Data / ARIA / attribute variants.** No `data-*`, `aria-*`,
  `supports-*`, `open`, `target`, `checked`, `disabled`, `indeterminate`, etc.
- **[missing] Structural pseudo variants.** No `first`, `last`, `odd`, `even`,
  `first-of-type`, `nth-*` helpers (only the raw `Pseudo(...)` escape hatch).
- **[missing] Pseudo-element variants.** No `before`, `after`, `placeholder`,
  `marker`, `selection`, `file`.
- **[missing] State variants beyond hover/focus/active.** No `focus-visible`,
  `focus-within`, `visited`, `disabled`.
- **[missing] Motion and print variants.** No `motion-safe`, `motion-reduce`,
  `print`.
- **[missing] Direction variants.** No `rtl` / `ltr`, and no logical-property
  variants.
- **[missing] Direct-child variant.** No `*` / `**` child combinator variants.
- **[missing] Container queries.** No `@container` support and no
  `@min-*` / `@max-*` container variants.
- **[missing] Dark mode strategies.** Only an ancestor class theme
  (`theme-dark`) is supported. There is no `prefers-color-scheme` media
  strategy and no data-attribute strategy.

---

## Base styles

- **[partial] Preflight.** The bundled reset is Tailwind CSS v3.2.4, not the v4
  preflight, and it is not wired to theme variables.
- **[missing] Base layer customization.** There is no `@layer base` equivalent
  and no way to add global base styles through the builder.

---

## Layout

- **[missing]** `columns`
- **[missing]** `break-after`, `break-before`, `break-inside`
- **[missing]** `box-decoration-break`
- **[missing]** `box-sizing`
- **[missing]** `float`, `clear`
- **[missing]** `isolation`
- **[missing]** `object-position`
- **[missing]** `overscroll-behavior`
- **[missing]** `visibility`
- **[partial] display.** Only `block`, `inline-block`, `inline`, `flex`,
  `grid`, `none`. Missing `inline-flex`, `inline-grid`, `table` family,
  `contents`, `flow-root`, `list-item`, etc.
- **[partial] position.** Missing `static` (and no `inset` logical variants).
- **[partial] inset / top / right / bottom / left.** Numeric spacing scale plus
  arbitrary lengths via `Length`; missing dedicated `auto`, `full`, fraction and
  negative helpers (all still expressible with a `Length`).
- **[partial] z-index.** Only integers; missing `auto` and negatives.
- **[partial] aspect-ratio.** Only `square` (1/1) and `video` (16/9); missing
  `3/2`, `4/3`, `21/9`, arbitrary values and custom properties.
- **[partial] object-fit.** Only `cover` and `contain`; missing `fill`,
  `scale-down`, `none`.

---

## Flexbox and grid

- **[missing]** `flex-basis`
- **[missing]** `flex` shorthand (`flex-1`, `flex-auto`, `flex-initial`,
  `flex-none`)
- **[missing]** `order`
- **[missing]** `grid-auto-flow`, `grid-auto-columns`, `grid-auto-rows`
- **[missing]** `justify-items`, `justify-self`
- **[missing]** `align-content`, `align-self`
- **[missing]** `place-content`, `place-items`, `place-self`
- **[partial] flex-grow / flex-shrink.** Only `Grow()` and `Shrink()` (value
  `1`); missing `0`, arbitrary values and the `flex` shorthands.
- **[partial] grid-template-columns / grid-template-rows.** `GridCols(n)` and
  `GridRows(n)` only emit `repeat(n, minmax(0, 1fr))`; missing named lines,
  subgrid, arbitrary tracks and `none`.
- **[partial] grid-column / grid-row.** Only `span` (`ColSpan`, `RowSpan`) and
  `ColStart`; missing end lines, `auto` and arbitrary placement.
- **[partial] justify-content / align-items.** Standalone helpers cover only a
  subset (`Start`, `End`, `Center`, and `Between` for justify). The rest
  (`Around`, `Evenly`, `Baseline`, `Stretch`) are reachable only through the
  composite `Flex(...)` call.
- **[partial] gap.** Numeric spacing scale plus arbitrary lengths via
  `Gap(Length)` / `GapX` / `GapY`; missing a `gap-px` shorthand (use
  `Gap(Length.Px(1))`).

---

## Spacing

- **[missing]** `space-x` / `space-y` (space between children)
- **[missing]** `divide-x` / `divide-y` (and divide colors/styles)
- **[missing]** logical properties: `ps`, `pe`, `pbs`, `pbe`, `ms`, `me`,
  `mbs`, `mbe`
- **[missing]** negative spacing values
- Padding (`p`, `px`, `py`, `pt`, `pr`, `pb`, `pl`) and margin
  (`m`, `mx`, `my`, `mt`, `mr`, `mb`, `ml`, auto) are implemented.

---

## Sizing

- **[missing]** `size-*`
- **[missing]** logical properties: `inline-size`, `min-inline-size`,
  `max-inline-size`, `block-size`, `min-block-size`, `max-block-size`
- **[partial] width / height.** Numeric scale plus `full`, `screen`, `auto` and
  arbitrary lengths via `W(Length)` / `H(Length)`; missing dedicated fraction,
  `min-content` / `max-content` / `fit-content` and `svh` / `lvh` / `dvh`
  helpers (still expressible with a `Length`).
- **[partial] min/max width & height.** The predefined `Size` set plus any
  `Length` (arbitrary values/units, including `none`/`auto`/`min`/`max`/`fit`
  passed as a `Length`); missing a dedicated numeric scale and per-axis keyword
  helpers.

---

## Typography

- **[partial] font-family.** Built-in `font-sans` / `font-serif` / `font-mono`
  (Tailwind default stacks), custom stacks via `Font(FontFamily)` and prepend
  helpers `Sans(...)` / `Serif(...)` / `Mono(...)`; missing the theme-variable
  form (`--font-*`).
- **[missing]** `font-smoothing`
- **[missing]** `font-stretch`
- **[missing]** `font-variant-numeric`
- **[missing]** `font-feature-settings`
- **[missing]** `letter-spacing` (tracking)
- **[missing]** `line-clamp`
- **[missing]** `list-style-image`, `list-style-position`, `list-style-type`
- **[missing]** text-decoration color, style, thickness and underline offset
- **[missing]** `text-overflow`
- **[missing]** `text-wrap` (`balance`, `pretty`, `nowrap`, `wrap`)
- **[missing]** `text-indent`
- **[missing]** `tab-size`
- **[missing]** `vertical-align`
- **[missing]** `word-break`
- **[missing]** `overflow-wrap`
- **[missing]** `hyphens`
- **[missing]** `content`
- **[partial] font-size.** Fixed seven-step scale (`text-xs`…`text-3xl`) plus
  arbitrary sizes via `Text(Length)`; missing the wider named scale and
  per-size line heights.
- **[partial] line-height.** Numeric `Leading(n)` scale plus arbitrary lengths
  via `Leading(Length)`; missing the named `leading-none` / `tight` / `snug` /
  `normal` / `relaxed` / `loose` helpers.
- **[partial] white-space.** Only `nowrap`; missing `normal`, `pre`, `pre-line`,
  `pre-wrap`, `break-spaces`.
- **[partial] font-weight.** Only `100`, `400`, `700`; missing the other
  weights and arbitrary values.
- **[partial] text-transform / font-style / text-align / decoration.** Only the
  common values, no overline/line-through or `normal` variants for every axis.

---

## Backgrounds

- **[missing]** `background-attachment`
- **[missing]** `background-clip`
- **[missing]** `background-image` (linear/radial/conic gradients)
- **[missing]** `background-origin`
- **[missing]** `background-position`
- **[missing]** `background-repeat`
- **[missing]** `background-size`
- Only `background-color` via `Background(Color)` is implemented (arbitrary
  colors via the `Color(name, value)` constructor); no opacity modifier or
  gradients.

---

## Borders

- **[missing]** per-side border widths (`border-t`, `border-r`, `border-b`,
  `border-l`, `border-x`, `border-y`)
- **[missing]** per-side border styles and colors
- **[missing]** per-corner / logical border radius (`rounded-t`, `rounded-l`,
  `rounded-ss`, …)
- **[missing]** `outline-color`
- **[missing]** `outline-style`
- **[missing]** `outline-offset`
- **[missing]** `divide-*` utilities
- **[partial] border-style.** Only `solid`, `dashed`, `dotted` (and `none`
  through the composite overload).
- **[partial] outline.** Only `OutlineWidth` and `OutlineHidden`.
- **[partial] ring.** A single hard-coded `box-shadow` ring; missing
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

- **[missing]** all `filter` utilities: `blur`, `brightness`, `contrast`,
  `drop-shadow`, `grayscale`, `hue-rotate`, `invert`, `saturate`, `sepia`.
- **[missing]** all `backdrop-filter` equivalents.

---

## Tables

- **[missing]** `border-collapse`
- **[missing]** `border-spacing`
- **[missing]** `table-layout`
- **[missing]** `caption-side`

---

## Transitions and animation

- **[missing]** `transition-property`
- **[missing]** `transition-behavior`
- **[missing]** `transition-duration`
- **[missing]** `transition-timing-function`
- **[missing]** `transition-delay`
- **[missing]** `animation` (`animate-*`) and `@keyframes` support

---

## Transforms

- **[missing]** `backface-visibility`
- **[missing]** `perspective`, `perspective-origin`
- **[missing]** `rotate`
- **[missing]** `scale`
- **[missing]** `skew`
- **[missing]** `transform`, `transform-origin`, `transform-style`
- **[missing]** `translate`
- **[missing]** `zoom`

---

## Interactivity

- **[missing]** `accent-color`
- **[missing]** `appearance`
- **[missing]** `caret-color`
- **[missing]** `color-scheme`
- **[missing]** `field-sizing`
- **[missing]** `pointer-events`
- **[missing]** `resize`
- **[missing]** `scroll-behavior`
- **[missing]** `scrollbar-color`, `scrollbar-width`, `scrollbar-gutter`
- **[missing]** `scroll-margin`, `scroll-padding`
- **[missing]** `scroll-snap-align`, `scroll-snap-stop`, `scroll-snap-type`
- **[missing]** `touch-action`
- **[missing]** `user-select`
- **[missing]** `will-change`
- **[partial] cursor.** Only `CursorPointer()`; missing the full cursor set.

---

## SVG

- **[missing]** `fill`
- **[missing]** `stroke`
- **[missing]** `stroke-width`

---

## Architecture and runtime gaps

These are not Tailwind utilities, but they limit how closely FluentCss can match
Tailwind's behavior in practice.

- **[broken] Container query API.** `IRootFluentCssBuilder.ContainerInlineSizeName`
  and `ContainerSizeName` are declared and throw `NotImplementedException`.
- **[runtime] Global static registry.** `ClassRegistry.DefaultRegistry` is
  process-wide and never resets. Utilities registered while rendering one
  request remain visible to every other request, which grows unbounded in a
  long-running server and makes per-request CSS isolation impossible.
- **[runtime] Runtime-generated stylesheet.** CSS is assembled per process and
  served from the registry instead of being emitted as a static asset, so there
  is no content-hashed stylesheet for long-lived caching.
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
- **[mutable] Cached class name.** `UtilityClass.ClassName` lazily caches into a
  mutable field, so a utility instance is not purely immutable on first read.
- **[partial] Raw CSS escape hatch.** `Arbitrary(className, property, value)`
  attaches arbitrary CSS declarations; there is still no way to attach a full
  stylesheet through the builder.
- **[missing] Variant constraints.** Only one media, one theme and one pseudo
  can be combined, and there are no arbitrary/custom variants.

---

## Implementation roadmap (working notes)

Notes captured for planning. Nothing here is committed work; it is a proposed
ordering of the gaps above, grouped by how they interact with the current
architecture (`BaseStyleBuilder` → `UtilityClass` → `ClassRegistry`).

### Tier 1 — correctness and architecture (do first)

These are not new features. They make the existing API trustworthy and they
unblock the families in Tier 3 that need to compose a shared CSS property.

- **[conflict] Per-element property resolution / canonical utility ordering.**
  Give every utility a stable sort key (category → property) so base rules are
  emitted in a deterministic, Tailwind-like order instead of registration order.
  This is the real fix for the `box-shadow` collision, duplicate base utilities,
  and future composing families (transforms, filters, transitions).
  - Alternative for `Ring` / `Shadow`: make them compose through CSS custom
    properties (`--tw-ring-*`, `--tw-shadow`) with one shared `box-shadow` rule.
- **[runtime] Instance registry + scoped builder.** Add a registry-instance path
  and a builder entry point that accepts a `ClassRegistry`, so Blazor
  (`CssStyles.razor`) and MVC (`FluentCssMiddleware`) can isolate per request /
  content hash. Keep `DefaultRegistry` as the default for compatibility.
- **[bug] Negative values.** The `int` overloads accept negatives but produce
  broken class names (`GetSpacingValue(-4)` → class `m--4`). Add proper negative
  helpers (`-mt-4`, `-inset-2`, `-z-10`) and negative `inset` / `z` / spacing.

### Tier 2 — small, high-value parity wins

- **[missing] Opacity modifier on colors.** Colors are `oklch(...)`, so
  `Background(Color.Red500, 50)` can emit `oklch(... / 0.5)` and class
  `bg-red-500/50`. Apply to `Color`, `Background`, `BorderColor`, `Ring` and the
  shadow colors. Biggest missing "feel" feature.
- **[missing] Pseudo-element variants.** `UtilityClass` currently strips colons
  and always emits a single `:`, so `::before` is not expressible. Carry the
  full pseudo token so `before`, `after`, `placeholder`, `marker`, `selection`,
  `file` work.
- **[missing] More state + structural pseudo variants.** Thin `Pseudo(...)`
  wrappers: `FocusVisible`, `FocusWithin`, `Visited`, `Disabled`, `Checked`,
  `First`, `Last`, `Odd`, `Even`, `Nth(n)`.
- **Mechanical utility gaps.** `size-*`; `inline-flex` / `inline-grid` /
  `contents` / `flow-root` / table family; `Static()`; `Order`; `FlexBasis`;
  `flex-1/auto/initial/none`; `AlignSelf` / `JustifySelf`; `AlignContent`;
  `Place*`; grid auto-flow / auto rows-cols; `col-end` / `row-start` /
  `row-end`; named `Leading*`; full `Whitespace*`; all font weights;
  `Tracking`; `TextWrap`.

### Tier 3 — whole missing families (self-contained features)

- Transforms: rotate, scale, translate, skew, transform-origin, perspective
  (needs the Tier 1 composition fix).
- Transitions & animation: duration, timing, delay, property, `@keyframes`.
- Filters / backdrop-filters: blur, brightness, contrast, grayscale, … (shared
  `filter` property, same composition issue).
- Backgrounds: gradients (linear/radial/conic), size/position/repeat/
  attachment/clip/origin.
- Tables and SVG: `border-collapse`, `border-spacing`, `fill`, `stroke`,
  `stroke-width`.

### Tier 4 — complex variant system

- **[broken] Container queries.** Implement the throwing stubs and add
  `@container` / `@min-*` / `@max-*` variants. Good candidate: the interface and
  stubs already exist.
- `group-*`, `peer-*`, `has-*` — compound selectors (`.group:hover .x`), not a
  pseudo suffix, so they need a selector-model extension in `UtilityClass`.
- Data / ARIA / attribute variants, `motion-safe` / `motion-reduce`, `print`,
  `rtl` / `ltr` + logical properties, dark-mode media strategy.

### Candidate next PR

1. **Correctness first (recommended):** canonical utility ordering + fix
   `Ring` / `Shadow` composition, negative values, `Size` bug. Makes the current
   API trustworthy and unblocks transforms/filters.
2. **Feature first:** opacity color modifiers + negative values + pseudo-element
   and state variants. Large, immediately visible parity jump; touches mostly
   `BaseStyleBuilder` and `UtilityClass`.

Avoid starting with `group-*` / container queries until the selector model and
registry ordering are settled — those changes depend on them.
