using Microsoft.Extensions.DependencyInjection;
using OLT.Core;
using OLT.DataAdapters.Tests.AdapterTests.Models;

namespace OLT.DataAdapters.Tests.AdapterTests
{

    public class AdapterResolverTests : BaseAdpaterTests
    {


        [Fact]
        public void GetAdapterTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();
                Assert.NotNull(adapterResolver.GetAdapter<BasicAdapterObject1, BasicAdapterObject2>(false));
                Assert.NotNull(adapterResolver.GetAdapter<BasicAdapterObject1, BasicAdapterObject2>(true));
                Assert.Null(adapterResolver.GetAdapter<BasicAdapterObject1, BasicAdapterObject3>(false));
                Assert.Throws<OltAdapterNotFoundException>(() => adapterResolver.GetAdapter<BasicAdapterObject1, BasicAdapterObject3>(true));


                Assert.Null(adapterResolver.GetAdapter<BasicAdapterObject2, BasicAdapterObject1>(false));
                Assert.NotNull(adapterResolver.GetAdapter<BasicAdapterObject2, BasicAdapterObject3>(false));
                Assert.NotNull(adapterResolver.GetAdapter<BasicAdapterObject2, BasicAdapterObject3>(true));
                Assert.Throws<OltAdapterNotFoundException>(() => adapterResolver.GetAdapter<BasicAdapterObject2, BasicAdapterObject1>(true));

