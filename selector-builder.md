Discussion between LLMs and me about a feature we are building.

----

Working on a Css selector builder for c#

I want user to be able to write calls fluentry

```
Selector
	.Media(string)
	.Theme(string) // this is part of the wider library, it will only prepend theme like `.dark [rest of the selector]`
	.Class(string)
	.Pseudo(string)
```

User can also skip media and theme and use only target and pseudo.

Since this is css format:

 [@media <query> {] [.theme-x ] <target>[:<pseudo>] { declarations } [}]
 
First interface can have media and inherit second interface

interface IRootSelectorBuilder: IThemeSelectorBuilder
{
	IThemeSelectorBuilder Media(string),
	// extension methods for Min(string), Max(string), Min(Lenght), Max(Lenght), MinXs(), ...
}

Media method returns IThemeSelectorBuilder because I don't want user to be able to select theme again in the same chain.

interface IThemeSelectorBuilder: ITargetSelectorBuilder
{
	ITargetSelectorBuilder Theme(string)
	ITargetSelectorBuilder Dark // shortcut for Theme("dark")
}

// theme is just a class at the top prefixed, like ".dark [rest of the selector]"

interface ITargetSelectorBuilder: IPseudoSelectorBuilder
{
	IPseudoSelectorBuilder Class,
	IPseudoSelectorBuilder Element,
	IPseudoSelectorBuilder Id,
}

IPseudoSelectorBuilder
{
	Pseudo(string)
	// with helper extension methods like Hover, etc...
}

How would you add support for `:not`, attributes present, attributes equal, etc... ?
Also, how could I support multiple target selectors? Like have same media, same theme, but target to class names?

I don't need exact code, just structure of interfaces, inheritance, etc...

---
To achieve this cleanly without locking the user into a rigid single-method path (e.g., allowing multiple classes, attributes, and pseudos on the same element), we need to model CSS selector concepts:

1. **At-Rules / Scoping:** Media Queries & Theme.
2. **Compound Selector:** Elements, Classes, Attributes, and Pseudo-classes (`:not`, `:hover`) can all belong to the *same* target.
3. **Selector List (`comma`):** Multiple targets sharing the same Media/Theme.

Here is the recommended structural design:

---

### 1. The Core Lifecycle Stages

Instead of strictly moving from `Target -> Pseudo` (which prevents chaining multiple classes like `.btn.primary` or mixing attributes and classes), use a **Compound Selector** model where targets can accumulate qualifications:

```text
[IRootSelectorBuilder]
        │
        ├── Media() ────────► [IThemeSelectorBuilder]
        │                             │
(skips) │                     Theme() │ (skips)
        ▼                             ▼
   ┌─────────────────────────────────────┐
   │       ITargetSelectorBuilder        │ ◄───┐ (Loop via .Or() / .Also())
   │   (Elements, IDs)                   │     │
   └─────────────────────────────────────┘     │
        │                                      │
        ▼                                      │
   ┌─────────────────────────────────────┐     │
   │      ICompoundSelectorBuilder       │ ────┤
   │  - Class, Attribute, Pseudo, Not    │     │
   │  - Returns self to allow chaining   │     │
   └─────────────────────────────────────┘     │
        │                                      │
        ├── PseudoElement (::before, ::after) ─┘
```

---

### 2. Interface Structure

#### Stage 1: Context (Media & Theme)

```csharp
// Entry point
public interface IRootSelectorBuilder : IThemeSelectorBuilder
{
    IThemeSelectorBuilder Media(string query);
}

public interface IThemeSelectorBuilder : ITargetSelectorBuilder
{
    ITargetSelectorBuilder Theme(string theme);
    ITargetSelectorBuilder Dark => Theme("dark");
}
```

---

#### Stage 2: Target & Modifiers (Attributes, `:not`, etc.)

*Note: In CSS, you can have only one element tag (e.g. `div`), and it must be first. But you can have unlimited classes, attributes, and pseudo-classes in any order.*

