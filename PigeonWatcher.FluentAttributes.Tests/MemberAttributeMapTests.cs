using System;
using System.Reflection;
using Xunit;

namespace PigeonWatcher.FluentAttributes.Tests;

public class MemberAttributeMapTests
{
    [Fact]
    public void Constructor_ShouldInitializeMemberInfo()
    {
        // Arrange
        PropertyInfo memberInfo = typeof(TestClass).GetProperty(nameof(TestClass.TestProperty))!;

        // Act
        TestMemberAttributeMap map = new(memberInfo);

        // Assert
        Assert.NotNull(map.MemberInfo);
        Assert.Equal(memberInfo, map.MemberInfo);
    }

    private class TestMemberAttributeMap : MemberAttributeMap
    {
        public TestMemberAttributeMap(MemberInfo memberInfo) : base(memberInfo)
        {
        }
    }

    private class TestClass
    {
        public int TestProperty { get; set; }
    }
}