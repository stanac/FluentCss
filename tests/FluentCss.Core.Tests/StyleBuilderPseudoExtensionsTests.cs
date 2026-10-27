namespace FluentCss.Core.Tests;

/// <summary>
/// The typed pseudo-class variants on the style builder, including composition with
/// theme, media and <c>!important</c>.
/// </summary>
public class StyleBuilderPseudoExtensionsTests
{
    private static InternalFluentCssBuilder CreateSut(out ClassRegistry registry)
    {
        registry = new ClassRegistry();
        return new InternalFluentCssBuilder(registry, null, null, null);
    }

    [Fact]
    public void Simple_Pseudo_Variants_Should_Generate_The_Expected_Class()
    {
        (Func<IFluentCssBuilder, Action<IOnlyStyleBuilder>, IFluentCssBuilder> Apply, string Pseudo)[] cases =
        [
            ((b, a) => b.Hover(a), "hover"),
            ((b, a) => b.Focus(a), "focus"),
            ((b, a) => b.Active(a), "active"),
            ((b, a) => b.Visited(a), "visited"),
            ((b, a) => b.Target(a), "target"),
            ((b, a) => b.FocusVisible(a), "focus-visible"),
            ((b, a) => b.FocusWithin(a), "focus-within"),
            ((b, a) => b.Disabled(a), "disabled"),
            ((b, a) => b.Enabled(a), "enabled"),
            ((b, a) => b.Checked(a), "checked"),
            ((b, a) => b.Indeterminate(a), "indeterminate"),
            ((b, a) => b.Default(a), "default"),
            ((b, a) => b.Required(a), "required"),
            ((b, a) => b.Optional(a), "optional"),
            ((b, a) => b.Valid(a), "valid"),
            ((b, a) => b.Invalid(a), "invalid"),
            ((b, a) => b.InRange(a), "in-range"),
            ((b, a) => b.OutOfRange(a), "out-of-range"),
            ((b, a) => b.ReadOnly(a), "read-only"),
            ((b, a) => b.ReadWrite(a), "read-write"),
            ((b, a) => b.PlaceholderShown(a), "placeholder-shown"),
            ((b, a) => b.Autofill(a), "autofill"),
            ((b, a) => b.UserValid(a), "user-valid"),
            ((b, a) => b.UserInvalid(a), "user-invalid"),
            ((b, a) => b.Open(a), "open"),
            ((b, a) => b.PopoverOpen(a), "popover-open"),
            ((b, a) => b.Modal(a), "modal"),
            ((b, a) => b.Fullscreen(a), "fullscreen"),
            ((b, a) => b.PictureInPicture(a), "picture-in-picture"),
            ((b, a) => b.Defined(a), "defined"),
            ((b, a) => b.Root(a), "root"),
            ((b, a) => b.Empty(a), "empty"),
            ((b, a) => b.First(a), "first-child"),
            ((b, a) => b.Last(a), "last-child"),
            ((b, a) => b.Only(a), "only-child"),
            ((b, a) => b.Odd(a), "nth-child(odd)"),
            ((b, a) => b.Even(a), "nth-child(even)"),
            ((b, a) => b.FirstOfType(a), "first-of-type"),
            ((b, a) => b.LastOfType(a), "last-of-type"),
            ((b, a) => b.OnlyOfType(a), "only-of-type"),
        ];

        foreach (var (apply, pseudo) in cases)
        {
            var sut = CreateSut(out ClassRegistry registry);

            apply(sut, x => x.Background(Color.Red500));

            UtilityClass added = registry.Single();
            added.Pseudo.Should().Be(pseudo);
            added.ClassName.Should().Be($"{pseudo}:bg-red-500");
            registry.Css.Should().Contain($".{UtilityClass.Escape(pseudo)}\\:bg-red-500:{pseudo} ", "the {0} variant should emit its selector", pseudo);
        }
    }