```csharp
// Starting an element/target
public interface ITargetSelectorBuilder : ICompoundSelectorBuilder
{
    ICompoundSelectorBuilder Element(string tag);
    ICompoundSelectorBuilder Id(string id);
}

// Chaining modifiers on that same target
public interface ICompoundSelectorBuilder : IPseudoElementBuilder
{
    // Multiple classes (.btn.primary)
    ICompoundSelectorBuilder Class(string className);

    // Attributes
    ICompoundSelectorBuilder Attribute(string name);
    ICompoundSelectorBuilder Attribute(string name, AttributeOperator op, string value);
    // Shortcut extensions: AttributeEquals("type", "submit"), AttrContains(...), etc.

    // Standard pseudo-classes
    ICompoundSelectorBuilder Pseudo(string pseudo);
    // Shortcut extensions: Hover(), Focus(), Disabled(), etc.

    // Functional pseudo-classes: :not, :is, :has
    // Using a nested builder delegate allows full selector composition inside :not(...)
    ICompoundSelectorBuilder Not(Func<ITargetSelectorBuilder, ICompoundSelectorBuilder> inner);
    ICompoundSelectorBuilder Has(Func<ITargetSelectorBuilder, ICompoundSelectorBuilder> inner);
}

// Pseudo-elements (::before, ::after) must come at the very end
public interface IPseudoElementBuilder
{
    IBuildableSelector PseudoElement(string pseudoElement); 
    // Shortcut extensions: Before(), After()
}
```

---

### 3. Solving Specific Requirements

#### A. Handling `:not(...)` and Functional Pseudos
The most flexible way to handle `:not` is via a **builder delegate (`Func<...>`)**. This lets users pass any valid compound or target selector:

```csharp
// Fluent usage:
Selector
    .Class("button")
    .Not(x => x.Class("disabled"))              // .button:not(.disabled)
    .Not(x => x.Attribute("data-state", "hidden"))  // .button:not([data-state="hidden"])
```
*Behind the scenes:* The delegate receives a fresh nested builder and wraps it in `:not(...)`.

#### B. Handling Attributes
Attributes live directly inside `ICompoundSelectorBuilder` returning `this` so they can be chained freely:

```csharp
// Fluent usage:
Selector
    .Element("input")
    .Attribute("required")                            // input[required]
    .AttrEquals("type", "text")                  // input[required][type="text"]
    .Class("form-control")                       // input[required][type="text"].form-control
    .Hover();
```

#### C. Handling Multiple Targets (Selector Lists sharing Media & Theme)
If you want:
`@media (min-width: 768px) { .dark .btn, .dark .card { ... } }`

There are two clean API patterns for this:

##### Option 1: Fluent Delimiter (`.Or()` or `.Also()`)
Add an `.Or()` / `.Also()` method on `ICompoundSelectorBuilder` that saves the current target into an internal list, resets the target pointer, and returns `ITargetSelectorBuilder`:

```csharp
public interface ICompoundSelectorBuilder
{
    // ...
    // Finalizes the current selector and starts another target 
    // under the SAME Media & Theme context
    ITargetSelectorBuilder Or(); // Or .Also()
}
```
*Fluent usage:*
```csharp
Selector
    .Media("screen and (min-width: 768px)")
    .Dark
    .Class("btn").Hover()
    .Or()
    .Class("card")
    .Build();

// Output:
// @media screen and (min-width: 768px) {
//    .dark .btn:hover, .dark .card { ... }
// }
```

##### Option 2: Lambda Grouping (`.Targets(...)`)
Alternatively, provide a branch at the Theme/Root level taking multiple target configurations:

```csharp
public interface IThemeSelectorBuilder
{
    // ...
    IBuildableSelector Targets(params Func<ITargetSelectorBuilder, ICompoundSelectorBuilder>[] targets);
}
```
*Fluent usage:*
```csharp
Selector
    .Dark
    .Targets(
        t => t.Class("btn").Hover(),
        t => t.Class("card")
    );
```

