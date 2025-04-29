using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PigeonWatcher.FluentAttributes;

/// <summary>
/// A <see cref="SymbolAttributeMap"/> for mapping <see cref="Attribute"/>s to fields.
/// </summary>
/// <param name="fieldInfo">The <see cref="System.Reflection.FieldInfo"/> of the mapped field.</param>
public class FieldAttributeMap(FieldInfo fieldInfo) : MemberAttributeMap(fieldInfo)
{
    /// <summary>
    /// The <see cref="System.Reflection.FieldInfo"/> of the mapped field.
    /// </summary>
    public FieldInfo FieldInfo { get; } = fieldInfo;
}