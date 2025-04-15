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
public class PropertyAttributeMap : SymbolAttributeMap
{
    /// <summary>
    /// The <see cref="PropertyInfo"/> of the mapped property.
    /// </summary>
    public required PropertyInfo PropertyInfo { get; init; }

    /// <summary>
    /// The property <see cref="Type"/>.
    /// </summary>
    public Type PropertyType => PropertyInfo.PropertyType;
}
