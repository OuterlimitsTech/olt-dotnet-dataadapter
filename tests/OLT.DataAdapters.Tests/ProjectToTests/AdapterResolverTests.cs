using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.DataAdapters.Tests.ProjectToTests.Models;

namespace OLT.DataAdapters.Tests.ProjectToTests;


public class AdapterResolverTests : BaseAdpaterTests
{


    [Fact]
    public void GetAdapterTests()
    {
        using (var provider = BuildProvider())
        {
            var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();
            Assert.NotNull(adapterResolver.GetAdapter<QueryableAdapterObject1, QueryableAdapterObject2>(false));
            Assert.NotNull(adapterResolver.GetAdapter<QueryableAdapterObject1, QueryableAdapterObject2>(true));
            Assert.Null(adapterResolver.GetAdapter<QueryableAdapterObject1, QueryableAdapterObject3>(false));
            Assert.Throws<OltAdapterNotFoundException>(() => adapterResolver.GetAdapter<QueryableAdapterObject1, QueryableAdapterObject3>(true));


            Assert.Null(adapterResolver.GetAdapter<QueryableAdapterObject2, QueryableAdapterObject1>(false));
            Assert.NotNull(adapterResolver.GetAdapter<QueryableAdapterObject2, QueryableAdapterObject3>(false));
            Assert.NotNull(adapterResolver.GetAdapter<QueryableAdapterObject2, QueryableAdapterObject3>(true));
            Assert.Throws<OltAdapterNotFoundException>(() => adapterResolver.GetAdapter<QueryableAdapterObject2, QueryableAdapterObject1>(true));

            Assert.Null(adapterResolver.GetAdapter<QueryableAdapterObject3, QueryableAdapterObject2>(false));
            Assert.Throws<OltAdapterNotFoundException>(() => adapterResolver.GetAdapter<QueryableAdapterObject3, QueryableAdapterObject2>(true));

        }
    }



    [Fact]
    public void CanProjectToTests()
    {
        using (var provider = BuildProvider())
        {
            var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();
            Assert.True(adapterResolver.CanProjectTo<QueryableAdapterObject1, QueryableAdapterObject2>());
            Assert.False(adapterResolver.CanProjectTo<QueryableAdapterObject1, QueryableAdapterObject3>());

            Assert.False(adapterResolver.CanProjectTo<QueryableAdapterObject2, QueryableAdapterObject1>());
            Assert.True(adapterResolver.CanProjectTo<QueryableAdapterObject2, QueryableAdapterObject3>());

            Assert.False(adapterResolver.CanProjectTo<QueryableAdapterObject3, QueryableAdapterObject1>());
            Assert.False(adapterResolver.CanProjectTo<QueryableAdapterObject3, QueryableAdapterObject2>());
        }
    }

    [Fact]
    public void ProjectToTests()
    {
        using (var provider = BuildProvider())
        {
            var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();
            var obj1Values = QueryableAdapterObject1.FakerList(23).OrderBy(p => p.FirstName).ThenBy(p => p.LastName).ToList();

            var obj2ResultQueryable = adapterResolver.ProjectTo<QueryableAdapterObject1, QueryableAdapterObject2>(obj1Values.AsQueryable());
            Assert.Throws<OltAdapterNotFoundException>(() => adapterResolver.ProjectTo<QueryableAdapterObject2, QueryableAdapterObject1>(QueryableAdapterObject2.FakerList(5).AsQueryable()));


            var result2 = adapterResolver
                .ProjectTo<QueryableAdapterObject1, QueryableAdapterObject2>(obj1Values.AsQueryable(), configAction => { configAction.DisableBeforeMap = true; configAction.DisableAfterMap = true; })
                .OrderBy(p => p.Name.First).ThenBy(p => p.Name.Last)
                .ToList();

            Assert.Equal(obj1Values.Select(s => s.FirstName), result2.Select(s => s.Name.First));
            Assert.Equal(obj1Values.Select(s => s.LastName), result2.Select(s => s.Name.Last));
        }
    }


    [Fact]
    public void CanMapTests()
    {
        using (var provider = BuildProvider())
        {
            var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();
            Assert.True(adapterResolver.CanMap<QueryableAdapterObject1, QueryableAdapterObject2>());
            Assert.True(adapterResolver.CanMap<QueryableAdapterObject2, QueryableAdapterObject1>());
            Assert.True(adapterResolver.CanMap<QueryableAdapterObject2, QueryableAdapterObject3>());
            Assert.True(adapterResolver.CanMap<QueryableAdapterObject3, QueryableAdapterObject2>());

            Assert.False(adapterResolver.CanMap<QueryableAdapterObject1, QueryableAdapterObject3>());
            Assert.False(adapterResolver.CanMap<QueryableAdapterObject3, QueryableAdapterObject1>());
        }
    }

    [Fact]
    public void MapTests()
    {
        using (var provider = BuildProvider())
        {
            var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();
            Assert.Throws<NotImplementedException>(() => adapterResolver.Map<QueryableAdapterObject1, QueryableAdapterObject2>(QueryableAdapterObject1.FakerData(), new QueryableAdapterObject2()));
            Assert.Throws<NotImplementedException>(() => adapterResolver.Map<QueryableAdapterObject2, QueryableAdapterObject3>(QueryableAdapterObject2.FakerData(), new QueryableAdapterObject3()));
        }
    }

    [Fact]
    public void MapListTests()
    {
        using (var provider = BuildProvider())
        {
            var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();
            Assert.Throws<AggregateException>(() => adapterResolver.Map<QueryableAdapterObject1, QueryableAdapterObject2>(QueryableAdapterObject1.FakerList(3)));
            Assert.Throws<AggregateException>(() => adapterResolver.Map<QueryableAdapterObject2, QueryableAdapterObject3>(QueryableAdapterObject2.FakerList(3)));
        }
    }
}
