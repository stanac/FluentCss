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

    /// <summary>Appends <c>:nth-child(n)</c>.</summary>
    ICompoundSelectorBuilder NthChild(int n);

    /// <summary>Appends <c>:nth-child(formula)</c>, for example <c>2n+1</c> or <c>odd</c>.</summary>
    ICompoundSelectorBuilder NthChild(string formula);

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

    /// <summary>Appends the <c>::before</c> pseudo-element.</summary>
    public static IBuildableSelector Before(this ICompoundSelectorBuilder builder) => builder.PseudoElement("before");

    /// <summary>Appends the <c>::after</c> pseudo-element.</summary>
    public static IBuildableSelector After(this ICompoundSelectorBuilder builder) => builder.PseudoElement("after");
}

/// <summary>Entry point for building a selector outside of the fluent CSS builder.</summary>
public static class Selector
{
    /// <summary>Creates a new root selector builder.</summary>
    public static IRootSelectorBuilder New() => new SelectorBuilder(null, null);
}
