using PigeonWatcher.FluentAttributes.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PigeonWatcher.FluentAttributes;

/// <summary>
/// A <see cref="SymbolAttributeMap"/> for mapping <see cref="Attribute"/>s to classes and structs.
/// </summary>
public abstract class TypeAttributeMap(Type type) : SymbolAttributeMap
{
    /// <summary>
    /// The symbol type.
    /// </summary>
    public Type Type { get; } = type;

    private Dictionary<string, PropertyAttributeMap>? _propertyAttributeMaps;
    /// <summary>
    /// The property maps for the properties belonging to the <see cref="Type"/>.
    /// </summary>
    private Dictionary<string, PropertyAttributeMap> PropertyAttributeMaps => _propertyAttributeMaps ??= [];

    /// <summary>
    /// Adds a <see cref="PropertyAttributeMap"/> for the specified <paramref name="propertyName"/>.
    /// </summary>
    /// <param name="propertyName">The property name.</param>
    /// <param name="propertyAttributeMap">The <see cref="PropertyAttributeMap"/> instance.</param>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="propertyAttributeMap"/> was added successfully; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public bool AddPropertyAttributeMap(string propertyName, PropertyAttributeMap propertyAttributeMap)
    {
        return PropertyAttributeMaps.TryAdd(propertyName, propertyAttributeMap);
    }

    /// <summary>
    /// Gets the <see cref="PropertyAttributeMap"/> for the specified <paramref name="propertyName"/>.
    /// </summary>
    /// <param name="propertyName">The property name.</param>
    /// <returns>The <see cref="PropertyAttributeMap"/> with the <paramref name="propertyName"/>.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Thrown if no <see cref="PropertyAttributeMap"/> with <paramref name="propertyName"/> is found.
    /// </exception>
    public PropertyAttributeMap GetPropertyAttributeMap(string propertyName)
    {
        if (TryGetPropertyAttributeMap(propertyName, out PropertyAttributeMap? propertyAttributeMap))
        {
            return propertyAttributeMap;
        }

        throw new KeyNotFoundException($"Property attribute map for '{propertyName}' not found.");
    }

    /// <summary>
    /// Checks if the <see cref="TypeAttributeMap"/> has a <see cref="PropertyAttributeMap"/> for the specified <paramref name="propertyName"/>
    /// </summary>
    /// <param name="propertyName">The property name.</param>
    /// <param name="propertyAttributeMap">The <see cref="PropertyAttributeMap"/> instance, or <see langword="null"/> if not found.</param>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="propertyAttributeMap"/> was found; otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGetPropertyAttributeMap(string propertyName, [NotNullWhen(true)] out PropertyAttributeMap? propertyAttributeMap)
    {
        return PropertyAttributeMaps.TryGetValue(propertyName, out propertyAttributeMap);
    }
}

/// <inheritdoc />
/// <typeparam name="T"> The mapped <see cref="Type"/>.</typeparam>
public class TypeAttributeMap<T> : TypeAttributeMap
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeAttributeMap{T}"/> class.
    /// </summary>
    public TypeAttributeMap() : base(typeof(T)) { }

    /// <summary>
    /// Gets the <see cref="PropertyAttributeMap"/> for the property defined in the <paramref name="expression"/>.
    /// </summary>
    /// <param name="expression">Expression to select a property belonging to the <see cref="TypeAttributeMap.Type"/>.</param>
    /// <returns>The <see cref="PropertyAttributeMap"/> for the property.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the expression does not relate to a property.</exception>
    public PropertyAttributeMap GetPropertyAttributeMap(Expression<Func<T, object?>> expression)
    {
        MemberExpression memberExpression = ExpressionUtilities.GetMemberExpression(expression.Body);
        if (!ExpressionUtilities.IsPropertyAccess(memberExpression))
        {
            throw new InvalidOperationException("The selected member is not a property.");
        }

        string propertyName = memberExpression.Member.Name;
        return GetPropertyAttributeMap(propertyName);
    }

    /// <summary>
    /// Checks if the <see cref="TypeAttributeMap"/> has a <see cref="PropertyAttributeMap"/> for the property defined 
    /// in the <paramref name="expression"/>.
    /// </summary>
    /// <param name="expression">Expression to select a property belonging to the <see cref="TypeAttributeMap.Type"/>.</param>
    /// <param name="propertyAttributeMap">
    /// The <see cref="PropertyAttributeMap"/> instance, or <see langword="null"/> if not found.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="propertyAttributeMap"/> was found; otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="InvalidOperationException">Thrown if the expression does not relate to a property.</exception>
    public bool TryGetPropertyAttributeMap(Expression<Func<T, object?>> expression, [NotNullWhen(true)] out PropertyAttributeMap? propertyAttributeMap)
    {
        MemberExpression memberExpression = ExpressionUtilities.GetMemberExpression(expression.Body);
        if (!ExpressionUtilities.IsPropertyAccess(memberExpression))
        {
            throw new InvalidOperationException("The selected member is not a property.");
        }

        string propertyName = memberExpression.Member.Name;
        return TryGetPropertyAttributeMap(propertyName, out propertyAttributeMap);
    }
}
