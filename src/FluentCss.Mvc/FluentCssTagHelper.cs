using FluentCss.Core;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace FluentCss.Mvc;

/// <summary>
/// Renders the FluentCss stylesheet link and a small script that keeps it in
/// sync with the registry.
/// </summary>
/// <remarks>
/// The link is written while the layout is rendering, before the body has
/// registered its classes, so its hash can be stale. The script re-reads the
/// current hash once the document has loaded and also watches for nodes added
/// later (for example an AJAX partial view), updating the link when the
/// stylesheet changes. It is self contained and adds nothing to the page.
/// </remarks>
[HtmlTargetElement("link", Attributes = AttributeName)]
public sealed class FluentCssTagHelper : TagHelper
{
    public const string AttributeName = "fluent-css";

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

        output.TagName = "link";
        output.TagMode = TagMode.SelfClosing;

        output.Attributes.RemoveAll(AttributeName);
        output.Attributes.SetAttribute("rel", "stylesheet");
        output.Attributes.SetAttribute("href", $"{PathPrefix}/{hash}.css");
        output.Attributes.SetAttribute("id", "fluentcss");
        output.Attributes.SetAttribute("data-fluentcss-hash", hash);

        output.PostElement.AppendHtml($"<script>{Script.Value}</script>");
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
