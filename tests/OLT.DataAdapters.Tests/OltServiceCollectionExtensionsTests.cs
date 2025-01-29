using Microsoft.Extensions.DependencyInjection;
using OLT.Core;

namespace OLT.DataAdapters.Tests;

public class OltServiceCollectionExtensionsTests
{
    [Fact]
    public void AddDataAdapters_ShouldThrowArgumentNullException_WhenServicesIsNull()
    {
        // Arrange
        IServiceCollection services = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => services.AddOltAdapters(builder => { }));
    }

    [Fact]
    public void AddDataAdapters_ShouldInvokeAction()
    {
        // Arrange
        var services = new ServiceCollection();
        var actionInvoked = false;

        // Act
        services.AddOltAdapters(builder => { actionInvoked = true; });

        // Assert
        Assert.True(actionInvoked);
    }

    [Fact]
    public void AddDataAdapters_ShouldReturnServiceCollection()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var result = services.AddOltAdapters(builder => { });

        // Assert
        Assert.Same(services, result);
    }
}
