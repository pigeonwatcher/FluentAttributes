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
        }

        private PropertyInfo GetTestPropertyInfo(string propertyName)
        {
            return typeof(TestClass).GetProperty(propertyName)
                ?? throw new InvalidOperationException($"Property '{propertyName}' not found.");
        }

        private class TestClass
        {
            [TestAttribute]
            public string TestProperty { get; set; } = string.Empty;
        }

        [Fact]
        public void Build_ShouldIncludeAttributes_WhenAttributesAreSet()
        {
            // Arrange
            var propertyInfo = GetTestPropertyInfo(nameof(TestClass.TestProperty));
            var builder = new PropertyAttributeMapBuilder(propertyInfo);
            var testAttribute = new TestAttribute();
            builder.WithAttribute(testAttribute);

            // Act
            var result = builder.Build();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(propertyInfo, result.PropertyInfo);
            Assert.True(result.HasAttribute<TestAttribute>());
        }

        [Fact]
        public void Build_ShouldIncludePredefinedAttributes_WhenFlagIsSet()
        {
            // Arrange
            var propertyInfo = GetTestPropertyInfo(nameof(TestClass.TestProperty));
            var builder = new PropertyAttributeMapBuilder(propertyInfo);
            builder.IncludePredefinedAttributes(true);

            // Act
            var result = builder.Build();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(propertyInfo, result.PropertyInfo);
            Assert.True(result.HasAttribute<TestAttribute>());
        }

        [Fact]
        public void Build_ShouldNotIncludePredefinedAttributes_WhenFlagIsNotSet()
        {
            // Arrange
            var propertyInfo = GetTestPropertyInfo(nameof(TestClass.TestProperty));
            var builder = new PropertyAttributeMapBuilder(propertyInfo);

            // Act
            var result = builder.Build();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(propertyInfo, result.PropertyInfo);
            Assert.False(result.HasAttribute<TestAttribute>());
        }
    }
}
