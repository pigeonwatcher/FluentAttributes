using System;
using System.Collections.Generic;
using Xunit;

namespace PigeonWatcher.FluentAttributes.Tests
{
    public class TypeAttributeMapContainerTests
    {
        private class TestTypeAttributeMap : TypeAttributeMap
        {
            public TestTypeAttributeMap(Type type) : base(type) { }
        }

        private class TestClass { }

        private class AnotherTestClass { }

        [Fact]
        public void Add_ShouldAddTypeAttributeMapSuccessfully()
        {
            // Arrange
            var container = new TypeAttributeMapContainer();
            var typeAttributeMap = new TestTypeAttributeMap(typeof(TestClass));

            // Act
            var result = container.Add(typeAttributeMap);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Add_ShouldReturnFalse_WhenTypeAttributeMapAlreadyExists()
        {
            // Arrange
            var container = new TypeAttributeMapContainer();
            var typeAttributeMap = new TestTypeAttributeMap(typeof(TestClass));
            container.Add(typeAttributeMap);

            // Act
            var result = container.Add(typeAttributeMap);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetAttributeMap_ByType_ShouldReturnTypeAttributeMap_WhenExists()
        {
            // Arrange
            var container = new TypeAttributeMapContainer();
            var typeAttributeMap = new TestTypeAttributeMap(typeof(TestClass));
            container.Add(typeAttributeMap);

            // Act
            var result = container.GetAttributeMap(typeof(TestClass));

            // Assert
            Assert.Equal(typeAttributeMap, result);
        }

        [Fact]
        public void GetAttributeMap_ByType_ShouldThrowKeyNotFoundException_WhenNotExists()
        {
            // Arrange
            var container = new TypeAttributeMapContainer();

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => container.GetAttributeMap(typeof(TestClass)));
        }

        [Fact]
        public void GetAttributeMap_ByGenericType_ShouldReturnTypeAttributeMap_WhenExists()
        {
            // Arrange
            var container = new TypeAttributeMapContainer();
            var typeAttributeMap = new TypeAttributeMap<TestClass>();
            container.Add(typeAttributeMap);

            // Act
            var result = container.GetAttributeMap<TestClass>();

            // Assert
            Assert.Equal(typeAttributeMap, result);
        }

        [Fact]
        public void GetAttributeMap_ByGenericType_ShouldThrowKeyNotFoundException_WhenNotExists()
        {
            // Arrange
            var container = new TypeAttributeMapContainer();

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => container.GetAttributeMap<TestClass>());
        }

        [Fact]
        public void TryGetAttributeMap_ByType_ShouldReturnTrueAndTypeAttributeMap_WhenExists()
        {
            // Arrange
            var container = new TypeAttributeMapContainer();
            var typeAttributeMap = new TestTypeAttributeMap(typeof(TestClass));
            container.Add(typeAttributeMap);

            // Act
            var result = container.TryGetAttributeMap(typeof(TestClass), out var retrievedMap);

            // Assert
            Assert.True(result);
            Assert.Equal(typeAttributeMap, retrievedMap);
        }

        [Fact]
        public void TryGetAttributeMap_ByType_ShouldReturnFalseAndNull_WhenNotExists()
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
        public void TryGetAttributeMap_ByGenericType_ShouldReturnTrueAndTypeAttributeMap_WhenExists()
        {
            // Arrange
            var container = new TypeAttributeMapContainer();
            var typeAttributeMap = new TypeAttributeMap<TestClass>();
            container.Add(typeAttributeMap);

            // Act
            var result = container.TryGetAttributeMap<TestClass>(out var retrievedMap);

            // Assert
            Assert.True(result);
            Assert.Equal(typeAttributeMap, retrievedMap);
        }

        [Fact]
        public void TryGetAttributeMap_ByGenericType_ShouldReturnFalseAndNull_WhenNotExists()
        {
            // Arrange
            var container = new TypeAttributeMapContainer();

            // Act
            var result = container.TryGetAttributeMap<TestClass>(out var retrievedMap);

            // Assert
            Assert.False(result);
            Assert.Null(retrievedMap);
        }

        [Fact]
        public void GetEnumerator_ShouldEnumerateAllTypeAttributeMaps()
        {
            // Arrange
            var container = new TypeAttributeMapContainer();
            var map1 = new TestTypeAttributeMap(typeof(TestClass));
            var map2 = new TestTypeAttributeMap(typeof(AnotherTestClass));
            container.Add(map1);
            container.Add(map2);

            // Act
            var enumeratedMaps = new List<TypeAttributeMap>();
            foreach (var map in container)
            {
                enumeratedMaps.Add(map);
            }

            // Assert
            Assert.Contains(map1, enumeratedMaps);
            Assert.Contains(map2, enumeratedMaps);
        }
    }
}
