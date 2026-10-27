using System;

namespace FluentCss.Core;

/// <summary>
/// A selector (or selector list) that is complete enough to be turned into CSS.
/// <see cref="object.ToString"/> returns the full selector, including any theme
/// prefix and at-rule wrapper.
/// </summary>
public interface IBuildableSelector
{
    /// <summary>The CSS text of the selector.</summary>
    string Css { get; }
}

/// <summary>
/// The terminal stage of a selector. Keyword pseudo-elements (<c>::before</c>,
/// <c>::after</c>, ...) close the selector. Functional pseudo-elements
/// (<c>::part(...)</c>, <c>::slotted(...)</c>) can be followed by pseudo-classes,
/// so they return to the compound stage.
/// </summary>
public interface IPseudoElementBuilder : IBuildableSelector
{
    /// <summary>Appends a keyword pseudo-element, for example <c>before</c>.</summary>
    IBuildableSelector PseudoElement(string pseudoElement);

    /// <summary>Appends a functional pseudo-element, for example <c>part</c> with <c>button</c>.</summary>
    ICompoundSelectorBuilder FunctionalPseudoElement(string name, string argument);
}

/// <summary>
/// The compound stage: classes, ids, attributes, pseudo-classes and combinators
/// can be appended in any order. Every method returns the same stage so chains
/// like <c>.btn.primary[disabled]:hover</c> can be built.
/// </summary>
public interface ICompoundSelectorBuilder : IPseudoElementBuilder
{
    /// <summary>Appends a class, for example <c>btn</c> becomes <c>.btn</c>.</summary>
    ICompoundSelectorBuilder Class(string className);

    /// <summary>Appends an id, for example <c>hero</c> becomes <c>#hero</c>.</summary>
    ICompoundSelectorBuilder Id(string id);

    /// <summary>Appends the universal selector <c>*</c>.</summary>
    ICompoundSelectorBuilder Universal();

    /// <summary>Appends an attribute presence selector, for example <c>[disabled]</c>.</summary>
    ICompoundSelectorBuilder Attribute(string name);

    /// <summary>Appends an attribute equality selector, for example <c>[type="text"]</c>.</summary>
    ICompoundSelectorBuilder Attribute(string name, string value);

    /// <summary>Appends an attribute selector with the given operator and case flag.</summary>
    ICompoundSelectorBuilder Attribute(string name, string value, AttributeOperator op, AttributeCase casing = AttributeCase.Default);

    /// <summary>Appends a pseudo-class, for example <c>hover</c> becomes <c>:hover</c>.</summary>
    ICompoundSelectorBuilder Pseudo(string pseudo);

    /// <summary>Appends a functional pseudo-class, for example <c>nth-child</c> with <c>2n+1</c>.</summary>
    ICompoundSelectorBuilder Pseudo(string pseudo, string argument);

    /// <summary>
    /// Appends a <c>:not(...)</c> functional pseudo-class with a selector list.
    /// The same syntax is used for <c>:is()</c> and <c>:where()</c>.
    /// </summary>
    ICompoundSelectorBuilder Not(params Func<ITargetSelectorBuilder, IBuildableSelector>[] selectors);

    /// <inheritdoc cref="Not"/>
    ICompoundSelectorBuilder Is(params Func<ITargetSelectorBuilder, IBuildableSelector>[] selectors);

    /// <inheritdoc cref="Not"/>
    ICompoundSelectorBuilder Where(params Func<ITargetSelectorBuilder, IBuildableSelector>[] selectors);

    /// <summary>
    /// Appends a <c>:has(...)</c> functional pseudo-class. The arguments are
    /// relative selectors, so they may start with a combinator such as
    /// <c>Child()</c> (<c>:has(&gt; img)</c>).
    /// </summary>
    ICompoundSelectorBuilder Has(params Func<IRelativeSelectorBuilder, ICompoundSelectorBuilder>[] selectors);

    /// <summary>Appends the descendant combinator (a space).</summary>
    ITargetSelectorBuilder Descendant();

    /// <summary>Appends the child combinator <c>&gt;</c>.</summary>
    ITargetSelectorBuilder Child();

    /// <summary>Appends the adjacent sibling combinator <c>+</c>.</summary>
    ITargetSelectorBuilder AdjacentSibling();

    /// <summary>Appends the general sibling combinator <c>~</c>.</summary>
    ITargetSelectorBuilder GeneralSibling();
}

