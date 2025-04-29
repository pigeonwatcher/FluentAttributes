using PigeonWatcher.FluentAttributes.Builders;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace PigeonWatcher.FluentAttributes.Tests.Builders;

public class FieldAttributeMapBuilderTests
{
    [Fact]
    public void Build_ShouldReturnFieldAttributeMap()
    {
        // Arrange
        FieldInfo fieldInfo = typeof(TestClass).GetField(nameof(TestClass.TestField))!;
        FieldAttributeMapBuilder builder = new(fieldInfo);

        // Act
        FieldAttributeMap result = builder.Build();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<FieldAttributeMap>(result);
        Assert.Equal(fieldInfo, result.FieldInfo);
    }

    [Fact]
    public void Build_ShouldIncludeAttributesFromField()
    {
        // Arrange
        FieldInfo fieldInfo = typeof(TestClass).GetField(nameof(TestClass.TestField))!;
        FieldAttributeMapBuilder builder = new(fieldInfo);
        builder.WithAttribute(new TestAttribute());

        // Act
        FieldAttributeMap result = builder.Build();

        // Assert
        Assert.True(result.HasAttribute<TestAttribute>());
    }

    [Fact]
    public void Build_ShouldIncludePredefinedAttributesFromField()
    {
        // Arrange
        FieldInfo fieldInfo = typeof(TestClass).GetField(nameof(TestClass.TestField))!;
        FieldAttributeMapBuilder builder = new(fieldInfo);
        builder.IncludePredefinedAttributes();

        // Act
        FieldAttributeMap result = builder.Build();

        // Assert
        Assert.True(result.HasAttribute<ObsoleteAttribute>());
    }

    [Fact]
    public void Build_ShouldHandleFieldWithoutAttributes()
    {
        // Arrange
        FieldInfo fieldInfo = typeof(TestClass).GetField(nameof(TestClass.TestField))!;
        FieldAttributeMapBuilder builder = new(fieldInfo);

        // Act
        FieldAttributeMap result = builder.Build();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Attributes);
    }

    private class TestClass
    {
        [Obsolete] public string TestField = string.Empty;
    }

    private class TestAttribute : Attribute;
}