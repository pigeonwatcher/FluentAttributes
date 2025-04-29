using PigeonWatcher.FluentAttributes.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace PigeonWatcher.FluentAttributes.Tests.Builders;

public class SymbolAttributeMapBuilderTests
{
    [Fact]
    public void IncludePredefinedAttributes_SetsFlagCorrectly()
    {
        // Arrange
        TestSymbolAttributeMapBuilder builder = new();

        // Act
        builder.IncludePredefinedAttributes();

        // Assert
        Assert.True(builder.IncludePredefinedAttributesFlag);
    }

    [Fact]
    public void WithAttribute_AddsAttributeToList()
    {
        // Arrange
        TestSymbolAttributeMapBuilder builder = new();
        TestAttribute attribute = new();

        // Act
        builder.WithAttribute(attribute);

        // Assert
        Assert.NotNull(builder.Attributes);
        Assert.Contains(attribute, builder.Attributes!);
    }

    [Fact]
    public void WithAttribute_Generic_AddsAndConfiguresAttribute()
    {
        // Arrange
        TestSymbolAttributeMapBuilder builder = new();

        // Act
        builder.WithAttribute<TestAttribute>(attr => attr.Property = "Configured");

        // Assert
        Assert.NotNull(builder.Attributes);
        TestAttribute? addedAttribute = builder.Attributes!.OfType<TestAttribute>().FirstOrDefault();
        Assert.NotNull(addedAttribute);
        Assert.Equal("Configured", addedAttribute!.Property);
    }

    [Fact]
    public void WithAttribute_Generic_UsesExistingAttributeIfPresent()
    {
        // Arrange
        TestSymbolAttributeMapBuilder builder = new();
        TestAttribute existingAttribute = new() { Property = "Existing" };
        builder.WithAttribute(existingAttribute);

        // Act
        builder.WithAttribute<TestAttribute>(attr => attr.Property = "Updated");

        // Assert
        Assert.Single(builder.Attributes);
        TestAttribute? updatedAttribute = builder.Attributes!.OfType<TestAttribute>().FirstOrDefault();
        Assert.NotNull(updatedAttribute);
        Assert.Equal("Updated", updatedAttribute!.Property);
    }

    private class TestAttribute : Attribute
    {
        public string? Property { get; set; }
    }

    private class TestSymbolAttributeMapBuilder : SymbolAttributeMapBuilder
    {
        public override SymbolAttributeMap Build()
        {
            return new MockSymbolAttributeMap();
        }

        private class MockSymbolAttributeMap : SymbolAttributeMap;
    }
}