/// <summary>
/// The target stage: an element name can be added to the current compound.
/// </summary>
public interface ITargetSelectorBuilder : ICompoundSelectorBuilder
{
    /// <summary>Appends a type selector, for example <c>div</c> or <c>input</c>.</summary>
    ICompoundSelectorBuilder Element(string tag);
}

/// <summary>
/// The builder passed to <see cref="ICompoundSelectorBuilder.Has"/>. It is a normal
/// target builder, but it may also start with a combinator.
/// </summary>
public interface IRelativeSelectorBuilder : ITargetSelectorBuilder
{
}

/// <summary>
/// A context builder that already carries the media/theme part of the selector and
/// can therefore start a selector list whose members all share that context.
/// </summary>
public interface ISelectorContextBuilder : ITargetSelectorBuilder
{
    /// <summary>
    /// Builds a selector list. Every target shares the current theme and, when the
    /// selector is rendered, the current at-rule wrapper.
    /// </summary>
    IBuildableSelector Targets(params Func<ITargetSelectorBuilder, IBuildableSelector>[] targets);
}

/// <summary>
/// The theme stage. Themes can only be selected once in a chain; the method returns
/// a context builder, not another theme builder.
/// </summary>
public interface IThemeSelectorBuilder : ISelectorContextBuilder
{
    /// <summary>
    /// Prefixes the selector with a theme class, for example <c>dark</c> becomes
    /// <c>.theme-dark</c> (the same <c>theme-</c> convention as the main builder).
    /// </summary>
    ISelectorContextBuilder Theme(string theme);
}

/// <summary>
/// The entry point. Media (or another at-rule) can be selected before targeting.
/// </summary>
public interface IRootSelectorBuilder : IThemeSelectorBuilder
{
    /// <summary>Wraps the selector in an <c>@media</c> rule.</summary>
    IThemeSelectorBuilder Media(string query);

    /// <summary>Wraps the selector in a <c>@container</c> rule.</summary>
    IThemeSelectorBuilder Container(string query);

    /// <summary>Wraps the selector in an <c>@supports</c> rule.</summary>
    IThemeSelectorBuilder Supports(string query);
}

/// <summary>The attribute matching operator used by <see cref="ICompoundSelectorBuilder.Attribute(string, string, AttributeOperator, AttributeCase)"/>.</summary>
public enum AttributeOperator
{
    /// <summary><c>[attr="value"]</c></summary>
    Exact,

    /// <summary><c>[attr~="value"]</c> — a whitespace-separated list containing <c>value</c>.</summary>
    Includes,

    /// <summary><c>[attr|="value"]</c> — equals <c>value</c> or starts with <c>value-</c>.</summary>
    HyphenPrefix,

    /// <summary><c>[attr^="value"]</c> — starts with <c>value</c>.</summary>
    Prefix,

    /// <summary><c>[attr$="value"]</c> — ends with <c>value</c>.</summary>
    Suffix,

    /// <summary><c>[attr*="value"]</c> — contains <c>value</c>.</summary>
    Contains
}

/// <summary>The case-sensitivity flag of an attribute selector.</summary>
public enum AttributeCase
{
    /// <summary>No flag; the document's default applies.</summary>
    Default,

    /// <summary><c>s</c> — force case-sensitive matching.</summary>
    Sensitive,

    /// <summary><c>i</c> — force case-insensitive matching.</summary>
    Insensitive
}

/// <summary>
/// Shortcut extension methods for the selector builder. They are extension methods
/// rather than default interface members so the library keeps working on older
/// runtimes that target <c>netstandard2.0</c>.
/// </summary>
public static class SelectorBuilderExtensions
{
    /// <summary>Shortcut for <see cref="IThemeSelectorBuilder.Theme"/> with the <c>dark</c> theme.</summary>
    public static ISelectorContextBuilder Dark(this IThemeSelectorBuilder builder) => builder.Theme("dark");

    /// <summary>Appends the <c>:hover</c> pseudo-class.</summary>
    public static ICompoundSelectorBuilder Hover(this ICompoundSelectorBuilder builder) => builder.Pseudo("hover");

    /// <summary>Appends the <c>:focus</c> pseudo-class.</summary>
    public static ICompoundSelectorBuilder Focus(this ICompoundSelectorBuilder builder) => builder.Pseudo("focus");

    /// <summary>Appends the <c>:active</c> pseudo-class.</summary>
    public static ICompoundSelectorBuilder Active(this ICompoundSelectorBuilder builder) => builder.Pseudo("active");

    /// <summary>Appends the <c>:disabled</c> pseudo-class.</summary>
    public static ICompoundSelectorBuilder Disabled(this ICompoundSelectorBuilder builder) => builder.Pseudo("disabled");

