using PigeonWatcher.FluentAttributes;
using PigeonWatcher.FluentAttributes.Utilities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Xunit;

namespace PigeonWatcher.FluentAttributes.Tests;

public class TypeAttributeMapTests
{
    [Fact]
    public void Add_ShouldAddMemberAttributeMap()
    {
        // Arrange
        Type type = typeof(string);
        TestTypeAttributeMap typeAttributeMap = new(type);
        PropertyInfo memberInfo = type.GetProperty("Length")!;
        TestMemberAttributeMap memberAttributeMap = new(memberInfo);

        // Act
        bool result = typeAttributeMap.Add(memberAttributeMap);

        // Assert
        Assert.True(result);
        Assert.Contains(memberAttributeMap, typeAttributeMap.MemberAttributeMaps);
    }

    [Fact]
    public void Add_ShouldReturnFalseIfMemberAlreadyExists()
    {
        // Arrange
        Type type = typeof(string);
        TestTypeAttributeMap typeAttributeMap = new(type);
        PropertyInfo memberInfo = type.GetProperty("Length")!;
        TestMemberAttributeMap memberAttributeMap = new(memberInfo);
        typeAttributeMap.Add(memberAttributeMap);

        // Act
        bool result = typeAttributeMap.Add(memberAttributeMap);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Get_ShouldReturnMemberAttributeMap()
    {
        // Arrange
        Type type = typeof(string);
        TestTypeAttributeMap typeAttributeMap = new(type);
        PropertyInfo memberInfo = type.GetProperty("Length")!;
        TestMemberAttributeMap memberAttributeMap = new(memberInfo);
        typeAttributeMap.Add(memberAttributeMap);

        // Act
        MemberAttributeMap result = typeAttributeMap.Get("Length");

        // Assert
        Assert.Equal(memberAttributeMap, result);
    }

    [Fact]
    public void Get_ShouldThrowKeyNotFoundExceptionIfMemberDoesNotExist()
    {
        // Arrange
        Type type = typeof(string);
        TestTypeAttributeMap typeAttributeMap = new(type);

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => typeAttributeMap.Get("NonExistentMember"));
    }

    [Fact]
    public void TryGet_ShouldReturnTrueAndOutMemberAttributeMapIfExists()
    {
        // Arrange
        Type type = typeof(string);
        TestTypeAttributeMap typeAttributeMap = new(type);
        PropertyInfo memberInfo = type.GetProperty("Length")!;
        TestMemberAttributeMap memberAttributeMap = new(memberInfo);
        typeAttributeMap.Add(memberAttributeMap);

        // Act
        bool result = typeAttributeMap.TryGet("Length", out MemberAttributeMap? retrievedMemberAttributeMap);

        // Assert
        Assert.True(result);
        Assert.Equal(memberAttributeMap, retrievedMemberAttributeMap);
    }

    [Fact]
    public void TryGet_ShouldReturnFalseIfMemberDoesNotExist()
    {
        // Arrange
        Type type = typeof(string);
        TestTypeAttributeMap typeAttributeMap = new(type);

        // Act
        bool result = typeAttributeMap.TryGet("NonExistentMember", out MemberAttributeMap? retrievedMemberAttributeMap);

        // Assert
        Assert.False(result);
        Assert.Null(retrievedMemberAttributeMap);
    }

    [Fact]
    public void GetGeneric_ShouldReturnTypedMemberAttributeMap()
    {
        // Arrange
        Type type = typeof(string);
        TestTypeAttributeMap typeAttributeMap = new(type);
        PropertyInfo memberInfo = type.GetProperty("Length")!;
        TestMemberAttributeMap memberAttributeMap = new(memberInfo);
        typeAttributeMap.Add(memberAttributeMap);

        // Act
        TestMemberAttributeMap result = typeAttributeMap.Get<TestMemberAttributeMap>("Length");

        // Assert
        Assert.Equal(memberAttributeMap, result);
    }

    [Fact]
    public void GetGeneric_ShouldThrowInvalidCastExceptionIfTypeMismatch()
    {
        // Arrange
        Type type = typeof(string);
        TestTypeAttributeMap typeAttributeMap = new(type);
        PropertyInfo memberInfo = type.GetProperty("Length")!;
        TestMemberAttributeMap memberAttributeMap = new(memberInfo);
        typeAttributeMap.Add(memberAttributeMap);

        // Act & Assert
        Assert.Throws<InvalidCastException>(() => typeAttributeMap.Get<AnotherMemberAttributeMap>("Length"));
    }

    [Fact]
    public void TryGetGeneric_ShouldReturnTrueAndOutTypedMemberAttributeMapIfExists()
    {
        // Arrange
        Type type = typeof(string);
        TestTypeAttributeMap typeAttributeMap = new(type);
        PropertyInfo memberInfo = type.GetProperty("Length")!;
        TestMemberAttributeMap memberAttributeMap = new(memberInfo);
        typeAttributeMap.Add(memberAttributeMap);

        // Act
        bool result = typeAttributeMap.TryGet<TestMemberAttributeMap>("Length", out TestMemberAttributeMap? retrievedMemberAttributeMap);

        // Assert
        Assert.True(result);
        Assert.Equal(memberAttributeMap, retrievedMemberAttributeMap);
    }

    [Fact]
    public void TryGetGeneric_ShouldReturnFalseIfTypeMismatch()
    {
        // Arrange
        Type type = typeof(string);
        TestTypeAttributeMap typeAttributeMap = new(type);
        PropertyInfo memberInfo = type.GetProperty("Length")!;
        TestMemberAttributeMap memberAttributeMap = new(memberInfo);
        typeAttributeMap.Add(memberAttributeMap);

        // Act
        bool result = typeAttributeMap.TryGet<AnotherMemberAttributeMap>("Length", out AnotherMemberAttributeMap? retrievedMemberAttributeMap);

        // Assert
        Assert.False(result);
        Assert.Null(retrievedMemberAttributeMap);
    }

    private class TestMemberAttributeMap : MemberAttributeMap
    {
        public TestMemberAttributeMap(MemberInfo memberInfo) : base(memberInfo)
        {
        }
    }

    private class AnotherMemberAttributeMap : MemberAttributeMap
    {
        public AnotherMemberAttributeMap(MemberInfo memberInfo) : base(memberInfo)
        {
        }
    }

    private class TestTypeAttributeMap : TypeAttributeMap
    {
        public TestTypeAttributeMap(Type type) : base(type)
        {
        }
    }
}