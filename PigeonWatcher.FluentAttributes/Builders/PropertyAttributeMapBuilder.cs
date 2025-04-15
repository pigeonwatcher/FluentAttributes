using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PigeonWatcher.FluentAttributes.Builders;

/// <summary>
/// The <see cref="SymbolAttributeMapBuilder{TMapperBuilder}"/> for building <see cref="PropertyAttributeMap"/> instances.
/// </summary>
public class PropertyAttributeMapBuilder(PropertyInfo propertyInfo) : SymbolAttributeMapBuilder<PropertyAttributeMapBuilder>
{
    /// <summary>
    /// Builds the <see cref="PropertyAttributeMap"/> instance.
    /// </summary>
    /// <returns>The built <see cref="PropertyAttributeMap"/> instance.</returns>
    public PropertyAttributeMap Build()
    {
        PropertyAttributeMap propertyAttributeMap = new()
        {
            PropertyInfo = propertyInfo,
        };

        if (Attributes is not null)
        {
            foreach (Attribute attribute in Attributes)
            {
                propertyAttributeMap.AddAttribute(attribute);
            }
        }

        if (IncludePredefinedAttributesFlag)
        {
            foreach (Attribute attribute in propertyInfo.GetCustomAttributes())
            {
                propertyAttributeMap.AddAttribute(attribute);
            }
        }

        return propertyAttributeMap;
    }
}
