namespace CssB.Core;

public interface ICssBuilder
{
    ICssBuilder Bg(Color color);
    ICssBuilder Text(Color color);
}