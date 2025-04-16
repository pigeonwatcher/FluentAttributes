using System;
using Xunit;

namespace PigeonWatcher.FluentAttributes.Tests
{
    public class SymbolAttributeMapTests
    {
        private class TestSymbolAttributeMap : SymbolAttributeMap
        {
            // A concrete implementation of the abstract class for testing purposes.
        }

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
        public void AddAttribute_ShouldReturnFalseIfAttributeAlreadyExists()
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
        public void HasAttribute_ShouldReturnTrueIfAttributeExists()
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
        public void HasAttribute_ShouldReturnFalseIfAttributeDoesNotExist()
        {
            // Arrange
            var map = new TestSymbolAttributeMap();

            // Act
            var result = map.HasAttribute<TestAttribute>();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void TryGetAttribute_ShouldReturnTrueAndOutAttributeIfExists()
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
        public void TryGetAttribute_ShouldReturnFalseAndNullIfAttributeDoesNotExist()
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
        public void GetAttribute_ShouldReturnAttributeIfExists()
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
        public void GetAttribute_ShouldThrowInvalidOperationExceptionIfAttributeDoesNotExist()
        {
            // Arrange
            var map = new TestSymbolAttributeMap();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => map.GetAttribute<TestAttribute>());
        }
    }
}
