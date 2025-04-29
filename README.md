# FluentAttributes
An attribute library for .NET that uses a fluent interface for applying attributes to classes, structs, properties, etc.

## Aim

**FluentAttributes** separates attribute metadata from implementation, enabling:
- **Decoupled Metadata**: Manage attributes in configuration classes.
- **Flexibility**: Change, add, or remove attributes without touching the original implementation.
- **Cleaner Code**: Limit the amount of attributes in original implementation.
  
## Get Started

1. **Create a Configuration** Create a class that derives from [ITypeAttributeMapConfiguration](PigeonWatcher.FluentAttributes/ITypeAttributeMapConfiguration.cs) with the generic set to the Type you want to map attributes to.

```csharp
public class ExampleAttributeMapConfiguration : ITypeAttributeMapConfiguration<Example>
{
    public void Configure(TypeAttributeMapBuilder<Example> builder)
    {
        // Type-level attribute
        builder.WithAttribute(new DataContractAttribute());

        // Property-level attribute
        builder
            .Property(x => x.ExampleProperty)
            .WithAttribute(new DataMemberAttribute { Name = "example_property" });

        // Field-level attribute
        builder
            .Field(x => x.ExampleField)
            .WithAttribute(new DataMemberAttribute { Name = "example_field" });
    }
}
```

2. **Build a Container**: Add your configuration to a [TypeMapAttributeContainer](PigeonWatcher.FluentAttributes/TypeAttributeMapContainer.cs).

```csharp
// Applying a single configuration
TypeAttributeMapContainerBuilder container = new()
    .ApplyConfiguration(new ExampleAttributeMapConfiguration())
    .Build();

// — or —

// Scanning all configurations in an assembly
TypeAttributeMapContainerBuilder container = new()
    .ApplyConfigurationsFromAssembly(typeof(TypeAttributeMapContainerBuilder).Assembly)
    .Build();
```

3. **Retrieve mapped attributes** Attributes can then be retrieved by calling 'GetAttribute()':

```csharp
// Get the attribute map for 'Example'
TypeAttributeMap<Example> exampleMap = container.GetAttributeMap<Example>();

// Retrieve the DataContractAttribute on the type
DataContractAttribute typeAttribute = exampleMap.GetAttribute<DataContractAttribute>();

// Retrieve the DataMemberAttribute on the property
DataMemberAttribute propAttribute = exampleMap
    .Get(x => x.ExampleProperty)
    .GetAttribute<DataMemberAttribute>();

// Retrieve the DataMemberAttribute on the field
DataMemberAttribute fieldAttribute = exampleMap
    .Get(x => x.ExampleField)
    .GetAttribute<DataMemberAttribute>();
```
