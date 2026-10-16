namespace FluentCss.Core.Tests;

/// <summary>
/// The fluent selector builder, including the <c>Targets(...)</c> selector-list
/// option and relative selectors for <c>:has()</c>.
/// </summary>
public class SelectorBuilderTests
{
    [Fact]
    public void Class_And_Pseudo_Should_Compose()
    {
        Selector.New().Class("btn").Hover().Css.Should().Be(".btn:hover");
    }

    [Fact]
    public void Element_Attribute_And_Class_Should_Compose_In_Order()
    {
        string css = Selector.New()
            .Element("input")
            .Attribute("type", "text")
            .Attribute("required")
            .Class("form-control")
            .Css;

        css.Should().Be("input[type=\"text\"][required].form-control");
    }

    [Fact]
    public void Id_Should_Compose_With_A_Preceding_Class()
    {
        Selector.New().Class("btn").Id("submit").Css.Should().Be(".btn#submit");
    }

    [Theory]
    [InlineData(AttributeOperator.Exact, "=")]
    [InlineData(AttributeOperator.Includes, "~=")]
    [InlineData(AttributeOperator.HyphenPrefix, "|=")]
    [InlineData(AttributeOperator.Prefix, "^=")]
    [InlineData(AttributeOperator.Suffix, "$=")]
    [InlineData(AttributeOperator.Contains, "*=")]
    public void Attribute_Should_Use_The_Operator(AttributeOperator op, string expected)
    {
        Selector.New().Attribute("data-x", "value", op).Css.Should().Be($"[data-x{expected}\"value\"]");
    }

    [Theory]
    [InlineData(AttributeCase.Insensitive, "[data-x=\"value\" i]")]
    [InlineData(AttributeCase.Sensitive, "[data-x=\"value\" s]")]
    [InlineData(AttributeCase.Default, "[data-x=\"value\"]")]
    public void Attribute_Should_Apply_The_Case_Flag(AttributeCase casing, string expected)
    {
        Selector.New().Attribute("data-x", "value", AttributeOperator.Exact, casing).Css.Should().Be(expected);
    }

    [Fact]
    public void Not_Should_Accept_A_Selector_List()
    {
        string css = Selector.New()
            .Class("input")
            .Not(
                t => t.Class("disabled"),
                t => t.Attribute("readonly"))
            .Css;

        css.Should().Be(".input:not(.disabled, [readonly])");
    }

    [Fact]
    public void Is_And_Where_Should_Accept_Selector_Lists()
    {
        Selector.New().Element("h2").Is(t => t.Class("title"), t => t.Id("main")).Css
            .Should().Be("h2:is(.title, #main)");

        Selector.New().Element("h2").Where(t => t.Class("title"), t => t.Id("main")).Css
            .Should().Be("h2:where(.title, #main)");
    }

    [Fact]
    public void NthChild_Should_Accept_A_Formula()
    {
        Selector.New().Element("li").NthChild("2n+1").Css.Should().Be("li:nth-child(2n+1)");
        Selector.New().Element("li").NthChild(3).Css.Should().Be("li:nth-child(3)");
    }

    [Fact]
    public void Combinators_Should_Separate_Targets()
    {
        Selector.New().Class("prose").Child().Element("h2").Hover().Css
            .Should().Be(".prose > h2:hover");

        Selector.New().Class("a").AdjacentSibling().Class("b").Css
            .Should().Be(".a + .b");

        Selector.New().Class("a").GeneralSibling().Class("b").Css
            .Should().Be(".a ~ .b");

        Selector.New().Class("a").Descendant().Class("b").Css
            .Should().Be(".a .b");
    }

    [Fact]
    public void Universal_Should_Be_Available_After_A_Combinator()
    {
        Selector.New().Class("a").Child().Universal().Css.Should().Be(".a > *");
    }

    [Fact]
    public void Is_Should_Compose_With_A_Following_Attribute()
    {
        string css = Selector.New()
            .Attribute("data-fc-toc-source")
            .Descendant()
            .Is(t => t.Element("h1"), t => t.Element("h2"), t => t.Element("h3"))
            .Attribute("id")
            .Css;

        css.Should().Be("[data-fc-toc-source] :is(h1, h2, h3)[id]");
    }

    [Fact]
    public void Has_Should_Accept_A_Single_Relative_Selector()
    {
        Selector.New().Class("card").Has(r => r.Child().Element("img")).Css
            .Should().Be(".card:has(> img)");
    }

