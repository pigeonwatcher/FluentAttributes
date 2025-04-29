using System;
using System.Reflection;
using Xunit;

namespace PigeonWatcher.FluentAttributes.Tests;

public class FieldAttributeMapTests
{
    [Fact]
    public void Constructor_ShouldInitializeFieldInfo()
    {
        // Arrange
        FieldInfo fieldInfo = typeof(TestClass).GetField(nameof(TestClass.TestField))!;

        // Act
        FieldAttributeMap map = new(fieldInfo);

        // Assert
        Assert.NotNull(map.FieldInfo);
        Assert.Equal(fieldInfo, map.FieldInfo);
    }

    private class TestClass
    {
        public int TestField;
    }
}