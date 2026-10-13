namespace FluentCss.Core.Tests;

/// <summary>
/// The <see cref="FontFamily"/> value type and the <c>Font</c>/<c>FontSans</c>/
/// <c>Sans</c>/<c>Serif</c>/<c>Mono</c> builder helpers.
/// </summary>
public class FontFamilyTests
{
    private const string ZBase32Alphabet = "ybndrfg8ejkmcpqxot1uwisza345h769";

    private readonly ClassRegistry _registry = new();

    private InternalRootFluentCssBuilder Sut => new(_registry, null, null, null);

    [Fact]
    public void Builtins_Should_Use_Clean_Tokens()
    {
        FontFamily.Sans.Token.Should().Be("sans");
        FontFamily.Serif.Token.Should().Be("serif");
        FontFamily.Mono.Token.Should().Be("mono");

        FontFamily.Sans.Value.Should().Contain("ui-sans-serif");
        FontFamily.Serif.Value.Should().Contain("ui-serif");
        FontFamily.Mono.Value.Should().Contain("ui-monospace");
    }

    [Fact]
    public void FontSans_Should_Emit_Font_Sans()
    {
        Sut.FontSans();

        var added = _registry.Single();
        added.ClassName.Should().Be("font-sans");
        added.Property.Should().Be("font-family");
        added.Value.Should().Be(FontFamily.SansStack);
        added.CssString.Should().Contain(".font-sans { font-family:");
    }

    [Fact]
    public void Font_With_Explicit_Name_Should_Use_The_Name_As_Token()
    {
        Sut.Font(new FontFamily("Inter, sans-serif", "brand"));

        var added = _registry.Single();
        added.ClassName.Should().Be("font-brand");
        added.Value.Should().Be("Inter, sans-serif");
    }

    [Fact]
    public void Font_Without_Name_Should_Derive_Token_From_The_Value()
    {
        var family = new FontFamily("Inter, system-ui, sans-serif");

        family.Name.Should().BeNull();
        family.Token.Should().StartWith("inter-system-ui-sans-serif-");
        Assert_Hash_Suffix(family.Token);

        Sut.Font(family);
        _registry.Single().ClassName.Should().Be("font-" + family.Token);
    }

    [Fact]
    public void Sans_Should_Prepend_Custom_Families_And_Hash_The_Token()
    {
        Sut.Sans("Inter");

        var added = _registry.Single();
        added.ClassName.Should().StartWith("font-sans-inter-");
        Assert_Hash_Suffix(added.ClassName);

        added.Value.Should().Be("Inter, " + FontFamily.SansStack);
    }

    [Fact]
    public void Sans_Should_Sanitize_All_Custom_Families()
    {
        Sut.Sans("Inter", "Helvetica Neue");

        var added = _registry.Single();
        added.ClassName.Should().StartWith("font-sans-inter-helvetica-neue-");
        added.Value.Should().Be("Inter, \"Helvetica Neue\", " + FontFamily.SansStack);
    }

    [Fact]
    public void Sans_Should_Leave_Generic_Keywords_Unquoted()
    {
        Sut.Sans("sans-serif");

        _registry.Single().Value.Should().Be("sans-serif, " + FontFamily.SansStack);
    }

    [Fact]
    public void Sans_With_No_Families_Should_Fall_Back_To_The_Default()
    {
        Sut.Sans();

        _registry.Single().ClassName.Should().Be("font-sans");
    }

    [Fact]
    public void Different_Bases_Should_Not_Collide_For_The_Same_Family()
    {
        var sans = FontFamily.Prepend("sans", FontFamily.SansStack, ["Inter"]);
        var serif = FontFamily.Prepend("serif", FontFamily.SerifStack, ["Inter"]);

        sans.Token.Should().NotBe(serif.Token);
        sans.Token.Should().StartWith("sans-inter-");
        serif.Token.Should().StartWith("serif-inter-");
    }

    [Fact]
    public void Sanitization_Collision_Should_Be_Resolved_By_The_Hash()
    {
        var spaced = new FontFamily("Inter Tight, sans-serif");
        var hyphenated = new FontFamily("Inter-Tight, sans-serif");

        // Both sanitize to the same readable form...
        spaced.Token.Should().StartWith("inter-tight-sans-serif-");
        hyphenated.Token.Should().StartWith("inter-tight-sans-serif-");

        // ...but the hash of the canonical families keeps them distinct.
        spaced.Token.Should().NotBe(hyphenated.Token);
    }

    [Fact]
    public void Token_Should_Be_Deterministic_For_Equal_Stacks()
    {
        var a = new FontFamily("Inter, system-ui, sans-serif");
        var b = new FontFamily("Inter, system-ui, sans-serif");

        a.Token.Should().Be(b.Token);
    }

    [Fact]
    public void Font_Should_Compose_With_Media()
    {
        Sut.Min(Breakpoints.Medium, md => md.Sans("Inter"));

        var added = _registry.Single();
        added.ClassName.Should().StartWith("min-md:font-sans-inter-");
        added.MediaQuery.Should().Be("(width >= 48rem)");
    }

    private static void Assert_Hash_Suffix(string token)
    {
        int dash = token.LastIndexOf('-');
        dash.Should().BeGreaterThan(0);

        string hash = token.Substring(dash + 1);
        hash.Should().HaveLength(5);
        hash.Should().MatchRegex($"^[{ZBase32Alphabet}]+$");
    }
}
