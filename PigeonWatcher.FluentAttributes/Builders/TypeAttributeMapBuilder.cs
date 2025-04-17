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
    /// Gets the <see cref="TypeAttributeMapBuilder{T}"/> instance from the <paramref name="expression"/>.
    /// </summary>
    /// <param name="expression">Expression to select a property belonging to the <see cref="TypeAttributeMap.Type"/>.</param>
    /// <returns>A <see cref="PropertyAttributeMapBuilder"/> instance.</returns>
    public PropertyAttributeMapBuilder Property(Expression<Func<T, object?>> expression)
    {
        MemberExpression memberExpression = ExpressionUtilities.GetMemberExpression(expression.Body);
        if (!ExpressionUtilities.IsPropertyAccess(memberExpression))
        {
            throw new InvalidOperationException("The selected member is not a property.");
        }

        string propertyName = memberExpression.Member.Name;
        if (!PropertyAttributeMapBuilders.TryGetValue(propertyName, out PropertyAttributeMapBuilder? propertyAttributeMapBuilder))
        {
            propertyAttributeMapBuilder = new PropertyAttributeMapBuilder(GetPropertyInfo(memberExpression));
            PropertyAttributeMapBuilders[propertyName] = propertyAttributeMapBuilder;
        }

        return propertyAttributeMapBuilder;
    }

    /// <summary>
    /// Builds the <see cref="TypeAttributeMap"/> instance.
    /// </summary>
    /// <returns>The built <see cref="TypeAttributeMap"/> instance.</returns>
    public TypeAttributeMap<T> Build()
    {
        TypeAttributeMap<T> typeAttributeMap = new();

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
    /// Gets the <see cref="PropertyInfo"/> from the <paramref name="memberExpression"/>.
    /// </summary>
    /// <param name="memberExpression">Expression to select a property belonging to the <see cref="TypeAttributeMap.Type"/>.</param>
    /// <returns>The <see cref="PropertyInfo"/> from the expression.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the expression does not relate to a property.</exception>
    private static PropertyInfo GetPropertyInfo(MemberExpression memberExpression)
    {
        if (memberExpression.Member is not PropertyInfo propertyInfo)
        {
            throw new InvalidOperationException("The selected member is not a property.");
        }

        return propertyInfo;
    }
}
