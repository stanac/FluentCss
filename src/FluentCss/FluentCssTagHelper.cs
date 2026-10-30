using FluentCss.Core;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace FluentCss;

/// <summary>
/// Renders everything FluentCss needs in the document head: the stylesheet link
/// and a small script that keeps it in sync with the registry.
/// </summary>
/// <remarks>
/// <para>Usage: <c>&lt;fluent-css /&gt;</c>.</para>
/// <para>
/// The link is written while the layout is rendering, before the body has
/// registered its classes, so its hash can be stale. The script re-reads the
/// current hash once the document has loaded and also watches for nodes added
/// later (for example an AJAX partial view), updating the link when the
/// stylesheet changes. It is self contained and adds nothing to the page.
/// </para>
/// <para>
/// A custom element is used rather than an attribute on <c>&lt;link&gt;</c>
/// because the helper emits two elements. If the tag helper is not registered
/// the raw element is inert, instead of a link that silently loads nothing.
/// </para>
/// </remarks>
[HtmlTargetElement("fluent-css")]
public sealed class FluentCssTagHelper : TagHelper
{
    private const string PathPrefix = "/fluentcss";

#if DEBUG
    private const string ScriptFileName = "fluentcss.js";
#else
    private const string ScriptFileName = "fluentcss.min.js";
#endif

    private static readonly Lazy<string> Script = new(LoadScript);

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var hash = ClassRegistry.DefaultRegistry.Sha256Hex;

        output.TagName = null;
        output.Content.SetHtmlContent(
            $"<link rel=\"stylesheet\" href=\"{PathPrefix}/{hash}.css\" id=\"fluentcss\" data-fluentcss-hash=\"{hash}\" />" +
            $"<script>{Script.Value}</script>");
    }

    private static string LoadScript()
    {
        var assembly = typeof(FluentCssTagHelper).Assembly;
        var name = assembly
            .GetManifestResourceNames()
            .FirstOrDefault(n => n.EndsWith("." + ScriptFileName, StringComparison.OrdinalIgnoreCase));

        if (name is null)
        {
            return string.Empty;
        }

        using var stream = assembly.GetManifestResourceStream(name)!;
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
