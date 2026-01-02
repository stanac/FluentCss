using System;

namespace CssB.Core;

/// <summary>
/// Css builder with pseudo builder methods
/// </summary>
public interface ICssBuilder: IStyleBuilder<ICssBuilder>
{
    /// <summary>
    /// Matches when a user designates an item with a pointing device, such as holding the mouse pointer over the item.
    /// </summary>
    /// <param name="pseudoBuilderAction">Css builder setup action for pseudo</param>
    ICssBuilder Hover(Action<IOnlyStyleBuilder> pseudoBuilderAction);

    /// <summary>
    /// Matches when an item is being activated by the user. For example, when the item is clicked on.
    /// </summary>
    /// <param name="pseudoBuilderAction">Css builder setup action for pseudo</param>
    ICssBuilder Active(Action<IOnlyStyleBuilder> pseudoBuilderAction);

    /// <summary>
    /// Matches when an element has focus.
    /// </summary>
    /// <param name="pseudoBuilderAction">Css builder setup action for pseudo</param>
    ICssBuilder Focus(Action<IOnlyStyleBuilder> pseudoBuilderAction);

    /// <summary>
    /// Matches custom pseudo prefix (e.g. :hover)
    /// </summary>
    /// <param name="pseudoPrefix">Pseudo prefix, must start with : (colon)</param>
    /// <param name="pseudoBuilderAction">Css builder setup action for pseudo</param>
    ICssBuilder Pseudo(string pseudoPrefix, Action<IOnlyStyleBuilder> pseudoBuilderAction);
}