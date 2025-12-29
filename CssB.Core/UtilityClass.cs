namespace CssB.Core;

public class UtilityClass
{
    public string ClassName { get; }
    public string Property { get; }
    public string Value { get; }

    public UtilityClass(string className, string property, string value)
    {
        Property = property;
        Value = value;
        ClassName = className;
    }

    public string CssString => field ??= $".{ClassName} {{ {Property}: {Value}; }}";

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
}