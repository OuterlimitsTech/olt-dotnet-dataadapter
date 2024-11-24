using System;
using OLT.Core;
using OLT.DataAdapters.Tests.Mock;
using Xunit;

namespace OLT.DataAdapters.Abstractions.Tests;

public class OltAdapterNotFoundExceptionTests
{
    [Fact]
    public void Constructor_ShouldSetMessage()
    {
        // Arrange
        var adapterName = "TestAdapter";
        var expectedMessage = $"Adapter Not Found {adapterName}";

        // Act
        var exception = new OltAdapterNotFoundException(adapterName);

        // Assert
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void GenericConstructor_ShouldSetMessage()
    {
        // Arrange
        var expectedMessage = $"Adapter Not Found {OltAdapterExtensions.BuildAdapterName<MockSource, MockDestination>()}";

        // Act
        var exception = new OltAdapterNotFoundException<MockSource, MockDestination>();

        // Assert
        Assert.Equal(expectedMessage, exception.Message);
    }
}

