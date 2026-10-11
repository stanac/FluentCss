using FluentCss.Core;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace FluentCss.Mvc;

/// <summary>
/// Renders the FluentCss stylesheet link and a small script that re-synchronises
/// it after the page finishes loading.
/// </summary>
/// <remarks>
/// The link is written while the layout is rendering, before the body has
/// registered its classes, so its hash can be stale. The script asks the
/// middleware for the latest hash once the document has loaded and swaps the
/// link if the stylesheet changed. This closes the gap without buffering the
/// response.
/// </remarks>
[HtmlTargetElement("link", Attributes = AttributeName)]
public sealed class FluentCssTagHelper : TagHelper
{
    public const string AttributeName = "fluent-css";

    private const string PathPrefix = "/fluentcss";

    private const string ScriptTemplate =
        """<script>(function(){function r(){var l=document.getElementById('fluentcss');if(!l)return;fetch('__PREFIX__/hash',{cache:'no-store'}).then(function(x){return x.ok?x.text():null}).then(function(h){if(!h)return;h=h.trim();if(h&&h!==l.getAttribute('data-fluentcss-hash')){l.setAttribute('data-fluentcss-hash',h);l.setAttribute('href','__PREFIX__/'+h+'.css');}}).catch(function(){});}if(document.readyState==='loading'){document.addEventListener('DOMContentLoaded',r);}else{r();}})();</script>""";

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

        output.PostElement.AppendHtml(BuildScript());
    }

    private static string BuildScript() =>
        ScriptTemplate.Replace("__PREFIX__", PathPrefix);
}
