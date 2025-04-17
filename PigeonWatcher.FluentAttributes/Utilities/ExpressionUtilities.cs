using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PigeonWatcher.FluentAttributes.Utilities;

/// <summary>
/// Provides utility methods for expressions.
/// </summary>
public static class ExpressionUtilities
{
    /// <summary>
    /// Checks if the <paramref name="memberExpression"/> is a property access expression.
    /// </summary>
    /// <param name="memberExpression">The <see cref="MemberExpression"/> to check.</param>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="memberExpression"/> is a property access expression; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public static bool IsPropertyAccess(MemberExpression memberExpression)
    => memberExpression.Member is PropertyInfo;

    /// <summary>
    /// Converts the <paramref name="expression"/> to a <see cref="MemberExpression"/>.
    /// </summary>
    /// <param name="expression">The <see cref="Expression"/> to convert.</param>
    /// <returns>The converted <see cref="MemberExpression"/>.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if <paramref name="expression"/> cannot be converted to a <see cref="MemberExpression"/>.
    /// </exception>
    public static MemberExpression GetMemberExpression(Expression expression)
    {
        if (expression is MemberExpression memberExpression)
        {
            return memberExpression;
        }
        else if (expression is UnaryExpression unaryExpression && unaryExpression.Operand is MemberExpression innerMemberExpression)
        {
            return innerMemberExpression;
        }

        throw new InvalidOperationException("The expression cannot be converted to a member expression.");
    }
}
