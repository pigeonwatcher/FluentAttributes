using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PigeonWatcher.FluentAttributes.Builders;

/// <summary>
/// The <see cref="SymbolAttributeMapBuilder{TMapperBuilder}"/> for building <see cref="TypeAttributeMap"/> instances.
/// </summary>
/// <typeparam name="T">The mapped <see cref="Type"/>.</typeparam>
public class TypeAttributeMapBuilder<T> : SymbolAttributeMapBuilder<TypeAttributeMapBuilder<T>>
{
    private Dictionary<string, PropertyAttributeMapBuilder>? _propertyAttributeMapBuilders;
    /// <summary>
    /// The <see cref="PropertyAttributeMapBuilder"/> instances for the properties belonging to the <see cref="TypeAttributeMap.Type"/>. 
    /// </summary>
    private Dictionary<string, PropertyAttributeMapBuilder> PropertyAttributeMapBuilders => _propertyAttributeMapBuilders ??= [];

    /// <summary>
    /// Gets the <see cref="TypeAttributeMapBuilder{T}"/> instance from the <paramref name="propertyExpression"/>.
    /// </summary>
    /// <param name="propertyExpression">Expression to select a property belonging to the <see cref="TypeAttributeMap.Type"/>.</param>
    /// <returns>A <see cref="PropertyAttributeMapBuilder"/> instance.</returns>
    public PropertyAttributeMapBuilder Property(Expression<Func<T, object?>> propertyExpression)
    {
        string propertyName = GetPropertyName(propertyExpression);
        if (!PropertyAttributeMapBuilders.TryGetValue(propertyName, out PropertyAttributeMapBuilder? propertyAttributeMapBuilder))
        {
            propertyAttributeMapBuilder = new PropertyAttributeMapBuilder(GetPropertyInfo(propertyExpression));
            PropertyAttributeMapBuilders[propertyName] = propertyAttributeMapBuilder;
        }

        return propertyAttributeMapBuilder;
    }

    /// <summary>
    /// Builds the <see cref="TypeAttributeMap"/> instance.
    /// </summary>
    /// <returns>The built <see cref="TypeAttributeMap"/> instance.</returns>
    public TypeAttributeMap Build()
    {
        TypeAttributeMap typeAttributeMap = new()
        {
            Type = typeof(T),
        };

        if (Attributes is not null)
        {
            foreach (Attribute attribute in Attributes)
            {
                typeAttributeMap.AddAttribute(attribute);
            }
        }

        if (IncludePredefinedAttributesFlag)
        {
            foreach (Attribute attribute in typeAttributeMap.Type.GetCustomAttributes())
            {
                typeAttributeMap.AddAttribute(attribute);
            }
        }

        if (_propertyAttributeMapBuilders is not null)
        {
            foreach ((string propertyName, PropertyAttributeMapBuilder propertyAttributeMapBuilder) in PropertyAttributeMapBuilders)
            {
                typeAttributeMap.AddPropertyAttributeMap(propertyName, propertyAttributeMapBuilder.Build());
            }
        }

        return typeAttributeMap;
    }

    /// <summary>
    /// Gets the name of the property from the <paramref name="propertyExpression"/>.
    /// </summary>
    /// <param name="propertyExpression">Expression to select a property belonging to the <see cref="TypeAttributeMap.Type"/>.</param>
    /// <returns>The property name.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the expression is does not select a valid property.</exception>
    private static string GetPropertyName(Expression<Func<T, object?>> propertyExpression)
    {
        if (propertyExpression.Body is MemberExpression member)
        {
            return member.Member.Name;
        }

        if (propertyExpression.Body is UnaryExpression unary && unary.Operand is MemberExpression unaryMember)
        {
            return unaryMember.Member.Name;
        }

        throw new InvalidOperationException("Invalid property selector expression");
    }

    /// <summary>
    /// Gets the <see cref="PropertyInfo"/> from the <paramref name="propertyExpression"/>.
    /// </summary>
    /// <param name="propertyExpression">Expression to select a property belonging to the <see cref="TypeAttributeMap.Type"/>.</param>
    /// <returns>The <see cref="PropertyInfo"/> from the expression.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the expression does not relate to a property.</exception>
    private static PropertyInfo GetPropertyInfo(Expression<Func<T, object?>> propertyExpression)
    {
        MemberExpression? memberExpr = propertyExpression.Body as MemberExpression;
        if (memberExpr == null && propertyExpression.Body is UnaryExpression unary && unary.Operand is MemberExpression unaryMember)
        {
            memberExpr = unaryMember;
        }

        if (memberExpr == null)
        {
            throw new InvalidOperationException("Invalid property selector expression");
        }

        if (memberExpr.Member is PropertyInfo propertyInfo)
        {
            return propertyInfo;
        }

        throw new InvalidOperationException("The selected member is not a property.");
    }
}
