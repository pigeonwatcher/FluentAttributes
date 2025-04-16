using System;
using System.Collections.Generic;
using Xunit;

namespace PigeonWatcher.FluentAttributes.Tests
{
    public class TypeAttributeMapTests
    {
        private class TestClass
        {
            public int TestProperty { get; set; }
        }

        [Fact]
        public void AddPropertyAttributeMap_ShouldAddSuccessfully()
        {
            // Arrange
            var typeAttributeMap = new TypeAttributeMap { Type = typeof(TestClass) };
            var propertyAttributeMap = new PropertyAttributeMap
            {
                PropertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.TestProperty))!
            };

            // Act
            var result = typeAttributeMap.AddPropertyAttributeMap(nameof(TestClass.TestProperty), propertyAttributeMap);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void AddPropertyAttributeMap_ShouldReturnFalseIfAlreadyExists()
        {
            // Arrange
            var typeAttributeMap = new TypeAttributeMap { Type = typeof(TestClass) };
            var propertyAttributeMap = new PropertyAttributeMap
            {
                PropertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.TestProperty))!
            };
            typeAttributeMap.AddPropertyAttributeMap(nameof(TestClass.TestProperty), propertyAttributeMap);

            // Act
            var result = typeAttributeMap.AddPropertyAttributeMap(nameof(TestClass.TestProperty), propertyAttributeMap);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void TryGetPropertyAttributeMap_ShouldReturnTrueIfExists()
        {
            // Arrange
            var typeAttributeMap = new TypeAttributeMap { Type = typeof(TestClass) };
            var propertyAttributeMap = new PropertyAttributeMap
            {
                PropertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.TestProperty))!
            };
            typeAttributeMap.AddPropertyAttributeMap(nameof(TestClass.TestProperty), propertyAttributeMap);

            // Act
            var result = typeAttributeMap.TryGetPropertyAttributeMap(nameof(TestClass.TestProperty), out var retrievedMap);

            // Assert
            Assert.True(result);
            Assert.NotNull(retrievedMap);
            Assert.Equal(propertyAttributeMap, retrievedMap);
        }

        [Fact]
        public void TryGetPropertyAttributeMap_ShouldReturnFalseIfNotExists()
        {
            // Arrange
            var typeAttributeMap = new TypeAttributeMap { Type = typeof(TestClass) };

            // Act
            var result = typeAttributeMap.TryGetPropertyAttributeMap(nameof(TestClass.TestProperty), out var retrievedMap);

            // Assert
            Assert.False(result);
            Assert.Null(retrievedMap);
        }

        [Fact]
        public void GetPropertyAttributeMap_ShouldReturnMapIfExists()
        {
            // Arrange
            var typeAttributeMap = new TypeAttributeMap { Type = typeof(TestClass) };
            var propertyAttributeMap = new PropertyAttributeMap
            {
                PropertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.TestProperty))!
            };
            typeAttributeMap.AddPropertyAttributeMap(nameof(TestClass.TestProperty), propertyAttributeMap);

            // Act
            var result = typeAttributeMap.GetPropertyAttributeMap(nameof(TestClass.TestProperty));

            // Assert
            Assert.NotNull(result);
            Assert.Equal(propertyAttributeMap, result);
        }

        [Fact]
        public void GetPropertyAttributeMap_ShouldThrowIfNotExists()
        {
            // Arrange
            var typeAttributeMap = new TypeAttributeMap { Type = typeof(TestClass) };

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() =>
                typeAttributeMap.GetPropertyAttributeMap(nameof(TestClass.TestProperty)));
        }
    }
}
