using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PigeonWatcher.FluentAttributes.Builders;

/// <summary>
/// The <see cref="SymbolAttributeMapBuilder"/> for building <see cref="MemberAttributeMap"/> instances.
/// </summary>
public abstract class MemberAttributeMapBuilder : SymbolAttributeMapBuilder
{
    /// <summary>
    /// Builds the <see cref="MemberAttributeMap"/> instance.
    /// </summary>
    /// <returns>The built <see cref="MemberAttributeMap"/> instance.</returns>
    public abstract override MemberAttributeMap Build();
}

/// <summary>
/// The <see cref="SymbolAttributeMapBuilder"/> for building <see cref="MemberAttributeMap"/> instances.
/// </summary>
/// <typeparam name="TAttributeMap">The <see cref="MemberAttributeMap"/> <see cref="Type"/> to build.</typeparam>
public abstract class MemberAttributeMapBuilder<TAttributeMap> : MemberAttributeMapBuilder where TAttributeMap : MemberAttributeMap
{
    /// <inheritdoc />
    public override MemberAttributeMapBuilder<TAttributeMap> IncludePredefinedAttributes(bool includePredefinedAttributes = true)
    {
        base.IncludePredefinedAttributes(includePredefinedAttributes);
        return this;
    }

    /// <inheritdoc />
    public override MemberAttributeMapBuilder<TAttributeMap> WithAttribute(Attribute attribute)
    {
        base.WithAttribute(attribute);
        return this;
    }

    /// <inheritdoc />
    public override MemberAttributeMapBuilder<TAttributeMap> WithAttribute<TAttribute>(Action<TAttribute> configure)
    {
        base.WithAttribute(configure);
        return this;
    }
}