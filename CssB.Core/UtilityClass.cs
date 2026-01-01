using System.Text;

namespace CssB.Core;

public class UtilityClass
{
    private readonly string _classNameNoPseudo;
    
    public string ClassName
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(field))
            {
                return field!;
            }

            string className = string.IsNullOrWhiteSpace(Pseudo)
                ? _classNameNoPseudo
                : $"{Pseudo}:{_classNameNoPseudo}";
            
            if (!string.IsNullOrWhiteSpace(Theme))
            {
                className = $"{Theme}:{className}";
            }

            field = className;
            return className;
        }
    }

    public string Property { get; }
    public string Value { get; }
    public string? Pseudo { get; private set; }
    public string? Theme { get; private set; }

    public UtilityClass(string className, string property, string value)
    {
        Property = property;
        Value = value;
        _classNameNoPseudo = className;
    }

    internal UtilityClass AddPseudo(string pseudo)
    {
        return new UtilityClass(_classNameNoPseudo, Property, Value)
        {
            Pseudo = pseudo.Trim(':'),
            Theme = Theme
        };
    }

    internal UtilityClass AddTheme(string? theme)
    {
        if (string.IsNullOrWhiteSpace(theme))
        {
            return this;
        }

        return new UtilityClass(_classNameNoPseudo, Property, Value)
        {
            Pseudo = Pseudo,
            Theme = theme
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
        StringBuilder sb = StringBuilderBag.TakeOrNew();

        sb.Append('.');

        if (!string.IsNullOrWhiteSpace(Theme))
        {
            sb.Append(Theme).Append(" .").Append(Theme).Append("\\:");
        }

        if (!string.IsNullOrWhiteSpace(Pseudo))
        {
            sb.Append(Pseudo).Append("\\:");
        }

        sb.Append(_classNameNoPseudo);

        if (!string.IsNullOrWhiteSpace(Pseudo))
        {
            sb.Append(':').Append(Pseudo);
        }

        sb.Append(" { ").Append(Property).Append(": ").Append(Value).Append(" } ");

        string css = sb.ToString();

        StringBuilderBag.ClearAndReturn(sb);

        return css;
    }
}