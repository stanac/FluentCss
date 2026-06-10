using System;

namespace FluentCss.Core;

/// <summary>
/// Css builder with pseudo and media
/// </summary>
public interface IRootFluentCssuilder: IFluentCssuilder
{
    /// <summary>
    /// Adds container name with type inline-size
    /// </summary>
    /// <param name="name">Name of the container</param>
    IRootFluentCssuilder ContainerInlineSizeName(string name);

    /// <summary>
    /// Adds container name with type size
    /// </summary>
    /// <param name="name">Name of the container</param>
    IRootFluentCssuilder ContainerSizeName(string name);

    /// <summary>
    /// Responsive builder for min-width: Md (default: 48rem)
    /// </summary>
    /// <param name="cssBuilderAction">CSS builder action</param>
    IRootFluentCssuilder MinMd(Action<IFluentCssuilder> cssBuilderAction);

    /// <summary>
    /// Responsive builder for min-width: Md (default: 48rem)
    /// </summary>
    /// <param name="cssBuilderAction">CSS builder action</param>
    IRootFluentCssuilder MaxMd(Action<IFluentCssuilder> cssBuilderAction);

    /// <summary>
    /// Custom media query builder
    /// </summary>
    /// <param name="query">Media query</param>
    /// <param name="classPrefix">Class prefix for creating class names</param>
    /// <param name="cssBuilderAction">CSS builder action</param>
    IRootFluentCssuilder Media(string query, string classPrefix, Action<IRootFluentCssuilder> cssBuilderAction);
}