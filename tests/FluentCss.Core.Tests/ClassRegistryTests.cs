using System.Collections;
using System.Text;

namespace FluentCss.Core.Tests;

public class ClassRegistryTests
{
    [Fact]
    public void Add_Should_ReturnTrue_When_Class_Is_New()
    {
        // Arrange
        ClassRegistry sut = [];
        UtilityClass util = new UtilityClass("bg-red", "background", "red");

        // Act
        bool result = sut.Add(util);

        // Assert
        result.Should().BeTrue();
        sut.Should().ContainSingle();
    }

    [Fact]
    public void Add_Should_ReturnFalse_When_Class_Is_Duplicate()
    {
        // Arrange
        ClassRegistry sut = [];
        UtilityClass util1 = new UtilityClass("bg-red", "background", "red");
        UtilityClass util2 = new UtilityClass("bg-red", "background", "red"); // Identical signature

        // Act
        sut.Add(util1);
        bool result = sut.Add(util2);

        // Assert
        result.Should().BeFalse();
        sut.Should().ContainSingle();
    }

    [Fact]
    public void Css_Property_Should_Accumulate_Strings()
    {
        // Arrange
        ClassRegistry sut =
        [
            new UtilityClass("a", "prop", "val"),
            new UtilityClass("b", "prop", "val")
            // Act
        ];

        // Act
        string css = sut.Css;

        // Assert
        css.Should().Contain(".a { prop: val }");
        css.Should().Contain(".b { prop: val }");
    }

    [Fact]
    public void OnClassAdded_Event_Should_Fire_Only_For_New_Classes()
    {
        // Arrange
        ClassRegistry sut = [];
        UtilityClass util = new UtilityClass("test", "p", "v");
        UtilityClass? captured = null;
        int callCount = 0;

        sut.OnClassAdded += (_, args) =>
        {
            captured = args;
            callCount++;
        };

        // Act
        sut.Add(util);
        sut.Add(util);

        // Assert
        callCount.Should().Be(1);
        captured.Should().Be(util);
    }

    [Fact]
    public void GetEnumerator_Should_Preserve_Registration_Order()
    {
        // Arrange
        ClassRegistry sut = [];
        UtilityClass first = new("a", "prop", "1");
        UtilityClass second = new("b", "prop", "2");
        UtilityClass third = new("c", "prop", "3");

        // Act
        sut.Add(second);
        sut.Add(first);
        sut.Add(third);

        // Assert
        sut.Select(x => x.ClassName).Should().Equal("b", "a", "c");
    }

    [Fact]
    public void AddSelector_Should_Merge_Declarations_In_Order_Under_The_Selector()
    {
        // Arrange
        ClassRegistry sut = [];
        UtilityClass padding = new("p-4", "padding-inline", "1rem");
        UtilityClass background = new("bg-blue-500", "background-color", "#3b82f6");

        // Act
        sut.AddSelector(".btn", [padding, background]);

        // Assert
        sut.Css.Should().Contain(".btn { padding-inline: 1rem; background-color: #3b82f6 }");
    }

    [Fact]
    public void AddSelector_Should_Emit_Important_Declarations()
    {
        // Arrange
        ClassRegistry sut = [];
        UtilityClass important = new UtilityClass("p-4", "padding", "1rem").AddImportant();

        // Act
        sut.AddSelector(".btn", [important]);

        // Assert
        sut.Css.Should().Contain(".btn { padding: 1rem !important }");
    }

    [Fact]
    public void AddSelector_Should_Apply_Pseudo_Theme_And_Media()
    {
        // Arrange
        ClassRegistry sut = [];
        UtilityClass hover = new UtilityClass("bg-blue-700", "background-color", "#1d4ed8").AddPseudo("hover");
        UtilityClass dark = new UtilityClass("bg-slate-800", "background-color", "#1e293b").AddTheme("theme-dark");
        UtilityClass md = new UtilityClass("px-6", "padding-inline", "1.5rem").AddMedia("(width >= 48rem)", "min-md");

        // Act
        sut.AddSelector(".btn", [hover]);
        sut.AddSelector(".card", [dark]);
        sut.AddSelector(".btn", [md]);

        // Assert
        sut.Css.Should().Contain(".btn:hover { background-color: #1d4ed8 }");
        sut.Css.Should().Contain(".theme-dark .card { background-color: #1e293b }");
        sut.Css.Should().Contain("@media (width >= 48rem) { .btn { padding-inline: 1.5rem } }");
    }

    [Fact]
    public void AddSelector_Should_Emit_Selector_Rules_Before_Utility_Rules()
    {
        // Arrange
        ClassRegistry sut = [];
        sut.Add(new UtilityClass("bg-red-500", "background-color", "#ef4444"));

        // Act
        sut.AddSelector(".btn", [new UtilityClass("px-4", "padding-inline", "1rem")]);

        // Assert
        string css = sut.Css;
        css.IndexOf(".btn {", StringComparison.Ordinal)
            .Should().BeLessThan(css.IndexOf(".bg-red-500", StringComparison.Ordinal));
    }

