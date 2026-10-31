using System;

namespace FluentCss.Core;

/// <summary>
/// Css builder with pseudo-class and pseudo-element variants.
/// </summary>
/// <remarks>
/// The typed pseudo helpers live in <see cref="StyleBuilderPseudoExtensions"/>
/// (for example <c>Hover</c>, <c>FocusVisible</c>, <c>Placeholder</c>, <c>Before</c>).
/// <see cref="Pseudo"/> is the primitive those helpers are built on and the escape
/// hatch for pseudos without a dedicated helper.
/// </remarks>
public interface IFluentCssBuilder: IImportantStyleBuilder<IFluentCssBuilder>
{
    /// <summary>
    /// Applies the styles under an arbitrary pseudo-class or pseudo-element. The
    /// colon count is derived from the name: known pseudo-elements get a double
    /// colon (<c>::placeholder</c>), everything else a single colon
    /// (<c>:hover</c>). Include the leading colons to force the shape.
    /// </summary>
    /// <example>
    /// <code>
    /// Css.Pseudo("focus-visible", x =&gt; x.Outline(2));    // :focus-visible
    /// Css.Pseudo("placeholder", x =&gt; x.Color(Color.Neutral400)); // ::placeholder
    /// Css.Pseudo("part(label)", x =&gt; x.FontBold());       // ::part(label)
    /// Css.Pseudo("::marker", x =&gt; x.Color(Color.Blue500)); // ::marker
    /// </code>
    /// </example>
    /// <param name="pseudoPrefix">
    /// Pseudo-class or pseudo-element name or functional form, without the leading
    /// colon, for example <c>hover</c>, <c>nth-child(2)</c>, <c>placeholder</c> or
    /// <c>part(label)</c>. A leading <c>:</c> / <c>::</c> overrides the detection.
    /// </param>
    /// <param name="pseudoBuilderAction">Css builder setup action for the pseudo scope.</param>
    IFluentCssBuilder Pseudo(string pseudoPrefix, Action<IOnlyStyleBuilder> pseudoBuilderAction);
}
