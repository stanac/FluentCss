namespace CssB.Core;

public class CssBuilder
{
    public static CssBuilder Css => new();

    private readonly List<string> _classNames = [];

    public CssBuilder Bg(Color color)
    {
        Add(new UtilityClass($"bg-{color.Name}", "background-color", color.Value));
        return this;
    }

    public CssBuilder Text(Color color)
    {
        Add(new UtilityClass($"text-{color.Name}", "color", color.Value));
        return this;
    }

    public CssBuilder Border(Color color)
    {
        Add(new UtilityClass($"border-{color.Name}", "border-color", color.Value));
        return this;
    }

    private void Add(UtilityClass c)
    {
        _classNames.Add(c.ClassName);
        ClassRegistry.DefaultRegistry.Add(c);
    }

    public override string ToString() => string.Join(" ", _classNames);
}