using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;

namespace PigeonWatcher.FluentAttributes.Tests
{
    public class TypeAttributeMapTests
    {
        private class TestAttribute : Attribute { }

        private class AnotherTestAttribute : Attribute { }

        private class TestClass
        {
            public string TestProperty { get; set; } = string.Empty;
        }

        private class TestTypeAttributeMap : TypeAttributeMap
        {
            public TestTypeAttributeMap(Type type) : base(type) { }
        }

        [Fact]
        public void Type_ShouldReturnCorrectType()
        {
            // Arrange
            var type = typeof(TestClass);
            var map = new TestTypeAttributeMap(type);

            // Act
            var result = map.Type;

            // Assert
            Assert.Equal(type, result);
        }

        [Fact]
        public void AddPropertyAttributeMap_ShouldAddSuccessfully()
        {
            // Arrange
            var map = new TestTypeAttributeMap(typeof(TestClass));
            var propertyMap = new PropertyAttributeMap
            {
                PropertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.TestProperty))!
            };

            // Act
            var result = map.AddPropertyAttributeMap(nameof(TestClass.TestProperty), propertyMap);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void AddPropertyAttributeMap_ShouldReturnFalse_WhenPropertyAlreadyExists()
        {
            // Arrange
            var map = new TestTypeAttributeMap(typeof(TestClass));
            var propertyMap = new PropertyAttributeMap
            {
                PropertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.TestProperty))!
            };
            map.AddPropertyAttributeMap(nameof(TestClass.TestProperty), propertyMap);

            // Act
            var result = map.AddPropertyAttributeMap(nameof(TestClass.TestProperty), propertyMap);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetPropertyAttributeMap_ShouldReturnPropertyMap_WhenExists()
        {
            // Arrange
            var map = new TestTypeAttributeMap(typeof(TestClass));
            var propertyMap = new PropertyAttributeMap
            {
                PropertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.TestProperty))!
            };
            map.AddPropertyAttributeMap(nameof(TestClass.TestProperty), propertyMap);

            // Act
            var result = map.GetPropertyAttributeMap(nameof(TestClass.TestProperty));

            // Assert
            Assert.Equal(propertyMap, result);
        }

        [Fact]
        public void GetPropertyAttributeMap_ShouldThrowKeyNotFoundException_WhenPropertyDoesNotExist()
        {
            // Arrange
            var map = new TestTypeAttributeMap(typeof(TestClass));

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => map.GetPropertyAttributeMap(nameof(TestClass.TestProperty)));
        }

        [Fact]
        public void TryGetPropertyAttributeMap_ShouldReturnTrueAndPropertyMap_WhenExists()
        {
            // Arrange
            var map = new TestTypeAttributeMap(typeof(TestClass));
            var propertyMap = new PropertyAttributeMap
            {
                PropertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.TestProperty))!
            };
            map.AddPropertyAttributeMap(nameof(TestClass.TestProperty), propertyMap);

            // Act
            var result = map.TryGetPropertyAttributeMap(nameof(TestClass.TestProperty), out var retrievedPropertyMap);

            // Assert
            Assert.True(result);
            Assert.Equal(propertyMap, retrievedPropertyMap);
        }

        [Fact]
        public void TryGetPropertyAttributeMap_ShouldReturnFalseAndNull_WhenPropertyDoesNotExist()
        {
            // Arrange
            var map = new TestTypeAttributeMap(typeof(TestClass));

            // Act
            var result = map.TryGetPropertyAttributeMap(nameof(TestClass.TestProperty), out var retrievedPropertyMap);

            // Assert
            Assert.False(result);
            Assert.Null(retrievedPropertyMap);
        }
    }

    public class TypeAttributeMapGenericTests
    {
        private class TestAttribute : Attribute { }

        private class TestClass
        {
            public string TestProperty { get; set; } = string.Empty;
        }

        [Fact]
        public void GetPropertyAttributeMap_ShouldReturnPropertyMap_WhenExpressionIsValid()
        {
            // Arrange
            var map = new TypeAttributeMap<TestClass>();
            var propertyMap = new PropertyAttributeMap
            {
                PropertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.TestProperty))!
            };
            map.AddPropertyAttributeMap(nameof(TestClass.TestProperty), propertyMap);

            // Act
            var result = map.GetPropertyAttributeMap(x => x.TestProperty);

            // Assert
            Assert.Equal(propertyMap, result);
        }

        [Fact]
        public void GetPropertyAttributeMap_ShouldThrowInvalidOperationException_WhenExpressionIsNotProperty()
        {
            // Arrange
            var map = new TypeAttributeMap<TestClass>();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => map.GetPropertyAttributeMap(x => x.ToString()));
        }

        [Fact]
        public void TryGetPropertyAttributeMap_ShouldReturnTrueAndPropertyMap_WhenExpressionIsValid()
        {
            // Arrange
            var map = new TypeAttributeMap<TestClass>();
            var propertyMap = new PropertyAttributeMap
            {
                PropertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.TestProperty))!
            };
            map.AddPropertyAttributeMap(nameof(TestClass.TestProperty), propertyMap);

            // Act
            var result = map.TryGetPropertyAttributeMap(x => x.TestProperty, out var retrievedPropertyMap);

            // Assert
            Assert.True(result);
            Assert.Equal(propertyMap, retrievedPropertyMap);
        }

        [Fact]
        public void TryGetPropertyAttributeMap_ShouldReturnFalseAndNull_WhenPropertyDoesNotExist()
        {
            // Arrange
            var map = new TypeAttributeMap<TestClass>();

            // Act
            var result = map.TryGetPropertyAttributeMap(x => x.TestProperty, out var retrievedPropertyMap);

            // Assert
            Assert.False(result);
            Assert.Null(retrievedPropertyMap);
        }

        [Fact]
        public void TryGetPropertyAttributeMap_ShouldThrowInvalidOperationException_WhenExpressionIsNotProperty()
        {
            // Arrange
            var map = new TypeAttributeMap<TestClass>();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => map.TryGetPropertyAttributeMap(x => x.ToString(), out _));
        }
    }
}
