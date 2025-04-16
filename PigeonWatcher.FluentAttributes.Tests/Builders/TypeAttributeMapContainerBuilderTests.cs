using PigeonWatcher.FluentAttributes.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace PigeonWatcher.FluentAttributes.Tests.Builders
{
    public class TypeAttributeMapContainerBuilderTests
    {
        private class TestEntity { }

        private class TestAttribute : Attribute { }

        private class TestConfiguration : ITypeAttributeMapConfiguration<TestEntity>
        {
            public void Configure(TypeAttributeMapBuilder<TestEntity> builder)
            {
                builder.WithAttribute(new TestAttribute());
            }
        }

        [Fact]
        public void ApplyConfigurationsFromAssembly_ShouldAddConfigurationsToContainer()
        {
            // Arrange
            var builder = new TypeAttributeMapContainerBuilder();
            var assembly = Assembly.GetExecutingAssembly();

            // Act
            builder.ApplyConfigurationsFromAssembly(assembly);
            var container = builder.Build();

            // Assert
            Assert.NotNull(container);
            Assert.True(container.TryGetAttributeMap(typeof(TestEntity), out var typeAttributeMap));
            Assert.NotNull(typeAttributeMap);
            Assert.True(typeAttributeMap.HasAttribute<TestAttribute>());
        }

        [Fact]
        public void ApplyConfigurationsFromAssembly_ShouldHandleEmptyAssembly()
        {
            // Arrange
            var builder = new TypeAttributeMapContainerBuilder();
            var emptyAssembly = Assembly.Load("System.Runtime"); // An assembly unlikely to have configurations

            // Act
            builder.ApplyConfigurationsFromAssembly(emptyAssembly);
            var container = builder.Build();

            // Assert
            Assert.NotNull(container);
            Assert.False(container.TryGetAttributeMap(typeof(TestEntity), out _));
        }

        [Fact]
        public void Build_ShouldReturnContainerAndResetBuilder()
        {
            // Arrange
            var builder = new TypeAttributeMapContainerBuilder();
            var assembly = Assembly.GetExecutingAssembly();
            builder.ApplyConfigurationsFromAssembly(assembly);

            // Act
            var container1 = builder.Build();
            var container2 = builder.Build();

            // Assert
            Assert.NotNull(container1);
            Assert.NotNull(container2);
            Assert.NotSame(container1, container2); // Ensure the builder resets after building
        }
    }
}
