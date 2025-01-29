using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.DataAdapters.Tests.AdapterTests.Models;

namespace OLT.DataAdapters.Tests.AdapterTests
{
    public class AdapterTests : BaseAdpaterTests
    {
        [Fact]
        public void Map()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();
                var adapter = new AdapterObject1ToAdapterObject2Adapter();
                var obj1 = BasicAdapterObject1.FakerData();

                var resultObj2 = new BasicAdapterObject2();
                adapter.Map(obj1, resultObj2);
                var mappedObj2 = adapterResolver.Map(obj1, new BasicAdapterObject2());
                Assert.Equal(resultObj2.Name.First, mappedObj2.Name.First);
                Assert.Equal(resultObj2.Name.Last, mappedObj2.Name.Last);

                var resultObj1 = new BasicAdapterObject1();
                adapter.Map(resultObj2, resultObj1);
                Assert.Equal(obj1.FirstName, resultObj1.FirstName);
                Assert.Equal(obj1.LastName, resultObj1.LastName);
                var mappedObj1 = adapterResolver.Map(resultObj2, new BasicAdapterObject1());
                Assert.Equal(resultObj1.FirstName, mappedObj1.FirstName);
                Assert.Equal(resultObj1.LastName, mappedObj1.LastName);
            }
        }
    }


}
