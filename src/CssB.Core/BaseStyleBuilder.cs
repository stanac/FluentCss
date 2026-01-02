using System.Collections.Generic;

namespace CssB.Core;

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