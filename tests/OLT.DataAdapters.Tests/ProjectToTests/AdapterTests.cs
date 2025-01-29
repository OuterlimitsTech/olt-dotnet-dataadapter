using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.DataAdapters.Tests.ProjectToTests.Adapters;
using OLT.DataAdapters.Tests.ProjectToTests.Models;

namespace OLT.DataAdapters.Tests.ProjectToTests
{

    public partial class AdapterTests : BaseAdpaterTests
    {
        [Fact]
        public void Map()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();
                var adapter = new AdapterObject1ToAdapterObject2QueryableAdapter();

                Assert.Throws<NotImplementedException>(() => adapter.Map(QueryableAdapterObject1.FakerData(), new QueryableAdapterObject2()));
                Assert.Throws<NotImplementedException>(() => adapterResolver.Map<QueryableAdapterObject1, QueryableAdapterObject2>(QueryableAdapterObject1.FakerData(), new QueryableAdapterObject2()));

                Assert.Throws<NotImplementedException>(() => adapter.Map(QueryableAdapterObject2.FakerData(), new QueryableAdapterObject1()));
                Assert.Throws<NotImplementedException>(() => adapterResolver.Map<QueryableAdapterObject2, QueryableAdapterObject1>(QueryableAdapterObject2.FakerData(), new QueryableAdapterObject1()));
            }
        }


        [Fact]
        public void ProjectTo()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();
                var adapter = new AdapterObject1ToAdapterObject2QueryableAdapter();
                var queryableObj1 = QueryableAdapterObject1.FakerList(3).AsQueryable();

                var obj2Result = adapter.Map(queryableObj1).ToList();
                var result = adapterResolver.ProjectTo<QueryableAdapterObject1, QueryableAdapterObject2>(queryableObj1).OrderBy(p => p.Name.First).ThenBy(p => p.Name.Last);

                Assert.Equal(queryableObj1.OrderBy(p => p.FirstName).ThenBy(p => p.LastName).Select(s => s.FirstName), result.Select(s => s.Name.First));
                Assert.Equal(queryableObj1.OrderBy(p => p.FirstName).ThenBy(p => p.LastName).Select(s => s.LastName), result.Select(s => s.Name.Last));
            }
        }
    }
}