    /// <summary>Appends the <c>:visited</c> pseudo-class.</summary>
    public static ICompoundSelectorBuilder Visited(this ICompoundSelectorBuilder builder) => builder.Pseudo("visited");

    /// <summary>Appends the <c>:target</c> pseudo-class.</summary>
    public static ICompoundSelectorBuilder Target(this ICompoundSelectorBuilder builder) => builder.Pseudo("target");

    /// <summary>Appends the <c>:focus-visible</c> pseudo-class.</summary>
    public static ICompoundSelectorBuilder FocusVisible(this ICompoundSelectorBuilder builder) => builder.Pseudo("focus-visible");

    /// <summary>Appends the <c>:focus-within</c> pseudo-class.</summary>
    public static ICompoundSelectorBuilder FocusWithin(this ICompoundSelectorBuilder builder) => builder.Pseudo("focus-within");

    /// <summary>Appends the <c>:enabled</c> pseudo-class.</summary>
    public static ICompoundSelectorBuilder Enabled(this ICompoundSelectorBuilder builder) => builder.Pseudo("enabled");

    /// <summary>Appends the <c>:checked</c> pseudo-class.</summary>
    public static ICompoundSelectorBuilder Checked(this ICompoundSelectorBuilder builder) => builder.Pseudo("checked");

    /// <summary>Appends the <c>:indeterminate</c> pseudo-class.</summary>
    public static ICompoundSelectorBuilder Indeterminate(this ICompoundSelectorBuilder builder) => builder.Pseudo("indeterminate");

    /// <summary>Appends the <c>:default</c> pseudo-class.</summary>
    public static ICompoundSelectorBuilder Default(this ICompoundSelectorBuilder builder) => builder.Pseudo("default");

    /// <summary>Appends the <c>:required</c> pseudo-class.</summary>
    public static ICompoundSelectorBuilder Required(this ICompoundSelectorBuilder builder) => builder.Pseudo("required");

    /// <summary>Appends the <c>:optional</c> pseudo-class.</summary>
    public static ICompoundSelectorBuilder Optional(this ICompoundSelectorBuilder builder) => builder.Pseudo("optional");

    /// <summary>Appends the <c>:valid</c> pseudo-class.</summary>
    public static ICompoundSelectorBuilder Valid(this ICompoundSelectorBuilder builder) => builder.Pseudo("valid");

    /// <summary>Appends the <c>:invalid</c> pseudo-class.</summary>
    public static ICompoundSelectorBuilder Invalid(this ICompoundSelectorBuilder builder) => builder.Pseudo("invalid");

    /// <summary>Appends the <c>:in-range</c> pseudo-class.</summary>
    public static ICompoundSelectorBuilder InRange(this ICompoundSelectorBuilder builder) => builder.Pseudo("in-range");

    /// <summary>Appends the <c>:out-of-range</c> pseudo-class.</summary>
    public static ICompoundSelectorBuilder OutOfRange(this ICompoundSelectorBuilder builder) => builder.Pseudo("out-of-range");

    /// <summary>Appends the <c>:read-only</c> pseudo-class.</summary>
    public static ICompoundSelectorBuilder ReadOnly(this ICompoundSelectorBuilder builder) => builder.Pseudo("read-only");

    /// <summary>Appends the <c>:read-write</c> pseudo-class.</summary>
    public static ICompoundSelectorBuilder ReadWrite(this ICompoundSelectorBuilder builder) => builder.Pseudo("read-write");

    /// <summary>Appends the <c>:placeholder-shown</c> pseudo-class.</summary>
    public static ICompoundSelectorBuilder PlaceholderShown(this ICompoundSelectorBuilder builder) => builder.Pseudo("placeholder-shown");

    /// <summary>Appends the <c>:autofill</c> pseudo-class.</summary>
    public static ICompoundSelectorBuilder Autofill(this ICompoundSelectorBuilder builder) => builder.Pseudo("autofill");

    /// <summary>Appends the <c>:user-valid</c> pseudo-class.</summary>
    public static ICompoundSelectorBuilder UserValid(this ICompoundSelectorBuilder builder) => builder.Pseudo("user-valid");

    /// <summary>Appends the <c>:user-invalid</c> pseudo-class.</summary>
    public static ICompoundSelectorBuilder UserInvalid(this ICompoundSelectorBuilder builder) => builder.Pseudo("user-invalid");

