using System.Security.Cryptography;
using System.Text;
using ZBase32;

namespace FluentCss.Core;

/// <summary>
/// A CSS <c>font-family</c> stack. Like <see cref="Color"/> and <see cref="Length"/>,
/// it carries the raw CSS <see cref="Value"/> and an optional <see cref="Name"/> that
/// is used as the class-name fragment.
/// </summary>
/// <remarks>
/// A font stack is a comma separated list of family names (<c>"Inter", ui-sans-serif,
/// system-ui, sans-serif</c>). Unlike a length, it cannot be used as a class token:
/// the HTML <c>class</c> attribute is whitespace separated, so a stack simply cannot
/// be one class name. When <see cref="Name"/> is not set a token is derived from the
/// families in <see cref="Value"/>: the family names are sanitized and suffixed with a
/// short Z-Base-32 hash of the canonical families, for example <c>inter-system-ui-7gq3</c>.
/// The hash keeps tokens unique when two different stacks sanitize to the same readable
/// form (<c>"Inter Tight"</c> and <c>"Inter-Tight"</c> both sanitize to
/// <c>inter-tight</c>).
/// </remarks>
public readonly struct FontFamily
{
    // Tailwind's default stacks (https://tailwindcss.com/docs/font-family).
    internal const string SansStack =
        "ui-sans-serif, system-ui, sans-serif, \"Apple Color Emoji\", \"Segoe UI Emoji\", " +
        "\"Segoe UI Symbol\", \"Noto Color Emoji\"";

    internal const string SerifStack =
        "ui-serif, Georgia, Cambria, \"Times New Roman\", Times, serif";

    internal const string MonoStack =
        "ui-monospace, SFMono-Regular, Menlo, Monaco, Consolas, \"Liberation Mono\", " +
        "\"Courier New\", monospace";

    /// <summary>Raw CSS value: the full, comma separated font stack.</summary>
    public string Value { get; }

    /// <summary>
    /// Optional name used as the class-name fragment. Must be unique and stable: it is
    /// the identity used to deduplicate generated CSS. When unset a token is derived
    /// from <see cref="Value"/>.
    /// </summary>
    public string? Name { get; }

    public FontFamily(string value, string? name = null)
    {
        Value = value;
        Name = name;
    }

    /// <summary>The default <c>font-sans</c> stack (class token <c>sans</c>).</summary>
    public static FontFamily Sans { get; } = new(SansStack, "sans");

    /// <summary>The default <c>font-serif</c> stack (class token <c>serif</c>).</summary>
    public static FontFamily Serif { get; } = new(SerifStack, "serif");

    /// <summary>The default <c>font-mono</c> stack (class token <c>mono</c>).</summary>
    public static FontFamily Mono { get; } = new(MonoStack, "mono");

    /// <summary>
    /// The fragment used in class names: the <see cref="Name"/> when set, otherwise a
    /// token derived from the families in <see cref="Value"/>.
    /// </summary>
    public string Token => string.IsNullOrWhiteSpace(Name) ? DeriveToken(Value, null) : Name!;

    /// <summary>
    /// Prepends custom families to a default stack. Used by the builder's
    /// <c>Sans(...)</c>/<c>Serif(...)</c>/<c>Mono(...)</c> conveniences. The derived
    /// token includes <paramref name="prefix"/> so the same custom family used with
    /// different bases does not collide (<c>sans-inter-…</c> vs <c>serif-inter-…</c>).
    /// </summary>
    internal static FontFamily Prepend(string prefix, string defaultStack, string[]? families)
    {
        if (families == null || families.Length == 0)
        {
            return new FontFamily(defaultStack, prefix);
        }

        List<string> cleaned = new(families.Length);
        foreach (string? family in families)
        {
            if (!string.IsNullOrWhiteSpace(family))
            {
                cleaned.Add(CollapseWhitespace(family!));
            }
        }

        if (cleaned.Count == 0)
        {
            return new FontFamily(defaultStack, prefix);
        }

        StringBuilder value = new();
        for (int i = 0; i < cleaned.Count; i++)
        {
            if (i > 0)
            {
                value.Append(", ");
            }

            value.Append(Quote(cleaned[i]));
        }

        value.Append(", ").Append(defaultStack);

        string readable = Sanitize(cleaned);
        string hash = ShortHash(cleaned);
        string token = readable.Length == 0
            ? $"{prefix}-{hash}"
            : $"{prefix}-{readable}-{hash}";

        return new FontFamily(value.ToString(), token);
    }

    private static string DeriveToken(string value, string? prefix)
    {
        List<string> families = SplitFamilies(value);
        string readable = Sanitize(families);
        string hash = ShortHash(families);

        string token = readable.Length == 0 ? hash : $"{readable}-{hash}";
        return string.IsNullOrEmpty(prefix) ? token : $"{prefix}-{token}";
    }

    private static List<string> SplitFamilies(string value)
    {
        List<string> families = new();

        if (string.IsNullOrWhiteSpace(value))
        {
            return families;
        }

        foreach (string raw in value!.Split(','))
        {
            string family = raw.Trim();

            if (family.Length >= 2 &&
                ((family[0] == '"' && family[family.Length - 1] == '"') ||
                 (family[0] == '\'' && family[family.Length - 1] == '\'')))
            {
                family = family.Substring(1, family.Length - 2).Trim();
            }

            if (family.Length > 0)
            {
                families.Add(family);
            }
        }

        return families;
    }

    private static string Sanitize(List<string> families)
    {
        List<string> parts = new(families.Count);

        foreach (string family in families)
        {
            string sanitized = SanitizeFamily(family);
            if (sanitized.Length > 0)
            {
                parts.Add(sanitized);
            }
        }

        return string.Join("-", parts);
    }

    private static string SanitizeFamily(string family)
    {
        StringBuilder sb = StringBuilderBag.TakeOrNew();
        bool lastWasDash = false;

        foreach (char c in family)
        {
            char lower = char.ToLowerInvariant(c);

            if ((lower >= 'a' && lower <= 'z') || (lower >= '0' && lower <= '9'))
            {
                sb.Append(lower);
                lastWasDash = false;
            }
            else if (!lastWasDash && sb.Length > 0)
            {
                sb.Append('-');
                lastWasDash = true;
            }
        }

        while (sb.Length > 0 && sb[sb.Length - 1] == '-')
        {
            sb.Length--;
        }

        string result = sb.ToString();
        StringBuilderBag.ClearAndReturn(sb);
        return result;
    }

    private static string ShortHash(List<string> families)
    {
        StringBuilder canonical = new();

        for (int i = 0; i < families.Count; i++)
        {
            if (i > 0)
            {
                canonical.Append(',');
            }

            canonical.Append(CollapseWhitespace(families[i]));
        }

        using SHA256 sha = SHA256.Create();
        byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(canonical.ToString()));
        string encoded = ZBase32Encoder.Encode(hash);

        return encoded.Length > 5 ? encoded.Substring(0, 5) : encoded;
    }

    private static string CollapseWhitespace(string value)
    {
        StringBuilder sb = new(value.Length);
        bool pendingSpace = false;

        foreach (char c in value.Trim())
        {
            if (char.IsWhiteSpace(c))
            {
                pendingSpace = sb.Length > 0;
                continue;
            }

            if (pendingSpace)
            {
                sb.Append(' ');
                pendingSpace = false;
            }

            sb.Append(c);
        }

        return sb.ToString();
    }

    private static string Quote(string family)
    {
        bool needsQuote = family.Length == 0 || char.IsDigit(family[0]);

        if (!needsQuote)
        {
            foreach (char c in family)
            {
                bool safe = c == '-' || c == '_' ||
                            (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') ||
                            (c >= '0' && c <= '9');

                if (!safe)
                {
                    needsQuote = true;
                    break;
                }
            }
        }

        return needsQuote ? "\"" + family.Replace("\"", "\\\"") + "\"" : family;
    }

    public override string ToString() => Token;
}
