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

            var sb = StringBuilderBag.TakeOrNew();

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

    private string? _cssString;
    public string CssString => _cssString ??= BuildCssString();

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
            Theme = Theme,
            MediaPrefix = MediaPrefix,
            MediaQuery = MediaQuery
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
            MediaQuery = MediaQuery
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
            MediaQuery = query
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
            sb.Append('.').Append(Theme).Append(' ');
        }

        sb.Append('.');

        if (!string.IsNullOrWhiteSpace(MediaPrefix))
        {
            sb.Append(MediaPrefix).Append("\\:");
        }

        if (!string.IsNullOrWhiteSpace(Theme))
        {
            sb.Append(Theme).Append("\\:");
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

        if (!string.IsNullOrWhiteSpace(MediaQuery))
        {
            sb.Append('}');
        }

        string css = sb.ToString();
        StringBuilderBag.ClearAndReturn(sb);

        return css;
    }
}