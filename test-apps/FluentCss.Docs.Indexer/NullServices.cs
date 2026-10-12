using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentCss.Docs.Indexer;

/// <summary>
/// Components rendered for the index are non-interactive, so JS interop is never
/// actually invoked. It still has to resolve, though, because components such as
/// <c>CodeBlock</c> inject <see cref="IJSRuntime"/>.
/// </summary>
internal sealed class NullJsRuntime : IJSRuntime
{
    public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args) =>
        new(default(TValue)!);

    public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args) =>
        new(default(TValue)!);
}

/// <summary>Minimal <see cref="NavigationManager"/> so components can be rendered outside a host.</summary>
internal sealed class NullNavigationManager : NavigationManager
{
    public NullNavigationManager() => Initialize("https://localhost/", "https://localhost/");

    protected override void NavigateToCore(string uri, bool forceLoad)
    {
    }
}
