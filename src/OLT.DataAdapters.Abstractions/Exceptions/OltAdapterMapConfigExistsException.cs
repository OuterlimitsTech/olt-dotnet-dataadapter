namespace OLT.Core;

public class OltAdapterMapConfigExistsException : Exception
{

    public OltAdapterMapConfigExistsException(string configMapName, string source, string destination) :
        base($"{configMapName} already exists for {source} -> {destination}")
    {

    }

}

public class OltAdapterMapConfigExistsException<TSource, TDestination> : OltAdapterMapConfigExistsException
{

    public OltAdapterMapConfigExistsException(IOltAdapterMapConfig<TSource, TDestination> configMap) :  base(OltAdapterExtensions.BuildBeforeMapName<TSource, TDestination>(), typeof(TSource).FullName ?? "Unknown",  typeof(TDestination).FullName ?? "Unknown")
    {

    }

}