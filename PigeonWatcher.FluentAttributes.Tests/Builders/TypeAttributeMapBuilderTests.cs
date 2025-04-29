using PigeonWatcher.FluentAttributes.Builders;
using PigeonWatcher.FluentAttributes.Utilities;
using System;
using System.Linq.Expressions;
using System.Reflection;
using Xunit;

namespace PigeonWatcher.FluentAttributes.Tests.Builders;

public class TypeAttributeMapBuilderTests
{
    [Fact]
    public void IncludePredefinedAttributes_ShouldSetFlag()
    {
        // Arrange
        TypeAttributeMapBuilder<TestClass> builder = new();

        // Act
        builder.IncludePredefinedAttributes();

        // Assert
        Assert.True(builder.IncludePredefinedAttributesFlag);
    }

    [Fact]
    public void WithAttribute_ShouldAddAttribute()
    {
        // Arrange
        TypeAttributeMapBuilder<TestClass> builder = new();
        ObsoleteAttribute attribute = new();

        // Act
        builder.WithAttribute(attribute);

        // Assert
        Assert.Contains(attribute, builder.Attributes!);
    }

    [Fact]
    public void WithAttribute_Generic_ShouldAddAndConfigureAttribute()
    {
        // Arrange
        TypeAttributeMapBuilder<TestClass> builder = new();

        // Act
        builder.WithAttribute<ObsoleteAttribute>(attr => attr.UrlFormat = "Test");

        // Assert
        Attribute attribute = Assert.Single(builder.Attributes!);
        Assert.IsType<ObsoleteAttribute>(attribute);
        Assert.Equal("Test", ((ObsoleteAttribute)attribute).UrlFormat);
    }

    [Fact]
    public void Property_ShouldReturnPropertyAttributeMapBuilder()
    {
        // Arrange
        TypeAttributeMapBuilder<TestClass> builder = new();

        // Act
        PropertyAttributeMapBuilder propertyBuilder = builder.Property(x => x.Property);

        // Assert
        Assert.NotNull(propertyBuilder);
        Assert.IsType<PropertyAttributeMapBuilder>(propertyBuilder);
    }

    [Fact]
    public void Property_ShouldThrowIfNotProperty()
    {
        // Arrange
        TypeAttributeMapBuilder<TestClass> builder = new();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => builder.Property(x => x.Field));
    }

    [Fact]
    public void Field_ShouldReturnFieldAttributeMapBuilder()
    {
        // Arrange
        TypeAttributeMapBuilder<TestClass> builder = new();

        // Act
        FieldAttributeMapBuilder fieldBuilder = builder.Field(x => x.Field);

        // Assert
        Assert.NotNull(fieldBuilder);
        Assert.IsType<FieldAttributeMapBuilder>(fieldBuilder);
    }

    [Fact]
    public void Field_ShouldThrowIfNotField()
    {
        // Arrange
        TypeAttributeMapBuilder<TestClass> builder = new();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => builder.Field(x => x.Property));
    }

    [Fact]
    public void Method_ShouldReturnMethodAttributeMapBuilder()
    {
        // Arrange
        TypeAttributeMapBuilder<TestClass> builder = new();

        // Act
        MethodAttributeMapBuilder methodBuilder = builder.Method(x => x.Method);

        // Assert
        Assert.NotNull(methodBuilder);
        Assert.IsType<MethodAttributeMapBuilder>(methodBuilder);
    }

    [Fact]
    public void Method_ShouldThrowIfNotMethod()
    {
        // Arrange
        TypeAttributeMapBuilder<TestClass> builder = new();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => builder.Method(x => () => true));
    }

    [Fact]
    public void Build_ShouldReturnTypeAttributeMap()
    {
        // Arrange
        TypeAttributeMapBuilder<TestClass> builder = new();
        builder.WithAttribute(new ObsoleteAttribute());
        builder.Property(x => x.Property).WithAttribute(new ObsoleteAttribute());
        builder.Field(x => x.Field).WithAttribute(new ObsoleteAttribute());
        builder.Method(x => x.Method).WithAttribute(new ObsoleteAttribute());

        // Act
        TypeAttributeMap<TestClass> typeAttributeMap = builder.Build();

        // Assert
        Assert.NotNull(typeAttributeMap);
        Assert.IsType<TypeAttributeMap<TestClass>>(typeAttributeMap);
        Assert.True(typeAttributeMap.HasAttribute<ObsoleteAttribute>());
        Assert.Equal(3, typeAttributeMap.MemberAttributeMaps.Count());
        Assert.True(typeAttributeMap.Get<PropertyAttributeMap>(x => x.Property).HasAttribute<ObsoleteAttribute>());
        Assert.True(typeAttributeMap.Get<FieldAttributeMap>(x => x.Field).HasAttribute<ObsoleteAttribute>());
        Assert.True(typeAttributeMap.Get<MethodAttributeMap>(x => x.Method).HasAttribute<ObsoleteAttribute>());
    }

    private class TestClass
    {
        public int Field;
        public string Property { get; } = string.Empty;

        public void Method()
        {
        }
    }
}