    [Fact]
    public void Functional_Pseudo_Variants_Should_Generate_The_Expected_Class()
    {
        (Func<IFluentCssBuilder, Action<IOnlyStyleBuilder>, IFluentCssBuilder> Apply, string Pseudo)[] cases =
        [
            ((b, a) => b.NthChild(2, a), "nth-child(2)"),
            ((b, a) => b.NthChild("2n+1", a), "nth-child(2n+1)"),
            ((b, a) => b.NthLastChild(1, a), "nth-last-child(1)"),
            ((b, a) => b.NthLastChild("2n", a), "nth-last-child(2n)"),
            ((b, a) => b.NthOfType(3, a), "nth-of-type(3)"),
            ((b, a) => b.NthOfType("odd", a), "nth-of-type(odd)"),
            ((b, a) => b.NthLastOfType(4, a), "nth-last-of-type(4)"),
            ((b, a) => b.NthLastOfType("even", a), "nth-last-of-type(even)"),
            ((b, a) => b.Lang("en", a), "lang(en)"),
            ((b, a) => b.Dir("rtl", a), "dir(rtl)"),
        ];

        foreach (var (apply, pseudo) in cases)
        {
            var sut = CreateSut(out ClassRegistry registry);

            apply(sut, x => x.Background(Color.Red500));

            UtilityClass added = registry.Single();
            added.Pseudo.Should().Be(pseudo);
            added.ClassName.Should().Be($"{pseudo}:bg-red-500");
            registry.Css.Should().Contain($".{UtilityClass.Escape(pseudo)}\\:bg-red-500:{pseudo} ", "the {0} variant should emit its selector", pseudo);
        }
    }

    [Fact]
    public void Pseudo_Variant_Should_Compose_With_A_Theme()
    {
        ClassRegistry registry = new();
        var sut = new InternalFluentCssBuilder(registry, "theme-dark", null, null);

        sut.FocusVisible(x => x.Outline(2));

        UtilityClass added = registry.Single();
        added.Theme.Should().Be("theme-dark");
        added.Pseudo.Should().Be("focus-visible");
        added.ClassName.Should().Be("theme-dark:focus-visible:outline-2");
    }

    [Fact]
    public void Pseudo_Variant_Should_Compose_With_Media()
    {
        ClassRegistry registry = new();
        var sut = new InternalRootFluentCssBuilder(registry, null, null, null);

        sut.MinMd(md => md.FocusVisible(x => x.Outline(2)));

        UtilityClass added = registry.Single();
        added.MediaPrefix.Should().Be("min-md");
        added.Pseudo.Should().Be("focus-visible");
        added.ClassName.Should().Be("min-md:focus-visible:outline-2");
        registry.Css.Should().Contain(".min-md\\:focus-visible\\:outline-2:focus-visible");
    }

    [Fact]
    public void Pseudo_Variant_Should_Compose_With_Important()
    {
        var sut = CreateSut(out ClassRegistry registry);

        sut.Checked(x => x.Important(c => c.Outline(2)));

        UtilityClass added = registry.Single();
        added.Pseudo.Should().Be("checked");
        added.Important.Should().BeTrue();
        added.ClassName.Should().Be("checked:!outline-2");
    }

    [Fact]
    public void Pseudo_Should_Remain_The_Escape_Hatch()
    {
        var sut = CreateSut(out ClassRegistry registry);

        sut.Pseudo("modal", x => x.Background(Color.Black));

        UtilityClass added = registry.Single();
        added.Pseudo.Should().Be("modal");
        added.ClassName.Should().Be("modal:bg-black");
    }

    [Fact]
    public void Builder_Should_Expose_The_Variant_Class_Names()
    {
        var sut = CreateSut(out _);

        sut.FocusVisible(x => x.Outline(2));
        sut.Important(c => c.P(4));

        sut.ToString().Should().Be("focus-visible:outline-2 !p-4");
    }
}
