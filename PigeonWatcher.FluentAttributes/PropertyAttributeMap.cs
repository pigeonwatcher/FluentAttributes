using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PigeonWatcher.FluentAttributes;

/// <summary>
/// A <see cref="SymbolAttributeMap"/> for mapping <see cref="Attribute"/>s to properties.
/// </summary>
/// <param name="propertyInfo">The <see cref="System.Reflection.PropertyInfo"/> of the mapped property.</param>
public class PropertyAttributeMap(PropertyInfo propertyInfo) : MemberAttributeMap(propertyInfo)
{
    /// <summary>
    /// The <see cref="System.Reflection.PropertyInfo"/> of the mapped property.
    /// </summary>
    public PropertyInfo PropertyInfo { get; } = propertyInfo;

    /// <summary>
    /// The property <see cref="Type"/>.
    /// </summary>
    public Type PropertyType => PropertyInfo.PropertyType;
}