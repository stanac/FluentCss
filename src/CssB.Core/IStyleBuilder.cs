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