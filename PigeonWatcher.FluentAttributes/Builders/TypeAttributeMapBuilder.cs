using PigeonWatcher.FluentAttributes.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PigeonWatcher.FluentAttributes.Builders;

/// <summary>
/// The <see cref="SymbolAttributeMapBuilder"/> for building <see cref="TypeAttributeMap"/> instances.
/// </summary>
/// <typeparam name="T">The mapped <see cref="Type"/>.</typeparam>
public class TypeAttributeMapBuilder<T> : SymbolAttributeMapBuilder
{
    private Dictionary<string, MemberAttributeMapBuilder>? _memberAttributeMapBuilders;

    /// <summary>
    /// The <see cref="MemberAttributeMapBuilder"/> instances for the members belonging to the <see cref="TypeAttributeMap.Type"/>. 
    /// </summary>
    private Dictionary<string, MemberAttributeMapBuilder> MemberAttributeMapBuilders => _memberAttributeMapBuilders ??= [];

    /// <inheritdoc />
    public override TypeAttributeMapBuilder<T> IncludePredefinedAttributes(bool includePredefinedAttributes = true)
    {
        base.IncludePredefinedAttributes(includePredefinedAttributes);
        return this;
    }

    /// <inheritdoc />
    public override TypeAttributeMapBuilder<T> WithAttribute(Attribute attribute)
    {
        base.WithAttribute(attribute);
        return this;
    }

    /// <inheritdoc />
    public override TypeAttributeMapBuilder<T> WithAttribute<TAttribute>(Action<TAttribute> configure)
    {
        base.WithAttribute(configure);
        return this;
    }

    /// <summary>
    /// Gets the <see cref="PropertyAttributeMapBuilder"/> instance from the <paramref name="expression"/>.
    /// </summary>
    /// <param name="expression">Expression to select a property belonging to the <see cref="TypeAttributeMap.Type"/>.</param>
    /// <returns>A <see cref="PropertyAttributeMapBuilder"/> instance.</returns>
    public PropertyAttributeMapBuilder Property(Expression<Func<T, object?>> expression)
    {
        if (ExpressionUtilities.GetMemberInfo(expression) is not PropertyInfo propertyInfo)
        {
            throw new InvalidOperationException("The selected member is not a property.");
        }

        string propertyName = propertyInfo.Name;
        if (!MemberAttributeMapBuilders.TryGetValue(propertyName, out MemberAttributeMapBuilder? builder))
        {
            builder = new PropertyAttributeMapBuilder(propertyInfo);
            MemberAttributeMapBuilders[propertyName] = builder;
        }

        return (PropertyAttributeMapBuilder)builder;
    }

    /// <summary>
    /// Gets the <see cref="FieldAttributeMapBuilder"/> instance from the <paramref name="expression"/>.
    /// </summary>
    /// <param name="expression">Expression to select a field belonging to the <see cref="TypeAttributeMap.Type"/>.</param>
    /// <returns>A <see cref="FieldAttributeMapBuilder"/> instance.</returns>
    public FieldAttributeMapBuilder Field(Expression<Func<T, object?>> expression)
    {
        if (ExpressionUtilities.GetMemberInfo(expression) is not FieldInfo fieldInfo)
        {
            throw new InvalidOperationException("The selected member is not a field.");
        }

        string fieldName = fieldInfo.Name;
        if (!MemberAttributeMapBuilders.TryGetValue(fieldName, out MemberAttributeMapBuilder? builder))
        {
            builder = new FieldAttributeMapBuilder(fieldInfo);
            MemberAttributeMapBuilders[fieldName] = builder;
        }

        return (FieldAttributeMapBuilder)builder;
    }

    /// <summary>
    /// Gets the <see cref="MethodAttributeMapBuilder"/> instance from the <paramref name="expression"/>.
    /// </summary>
    /// <param name="expression">Expression to select a method belonging to the <see cref="TypeAttributeMap.Type"/>.</param>
    /// <returns>A <see cref="MethodAttributeMapBuilder"/> instance.</returns>
    public MethodAttributeMapBuilder Method(Expression<Func<T, Delegate>> expression)
    {
        if (ExpressionUtilities.GetMemberInfo(expression) is not MethodInfo methodInfo)
        {
            throw new InvalidOperationException("The selected member is not a method.");
        }

        string methodName = methodInfo.Name;
        if (!MemberAttributeMapBuilders.TryGetValue(methodName, out MemberAttributeMapBuilder? builder))
        {
            builder = new MethodAttributeMapBuilder(methodInfo);
            MemberAttributeMapBuilders[methodName] = builder;
        }

        return (MethodAttributeMapBuilder)builder;
    }

    /// <summary>
    /// Builds the <see cref="TypeAttributeMap"/> instance.
    /// </summary>
    /// <returns>The built <see cref="TypeAttributeMap"/> instance.</returns>
    public override TypeAttributeMap<T> Build()
    {
        TypeAttributeMap<T> typeAttributeMap = new();
        BuildAttributes(typeAttributeMap);
        BuildPredefinedAttributes(typeAttributeMap, typeof(T).GetCustomAttributes());

        if (_memberAttributeMapBuilders != null)
        {
            foreach (MemberAttributeMapBuilder memberBuilder in MemberAttributeMapBuilders.Values)
            {
                typeAttributeMap.Add(memberBuilder.Build());
            }
        }

        return typeAttributeMap;
    }
}