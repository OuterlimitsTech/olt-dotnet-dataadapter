using OLT.Core;

namespace OLT.DataAdapters.Tests;

public class OltAdapterExceptionTests
{
    [Fact]
    public void Constructor_ShouldSetMessage()
    {
        // Arrange
        var expectedMessage = "Test exception message";

        // Act
        var exception = new OltAdapterException(expectedMessage);

        // Assert
        Assert.Equal(expectedMessage, exception.Message);
    }
}

