using System.Collections.Concurrent;

namespace OLT.Core;

/// <summary>
/// Provides configuration for OLT Adapter mappings.
/// </summary>
public static class OltAdapterMapConfigs
{
    /// <summary>
    /// Configuration for mappings to be applied before the main mapping.
    /// </summary>
    public static class BeforeMap
    {
        private static readonly ConcurrentDictionary<string, IOltAdapterMapConfig> _mapConfigs = new ConcurrentDictionary<string, IOltAdapterMapConfig>();

        /// <summary>
        /// Checks if a before map configuration is registered for the given source and destination types.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <typeparam name="TDestination">The destination type.</typeparam>
        /// <returns>True if registered, otherwise false.</returns>
        public static bool IsRegistered<TSource, TDestination>()
        {
            var name = OltAdapterExtensions.BuildBeforeMapName<TSource, TDestination>();
            return _mapConfigs.ContainsKey(name);
        }

        /// <summary>
        /// Applies the before map configuration to the given queryable if registered.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <typeparam name="TDestination">The destination type.</typeparam>
        /// <param name="queryable">The queryable to apply the configuration to.</param>
        /// <returns>The modified queryable.</returns>
        public static IQueryable<TSource> Apply<TSource, TDestination>(IQueryable<TSource> queryable)
        {
            if (IsRegistered<TSource, TDestination>())
            {
                var name = OltAdapterExtensions.BuildBeforeMapName<TSource, TDestination>();
                var mapConfig = _mapConfigs[name] as IOltBeforeMap<TSource, TDestination> ?? throw new InvalidCastException("IOltBeforeMap");
                queryable = mapConfig.BeforeMap(queryable);
            }
            return queryable;
        }

        /// <summary>
        /// Registers a before map configuration.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <typeparam name="TDestination">The destination type.</typeparam>
        /// <param name="configMap">The configuration map to register.</param>
        /// <param name="throwException">Whether to throw an exception if the configuration already exists.</param>
        /// <returns>True if registered successfully, otherwise false.</returns>
        public static bool Register<TSource, TDestination>(IOltBeforeMap<TSource, TDestination> configMap, bool throwException)
        {
            var name = OltAdapterExtensions.BuildBeforeMapName<TSource, TDestination>();
            if (!_mapConfigs.ContainsKey(name))
            {
                return _mapConfigs.TryAdd(name, configMap);
            }

            if (throwException)
            {
                throw new OltAdapterMapConfigExistsException<TSource, TDestination>(configMap);
            }

            return false;
        }

        /// <summary>
        /// Registers a before map configuration using a function.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <typeparam name="TDestination">The destination type.</typeparam>
        /// <param name="func">The function to register.</param>
        /// <param name="throwException">Whether to throw an exception if the configuration already exists.</param>
        /// <returns>True if registered successfully, otherwise false.</returns>
        public static bool Register<TSource, TDestination>(Func<IQueryable<TSource>, IOrderedQueryable<TSource>> func, bool throwException)
        {
            return Register(new OltBeforeMapOrderBy<TSource, TDestination>(func), throwException);
        }
    }

    /// <summary>
    /// Configuration for mappings to be applied after the main mapping.
    /// </summary>
    public static class AfterMap
    {
        private static readonly ConcurrentDictionary<string, IOltAdapterMapConfig> _mapConfigs = new ConcurrentDictionary<string, IOltAdapterMapConfig>();

        /// <summary>
        /// Checks if an after map configuration is registered for the given source and destination types.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <typeparam name="TDestination">The destination type.</typeparam>
        /// <returns>True if registered, otherwise false.</returns>
        public static bool IsRegistered<TSource, TDestination>()
        {
            var name = OltAdapterExtensions.BuildAfterMapName<TSource, TDestination>();
            return _mapConfigs.ContainsKey(name);
        }

        /// <summary>
        /// Applies the after map configuration to the given queryable if registered.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <typeparam name="TDestination">The destination type.</typeparam>
        /// <param name="queryable">The queryable to apply the configuration to.</param>
        /// <returns>The modified queryable.</returns>
        public static IQueryable<TDestination> Apply<TSource, TDestination>(IQueryable<TDestination> queryable)
        {
            if (IsRegistered<TSource, TDestination>())
            {
                var name = OltAdapterExtensions.BuildAfterMapName<TSource, TDestination>();
                var mapConfig = _mapConfigs[name] as IOltAfterMap<TSource, TDestination> ?? throw new InvalidCastException("IOltAfterMap");
                queryable = mapConfig.AfterMap(queryable);
            }
            return queryable;
        }

        /// <summary>
        /// Registers an after map configuration.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <typeparam name="TDestination">The destination type.</typeparam>
        /// <param name="configMap">The configuration map to register.</param>
        /// <param name="throwException">Whether to throw an exception if the configuration already exists.</param>
        /// <returns>True if registered successfully, otherwise false.</returns>
        public static bool Register<TSource, TDestination>(IOltAfterMap<TSource, TDestination> configMap, bool throwException)
        {
            var name = OltAdapterExtensions.BuildAfterMapName<TSource, TDestination>();
            if (!_mapConfigs.ContainsKey(name))
            {
                return _mapConfigs.TryAdd(name, configMap);
            }

            if (throwException)
            {
                throw new OltAdapterMapConfigExistsException<TSource, TDestination>(configMap);
            }

            return false;
        }

        /// <summary>
        /// Registers an after map configuration using a function.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <typeparam name="TDestination">The destination type.</typeparam>
        /// <param name="func">The function to register.</param>
        /// <param name="throwException">Whether to throw an exception if the configuration already exists.</param>
        /// <returns>True if registered successfully, otherwise false.</returns>
        public static bool Register<TSource, TDestination>(Func<IQueryable<TDestination>, IOrderedQueryable<TDestination>> func, bool throwException)
        {
            return Register(new OltAfterMapOrderBy<TSource, TDestination>(func), throwException);
        }
    }
}