### Summary of Benefits
* **No trapped paths:** Since `ICompoundSelectorBuilder` returns itself, users can chain `.Class().Attribute().Class().Pseudo()` in any order.
* **Separation of Concerns:** Media and Theme are defined only at the top level and automatically distributed to all comma-separated targets.
* **Extensible `:not`:** Passing a nested builder into `:not()` keeps the syntax clean and allows reusing the exact same selector API for `:is()`, `:where()`, and `:has()`.

----

Additionally

To build a selector builder that feels complete and handles modern CSS without hitting dead ends, here are the architectural and syntax gaps you will encounter:

---

### 1. Relative Selectors (Crucial for `:has`)
Combinators (`>`, `+`, `~`, ` `) normally sit between two targets (`A > B`). However, **relative selectors** allow a selector to *start* with a combinator without a preceding target.

*   **CSS Example:** `:has(> img)`, `:has(+ .title)`
*   **Interface Impact:** Your inner builder (passed into `:has()`) cannot demand an element or class first. It must allow starting directly with a combinator:
    ```csharp
    // Needs to support starting with child ('>') directly
    t.Has(rel => rel.Child().Element("img"))
    ```

---

### 2. Multi-Argument Lists for `:is()`, `:where()`, and `:not()`
In modern CSS (Selectors Level 4), `:is()`, `:where()`, and `:not()` accept **selector lists** (comma-separated), not just a single selector.

*   **CSS Example:** `:is(h1, h2, h3)`, `:not(.active, .disabled)`
*   **Interface Impact:** The parameter should accept multiple selectors (via `params` or selector lists):
    ```csharp
    ICompoundSelectorBuilder Is(params Func<ITargetSelectorBuilder, IBuildableSelector>[] selectors);
    ICompoundSelectorBuilder Where(params Func<ITargetSelectorBuilder, IBuildableSelector>[] selectors);
    ICompoundSelectorBuilder Not(params Func<ITargetSelectorBuilder, IBuildableSelector>[] selectors);
    ```

---

### 3. Structural Pseudo-Classes with Formulae & Filters
Simple string-based pseudos (like `:hover`) don't fit indexed selectors:

*   **Formulas:** `:nth-child(2n + 1)`, `:nth-last-child(...)`, `:nth-of-type(...)`.
*   **The Modern `of <selector>` Syntax (Selectors L4):**
    *   CSS: `:nth-child(2 of .featured)` (targets the 2nd `.featured` item, not just the 2nd child overall).
*   **Interface Impact:**
    ```csharp
    // Standard:
    NthChild(int n)
    NthChild(string formula) // "2n+1", "odd", "even"

    // Modern 'of' filter:
    NthChild(string formula, Func<ITargetSelectorBuilder, IBuildableSelector> ofSelector)
    ```

---

### 4. Native CSS Nesting (`&`)
Native CSS Nesting is universally supported across modern browsers. When writing CSS or CSS-in-JS, builders frequently need to express:

1.  **Compound self-nesting:** `&.active`
2.  **Child/Descendant nesting:** `& > .child`
3.  **Ancestor/Context nesting:** `.dark &`, `.is-collapsed &` (where `&` appears at the *end* of the selector).
*   **Interface Impact:** You may want an explicit `.Ampersand()` or `.Self()` target, or a reverse-scoping mechanism like `.Inside(parent => parent.Class("dark"))`.

---

### 5. Beyond `@media`: Modern At-Rules
Restricting the top-level query strictly to `.Media()` leaves out the two other ubiquitous modern queries:

*   **`@container` (Container Queries):** `@container (min-width: 400px) { ... }`
*   **`@supports` (Feature Queries):** `@supports (display: grid) { ... }`
*   **`@scope` (Scoping):** `@scope (.card) to (.content) { ... }`
*   **Interface Impact:** Instead of coupling `IRootSelectorBuilder` solely to `Media()`, generalize at-rules or offer parallel entry methods:
    ```csharp
    public interface IRootSelectorBuilder : IThemeSelectorBuilder
    {
        IThemeSelectorBuilder Media(string query);
        IThemeSelectorBuilder Container(string query);
        IThemeSelectorBuilder Supports(string query);
    }
    ```

