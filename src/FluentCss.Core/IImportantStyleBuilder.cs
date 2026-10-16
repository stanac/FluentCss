using System;

namespace FluentCss.Core;

/// <summary>
/// A builder from which an <c>!important</c> override block can be started.
/// </summary>
/// <remarks>
/// The lambda passed to <see cref="Important"/> receives an
/// <see cref="IStyleScopeBuilder"/>, which deliberately has no <c>Important</c>
/// method, so override blocks cannot be nested inside one another.
/// </remarks>
public interface IImportantStyleBuilder<out T> : IStyleBuilder<T>
{
    /// <summary>
    /// Marks every declaration added in <paramref name="styles"/> as
    /// <c>!important</c>. Several declarations can be added in one call.
    /// </summary>
    T Important(Action<IStyleScopeBuilder> styles);
}

/// <summary>
/// The builder passed to <see cref="IImportantStyleBuilder{T}.Important"/>.
/// It exposes the style utilities but not <c>Important</c>.
/// </summary>
public interface IStyleScopeBuilder : IStyleBuilder<IStyleScopeBuilder>
{
}
