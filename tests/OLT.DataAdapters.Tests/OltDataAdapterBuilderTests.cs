using Microsoft.Extensions.DependencyInjection;
using OLT.Core;

namespace OLT.DataAdapters.Tests
{

    public class OltDataAdapterBuilderTests
    {
        [Fact]
        public void Constructor_ShouldInitializeProperties()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var builder = new OltDataAdapterBuilder_Test(services);

            // Assert
            builder.WithServiceLifetime(ServiceLifetime.Singleton);
            Assert.Equal(ServiceLifetime.Singleton, builder.ServiceLifetime);
        }
      

        [Fact]
        public void WithServiceLifetime_ShouldSetServiceLifetime()
        {
            // Arrange
            var services = new ServiceCollection();
            var builder = new OltDataAdapterBuilder_Test(services);

            // Act
            builder.WithServiceLifetime(ServiceLifetime.Transient);

            // Assert
            builder.WithServiceLifetime(ServiceLifetime.Transient);
            Assert.Equal(ServiceLifetime.Transient, builder.ServiceLifetime);
        }

        [Fact]
        public void Build_ShouldAddServicesToServiceCollection()
        {
            // Arrange
            var services = new ServiceCollection();
            var builder = new OltDataAdapterBuilder(services);

            // Act
            builder.Build();

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var resolver = serviceProvider.GetService<IOltAdapterResolver>();
            Assert.NotNull(resolver);
        }

        public class OltDataAdapterBuilder_Test : OltDataAdapterBuilder
        {
            public OltDataAdapterBuilder_Test(IServiceCollection services) : base(services)
            {
            }

            public ServiceLifetime ServiceLifetime => base._serviceLifetime;
        }

    }
}
