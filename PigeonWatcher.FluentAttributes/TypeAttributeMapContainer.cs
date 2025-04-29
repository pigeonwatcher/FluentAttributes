using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PigeonWatcher.FluentAttributes;

/// <summary>
/// Represents a container that contains the mapping of <see cref="Type"/>s.
/// </summary>
public sealed class TypeAttributeMapContainer : IEnumerable<TypeAttributeMap>
{
    private readonly Dictionary<Type, TypeAttributeMap> _typeAttributeMaps = [];

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
    /// Gets the <see cref="TypeAttributeMap"/> for the specified <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The mapped <see cref="Type"/>.</typeparam>
    /// <returns>The <see cref="TypeAttributeMap"/> instances for the specified <typeparamref name="T"/>.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the <see cref="TypeAttributeMap"/> does not exist.</exception>
    public TypeAttributeMap<T> GetAttributeMap<T>()
    {
        if (TryGetAttributeMap(out TypeAttributeMap<T>? typeAttributeMap))
        {
            return typeAttributeMap;
        }

        throw new KeyNotFoundException($"No TypeAttributeMap found for type {typeof(T).FullName}.");
    }

    /// <summary>
    /// Gets the <see cref="TypeAttributeMap"/> for the specified <see cref="Type"/>.
    /// </summary>
    /// <param name="type">The mapped <see cref="Type"/>.</param>
    /// <returns>The <see cref="TypeAttributeMap"/> instances for the specified <see cref="Type"/>.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the <see cref="TypeAttributeMap"/> does not exist.</exception>
    public TypeAttributeMap GetAttributeMap(Type type)
    {
        if (TryGetAttributeMap(type, out TypeAttributeMap? typeAttributeMap))
        {
            return typeAttributeMap;
        }

        throw new KeyNotFoundException($"No TypeAttributeMap found for type {type.FullName}.");
    }

    /// <summary>
    /// Tries to get a <see cref="TypeAttributeMap"/> for the specified <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The mapped <see cref="Type"/>.</typeparam>
    /// <param name="typeAttributeMap">The <see cref="TypeAttributeMap"/> instance, or <see langword="null"/> if not found.</param>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="typeAttributeMap"/> was found; otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGetAttributeMap<T>([NotNullWhen(true)] out TypeAttributeMap<T>? typeAttributeMap)
    {
        if (TryGetAttributeMap(typeof(T), out TypeAttributeMap? foundTypeAttributeMap))
        {
            typeAttributeMap = (TypeAttributeMap<T>)foundTypeAttributeMap;
            return true;
        }

        typeAttributeMap = null;
        return false;
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
}