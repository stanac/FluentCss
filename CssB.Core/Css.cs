using System;
using System.Collections.Generic;

namespace CssB.Core;

public interface IStyleBuilder<out T> where T: IStyleBuilder<T>
{
    T Background(Color color);
    T Color(Color color);
    T BorderColor(Color color);
}

public interface ICssBuilder : IStyleBuilder<ICssBuilder>, IRootCssBuilder;

public interface IInsidePseudoCssBuilder: IStyleBuilder<IInsidePseudoCssBuilder>;

public abstract class BaseStyleBuilder<T>: IStyleBuilder<T> where T: IStyleBuilder<T>
{
    protected internal string? MediaQuery { get; }
    protected internal string? MediaPrefix { get; }
    private protected string? Theme { get; }

    private protected BaseStyleBuilder(string? theme, string? mediaQuery, string? mediaPrefix)
    {
        MediaQuery = mediaQuery;
        MediaPrefix = mediaPrefix;
        Theme = theme;
    }

    protected internal abstract void AddClass(UtilityClass c);

    protected abstract T ReturnSelf();

    public T Background(Color color) => AddInternal(new UtilityClass($"bg-{color.Name}", "background-color", color.Value));

    public T Color(Color color) => AddInternal(new UtilityClass($"text-{color.Name}", "color", color.Value));

    public T BorderColor(Color color) => AddInternal(new UtilityClass($"border-{color.Name}", "border-color", color.Value));

    private T AddInternal(UtilityClass c)
    {
        c = c.AddTheme(Theme).AddMedia(MediaQuery, MediaPrefix);
        AddClass(c);
        return ReturnSelf();
    }
}

internal class InternalCssBuilder: BaseStyleBuilder<ICssBuilder>, ICssBuilder
{
    private readonly List<string> _classNames = [];
    private readonly InternalCssBuilder? _parentBuilder;

    internal InternalCssBuilder(string? theme, string? mediaQuery, string? mediaPrefix, InternalCssBuilder? parentBuilder = null) 
        : base(theme, mediaQuery, mediaPrefix)
    {
        _parentBuilder = parentBuilder;
    }

    protected internal override void AddClass(UtilityClass c)
    {
        c = c.AddTheme(Theme).AddMedia(MediaQuery, MediaPrefix);

        if (_parentBuilder != null)
        {
            _parentBuilder.AddClass(c);
        }
        else
        {
            _classNames.Add(c.ClassName);
            ClassRegistry.DefaultRegistry.Add(c);
        }
    }

    public IRootCssBuilder Hover(Action<IInsidePseudoCssBuilder> pseudoBuilder) => Pseudo(":hover", pseudoBuilder);

    public IRootCssBuilder Active(Action<IInsidePseudoCssBuilder> pseudoBuilder) => Pseudo(":action", pseudoBuilder);

    public IRootCssBuilder Focus(Action<IInsidePseudoCssBuilder> pseudoBuilder) => Pseudo(":focus", pseudoBuilder);

    public IRootCssBuilder Pseudo(string pseudoPrefix, Action<IInsidePseudoCssBuilder> pseudoBuilderAction)
    {
        IInsidePseudoCssBuilder builder = new InsidePseudoBuilder(this, pseudoPrefix, Theme);
        pseudoBuilderAction(builder);
        return ReturnSelf();
    }

    public IRootCssBuilder ContainerInlineSizeName(string name)
    {
        AddClass(new UtilityClass($"container-{name}-inline-size", "container", $"{name} / inline-size;"));
        return ReturnSelf();
    }

    public IRootCssBuilder ContainerSizeName(string name)
    {
        AddClass(new UtilityClass($"container-{name}-size", "container", $"{name} / size;"));
        return ReturnSelf();
    }

    public IRootCssBuilder MinMd(Action<ICssBuilder> cssBuilderAction) => Media("min-width: 48rem", "min-md", cssBuilderAction);

    public IRootCssBuilder MaxMd(Action<ICssBuilder> cssBuilderAction) => Media("max-width: 48rem", "max-md", cssBuilderAction);

    public IRootCssBuilder Media(string query, string classPrefix, Action<ICssBuilder> cssBuilderAction)
    {
        ICssBuilder builder = new InternalCssBuilder(Theme, query, classPrefix, this);
        cssBuilderAction(builder);
        return this;
    }

    protected override ICssBuilder ReturnSelf() => this;

    public override string ToString() => string.Join(" ", _classNames);
}

internal class InsidePseudoBuilder : BaseStyleBuilder<IInsidePseudoCssBuilder>, IInsidePseudoCssBuilder
{
    private readonly InternalCssBuilder _parentBuilder;
    private readonly string _pseudo;

    public InsidePseudoBuilder(InternalCssBuilder parentBuilder, string pseudo, string? theme)
        :base(theme, parentBuilder.MediaQuery, parentBuilder.MediaPrefix)
    {
        _parentBuilder = parentBuilder;
        _pseudo = pseudo;
    }

    protected internal override void AddClass(UtilityClass c)
    {
        _parentBuilder.AddClass(
            c.AddPseudo(_pseudo)
                .AddTheme(Theme));
    }

    protected override IInsidePseudoCssBuilder ReturnSelf() => this;
}

public static class CssBuilder
{
    public static ICssBuilder Css => new InternalCssBuilder(null, null, null);

    public static ICssBuilder Dark => Theme("dark");

    public static ICssBuilder Theme(string theme) => new InternalCssBuilder($"theme-{theme}", null, null);
}

public interface IRootCssBuilder
{
    /// <summary>
    /// Matches when a user designates an item with a pointing device, such as holding the mouse pointer over the item.
    /// </summary>
    /// <param name="pseudoBuilderAction">Css builder setup action for pseudo</param>
    IRootCssBuilder Hover(Action<IInsidePseudoCssBuilder> pseudoBuilderAction);

    /// <summary>
    /// Matches when an item is being activated by the user. For example, when the item is clicked on.
    /// </summary>
    /// <param name="pseudoBuilderAction">Css builder setup action for pseudo</param>
    IRootCssBuilder Active(Action<IInsidePseudoCssBuilder> pseudoBuilderAction);

    /// <summary>
    /// Matches when an element has focus.
    /// </summary>
    /// <param name="pseudoBuilderAction">Css builder setup action for pseudo</param>
    IRootCssBuilder Focus(Action<IInsidePseudoCssBuilder> pseudoBuilderAction);

    /// <summary>
    /// Matches custom pseudo prefix (e.g. :hover)
    /// </summary>
    /// <param name="pseudoPrefix">Pseudo prefix, must start with : (colon)</param>
    /// <param name="pseudoBuilderAction">Css builder setup action for pseudo</param>
    IRootCssBuilder Pseudo(string pseudoPrefix, Action<IInsidePseudoCssBuilder> pseudoBuilderAction);

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
    IRootCssBuilder Media(string query, string classPrefix, Action<ICssBuilder> cssBuilderAction);
}