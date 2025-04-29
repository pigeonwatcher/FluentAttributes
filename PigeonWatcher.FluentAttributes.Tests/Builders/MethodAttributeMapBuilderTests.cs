using PigeonWatcher.FluentAttributes.Builders;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace PigeonWatcher.FluentAttributes.Tests.Builders;

public class MethodAttributeMapBuilderTests
{
    [Fact]
    public void Build_ShouldReturnMethodAttributeMap()
    {
        // Arrange
        MethodInfo propertyInfo = typeof(TestClass).GetMethod(nameof(TestClass.Method))!;
        MethodAttributeMapBuilder builder = new(propertyInfo);

        // Act
        MethodAttributeMap result = builder.Build();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<MethodAttributeMap>(result);
        Assert.Equal(propertyInfo, result.MethodInfo);
    }

    [Fact]
    public void Build_ShouldIncludeAttributesFromMethod()
    {
        // Arrange
        MethodInfo propertyInfo = typeof(TestClass).GetMethod(nameof(TestClass.Method))!;
        MethodAttributeMapBuilder builder = new(propertyInfo);
        builder.WithAttribute(new TestAttribute());

        // Act
        MethodAttributeMap result = builder.Build();

        // Assert
        Assert.True(result.HasAttribute<TestAttribute>());
    }

    [Fact]
    public void Build_ShouldIncludePredefinedAttributesFromMethod()
    {
        // Arrange
        MethodInfo propertyInfo = typeof(TestClass).GetMethod(nameof(TestClass.Method))!;
        MethodAttributeMapBuilder builder = new(propertyInfo);
        builder.IncludePredefinedAttributes();

        // Act
        MethodAttributeMap result = builder.Build();

        // Assert
        Assert.True(result.HasAttribute<ObsoleteAttribute>());
    }

    [Fact]
    public void Build_ShouldHandleMethodWithoutAttributes()
    {
        // Arrange
        MethodInfo methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.Method))!;
        MethodAttributeMapBuilder builder = new(methodInfo);

        // Act
        MethodAttributeMap result = builder.Build();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Attributes);
    }

    private class TestClass
    {
        [Obsolete]
        public void Method()
        {
        }
    }

    private class TestAttribute : Attribute;
}