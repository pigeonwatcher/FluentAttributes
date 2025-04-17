# FluentAttributes
An attribute library for .NET that uses a fluent interface for applying attributes to classes, structs, properties, etc.

## Aim

**FluentAttributes** separates attribute metadata from implementation, enabling:
- **Decoupled Metadata**: Manage attributes in configuration classes.
- **Flexibility**: Change, add, or remove attributes without touching the original implementation.
- **Cleaner Code**: Limit the amount of attributes in original implementation.
  
## Get Started

1. **Create a Configuration** Create a class that derives from [ITypeAttributeMapConfiguration](PigeonWatcher.FluentAttributes/ITypeAttributeMapConfiguration.cs) with the generic set to the Type you want to map attributes to.

``` 
    public class ExampleAttributeMapConfiguration : ITypeAttributeMapConfiguration<Example>
    {
        public void Configure(TypeAttributeMapBuilder<Example> builder)
        {
            builder.WithAttribute(new DataContractAttribute());

            builder.Property(x => x.ExampleProperty)
                .WithAttribute(new JsonPropertyNameAttribute("example_property"))
                .WithAttribute(new DataMemberAttribute { Name = "example_property" });
        }
    }
```

2. **Build a Container**: Add your configuration to a [TypeMapAttributeContainer](PigeonWatcher.FluentAttributes/TypeAttributeMapContainer.cs).

```csharp
TypeAttributeMapContainer container = new TypeAttributeMapContainerBuilder()
    .ApplyConfiguration(new ExampleAttributeMapConfiguration())
    .Build();

// Or scan assembly.
TypeAttributeMapContainer container = new TypeAttributeMapContainerBuilder()
    .ApplyConfigurationsFromAssembly(typeof(TypeAttributeMapContainerBuilder).Assembly)
    .Build();
```

3. **Retrieve mapped attributes** Attributes can then be retrieved by calling 'GetAttribute()':

```csharp
TypeAttributeMap<Example> exampleAttributeMap = container.GetAttributeMap<Example>();
DataContractAttribute exampleDataContract = exampleAttributeMap.GetAttribute<DataContractAttribute>();
```

## Limitations

Currently, this library only supports attributes on classes, structs, enums and properties. Support for members such as fields and methods intend to be implemented in the future. 