                Assert.Null(adapterResolver.GetAdapter<BasicAdapterObject3, BasicAdapterObject2>(false));
                Assert.Throws<OltAdapterNotFoundException>(() => adapterResolver.GetAdapter<BasicAdapterObject3, BasicAdapterObject2>(true));               
                
            }
        }

        [Fact]
        public void GetAdapterNameTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();
                Assert.Equal($"{typeof(BasicAdapterObject1).FullName}->{typeof(BasicAdapterObject2).FullName}", adapterResolver.GetAdapter<BasicAdapterObject1, BasicAdapterObject2>()?.Name);
                Assert.Equal(OltAdapterExtensions.BuildAdapterName<BasicAdapterObject1, BasicAdapterObject2>(), adapterResolver.GetAdapter<BasicAdapterObject1, BasicAdapterObject2>()?.Name);
                Assert.NotEqual($"{typeof(BasicAdapterObject2)}->{typeof(BasicAdapterObject1).FullName}", adapterResolver.GetAdapter<BasicAdapterObject1, BasicAdapterObject2>()?.Name);
            }
        }

        [Fact]
        public void CanMapTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();
                Assert.True(adapterResolver.CanMap<BasicAdapterObject1, BasicAdapterObject2>());
                Assert.True(adapterResolver.CanMap<BasicAdapterObject2, BasicAdapterObject1>());
                Assert.True(adapterResolver.CanMap<BasicAdapterObject2, BasicAdapterObject3>());
                Assert.True(adapterResolver.CanMap<BasicAdapterObject3, BasicAdapterObject2>());

                Assert.False(adapterResolver.CanMap<BasicAdapterObject1, BasicAdapterObject3>());
                Assert.False(adapterResolver.CanMap<BasicAdapterObject3, BasicAdapterObject1>());

            }
        }

        [Fact]
        public void CanProjectToTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();
                Assert.False(adapterResolver.CanProjectTo<BasicAdapterObject1, BasicAdapterObject2>());
                Assert.False(adapterResolver.CanProjectTo<BasicAdapterObject1, BasicAdapterObject3>());

                Assert.False(adapterResolver.CanProjectTo<BasicAdapterObject2, BasicAdapterObject1>());
                Assert.False(adapterResolver.CanProjectTo<BasicAdapterObject2, BasicAdapterObject3>());

                Assert.False(adapterResolver.CanProjectTo<BasicAdapterObject3, BasicAdapterObject1>());
                Assert.False(adapterResolver.CanProjectTo<BasicAdapterObject3, BasicAdapterObject2>());
            }
        }

        [Fact]
        public void ProjectToTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();
                Assert.Throws<OltAdapterNotFoundException>(() => adapterResolver.ProjectTo<BasicAdapterObject1, BasicAdapterObject2>(BasicAdapterObject1.FakerList(3).AsQueryable()));
                Assert.Throws<OltAdapterNotFoundException>(() => adapterResolver.ProjectTo<BasicAdapterObject2, BasicAdapterObject3>(BasicAdapterObject2.FakerList(3).AsQueryable()));
            }
        }


        [Fact]
        public void MapTests()
        {
            using (var provider = BuildProvider())
            {
                var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();
                var obj1 = BasicAdapterObject1.FakerData();

                var obj2Result = adapterResolver.Map<BasicAdapterObject1, BasicAdapterObject2>(obj1, new BasicAdapterObject2());
                Assert.NotNull(obj2Result);
                Assert.Equal(obj1.FirstName, obj2Result.Name?.First);
                Assert.Equal(obj1.LastName, obj2Result.Name?.Last);
                var mappedBackObj1 = adapterResolver.Map<BasicAdapterObject2, BasicAdapterObject1>(obj2Result, new BasicAdapterObject1());
                Assert.Equal(obj1.FirstName, mappedBackObj1.FirstName);
                Assert.Equal(obj1.LastName, mappedBackObj1.LastName);

                var obj3 = BasicAdapterObject3.FakerData();
                obj2Result = adapterResolver.Map<BasicAdapterObject3, BasicAdapterObject2>(obj3, new BasicAdapterObject2());
                Assert.Equal(obj3.First, obj2Result.Name?.First);
                Assert.Equal(obj3.Last, obj2Result.Name?.Last);
                var mappedBackObj3 = adapterResolver.Map<BasicAdapterObject2, BasicAdapterObject3>(obj2Result, new BasicAdapterObject3());
                Assert.Equal(obj3.First, mappedBackObj3.First);
                Assert.Equal(obj3.Last, mappedBackObj3.Last);
            }
        }

        [Fact]
        public void MapListTests()
        {
            using (var provider = BuildProvider())
            {                
                var adapterResolver = provider.GetRequiredService<IOltAdapterResolver>();
                var obj1Values = BasicAdapterObject1.FakerList(3).OrderBy(p => p.FirstName).ThenBy(p => p.LastName).ToList();
                var obj2Result = adapterResolver.Map<BasicAdapterObject1, BasicAdapterObject2>(obj1Values).OrderBy(p => p.Name.First).ThenBy(p => p.Name.Last).ToList();

                Assert.Equal(obj1Values.Count, obj2Result.Count);                
                Assert.Equal(obj1Values.Select(s => s.FirstName), obj2Result.Select(s => s.Name?.First));
                Assert.Equal(obj1Values.Select(s => s.LastName), obj2Result.Select(s => s.Name?.Last));

                var mappedBackObj1Values = adapterResolver.Map<BasicAdapterObject2, BasicAdapterObject1>(obj2Result).OrderBy(p => p.FirstName).ThenBy(p => p.LastName).ToList();
                Assert.Equal(obj1Values.Count, mappedBackObj1Values.Count);
                Assert.Equal(obj1Values.Select(s => s.FirstName), mappedBackObj1Values.Select(s => s.FirstName));
                Assert.Equal(obj1Values.Select(s => s.LastName), mappedBackObj1Values.Select(s => s.LastName));
                
                var obj3Values = BasicAdapterObject3.FakerList(3).OrderBy(p => p.First).ThenBy(p => p.Last).ToList();
                obj2Result = adapterResolver.Map<BasicAdapterObject3, BasicAdapterObject2>(obj3Values).OrderBy(p => p.Name.First).ThenBy(p => p.Name.Last).ToList();
                Assert.Equal(obj3Values.Count, obj2Result.Count);
                Assert.Equal(obj3Values.Select(s => s.First), obj2Result.Select(s => s.Name?.First));
                Assert.Equal(obj3Values.Select(s => s.Last), obj2Result.Select(s => s.Name?.Last));

                var mappedBackObj3Values = adapterResolver.Map<BasicAdapterObject2, BasicAdapterObject3>(obj2Result).OrderBy(p => p.First).ThenBy(p => p.Last).ToList();
                Assert.Equal(obj3Values.Count, mappedBackObj3Values.Count);
                Assert.Equal(obj3Values.Select(s => s.First), mappedBackObj3Values.Select(s => s.First));
                Assert.Equal(obj3Values.Select(s => s.Last), mappedBackObj3Values.Select(s => s.Last));
            }
        }




    }


}
