using System;

namespace FluentCss.Core;

/// <summary>
/// Responsive helpers for <see cref="IRootFluentCssBuilder"/>.
/// </summary>
/// <remarks>
/// These are extension methods on purpose: an implementation only has to provide the
/// generic <see cref="IRootFluentCssBuilder.Media"/> primitive and does not need one
/// method per breakpoint. Use <see cref="Min"/> / <see cref="Max"/> with any
/// <see cref="Length"/>, or the named helpers for the values in <see cref="Breakpoints"/>.
/// </remarks>
public static class RootFluentCssBuilderExtensions
{
    /// <summary>
    /// Applies the styles when the viewport is at least <paramref name="size"/> wide.
    /// The comparison is inclusive (<c>&gt;=</c>) so the exact breakpoint belongs to the
    /// min side, matching Tailwind.
    /// </summary>
    public static IRootFluentCssBuilder Min(this IRootFluentCssBuilder builder, Length size, Action<IFluentCssBuilder> cssBuilderAction) =>
        builder.Media($"width >= {size.Value}", $"min-{size.Token}", cssBuilderAction);

    /// <summary>
    /// Applies the styles when the viewport is smaller than <paramref name="size"/>.
    /// The comparison is exclusive (<c>&lt;</c>) so it never overlaps with
    /// <see cref="Min"/> of the same length.
    /// </summary>
    public static IRootFluentCssBuilder Max(this IRootFluentCssBuilder builder, Length size, Action<IFluentCssBuilder> cssBuilderAction) =>
        builder.Media($"width < {size.Value}", $"max-{size.Token}", cssBuilderAction);

    /// <summary>Min helper for <see cref="Breakpoints.XSmall"/> (20rem).</summary>
    public static IRootFluentCssBuilder MinXs(this IRootFluentCssBuilder builder, Action<IFluentCssBuilder> cssBuilderAction) =>
        builder.Min(Breakpoints.XSmall, cssBuilderAction);

    /// <summary>Min helper for <see cref="Breakpoints.Small"/> (40rem).</summary>
    public static IRootFluentCssBuilder MinSm(this IRootFluentCssBuilder builder, Action<IFluentCssBuilder> cssBuilderAction) =>
        builder.Min(Breakpoints.Small, cssBuilderAction);

    /// <summary>Min helper for <see cref="Breakpoints.Medium"/> (48rem).</summary>
    public static IRootFluentCssBuilder MinMd(this IRootFluentCssBuilder builder, Action<IFluentCssBuilder> cssBuilderAction) =>
        builder.Min(Breakpoints.Medium, cssBuilderAction);

    /// <summary>Min helper for <see cref="Breakpoints.Large"/> (64rem).</summary>
    public static IRootFluentCssBuilder MinLg(this IRootFluentCssBuilder builder, Action<IFluentCssBuilder> cssBuilderAction) =>
        builder.Min(Breakpoints.Large, cssBuilderAction);

    /// <summary>Min helper for <see cref="Breakpoints.XLarge"/> (80rem).</summary>
    public static IRootFluentCssBuilder MinXl(this IRootFluentCssBuilder builder, Action<IFluentCssBuilder> cssBuilderAction) =>
        builder.Min(Breakpoints.XLarge, cssBuilderAction);

    /// <summary>Min helper for <see cref="Breakpoints.XXLarge"/> (96rem).</summary>
    public static IRootFluentCssBuilder Min2Xl(this IRootFluentCssBuilder builder, Action<IFluentCssBuilder> cssBuilderAction) =>
        builder.Min(Breakpoints.XXLarge, cssBuilderAction);

    /// <summary>Max helper for <see cref="Breakpoints.XSmall"/> (20rem).</summary>
    public static IRootFluentCssBuilder MaxXs(this IRootFluentCssBuilder builder, Action<IFluentCssBuilder> cssBuilderAction) =>
        builder.Max(Breakpoints.XSmall, cssBuilderAction);

    /// <summary>Max helper for <see cref="Breakpoints.Small"/> (40rem).</summary>
    public static IRootFluentCssBuilder MaxSm(this IRootFluentCssBuilder builder, Action<IFluentCssBuilder> cssBuilderAction) =>
        builder.Max(Breakpoints.Small, cssBuilderAction);

    /// <summary>Max helper for <see cref="Breakpoints.Medium"/> (48rem).</summary>
    public static IRootFluentCssBuilder MaxMd(this IRootFluentCssBuilder builder, Action<IFluentCssBuilder> cssBuilderAction) =>
        builder.Max(Breakpoints.Medium, cssBuilderAction);

    /// <summary>Max helper for <see cref="Breakpoints.Large"/> (64rem).</summary>
    public static IRootFluentCssBuilder MaxLg(this IRootFluentCssBuilder builder, Action<IFluentCssBuilder> cssBuilderAction) =>
        builder.Max(Breakpoints.Large, cssBuilderAction);

    /// <summary>Max helper for <see cref="Breakpoints.XLarge"/> (80rem).</summary>
    public static IRootFluentCssBuilder MaxXl(this IRootFluentCssBuilder builder, Action<IFluentCssBuilder> cssBuilderAction) =>
        builder.Max(Breakpoints.XLarge, cssBuilderAction);

    /// <summary>Max helper for <see cref="Breakpoints.XXLarge"/> (96rem).</summary>
    public static IRootFluentCssBuilder Max2Xl(this IRootFluentCssBuilder builder, Action<IFluentCssBuilder> cssBuilderAction) =>
        builder.Max(Breakpoints.XXLarge, cssBuilderAction);
}
