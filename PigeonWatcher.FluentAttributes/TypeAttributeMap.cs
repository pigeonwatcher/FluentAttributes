using PigeonWatcher.FluentAttributes.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PigeonWatcher.FluentAttributes;

/// <summary>
/// A <see cref="SymbolAttributeMap"/> for mapping <see cref="Attribute"/>s to classes and structs.
/// </summary>
public abstract class TypeAttributeMap(Type type) : SymbolAttributeMap
{
    private Dictionary<string, MemberAttributeMap>? _memberAttributeMapLookup;

    /// <summary>
    /// The symbol type.
    /// </summary>
    public Type Type { get; } = type;

    /// <summary>
    /// The <see cref="MemberAttributeMap"/>s for the members belonging to the <see cref="Type"/>.
    /// </summary>
    public IReadOnlyCollection<MemberAttributeMap> MemberAttributeMaps => MemberAttributeMapLookup.Values;

    /// <summary>
    /// The property maps for the members belonging to the <see cref="Type"/>.
    /// </summary>
    private Dictionary<string, MemberAttributeMap> MemberAttributeMapLookup => _memberAttributeMapLookup ??= [];

    /// <summary>
    /// Adds a <see cref="MemberAttributeMap"/> for a member of the <see cref="System.Type"/>.
    /// </summary>
    /// <param name="memberAttributeMap">The <see cref="MemberAttributeMap"/> instance.</param>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="memberAttributeMap"/> was added successfully; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public bool Add(MemberAttributeMap memberAttributeMap)
    {
        string memberName = memberAttributeMap.MemberInfo.Name;
        return MemberAttributeMapLookup.TryAdd(memberName, memberAttributeMap);
    }

    /// <summary>
    /// Gets the <see cref="MemberAttributeMap"/> for the specified <paramref name="memberName"/>.
    /// </summary>
    /// <param name="memberName">The name of the member in its <see cref="MemberInfo"/>.</param>
    /// <returns>The <see cref="MemberAttributeMap"/> instance.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the <see cref="MemberAttributeMap"/> is not found.</exception>
    public MemberAttributeMap Get(string memberName)
    {
        if (TryGet(memberName, out MemberAttributeMap? memberAttributeMap))
        {
            return memberAttributeMap;
        }

        throw new KeyNotFoundException($"Member attribute map for '{memberName}' not found.");
    }

    /// <summary>
    /// Checks if the <see cref="TypeAttributeMap"/> has a <see cref="MemberAttributeMap"/> for the specified 
    /// <paramref name="memberName"/>
    /// </summary>
    /// <param name="memberName">The name of the member in its <see cref="MemberInfo"/>.</param>
    /// <param name="memberAttributeMap">
    /// The <see cref="MemberAttributeMap"/> instance, or <see langword="null"/> if not found.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="memberAttributeMap"/> was found; otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGet(string memberName, [NotNullWhen(true)] out MemberAttributeMap? memberAttributeMap)
    {
        return MemberAttributeMapLookup.TryGetValue(memberName, out memberAttributeMap);
    }

    /// <summary>
    /// Gets the <see cref="MemberAttributeMap"/> for the specified <paramref name="memberName"/>.
    /// </summary>
    /// <typeparam name="TMemberAttributeMap">The <see cref="MemberAttributeMap"/> <see cref="System.Type"/>.</typeparam>
    /// <param name="memberName">The name of the member in its <see cref="MemberInfo"/>.</param>
    /// <returns>The <typeparamref name="TMemberAttributeMap"/> instance.</returns>
    /// <exception cref="InvalidCastException">
    /// Thrown if the <see cref="MemberAttributeMap"/> is not derived from <typeparamref name="TMemberAttributeMap"/>
    /// </exception>
    public TMemberAttributeMap Get<TMemberAttributeMap>(string memberName) where TMemberAttributeMap : MemberAttributeMap
    {
        return Get(memberName) as TMemberAttributeMap ??
               throw new InvalidCastException($"Member attribute map for '{memberName}' does not derive from {typeof(TMemberAttributeMap)}.");
    }

