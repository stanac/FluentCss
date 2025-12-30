using System;
using System.Collections.Generic;

namespace CssB.Core;

public interface IStyleBuilder<out T>
{
    T Background(Color color);
    T Color(Color color);
    T Border(Color color);
}

public interface ICssBuilder: IStyleBuilder<ICssBuilder>, IPseudoBuilder;

public interface IInsidePseudoCssBuilder: IStyleBuilder<IInsidePseudoCssBuilder>;

public abstract class BaseStyleBuilder<T>: IStyleBuilder<T>
{
    protected internal abstract void AddClass(UtilityClass c);

    protected abstract T ReturnSelf();

    public T Background(Color color)
    {
        AddClass(new UtilityClass($"bg-{color.Name}", "background-color", color.Value));
        return ReturnSelf();
    }

    public T Color(Color color)
    {
        AddClass(new UtilityClass($"text-{color.Name}", "color", color.Value));
        return ReturnSelf();
    }

    public T Border(Color color)
    {
        AddClass(new UtilityClass($"border-{color.Name}", "border-color", color.Value));
        return ReturnSelf();
    }
}

internal class InternalCssBuilder: BaseStyleBuilder<ICssBuilder>, ICssBuilder
{
    private readonly List<string> _classNames = [];

    protected internal override void AddClass(UtilityClass c)
    {
        _classNames.Add(c.ClassName);
        ClassRegistry.DefaultRegistry.Add(c);
    }

    public IPseudoBuilder Hover(Action<IInsidePseudoCssBuilder> pseudoBuilder) => Pseudo(":hover", pseudoBuilder);

    public IPseudoBuilder Active(Action<IInsidePseudoCssBuilder> pseudoBuilder) => Pseudo(":action", pseudoBuilder);

    public IPseudoBuilder Focus(Action<IInsidePseudoCssBuilder> pseudoBuilder) => Pseudo(":focus", pseudoBuilder);

    public IPseudoBuilder Pseudo(string pseudoPrefix, Action<IInsidePseudoCssBuilder> pseudoBuilder)
    {
        IInsidePseudoCssBuilder builder = new InsidePseudoBuilder(this, pseudoPrefix);
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

    public InsidePseudoBuilder(InternalCssBuilder parentBuilder, string pseudo)
    {
        _parentBuilder = parentBuilder;
        _pseudo = pseudo;
    }

    protected internal override void AddClass(UtilityClass c)
    {
        _parentBuilder.AddClass(c.AddPseudo(_pseudo));
    }

    protected override IInsidePseudoCssBuilder ReturnSelf() => this;
}

public static class CssBuilder
{
    public static ICssBuilder Css => new InternalCssBuilder();
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
