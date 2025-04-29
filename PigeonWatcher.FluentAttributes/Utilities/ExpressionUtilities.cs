using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PigeonWatcher.FluentAttributes.Utilities;

/// <summary>
/// Provides utility methods for expressions.
/// </summary>
public static class ExpressionUtilities
{
    /// <summary>
    /// Extracts the <see cref="MethodInfo"/> from the <paramref name="expression"/>.
    /// </summary>
    /// <param name="expression">A <see cref="LambdaExpression"/> that accesses a member.</param>
    /// <returns>The <see cref="MethodInfo"/> referred to in the <paramref name="expression"/>.</returns>
    public static MemberInfo GetMemberInfo(LambdaExpression expression)
    {
        if (expression == null)
        {
            throw new ArgumentNullException(nameof(expression));
        }

        Expression body = expression.Body;
        if (body is UnaryExpression unaryExpression && unaryExpression.NodeType == ExpressionType.Convert)
        {
            body = unaryExpression.Operand;
        }

        switch (body)
        {
            case MemberExpression memberExpression:
                return memberExpression.Member;
            case MethodCallExpression methodCallExpression:
                return methodCallExpression.Method;
            default:
                throw new ArgumentException("Expression must be a simple member access (field/property) or method call.", nameof(expression));
        }
    }
}