using System;
using System.Reflection;
using Xunit;

namespace PigeonWatcher.FluentAttributes.Tests
{
    public class PropertyAttributeMapTests
    {
        private class TestClass
        {
            public int TestProperty { get; set; }
        }

        [Fact]
        public void PropertyInfo_ShouldBeSetCorrectly()
        {
            // Arrange
            var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.TestProperty));
            var map = new PropertyAttributeMap
            {
                PropertyInfo = propertyInfo!
            };

            // Act
            var result = map.PropertyInfo;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(propertyInfo, result);
        }

        [Fact]
        public void PropertyType_ShouldReturnCorrectType()
        {
            // Arrange
            var propertyInfo = typeof(TestClass).GetProperty(nameof(TestClass.TestProperty));
            var map = new PropertyAttributeMap
            {
                PropertyInfo = propertyInfo!
            };

            // Act
            var result = map.PropertyType;

            // Assert
            Assert.Equal(typeof(int), result);
        }
    }
}
