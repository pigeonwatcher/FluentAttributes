using System;
using System.Collections.Generic;
using Xunit;

namespace PigeonWatcher.FluentAttributes.Tests
{
    public class TypeAttributeMapContainerTests
    {
        private class TestClass { }

        [Fact]
        public void Add_ShouldAddTypeAttributeMapSuccessfully()
        {
            // Arrange
            var container = new TypeAttributeMapContainer();
            var typeAttributeMap = new TypeAttributeMap { Type = typeof(TestClass) };

            // Act
            var result = container.Add(typeAttributeMap);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Add_ShouldReturnFalseIfTypeAlreadyExists()
        {
            // Arrange
            var container = new TypeAttributeMapContainer();
            var typeAttributeMap = new TypeAttributeMap { Type = typeof(TestClass) };
            container.Add(typeAttributeMap);

            // Act
            var result = container.Add(typeAttributeMap);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void TryGetAttributeMap_ShouldReturnTrueIfTypeExists()
        {
            // Arrange
            var container = new TypeAttributeMapContainer();
            var typeAttributeMap = new TypeAttributeMap { Type = typeof(TestClass) };
            container.Add(typeAttributeMap);

            // Act
            var result = container.TryGetAttributeMap(typeof(TestClass), out var retrievedMap);

            // Assert
            Assert.True(result);
            Assert.NotNull(retrievedMap);
            Assert.Equal(typeAttributeMap, retrievedMap);
        }

        [Fact]
        public void TryGetAttributeMap_ShouldReturnFalseIfTypeDoesNotExist()
        {
            // Arrange
            var container = new TypeAttributeMapContainer();

            // Act
            var result = container.TryGetAttributeMap(typeof(TestClass), out var retrievedMap);

            // Assert
            Assert.False(result);
            Assert.Null(retrievedMap);
        }

        [Fact]
        public void GetEnumerator_ShouldEnumerateAllTypeAttributeMaps()
        {
            // Arrange
            var container = new TypeAttributeMapContainer();
            var typeAttributeMap1 = new TypeAttributeMap { Type = typeof(TestClass) };
            var typeAttributeMap2 = new TypeAttributeMap { Type = typeof(string) };
            container.Add(typeAttributeMap1);
            container.Add(typeAttributeMap2);

            // Act
            var enumeratedMaps = new List<TypeAttributeMap>();
            foreach (var map in container)
            {
                enumeratedMaps.Add(map);
            }

            // Assert
            Assert.Contains(typeAttributeMap1, enumeratedMaps);
            Assert.Contains(typeAttributeMap2, enumeratedMaps);
        }
    }
}
