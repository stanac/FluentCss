using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentCss.Core;

internal static class StringBuilderBag
{
    private static readonly Stack<StringBuilder> Items = new();
    private static readonly object Sync = new();

    public static StringBuilder TakeOrNew()
    {
        lock (Sync)
        {
            if (Items.Any())
            {
                return Items.Pop();
            }

            return new StringBuilder();
        }
    }

    public static void ClearAndReturn(StringBuilder sb)
    {
        sb.Clear();

        lock (Sync)
        {
            if (Items.Count < 12)
            {
                Items.Push(sb);
            }
        }
    }
}