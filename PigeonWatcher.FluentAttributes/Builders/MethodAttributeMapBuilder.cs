using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PigeonWatcher.FluentAttributes.Builders;

/// <summary>
/// The <see cref="SymbolAttributeMapBuilder"/> for building <see cref="MethodAttributeMap"/> instances.
/// </summary>
public class MethodAttributeMapBuilder(MethodInfo methodInfo) : MemberAttributeMapBuilder<MethodAttributeMap>
{
    /// <summary>
    /// Builds the <see cref="MethodAttributeMap"/> instance.
    /// </summary>
    /// <returns>The built <see cref="MethodAttributeMap"/> instance.</returns>
    public override MethodAttributeMap Build()
    {
        MethodAttributeMap propertyAttributeMap = new(methodInfo);
        BuildAttributes(propertyAttributeMap);
        BuildPredefinedAttributes(propertyAttributeMap, methodInfo.GetCustomAttributes());
        return propertyAttributeMap;
    }
}