---

### 6. Attribute Matching Modifiers
Attributes aren't just `[attr]` and `[attr=value]`. CSS supports:

*   **All Match Operators:**
    *   `[attr^="val"]` (starts with)
    *   `[attr$="val"]` (ends with)
    *   `[attr*="val"]` (contains substring)
    *   `[attr~="val"]` (contains whitespace-separated word)
    *   `[attr|="val"]` (hyphen-delimited prefix, e.g., `lang|="en"`)
*   **Case Sensitivity Flags (`i` and `s`):**
    *   CSS: `[data-state="active" i]` (case-insensitive)
    *   CSS: `[type="SUBMIT" s]` (strictly case-sensitive)
*   **Interface Impact:**
    ```csharp
    public enum AttrOperator { Exact, Contains, StartsWith, EndsWith, ListContains, HyphenPrefix }
    public enum AttrCase { Default, Sensitive, Insensitive }

    ICompoundSelectorBuilder Attr(string name, AttrOperator op, string value, AttrCase casing = AttrCase.Default);
    ```

---

### 7. Functional Pseudo-Elements
Pseudo-elements are not only simple keywords like `::before` or `::after`. Shadow DOM and modern layout specs use functional pseudo-elements taking parameters:

*   **`::part(...)`:** Targets exposed shadow DOM elements (e.g., `my-element::part(button)`).
*   **`::slotted(...)`:** Targets slotted elements inside shadow DOM (e.g., `::slotted(span)`).
*   **`::view-transition-group(...)` / `::highlight(...)`**
*   **Chaining after Pseudo-Elements:** While pseudo-elements usually come last, modern CSS allows some pseudo-classes *after* a pseudo-element (e.g., `::part(button):hover`).

---

### 8. Universal Selector (`*`) & Escaping
*   **Universal Selector:** You need an explicit `.Any()` or `.Universal()` for patterns like `> *` (all direct children) or `*.highlight`.
*   **CSS Escaping:** In frameworks like Tailwind or utility-first systems, class names often contain colons, slashes, or brackets (e.g., `md:hover:bg-blue-500`, `w-1/2`, `top-[10px]`). 
    *   *Decision:* Does your `.Class("md:w-1/2")` automatically escape invalid CSS identifiers to `\.md\:w-1\/2`, or do you expect the user to pass pre-escaped strings? Automatic escaping via a built-in `Css.Escape(...)` utility prevents invalid CSS output.

---

### Recommended Structural Adjustment

To accommodate combinators and relative selectors cleanly:

```text
Target Builder  ──(Class/Attr/Pseudo)──►  Compound Builder
     ▲                                          │
     │                                     Combinator
     │                          (Child, Descendant, Sibling)
     └──────────────────────────────────────────┘
```

1. **`ICompoundSelectorBuilder`** returns a combinator method:
   * `.Child()` (`>`)
   * `.Descendant()` (` `)
   * `.AdjacentSibling()` (`+`)
   * `.GeneralSibling()` (`~`)
2. Calling any combinator **resets back to `ITargetSelectorBuilder`**, creating the continuous chain:
   ```csharp
   Selector
       .Class("prose")
       .Child()               // returns ITargetSelectorBuilder
       .Element("h2")         // returns ICompoundSelectorBuilder
       .Hover();
   // Output: .prose > h2:hover
   ```
3. A **`IRelativeSelectorBuilder`** (inheriting combinator capabilities first) can be passed into `.Has(...)` so users can seamlessly write:
   ```csharp
   .Has(rel => rel.Child().Element("img"))
   // Output: :has(> img)
   ```
   
---

New findings:

> Self-contained notes appended so a later session can pick this up. Everything
> below is the current state of the design, not yet implemented.

## Context

We are designing a **selector builder** for FluentCss. The goal is a typed,
fluent API to define reusable rules / component classes at startup, for example:

```csharp
Css.ApplySelector(
    s => s.Class("btn").Hover(),          // selector (this document)
    css => css.Px(4).Py(2).Background(Color.Blue600)); // declarations (IOnlyStyleBuilder)
```

