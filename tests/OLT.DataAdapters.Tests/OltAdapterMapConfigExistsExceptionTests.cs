using System;
using OLT.Core;
using OLT.DataAdapters.Tests.Mock;
using Xunit;

namespace OLT.DataAdapters.Abstractions.Tests;

public class OltAdapterMapConfigExistsExceptionTests
{
    [Fact]
    public void Constructor_ShouldSetMessage()
    {
        // Arrange
        var configMapName = "TestConfigMap";
        var source = "SourceType";
        var destination = "DestinationType";
        var expectedMessage = $"{configMapName} already exists for {source} -> {destination}";

        // Act
        var exception = new OltAdapterMapConfigExistsException(configMapName, source, destination);

        // Assert
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void GenericConstructor_ShouldSetMessage()
    {
        // Arrange
        var configMap = new MockOltAdapterMapConfig();
        var expectedMessage = $"{OltAdapterExtensions.BuildBeforeMapName<MockSource, MockDestination>()} already exists for {typeof(MockSource).FullName} -> {typeof(MockDestination).FullName}";

        // Act
        var exception = new OltAdapterMapConfigExistsException<MockSource, MockDestination>(configMap);

        // Assert
        Assert.Equal(expectedMessage, exception.Message);
    }
}

