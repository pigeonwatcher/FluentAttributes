using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PigeonWatcher.FluentAttributes;

/// <summary>
/// A <see cref="SymbolAttributeMap"/> for mapping <see cref="Attribute"/>s to members.
/// </summary>
public abstract class MemberAttributeMap(MemberInfo memberInfo) : SymbolAttributeMap
{
    /// <summary>
    /// The symbol <see cref="MemberInfo"/>.
    /// </summary>
    public MemberInfo MemberInfo { get; } = memberInfo;
}