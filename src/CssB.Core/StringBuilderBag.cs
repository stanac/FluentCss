using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CssB.Core;

internal static class StringBuilderBag
{
    private static readonly Stack<StringBuilder> _items = new();
    private static readonly object _sync = new();

    public static StringBuilder TakeOrNew()
    {
        lock (_sync)
        {
            if (_items.Any())
            {
                return _items.Pop();
            }

            return new StringBuilder();
        }
    }

    public static void ClearAndReturn(StringBuilder sb)
    {
        sb.Clear();

        lock (_sync)
        {
            if (_items.Count < 12)
            {
                _items.Push(sb);
            }
        }
    }
}