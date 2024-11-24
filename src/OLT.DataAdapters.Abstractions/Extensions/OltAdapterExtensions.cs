namespace OLT.Core;


public static class OltAdapterExtensions
{
    /// <summary>
    /// Builds the adapter name using the full names of the source and destination types.
    /// </summary>
    /// <typeparam name="TObj1">The type of the source object.</typeparam>
    /// <typeparam name="TObj2">The type of the destination object.</typeparam>
    /// <returns>A string representing the adapter name.</returns>
    public static string BuildAdapterName<TObj1, TObj2>()
    {
        return $"{typeof(TObj1).FullName}->{typeof(TObj2).FullName}";
    }

    /// <summary>
    /// Builds the name for the AfterMap configuration using the adapter name.
    /// </summary>
    /// <typeparam name="TObj1">The type of the source object.</typeparam>
    /// <typeparam name="TObj2">The type of the destination object.</typeparam>
    /// <returns>A string representing the AfterMap name.</returns>
    public static string BuildAfterMapName<TObj1, TObj2>()
    {
        return $"{BuildAdapterName<TObj1, TObj2>()}_AfterMap";
    }

    /// <summary>
    /// Builds the name for the BeforeMap configuration using the adapter name.
    /// </summary>
    /// <typeparam name="TObj1">The type of the source object.</typeparam>
    /// <typeparam name="TObj2">The type of the destination object.</typeparam>
    /// <returns>A string representing the BeforeMap name.</returns>
    public static string BuildBeforeMapName<TObj1, TObj2>()
    {
        return $"{BuildAdapterName<TObj1, TObj2>()}_BeforeMap";
    }

    /// <summary>
    /// Sets default OrderBy of <typeparamref name="TSource"/> for Paged Resultsets.
    /// </summary>
    /// <typeparam name="TSource">The type of the source object.</typeparam>
    /// <typeparam name="TDestination">The type of the destination object.</typeparam>
    /// <param name="adapter">The adapter instance.</param>
    /// <param name="func">The function to order the source queryable.</param>
    /// <returns>True if the registration was successful, otherwise false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the adapter or func is null.</exception>
    public static bool WithOrderBy<TSource, TDestination>(this IOltAdapterQueryable<TSource, TDestination> adapter, Func<IQueryable<TSource>, IOrderedQueryable<TSource>> func)
        where TSource : class
    {
        ArgumentNullException.ThrowIfNull(adapter);
        ArgumentNullException.ThrowIfNull(func);

        return OltAdapterMapConfigs.BeforeMap.Register<TSource, TDestination>(func, false);
    }

    /// <summary>
    /// Sets default OrderBy of <typeparamref name="TDestination"/> for Paged Resultsets.
    /// </summary>
    /// <typeparam name="TSource">The type of the source object.</typeparam>
    /// <typeparam name="TDestination">The type of the destination object.</typeparam>
    /// <param name="adapter">The adapter instance.</param>
    /// <param name="func">The function to order the destination queryable.</param>
    /// <returns>True if the registration was successful, otherwise false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the adapter or func is null.</exception>
    public static bool WithOrderBy<TSource, TDestination>(this IOltAdapterQueryable<TSource, TDestination> adapter, Func<IQueryable<TDestination>, IOrderedQueryable<TDestination>> func)
        where TSource : class
    {
        ArgumentNullException.ThrowIfNull(adapter);
        ArgumentNullException.ThrowIfNull(func);

        return OltAdapterMapConfigs.AfterMap.Register<TSource, TDestination>(func, false);
    }

    /// <summary>
    /// Registers a BeforeMap configuration for the adapter.
    /// </summary>
    /// <typeparam name="TSource">The type of the source object.</typeparam>
    /// <typeparam name="TDestination">The type of the destination object.</typeparam>
    /// <param name="adapter">The adapter instance.</param>
    /// <param name="beforeMap">The BeforeMap configuration.</param>
    /// <returns>True if the registration was successful, otherwise false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the adapter or beforeMap is null.</exception>
    public static bool BeforeMap<TSource, TDestination>(this IOltAdapterQueryable<TSource, TDestination> adapter, IOltBeforeMap<TSource, TDestination> beforeMap)
    {
        ArgumentNullException.ThrowIfNull(adapter);
        ArgumentNullException.ThrowIfNull(beforeMap);

        return OltAdapterMapConfigs.BeforeMap.Register(beforeMap, false);
    }

    /// <summary>
    /// Registers an AfterMap configuration for the adapter.
    /// </summary>
    /// <typeparam name="TSource">The type of the source object.</typeparam>
    /// <typeparam name="TDestination">The type of the destination object.</typeparam>
    /// <param name="adapter">The adapter instance.</param>
    /// <param name="afterMap">The AfterMap configuration.</param>
    /// <returns>True if the registration was successful, otherwise false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the adapter or afterMap is null.</exception>
    public static bool AfterMap<TSource, TDestination>(this IOltAdapterQueryable<TSource, TDestination> adapter, IOltAfterMap<TSource, TDestination> afterMap)
        where TSource : class
    {
        ArgumentNullException.ThrowIfNull(adapter);
        ArgumentNullException.ThrowIfNull(afterMap);

        return OltAdapterMapConfigs.AfterMap.Register(afterMap, false);
    }
}