    [Fact]
    public void ApplySelector_Should_Keep_Declaration_Order()
    {
        // Arrange
        ClassRegistry registry = new();
        InternalRootFluentCssBuilder sut = new(registry, null, null, null);

        // Act
        sut.ApplySelector(".p", css => css.P(4).P(2));

        // Assert
        registry.Css.Should().Contain(".p { padding: 1rem; padding: 0.5rem }");
    }

    [Fact]
    public void Css_Should_Be_Cached_Until_A_Class_Is_Added()
    {
        // Arrange
        ClassRegistry sut = [];
        UtilityClass util = new("a", "prop", "val");

        // Act
        string first = sut.Css;
        string second = sut.Css;

        // Assert
        second.Should().BeSameAs(first);

        sut.Add(util);
        sut.Css.Should().NotBeSameAs(first);
    }

    [Fact]
    public void CssUtf8_Should_Be_The_Utf8_Encoding_Of_Css()
    {
        // Arrange
        ClassRegistry sut = [];
        sut.Add(new UtilityClass("a", "prop", "val"));

        // Act
        byte[] bytes = sut.CssUtf8;

        // Assert
        bytes.Should().Equal(Encoding.UTF8.GetBytes(sut.Css));
    }

    [Fact]
    public void Sha256Hex_Should_Be_The_Lowercase_Hex_Of_Sha256()
    {
        // Arrange
        ClassRegistry sut = [];
        sut.Add(new UtilityClass("a", "prop", "val"));

        // Act
        string hex = sut.Sha256Hex;

        // Assert
        hex.Should().Be(Convert.ToHexString(sut.Sha256).ToLowerInvariant());
    }

    [Fact]
    public void Sha256Hex_Should_Change_When_A_Class_Is_Added()
    {
        // Arrange
        ClassRegistry sut = [];
        sut.Add(new UtilityClass("a", "prop", "val"));

        // Act
        string before = sut.Sha256Hex;
        sut.Add(new UtilityClass("b", "prop", "val"));
        string after = sut.Sha256Hex;

        // Assert
        after.Should().NotBe(before);
    }

    [Fact]
    public void NonGeneric_Enumeration_Should_Yield_All_Classes()
    {
        // Arrange
        ClassRegistry sut = [];
        sut.Add(new UtilityClass("a", "prop", "1"));
        sut.Add(new UtilityClass("b", "prop", "2"));

        IEnumerable enumerable = sut;
        var names = new List<string>();

        // Act
        foreach (UtilityClass item in enumerable)
        {
            names.Add(item.ClassName);
        }

        // Assert
        names.Should().Equal("a", "b");
    }

    [Fact]
    public void AddSelector_Should_Group_Variants_Into_Separate_Rules()
    {
        // Arrange
        ClassRegistry sut = [];
        UtilityClass baseRule = new("p-4", "padding", "1rem");
        UtilityClass hover = new UtilityClass("p-6", "padding", "1.5rem").AddPseudo("hover");
        UtilityClass md = new UtilityClass("p-8", "padding", "2rem").AddMedia("(width >= 48rem)", "min-md");

        // Act
        sut.AddSelector(".btn", [baseRule, hover, md]);

        // Assert
        sut.Css.Should().Contain(".btn { padding: 1rem }");
        sut.Css.Should().Contain(".btn:hover { padding: 1.5rem }");
        sut.Css.Should().Contain("@media (width >= 48rem) { .btn { padding: 2rem } }");
    }

    [Fact]
    public void AddSelector_Should_Order_Media_Rules_By_Breakpoint()
    {
        // Arrange
        ClassRegistry sut = [];
        UtilityClass large = new UtilityClass("p-lg", "padding", "2rem").AddMedia("(width >= 64rem)", "min-lg");
        UtilityClass medium = new UtilityClass("p-md", "padding", "1rem").AddMedia("(width >= 48rem)", "min-md");

        // Act
        sut.AddSelector(".card", [large]);
        sut.AddSelector(".card", [medium]);

        // Assert
        string css = sut.Css;
        css.IndexOf("@media (width >= 48rem)", StringComparison.Ordinal)
            .Should().BeLessThan(css.IndexOf("@media (width >= 64rem)", StringComparison.Ordinal));
    }

    [Fact]
    public void AddSelector_Should_Ignore_An_Empty_Batch()
    {
        // Arrange
        ClassRegistry sut = [];

        // Act
        sut.AddSelector(".x", []);

        // Assert
        sut.Css.Should().NotContain(".x");
    }

    [Fact]
    public void AddSelector_Should_Raise_OnClassAdded()
    {
        // Arrange
        ClassRegistry sut = [];
        int calls = 0;
        sut.OnClassAdded += (_, _) => calls++;

        // Act
        sut.AddSelector(".x", [new UtilityClass("a", "prop", "val")]);

        // Assert
        calls.Should().Be(1);
    }

    [Fact]
    public void AddSelector_Should_Append_A_Rule_Per_Call_For_The_Same_Selector()
    {
        // Arrange
        ClassRegistry sut = [];

        // Act
        sut.AddSelector(".x", [new UtilityClass("a", "padding", "1rem")]);
        sut.AddSelector(".x", [new UtilityClass("b", "margin", "0.5rem")]);

        // Assert
        sut.Css.Should().Contain(".x { padding: 1rem }");
        sut.Css.Should().Contain(".x { margin: 0.5rem }");
    }
}