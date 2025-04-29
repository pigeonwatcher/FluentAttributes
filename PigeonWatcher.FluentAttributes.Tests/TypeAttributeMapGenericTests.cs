using System.Linq.Expressions;
using System.Reflection;

namespace PigeonWatcher.FluentAttributes.Tests;

public class TypeAttributeMapGenericTests
{
    [Fact]
    public void Get_WithExpression_ShouldReturnMemberAttributeMap()
    {
        // Arrange
        TypeAttributeMap<TestType> typeAttributeMap = new();
        PropertyInfo memberInfo = typeof(TestType).GetProperty(nameof(TestType.Property))!;
        TestMemberAttributeMap memberAttributeMap = new(memberInfo);
        typeAttributeMap.Add(memberAttributeMap);

        // Act
        MemberAttributeMap result = typeAttributeMap.Get(x => x.Property);

        // Assert
        Assert.Equal(memberAttributeMap, result);
    }

    [Fact]
    public void Get_WithExpression_ShouldThrowKeyNotFoundExceptionIfMemberDoesNotExist()
    {
        // Arrange
        TypeAttributeMap<TestType> typeAttributeMap = new();

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => typeAttributeMap.Get(x => x.Property));
    }

    [Fact]
    public void TryGet_WithExpression_ShouldReturnTrueAndOutMemberAttributeMapIfExists()
    {
        // Arrange
        TypeAttributeMap<TestType> typeAttributeMap = new();
        PropertyInfo memberInfo = typeof(TestType).GetProperty(nameof(TestType.Property))!;
        TestMemberAttributeMap memberAttributeMap = new(memberInfo);
        typeAttributeMap.Add(memberAttributeMap);

        // Act
        bool result = typeAttributeMap.TryGet(x => x.Property, out MemberAttributeMap? retrievedMemberAttributeMap);

        // Assert
        Assert.True(result);
        Assert.Equal(memberAttributeMap, retrievedMemberAttributeMap);
    }

    [Fact]
    public void TryGet_WithExpression_ShouldReturnFalseIfMemberDoesNotExist()
    {
        // Arrange
        TypeAttributeMap<TestType> typeAttributeMap = new();

        // Act
        bool result = typeAttributeMap.TryGet(x => x.Property, out MemberAttributeMap? retrievedMemberAttributeMap);

        // Assert
        Assert.False(result);
        Assert.Null(retrievedMemberAttributeMap);
    }

    [Fact]
    public void GetGeneric_WithExpression_ShouldReturnTypedMemberAttributeMap()
    {
        // Arrange
        TypeAttributeMap<TestType> typeAttributeMap = new();
        PropertyInfo memberInfo = typeof(TestType).GetProperty(nameof(TestType.Property))!;
        TestMemberAttributeMap memberAttributeMap = new(memberInfo);
        typeAttributeMap.Add(memberAttributeMap);

        // Act
        TestMemberAttributeMap result = typeAttributeMap.Get<TestMemberAttributeMap>(x => x.Property);

        // Assert
        Assert.Equal(memberAttributeMap, result);
    }

    [Fact]
    public void GetGeneric_WithExpression_ShouldThrowInvalidCastExceptionIfTypeMismatch()
    {
        // Arrange
        TypeAttributeMap<TestType> typeAttributeMap = new();
        PropertyInfo memberInfo = typeof(TestType).GetProperty(nameof(TestType.Property))!;
        TestMemberAttributeMap memberAttributeMap = new(memberInfo);
        typeAttributeMap.Add(memberAttributeMap);

        // Act & Assert
        Assert.Throws<InvalidCastException>(() => typeAttributeMap.Get<AnotherMemberAttributeMap>(x => x.Property));
    }

    [Fact]
    public void TryGetGeneric_WithExpression_ShouldReturnTrueAndOutTypedMemberAttributeMapIfExists()
    {
        // Arrange
        TypeAttributeMap<TestType> typeAttributeMap = new();
        PropertyInfo memberInfo = typeof(TestType).GetProperty(nameof(TestType.Property))!;
        TestMemberAttributeMap memberAttributeMap = new(memberInfo);
        typeAttributeMap.Add(memberAttributeMap);

        // Act
        bool result = typeAttributeMap.TryGet<TestMemberAttributeMap>(x => x.Property, out TestMemberAttributeMap? retrievedMemberAttributeMap);

        // Assert
        Assert.True(result);
        Assert.Equal(memberAttributeMap, retrievedMemberAttributeMap);
    }

    [Fact]
    public void TryGetGeneric_WithExpression_ShouldReturnFalseIfTypeMismatch()
    {
        // Arrange
        TypeAttributeMap<TestType> typeAttributeMap = new();
        PropertyInfo memberInfo = typeof(TestType).GetProperty(nameof(TestType.Property))!;
        TestMemberAttributeMap memberAttributeMap = new(memberInfo);
        typeAttributeMap.Add(memberAttributeMap);

        // Act
        bool result = typeAttributeMap.TryGet<AnotherMemberAttributeMap>(x => x.Property, out AnotherMemberAttributeMap? retrievedMemberAttributeMap);

        // Assert
        Assert.False(result);
        Assert.Null(retrievedMemberAttributeMap);
    }

    private class TestType
    {
        public string Field;
        public int Property { get; set; }
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
}