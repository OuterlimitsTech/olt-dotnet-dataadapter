using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.DataAdapters.Tests.PagedAdapterTests.Adapters;
using OLT.DataAdapters.Tests.PagedAdapterTests.Models;

namespace OLT.DataAdapters.Tests.PagedAdapterTests;

public class AdapterTests : BaseAdpaterTests
{
    private readonly Func<IQueryable<PagedAdapterObject1>, IQueryable<PagedAdapterObject1>> _defaultOrder = entity => entity.OrderBy(p => p.LastName).ThenBy(p => p.FirstName);

    [Fact]
    public void Map()
    {
        using (var provider = BuildProvider())
        {
            var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();                
            var adapter = new AdapterObject1ToAdapterObject2PagedAdapter();
            var obj1 = PagedAdapterObject1.FakerData();

            var resultObj2 = new PagedAdapterObject2();
            adapter.Map(obj1, resultObj2);
            var result1 = adapterResolver.Map(obj1, new PagedAdapterObject2());
            Assert.Equal(resultObj2.Name.First, result1.Name.First);
            Assert.Equal(resultObj2.Name.Last, result1.Name.Last);
            //adapterResolver.Map(obj1, new PagedAdapterObject2()).Should().BeEquivalentTo(resultObj2);

            var resultObj1 = new PagedAdapterObject1();
            adapter.Map(resultObj2, resultObj1);
            Assert.Equal(resultObj2.Name.First, resultObj1.FirstName);
            Assert.Equal(resultObj2.Name.Last, resultObj1.LastName);


            var result2 = adapterResolver.Map(resultObj2, new PagedAdapterObject1());
            Assert.Equal(resultObj2.Name.First, result2.FirstName);
            Assert.Equal(resultObj2.Name.Last, result2.LastName);

        }
    }

    [Fact]
    public void MapPaged()
    {
        using (var provider = BuildProvider())
        {
            
            var adapter = new AdapterObject1ToAdapterObject2PagedAdapter();
            var obj1List = PagedAdapterObject1.FakerList(23);
            var queryable = obj1List.AsQueryable();

            var resultQueryable = adapter.Map(queryable);
            Assert.Equal(obj1List.Count, resultQueryable.Count());

            var orderedExpected = _defaultOrder(queryable).ToList();
            var orderedResult = adapter.DefaultOrderBy(queryable).ToList();
            Assert.Equal(orderedExpected.Select(s => s.FirstName), orderedResult.Select(s => s.FirstName));
            Assert.Equal(orderedExpected.Select(s => s.LastName), orderedResult.Select(s => s.LastName));

        }
    }
}
