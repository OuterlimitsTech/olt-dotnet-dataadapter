using Microsoft.Extensions.DependencyInjection;
using OLT.Utility.AssemblyScanner;

namespace OLT.Core;

public static class OltServiceCollectionExtensions
{
    /// <summary>
    /// Scans Assemblies <see cref="IOltAdapter"/> using Scutor <seealso cref="ServiceCollectionExtensions.Scan(IServiceCollection, Action{Scrutor.ITypeSourceSelector})"/>
    /// </summary>
    /// <remarks>
    /// See <a href="https://github.com/OuterlimitsTech/olt-dotnet-utility-libraries/blob/e77797d1c8a783fe7fda49968c5a45bf59add7d8/src/OLT.Utility.AssemblyScanner/README.md">documentation</a> for more details.
    /// </remarks>
    /// <param name="services"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IServiceCollection AddDataAdapters(this IServiceCollection services, Action<OltAssemblyScanBuilder> action)
    {
        ArgumentNullException.ThrowIfNull(services);

        var assemblyScanner = new OltAssemblyScanBuilder().ExcludeMicrosoft().ExcludeAutomapper();
        action(assemblyScanner);
        var assemblies = assemblyScanner.Build();
        return services;
    }

}
