using System;
using System.Collections.Generic;

namespace FluentCss.Core;

/// <summary>
/// Decides whether a pseudo name refers to a pseudo-element (<c>::name</c>) or a
/// pseudo-class (<c>:name</c>).
/// </summary>
/// <remarks>
/// The builder takes a single <c>Pseudo(name, ...)</c> method and derives the
/// colon count here. A caller that needs to be explicit can force the shape by
/// including the leading colons in the name (<c>::placeholder</c> or
/// <c>:placeholder</c>); otherwise a known pseudo-element name gets a double
/// colon and everything else a single colon.
/// </remarks>
internal static class PseudoNames
{
    // Pseudo-elements from CSS Pseudo-Elements Level 4 (and the widely shipped
    // vendor-agnostic ones). Functional forms are matched by their head name, for
    // example "part(label)" matches "part".
    private static readonly HashSet<string> Elements = new(StringComparer.OrdinalIgnoreCase)
    {
        "after",
        "before",
        "first-letter",
        "first-line",
        "selection",
        "backdrop",
        "placeholder",
        "marker",
        "file-selector-button",
        "cue",
        "cue-region",
        "grammar-error",
        "spelling-error",
        "target-text",
        "highlight",
        "part",
        "slotted",
        "view-transition",
        "view-transition-group",
        "view-transition-image-pair",
        "view-transition-old",
        "view-transition-new",
        "details-content",
        "checkmark",
        "picker",
        "picker-icon",
        "scroll-marker",
        "scroll-marker-group",
        "scroll-button"
    };

    /// <summary>
    /// Strips a leading colon prefix (if any) and decides the colon count.
    /// </summary>
    /// <returns>
    /// The bare pseudo name without leading colons, and whether it is a
    /// pseudo-element.
    /// </returns>
    internal static (string Name, bool IsElement) Normalize(string pseudo)
    {
        if (pseudo.StartsWith("::", StringComparison.Ordinal))
        {
            return (pseudo.Substring(2).Trim(), true);
        }

        if (pseudo.StartsWith(":", StringComparison.Ordinal))
        {
            return (pseudo.Substring(1).Trim(), false);
        }

        string bare = pseudo.Trim();
        int parenthesis = bare.IndexOf('(');
        string head = parenthesis >= 0 ? bare.Substring(0, parenthesis) : bare;

        return (bare, Elements.Contains(head.Trim()));
    }
}
