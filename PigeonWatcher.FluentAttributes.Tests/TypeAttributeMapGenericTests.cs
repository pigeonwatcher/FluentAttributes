using System.Linq.Expressions;
using System.Reflection;

namespace PigeonWatcher.FluentAttributes.Tests
{
    public class TypeAttributeMapGenericTests
    {
        private class TestType
        {
            public int Property { get; set; }
            public string Field;
        }

        private class TestMemberAttributeMap : MemberAttributeMap
        {
            public TestMemberAttributeMap(MemberInfo memberInfo) : base(memberInfo) { }
        }

        private class AnotherMemberAttributeMap : MemberAttributeMap
        {
            public AnotherMemberAttributeMap(MemberInfo memberInfo) : base(memberInfo) { }
        }

        [Fact]
        public void Get_WithExpression_ShouldReturnMemberAttributeMap()
        {
            // Arrange
            var typeAttributeMap = new TypeAttributeMap<TestType>();
            var memberInfo = typeof(TestType).GetProperty(nameof(TestType.Property))!;
            var memberAttributeMap = new TestMemberAttributeMap(memberInfo);
            typeAttributeMap.Add(memberAttributeMap);

            // Act
            var result = typeAttributeMap.Get(x => x.Property);

            // Assert
            Assert.Equal(memberAttributeMap, result);
        }

        [Fact]
        public void Get_WithExpression_ShouldThrowKeyNotFoundExceptionIfMemberDoesNotExist()
        {
            // Arrange
            var typeAttributeMap = new TypeAttributeMap<TestType>();

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => typeAttributeMap.Get(x => x.Property));
        }

        [Fact]
        public void TryGet_WithExpression_ShouldReturnTrueAndOutMemberAttributeMapIfExists()
        {
            // Arrange
            var typeAttributeMap = new TypeAttributeMap<TestType>();
            var memberInfo = typeof(TestType).GetProperty(nameof(TestType.Property))!;
            var memberAttributeMap = new TestMemberAttributeMap(memberInfo);
            typeAttributeMap.Add(memberAttributeMap);

            // Act
            var result = typeAttributeMap.TryGet(x => x.Property, out var retrievedMemberAttributeMap);

            // Assert
            Assert.True(result);
            Assert.Equal(memberAttributeMap, retrievedMemberAttributeMap);
        }

        [Fact]
        public void TryGet_WithExpression_ShouldReturnFalseIfMemberDoesNotExist()
        {
            // Arrange
            var typeAttributeMap = new TypeAttributeMap<TestType>();

            // Act
            var result = typeAttributeMap.TryGet(x => x.Property, out var retrievedMemberAttributeMap);

            // Assert
            Assert.False(result);
            Assert.Null(retrievedMemberAttributeMap);
        }

        [Fact]
        public void GetGeneric_WithExpression_ShouldReturnTypedMemberAttributeMap()
        {
            // Arrange
            var typeAttributeMap = new TypeAttributeMap<TestType>();
            var memberInfo = typeof(TestType).GetProperty(nameof(TestType.Property))!;
            var memberAttributeMap = new TestMemberAttributeMap(memberInfo);
            typeAttributeMap.Add(memberAttributeMap);

            // Act
            var result = typeAttributeMap.Get<TestMemberAttributeMap>(x => x.Property);

            // Assert
            Assert.Equal(memberAttributeMap, result);
        }

        [Fact]
        public void GetGeneric_WithExpression_ShouldThrowInvalidCastExceptionIfTypeMismatch()
        {
            // Arrange
            var typeAttributeMap = new TypeAttributeMap<TestType>();
            var memberInfo = typeof(TestType).GetProperty(nameof(TestType.Property))!;
            var memberAttributeMap = new TestMemberAttributeMap(memberInfo);
            typeAttributeMap.Add(memberAttributeMap);

            // Act & Assert
            Assert.Throws<InvalidCastException>(() => typeAttributeMap.Get<AnotherMemberAttributeMap>(x => x.Property));
        }

        [Fact]
        public void TryGetGeneric_WithExpression_ShouldReturnTrueAndOutTypedMemberAttributeMapIfExists()
        {
            // Arrange
            var typeAttributeMap = new TypeAttributeMap<TestType>();
            var memberInfo = typeof(TestType).GetProperty(nameof(TestType.Property))!;
            var memberAttributeMap = new TestMemberAttributeMap(memberInfo);
            typeAttributeMap.Add(memberAttributeMap);

            // Act
            var result = typeAttributeMap.TryGet<TestMemberAttributeMap>(x => x.Property, out var retrievedMemberAttributeMap);

            // Assert
            Assert.True(result);
            Assert.Equal(memberAttributeMap, retrievedMemberAttributeMap);
        }

        [Fact]
        public void TryGetGeneric_WithExpression_ShouldReturnFalseIfTypeMismatch()
        {
            // Arrange
            var typeAttributeMap = new TypeAttributeMap<TestType>();
            var memberInfo = typeof(TestType).GetProperty(nameof(TestType.Property))!;
            var memberAttributeMap = new TestMemberAttributeMap(memberInfo);
            typeAttributeMap.Add(memberAttributeMap);

            // Act
            var result = typeAttributeMap.TryGet<AnotherMemberAttributeMap>(x => x.Property, out var retrievedMemberAttributeMap);

            // Assert
            Assert.False(result);
            Assert.Null(retrievedMemberAttributeMap);
        }
    }
}
