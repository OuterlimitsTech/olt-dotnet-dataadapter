using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace OLT.Core;

/// <summary>
/// <see cref="IOltAdapter"/> Scan Utility using Scutor <seealso cref="ServiceCollectionExtensions.Scan(IServiceCollection, Action{Scrutor.ITypeSourceSelector})"/>
/// </summary>
public class OltDataAdapterBuilder
{
    private readonly List<Assembly> _scanAssemblies = new List<Assembly>();

    /// <summary>
    /// The service collection for dependency injection.
    /// </summary>
    protected readonly IServiceCollection _services;

    /// <summary>
    /// The service lifetime for Automapper.
    /// </summary>
    protected ServiceLifetime _serviceLifetime = ServiceLifetime.Singleton;

    /// <summary>
    /// Requires <see cref="IServiceCollection"/>
    /// </summary>
    /// <param name="services"></param>
    public OltDataAdapterBuilder(IServiceCollection services)
    {
        _services = services;
    }

    /// <summary>
    /// Add mapping definitions contained in assemblies.
    /// Looks for <see cref="IOltAdapter" /> 
    /// </summary>
    /// <param name="assembliesToScan">Assemblies containing mapping definitions</param>
    public virtual OltDataAdapterBuilder AddAdapters(IEnumerable<Assembly> assembliesToScan)
    {
        _scanAssemblies.AddRange(assembliesToScan);
        return this;
    }

    /// <summary>
    /// Add mapping definitions contained in assemblies.
    /// Looks for <see cref="IOltAdapter" /> 
    /// </summary>
    /// <param name="assembliesToScan">Assemblies containing mapping definitions</param>
    public virtual OltDataAdapterBuilder AddAdapters(params Assembly[] assembliesToScan)
    {
        _scanAssemblies.AddRange(assembliesToScan);
        return this;
    }

    /// <summary>
    /// Set the <see cref="ServiceLifetime"/> for Automapper
    /// </summary>
    /// <remarks>
    /// Default is <see cref="ServiceLifetime.Transient"/>
    /// </remarks>
    /// <param name="value"></param>
    /// <returns></returns>
    public virtual OltDataAdapterBuilder WithServiceLifetime(ServiceLifetime value)
    {
        _serviceLifetime = value;
        return this;
    }

    
    public virtual void Build()
    {
        _services.AddSingleton<IOltAdapterResolver, OltAdapterResolver>();

        _services.Scan(sc =>
             sc.FromAssemblies(_scanAssemblies)
                 .AddClasses(classes => classes.AssignableTo<IOltAdapter>())
                 .AsImplementedInterfaces()
                 .WithLifetime(_serviceLifetime));

    }

}
