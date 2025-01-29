using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.DataAdapters.Tests.Assets.Models;
using OLT.DataAdapters.Tests.PagedAdapterTests.Models;

namespace OLT.DataAdapters.Tests.PagedAdapterTests
{

    public class AdapterResolverTests : BaseAdpaterTests
    {


        [Fact]
        public void GetAdapterTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();
                Assert.NotNull(adapterResolver.GetAdapter<PagedAdapterObject1, PagedAdapterObject2>(false));
                Assert.NotNull(adapterResolver.GetAdapter<PagedAdapterObject1, PagedAdapterObject2>(true));
                Assert.Null(adapterResolver.GetAdapter<PagedAdapterObject1, PagedAdapterObject3>(false));
                Assert.Throws<OltAdapterNotFoundException>(() => adapterResolver.GetAdapter<PagedAdapterObject1, PagedAdapterObject3>(true));                
            }
        }

        [Fact]
        public void GetAdapterNameTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();
                Assert.Equal($"{typeof(PagedAdapterObject1).FullName}->{typeof(PagedAdapterObject2).FullName}", adapterResolver.GetAdapter<PagedAdapterObject1, PagedAdapterObject2>()?.Name);
                Assert.Equal(OltAdapterExtensions.BuildAdapterName<PagedAdapterObject1, PagedAdapterObject2>(), adapterResolver.GetAdapter<PagedAdapterObject1, PagedAdapterObject2>()?.Name);
                Assert.NotEqual($"{typeof(PagedAdapterObject2)}->{typeof(PagedAdapterObject1).FullName}", adapterResolver.GetAdapter<PagedAdapterObject1, PagedAdapterObject2>()?.Name);
            }
        }

        [Fact]
        public void CanMapTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();
                Assert.True(adapterResolver.CanMap<PagedAdapterObject1, PagedAdapterObject2>());
                Assert.True(adapterResolver.CanMap<PagedAdapterObject2, PagedAdapterObject1>());
                Assert.False(adapterResolver.CanMap<PagedAdapterObject2, PagedAdapterObject3>());
                Assert.False(adapterResolver.CanMap<PagedAdapterObject3, PagedAdapterObject2>());
                Assert.True(adapterResolver.CanMap<PagedAdapterObject1, PagedAdapterObject3>());
                Assert.True(adapterResolver.CanMap<PagedAdapterObject3, PagedAdapterObject1>());
            }
        }

       

        [Fact]
        public void CanProjectToTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();
                Assert.True(adapterResolver.CanProjectTo<PagedAdapterObject1, PagedAdapterObject2>());
                Assert.False(adapterResolver.CanProjectTo<PagedAdapterObject1, PagedAdapterObject3>());

                Assert.False(adapterResolver.CanProjectTo<PagedAdapterObject2, PagedAdapterObject1>());
                Assert.False(adapterResolver.CanProjectTo<PagedAdapterObject2, PagedAdapterObject3>());

                Assert.True(adapterResolver.CanProjectTo<PagedAdapterObject3, PagedAdapterObject1>());
                Assert.False(adapterResolver.CanProjectTo<PagedAdapterObject3, PagedAdapterObject2>());
            }
        }

        [Fact]
        public void ProjectToTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();
                Assert.Throws<OltAdapterNotFoundException>(() => adapterResolver.ProjectTo<PagedAdapterObject2, PagedAdapterObject3>(PagedAdapterObject2.FakerList(3).AsQueryable()));

                var obj1Values = PagedAdapterObject1.FakerList(5).AsQueryable();
                var obj2ResultQueryable = adapterResolver.ProjectTo<PagedAdapterObject1, PagedAdapterObject2>(obj1Values.AsQueryable());

                var expected = obj1Values
                .Select(s => new PagedAdapterObject2
                {
                    Name = new PersonName
                    {
                        First = s.FirstName,
                        Last = s.LastName,
                    }
                }).ToList();

                var results = obj2ResultQueryable.ToList();
                var expected2 = expected.OrderBy(p => p.Name!.Last).ThenBy(p => p.Name!.First).ToList();

                Assert.Equal(expected2.Select(s => s.Name.First), results.Select(s => s.Name.First));
                Assert.Equal(expected2.Select(s => s.Name.Last), results.Select(s => s.Name.Last));

                var projectedResult = adapterResolver
                    .ProjectTo<PagedAdapterObject1, PagedAdapterObject2>(obj1Values.AsQueryable(), configAction => { configAction.DisableBeforeMap = true; configAction.DisableAfterMap = true; })
                    .ToList();

                Assert.Equal(expected.Select(s => s.Name.First), projectedResult.Select(s => s.Name.First));
                Assert.Equal(expected.Select(s => s.Name.Last), projectedResult.Select(s => s.Name.Last));

                try
                {
                    adapterResolver.ProjectTo<PagedAdapterObject1, PagedAdapterObject2>(PagedAdapterObject1.FakerList(3).AsQueryable());
                    Assert.True(true);
                }
                catch
                {
                    Assert.True(false);
                }
            }
        }

        [Fact]
        public void ApplyDefaultOrderByTest()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();
                var obj1Values = PagedAdapterObject1.FakerList(56);
                var obj2Result = adapterResolver.ApplyDefaultOrderBy<PagedAdapterObject1, PagedAdapterObject2>(obj1Values.AsQueryable()).ToList();
                var expected = obj1Values.OrderBy(p => p.LastName).ThenBy(p => p.FirstName).ToList();
                Assert.Equal(expected.Select(s => s.FirstName), obj2Result.Select(s => s.FirstName));
                Assert.Equal(expected.Select(s => s.LastName), obj2Result.Select(s => s.LastName));
            }
        }

        [Fact]
        public void MapTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();
                var obj1 = PagedAdapterObject1.FakerData();

                var obj2Result = adapterResolver.Map<PagedAdapterObject1, PagedAdapterObject2>(obj1, new PagedAdapterObject2());
                Assert.Equal(obj1.FirstName, obj2Result!.Name!.First);
                Assert.Equal(obj1.LastName, obj2Result.Name!.Last);
                var mappedResult = adapterResolver.Map<PagedAdapterObject2, PagedAdapterObject1>(obj2Result, new PagedAdapterObject1());
                Assert.Equal(obj1.FirstName, mappedResult.FirstName);
                Assert.Equal(obj1.LastName, mappedResult.LastName);

            }
        }

        [Fact]
        public void MapListTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();
                var obj1Values = PagedAdapterObject1.FakerList(3).OrderBy(p => p.FirstName).ThenBy(p => p.LastName).ToList();
                var obj2Result = adapterResolver.Map<PagedAdapterObject1, PagedAdapterObject2>(obj1Values).OrderBy(p => p.Name.First).ThenBy(p => p.Name.Last).ToList();
                Assert.Equal(obj1Values.Count, obj2Result.Count);
                Assert.Equal(obj1Values.Select(s => s.FirstName), obj2Result.Select(s => s.Name!.First));
                Assert.Equal(obj1Values.Select(s => s.LastName), obj2Result.Select(s => s.Name!.Last));
                var mappedResult = adapterResolver.Map<PagedAdapterObject2, PagedAdapterObject1>(obj2Result).OrderBy(p => p.FirstName).ThenBy(p => p.LastName).ToList();
                Assert.Equal(obj1Values.Select(s => s.FirstName), mappedResult.Select(s => s.FirstName));
                Assert.Equal(obj1Values.Select(s => s.LastName), mappedResult.Select(s => s.LastName));
            }
        }


        //[Fact]
        //public void PagedTests()
        //{
        //    using (var provider = BuildProvider())
        //    {
        //        var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();
        //        var obj3Values = PagedAdapterObject3.FakerList(10);
        //        var queryable = obj3Values.AsQueryable();

        //        var expectedResults = obj3Values.Select(s => new PagedAdapterObject1 { FirstName = s.First, LastName = s.Last });

        //        var pagingParams = new OltPagingParams { Page = 1, Size = 25 };
        //        var results = adapterResolver.ProjectTo<PagedAdapterObject3, PagedAdapterObject1>(queryable).ToPaged(pagingParams);
        //        results.Data.Should().BeEquivalentTo(expectedResults.OrderBy(p => p.LastName).ThenBy(p => p.FirstName));

        //    }
        //}

    }


}
