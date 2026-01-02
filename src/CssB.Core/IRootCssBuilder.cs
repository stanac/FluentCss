using System;

namespace CssB.Core;

/// <summary>
/// Css builder with pseudo and media
/// </summary>
public interface IRootCssBuilder: ICssBuilder
{
    /// <summary>
    /// Adds container name with type inline-size
    /// </summary>
    /// <param name="name">Name of the container</param>
    IRootCssBuilder ContainerInlineSizeName(string name);

    /// <summary>
    /// Adds container name with type size
    /// </summary>
    /// <param name="name">Name of the container</param>
    IRootCssBuilder ContainerSizeName(string name);

    /// <summary>
    /// Responsive builder for min-width: Md (default: 48rem)
    /// </summary>
    /// <param name="cssBuilderAction">CSS builder action</param>
    IRootCssBuilder MinMd(Action<ICssBuilder> cssBuilderAction);

    /// <summary>
    /// Responsive builder for min-width: Md (default: 48rem)
    /// </summary>
    /// <param name="cssBuilderAction">CSS builder action</param>
    IRootCssBuilder MaxMd(Action<ICssBuilder> cssBuilderAction);

    /// <summary>
    /// Custom media query builder
    /// </summary>
    /// <param name="query">Media query</param>
    /// <param name="classPrefix">Class prefix for creating class names</param>
    /// <param name="cssBuilderAction">CSS builder action</param>
    IRootCssBuilder Media(string query, string classPrefix, Action<IRootCssBuilder> cssBuilderAction);
}