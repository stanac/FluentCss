namespace FluentCss.Docs.Indexer;

/// <summary>
/// The serialized shape of <c>search-index.json</c>. Kept deliberately small: the
/// browser builds the MiniSearch inverted index from these documents at load time.
/// </summary>
internal sealed class SearchIndex
{
    public int Version { get; init; } = 1;

    public IReadOnlyList<IndexPage> Pages { get; init; } = [];
}

internal sealed class IndexPage
{
    public string Route { get; init; } = "";

    public string Title { get; init; } = "";

    /// <summary>The navigation group ("Getting started", "Core concepts", ...).</summary>
    public string Section { get; init; } = "";

    public IReadOnlyList<IndexSection> Sections { get; init; } = [];
}

internal sealed class IndexSection
{
    /// <summary>Anchor id, matching the slug the "On this page" script assigns.</summary>
    public string Id { get; set; } = "";

    public string Heading { get; set; } = "";

    public int Level { get; set; }

    public string Text { get; set; } = "";

    public string Code { get; set; } = "";
}
