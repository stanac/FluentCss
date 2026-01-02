using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CssB.Core;

public class ClassRegistry: IEnumerable<UtilityClass>
{
    public event EventHandler<UtilityClass>? OnClassAdded;
    private readonly object _sync = new();
    private readonly HashSet<UtilityClass> _classes = [];
    private string _css = "";

    public string Css
    {
        get
        {
            lock (_sync)
            {
                return _css;
            }
        }
    }

    public static ClassRegistry DefaultRegistry { get; } = new();

    public bool Add(UtilityClass c)
    {
        bool added;

        lock (_sync)
        {
            added = _classes.Add(c);

            if (added)
            {
                _css += $"\n{c.CssString}";
            }
        }

        if (added)
        {
            EventHandler<UtilityClass>? handler = OnClassAdded;
            handler?.Invoke(this, c);
        }

        return added;
    }

    public IEnumerator<UtilityClass> GetEnumerator()
    {
        List<UtilityClass> classes;

        lock (_sync)
        {
            classes = _classes.ToList();
        }
        
        foreach (UtilityClass c in classes)
        {
            yield return c;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}