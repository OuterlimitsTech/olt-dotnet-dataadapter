using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OLT.Core;
using OLT.DataAdapters.Tests.ProjectToTests.Adapters;

namespace OLT.DataAdapters.Tests.ProjectToTests
{
    public abstract class BaseAdpaterTests 
    {
        protected ServiceProvider BuildProvider()
        {
            var services = new ServiceCollection();
            services.AddLogging(config => config.AddConsole());
            services.AddSingleton<IOltAdapterResolver, OltAdapterResolver>();
            services.AddSingleton<IOltAdapter, AdapterObject1ToAdapterObject2QueryableAdapter>();
            services.AddSingleton<IOltAdapter, AdapterObject2ToAdapterObject3QueryableAdapter>();
            return services.BuildServiceProvider();
        }
    }
}
