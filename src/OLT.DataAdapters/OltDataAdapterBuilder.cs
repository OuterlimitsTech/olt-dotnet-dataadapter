using Microsoft.Extensions.DependencyInjection;
using OLT.Utility.AssemblyScanner;

namespace OLT.Core;

/// <summary>
/// <see cref="IOltAdapter"/> Scan Utility using Scutor <seealso cref="ServiceCollectionExtensions.Scan(IServiceCollection, Action{Scrutor.ITypeSourceSelector})"/>
/// </summary>
public class OltDataAdapterBuilder
{
    /// <summary>
    /// The service collection for dependency injection.
    /// </summary>
    protected readonly IServiceCollection _services;
    /// <summary>
    /// The assembly scanner for Automapper.
    /// </summary>
    protected readonly OltAssemblyScanBuilder _assemblyScanner;
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
        _assemblyScanner = new OltAssemblyScanBuilder().ExcludeMicrosoft().ExcludeAutomapper();
    }

    /// <summary>
    /// Requires <see cref="IServiceCollection"/> a
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assemblyScanner"></param>
    public OltDataAdapterBuilder(IServiceCollection services, OltAssemblyScanBuilder assemblyScanner)
    {
        _services = services;
        _assemblyScanner = assemblyScanner;
    }

    public OltAssemblyScanBuilder AssemblyScanner
    {
        get
        {
            return _assemblyScanner;
        }
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

        _assemblyScanner.ExcludeAutomapper();
        var assemblies = _assemblyScanner.Build();

        _services.Scan(sc =>
             sc.FromAssemblies(assemblies)
                 .AddClasses(classes => classes.AssignableTo<IOltAdapter>())
                 .AsImplementedInterfaces()
                 .WithLifetime(_serviceLifetime));

    }

}