`ApplySelector` feeds `ClassRegistry.AddSelector(selector, UtilityClass[])`, which
already exists. The selector builder is the missing piece. Related work already
done in this branch:

- `IRootFluentCssBuilder.ApplySelector(string, Action<IOnlyStyleBuilder>)` and the
  `ApplyClass` / `ApplyId` extension methods.
- `ClassRegistry.AddSelector(string, UtilityClass[])` implemented: groups by
  `(Theme, Pseudo, MediaQuery)`, emits selector rules **before** utility rules,
  media rules sorted like utility media rules, caches invalidated, `OnClassAdded`
  raised (currently with `utilities[0]` — may be replaced by a dedicated event).
- `ClassRegistry.GetEnumerator()` is now insertion-ordered via `ConcurrentQueue`,
  so declaration order inside an applied selector is deterministic.
- Core tests: 153 passing.

## Settled decisions (do not relitigate)

1. **Media/Theme come before the target.** The fluent chain must read like the
   emitted selector: `@media [theme] target[:pseudo]`. The staged interfaces below
   enforce it and we intentionally accept that limitation.
2. **`Id` is a compound modifier**, not a target-only method. `#id` composes with
   classes/attributes in any order (`.btn#submit`, `#submit.btn`).
3. **Shortcuts are extension methods**, not default interface members (the library
   targets `netstandard2.0`; default interface methods would break older runtimes).
   This covers `Dark`, `Hover`/`Focus`/`Active`/…, `Before`/`After`, `Aria`, `Min`/
   `Max`/`MinMd`/….
4. **No `Build()`.** A chain ends in `ToString()`, which returns the full selector
   string (including the media/theme wrapping).
5. **Always spell out `Attribute`** — never abbreviate to `Attr`.
6. Compound selector model + nested `Func` for functional pseudos + selector lists
   via `Or()` is the agreed core shape.

## Findings / actions to fold in

1. **`ToString()`/wrapper model and `Or()`.** A pure "each call wraps the previous"
   chain is fine for one selector, but selector lists (`Or()`) are where it gets
   awkward: the media wrapper is created first yet must wrap everything created
   after it. Keep the rest wrapper-based, but allow `Or()` to use a small internal
   accumulator. Decide this explicitly.
2. **Relative selectors (`:has`).** A relative selector may start with `>` `+` `~`
   but **not** a bare descendant combinator, so `IRelativeSelectorBuilder` should
   expose `Child()` / `AdjacentSibling()` / `GeneralSibling()` but not a leading
   `Descendant()`. Relative selectors can also be full complex selectors
   (`:has(> .a + .b)`), so after the leading combinator use the normal
   target → compound → combinator chain.
3. **Reuse `Or()` for `:is` / `:where` / `:not` / `:nth-child(... of ...)` lists.**
   Prefer `Not(n => n.Class("a").Or().Attribute("disabled"))` over `params Func[]`
   (allocation, reads badly). `params` may be added later as sugar only.
   Document the semantics: `:is()`/`:where()` parse forgivingly, `:not()` does not;
   `:is`/`:not`/`:has` take the most specific argument's specificity, `:where` is 0.
4. **Structural pseudos.** Add `NthLastChild`, `NthOfType`, `NthLastOfType` next to
   `NthChild`. The `of <selector-list>` argument must accept a selector list
   (`Or()`-capable), not a single selector.
5. **Native nesting `&` is a separate feature, not part of this builder.** It only
   makes sense once rules can be emitted nested; the registry emits flat rules
   today. `&.active` and `& > .child` are already expressible as compound /
   combinator chains. Only ancestor context (`.dark &`) is new — add an explicit
   `Inside(parent => …)` prefix helper (like theme already does) and defer true `&`
   / nested-rule emission.
