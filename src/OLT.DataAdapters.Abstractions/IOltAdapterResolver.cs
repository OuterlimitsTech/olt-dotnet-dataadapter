namespace OLT.Core;

/// <summary>
/// Interface for resolving OLT adapters and performing mapping and projection operations.
/// </summary>
public interface IOltAdapterResolver
{
    /// <summary>
    /// Applies the default order by to the given queryable.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    /// <param name="queryable">The queryable to apply the order by.</param>
    /// <returns>The queryable with the default order by applied.</returns>
    IQueryable<TSource> ApplyDefaultOrderBy<TSource, TDestination>(IQueryable<TSource> queryable);

    /// <summary>
    /// Gets the adapter for the specified source and destination types.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    /// <param name="throwException">Whether to throw an exception if the adapter is not found.</param>
    /// <returns>The adapter for the specified types, or null if not found and throwException is false.</returns>
    IOltAdapter<TSource, TDestination>? GetAdapter<TSource, TDestination>(bool throwException = true);

    /// <summary>
    /// Checks if a projection to the specified destination type can be performed.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    /// <returns>True if the projection can be performed, otherwise false.</returns>
    bool CanProjectTo<TSource, TDestination>();

    /// <summary>
    /// Projects the source queryable to the specified destination type using the configured adapter.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    /// <param name="source">The source queryable.</param>
    /// <param name="configAction">The configuration action for the adapter.</param>
    /// <returns>The projected queryable of the destination type.</returns>
    /// <exception cref="OltAdapterNotFoundException">Thrown if the adapter is not found and throwException is true.</exception>
    IQueryable<TDestination> ProjectTo<TSource, TDestination>(IQueryable<TSource> source, Action<OltAdapterActionConfig>? configAction = null);

    /// <summary>
    /// Checks if a mapping to the specified destination type can be performed.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    /// <returns>True if the mapping can be performed, otherwise false.</returns>
    bool CanMap<TSource, TDestination>();

    /// <summary>
    /// Maps the source list to the specified destination type using the configured adapter.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    /// <param name="source">The source list.</param>
    /// <returns>The mapped list of the destination type.</returns>
    List<TDestination> Map<TSource, TDestination>(List<TSource> source);

    /// <summary>
    /// Maps the source object to the specified destination object using the configured adapter.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    /// <param name="source">The source object.</param>
    /// <param name="destination">The destination object.</param>
    /// <returns>The mapped destination object.</returns>
    TDestination Map<TSource, TDestination>(TSource source, TDestination destination);
}

