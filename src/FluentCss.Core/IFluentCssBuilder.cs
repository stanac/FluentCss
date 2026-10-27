using System;

namespace FluentCss.Core;

/// <summary>
/// Css builder with pseudo-class variants.
/// </summary>
/// <remarks>
/// The typed pseudo-class helpers live in <see cref="StyleBuilderPseudoExtensions"/>
/// (for example <c>Hover</c>, <c>FocusVisible</c>, <c>Checked</c>).
/// <see cref="Pseudo"/> is the primitive those helpers are built on and the escape
/// hatch for pseudo-classes without a dedicated helper.
/// </remarks>
public interface IFluentCssBuilder: IImportantStyleBuilder<IFluentCssBuilder>
{
    /// <summary>
    /// Applies the styles under an arbitrary pseudo-class.
    /// </summary>
    /// <example>
    /// <code>
    /// Css.Pseudo("focus-visible", x =&gt; x.Outline(2));
    /// </code>
    /// </example>
    /// <param name="pseudoPrefix">
    /// Pseudo-class name or functional form, without the leading colon, for example
    /// <c>hover</c>, <c>focus-visible</c> or <c>nth-child(2)</c>.
    /// </param>
    /// <param name="pseudoBuilderAction">Css builder setup action for the pseudo-class scope.</param>
    IFluentCssBuilder Pseudo(string pseudoPrefix, Action<IOnlyStyleBuilder> pseudoBuilderAction);
}
