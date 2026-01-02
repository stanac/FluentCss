using System;
using System.Collections.Generic;

namespace CssB.Core;

/// <summary>
/// Base style builder interface
/// </summary>
public interface IStyleBuilder<out T>
{
    T Background(Color color);
    T Color(Color color);
    T BorderColor(Color color);
}

public interface IOnlyStyleBuilder : IStyleBuilder<IOnlyStyleBuilder>;

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

public abstract class BaseStyleBuilder<T>: IStyleBuilder<T>
{
    private readonly List<string> _classNames = [];

    protected internal string? MediaQuery { get; }
    protected internal string? MediaPrefix { get; }
    protected internal string? Theme { get; }

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
        _classNames.Add(c.ClassName);
        return ReturnSelf();
    }

    public override string ToString() => string.Join(" ", _classNames);

    protected void AddClassName(string className)
    {
        _classNames.Add(className);
    }
}

internal class OnlyStyleBuilder : BaseStyleBuilder<IOnlyStyleBuilder>, IOnlyStyleBuilder
{
    private readonly ClassRegistry _registry;
    private readonly string _pseudo;
    private readonly List<string> _classNames = [];

    internal OnlyStyleBuilder(ClassRegistry registry, string? theme, string? mediaQuery, string? mediaPrefix, string pseudo) 
        : base(theme, mediaQuery, mediaPrefix)
    {
        _registry = registry;
        _pseudo = pseudo;
    }

    protected internal override void AddClass(UtilityClass c)
    {
        c = c.AddTheme(Theme).AddMedia(MediaQuery, MediaPrefix).AddPseudo(_pseudo);
        _classNames.Add(c.ClassName);
        _registry.Add(c);
    }

    protected override IOnlyStyleBuilder ReturnSelf() => this;

    public override string ToString() => string.Join(" ", _classNames);
}

internal class InternalCssBuilder : BaseStyleBuilder<ICssBuilder>, ICssBuilder
{
    private readonly ClassRegistry _registry;

    internal InternalCssBuilder(ClassRegistry registry, string? theme, string? mediaQuery, string? mediaPrefix) : base(theme, mediaQuery, mediaPrefix)
    {
        _registry = registry;
    }

    protected internal override void AddClass(UtilityClass c)
    {
        c = c.AddTheme(Theme).AddMedia(MediaQuery, MediaPrefix);
        _registry.Add(c);
    }

    protected override ICssBuilder ReturnSelf() => this;

    public ICssBuilder Hover(Action<IOnlyStyleBuilder> pseudoBuilderAction) => Pseudo("hover", pseudoBuilderAction);

    public ICssBuilder Active(Action<IOnlyStyleBuilder> pseudoBuilderAction) => Pseudo("active", pseudoBuilderAction);

    public ICssBuilder Focus(Action<IOnlyStyleBuilder> pseudoBuilderAction) => Pseudo("focus", pseudoBuilderAction);

    public ICssBuilder Pseudo(string pseudoPrefix, Action<IOnlyStyleBuilder> pseudoBuilderAction)
    {
        IOnlyStyleBuilder builder = new OnlyStyleBuilder(_registry, Theme, MediaQuery, MediaPrefix, pseudoPrefix);
        pseudoBuilderAction(builder);
        base.AddClassName(builder.ToString());
        return ReturnSelf();
    }
}

internal class InternalRootCssBuilder : InternalCssBuilder, IRootCssBuilder
{
    private readonly ClassRegistry _registry;

    internal InternalRootCssBuilder(ClassRegistry registry, string? theme, string? mediaQuery, string? mediaPrefix) 
        : base(registry, theme, mediaQuery, mediaPrefix)
    {
        _registry = registry;
    }

    public IRootCssBuilder ContainerInlineSizeName(string name)
    {
        throw new NotImplementedException();
    }

    public IRootCssBuilder ContainerSizeName(string name)
    {
        throw new NotImplementedException();
    }

    public IRootCssBuilder MinMd(Action<ICssBuilder> cssBuilderAction) => Media("width > 48rem", "max-md", cssBuilderAction);

    public IRootCssBuilder MaxMd(Action<ICssBuilder> cssBuilderAction) => Media("width < 48rem", "max-md", cssBuilderAction);

    public IRootCssBuilder Media(string query, string classPrefix, Action<IRootCssBuilder> cssBuilderAction)
    {
        InternalRootCssBuilder builder = new InternalRootCssBuilder(_registry, Theme, query, classPrefix);
        cssBuilderAction(builder);
        AddClassName(builder.ToString());
        return this;
    }
}

public static class CssBuilder
{
    public static IRootCssBuilder Css => new InternalRootCssBuilder(ClassRegistry.DefaultRegistry, null, null, null);

    public static IRootCssBuilder Dark => Theme("dark");

    public static IRootCssBuilder Theme(string theme) => new InternalRootCssBuilder(ClassRegistry.DefaultRegistry, $"theme-{theme}", null, null);
}
