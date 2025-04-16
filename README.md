# FluentAttributes
An attribute library for .NET that uses a fluent interface for applying attributes to classes, structs, properties, etc.

## Aim

FluentAttributes aims to provide a flexible and clean way for applying C# attributes to classes, structs, properties, etc, without explicitly declaring attributes on the implementation itself. The benefit of this separation enables modification to the implementation metadata without needing to touch the original implementation. This is useful in projects where attributes may be added, changed or removed over time.

## Get Started

Create a class that derives from ITypeAttributeMapConfiguration with the generic set to the Type you want to map attributes to.

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

The attributes definied in your ITypeAttributeMapConfiguration implementation can then be accessed via a TypeAttributeMapContainer. To create a TypeMapAttributeContainer, you can use the TypeAttributeMapBuilder class like so:

```
TypeAttributeMapContainer container = new TypeAttributeMapContainerBuilder()
    .ApplyConfiguration(new ExampleAttributeMapConfiguration())
    .Build();
```
Or
```
TypeAttributeMapContainer container = new TypeAttributeMapContainerBuilder()
    .ApplyConfigurationsFromAssembly(typeof(TypeAttributeMapContainerBuilder).Assembly)
    .Build();
```

Attributes can then be accessed using ...

## Limitations

Currently, this library only supports attributes on classes, structs, enums and properties. Support for members such as fields and methods intend to be implemented in the future. 
