using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PigeonWatcher.FluentAttributes.Tests;

public class TypeAttributeMapContainerTests
{
    [Fact]
    public void Add_ShouldAddTypeAttributeMap()
    {
        // Arrange
        TypeAttributeMapContainer container = new();
        TestTypeAttributeMap<TestClass> typeAttributeMap = new();

        // Act
        bool result = container.Add(typeAttributeMap);

        // Assert
        Assert.True(result);
        Assert.Contains(typeAttributeMap, container);
    }

    [Fact]
    public void Add_ShouldReturnFalseIfTypeAlreadyExists()
    {
        // Arrange
        TypeAttributeMapContainer container = new();
        TestTypeAttributeMap<TestClass> typeAttributeMap = new();
        container.Add(typeAttributeMap);

        // Act
        bool result = container.Add(typeAttributeMap);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetAttributeMap_ShouldReturnTypeAttributeMapIfExists()
    {
        // Arrange
        TypeAttributeMapContainer container = new();
        TestTypeAttributeMap<TestClass> typeAttributeMap = new();
        container.Add(typeAttributeMap);

        // Act
        TypeAttributeMap retrievedMap = container.GetAttributeMap(typeof(TestClass));

        // Assert
        Assert.NotNull(retrievedMap);
        Assert.Equal(typeAttributeMap, retrievedMap);
    }

    [Fact]
    public void GetAttributeMap_ShouldThrowKeyNotFoundExceptionIfTypeDoesNotExist()
    {
        // Arrange
        TypeAttributeMapContainer container = new();

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => container.GetAttributeMap(typeof(TestClass)));
    }

    [Fact]
    public void GetAttributeMap_Generic_ShouldReturnTypeAttributeMapIfExists()
    {
        // Arrange
        TypeAttributeMapContainer container = new();
        TestTypeAttributeMap<TestClass> typeAttributeMap = new();
        container.Add(typeAttributeMap);

        // Act
        TypeAttributeMap<TestClass> retrievedMap = container.GetAttributeMap<TestClass>();

        // Assert
        Assert.NotNull(retrievedMap);
        Assert.Same(typeAttributeMap, retrievedMap);
    }

    [Fact]
    public void GetAttributeMap_Generic_ShouldThrowKeyNotFoundExceptionIfTypeDoesNotExist()
    {
        // Arrange
        TypeAttributeMapContainer container = new();

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => container.GetAttributeMap<TestClass>());
    }

    [Fact]
    public void TryGetAttributeMap_ShouldReturnTrueAndTypeAttributeMapIfExists()
    {
        // Arrange
        TypeAttributeMapContainer container = new();
        TestTypeAttributeMap<TestClass> typeAttributeMap = new();
        container.Add(typeAttributeMap);

        // Act
        bool result = container.TryGetAttributeMap(typeof(TestClass), out TypeAttributeMap? retrievedMap);

        // Assert
        Assert.True(result);
        Assert.NotNull(retrievedMap);
        Assert.Equal(typeAttributeMap, retrievedMap);
    }

    [Fact]
    public void TryGetAttributeMap_ShouldReturnFalseIfTypeDoesNotExist()
    {
        // Arrange
        TypeAttributeMapContainer container = new();

        // Act
        bool result = container.TryGetAttributeMap(typeof(TestClass), out TypeAttributeMap? retrievedMap);

        // Assert
        Assert.False(result);
        Assert.Null(retrievedMap);
    }

    [Fact]
    public void TryGetAttributeMap_Generic_ShouldReturnTrueAndTypeAttributeMapIfExists()
    {
        // Arrange
        TypeAttributeMapContainer container = new();
        TestTypeAttributeMap<TestClass> typeAttributeMap = new();
        container.Add(typeAttributeMap);

        // Act
        bool result = container.TryGetAttributeMap<TestClass>(out TypeAttributeMap<TestClass>? retrievedMap);

        // Assert
        Assert.True(result);
        Assert.NotNull(retrievedMap);
        Assert.Same(typeAttributeMap, retrievedMap);
    }

    [Fact]
    public void TryGetAttributeMap_Generic_ShouldReturnFalseIfTypeDoesNotExist()
    {
        // Arrange
        TypeAttributeMapContainer container = new();

        // Act
        bool result = container.TryGetAttributeMap<TestClass>(out TypeAttributeMap<TestClass>? retrievedMap);

        // Assert
        Assert.False(result);
        Assert.Null(retrievedMap);
    }

    [Fact]
    public void GetEnumerator_ShouldEnumerateAllTypeAttributeMaps()
    {
        // Arrange
        TypeAttributeMapContainer container = new();
        TestTypeAttributeMap<TestClass> typeAttributeMap1 = new();
        TestTypeAttributeMap<string> typeAttributeMap2 = new();
        container.Add(typeAttributeMap1);
        container.Add(typeAttributeMap2);

        // Act
        List<TypeAttributeMap> enumeratedMaps = container.ToList();

        // Assert
        Assert.Contains(typeAttributeMap1, enumeratedMaps);
        Assert.Contains(typeAttributeMap2, enumeratedMaps);
    }

    private class TestTypeAttributeMap<T> : TypeAttributeMap<T>;

    private class TestClass
    {
    }
}