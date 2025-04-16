using PigeonWatcher.FluentAttributes.Builders;
using System;
using System.Linq.Expressions;
using System.Reflection;
using Xunit;

namespace PigeonWatcher.FluentAttributes.Tests.Builders
{
    public class TypeAttributeMapBuilderTests
    {
        private class TestAttribute : Attribute { }

        [TestAttribute]
        private class TestClass
        {
            [TestAttribute]
            public string TestProperty { get; set; } = string.Empty;

            public int AnotherProperty { get; set; }
        }

        [Fact]
        public void Property_ShouldReturnPropertyAttributeMapBuilder()
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
        public void Property_ShouldReuseExistingPropertyAttributeMapBuilder()
        {
            // Arrange
            var builder = new TypeAttributeMapBuilder<TestClass>();
            var firstBuilder = builder.Property(x => x.TestProperty);

            // Act
            var secondBuilder = builder.Property(x => x.TestProperty);

            // Assert
            Assert.Same(firstBuilder, secondBuilder);
        }

        [Fact]
        public void Build_ShouldIncludeAttributes_WhenAttributesAreSet()
        {
            // Arrange
            var builder = new TypeAttributeMapBuilder<TestClass>();
            var testAttribute = new TestAttribute();
            builder.WithAttribute(testAttribute);

            // Act
            var result = builder.Build();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(typeof(TestClass), result.Type);
            Assert.True(result.HasAttribute<TestAttribute>());
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
            Assert.NotNull(result);
            Assert.Equal(typeof(TestClass), result.Type);
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
            Assert.NotNull(result);
            Assert.Equal(typeof(TestClass), result.Type);
            Assert.False(result.HasAttribute<TestAttribute>());
        }

        [Fact]
        public void Build_ShouldIncludePropertyAttributeMaps_WhenPropertiesAreConfigured()
        {
            // Arrange
            var builder = new TypeAttributeMapBuilder<TestClass>();
            builder.Property(x => x.TestProperty).WithAttribute(new TestAttribute());

            // Act
            var result = builder.Build();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.TryGetPropertyAttributeMap(nameof(TestClass.TestProperty), out var propertyMap));
            Assert.NotNull(propertyMap);
            Assert.True(propertyMap.HasAttribute<TestAttribute>());
        }

        [Fact]
        public void GetPropertyName_ShouldReturnCorrectPropertyName()
        {
            // Arrange
            Expression<Func<TestClass, object?>> expression = x => x.TestProperty;

            // Act
            var propertyName = typeof(TypeAttributeMapBuilder<TestClass>)
                .GetMethod("GetPropertyName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.Invoke(null, new object[] { expression });

            // Assert
            Assert.Equal(nameof(TestClass.TestProperty), propertyName);
        }

        [Fact]
        public void GetPropertyName_ShouldThrowException_ForInvalidExpression()
        {
            // Arrange
            Expression<Func<TestClass, object?>> expression = x => x.GetHashCode();

            // Act & Assert

            var exception = Assert.Throws<TargetInvocationException>(() =>
                typeof(TypeAttributeMapBuilder<TestClass>)
                    .GetMethod("GetPropertyName", BindingFlags.NonPublic | BindingFlags.Static)
                    ?.Invoke(null, new object[] { expression })
            );

            var innerException = Assert.IsType<InvalidOperationException>(exception.InnerException);
            Assert.Equal("Invalid property selector expression", innerException.Message);
        }

        [Fact]
        public void GetPropertyInfo_ShouldReturnCorrectPropertyInfo()
        {
            // Arrange
            Expression<Func<TestClass, object?>> expression = x => x.TestProperty;

            // Act
            var propertyInfo = typeof(TypeAttributeMapBuilder<TestClass>)
                .GetMethod("GetPropertyInfo", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.Invoke(null, new object[] { expression });

            // Assert
            Assert.NotNull(propertyInfo);
            Assert.Equal(nameof(TestClass.TestProperty), ((System.Reflection.PropertyInfo)propertyInfo!).Name);
        }

        [Fact]
        public void GetPropertyInfo_ShouldThrowException_ForInvalidExpression()
        {
            // Arrange
            Expression<Func<TestClass, object?>> expression = x => x.GetHashCode();

            // Act & Assert
            var exception = Assert.Throws<TargetInvocationException>(() =>
                typeof(TypeAttributeMapBuilder<TestClass>)
                    .GetMethod("GetPropertyInfo", BindingFlags.NonPublic | BindingFlags.Static)
                    ?.Invoke(null, new object[] { expression })
            );

            var innerException = Assert.IsType<InvalidOperationException>(exception.InnerException);
            Assert.Equal("Invalid property selector expression", innerException.Message);
        }
    }
}
