namespace FluentCss.Core.Tests;

/// <summary>
/// Coverage for pseudo variants. A single <c>Pseudo</c> method decides whether to
/// emit one colon (pseudo-class) or two (pseudo-element).
/// </summary>
public class PseudoVariantTests
{
    private static UtilityClass Single(Action<InternalRootFluentCssBuilder> apply)
    {
        ClassRegistry registry = new();
        var sut = new InternalRootFluentCssBuilder(registry, null, null, null);

        apply(sut);

        return registry.Single();
    }

    [Fact]
    public void Pseudo_With_A_PseudoClass_Should_Emit_A_Single_Colon()
    {
        var added = Single(b => b.Pseudo("hover", p => p.Background(Color.Blue500)));

        added.ClassName.Should().Be("hover:bg-blue-500");
        added.Pseudo.Should().Be("hover");
        added.PseudoIsElement.Should().BeFalse();
        added.CssString.Should().StartWith(".hover\\:bg-blue-500:hover {");
    }

    [Fact]
    public void Pseudo_With_A_Known_PseudoElement_Should_Emit_A_Double_Colon()
    {
        var added = Single(b => b.Placeholder(p => p.Color(Color.Neutral400)));

        added.ClassName.Should().Be("placeholder:text-neutral-400");
        added.Pseudo.Should().Be("placeholder");
        added.PseudoIsElement.Should().BeTrue();
        added.CssString.Should().StartWith(".placeholder\\:text-neutral-400::placeholder {");
        added.CssString.Should().Contain("color: " + Color.Neutral400.Value);
    }

    [Fact]
    public void Functional_PseudoElement_Should_Be_Detected_By_Its_Head_Name()
    {
        var added = Single(b => b.Pseudo("part(label)", p => p.Background(Color.Black)));

        added.ClassName.Should().Be("part(label):bg-black");
        added.PseudoIsElement.Should().BeTrue();
        added.CssString.Should().StartWith(".part\\(label\\)\\:bg-black::part(label) {");
    }

    [Fact]
    public void Leading_Colons_Should_Override_The_Detection()
    {
        var forcedElement = Single(b => b.Pseudo("::hover", p => p.Background(Color.Blue500)));
        forcedElement.Pseudo.Should().Be("hover");
        forcedElement.PseudoIsElement.Should().BeTrue();
        forcedElement.CssString.Should().Contain("::hover");

        var forcedClass = Single(b => b.Pseudo(":placeholder", p => p.Color(Color.Red500)));
        forcedClass.Pseudo.Should().Be("placeholder");
        forcedClass.PseudoIsElement.Should().BeFalse();
        forcedClass.CssString.Should().Contain(":placeholder");
    }

    [Fact]
    public void Unknown_Pseudo_Should_Default_To_A_PseudoClass()
    {
        var added = Single(b => b.Pseudo("my-state", p => p.Opacity(50)));

        added.PseudoIsElement.Should().BeFalse();
        added.CssString.Should().Contain(":my-state");
    }

    [Fact]
    public void Typed_PseudoElement_Helpers_Should_Emit_Double_Colon()
    {
        Single(b => b.Before(p => p.Background(Color.Red500))).CssString.Should().Contain("::before");
        Single(b => b.After(p => p.Background(Color.Red500))).CssString.Should().Contain("::after");
        Single(b => b.Marker(p => p.Color(Color.Black))).ClassName.Should().Be("marker:text-black");
        Single(b => b.Selection(p => p.Background(Color.Blue200))).ClassName.Should().Be("selection:bg-blue-200");
        Single(b => b.File(p => p.Rounded())).ClassName.Should().Be("file-selector-button:rounded");
        Single(b => b.Backdrop(p => p.Opacity(50))).ClassName.Should().Be("backdrop:opacity-50");
        Single(b => b.FirstLetter(p => p.FontBold())).ClassName.Should().Be("first-letter:font-bold");
        Single(b => b.FirstLine(p => p.Italic())).ClassName.Should().Be("first-line:italic");
    }

    [Fact]
    public void PseudoElement_Should_Compose_With_Theme_And_Media()
    {
        ClassRegistry registry = new();
        var sut = new InternalRootFluentCssBuilder(registry, null, null, null);

        sut.Media("min-width: 30rem", "sm", md => md.Placeholder(p => p.Color(Color.Red500)));

        var added = registry.Single();
        added.ClassName.Should().Be("sm:placeholder:text-red-500");
        added.CssString.Should().StartWith("@media (min-width: 30rem) { ");
        added.CssString.Should().Contain("::placeholder");
    }

    [Fact]
    public void AddPseudo_Should_Be_Immutable()
    {
        var original = new UtilityClass("text-red-500", "color", "#f00");

        var result = original.AddPseudo("placeholder");

        result.Should().NotBeSameAs(original);
        result.Pseudo.Should().Be("placeholder");
        result.PseudoIsElement.Should().BeTrue();
        result.ClassName.Should().Be("placeholder:text-red-500");
        original.Pseudo.Should().BeNull();
        original.PseudoIsElement.Should().BeFalse();
        original.ClassName.Should().Be("text-red-500");
    }

    [Fact]
    public void SelectorSuffix_Should_Append_To_The_Class_Selector()
    {
        var original = new UtilityClass("space-x-4", "margin-inline-start", "1rem");

        var result = original.WithSelectorSuffix("> * + *");

        result.Should().NotBeSameAs(original);
        result.SelectorSuffix.Should().Be("> * + *");
        result.CssString.Should().Be(".space-x-4 > * + * { margin-inline-start: 1rem } ");
        original.SelectorSuffix.Should().BeNull();
        original.CssString.Should().Be(".space-x-4 { margin-inline-start: 1rem } ");
    }
}
