using System;
using System.Reflection;
using Xunit;

namespace PigeonWatcher.FluentAttributes.Tests
{
    public class PropertyAttributeMapTests
    {
        private class TestAttribute : Attribute { }

        private class TestClass
        {
            [Test]
            public string TestProperty { get; set; } = string.Empty;
        }

        [Fact]
        public void PropertyInfo_ShouldReturnCorrectPropertyInfo()
        {
            // Arrange
            var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.TestProperty))!;
            var map = new PropertyAttributeMap
            {
                PropertyInfo = propertyInfo
            };

            // Act
            var result = map.PropertyInfo;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(propertyInfo, result);
        }

        [Fact]
        public void PropertyType_ShouldReturnCorrectPropertyType()
        {
            // Arrange
            var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.TestProperty))!;
            var map = new PropertyAttributeMap
            {
                PropertyInfo = propertyInfo
            };

            // Act
            var result = map.PropertyType;

            // Assert
            Assert.Equal(typeof(string), result);
        }

        [Fact]
        public void AddAttribute_ShouldAddAttributeSuccessfully()
        {
            // Arrange
            var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.TestProperty))!;
            var map = new PropertyAttributeMap
            {
                PropertyInfo = propertyInfo
            };
            var attribute = new TestAttribute();

            // Act
            var result = map.AddAttribute(attribute);

            // Assert
            Assert.True(result);
            Assert.True(map.HasAttribute<TestAttribute>());
        }

        [Fact]
        public void HasAttribute_ShouldReturnTrue_WhenAttributeExists()
        {
            // Arrange
            var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.TestProperty))!;
            var map = new PropertyAttributeMap
            {
                PropertyInfo = propertyInfo
            };
            var attribute = new TestAttribute();
            map.AddAttribute(attribute);

            // Act
            var result = map.HasAttribute<TestAttribute>();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void HasAttribute_ShouldReturnFalse_WhenAttributeDoesNotExist()
        {
            // Arrange
            var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.TestProperty))!;
            var map = new PropertyAttributeMap
            {
                PropertyInfo = propertyInfo
            };

            // Act
            var result = map.HasAttribute<TestAttribute>();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void TryGetAttribute_ShouldReturnTrueAndAttribute_WhenAttributeExists()
        {
            // Arrange
            var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.TestProperty))!;
            var map = new PropertyAttributeMap
            {
                PropertyInfo = propertyInfo
            };
            var attribute = new TestAttribute();
            map.AddAttribute(attribute);

            // Act
            var result = map.TryGetAttribute<TestAttribute>(out var retrievedAttribute);

            // Assert
            Assert.True(result);
            Assert.NotNull(retrievedAttribute);
            Assert.IsType<TestAttribute>(retrievedAttribute);
        }

        [Fact]
        public void TryGetAttribute_ShouldReturnFalseAndNull_WhenAttributeDoesNotExist()
        {
            // Arrange
            var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.TestProperty))!;
            var map = new PropertyAttributeMap
            {
                PropertyInfo = propertyInfo
            };

            // Act
            var result = map.TryGetAttribute<TestAttribute>(out var retrievedAttribute);

            // Assert
            Assert.False(result);
            Assert.Null(retrievedAttribute);
        }

        [Fact]
        public void GetAttribute_ShouldReturnAttribute_WhenAttributeExists()
        {
            // Arrange
            var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.TestProperty))!;
            var map = new PropertyAttributeMap
            {
                PropertyInfo = propertyInfo
            };
            var attribute = new TestAttribute();
            map.AddAttribute(attribute);

            // Act
            var retrievedAttribute = map.GetAttribute<TestAttribute>();

            // Assert
            Assert.NotNull(retrievedAttribute);
            Assert.IsType<TestAttribute>(retrievedAttribute);
        }

        [Fact]
        public void GetAttribute_ShouldThrowInvalidOperationException_WhenAttributeDoesNotExist()
        {
            // Arrange
            var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.TestProperty))!;
            var map = new PropertyAttributeMap
            {
                PropertyInfo = propertyInfo
            };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => map.GetAttribute<TestAttribute>());
        }
    }
}
