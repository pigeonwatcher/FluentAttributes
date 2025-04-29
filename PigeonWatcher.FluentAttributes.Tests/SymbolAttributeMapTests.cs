using System;
using System.Collections.Generic;
using Xunit;

namespace PigeonWatcher.FluentAttributes.Tests;

public class SymbolAttributeMapTests
{
    [Fact]
    public void AddAttribute_ShouldAddAttributeSuccessfully()
    {
        // Arrange
        TestSymbolAttributeMap map = new();
        ObsoleteAttribute attribute = new();

        // Act
        bool result = map.AddAttribute(attribute);

        // Assert
        Assert.True(result);
        Assert.True(map.HasAttribute<ObsoleteAttribute>());
    }

    [Fact]
    public void AddAttribute_ShouldReturnFalseIfAttributeAlreadyExists()
    {
        // Arrange
        TestSymbolAttributeMap map = new();
        ObsoleteAttribute attribute = new();
        map.AddAttribute(attribute);

        // Act
        bool result = map.AddAttribute(attribute);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void HasAttribute_ShouldReturnTrueIfAttributeExists()
    {
        // Arrange
        TestSymbolAttributeMap map = new();
        ObsoleteAttribute attribute = new();
        map.AddAttribute(attribute);

        // Act
        bool result = map.HasAttribute<ObsoleteAttribute>();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasAttribute_ShouldReturnFalseIfAttributeDoesNotExist()
    {
        // Arrange
        TestSymbolAttributeMap map = new();

        // Act
        bool result = map.HasAttribute<ObsoleteAttribute>();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TryGetAttribute_ShouldReturnTrueAndOutAttributeIfExists()
    {
        // Arrange
        TestSymbolAttributeMap map = new();
        ObsoleteAttribute attribute = new();
        map.AddAttribute(attribute);

        // Act
        bool result = map.TryGetAttribute(out ObsoleteAttribute? foundAttribute);

        // Assert
        Assert.True(result);
        Assert.NotNull(foundAttribute);
        Assert.IsType<ObsoleteAttribute>(foundAttribute);
    }

    [Fact]
    public void TryGetAttribute_ShouldReturnFalseAndNullIfAttributeDoesNotExist()
    {
        // Arrange
        TestSymbolAttributeMap map = new();

        // Act
        bool result = map.TryGetAttribute(out ObsoleteAttribute? foundAttribute);

        // Assert
        Assert.False(result);
        Assert.Null(foundAttribute);
    }

    [Fact]
    public void GetAttribute_ShouldReturnAttributeIfExists()
    {
        // Arrange
        TestSymbolAttributeMap map = new();
        ObsoleteAttribute attribute = new();
        map.AddAttribute(attribute);

        // Act
        ObsoleteAttribute foundAttribute = map.GetAttribute<ObsoleteAttribute>();

        // Assert
        Assert.NotNull(foundAttribute);
        Assert.IsType<ObsoleteAttribute>(foundAttribute);
    }

    [Fact]
    public void GetAttribute_ShouldThrowInvalidOperationExceptionIfAttributeDoesNotExist()
    {
        // Arrange
        TestSymbolAttributeMap map = new();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => map.GetAttribute<ObsoleteAttribute>());
    }

    private class TestSymbolAttributeMap : SymbolAttributeMap;
}