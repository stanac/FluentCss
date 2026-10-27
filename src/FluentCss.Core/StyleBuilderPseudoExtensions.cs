using System;

namespace FluentCss.Core;

/// <summary>
/// Typed pseudo-class variants for <see cref="IFluentCssBuilder"/>. Each helper is a
/// thin wrapper over <see cref="IFluentCssBuilder.Pseudo(string, Action{IOnlyStyleBuilder})"/>,
/// which remains available as the escape hatch for pseudo-classes without a helper.
/// </summary>
/// <remarks>
/// The helpers compose with theme, media and <c>!important</c> like any other
/// utility variant because they run inside the same pseudo scope, for example
/// <c>Css.MinMd(md =&gt; md.FocusVisible(c =&gt; c.Outline(2)))</c> generates
/// <c>min-md:focus-visible:outline-2</c>.
/// </remarks>
public static class StyleBuilderPseudoExtensions
{
    // ---------------------------------------------------------------------
    // State
    // ---------------------------------------------------------------------

    /// <summary>Matches <c>:hover</c>.</summary>
    public static IFluentCssBuilder Hover(this IFluentCssBuilder builder, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo("hover", pseudoBuilderAction);

    /// <summary>Matches <c>:focus</c>.</summary>
    public static IFluentCssBuilder Focus(this IFluentCssBuilder builder, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo("focus", pseudoBuilderAction);

    /// <summary>Matches <c>:active</c>.</summary>
    public static IFluentCssBuilder Active(this IFluentCssBuilder builder, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo("active", pseudoBuilderAction);

    /// <summary>Matches <c>:visited</c>.</summary>
    public static IFluentCssBuilder Visited(this IFluentCssBuilder builder, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo("visited", pseudoBuilderAction);

    /// <summary>Matches <c>:target</c>.</summary>
    public static IFluentCssBuilder Target(this IFluentCssBuilder builder, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo("target", pseudoBuilderAction);

    /// <summary>Matches <c>:focus-visible</c>.</summary>
    public static IFluentCssBuilder FocusVisible(this IFluentCssBuilder builder, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo("focus-visible", pseudoBuilderAction);

    /// <summary>Matches <c>:focus-within</c>.</summary>
    public static IFluentCssBuilder FocusWithin(this IFluentCssBuilder builder, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo("focus-within", pseudoBuilderAction);

    /// <summary>Matches <c>:open</c> (for example an open <c>&lt;details&gt;</c>).</summary>
    public static IFluentCssBuilder Open(this IFluentCssBuilder builder, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo("open", pseudoBuilderAction);

    /// <summary>Matches <c>:popover-open</c>.</summary>
    public static IFluentCssBuilder PopoverOpen(this IFluentCssBuilder builder, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo("popover-open", pseudoBuilderAction);

    /// <summary>Matches <c>:modal</c>.</summary>
    public static IFluentCssBuilder Modal(this IFluentCssBuilder builder, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo("modal", pseudoBuilderAction);

    /// <summary>Matches <c>:fullscreen</c>.</summary>
    public static IFluentCssBuilder Fullscreen(this IFluentCssBuilder builder, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo("fullscreen", pseudoBuilderAction);

    /// <summary>Matches <c>:picture-in-picture</c>.</summary>
    public static IFluentCssBuilder PictureInPicture(this IFluentCssBuilder builder, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo("picture-in-picture", pseudoBuilderAction);

    // ---------------------------------------------------------------------
    // Form state
    // ---------------------------------------------------------------------

    /// <summary>Matches <c>:disabled</c>.</summary>
    public static IFluentCssBuilder Disabled(this IFluentCssBuilder builder, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo("disabled", pseudoBuilderAction);

    /// <summary>Matches <c>:enabled</c>.</summary>
    public static IFluentCssBuilder Enabled(this IFluentCssBuilder builder, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo("enabled", pseudoBuilderAction);

    /// <summary>Matches <c>:checked</c>.</summary>
    public static IFluentCssBuilder Checked(this IFluentCssBuilder builder, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo("checked", pseudoBuilderAction);

    /// <summary>Matches <c>:indeterminate</c>.</summary>
    public static IFluentCssBuilder Indeterminate(this IFluentCssBuilder builder, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo("indeterminate", pseudoBuilderAction);

    /// <summary>Matches <c>:default</c>.</summary>
    public static IFluentCssBuilder Default(this IFluentCssBuilder builder, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo("default", pseudoBuilderAction);

    /// <summary>Matches <c>:required</c>.</summary>
    public static IFluentCssBuilder Required(this IFluentCssBuilder builder, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo("required", pseudoBuilderAction);

    /// <summary>Matches <c>:optional</c>.</summary>
    public static IFluentCssBuilder Optional(this IFluentCssBuilder builder, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo("optional", pseudoBuilderAction);

    /// <summary>Matches <c>:valid</c>.</summary>
    public static IFluentCssBuilder Valid(this IFluentCssBuilder builder, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo("valid", pseudoBuilderAction);

    /// <summary>Matches <c>:invalid</c>.</summary>
    public static IFluentCssBuilder Invalid(this IFluentCssBuilder builder, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo("invalid", pseudoBuilderAction);

    /// <summary>Matches <c>:in-range</c>.</summary>
    public static IFluentCssBuilder InRange(this IFluentCssBuilder builder, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo("in-range", pseudoBuilderAction);

    /// <summary>Matches <c>:out-of-range</c>.</summary>
    public static IFluentCssBuilder OutOfRange(this IFluentCssBuilder builder, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo("out-of-range", pseudoBuilderAction);

    /// <summary>Matches <c>:read-only</c>.</summary>
    public static IFluentCssBuilder ReadOnly(this IFluentCssBuilder builder, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo("read-only", pseudoBuilderAction);

    /// <summary>Matches <c>:read-write</c>.</summary>
    public static IFluentCssBuilder ReadWrite(this IFluentCssBuilder builder, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo("read-write", pseudoBuilderAction);

    /// <summary>Matches <c>:placeholder-shown</c>.</summary>
    public static IFluentCssBuilder PlaceholderShown(this IFluentCssBuilder builder, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo("placeholder-shown", pseudoBuilderAction);

    /// <summary>Matches <c>:autofill</c>.</summary>
    public static IFluentCssBuilder Autofill(this IFluentCssBuilder builder, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo("autofill", pseudoBuilderAction);

    /// <summary>Matches <c>:user-valid</c>.</summary>
    public static IFluentCssBuilder UserValid(this IFluentCssBuilder builder, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo("user-valid", pseudoBuilderAction);

    /// <summary>Matches <c>:user-invalid</c>.</summary>
    public static IFluentCssBuilder UserInvalid(this IFluentCssBuilder builder, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo("user-invalid", pseudoBuilderAction);

    // ---------------------------------------------------------------------
    // Structural
    // ---------------------------------------------------------------------

    /// <summary>Matches <c>:root</c>.</summary>
    public static IFluentCssBuilder Root(this IFluentCssBuilder builder, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo("root", pseudoBuilderAction);

    /// <summary>Matches <c>:empty</c>.</summary>
    public static IFluentCssBuilder Empty(this IFluentCssBuilder builder, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo("empty", pseudoBuilderAction);

    /// <summary>Matches <c>:first-child</c>.</summary>
    public static IFluentCssBuilder First(this IFluentCssBuilder builder, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo("first-child", pseudoBuilderAction);

    /// <summary>Matches <c>:last-child</c>.</summary>
    public static IFluentCssBuilder Last(this IFluentCssBuilder builder, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo("last-child", pseudoBuilderAction);

    /// <summary>Matches <c>:only-child</c>.</summary>
    public static IFluentCssBuilder Only(this IFluentCssBuilder builder, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo("only-child", pseudoBuilderAction);

    /// <summary>Matches <c>:nth-child(odd)</c>.</summary>
    public static IFluentCssBuilder Odd(this IFluentCssBuilder builder, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo("nth-child(odd)", pseudoBuilderAction);

    /// <summary>Matches <c>:nth-child(even)</c>.</summary>
    public static IFluentCssBuilder Even(this IFluentCssBuilder builder, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo("nth-child(even)", pseudoBuilderAction);

    /// <summary>Matches <c>:first-of-type</c>.</summary>
    public static IFluentCssBuilder FirstOfType(this IFluentCssBuilder builder, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo("first-of-type", pseudoBuilderAction);

    /// <summary>Matches <c>:last-of-type</c>.</summary>
    public static IFluentCssBuilder LastOfType(this IFluentCssBuilder builder, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo("last-of-type", pseudoBuilderAction);

    /// <summary>Matches <c>:only-of-type</c>.</summary>
    public static IFluentCssBuilder OnlyOfType(this IFluentCssBuilder builder, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo("only-of-type", pseudoBuilderAction);

    /// <summary>Matches <c>:defined</c> (a defined custom element).</summary>
    public static IFluentCssBuilder Defined(this IFluentCssBuilder builder, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo("defined", pseudoBuilderAction);

    // ---------------------------------------------------------------------
    // Functional
    // ---------------------------------------------------------------------

    /// <summary>Matches <c>:nth-child(n)</c>.</summary>
    public static IFluentCssBuilder NthChild(this IFluentCssBuilder builder, int n, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo($"nth-child({n})", pseudoBuilderAction);

    /// <summary>Matches <c>:nth-child(formula)</c>, for example <c>2n+1</c> or <c>odd</c>.</summary>
    public static IFluentCssBuilder NthChild(this IFluentCssBuilder builder, string formula, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo($"nth-child({RequireArgument(formula, nameof(formula))})", pseudoBuilderAction);

    /// <summary>Matches <c>:nth-last-child(n)</c>.</summary>
    public static IFluentCssBuilder NthLastChild(this IFluentCssBuilder builder, int n, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo($"nth-last-child({n})", pseudoBuilderAction);

    /// <summary>Matches <c>:nth-last-child(formula)</c>, for example <c>2n+1</c> or <c>odd</c>.</summary>
    public static IFluentCssBuilder NthLastChild(this IFluentCssBuilder builder, string formula, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo($"nth-last-child({RequireArgument(formula, nameof(formula))})", pseudoBuilderAction);

    /// <summary>Matches <c>:nth-of-type(n)</c>.</summary>
    public static IFluentCssBuilder NthOfType(this IFluentCssBuilder builder, int n, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo($"nth-of-type({n})", pseudoBuilderAction);

    /// <summary>Matches <c>:nth-of-type(formula)</c>, for example <c>2n+1</c> or <c>odd</c>.</summary>
    public static IFluentCssBuilder NthOfType(this IFluentCssBuilder builder, string formula, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo($"nth-of-type({RequireArgument(formula, nameof(formula))})", pseudoBuilderAction);

    /// <summary>Matches <c>:nth-last-of-type(n)</c>.</summary>
    public static IFluentCssBuilder NthLastOfType(this IFluentCssBuilder builder, int n, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo($"nth-last-of-type({n})", pseudoBuilderAction);

    /// <summary>Matches <c>:nth-last-of-type(formula)</c>, for example <c>2n+1</c> or <c>odd</c>.</summary>
    public static IFluentCssBuilder NthLastOfType(this IFluentCssBuilder builder, string formula, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo($"nth-last-of-type({RequireArgument(formula, nameof(formula))})", pseudoBuilderAction);

    /// <summary>Matches <c>:lang(language)</c>.</summary>
    public static IFluentCssBuilder Lang(this IFluentCssBuilder builder, string language, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo($"lang({RequireArgument(language, nameof(language))})", pseudoBuilderAction);

    /// <summary>Matches <c>:dir(direction)</c>, for example <c>:dir(rtl)</c>.</summary>
    public static IFluentCssBuilder Dir(this IFluentCssBuilder builder, string direction, Action<IOnlyStyleBuilder> pseudoBuilderAction) =>
        builder.Pseudo($"dir({RequireArgument(direction, nameof(direction))})", pseudoBuilderAction);

    private static string RequireArgument(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("A pseudo-class argument cannot be empty.", parameterName);
        }

        return value.Trim();
    }
}
