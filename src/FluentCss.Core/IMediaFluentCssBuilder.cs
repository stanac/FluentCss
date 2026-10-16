namespace FluentCss.Core;

/// <summary>
/// Css builder with pseudo and media
/// </summary>
public interface IMediaFluentCssBuilder: IFluentCssBuilder
{
    /// <summary>
    /// Custom media query builder. This is the single primitive the responsive
    /// helpers are built on: see <see cref="MediaFluentCssBuilderExtensions.Min"/>,
    /// <see cref="MediaFluentCssBuilderExtensions.Max"/> and the named breakpoint
    /// helpers (for example <see cref="MediaFluentCssBuilderExtensions.MinMd"/>).
    /// </summary>
    /// <param name="query">Media query</param>
    /// <param name="classPrefix">Class prefix for creating class names</param>
    /// <param name="cssBuilderAction">CSS builder action</param>
    IMediaFluentCssBuilder Media(string query, string classPrefix, Action<IMediaFluentCssBuilder> cssBuilderAction);
}