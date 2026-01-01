using System;
using System.Collections.Generic;

namespace CssB.Core;

public interface IStyleBuilder<out T> where T: IStyleBuilder<T>
{
    T Background(Color color);
    T Color(Color color);
    T Border(Color color);
}

public interface ICssBuilder: IStyleBuilder<ICssBuilder>, IPseudoBuilder;

public interface IInsidePseudoCssBuilder: IStyleBuilder<IInsidePseudoCssBuilder>;

public abstract class BaseStyleBuilder<T>: IStyleBuilder<T> where T: IStyleBuilder<T>
{
    private protected string? Theme { get; }

    private protected BaseStyleBuilder(string? theme)
    {
        Theme = theme;
    }

    protected internal abstract void AddClass(UtilityClass c);

    protected abstract T ReturnSelf();

    public T Background(Color color)
    {
        AddClass(new UtilityClass($"bg-{color.Name}", "background-color", color.Value).AddTheme(Theme));
        return ReturnSelf();
    }

    public T Color(Color color)
    {
        AddClass(new UtilityClass($"text-{color.Name}", "color", color.Value).AddTheme(Theme));
        return ReturnSelf();
    }

    public T Border(Color color)
    {
        AddClass(new UtilityClass($"border-{color.Name}", "border-color", color.Value).AddTheme(Theme));
        return ReturnSelf();
    }
}

internal class InternalCssBuilder: BaseStyleBuilder<ICssBuilder>, ICssBuilder
{
    private readonly List<string> _classNames = [];

    internal InternalCssBuilder(string? theme) : base(theme)
    {
    }

    protected internal override void AddClass(UtilityClass c)
    {
        if (!string.IsNullOrWhiteSpace(Theme) && string.IsNullOrWhiteSpace(c.Theme))
        {
            c = c.AddTheme(Theme);
        }

        _classNames.Add(c.ClassName);
        ClassRegistry.DefaultRegistry.Add(c);
    }

    public IPseudoBuilder Hover(Action<IInsidePseudoCssBuilder> pseudoBuilder) => Pseudo(":hover", pseudoBuilder);

    public IPseudoBuilder Active(Action<IInsidePseudoCssBuilder> pseudoBuilder) => Pseudo(":action", pseudoBuilder);

    public IPseudoBuilder Focus(Action<IInsidePseudoCssBuilder> pseudoBuilder) => Pseudo(":focus", pseudoBuilder);

    public IPseudoBuilder Pseudo(string pseudoPrefix, Action<IInsidePseudoCssBuilder> pseudoBuilder)
    {
        IInsidePseudoCssBuilder builder = new InsidePseudoBuilder(this, pseudoPrefix, Theme);
        pseudoBuilder(builder);
        return ReturnSelf();
    }

    protected override ICssBuilder ReturnSelf() => this;

    public override string ToString() => string.Join(" ", _classNames);
}

internal class InsidePseudoBuilder : BaseStyleBuilder<IInsidePseudoCssBuilder>, IInsidePseudoCssBuilder
{
    private readonly InternalCssBuilder _parentBuilder;
    private readonly string _pseudo;

    public InsidePseudoBuilder(InternalCssBuilder parentBuilder, string pseudo, string? theme): base(theme)
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
    public static ICssBuilder Css => new InternalCssBuilder(null);

    public static ICssBuilder Dark => Theme("dark");

    public static ICssBuilder Theme(string theme) => new InternalCssBuilder($"theme-{theme}");
}

public interface IPseudoBuilder
{
    /// <summary>
    /// Matches when a user designates an item with a pointing device, such as holding the mouse pointer over the item.
    /// </summary>
    /// <param name="pseudoBuilder">Css builder setup action for pseudo</param>
    IPseudoBuilder Hover(Action<IInsidePseudoCssBuilder> pseudoBuilder);

    /// <summary>
    /// Matches when an item is being activated by the user. For example, when the item is clicked on.
    /// </summary>
    /// <param name="pseudoBuilder">Css builder setup action for pseudo</param>
    IPseudoBuilder Active(Action<IInsidePseudoCssBuilder> pseudoBuilder);

    /// <summary>
    /// Matches when an element has focus.
    /// </summary>
    /// <param name="pseudoBuilder">Css builder setup action for pseudo</param>
    IPseudoBuilder Focus(Action<IInsidePseudoCssBuilder> pseudoBuilder);

    /// <summary>
    /// Matches custom pseudo prefix (e.g. :hover)
    /// </summary>
    /// <param name="pseudoPrefix">Pseudo prefix, must start with : (colon)</param>
    /// <param name="pseudoBuilder">Css builder setup action for pseudo</param>
    IPseudoBuilder Pseudo(string pseudoPrefix, Action<IInsidePseudoCssBuilder> pseudoBuilder);
}
