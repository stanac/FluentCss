namespace CssB.Core;

public class UtilityClass
{
    private readonly string _classNameNoPseudo;

    public string ClassName =>
        string.IsNullOrWhiteSpace(Pseudo)
            ? _classNameNoPseudo
            : $"{Pseudo}:{_classNameNoPseudo}";

    public string Property { get; }
    public string Value { get; }
    public string? Pseudo { get; private set; }

    public UtilityClass(string className, string property, string value)
    {
        Property = property;
        Value = value;
        _classNameNoPseudo = className;
    }

    public UtilityClass AddPseudo(string pseudo)
    {
        return new UtilityClass(ClassName, Property, Value)
        {
            Pseudo = pseudo.Trim(':')
        };
    }

    public string CssString => field ??= BuildCssString();

    public override int GetHashCode() => ClassName.GetHashCode();

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        
        if (obj is UtilityClass c)
        {
            return c.ClassName == ClassName;
        }

        return false;
    }

    public static bool operator ==(UtilityClass? c1, UtilityClass? c2)
    {
        if (c1 is null && c2 is null) return true;
        if (c1 is null || c2 is null) return false;

        return c1.ClassName == c2.ClassName;
    }

    public static bool operator !=(UtilityClass? c1, UtilityClass? c2)
    {
        return !(c1 == c2);
    }

    private string BuildCssString()
    {
        if (string.IsNullOrWhiteSpace(Pseudo))
        {
            return $".{ClassName} {{ {Property}: {Value}; }}";
        }

        return $".{Pseudo}\\:{_classNameNoPseudo}:{Pseudo} {{ {Property}: {Value} }}";
    }
}