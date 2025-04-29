using PigeonWatcher.FluentAttributes.Builders;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace PigeonWatcher.FluentAttributes.Tests.Builders;

public class PropertyAttributeMapBuilderTests
{
    [Fact]
    public void Build_ShouldReturnPropertyAttributeMap()
    {
        // Arrange
        PropertyInfo propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.TestProperty))!;
        PropertyAttributeMapBuilder builder = new(propertyInfo);

        // Act
        PropertyAttributeMap result = builder.Build();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<PropertyAttributeMap>(result);
        Assert.Equal(propertyInfo, result.PropertyInfo);
    }

    [Fact]
    public void Build_ShouldIncludeAttributesFromProperty()
    {
        // Arrange
        PropertyInfo propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.TestProperty))!;
        PropertyAttributeMapBuilder builder = new(propertyInfo);
        builder.WithAttribute(new TestAttribute());

        // Act
        PropertyAttributeMap result = builder.Build();

        // Assert
        Assert.True(result.HasAttribute<TestAttribute>());
    }

    [Fact]
    public void Build_ShouldIncludePredefinedAttributesFromProperty()
    {
        // Arrange
        PropertyInfo propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.TestProperty))!;
        PropertyAttributeMapBuilder builder = new(propertyInfo);
        builder.IncludePredefinedAttributes();

        // Act
        PropertyAttributeMap result = builder.Build();

        // Assert
        Assert.True(result.HasAttribute<ObsoleteAttribute>());
    }

    [Fact]
    public void Build_ShouldHandlePropertyWithoutAttributes()
    {
        // Arrange
        PropertyInfo propertyInfo = typeof(string).GetProperty(nameof(string.Length))!;
        PropertyAttributeMapBuilder builder = new(propertyInfo);

        // Act
        PropertyAttributeMap result = builder.Build();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Attributes);
    }

    private class TestClass
    {
        [Obsolete] public string TestProperty { get; set; } = string.Empty;
    }

    private class TestAttribute : Attribute;
}