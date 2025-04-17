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

        private class AnotherTestAttribute : Attribute { }

        private class TestSymbolAttributeMapBuilder : SymbolAttributeMapBuilder<TestSymbolAttributeMapBuilder> { }

        [Fact]
        public void IncludePredefinedAttributes_ShouldSetFlagCorrectly()
        {
            // Arrange
            var builder = new TestSymbolAttributeMapBuilder();

            // Act
            builder.IncludePredefinedAttributes(true);

            // Assert
            Assert.True(builder.IncludePredefinedAttributesFlag);

            // Act
            builder.IncludePredefinedAttributes(false);

            // Assert
            Assert.False(builder.IncludePredefinedAttributesFlag);
        }

        [Fact]
        public void WithAttribute_ShouldAddAttributeSuccessfully()
        {
            // Arrange
            var builder = new TestSymbolAttributeMapBuilder();
            var attribute = new TestAttribute();

            // Act
            builder.WithAttribute(attribute);

            // Assert
            Assert.NotNull(builder.Attributes);
            Assert.Contains(attribute, builder.Attributes!);
        }

        [Fact]
        public void WithAttribute_ShouldAddMultipleAttributes()
        {
            // Arrange
            var builder = new TestSymbolAttributeMapBuilder();
            var attribute1 = new TestAttribute();
            var attribute2 = new AnotherTestAttribute();

            // Act
            builder.WithAttribute(attribute1);
            builder.WithAttribute(attribute2);

            // Assert
            Assert.NotNull(builder.Attributes);
            Assert.Contains(attribute1, builder.Attributes!);
            Assert.Contains(attribute2, builder.Attributes!);
        }

        [Fact]
        public void WithAttribute_Generic_ShouldAddAndConfigureAttribute_WhenNotExists()
        {
            // Arrange
            var builder = new TestSymbolAttributeMapBuilder();

            // Act
            builder.WithAttribute<TestAttribute>(attr => attr.Property = "TestValue");

            // Assert
            Assert.NotNull(builder.Attributes);
            var attribute = Assert.Single(builder.Attributes!);
            Assert.IsType<TestAttribute>(attribute);
            Assert.Equal("TestValue", ((TestAttribute)attribute).Property);
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
            var attribute = Assert.Single(builder.Attributes!);
            Assert.IsType<TestAttribute>(attribute);
            Assert.Equal("UpdatedValue", ((TestAttribute)attribute).Property);
        }

        [Fact]
        public void WithAttribute_Generic_ShouldAddMultipleAttributesOfDifferentTypes()
        {
            // Arrange
            var builder = new TestSymbolAttributeMapBuilder();

            // Act
            builder.WithAttribute<TestAttribute>(attr => attr.Property = "TestValue");
            builder.WithAttribute<AnotherTestAttribute>(_ => { });

            // Assert
            Assert.NotNull(builder.Attributes);
            Assert.Equal(2, builder.Attributes!.Count);
            Assert.Contains(builder.Attributes!, attr => attr is TestAttribute);
            Assert.Contains(builder.Attributes!, attr => attr is AnotherTestAttribute);
        }
    }
}
