using PigeonWatcher.FluentAttributes.Builders;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xunit;

namespace PigeonWatcher.FluentAttributes.Tests.Builders;

public class TypeAttributeMapContainerBuilderTests
{
    [Fact]
    public void ApplyConfiguration_ShouldThrowIfConfigurationIsNull()
    {
        // Arrange
        TypeAttributeMapContainerBuilder builder = new();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.ApplyConfiguration<TestClass>(null!));
    }

    [Fact]
    public void ApplyConfiguration_ShouldApplyConfiguration()
    {
        // Arrange
        TypeAttributeMapContainerBuilder builder = new();
        TestConfiguration configuration = new();

        // Act
        builder.ApplyConfiguration(configuration);

        // Assert
        TypeAttributeMapContainer container = builder.Build();
        TypeAttributeMap<TestClass> typeAttributeMap = container.GetAttributeMap<TestClass>();
        Assert.NotNull(typeAttributeMap);
        Assert.True(typeAttributeMap.HasAttribute<ObsoleteAttribute>());
    }

    [Fact]
    public void ApplyConfigurationsFromAssembly_ShouldApplyAllConfigurations()
    {
        // Arrange
        TypeAttributeMapContainerBuilder builder = new();
        Assembly assembly = Assembly.GetExecutingAssembly();

        // Act
        builder.ApplyConfigurationsFromAssembly(assembly);

        // Assert
        TypeAttributeMapContainer container = builder.Build();
        TypeAttributeMap<TestClass> typeAttributeMap = container.GetAttributeMap<TestClass>();
        Assert.NotNull(typeAttributeMap);
        Assert.True(typeAttributeMap.HasAttribute<ObsoleteAttribute>());
    }

    [Fact]
    public void ApplyConfigurationsFromAssembly_ShouldHandleEmptyAssembly()
    {
        // Arrange
        TypeAttributeMapContainerBuilder builder = new();
        Assembly emptyAssembly = Assembly.Load("System.Runtime");

        // Act
        builder.ApplyConfigurationsFromAssembly(emptyAssembly);

        // Assert
        TypeAttributeMapContainer container = builder.Build();
        Assert.Empty(container);
    }

    [Fact]
    public void Build_ShouldReturnContainer()
    {
        // Arrange
        TypeAttributeMapContainerBuilder builder = new();
        TestConfiguration configuration = new();
        builder.ApplyConfiguration(configuration);

        // Act
        TypeAttributeMapContainer container = builder.Build();

        // Assert
        Assert.NotNull(container);
        Assert.IsType<TypeAttributeMapContainer>(container);
    }

    [Fact]
    public void Build_ShouldResetContainerAfterBuild()
    {
        // Arrange
        TypeAttributeMapContainerBuilder builder = new();
        TestConfiguration configuration = new();
        builder.ApplyConfiguration(configuration);

        // Act
        TypeAttributeMapContainer container1 = builder.Build();
        TypeAttributeMapContainer container2 = builder.Build();

        // Assert
        Assert.NotSame(container1, container2);
        Assert.Empty(container2);
    }

    private class TestConfiguration : ITypeAttributeMapConfiguration<TestClass>
    {
        public void Configure(TypeAttributeMapBuilder<TestClass> builder)
        {
            builder.WithAttribute(new ObsoleteAttribute());
        }
    }

    private class TestClass
    {
    }

    public class ExampleAttributeMapConfiguration : ITypeAttributeMapConfiguration<Example>
    {
        public void Configure(TypeAttributeMapBuilder<Example> builder)
        {
            builder.WithAttribute(new DataContractAttribute());

            builder.Property(x => x.ExampleProperty)
                .WithAttribute(new DataMemberAttribute { Name = "example_property" });

            builder.Field(x => x.ExampleField)
                .WithAttribute(new DataMemberAttribute { Name = "example_field" });
        }
    }

    private class Example
    {
        public string ExampleProperty { get; set; } = string.Empty;
        public int ExampleField;
    }
}