    /// <summary>Appends the <c>:open</c> pseudo-class.</summary>
    public static ICompoundSelectorBuilder Open(this ICompoundSelectorBuilder builder) => builder.Pseudo("open");

    /// <summary>Appends the <c>:popover-open</c> pseudo-class.</summary>
    public static ICompoundSelectorBuilder PopoverOpen(this ICompoundSelectorBuilder builder) => builder.Pseudo("popover-open");

    /// <summary>Appends the <c>:modal</c> pseudo-class.</summary>
    public static ICompoundSelectorBuilder Modal(this ICompoundSelectorBuilder builder) => builder.Pseudo("modal");

    /// <summary>Appends the <c>:fullscreen</c> pseudo-class.</summary>
    public static ICompoundSelectorBuilder Fullscreen(this ICompoundSelectorBuilder builder) => builder.Pseudo("fullscreen");

    /// <summary>Appends the <c>:picture-in-picture</c> pseudo-class.</summary>
    public static ICompoundSelectorBuilder PictureInPicture(this ICompoundSelectorBuilder builder) => builder.Pseudo("picture-in-picture");

    /// <summary>Appends the <c>:defined</c> pseudo-class.</summary>
    public static ICompoundSelectorBuilder Defined(this ICompoundSelectorBuilder builder) => builder.Pseudo("defined");

    /// <summary>Appends the <c>:root</c> pseudo-class.</summary>
    public static ICompoundSelectorBuilder Root(this ICompoundSelectorBuilder builder) => builder.Pseudo("root");

    /// <summary>Appends the <c>:empty</c> pseudo-class.</summary>
    public static ICompoundSelectorBuilder Empty(this ICompoundSelectorBuilder builder) => builder.Pseudo("empty");

    /// <summary>Appends the <c>:first-child</c> pseudo-class.</summary>
    public static ICompoundSelectorBuilder First(this ICompoundSelectorBuilder builder) => builder.Pseudo("first-child");

    /// <summary>Appends the <c>:last-child</c> pseudo-class.</summary>
    public static ICompoundSelectorBuilder Last(this ICompoundSelectorBuilder builder) => builder.Pseudo("last-child");

    /// <summary>Appends the <c>:only-child</c> pseudo-class.</summary>
    public static ICompoundSelectorBuilder Only(this ICompoundSelectorBuilder builder) => builder.Pseudo("only-child");

    /// <summary>Appends the <c>:nth-child(odd)</c> pseudo-class.</summary>
    public static ICompoundSelectorBuilder Odd(this ICompoundSelectorBuilder builder) => builder.Pseudo("nth-child(odd)");

    /// <summary>Appends the <c>:nth-child(even)</c> pseudo-class.</summary>
    public static ICompoundSelectorBuilder Even(this ICompoundSelectorBuilder builder) => builder.Pseudo("nth-child(even)");

    /// <summary>Appends the <c>:first-of-type</c> pseudo-class.</summary>
    public static ICompoundSelectorBuilder FirstOfType(this ICompoundSelectorBuilder builder) => builder.Pseudo("first-of-type");

    /// <summary>Appends the <c>:last-of-type</c> pseudo-class.</summary>
    public static ICompoundSelectorBuilder LastOfType(this ICompoundSelectorBuilder builder) => builder.Pseudo("last-of-type");

    /// <summary>Appends the <c>:only-of-type</c> pseudo-class.</summary>
    public static ICompoundSelectorBuilder OnlyOfType(this ICompoundSelectorBuilder builder) => builder.Pseudo("only-of-type");

    /// <summary>Appends <c>:nth-child(n)</c>.</summary>
    public static ICompoundSelectorBuilder NthChild(this ICompoundSelectorBuilder builder, int n) =>
        builder.Pseudo("nth-child", n.ToString());

    /// <summary>Appends <c>:nth-child(formula)</c>, for example <c>2n+1</c> or <c>odd</c>.</summary>
    public static ICompoundSelectorBuilder NthChild(this ICompoundSelectorBuilder builder, string formula) =>
        builder.Pseudo("nth-child", RequireArgument(formula, nameof(formula)));

    /// <summary>Appends <c>:nth-last-child(n)</c>.</summary>
    public static ICompoundSelectorBuilder NthLastChild(this ICompoundSelectorBuilder builder, int n) =>
        builder.Pseudo("nth-last-child", n.ToString());

    /// <summary>Appends <c>:nth-last-child(formula)</c>, for example <c>2n+1</c> or <c>odd</c>.</summary>
    public static ICompoundSelectorBuilder NthLastChild(this ICompoundSelectorBuilder builder, string formula) =>
        builder.Pseudo("nth-last-child", RequireArgument(formula, nameof(formula)));