6. **At-rules beyond `@media`.** `Container` / `Supports` returning
   `IThemeSelectorBuilder` allow only **one** at-rule. Real CSS stacks them
   (`@supports` inside `@media`). Decide: v1 = single at-rule, documented limit;
   later = accumulate at-rules in a stack and emit inside-out. `@scope` (different
   shape: `@scope (a) to (b)`) and `@layer` (cascade layers) are likely future.
7. **Attribute signature and naming.**
   ```csharp
   ISelectorBuilder Attribute(string name);
   ISelectorBuilder Attribute(string name, string value);
   ISelectorBuilder Attribute(string name, string value, AttributeOperator op,
                               AttributeCase casing = AttributeCase.Default);
   ```
   `AttributeOperator`: `Exact` (`=`), `Includes` (`~=`, prefer over
   "ListContains"), `HyphenPrefix` (`|=`), `Prefix` (`^=`), `Suffix` (`$=`),
   `Contains` (`*=`). `AttributeCase`: `Default` / `Sensitive` (`s`) / `Insensitive`
   (`i`).
8. **Pseudo-elements: split keyword vs functional.** Keyword ones (`Before`,
   `After`, `Marker`, …) are terminal. Functional ones (`Part(...)`, `Slotted(...)`)
   can be followed by pseudo-classes (`::part(button):hover`), so they return the
   compound builder. State this split in the document.
9. **Universal selector.** `Universal()` (not `Any()`) as a simple selector,
   available on the compound builder **and** after a combinator
   (`.Child().Universal()`).
10. **Escaping gap.** Auto-escape class names in `Class(...)` (the library already
    escapes utility selectors). But the current shared `UtilityClass.Escape` does
    **not** escape a **leading digit**, which is invalid in a CSS identifier
    (`Class("2xl")` → invalid `.2xl`). Needs the hex form (`.\32 xl`). Fix the
    shared escaper before reusing it here.
11. **Combinators after pseudo-elements.** Combinators must not be available after
    a terminal pseudo-element (otherwise `.a::before > .b`), except inside `:has`
    via `IRelativeSelectorBuilder`.
12. **Runtime validation.** Types cannot catch everything; define errors for:
    empty selector-list segment (`Or()` with nothing before it), empty `Not`/`Has`
    inner selector, empty `Class`/`Id`/`Attribute` names, repeated `Id`, and
    `Attribute` with an operator but no value. Suggest: throw with a clear message.

## Open questions for next session

- `Or()` vs `Targets(...)`: keep only `Or()` (with validation) or ship both?
- At-rule stacking in v1 or later? (`@media` only, or `@media`+`@supports`+...) 
- Naming: `IRootSelectorBuilder` vs `ISelectorBuilder`; `ITargetSelectorBuilder`
  vs `ICompoundSelectorBuilder`. The current internal stub is `ISelectorBuilder`.
- Should nested `Not`/`Is`/`Where`/`Has` exclude pseudo-elements? (`:not(::before)`
  is not a thing.) Their nested param type should probably not expose
  `IPseudoElementBuilder`, and must not expose theme/media (already the case if it
  starts at `ITargetSelectorBuilder`).
- Whether to raise `OnClassAdded` or add a dedicated change event for
  `AddSelector` (currently uses `utilities[0]`).

## Suggested next steps

1. Lock the interface list: `IRootSelectorBuilder`, `IThemeSelectorBuilder`,
   `ITargetSelectorBuilder`, `ICompoundSelectorBuilder`, `IPseudoElementBuilder`,
   `IRelativeSelectorBuilder`, plus `AttributeOperator` / `AttributeCase`.
2. Implement the internal selector node(s) that back `ToString()` (single selector
   first; selector lists second).
3. Add the `ApplySelector(Action<IRootSelectorBuilder>, Action<IOnlyStyleBuilder>)`
   overload (plus `Func` variants if target enforcement is wanted).
4. Add extension-method shortcuts (`Dark`, `Hover`, `Aria`, `Min`/`Max`, ...).
5. Fix leading-digit escaping in the shared escaper.
6. Tests: compound chaining, `:not`/`:has` nesting, relative selectors, selector
   lists, theme/media wrapping, attribute operators, `ToString()` output,
   validation errors.


