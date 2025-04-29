using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PigeonWatcher.FluentAttributes.Builders;

/// <summary>
/// The <see cref="SymbolAttributeMapBuilder"/> for building <see cref="PropertyAttributeMap"/> instances.
/// </summary>
public class PropertyAttributeMapBuilder(PropertyInfo propertyInfo) : MemberAttributeMapBuilder<PropertyAttributeMap>
{
    /// <summary>
    /// Builds the <see cref="PropertyAttributeMap"/> instance.
    /// </summary>
    /// <returns>The built <see cref="PropertyAttributeMap"/> instance.</returns>
    public override PropertyAttributeMap Build()
    {
        PropertyAttributeMap propertyAttributeMap = new(propertyInfo);
        BuildAttributes(propertyAttributeMap);
        BuildPredefinedAttributes(propertyAttributeMap, propertyInfo.GetCustomAttributes());
        return propertyAttributeMap;
    }
}


