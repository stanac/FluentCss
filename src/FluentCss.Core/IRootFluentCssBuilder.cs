using System;

namespace FluentCss.Core;

/// <summary>
/// Css builder with pseudo and media
/// </summary>
public interface IRootFluentCssBuilder: IFluentCssBuilder
{
    /// <summary>
    /// Adds container name with type inline-size
    /// </summary>
    /// <param name="name">Name of the container</param>
    IRootFluentCssBuilder ContainerInlineSizeName(string name);

    /// <summary>
    /// Adds container name with type size
    /// </summary>
    /// <param name="name">Name of the container</param>
    IRootFluentCssBuilder ContainerSizeName(string name);

    /// <summary>
    /// Responsive builder for min-width: Md (default: 48rem)
    /// </summary>
    /// <param name="cssBuilderAction">CSS builder action</param>
    IRootFluentCssBuilder MinMd(Action<IFluentCssBuilder> cssBuilderAction);

    /// <summary>
    /// Responsive builder for min-width: Md (default: 48rem)
    /// </summary>
    /// <param name="cssBuilderAction">CSS builder action</param>
    IRootFluentCssBuilder MaxMd(Action<IFluentCssBuilder> cssBuilderAction);

    /// <summary>
    /// Custom media query builder
    /// </summary>
    /// <param name="query">Media query</param>
    /// <param name="classPrefix">Class prefix for creating class names</param>
    /// <param name="cssBuilderAction">CSS builder action</param>
    IRootFluentCssBuilder Media(string query, string classPrefix, Action<IRootFluentCssBuilder> cssBuilderAction);
}