using System.Collections;
using System.Collections.Concurrent;

namespace FluentCss.Core;

internal class ConcurrentSet<T> : IEnumerable<T> where T: notnull
{
    private const byte Dummy = 0;
    private readonly ConcurrentDictionary<T, byte> _dict = new();

    public bool TryAdd(T item) => _dict.TryAdd(item, Dummy);

    public IEnumerator<T> GetEnumerator()
    {
        foreach (KeyValuePair<T, byte> kv in _dict)
        {
            yield return kv.Key;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
