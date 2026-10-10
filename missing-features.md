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

## Core concepts and tooling

- **[missing] Theme variables / design tokens.** There are no CSS custom
  properties and no `@theme` equivalent, so palettes, spacing, radii and fonts
  cannot be re-themed at runtime.
- **[missing] Configuration and customization.** There is no config file or API
  to extend or replace the color palette, spacing scale, breakpoints, fonts or
  container sizes.
- **[missing] Functions and directives.** No `@apply`, `@layer`, `@utility`,
  `@variant`, `@custom-variant`, `@source`, `@config`, `@plugin`, `--alpha()`,
  `--spacing()` or similar.
- **[missing] Detecting classes in source files.** There is no content scanner
  and no on-demand JIT compilation. Utilities are emitted at runtime by the
  builder, not extracted from markup.
- **[missing] Arbitrary values and properties.** No `p-[13px]`,
  `bg-[#123456]` or `[mask-type:luminance]` syntax.
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
- **[partial] Responsive breakpoints.** Only a single `md` breakpoint
  (`48rem`) is built in. Other breakpoints require the manual
  `Media(query, prefix, ...)` helper; there are no named `sm`, `lg`, `xl`,
  `2xl` helpers.
- **[missing] Container queries.** No `@container` support and no
  `@min-*` / `@max-*` container variants.
- **[missing] Dark mode strategies.** Only an ancestor class theme
  (`theme-dark`) is supported. There is no `prefers-color-scheme` media
  strategy and no data-attribute strategy.
- **[missing] Editor setup / IntelliSense.** No VS Code/Visual Studio extension
  or language server for class completion.
- **[missing] Build tooling.** No CLI, PostCSS plugin, Vite plugin or
  standalone build/extraction step.

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
- **[partial] inset / top / right / bottom / left.** Only the numeric spacing
  scale; missing `auto`, `full`, fractions and negative values.
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
- **[partial] gap.** Only the numeric spacing scale; no arbitrary lengths,
  `gap-px` or custom properties.

---

## Spacing

- **[missing]** `space-x` / `space-y` (space between children)
- **[missing]** `divide-x` / `divide-y` (and divide colors/styles)
- **[missing]** logical properties: `ps`, `pe`, `pbs`, `pbe`, `ms`, `me`,
  `mbs`, `mbe`
- **[missing]** negative spacing values
- **[missing]** arbitrary values and custom properties
- Padding (`p`, `px`, `py`, `pt`, `pr`, `pb`, `pl`) and margin
  (`m`, `mx`, `my`, `mt`, `mr`, `mb`, `ml`, auto) are implemented.

---

## Sizing

- **[missing]** `size-*`
- **[missing]** logical properties: `inline-size`, `min-inline-size`,
  `max-inline-size`, `block-size`, `min-block-size`, `max-block-size`
- **[partial] width / height.** Only the spacing scale plus `full`, `screen`
  and `auto`; missing fractions, `min-content`, `max-content`, `fit-content`,
  `svh`/`lvh`/`dvh`, and arbitrary values.
- **[partial] min/max width & height.** Only the predefined `Size` set; no
  numeric scale, no `none`/`auto`/`min`/`max`/`fit` on every axis, no
  arbitrary values.
- **[bug/parity] Named sizes.** `Size._2Xl` and `Size._3Xl` both resolve to
  `96rem` and generate the class prefixes `2l` and `3l` (instead of `2xl` and
  `3xl`).

---

## Typography

- **[missing]** `font-family` (`font-sans`, `font-serif`, `font-mono`)
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
- **[partial] font-size.** Fixed seven-step scale (`text-xs`…`text-3xl`);
  missing the wider named scale, arbitrary sizes and per-size line heights.
- **[partial] line-height.** Only the numeric `Leading(n)` scale; missing
  `leading-none`, `tight`, `snug`, `normal`, `relaxed`, `loose` and arbitrary
  values.
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
- Only `background-color` via `Background(Color)` is implemented, with no
  opacity modifier, arbitrary colors or gradients.

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
- **[runtime] No CSS extraction for production.** CSS is generated at runtime,
  so there is no build-time tree-shaking and no static stylesheet artifact.
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
- **[missing] Raw CSS escape hatch.** There is no way to attach arbitrary CSS
  declarations or a stylesheet through the builder.
- **[missing] Variant constraints.** Only one media, one theme and one pseudo
  can be combined, and there are no arbitrary/custom variants.