    /// <summary>Appends <c>:nth-of-type(n)</c>.</summary>
    public static ICompoundSelectorBuilder NthOfType(this ICompoundSelectorBuilder builder, int n) =>
        builder.Pseudo("nth-of-type", n.ToString());

    /// <summary>Appends <c>:nth-of-type(formula)</c>, for example <c>2n+1</c> or <c>odd</c>.</summary>
    public static ICompoundSelectorBuilder NthOfType(this ICompoundSelectorBuilder builder, string formula) =>
        builder.Pseudo("nth-of-type", RequireArgument(formula, nameof(formula)));

    /// <summary>Appends <c>:nth-last-of-type(n)</c>.</summary>
    public static ICompoundSelectorBuilder NthLastOfType(this ICompoundSelectorBuilder builder, int n) =>
        builder.Pseudo("nth-last-of-type", n.ToString());

    /// <summary>Appends <c>:nth-last-of-type(formula)</c>, for example <c>2n+1</c> or <c>odd</c>.</summary>
    public static ICompoundSelectorBuilder NthLastOfType(this ICompoundSelectorBuilder builder, string formula) =>
        builder.Pseudo("nth-last-of-type", RequireArgument(formula, nameof(formula)));

    /// <summary>Appends <c>:lang(language)</c>.</summary>
    public static ICompoundSelectorBuilder Lang(this ICompoundSelectorBuilder builder, string language) =>
        builder.Pseudo("lang", RequireArgument(language, nameof(language)));

    /// <summary>Appends <c>:dir(direction)</c>, for example <c>:dir(rtl)</c>.</summary>
    public static ICompoundSelectorBuilder Dir(this ICompoundSelectorBuilder builder, string direction) =>
        builder.Pseudo("dir", RequireArgument(direction, nameof(direction)));

    /// <summary>Appends a <c>[data-name]</c> presence selector.</summary>
    public static ICompoundSelectorBuilder Data(this ICompoundSelectorBuilder builder, string name) =>
        builder.Attribute(PrefixedAttribute("data", name));

    /// <summary>Appends a <c>[data-name="value"]</c> selector.</summary>
    public static ICompoundSelectorBuilder Data(this ICompoundSelectorBuilder builder, string name, string value) =>
        builder.Attribute(PrefixedAttribute("data", name), value);

    /// <summary>Appends a <c>[data-name op "value"]</c> selector.</summary>
    public static ICompoundSelectorBuilder Data(this ICompoundSelectorBuilder builder, string name, string value, AttributeOperator op, AttributeCase casing = AttributeCase.Default) =>
        builder.Attribute(PrefixedAttribute("data", name), value, op, casing);

    /// <summary>Appends an <c>[aria-name]</c> presence selector.</summary>
    public static ICompoundSelectorBuilder Aria(this ICompoundSelectorBuilder builder, string name) =>
        builder.Attribute(PrefixedAttribute("aria", name));

    /// <summary>Appends an <c>[aria-name="value"]</c> selector.</summary>
    public static ICompoundSelectorBuilder Aria(this ICompoundSelectorBuilder builder, string name, string value) =>
        builder.Attribute(PrefixedAttribute("aria", name), value);

    /// <summary>Appends an <c>[aria-name op "value"]</c> selector.</summary>
    public static ICompoundSelectorBuilder Aria(this ICompoundSelectorBuilder builder, string name, string value, AttributeOperator op, AttributeCase casing = AttributeCase.Default) =>
        builder.Attribute(PrefixedAttribute("aria", name), value, op, casing);

    /// <summary>Appends the <c>::before</c> pseudo-element.</summary>
    public static IBuildableSelector Before(this ICompoundSelectorBuilder builder) => builder.PseudoElement("before");

    /// <summary>Appends the <c>::after</c> pseudo-element.</summary>
    public static IBuildableSelector After(this ICompoundSelectorBuilder builder) => builder.PseudoElement("after");

    private static string PrefixedAttribute(string prefix, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("An attribute name cannot be empty.", nameof(name));
        }

        return prefix + "-" + name.Trim();
    }

    private static string RequireArgument(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("A pseudo-class argument cannot be empty.", parameterName);
        }

        return value.Trim();
    }
}

/// <summary>Entry point for building a selector outside of the fluent CSS builder.</summary>
public static class Selector
{
    /// <summary>Creates a new root selector builder.</summary>
    public static IRootSelectorBuilder New() => new SelectorBuilder(null, null);
}
