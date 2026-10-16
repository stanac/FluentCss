using System.Text;

namespace FluentCss.Core;

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

            StringBuilder sb = StringBuilderBag.TakeOrNew();

            if (!string.IsNullOrWhiteSpace(MediaPrefix))
            {
                sb.Append(MediaPrefix).Append(':');
            }

            if (!string.IsNullOrWhiteSpace(Theme))
            {
                sb.Append(Theme).Append(':');
            }

            if (!string.IsNullOrWhiteSpace(Pseudo))
            {
                sb.Append(Pseudo).Append(':');
            }

            if (Important)
            {
                sb.Append('!');
            }

            sb.Append(_classNameNoPseudo);

            field = sb.ToString();
            StringBuilderBag.ClearAndReturn(sb);

            return field!;
        }
    }

    public string Property { get; }
    public string Value { get; }
    public string? Pseudo { get; private set; }
    public string? Theme { get; private set; }
    public string? MediaQuery { get; private set; }
    public string? MediaPrefix { get; private set; }

    /// <summary>
    /// True when the value is emitted with <c>!important</c>. A class can have at
    /// most one identity, so the flag is part of the class name (for example
    /// <c>!p-4</c>) as well as the declaration.
    /// </summary>
    public bool Important { get; private set; }

    private string? _cssString;
    public string CssString => _cssString ??= BuildCssString();

    public UtilityClass(string className, string property, string value)
    {
        Property = property;
        Value = value;
        _classNameNoPseudo = className;
    }

    internal UtilityClass AddPseudo(string? pseudo)
    {
        if (string.IsNullOrWhiteSpace(pseudo))
        {
            return this;
        }

        return new UtilityClass(_classNameNoPseudo, Property, Value)
        {
            Pseudo = pseudo!.Trim(':'),
            Theme = Theme,
            MediaPrefix = MediaPrefix,
            MediaQuery = MediaQuery,
            Important = Important
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
            Theme = theme,
            MediaPrefix = MediaPrefix,
            MediaQuery = MediaQuery,
            Important = Important
        };
    }

    internal UtilityClass AddMedia(string? query, string? prefix)
    {
        if (string.IsNullOrWhiteSpace(query) || string.IsNullOrWhiteSpace(prefix)) return this;

        query = query!.Trim();

        if (!query.StartsWith("(") || !query.EndsWith(")"))
        {
            query = $"({query})";
        }

        return new UtilityClass(_classNameNoPseudo, Property, Value)
        {
            Pseudo = Pseudo,
            Theme = Theme,
            MediaPrefix = prefix,
            MediaQuery = query,
            Important = Important
        };
    }

    internal UtilityClass AddImportant()
    {
        if (Important)
        {
            return this;
        }

        return new UtilityClass(_classNameNoPseudo, Property, Value)
        {
            Pseudo = Pseudo,
            Theme = Theme,
            MediaPrefix = MediaPrefix,
            MediaQuery = MediaQuery,
            Important = true
        };
    }

    public override int GetHashCode() => ClassName.GetHashCode();

    public override bool Equals(object? obj)
    {
        return obj is UtilityClass c && c.ClassName == ClassName;
    }

    public static bool operator ==(UtilityClass? c1, UtilityClass? c2)
    {
        if (c1 is null && c2 is null) return true;
        if (c1 is null || c2 is null) return false;
        return c1.ClassName == c2.ClassName;
    }

    public static bool operator !=(UtilityClass? c1, UtilityClass? c2) => !(c1 == c2);

    private string BuildCssString()
    {
        StringBuilder sb = StringBuilderBag.TakeOrNew();

        if (!string.IsNullOrWhiteSpace(MediaQuery))
        {
            sb.Append("@media ").Append(MediaQuery).Append(" { ");
        }

        if (!string.IsNullOrWhiteSpace(Theme))
        {
            sb.Append('.').Append(Escape(Theme!)).Append(' ');
        }

        sb.Append('.');

        if (!string.IsNullOrWhiteSpace(MediaPrefix))
        {
            sb.Append(Escape(MediaPrefix!)).Append("\\:");
        }

        if (!string.IsNullOrWhiteSpace(Theme))
        {
            sb.Append(Escape(Theme!)).Append("\\:");
        }

        if (!string.IsNullOrWhiteSpace(Pseudo))
        {
            sb.Append(Escape(Pseudo!)).Append("\\:");
        }

        if (Important)
        {
            sb.Append("\\!");
        }

        sb.Append(Escape(_classNameNoPseudo));

        if (!string.IsNullOrWhiteSpace(Pseudo))
        {
            sb.Append(':').Append(Pseudo);
        }

        sb.Append(" { ").Append(Property).Append(": ").Append(Value);

        if (Important)
        {
            sb.Append(" !important");
        }

        sb.Append(" } ");

        if (!string.IsNullOrWhiteSpace(MediaQuery))
        {
            sb.Append('}');
        }

        string css = sb.ToString();
        StringBuilderBag.ClearAndReturn(sb);

        return css;
    }

    /// <summary>
    /// Escapes a class-name fragment for use in a CSS selector. The class attribute
    /// keeps the readable form (for example <c>min-1.5rem</c>); only the selector
    /// needs the escapes (<c>.min-1\\.5rem\\:</c>).
    /// </summary>
    internal static string Escape(string value)
    {
        int firstSpecial = -1;

        for (int i = 0; i < value.Length; i++)
        {
            if (!IsSafeIdentifierChar(value[i]))
            {
                firstSpecial = i;
                break;
            }
        }

        // A CSS identifier cannot start with a digit. Even though a digit is a
        // "safe" character mid-identifier, it has to be hex-escaped when it is
        // first (for example "2xl" becomes "\32 xl", which is the class .2xl).
        bool leadingDigit = value.Length > 0 && value[0] >= '0' && value[0] <= '9';

        if (firstSpecial < 0 && !leadingDigit)
        {
            return value;
        }

        StringBuilder sb = StringBuilderBag.TakeOrNew();
        int start = 0;

        if (leadingDigit)
        {
            sb.Append('\\').Append(((int)value[0]).ToString("x")).Append(' ');
            start = 1;
        }

        sb.Append(value, start, (firstSpecial < 0 ? value.Length : firstSpecial) - start);

        for (int i = firstSpecial < 0 ? value.Length : firstSpecial; i < value.Length; i++)
        {
            char c = value[i];

            if (!IsSafeIdentifierChar(c))
            {
                sb.Append('\\');
            }

            sb.Append(c);
        }

        string result = sb.ToString();
        StringBuilderBag.ClearAndReturn(sb);
        return result;
    }

    private static bool IsSafeIdentifierChar(char c) =>
        (c >= 'a' && c <= 'z') ||
        (c >= 'A' && c <= 'Z') ||
        (c >= '0' && c <= '9') ||
        c == '-' || c == '_';
}