using PigeonWatcher.FluentAttributes.Builders;
using System;
using System.Collections.Generic;
using Xunit;

namespace PigeonWatcher.FluentAttributes.Tests.Builders
{
    public class SymbolAttributeMapBuilderTests
    {
        private class TestAttribute : Attribute
        {
            public string? Property { get; set; }
        }

        private class TestSymbolAttributeMapBuilder : SymbolAttributeMapBuilder<TestSymbolAttributeMapBuilder>
        {
        }

        [Fact]
        public void IncludePredefinedAttributes_ShouldSetFlagCorrectly()
        {
            // Arrange
            var builder = new TestSymbolAttributeMapBuilder();

            // Act
            builder.IncludePredefinedAttributes(true);

            // Assert
            Assert.True(builder.IncludePredefinedAttributesFlag);
        }

        [Fact]
        public void IncludePredefinedAttributes_ShouldUnsetFlagCorrectly()
        {
            // Arrange
            var builder = new TestSymbolAttributeMapBuilder();

            // Act
            builder.IncludePredefinedAttributes(false);

            // Assert
            Assert.False(builder.IncludePredefinedAttributesFlag);
        }

        [Fact]
        public void WithAttribute_ShouldAddAttributeToList()
        {
            // Arrange
            var builder = new TestSymbolAttributeMapBuilder();
            var attribute = new TestAttribute();

            // Act
            builder.WithAttribute(attribute);

            // Assert
            Assert.NotNull(builder.Attributes);
            Assert.Contains(attribute, builder.Attributes);
        }

        [Fact]
        public void WithAttribute_Generic_ShouldAddNewAttributeIfNotExists()
        {
            // Arrange
            var builder = new TestSymbolAttributeMapBuilder();

            // Act
            builder.WithAttribute<TestAttribute>(attr => attr.Property = "TestValue");

            // Assert
            Assert.NotNull(builder.Attributes);
            var addedAttribute = Assert.Single(builder.Attributes);
            Assert.IsType<TestAttribute>(addedAttribute);
            Assert.Equal("TestValue", ((TestAttribute)addedAttribute).Property);
        }

        [Fact]
        public void WithAttribute_Generic_ShouldConfigureExistingAttribute()
        {
            // Arrange
            var builder = new TestSymbolAttributeMapBuilder();
            builder.WithAttribute<TestAttribute>(attr => attr.Property = "InitialValue");

            // Act
            builder.WithAttribute<TestAttribute>(attr => attr.Property = "UpdatedValue");

            // Assert
            Assert.NotNull(builder.Attributes);
            var addedAttribute = Assert.Single(builder.Attributes);
            Assert.IsType<TestAttribute>(addedAttribute);
            Assert.Equal("UpdatedValue", ((TestAttribute)addedAttribute).Property);
        }
    }
}
