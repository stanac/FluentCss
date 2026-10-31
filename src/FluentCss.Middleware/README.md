# FluentCss.Middleware

ASP.NET Core middleware and dependency-injection extensions for **FluentCss**. It
serves the generated stylesheet at `/fluentcss/{hash}.css` (with gzip, `ETag`
and immutable caching) and keeps it in sync after client-side DOM changes.

Use it in MVC / Razor Pages apps together with the main
[`FluentCss`](https://www.nuget.org/packages/FluentCss) package:

```csharp
builder.Services.AddFluentCss();

var app = builder.Build();

app.UseRouting();
app.UseFluentCss();
app.UseAuthorization();
```

- Main package: https://www.nuget.org/packages/FluentCss
- Documentation: https://fluentcss.stanac.dev
