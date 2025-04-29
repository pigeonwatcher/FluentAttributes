using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PigeonWatcher.FluentAttributes.Builders;

/// <summary>
/// The <see cref="SymbolAttributeMapBuilder"/> for building <see cref="FieldAttributeMap"/> instances.
/// </summary>
public class FieldAttributeMapBuilder(FieldInfo fieldInfo) : MemberAttributeMapBuilder<FieldAttributeMap>
{
    /// <summary>
    /// Builds the <see cref="FieldAttributeMap"/> instance.
    /// </summary>
    /// <returns>The built <see cref="FieldAttributeMap"/> instance.</returns>
    public override FieldAttributeMap Build()
    {
        FieldAttributeMap fieldAttributeMap = new(fieldInfo);
        BuildAttributes(fieldAttributeMap);
        BuildPredefinedAttributes(fieldAttributeMap, fieldInfo.GetCustomAttributes());
        return fieldAttributeMap;
    }
}


