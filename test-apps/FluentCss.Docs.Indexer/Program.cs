using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using FluentCss.Docs.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace FluentCss.Docs.Indexer;

/// <summary>
/// Renders every documentation page to HTML with <see cref="HtmlRenderer"/> and
/// writes a per-section JSON index used by the client-side MiniSearch engine.
/// Rendering (rather than parsing the .razor sources) is required because parts of
/// the docs are produced at render time by reflection and by the builder itself.
/// </summary>
internal static class Program
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
    };

    public static async Task<int> Main(string[] args)
    {
        var outputPath = args.Length > 0
            ? Path.GetFullPath(args[0])
            : DefaultOutputPath();

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSingleton<IJSRuntime, NullJsRuntime>();
        services.AddSingleton<NavigationManager, NullNavigationManager>();

        await using var provider = services.BuildServiceProvider();
        var loggerFactory = provider.GetRequiredService<ILoggerFactory>();

        var routes = BuildRouteMap(typeof(DocsNavigation).Assembly);
        var pages = new List<IndexPage>();

        await using var renderer = new HtmlRenderer(provider, loggerFactory);

        foreach (var group in DocsNavigation.Sections)
        {
            foreach (var item in group.Items)
            {
                if (!routes.TryGetValue(item.Href, out var componentType))
                {
                    Console.Error.WriteLine($"warning: no component found for route '{item.Href}'");
                    continue;
                }

                var html = await renderer.Dispatcher.InvokeAsync(async () =>
                {
                    var root = await renderer.RenderComponentAsync(componentType, ParameterView.Empty);
                    return root.ToHtmlString();
                });

                var sections = ExtractSections(html);
                pages.Add(new IndexPage
                {
                    Route = item.Href,
                    Title = item.Title,
                    Section = group.Title,
                    Sections = sections,
                });

                Console.WriteLine($"  {item.Href} -> {sections.Count} section(s)");
            }
        }

        var index = new SearchIndex
        {
            Pages = pages,
        };

        var json = JsonSerializer.Serialize(index, JsonOptions);
        Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
        await File.WriteAllTextAsync(outputPath, json + Environment.NewLine);

        Console.WriteLine(
            $"Wrote {pages.Count} page(s), {pages.Sum(p => p.Sections.Count)} section(s) to {outputPath}");

        return 0;
    }

    private static string DefaultOutputPath() => Path.GetFullPath(Path.Combine(
        AppContext.BaseDirectory,
        "..", "..", "..", "..",
        "FluentCss.Docs", "wwwroot", "search-index.json"));

    private static Dictionary<string, Type> BuildRouteMap(Assembly assembly)
    {
        var map = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

        foreach (var type in assembly.GetTypes())
        {
            if (type.IsAbstract || !typeof(IComponent).IsAssignableFrom(type))
            {
                continue;
            }

            foreach (var route in type.GetCustomAttributes<RouteAttribute>())
            {
                if (!string.IsNullOrWhiteSpace(route.Template))
                {
                    map[route.Template] = type;
                }
            }
        }

        return map;
    }

    /// <summary>
    /// Walks the rendered document in order, starting a new section at every
    /// heading. Body text goes to <c>Text</c>; code (inline and blocks) goes to
    /// <c>Code</c> so API names stay searchable. Heading anchors reproduce the
    /// slug algorithm used by docs.js so deep links line up.
    /// </summary>
    private static List<IndexSection> ExtractSections(string html)
    {
        var document = new HtmlParser().ParseDocument(html);
        var root = (INode?)document.QuerySelector("[data-fc-toc-source]")
                   ?? document.QuerySelector("body")
                   ?? document.DocumentElement;

        var sections = new List<IndexSection>();
        var used = new Dictionary<string, int>(StringComparer.Ordinal);
        var text = new StringBuilder();
        var code = new StringBuilder();
        IndexSection? current = null;

        void Flush()
        {
            if (current is not null)
            {
                current.Text = Collapse(text.ToString());
                current.Code = Collapse(code.ToString());
                sections.Add(current);
            }

            text.Clear();
            code.Clear();
        }

        void Walk(INode node)
        {
            if (node is IElement element)
            {
                var tag = element.LocalName;

                if (IsHeading(tag))
                {
                    Flush();
                    current = CreateSection(element, sections.Count, used);
                    return;
                }

                if (tag is "script" or "style" or "title" or "meta" or "link" or "head")
                {
                    return;
                }

                if (tag is "pre" or "code")
                {
                    code.Append(' ').Append(element.TextContent);
                    return;
                }

                foreach (var child in element.ChildNodes)
                {
                    Walk(child);
                }

                return;
            }

            if (node is IText textNode)
            {
                text.Append(' ').Append(textNode.Data);
            }
        }

        foreach (var child in root.ChildNodes)
        {
            Walk(child);
        }

        Flush();
        return sections;
    }

    private static IndexSection CreateSection(IElement heading, int index, Dictionary<string, int> used)
    {
        var text = Collapse(heading.TextContent);
        var id = heading.GetAttribute("id");

        if (string.IsNullOrEmpty(id))
        {
            var slug = Slugify(text);
            if (slug.Length == 0)
            {
                slug = "section-" + (index + 1);
            }

            used.TryGetValue(slug, out var count);
            count++;
            used[slug] = count;
            id = count == 1 ? slug : slug + "-" + count;
        }

        return new IndexSection
        {
            Id = id,
            Heading = text,
            Level = heading.LocalName[1] - '0',
        };
    }

    private static bool IsHeading(string tag) =>
        tag.Length == 2 && tag[0] == 'h' && tag[1] is >= '1' and <= '6';

    private static string Collapse(string value) =>
        Regex.Replace(value, "\\s+", " ").Trim();

    /// <summary>Mirrors the slugify() function in docs.js.</summary>
    private static string Slugify(string value)
    {
        var slug = (value ?? "").ToLowerInvariant().Trim();
        slug = Regex.Replace(slug, "[^a-z0-9_\\s-]", "");
        slug = Regex.Replace(slug, "[\\s_]+", "-");
        return slug.Trim('-');
    }
}