    /// <summary>
    /// Checks if the <see cref="TypeAttributeMap"/> has a <see cref="MemberAttributeMap"/> for the specified 
    /// <paramref name="memberName"/>
    /// </summary>
    /// <typeparam name="TMemberAttributeMap">The <see cref="MemberAttributeMap"/> <see cref="System.Type"/>.</typeparam>
    /// <param name="memberName">The name of the member in its <see cref="MemberInfo"/>.</param>
    /// <param name="memberAttributeMap">
    /// The <typeparamref name="TMemberAttributeMap"/> instance, or <see langword="null"/> if not found.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="memberAttributeMap"/> was found; otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGet<TMemberAttributeMap>(string memberName, [NotNullWhen(true)] out TMemberAttributeMap? memberAttributeMap)
        where TMemberAttributeMap : MemberAttributeMap
    {
        if (TryGet(memberName, out MemberAttributeMap? memberAttributeMapUncast))
        {
            memberAttributeMap = memberAttributeMapUncast as TMemberAttributeMap;
            return memberAttributeMap != null;
        }

        memberAttributeMap = null;
        return false;
    }
}

/// <inheritdoc />
/// <typeparam name="T"> The mapped <see cref="Type"/>.</typeparam>
public class TypeAttributeMap<T> : TypeAttributeMap
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeAttributeMap{T}"/> class.
    /// </summary>
    public TypeAttributeMap() : base(typeof(T))
    {
    }

    // TODO: Add non-generic methods for getting the member attribute map.

    /// <summary>
    /// Gets the <see cref="MemberAttributeMap"/> for the member defined in the <paramref name="expression"/>.
    /// </summary>
    /// <param name="expression">Expression to select a member belonging to the <see cref="TypeAttributeMap.Type"/>.</param>
    /// <returns>The  <see cref="MemberAttributeMap"/> instance.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the <see cref="MemberAttributeMap"/> is not found.</exception>
    public MemberAttributeMap Get(Expression<Func<T, object?>> expression)
    {
        MemberInfo memberInfo = ExpressionUtilities.GetMemberInfo(expression);
        string propertyName = memberInfo.Name;
        return Get(propertyName);
    }

    /// <summary>
    /// Checks if the <see cref="TypeAttributeMap"/> has a <see cref="MemberAttributeMap"/> for the member defined in 
    /// the <paramref name="expression"/>.
    /// </summary>
    /// <param name="expression">Expression to select a member belonging to the <see cref="TypeAttributeMap.Type"/>.</param>
    /// <param name="memberAttributeMap">
    /// The <see cref="MemberAttributeMap"/> instance, or <see langword="null"/> if not found.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="memberAttributeMap"/> was found; otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGet(Expression<Func<T, object?>> expression, [NotNullWhen(true)] out MemberAttributeMap? memberAttributeMap)
    {
        MemberInfo memberInfo = ExpressionUtilities.GetMemberInfo(expression);
        string propertyName = memberInfo.Name;
        return TryGet(propertyName, out memberAttributeMap);
    }

    /// <summary>
    /// Gets the <see cref="MemberAttributeMap"/> for the member defined in the <paramref name="expression"/>.
    /// </summary>
    /// <typeparam name="TMemberAttributeMap">The <see cref="MemberAttributeMap"/> <see cref="Type"/>.</typeparam>
    /// <param name="expression">Expression to select a member belonging to the <see cref="TypeAttributeMap.Type"/>.</param>
    /// <returns>The <typeparamref name="TMemberAttributeMap"/> instance.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the <see cref="MemberAttributeMap"/> is not found.</exception>
    public TMemberAttributeMap Get<TMemberAttributeMap>(Expression<Func<T, object?>> expression)
        where TMemberAttributeMap : MemberAttributeMap
    {
        MemberInfo memberInfo = ExpressionUtilities.GetMemberInfo(expression);
        string propertyName = memberInfo.Name;
        return Get<TMemberAttributeMap>(propertyName);
    }

    /// <summary>
    /// Checks if the <see cref="TypeAttributeMap"/> has a <see cref="MemberAttributeMap"/> for the member defined in 
    /// the <paramref name="expression"/>.
    /// </summary>
    /// <typeparam name="TMemberAttributeMap">The <see cref="MemberAttributeMap"/> <see cref="System.Type"/>.</typeparam>
    /// <param name="expression">Expression to select a member belonging to the <see cref="TypeAttributeMap.Type"/>.</param>
    /// <param name="memberAttributeMap">
    /// The <typeparamref name="TMemberAttributeMap"/> instance, or <see langword="null"/> if not found.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="memberAttributeMap"/> was found; otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGet<TMemberAttributeMap>(Expression<Func<T, object?>> expression, [NotNullWhen(true)] out TMemberAttributeMap? memberAttributeMap)
        where TMemberAttributeMap : MemberAttributeMap
    {
        MemberInfo memberInfo = ExpressionUtilities.GetMemberInfo(expression);
        string propertyName = memberInfo.Name;
        return TryGet(propertyName, out memberAttributeMap);
    }
}