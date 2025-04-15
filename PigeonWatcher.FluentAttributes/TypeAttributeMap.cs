using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PigeonWatcher.FluentAttributes;

/// <summary>
/// A <see cref="SymbolAttributeMap"/> for mapping <see cref="Attribute"/>s to classes and structs.
/// </summary>
public class TypeAttributeMap : SymbolAttributeMap
{
    /// <summary>
    /// The symbol type.
    /// </summary>
    public required Type Type { get; init; }

    private Dictionary<string, PropertyAttributeMap>? _propertyAttributeMaps;
    /// <summary>
    /// The property maps for the properties belonging to the <see cref="Type"/>.
    /// </summary>
    private Dictionary<string, PropertyAttributeMap> PropertyAttributeMaps => _propertyAttributeMaps ??= new();

    /// <summary>
    /// Adds a <see cref="PropertyAttributeMap"/> for the specified <paramref name="propertyName"/>.
    /// </summary>
    /// <param name="propertyName">The property name.</param>
    /// <param name="propertyAttributeMap">The <see cref="PropertyAttributeMap"/> instance.</param>
    /// <returns>
    /// <see cref="true"/> if the <paramref name="propertyAttributeMap"/> was added successfully; otherwise,
    /// <see cref="false"/>.
    /// </returns>
    public bool AddPropertyAttributeMap(string propertyName, PropertyAttributeMap propertyAttributeMap)
    {
        return PropertyAttributeMaps.TryAdd(propertyName, propertyAttributeMap);
    }

    /// <summary>
    /// Checks if the <see cref="TypeAttributeMap"/> has a <see cref="PropertyAttributeMap"/> for the specified <paramref name="propertyName"/>
    /// </summary>
    /// <param name="propertyName">The property name.</param>
    /// <param name="propertyAttributeMap">The <see cref="PropertyAttributeMap"/> instance, or <see langword="null"/> if not found.</param>
    /// <returns>
    /// <see cref="true"/> if the <paramref name="propertyAttributeMap"/> was found; otherwise, <see cref="false"/>.
    /// </returns>
    public bool TryGetPropertyAttributeMap(string propertyName, [NotNullWhen(true)] out PropertyAttributeMap? propertyAttributeMap)
    {
        return PropertyAttributeMaps.TryGetValue(propertyName, out propertyAttributeMap);
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
}
