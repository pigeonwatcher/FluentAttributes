using PigeonWatcher.FluentAttributes.Builders;
using PigeonWatcher.FluentAttributes.Utilities;
using System;
using System.Linq.Expressions;
using System.Reflection;
using Xunit;

namespace PigeonWatcher.FluentAttributes.Tests.Builders
{
    public class TypeAttributeMapBuilderTests
    {
        private class TestAttribute : Attribute
        {
            public string? Property { get; set; }
        }

        private class AnotherTestAttribute : Attribute { }

        [Test]
        private class TestClass
        {
            [Test]
            public string TestProperty { get; set; } = string.Empty;

            public int AnotherProperty { get; set; }
        }

        [Fact]
        public void Property_ShouldReturnPropertyAttributeMapBuilder_ForValidProperty()
        {
            // Arrange
            var builder = new TypeAttributeMapBuilder<TestClass>();

            // Act
            var propertyBuilder = builder.Property(x => x.TestProperty);

            // Assert
            Assert.NotNull(propertyBuilder);
            Assert.IsType<PropertyAttributeMapBuilder>(propertyBuilder);
        }

        [Fact]
        public void Property_ShouldThrowInvalidOperationException_ForNonPropertyExpression()
        {
            // Arrange
            var builder = new TypeAttributeMapBuilder<TestClass>();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => builder.Property(x => x.ToString()));
        }

        [Fact]
        public void Property_ShouldReturnSameBuilder_ForSameProperty()
        {
            // Arrange
            var builder = new TypeAttributeMapBuilder<TestClass>();

            // Act
            var propertyBuilder1 = builder.Property(x => x.TestProperty);
            var propertyBuilder2 = builder.Property(x => x.TestProperty);

            // Assert
            Assert.Same(propertyBuilder1, propertyBuilder2);
        }

        [Fact]
        public void Build_ShouldCreateTypeAttributeMap_WithAttributesAndProperties()
        {
            // Arrange
            var builder = new TypeAttributeMapBuilder<TestClass>();
            builder.WithAttribute<TestAttribute>(attr => attr.Property = "TestValue");
            builder.Property(x => x.TestProperty).WithAttribute<AnotherTestAttribute>(_ => { });

            // Act
            var result = builder.Build();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasAttribute<TestAttribute>());
            Assert.True(result.TryGetPropertyAttributeMap(nameof(TestClass.TestProperty), out var propertyMap));
            Assert.NotNull(propertyMap);
            Assert.True(propertyMap.HasAttribute<AnotherTestAttribute>());
        }

        [Fact]
        public void Build_ShouldIncludePredefinedAttributes_WhenFlagIsSet()
        {
            // Arrange
            var builder = new TypeAttributeMapBuilder<TestClass>();
            builder.IncludePredefinedAttributes(true);

            // Act
            var result = builder.Build();

            // Assert
            Assert.True(result.HasAttribute<TestAttribute>());
        }

        [Fact]
        public void Build_ShouldNotIncludePredefinedAttributes_WhenFlagIsNotSet()
        {
            // Arrange
            var builder = new TypeAttributeMapBuilder<TestClass>();

            // Act
            var result = builder.Build();

            // Assert
            Assert.False(result.HasAttribute<TestAttribute>());
        }

        [Fact]
        public void Build_ShouldIncludeBothAddedAndPredefinedAttributes()
        {
            // Arrange
            var builder = new TypeAttributeMapBuilder<TestClass>();
            builder.WithAttribute<AnotherTestAttribute>(_ => { });
            builder.IncludePredefinedAttributes(true);

            // Act
            var result = builder.Build();

            // Assert
            Assert.True(result.HasAttribute<TestAttribute>());
            Assert.True(result.HasAttribute<AnotherTestAttribute>());
        }

        [Fact]
        public void Build_ShouldIncludePropertyAttributeMaps()
        {
            // Arrange
            var builder = new TypeAttributeMapBuilder<TestClass>();
            builder.Property(x => x.TestProperty).WithAttribute<TestAttribute>(_ => { });

            // Act
            var result = builder.Build();

            // Assert
            Assert.True(result.TryGetPropertyAttributeMap(nameof(TestClass.TestProperty), out var propertyMap));
            Assert.NotNull(propertyMap);
            Assert.True(propertyMap.HasAttribute<TestAttribute>());
        }
    }
}
