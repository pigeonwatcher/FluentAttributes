using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    protected List<Attribute>? Attributes { get; set; }

    protected bool IncludePredefinedAttributesFlag;

    /// <summary>
    /// Sets a flag to include predefined attributes in the attribute map. 
    /// </summary>
    /// <param name="includePredefinedAttributes">
    /// <see cref="true"/> to include predefined attributes; otherwise, <see cref="false"/>.
    /// </param>
    /// <returns>The <see cref="TMapBuilder"/> instance.</returns>
    public TMapBuilder IncludePredefinedAttributes(bool includePredefinedAttributes = true)
    {
        IncludePredefinedAttributesFlag = includePredefinedAttributes;
        return (TMapBuilder)this;
    }

    /// <summary>
    /// Adds an attribute to the symbol map.
    /// </summary>
    /// <param name="attribute">The <see cref="Attribute"/> instance.</param>
    /// <returns>The <see cref="TMapBuilder"/> instance.</returns>
    public TMapBuilder WithAttribute(Attribute attribute)
    {
        Attributes ??= new();
        Attributes.Add(attribute);
        return (TMapBuilder)this;
    }
}
