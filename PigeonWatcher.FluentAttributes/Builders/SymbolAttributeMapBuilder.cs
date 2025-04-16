using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PigeonWatcher.FluentAttributes.Builders;

/// <summary>
/// The base builder class for all <see cref="SymbolAttributeMap"/> builders.
/// </summary>
public abstract class SymbolAttributeMapBuilder<TMapBuilder> where TMapBuilder : SymbolAttributeMapBuilder<TMapBuilder>
{
    /// <summary>
    /// The symbol <see cref="Attribute"/>s.
    /// </summary>
    public List<Attribute>? Attributes { get; private set; }

    public bool IncludePredefinedAttributesFlag { get; private set; }

    /// <summary>
    /// Sets a flag to include predefined <see cref="Attribute"/> in the attribute map. 
    /// </summary>
    /// <param name="includePredefinedAttributes">
    /// <see langword="true"/> to include predefined attributes; otherwise, <see langword="false"/>.
    /// </param>
    /// <returns>The <see cref="TMapBuilder"/> instance.</returns>
    public TMapBuilder IncludePredefinedAttributes(bool includePredefinedAttributes = true)
    {
        IncludePredefinedAttributesFlag = includePredefinedAttributes;
        return (TMapBuilder)this;
    }

    /// <summary>
    /// Adds an <see cref="Attribute"/> to the symbol map.
    /// </summary>
    /// <param name="attribute">The <see cref="Attribute"/> instance.</param>
    /// <returns>The <see cref="TMapBuilder"/> instance.</returns>
    public TMapBuilder WithAttribute(Attribute attribute)
    {
        Attributes ??= [];
        Attributes.Add(attribute);
        return (TMapBuilder)this;
    }

    /// <summary>
    /// Adds or retrieves an existing <see cref="Attribute"/> of <see cref="Type"/> <typeparamref name="TAttribute"/>,
    /// and then applies the specified configuration action on it.
    /// </summary>
    /// <typeparam name="TAttribute">
    /// The <see cref="Attribute"/> <see cref="Type"/>. Must inherit from <see cref="Attribute"/> and have a 
    /// parameterless constructor.
    /// </typeparam>
    /// <param name="configure">=An <see cref="Action"/> to configure the <see cref="Attribute"/>.</param>
    /// <returns>The <see cref="TMapBuilder"/> instance.</returns>
    /// <example>
    /// This example demonstrates how to add an <c>ExampleAttribute</c> and configure its <c>Property</c> member:
    /// <code>
    /// builder.WithAttribute&lt;ExampleAttribute&gt;(attribute => 
    /// {
    ///     attribute.Property = "value";
    /// });
    /// </code>
    /// </example>
    public TMapBuilder WithAttribute<TAttribute>(Action<TAttribute> configure)
        where TAttribute : Attribute, new()
    {
        Attributes ??= [];
        Attribute? attribute = Attributes.OfType<TAttribute>().FirstOrDefault();
        if (attribute == null)
        {
            attribute = new TAttribute();
            Attributes.Add(attribute);
        }

        configure((TAttribute)attribute);

        return (TMapBuilder)this;
    }

}
