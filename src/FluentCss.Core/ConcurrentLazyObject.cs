namespace FluentCss.Core;

internal class ConcurrentLazyObject<T> where T: class
{
    private readonly Func<T> _build;
    private readonly object _sync = new();
    private volatile T? _value;

    public ConcurrentLazyObject(Func<T> build)
    {
        _build = build;
    }

    public T Get()
    {
        T? value = _value;

        if (value is null)
        {
            lock (_sync)
            {
                if (_value is null)
                {
                    _value = _build();
                }

                value = _value;
            }
        }

        return value;
    }

    public void Invalidate()
    {
        lock (_sync)
        {
            _value = null;
        }
    }
}