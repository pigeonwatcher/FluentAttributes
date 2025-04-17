using PigeonWatcher.FluentAttributes.Utilities;
using System;
using System.Linq.Expressions;
using System.Reflection;
using Xunit;

namespace PigeonWatcher.FluentAttributes.Tests.Utilities
{
    public class ExpressionUtilitiesTests
    {
        private class TestClass
        {
            public string TestProperty { get; set; } = string.Empty;
            public int TestField;
        }

        [Fact]
        public void IsPropertyAccess_ShouldReturnTrue_ForPropertyAccess()
        {
            // Arrange
            Expression<Func<TestClass, string>> expression = x => x.TestProperty;
            var memberExpression = (MemberExpression)expression.Body;

            // Act
            var result = ExpressionUtilities.IsPropertyAccess(memberExpression);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsPropertyAccess_ShouldReturnFalse_ForFieldAccess()
        {
            // Arrange
            Expression<Func<TestClass, int>> expression = x => x.TestField;
            var memberExpression = (MemberExpression)expression.Body;

            // Act
            var result = ExpressionUtilities.IsPropertyAccess(memberExpression);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetMemberExpression_ShouldReturnMemberExpression_ForDirectPropertyAccess()
        {
            // Arrange
            Expression<Func<TestClass, string>> expression = x => x.TestProperty;

            // Act
            var result = ExpressionUtilities.GetMemberExpression(expression.Body);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.GetType().IsSubclassOf(typeof(MemberExpression)));
            Assert.Equal(nameof(TestClass.TestProperty), result.Member.Name);
        }

        [Fact]
        public void GetMemberExpression_ShouldReturnMemberExpression_ForUnaryExpression()
        {
            // Arrange
            Expression<Func<TestClass, object>> expression = x => x.TestProperty;

            // Act
            var result = ExpressionUtilities.GetMemberExpression(expression.Body);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.GetType().IsSubclassOf(typeof(MemberExpression)));
            Assert.Equal(nameof(TestClass.TestProperty), result.Member.Name);
        }

        [Fact]
        public void GetMemberExpression_ShouldThrowInvalidOperationException_ForNonMemberExpression()
        {
            // Arrange
            Expression<Func<TestClass, int>> expression = x => x.TestField + 1;

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => ExpressionUtilities.GetMemberExpression(expression.Body));
        }
    }
}
