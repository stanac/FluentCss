using System;

namespace FluentCss.Core;

/// <summary>
/// Responsive helpers for <see cref="IMediaFluentCssBuilder"/>.
/// </summary>
/// <remarks>
/// These are extension methods on purpose: an implementation only has to provide the
/// generic <see cref="IMediaFluentCssBuilder.Media"/> primitive and does not need one
/// method per breakpoint. Use <see cref="Min"/> / <see cref="Max"/> with any
/// <see cref="Length"/>, or the named helpers for the values in <see cref="Breakpoints"/>.
/// </remarks>
public static class MediaFluentCssBuilderExtensions
{
    /// <summary>
    /// Applies the styles when the viewport is at least <paramref name="size"/> wide.
    /// The comparison is inclusive (<c>&gt;=</c>) so the exact breakpoint belongs to the
    /// min side, matching Tailwind.
    /// </summary>
    public static IMediaFluentCssBuilder Min(this IMediaFluentCssBuilder builder, Length size, Action<IFluentCssBuilder> cssBuilderAction) =>
        builder.Media($"width >= {size.Value}", $"min-{size.Token}", cssBuilderAction);

    /// <summary>
    /// Applies the styles when the viewport is smaller than <paramref name="size"/>.
    /// The comparison is exclusive (<c>&lt;</c>) so it never overlaps with
    /// <see cref="Min"/> of the same length.
    /// </summary>
    public static IMediaFluentCssBuilder Max(this IMediaFluentCssBuilder builder, Length size, Action<IFluentCssBuilder> cssBuilderAction) =>
        builder.Media($"width < {size.Value}", $"max-{size.Token}", cssBuilderAction);

    /// <summary>Min helper for <see cref="Breakpoints.XSmall"/> (20rem).</summary>
    public static IMediaFluentCssBuilder MinXs(this IMediaFluentCssBuilder builder, Action<IFluentCssBuilder> cssBuilderAction) =>
        builder.Min(Breakpoints.XSmall, cssBuilderAction);

    /// <summary>Min helper for <see cref="Breakpoints.Small"/> (40rem).</summary>
    public static IMediaFluentCssBuilder MinSm(this IMediaFluentCssBuilder builder, Action<IFluentCssBuilder> cssBuilderAction) =>
        builder.Min(Breakpoints.Small, cssBuilderAction);

    /// <summary>Min helper for <see cref="Breakpoints.Medium"/> (48rem).</summary>
    public static IMediaFluentCssBuilder MinMd(this IMediaFluentCssBuilder builder, Action<IFluentCssBuilder> cssBuilderAction) =>
        builder.Min(Breakpoints.Medium, cssBuilderAction);

    /// <summary>Min helper for <see cref="Breakpoints.Large"/> (64rem).</summary>
    public static IMediaFluentCssBuilder MinLg(this IMediaFluentCssBuilder builder, Action<IFluentCssBuilder> cssBuilderAction) =>
        builder.Min(Breakpoints.Large, cssBuilderAction);

    /// <summary>Min helper for <see cref="Breakpoints.XLarge"/> (80rem).</summary>
    public static IMediaFluentCssBuilder MinXl(this IMediaFluentCssBuilder builder, Action<IFluentCssBuilder> cssBuilderAction) =>
        builder.Min(Breakpoints.XLarge, cssBuilderAction);

    /// <summary>Min helper for <see cref="Breakpoints.XXLarge"/> (96rem).</summary>
    public static IMediaFluentCssBuilder Min2Xl(this IMediaFluentCssBuilder builder, Action<IFluentCssBuilder> cssBuilderAction) =>
        builder.Min(Breakpoints.XXLarge, cssBuilderAction);

    /// <summary>Max helper for <see cref="Breakpoints.XSmall"/> (20rem).</summary>
    public static IMediaFluentCssBuilder MaxXs(this IMediaFluentCssBuilder builder, Action<IFluentCssBuilder> cssBuilderAction) =>
        builder.Max(Breakpoints.XSmall, cssBuilderAction);

    /// <summary>Max helper for <see cref="Breakpoints.Small"/> (40rem).</summary>
    public static IMediaFluentCssBuilder MaxSm(this IMediaFluentCssBuilder builder, Action<IFluentCssBuilder> cssBuilderAction) =>
        builder.Max(Breakpoints.Small, cssBuilderAction);

    /// <summary>Max helper for <see cref="Breakpoints.Medium"/> (48rem).</summary>
    public static IMediaFluentCssBuilder MaxMd(this IMediaFluentCssBuilder builder, Action<IFluentCssBuilder> cssBuilderAction) =>
        builder.Max(Breakpoints.Medium, cssBuilderAction);

    /// <summary>Max helper for <see cref="Breakpoints.Large"/> (64rem).</summary>
    public static IMediaFluentCssBuilder MaxLg(this IMediaFluentCssBuilder builder, Action<IFluentCssBuilder> cssBuilderAction) =>
        builder.Max(Breakpoints.Large, cssBuilderAction);

    /// <summary>Max helper for <see cref="Breakpoints.XLarge"/> (80rem).</summary>
    public static IMediaFluentCssBuilder MaxXl(this IMediaFluentCssBuilder builder, Action<IFluentCssBuilder> cssBuilderAction) =>
        builder.Max(Breakpoints.XLarge, cssBuilderAction);

    /// <summary>Max helper for <see cref="Breakpoints.XXLarge"/> (96rem).</summary>
    public static IMediaFluentCssBuilder Max2Xl(this IMediaFluentCssBuilder builder, Action<IFluentCssBuilder> cssBuilderAction) =>
        builder.Max(Breakpoints.XXLarge, cssBuilderAction);

    /// <summary>
    /// Applies the Tailwind-style <c>container</c>: full width plus a max-width that
    /// steps up at each breakpoint. Centering is not included; add <c>MxAuto()</c>
    /// when the container should be centered.
    /// </summary>
    public static IMediaFluentCssBuilder Container(this IMediaFluentCssBuilder builder)
    {
        builder.WFull();
        builder.MinSm(m => m.MaxW(Length.Px(640)));
        builder.MinMd(m => m.MaxW(Length.Px(768)));
        builder.MinLg(m => m.MaxW(Length.Px(1024)));
        builder.MinXl(m => m.MaxW(Length.Px(1280)));
        builder.Min2Xl(m => m.MaxW(Length.Px(1536)));
        return builder;
    }
}
