namespace OLT.Core;

/// <summary>
/// Represents a generic adapter interface for mapping objects.
/// </summary>
public interface IOltAdapter
{
    /// <summary>
    /// Gets the name of the adapter.
    /// </summary>
    string Name { get; }
}

/// <summary>
/// Represents a generic adapter interface for mapping objects between two types.
/// </summary>
/// <typeparam name="TObj1">The type of the first object.</typeparam>
/// <typeparam name="TObj2">The type of the second object.</typeparam>
public interface IOltAdapter<TObj1, TObj2> : IOltAdapter
{
    /// <summary>
    /// Maps the source object of type <typeparamref name="TObj1"/> to the destination object of type <typeparamref name="TObj2"/>.
    /// </summary>
    /// <param name="source">The source object.</param>
    /// <param name="destination">The destination object.</param>
    void Map(TObj1 source, TObj2 destination);

    /// <summary>
    /// Maps the source object of type <typeparamref name="TObj2"/> to the destination object of type <typeparamref name="TObj1"/>.
    /// </summary>
    /// <param name="source">The source object.</param>
    /// <param name="destination">The destination object.</param>
    void Map(TObj2 source, TObj1 destination);

    /// <summary>
    /// Maps a collection of source objects of type <typeparamref name="TObj1"/> to a collection of destination objects of type <typeparamref name="TObj2"/>.
    /// </summary>
    /// <param name="sourceItems">The collection of source objects.</param>
    /// <returns>A collection of mapped destination objects.</returns>
    IEnumerable<TObj2> Map(IEnumerable<TObj1> sourceItems);

    /// <summary>
    /// Maps a collection of source objects of type <typeparamref name="TObj2"/> to a collection of destination objects of type <typeparamref name="TObj1"/>.
    /// </summary>
    /// <param name="sourceItems">The collection of source objects.</param>
    /// <returns>A collection of mapped destination objects.</returns>
    IEnumerable<TObj1> Map(IEnumerable<TObj2> sourceItems);
}