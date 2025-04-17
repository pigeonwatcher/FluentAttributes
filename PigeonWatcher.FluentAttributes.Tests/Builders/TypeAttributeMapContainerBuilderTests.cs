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
        private class TestAttribute : Attribute { }

        private class TestClass { }

        private class AnotherTestClass { }

        private class TestConfiguration : ITypeAttributeMapConfiguration<TestClass>
        {
            public void Configure(TypeAttributeMapBuilder<TestClass> builder)
            {
                builder.WithAttribute<TestAttribute>(_ => { });
            }
        }

        private class AnotherTestConfiguration : ITypeAttributeMapConfiguration<AnotherTestClass>
        {
            public void Configure(TypeAttributeMapBuilder<AnotherTestClass> builder)
            {
                builder.WithAttribute<TestAttribute>(_ => { });
            }
        }

        [Fact]
        public void ApplyConfiguration_ShouldAddConfigurationToContainer()
        {
            // Arrange
            var builder = new TypeAttributeMapContainerBuilder();
            var configuration = new TestConfiguration();

            // Act
            builder.ApplyConfiguration(configuration);
            var container = builder.Build();

            // Assert
            Assert.NotNull(container);
            Assert.True(container.TryGetAttributeMap<TestClass>(out var typeAttributeMap));
            Assert.NotNull(typeAttributeMap);
            Assert.True(typeAttributeMap.HasAttribute<TestAttribute>());
        }

        [Fact]
        public void ApplyConfiguration_ShouldThrowArgumentNullException_WhenConfigurationIsNull()
        {
            // Arrange
            var builder = new TypeAttributeMapContainerBuilder();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => builder.ApplyConfiguration<TestClass>(null!));
        }

        [Fact]
        public void ApplyConfigurationsFromAssembly_ShouldAddAllConfigurationsFromAssembly()
        {
            // Arrange
            var builder = new TypeAttributeMapContainerBuilder();
            var assembly = Assembly.GetExecutingAssembly();

            // Act
            builder.ApplyConfigurationsFromAssembly(assembly);
            var container = builder.Build();

            // Assert
            Assert.NotNull(container);
            Assert.True(container.TryGetAttributeMap<TestClass>(out var testClassMap));
            Assert.NotNull(testClassMap);
            Assert.True(testClassMap.HasAttribute<TestAttribute>());

            Assert.True(container.TryGetAttributeMap<AnotherTestClass>(out var anotherTestClassMap));
            Assert.NotNull(anotherTestClassMap);
            Assert.True(anotherTestClassMap.HasAttribute<TestAttribute>());
        }

        [Fact]
        public void Build_ShouldReturnTypeAttributeMapContainer()
        {
            // Arrange
            var builder = new TypeAttributeMapContainerBuilder();
            var configuration = new TestConfiguration();
            builder.ApplyConfiguration(configuration);

            // Act
            var container = builder.Build();

            // Assert
            Assert.NotNull(container);
            Assert.True(container.TryGetAttributeMap<TestClass>(out var typeAttributeMap));
            Assert.NotNull(typeAttributeMap);
        }

        [Fact]
        public void Build_ShouldResetContainerAfterBuild()
        {
            // Arrange
            var builder = new TypeAttributeMapContainerBuilder();
            var configuration = new TestConfiguration();
            builder.ApplyConfiguration(configuration);

            // Act
            var container1 = builder.Build();
            var container2 = builder.Build();

            // Assert
            Assert.NotNull(container1);
            Assert.Empty(container2); // The second build should return an empty container.
        }
    }
}
