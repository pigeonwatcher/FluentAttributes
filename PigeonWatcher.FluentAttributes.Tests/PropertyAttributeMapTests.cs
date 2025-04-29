using System;
using System.Reflection;
using Xunit;

namespace PigeonWatcher.FluentAttributes.Tests;

public class PropertyAttributeMapTests
{
    [Fact]
    public void Constructor_ShouldInitializePropertyInfo()
    {
        // Arrange
        PropertyInfo propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.TestProperty))!;

        // Act
        PropertyAttributeMap map = new(propertyInfo);

        // Assert
        Assert.NotNull(map.PropertyInfo);
        Assert.Equal(propertyInfo, map.PropertyInfo);
    }

    [Fact]
    public void PropertyType_ShouldReturnCorrectType()
    {
        // Arrange
        PropertyInfo propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.TestProperty))!;
        PropertyAttributeMap map = new(propertyInfo);

        // Act
        Type propertyType = map.PropertyType;

        // Assert
        Assert.Equal(typeof(int), propertyType);
    }

    private class TestClass
    {
        public int TestProperty { get; set; }
    }
}