using PigeonWatcher.FluentAttributes.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PigeonWatcher.FluentAttributes;

/// <summary>
/// Interface for configuring a <see cref="TypeAttributeMap"/> instance.
/// </summary>
/// <typeparam name="T">The mapped <see cref="Type"/>.</typeparam>
public interface ITypeAttributeMapConfiguration<T>
{
    /// <summary>
    /// Configures the <see cref="TypeAttributeMapBuilder{T}"/> instance.
    /// </summary>
    /// <param name="builder">The <see cref="TypeAttributeMapBuilder{T}"/> instance to configure.</param>
    void Configure(TypeAttributeMapBuilder<T> builder);
}
