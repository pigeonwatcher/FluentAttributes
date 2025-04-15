using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PigeonWatcher.FluentAttributes;

/// <summary>
/// Represents a container that contains the mapping of <see cref="Type"/>s.
/// </summary>
public sealed class TypeAttributeMapContainer : IEnumerable<TypeAttributeMap>
{
    private readonly Dictionary<Type, TypeAttributeMap> _typeAttributeMaps = new();

    /// <summary>
    /// Adds a <see cref="TypeAttributeMap"/> to the model.
    /// </summary>
    /// <param name="typeAttributeMap">The <see cref="TypeAttributeMap"/> to add.</param>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="typeAttributeMap"/> was added to the model successfully; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public bool Add(TypeAttributeMap typeAttributeMap)
    {
        ArgumentNullException.ThrowIfNull(typeAttributeMap);
        return _typeAttributeMaps.TryAdd(typeAttributeMap.Type, typeAttributeMap);
    }

    /// <summary>
    /// Tries to get a <see cref="TypeAttributeMap"/> for the specified <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The mapped <see cref="Type"/>.</param>
    /// <param name="typeAttributeMap">The <see cref="TypeAttributeMap"/> instance, or <see langword="null"/> if not found.</param>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="typeAttributeMap"/> was found; otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGetAttributeMap(Type type, [NotNullWhen(true)] out TypeAttributeMap? typeAttributeMap)
    {
        return _typeAttributeMaps.TryGetValue(type, out typeAttributeMap);
    }

    /// <inheritdoc/>
    public IEnumerator<TypeAttributeMap> GetEnumerator()
    {
        return _typeAttributeMaps.Values.GetEnumerator();
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
