using PigeonWatcher.FluentAttributes.Builders;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace PigeonWatcher.FluentAttributes.Tests.Builders
{
    public class PropertyAttributeMapBuilderTests
    {
        private class TestAttribute : Attribute
        {
            public string? Property { get; set; }
        }

        private class AnotherTestAttribute : Attribute { }

        private class TestClass
        {
            [Test]
            public string TestProperty { get; set; } = string.Empty;
        }

        [Fact]
        public void Build_ShouldCreatePropertyAttributeMap_WithPropertyInfo()
        {
            // Arrange
            var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.TestProperty))!;
            var builder = new PropertyAttributeMapBuilder(propertyInfo);

            // Act
            var result = builder.Build();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(propertyInfo, result.PropertyInfo);
        }

        [Fact]
        public void Build_ShouldIncludeAttributesAddedViaWithAttribute()
        {
            // Arrange
            var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.TestProperty))!;
            var builder = new PropertyAttributeMapBuilder(propertyInfo);
            var attribute = new TestAttribute { Property = "TestValue" };

            // Act
            builder.WithAttribute(attribute);
            var result = builder.Build();

            // Assert
            Assert.True(result.HasAttribute<TestAttribute>());
            var retrievedAttribute = result.GetAttribute<TestAttribute>();
            Assert.Equal("TestValue", retrievedAttribute.Property);
        }

        [Fact]
        public void Build_ShouldIncludePredefinedAttributes_WhenFlagIsSet()
        {
            // Arrange
            var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.TestProperty))!;
            var builder = new PropertyAttributeMapBuilder(propertyInfo);

            // Act
            builder.IncludePredefinedAttributes(true);
            var result = builder.Build();

            // Assert
            Assert.True(result.HasAttribute<TestAttribute>());
        }

        [Fact]
        public void Build_ShouldNotIncludePredefinedAttributes_WhenFlagIsNotSet()
        {
            // Arrange
            var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.TestProperty))!;
            var builder = new PropertyAttributeMapBuilder(propertyInfo);

            // Act
            var result = builder.Build();

            // Assert
            Assert.False(result.HasAttribute<TestAttribute>());
        }

        [Fact]
        public void Build_ShouldIncludeBothAddedAndPredefinedAttributes()
        {
            // Arrange
            var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.TestProperty))!;
            var builder = new PropertyAttributeMapBuilder(propertyInfo);
            var customAttribute = new AnotherTestAttribute();

            // Act
            builder.WithAttribute(customAttribute);
            builder.IncludePredefinedAttributes(true);
            var result = builder.Build();

            // Assert
            Assert.True(result.HasAttribute<TestAttribute>());
            Assert.True(result.HasAttribute<AnotherTestAttribute>());
        }
    }
}
