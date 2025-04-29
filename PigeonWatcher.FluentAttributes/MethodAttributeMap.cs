using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PigeonWatcher.FluentAttributes;

/// <summary>
/// A <see cref="SymbolAttributeMap"/> for mapping <see cref="Attribute"/>s to methods.
/// </summary>
/// <param name="methodInfo">The <see cref="System.Reflection.MethodInfo"/> of the mapped method.</param>
public class MethodAttributeMap(MethodInfo methodInfo) : MemberAttributeMap(methodInfo)
{
    /// <summary>
    /// The <see cref="System.Reflection.MethodInfo"/> of the mapped property.
    /// </summary>
    public MethodInfo MethodInfo { get; } = methodInfo;
}
