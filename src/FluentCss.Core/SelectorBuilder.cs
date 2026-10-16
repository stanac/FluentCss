using System;
using System.Collections.Generic;
using System.Text;

namespace FluentCss.Core;

/// <summary>
/// The internal selector node that backs every selector-builder interface. All the
/// stages are implemented by the same object: the interfaces returned by each method
/// are what enforces the reading order, not the class hierarchy.
/// </summary>
internal sealed class SelectorBuilder :
    IRootSelectorBuilder,
    IThemeSelectorBuilder,
    ISelectorContextBuilder,
    IRelativeSelectorBuilder,
    ICompoundSelectorBuilder,
    IPseudoElementBuilder,
    IBuildableSelector
{
    private string? _atRule;
    private string? _theme;
    private string? _built;
    private readonly StringBuilder _selector = new();

    internal SelectorBuilder(string? atRule, string? theme)
    {
        _atRule = atRule;
        _theme = theme;
    }

    public string Css => ToString();

    public override string ToString() => _built ?? WrapForOutput(WithThemePrefix(_selector.ToString()));

    // ---------------------------------------------------------------------
    // Context (at-rules and theme)
    //
    // These mutate the current builder and return it. That keeps the builder
    // usable from an Action<IRootSelectorBuilder>: the action mutates the
    // instance it is handed, so its Css can be read afterwards.
    // ---------------------------------------------------------------------

    public IThemeSelectorBuilder Media(string query)
    {
        _atRule = AtRule("@media", query);
        return this;
    }

    public IThemeSelectorBuilder Container(string query)
    {
        _atRule = AtRule("@container", query);
        return this;
    }

    public IThemeSelectorBuilder Supports(string query)
    {
        _atRule = AtRule("@supports", query);
        return this;
    }

    public ISelectorContextBuilder Theme(string theme)
    {
        if (string.IsNullOrWhiteSpace(theme))
        {
            throw new ArgumentException("A theme cannot be empty.", nameof(theme));
        }

        _theme = "theme-" + theme.Trim();
        return this;
    }

    public IBuildableSelector Targets(params Func<ITargetSelectorBuilder, IBuildableSelector>[] targets)
    {
        if (targets is null || targets.Length == 0)
        {
            throw new ArgumentException("At least one target is required.", nameof(targets));
        }

        string[] parts = new string[targets.Length];

        for (int i = 0; i < targets.Length; i++)
        {
            Func<ITargetSelectorBuilder, IBuildableSelector> target = targets[i]
                ?? throw new ArgumentException("A target cannot be null.", nameof(targets));

            SelectorBuilder child = new(_atRule, _theme);
            _ = target(child);

            parts[i] = child.WithThemePrefix(RequireSelector(child, "target"));
        }

        return Built(WrapForOutput(string.Join(", ", parts)));
    }

    // ---------------------------------------------------------------------
    // Compound modifiers
    // ---------------------------------------------------------------------

    public ICompoundSelectorBuilder Class(string className)
    {
        Require(className, nameof(className), "class");
        _selector.Append('.').Append(UtilityClass.Escape(className.Trim()));
        return this;
    }

    public ICompoundSelectorBuilder Id(string id)
    {
        Require(id, nameof(id), "id");
        _selector.Append('#').Append(UtilityClass.Escape(id.Trim()));
        return this;
    }

    public ICompoundSelectorBuilder Element(string tag)
    {
        Require(tag, nameof(tag), "element");
        _selector.Append(tag.Trim());
        return this;
    }

    public ICompoundSelectorBuilder Universal()
    {
        _selector.Append('*');
        return this;
    }

    public ICompoundSelectorBuilder Attribute(string name)
    {
        Require(name, nameof(name), "attribute name");
        _selector.Append('[').Append(name.Trim()).Append(']');
        return this;
    }

    public ICompoundSelectorBuilder Attribute(string name, string value) =>
        Attribute(name, value, AttributeOperator.Exact, AttributeCase.Default);

    public ICompoundSelectorBuilder Attribute(string name, string value, AttributeOperator op, AttributeCase casing = AttributeCase.Default)
    {
        Require(name, nameof(name), "attribute name");

        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        _selector.Append('[')
            .Append(name.Trim())
            .Append(OperatorText(op))
            .Append('"')
            .Append(EscapeAttributeValue(value))
            .Append('"');

        switch (casing)
        {
            case AttributeCase.Insensitive:
                _selector.Append(" i");
                break;
            case AttributeCase.Sensitive:
                _selector.Append(" s");
                break;
        }

        _selector.Append(']');
        return this;
    }

    public ICompoundSelectorBuilder Pseudo(string pseudo)
    {
        Require(pseudo, nameof(pseudo), "pseudo-class");
        _selector.Append(':').Append(TrimPseudo(pseudo));
        return this;
    }

    public ICompoundSelectorBuilder Pseudo(string pseudo, string argument)
    {
        Require(pseudo, nameof(pseudo), "pseudo-class");

        if (argument is null)
        {
            throw new ArgumentNullException(nameof(argument));
        }

        _selector.Append(':').Append(TrimPseudo(pseudo)).Append('(').Append(argument).Append(')');
        return this;
    }

    public ICompoundSelectorBuilder Not(params Func<ITargetSelectorBuilder, IBuildableSelector>[] selectors) =>
        AppendFunctional(":not", BuildSelectorList(selectors, "not"));

    public ICompoundSelectorBuilder Is(params Func<ITargetSelectorBuilder, IBuildableSelector>[] selectors) =>
        AppendFunctional(":is", BuildSelectorList(selectors, "is"));

    public ICompoundSelectorBuilder Where(params Func<ITargetSelectorBuilder, IBuildableSelector>[] selectors) =>
        AppendFunctional(":where", BuildSelectorList(selectors, "where"));

    public ICompoundSelectorBuilder Has(params Func<IRelativeSelectorBuilder, ICompoundSelectorBuilder>[] selectors) =>
        AppendFunctional(":has", BuildRelativeSelectorList(selectors));

    public ICompoundSelectorBuilder NthChild(int n) => AppendFunctional(":nth-child", n.ToString());

    public ICompoundSelectorBuilder NthChild(string formula)
    {
        Require(formula, nameof(formula), "formula");
        return AppendFunctional(":nth-child", formula.Trim());
    }

    // ---------------------------------------------------------------------
    // Combinators
    // ---------------------------------------------------------------------

    public ITargetSelectorBuilder Descendant()
    {
        _selector.Append(' ');
        return this;
    }

    public ITargetSelectorBuilder Child()
    {
        _selector.Append(" > ");
        return this;
    }

    public ITargetSelectorBuilder AdjacentSibling()
    {
        _selector.Append(" + ");
        return this;
    }

    public ITargetSelectorBuilder GeneralSibling()
    {
        _selector.Append(" ~ ");
        return this;
    }

    // ---------------------------------------------------------------------
    // Pseudo-elements
    // ---------------------------------------------------------------------

    public IBuildableSelector PseudoElement(string pseudoElement)
    {
        Require(pseudoElement, nameof(pseudoElement), "pseudo-element");
        _selector.Append("::").Append(pseudoElement.Trim().TrimStart(':'));
        return this;
    }

    public ICompoundSelectorBuilder FunctionalPseudoElement(string name, string argument)
    {
        Require(name, nameof(name), "pseudo-element");

        if (argument is null)
        {
            throw new ArgumentNullException(nameof(argument));
        }

        _selector.Append("::").Append(name.Trim().TrimStart(':')).Append('(').Append(argument).Append(')');
        return this;
    }

    // ---------------------------------------------------------------------
    // Internals
    // ---------------------------------------------------------------------

    private IBuildableSelector Built(string css)
    {
        _built = css;
        return this;
    }

    private ICompoundSelectorBuilder AppendFunctional(string name, string argument)
    {
        _selector.Append(name).Append('(').Append(argument).Append(')');
        return this;
    }

    private static string BuildSelectorList(
        Func<ITargetSelectorBuilder, IBuildableSelector>[] selectors,
        string kind)
    {
        if (selectors is null || selectors.Length == 0)
        {
            throw new ArgumentException($"At least one selector is required for :{kind}().", nameof(selectors));
        }

        string[] parts = new string[selectors.Length];

        for (int i = 0; i < selectors.Length; i++)
        {
            Func<ITargetSelectorBuilder, IBuildableSelector> selector = selectors[i]
                ?? throw new ArgumentException("A selector cannot be null.", nameof(selectors));

            SelectorBuilder child = new(null, null);
            _ = selector(child);
            parts[i] = RequireSelector(child, "selector");
        }

        return string.Join(", ", parts);
    }

    private static string BuildRelativeSelectorList(
        Func<IRelativeSelectorBuilder, ICompoundSelectorBuilder>[] selectors)
    {
        if (selectors is null || selectors.Length == 0)
        {
            throw new ArgumentException("At least one relative selector is required for :has().", nameof(selectors));
        }

        string[] parts = new string[selectors.Length];

        for (int i = 0; i < selectors.Length; i++)
        {
            Func<IRelativeSelectorBuilder, ICompoundSelectorBuilder> selector = selectors[i]
                ?? throw new ArgumentException("A selector cannot be null.", nameof(selectors));

            SelectorBuilder child = new(null, null);
            _ = selector(child);
            parts[i] = RequireSelector(child, "relative selector");
        }

        return string.Join(", ", parts);
    }

    private static string RequireSelector(SelectorBuilder builder, string kind)
    {
        string raw = builder._selector.ToString().Trim();

        if (raw.Length == 0)
        {
            throw new InvalidOperationException($"A {kind} must contain at least one simple selector.");
        }

        return raw;
    }

    private string WithThemePrefix(string raw)
    {
        if (string.IsNullOrWhiteSpace(_theme))
        {
            return raw;
        }

        return "." + UtilityClass.Escape(_theme!) + " " + raw;
    }

    private string WrapForOutput(string content)
    {
        if (string.IsNullOrWhiteSpace(_atRule))
        {
            return content;
        }

        return _atRule + " { " + content + " }";
    }

    private static string AtRule(string name, string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            throw new ArgumentException("A query cannot be empty.", nameof(query));
        }

        return name + " " + query.Trim();
    }

    private static string OperatorText(AttributeOperator op) => op switch
    {
        AttributeOperator.Exact => "=",
        AttributeOperator.Includes => "~=",
        AttributeOperator.HyphenPrefix => "|=",
        AttributeOperator.Prefix => "^=",
        AttributeOperator.Suffix => "$=",
        AttributeOperator.Contains => "*=",
        _ => throw new ArgumentOutOfRangeException(nameof(op), op, "Unknown attribute operator.")
    };

    private static string EscapeAttributeValue(string value)
    {
        StringBuilder sb = StringBuilderBag.TakeOrNew();

        foreach (char c in value)
        {
            if (c == '"' || c == '\\')
            {
                sb.Append('\\');
            }

            sb.Append(c);
        }

        string result = sb.ToString();
        StringBuilderBag.ClearAndReturn(sb);
        return result;
    }

    private static string TrimPseudo(string pseudo) => pseudo.Trim().TrimStart(':');

    private static void Require(string? value, string parameterName, string kind)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"A {kind} cannot be empty.", parameterName);
        }
    }
}