    [Fact]
    public void Has_Should_Accept_Multiple_Relative_Selectors()
    {
        string css = Selector.New()
            .Class("card")
            .Has(
                r => r.Child().Element("img"),
                r => r.AdjacentSibling().Class("title").Hover())
            .Css;

        css.Should().Be(".card:has(> img, + .title:hover)");
    }

    [Fact]
    public void Has_Should_Accept_A_Full_Complex_Relative_Selector()
    {
        string css = Selector.New()
            .Class("card")
            .Has(r => r.Child().Class("a").AdjacentSibling().Class("b"))
            .Css;

        css.Should().Be(".card:has(> .a + .b)");
    }

    [Fact]
    public void Has_Should_Not_Require_A_Leading_Combinator()
    {
        Selector.New().Class("card").Has(r => r.Class("featured")).Css
            .Should().Be(".card:has(.featured)");
    }

    [Fact]
    public void PseudoElement_Should_Terminate_The_Selector()
    {
        Selector.New().Class("btn").Before().Css.Should().Be(".btn::before");
    }

    [Fact]
    public void Functional_PseudoElement_Should_Allow_Following_Pseudo_Classes()
    {
        Selector.New().Element("my-button").FunctionalPseudoElement("part", "button").Hover().Css
            .Should().Be("my-button::part(button):hover");
    }

    [Fact]
    public void Theme_Should_Prefix_The_Selector_With_The_Theme_Convention()
    {
        Selector.New().Theme("dark").Class("card").Css.Should().Be(".theme-dark .card");
    }

    [Fact]
    public void Media_And_Theme_Should_Wrap_The_Selector()
    {
        string css = Selector.New()
            .Media("screen and (min-width: 768px)")
            .Dark()
            .Class("btn")
            .Hover()
            .Css;

        css.Should().Be("@media screen and (min-width: 768px) { .theme-dark .btn:hover }");
    }

    [Fact]
    public void Container_Should_Wrap_The_Selector()
    {
        string css = Selector.New()
            .Container("(min-width: 400px)")
            .Class("card")
            .Css;

        css.Should().Be("@container (min-width: 400px) { .card }");
    }

    [Fact]
    public void Supports_Should_Wrap_The_Selector()
    {
        string css = Selector.New()
            .Supports("(display: grid)")
            .Class("grid")
            .Css;

        css.Should().Be("@supports (display: grid) { .grid }");
    }

    [Fact]
    public void Targets_Should_Build_A_Selector_List()
    {
        string css = Selector.New()
            .Targets(
                t => t.Class("primary"),
                t => t.Class("secondary"))
            .Css;

        css.Should().Be(".primary, .secondary");
    }

    [Fact]
    public void Targets_Should_Share_Media_And_Theme()
    {
        string css = Selector.New()
            .Media("screen and (min-width: 768px)")
            .Dark()
            .Targets(
                t => t.Class("btn").Hover(),
                t => t.Class("card"),
                t => t.Class("panel").Child().Class("title"))
            .Css;

        css.Should().Be(
            "@media screen and (min-width: 768px) { .theme-dark .btn:hover, .theme-dark .card, .theme-dark .panel > .title }");
    }

    [Fact]
    public void Targets_Should_Accept_Pseudo_Elements()
    {
        string css = Selector.New()
            .Targets(
                t => t.Class("a").Before(),
                t => t.Class("b").After())
            .Css;

        css.Should().Be(".a::before, .b::after");
    }

    [Fact]
    public void Class_Should_Escape_A_Leading_Digit()
    {
        Selector.New().Class("2xl").Css.Should().Be(".\\32 xl");
    }

    [Fact]
    public void Class_Should_Escape_Special_Characters()
    {
        Selector.New().Class("md:hover:bg-blue-500").Css.Should().Be(".md\\:hover\\:bg-blue-500");
    }

    [Fact]
    public void Targets_Should_Require_At_Least_One_Target()
    {
        Action act = () => Selector.New().Targets();

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Has_Should_Require_At_Least_One_Selector()
    {
        Action act = () => Selector.New().Class("card").Has();

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Not_Should_Require_At_Least_One_Selector()
    {
        Action act = () => Selector.New().Class("card").Not();

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Empty_Target_Should_Throw()
    {
        Action act = () => Selector.New().Targets(t => t);

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Empty_Class_Name_Should_Throw()
    {
        Action act = () => Selector.New().Class("  ");

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Empty_Media_Query_Should_Throw()
    {
        Action act = () => Selector.New().Media("");

        act.Should().Throw<ArgumentException>();
    }
}
