using Microsoft.Extensions.DependencyInjection;
using OLT.Utility.AssemblyScanner;
using System.Reflection;

namespace OLT.Core;

public static class OltServiceCollectionExtensions
{
    /// <summary>
    /// Adds data adapters to the service collection and scans the specified assemblies.
    /// </summary>
    /// <remarks>
    /// uses Scutor <seealso cref="ServiceCollectionExtensions.Scan(IServiceCollection, Action{Scrutor.ITypeSourceSelector})"/>
    /// </remarks>
    /// <param name="services">The IServiceCollection to add the services to.</param>
    /// <param name="action">An action to configure the OltAutoMapperBuilder.</param>
    /// <returns>The IServiceCollection with the added services.</returns>
    public static IServiceCollection AddOltAdapters(this IServiceCollection services, Action<OltDataAdapterBuilder> action)
    {
        ArgumentNullException.ThrowIfNull(services);
        var builder = new OltDataAdapterBuilder(services);
        action(builder);
        builder.Build();
        return services;
    }

    ///// <summary>
    ///// Adds data adapters to the service collection and scans the specified assemblies.
    ///// </summary>
    ///// <remarks>
    ///// <list type="bullet">
    ///// <item>
    ///// Scans Assemblies <see cref="IOltAdapter"/> using Scutor <seealso cref="ServiceCollectionExtensions.Scan(IServiceCollection, Action{Scrutor.ITypeSourceSelector})"/>
    ///// </item>
    ///// <item>
    ///// See Scanner <a href="https://github.com/OuterlimitsTech/olt-dotnet-utility-libraries/blob/e77797d1c8a783fe7fda49968c5a45bf59add7d8/src/OLT.Utility.AssemblyScanner/README.md">documentation</a> for more details.
    ///// </item>
    ///// </list>
    ///// </remarks>
    ///// <param name="services"></param>
    ///// <returns></returns>
    //public static OltDataAdapterBuilder AddOltAdapters(this IServiceCollection services)
    //{
    //    ArgumentNullException.ThrowIfNull(services);
    //    return new OltDataAdapterBuilder(services);
    //}

    ///// <summary>
    ///// Adds data adapters to the service collection and scans the specified assemblies.
    ///// </summary>
    ///// <remarks>
    ///// Scans Assemblies <see cref="IOltAdapter"/> using Scutor <seealso cref="ServiceCollectionExtensions.Scan(IServiceCollection, Action{Scrutor.ITypeSourceSelector})"/>
    ///// </remarks>
    ///// <param name="services">The service collection to add the data adapters to.</param>
    ///// <param name="assemblies">The assemblies to scan for data adapters.</param>
    ///// <returns>An instance of <see cref="OltDataAdapterBuilder"/> to configure the data adapters.</returns>
    ///// <exception cref="ArgumentNullException">Thrown when the services parameter is null.</exception>
    //public static OltDataAdapterBuilder AddOltAdapters(this IServiceCollection services, params Assembly[] assemblies)
    //{
    //    ArgumentNullException.ThrowIfNull(services);
    //    var builder = new OltDataAdapterBuilder(services);
    //    builder.AssemblyScanner.IncludeAssembly(assemblies);
    //    return builder;
    //}

    ///// <summary>
    ///// Scans Assemblies <see cref="IOltAdapter"/> using Scutor <seealso cref="ServiceCollectionExtensions.Scan(IServiceCollection, Action{Scrutor.ITypeSourceSelector})"/>
    ///// </summary>
    ///// <remarks>
    ///// See <a href="https://github.com/OuterlimitsTech/olt-dotnet-utility-libraries/blob/e77797d1c8a783fe7fda49968c5a45bf59add7d8/src/OLT.Utility.AssemblyScanner/README.md">documentation</a> for more details.
    ///// </remarks>
    ///// <param name="services"></param>
    ///// <param name="assemblyScanner"></param>
    ///// <returns></returns>
    //public static OltDataAdapterBuilder AddOltAdapters(this IServiceCollection services, OltAssemblyScanBuilder assemblyScanner)
    //{
    //    ArgumentNullException.ThrowIfNull(services);
    //    return new OltDataAdapterBuilder(services, assemblyScanner);
    //}

}
