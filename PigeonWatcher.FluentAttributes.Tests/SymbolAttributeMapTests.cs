using System;
using System.Collections.Generic;
using Xunit;

namespace PigeonWatcher.FluentAttributes.Tests
{
    public class SymbolAttributeMapTests
    {
        private class TestSymbolAttributeMap : SymbolAttributeMap { }

        private class TestAttribute : Attribute { }

        private class AnotherTestAttribute : Attribute { }

        [Fact]
        public void AddAttribute_ShouldAddAttributeSuccessfully()
        {
            // Arrange
            var map = new TestSymbolAttributeMap();
            var attribute = new TestAttribute();

            // Act
            var result = map.AddAttribute(attribute);

            // Assert
            Assert.True(result);
            Assert.True(map.HasAttribute<TestAttribute>());
        }

        [Fact]
        public void AddAttribute_ShouldReturnFalse_WhenAttributeAlreadyExists()
        {
            // Arrange
            var map = new TestSymbolAttributeMap();
            var attribute = new TestAttribute();
            map.AddAttribute(attribute);

            // Act
            var result = map.AddAttribute(attribute);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void HasAttribute_ShouldReturnTrue_WhenAttributeExists()
        {
            // Arrange
            var map = new TestSymbolAttributeMap();
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
            var map = new TestSymbolAttributeMap();

            // Act
            var result = map.HasAttribute<TestAttribute>();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void TryGetAttribute_ShouldReturnTrueAndAttribute_WhenAttributeExists()
        {
            // Arrange
            var map = new TestSymbolAttributeMap();
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
            var map = new TestSymbolAttributeMap();

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
            var map = new TestSymbolAttributeMap();
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
            var map = new TestSymbolAttributeMap();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => map.GetAttribute<TestAttribute>());
        }
    }
}
