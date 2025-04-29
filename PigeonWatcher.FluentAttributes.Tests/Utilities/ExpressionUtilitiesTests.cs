using PigeonWatcher.FluentAttributes.Utilities;
using System;
using System.Linq.Expressions;
using System.Reflection;
using Xunit;

namespace PigeonWatcher.FluentAttributes.Tests.Utilities;

public class ExpressionUtilitiesTests
{
    [Fact]
    public void GetMemberInfo_NullExpression_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ExpressionUtilities.GetMemberInfo(null));
    }

    [Fact]
    public void GetMemberInfo_MemberExpression_ReturnsMemberInfo()
    {
        // Arrange - lambda accessing a property.
        Expression<Func<TestClass, int>> expr = d => d.Value;

        // Act
        MemberInfo memberInfo = ExpressionUtilities.GetMemberInfo(expr);

        // Assert
        Assert.Equal(typeof(TestClass).GetProperty(nameof(TestClass.Value)), memberInfo);
    }

    [Fact]
    public void GetMemberInfo_MethodCallExpression_ReturnsMethodInfo()
    {
        // Arrange - lambda calling a method.
        Expression<Func<TestClass, int>> expr = d => d.GetValue();

        // Act
        MemberInfo memberInfo = ExpressionUtilities.GetMemberInfo(expr);

        // Assert
        Assert.Equal(typeof(TestClass).GetMethod(nameof(TestClass.GetValue)), memberInfo);
    }

    [Fact]
    public void GetMemberInfo_InvalidExpression_ThrowsArgumentException()
    {
        // Arrange - lambda that doesn't represent a property or method call.
        Expression<Func<TestClass, int>> expr = d => 42;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => ExpressionUtilities.GetMemberInfo(expr));
    }

    [Fact]
    public void GetMemberInfo_UnaryExpression_UnwrapsAndReturnsMemberInfo()
    {
        // Arrange
        Expression<Func<TestClass, object>> expr = d => d.Value;

        // Act
        MemberInfo memberInfo = ExpressionUtilities.GetMemberInfo(expr);

        // Assert
        Assert.Equal(typeof(TestClass).GetProperty(nameof(TestClass.Value)), memberInfo);
    }

    private class TestClass
    {
        public int Value { get; set; }

        public int GetValue()
        {
            return Value;
        }
    }
}