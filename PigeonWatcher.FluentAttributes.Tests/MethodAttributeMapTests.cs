using System;
using System.Reflection;
using Xunit;

namespace PigeonWatcher.FluentAttributes.Tests;

public class MethodAttributeMapTests
{
    [Fact]
    public void Constructor_ShouldInitializeMethodInfo()
    {
        // Arrange
        MethodInfo methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.TestMethod))!;

        // Act
        MethodAttributeMap map = new(methodInfo);

        // Assert
        Assert.NotNull(map.MethodInfo);
        Assert.Equal(methodInfo, map.MethodInfo);
    }

    private class TestClass
    {
        public void TestMethod()
        {
        }
    }
}