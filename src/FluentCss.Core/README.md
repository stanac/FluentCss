# FluentCss.Core

Core builder, registry, typed tokens and utility methods for **FluentCss** — a
strongly-typed, Tailwind-inspired utility CSS builder for .NET.

Most applications should reference the main
[`FluentCss`](https://www.nuget.org/packages/FluentCss) package instead; this
package is pulled in transitively. Use `FluentCss.Core` directly in console or
other non-ASP.NET .NET apps:

```csharp
using FluentCss.Core;

var classes = FluentCssBuilder.Css.Px(4).Py(2).RoundedMd().Background(Color.Blue600);
string classAttribute = classes.ToString()!;
string css = ClassRegistry.DefaultRegistry.Css;
```

- Main package: https://www.nuget.org/packages/FluentCss
- Documentation: https://fluentcss.stanac.dev
