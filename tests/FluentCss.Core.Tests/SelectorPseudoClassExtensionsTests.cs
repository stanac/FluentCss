namespace FluentCss.Core.Tests;

/// <summary>
/// Pseudo-class, data and aria helpers on the fluent selector builder.
/// <see cref="ICompoundSelectorBuilder.Pseudo(string)"/> remains the escape hatch.
/// </summary>
public class SelectorPseudoClassExtensionsTests
{
    [Fact]
    public void Simple_Pseudo_Helpers_Should_Append_Their_Pseudo_Class()
    {
        (Func<ICompoundSelectorBuilder, ICompoundSelectorBuilder> Apply, string Pseudo)[] cases =
        [
            (b => b.Hover(), "hover"),
            (b => b.Focus(), "focus"),
            (b => b.Active(), "active"),
            (b => b.Visited(), "visited"),
            (b => b.Target(), "target"),
            (b => b.FocusVisible(), "focus-visible"),
            (b => b.FocusWithin(), "focus-within"),
            (b => b.Disabled(), "disabled"),
            (b => b.Enabled(), "enabled"),
            (b => b.Checked(), "checked"),
            (b => b.Indeterminate(), "indeterminate"),
            (b => b.Default(), "default"),
            (b => b.Required(), "required"),
            (b => b.Optional(), "optional"),
            (b => b.Valid(), "valid"),
            (b => b.Invalid(), "invalid"),
            (b => b.InRange(), "in-range"),
            (b => b.OutOfRange(), "out-of-range"),
            (b => b.ReadOnly(), "read-only"),
            (b => b.ReadWrite(), "read-write"),
            (b => b.PlaceholderShown(), "placeholder-shown"),
            (b => b.Autofill(), "autofill"),
            (b => b.UserValid(), "user-valid"),
            (b => b.UserInvalid(), "user-invalid"),
            (b => b.Open(), "open"),
            (b => b.PopoverOpen(), "popover-open"),
            (b => b.Modal(), "modal"),
            (b => b.Fullscreen(), "fullscreen"),
            (b => b.PictureInPicture(), "picture-in-picture"),
            (b => b.Defined(), "defined"),
            (b => b.Root(), "root"),
            (b => b.Empty(), "empty"),
            (b => b.First(), "first-child"),
            (b => b.Last(), "last-child"),
            (b => b.Only(), "only-child"),
            (b => b.Odd(), "nth-child(odd)"),
            (b => b.Even(), "nth-child(even)"),
            (b => b.FirstOfType(), "first-of-type"),
            (b => b.LastOfType(), "last-of-type"),
            (b => b.OnlyOfType(), "only-of-type"),
        ];

        foreach (var (apply, pseudo) in cases)
        {
            apply(Selector.New().Class("x")).Css.Should().Be($".x:{pseudo}", "the {0} helper should append its pseudo-class", pseudo);
        }
    }

    [Fact]
    public void Functional_Pseudo_Helpers_Should_Append_Their_Argument()
    {
        (Func<ICompoundSelectorBuilder, ICompoundSelectorBuilder> Apply, string Pseudo)[] cases =
        [
            (b => b.NthChild(2), "nth-child(2)"),
            (b => b.NthChild("2n+1"), "nth-child(2n+1)"),
            (b => b.NthLastChild(1), "nth-last-child(1)"),
            (b => b.NthLastChild("2n"), "nth-last-child(2n)"),
            (b => b.NthOfType(3), "nth-of-type(3)"),
            (b => b.NthOfType("odd"), "nth-of-type(odd)"),
            (b => b.NthLastOfType(4), "nth-last-of-type(4)"),
            (b => b.NthLastOfType("even"), "nth-last-of-type(even)"),
            (b => b.Lang("en"), "lang(en)"),
            (b => b.Dir("rtl"), "dir(rtl)"),
        ];

        foreach (var (apply, pseudo) in cases)
        {
            apply(Selector.New().Class("x")).Css.Should().Be($".x:{pseudo}", "the {0} helper should append its argument", pseudo);
        }
    }

    [Fact]
    public void Pseudo_Should_Still_Be_The_Escape_Hatch()
    {
        Selector.New().Class("x").Pseudo("focus-visible").Css.Should().Be(".x:focus-visible");
        Selector.New().Class("x").Pseudo("nth-child", "2").Css.Should().Be(".x:nth-child(2)");
    }

    [Fact]
    public void Data_Should_Build_Data_Attribute_Selectors()
    {
        Selector.New().Class("x").Data("open").Css.Should().Be(".x[data-open]");
        Selector.New().Class("x").Data("state", "open").Css.Should().Be(".x[data-state=\"open\"]");
        Selector.New().Class("x").Data("state", "open", AttributeOperator.Includes).Css.Should().Be(".x[data-state~=\"open\"]");
        Selector.New().Class("x").Data("state", "open", AttributeOperator.Exact, AttributeCase.Insensitive).Css
            .Should().Be(".x[data-state=\"open\" i]");
    }

    [Fact]
    public void Aria_Should_Build_Aria_Attribute_Selectors()
    {
        Selector.New().Class("x").Aria("expanded").Css.Should().Be(".x[aria-expanded]");
        Selector.New().Class("x").Aria("expanded", "true").Css.Should().Be(".x[aria-expanded=\"true\"]");
        Selector.New().Class("x").Aria("checked", "true", AttributeOperator.Exact, AttributeCase.Sensitive).Css
            .Should().Be(".x[aria-checked=\"true\" s]");
    }

    [Fact]
    public void Data_And_Aria_Should_Work_Through_ApplySelector()
    {
        ClassRegistry registry = new();
        var sut = new InternalRootFluentCssBuilder(registry, null, null, null);

        sut.ApplySelector(s => s.Data("state", "open").Class("card"), css => css.P(4));
        sut.ApplySelector(s => s.Aria("expanded", "true").Class("menu"), css => css.P(2));

        registry.Css.Should().Contain("[data-state=\"open\"].card { padding: 1rem }");
        registry.Css.Should().Contain("[aria-expanded=\"true\"].menu { padding: 0.5rem }");
    }

    [Fact]
    public void Empty_Formulas_And_Names_Should_Throw()
    {
        Action formula = () => Selector.New().Class("x").NthChild("  ");
        Action lang = () => Selector.New().Class("x").Lang("");
        Action dir = () => Selector.New().Class("x").Dir("");
        Action data = () => Selector.New().Class("x").Data(" ");
        Action aria = () => Selector.New().Class("x").Aria(" ");

        formula.Should().Throw<ArgumentException>();
        lang.Should().Throw<ArgumentException>();
        dir.Should().Throw<ArgumentException>();
        data.Should().Throw<ArgumentException>();
        aria.Should().Throw<ArgumentException>();
    }
}
