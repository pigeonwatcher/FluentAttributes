using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PigeonWatcher.FluentAttributes;

/// <summary>
/// The base class for mapping <see cref="Attribute"/>s to symbols (classes, structs, properties, etc.).
/// </summary>
public abstract class SymbolAttributeMap
{
    private Dictionary<Type, Attribute>? _attributes;
    /// <summary>
    /// The symbol <see cref="Attribute"/>s. The key is the <see cref="Attribute"/> <see cref="Type"/>, and the value is 
    /// the <see cref="Attribute"/> instance.
    /// </summary>
    private Dictionary<Type, Attribute> Attributes => _attributes ??= [];

    /// <summary>
    /// Adds an <see cref="Attribute"/> to the symbol.
    /// </summary>
    /// <param name="attribute">The <see cref="Attribute"/> instance.</param>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="attribute"/> was added successfully; otherwise, <see langword="false"/>.
    /// </returns>
    public bool AddAttribute(Attribute attribute)
    {
        ArgumentNullException.ThrowIfNull(attribute);
        return Attributes.TryAdd(attribute.GetType(), attribute);
    }

    /// <summary>
    /// Checks if the symbol has an <see cref="Attribute"/> of <see cref="Type"/> <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The <see cref="Attribute"/> <see cref="Type"/>.</typeparam>
    /// <returns>
    /// <see langword="true"/> if an <see cref="Attribute"/>of <see cref="Type"/> <typeparamref name="T"/> is 
    /// present; otherwise, <see langword="false"/>.
    /// </returns>
    public bool HasAttribute<T>() where T : Attribute
    {
        return Attributes.ContainsKey(typeof(T));
    }

    /// <summary>
    /// Tries to get an <see cref="Attribute"/> of <see cref="Type"/> <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The <paramref name="attribute"/> <see cref="Type"/>.</typeparam>
    /// <param name="attribute">
    /// The <see cref="Attribute"/> of <see cref="Type"/> <typeparamref name="T"/></param>, or <see langword="null"/>
    /// if not found.
    /// <returns>
    /// <see langword="true" /> if an <see cref="Attribute"/> of <see cref="Type"/> <typeparamref name="T"/> is
    /// present; otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGetAttribute<T>([NotNullWhen(true)] out T? attribute) where T : Attribute
    {
        if (Attributes.TryGetValue(typeof(T), out Attribute? foundAttribute))
        {
            attribute = (T)foundAttribute;
            return true;
        }

        attribute = default;
        return false;
    }

    /// <summary>
    /// Gets an <see cref="Attribute"/> of <see cref="Type"/> <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The <see cref="Attribute"/> <see cref="Type"/>.</typeparam>
    /// <returns>The <see cref="Attribute"/> of <see cref="Type"/> <typeparamref name="T"/>.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if no <see cref="Attribute"/> is found of <see cref="Type"/> <typeparamref name="T"/>.
    /// </exception>
    public T GetAttribute<T>() where T : Attribute
    {
        if (Attributes.TryGetValue(typeof(T), out Attribute? foundAttribute))
        {
            return (T)foundAttribute;
        }

        throw new InvalidOperationException($"Attribute of type {typeof(T).Name} not found.");
    }
}
