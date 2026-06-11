using System.Collections;
using System.Text;

namespace FluentCss.Core;

public class ClassRegistry : IEnumerable<UtilityClass>
{
    public event EventHandler<UtilityClass>? OnClassAdded;
    private readonly ConcurrentSet<UtilityClass> _classes = new();
    private readonly StringBuilder _cssBuilder = new();
    private readonly object _cssBuilderLock = new();
    private readonly ConcurrentLazyObject<string> _cssObject;
    private readonly ConcurrentLazyObject<byte[]> _bytesObject;

    public string Css => _cssObject.Get();
    public byte[] CssUtf8 => _bytesObject.Get();

    public static ClassRegistry DefaultRegistry { get; } = new();

    public ClassRegistry()
    {
        _cssObject = new(() =>
        {
            lock (_cssBuilderLock)
            {
                return _cssBuilder.ToString();
            }
        });
        _bytesObject = new(() => Encoding.UTF8.GetBytes(Css));
    }

    public bool Add(UtilityClass c)
    {
        if (!_classes.TryAdd(c))
        {
            return false;
        }

        lock (_cssBuilderLock)
        {
            _cssBuilder.Append('\n');
            _cssBuilder.Append(c.CssString);
        }
        
        _cssObject.Invalidate();
        _bytesObject.Invalidate();

        EventHandler<UtilityClass>? handler = OnClassAdded;
        handler?.Invoke(this, c);

        return true;
    }

    public IEnumerator<UtilityClass> GetEnumerator() => _classes.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}