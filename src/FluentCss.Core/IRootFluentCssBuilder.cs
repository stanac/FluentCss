namespace FluentCss.Core;

public interface IRootFluentCssBuilder : IMediaFluentCssBuilder
{
    IRootFluentCssBuilder ApplySelector(string selector, Action<IOnlyStyleBuilder> cssBuilderAction);
}

public static class RootFluentCssBuilderInterfaceExtensions
{
    /// <summary>
    /// Registers a selector built with the fluent <see cref="IRootSelectorBuilder"/>
    /// API. The selector is rendered to its CSS text and handed to
    /// <see cref="IRootFluentCssBuilder.ApplySelector(string, Action{IOnlyStyleBuilder})"/>.
    /// </summary>
    /// <example>
    /// <code>
    /// Css.ApplySelector(s => s.Class("btn").Hover(), css => css.Px(4).Py(2));
    /// </code>
    /// </example>
    public static IRootFluentCssBuilder ApplySelector(
        this IRootFluentCssBuilder cssBuilder,
        Action<IRootSelectorBuilder> selectorBuilderAction,
        Action<IOnlyStyleBuilder> cssBuilderAction)
    {
        if (cssBuilder is null)
        {
            throw new ArgumentNullException(nameof(cssBuilder));
        }

        if (selectorBuilderAction is null)
        {
            throw new ArgumentNullException(nameof(selectorBuilderAction));
        }

        IRootSelectorBuilder selectorBuilder = Selector.New();
        selectorBuilderAction(selectorBuilder);

        string selector = selectorBuilder.Css;

        if (string.IsNullOrWhiteSpace(selector))
        {
            throw new InvalidOperationException("The selector builder produced an empty selector.");
        }

        return cssBuilder.ApplySelector(selector, cssBuilderAction);
    }

    public static IRootFluentCssBuilder ApplyClass(this IRootFluentCssBuilder cssBuilder, string className, Action<IOnlyStyleBuilder> cssBuilderAction)
    {
        if (!className.StartsWith("."))
        {
            className = "." + className;
        }

        return cssBuilder.ApplySelector(className, cssBuilderAction);
    }

    public static IRootFluentCssBuilder ApplyId(this IRootFluentCssBuilder cssBuilder, string elementId, Action<IOnlyStyleBuilder> cssBuilderAction)
    {
        if (!elementId.StartsWith("#"))
        {
            elementId = "#" + elementId;
        }

        return cssBuilder.ApplySelector(elementId, cssBuilderAction);
    }
}