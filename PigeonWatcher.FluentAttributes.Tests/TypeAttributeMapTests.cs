using PigeonWatcher.FluentAttributes;
using PigeonWatcher.FluentAttributes.Utilities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Xunit;

namespace PigeonWatcher.FluentAttributes.Tests
{
    public class TypeAttributeMapTests
    {
        private class TestMemberAttributeMap : MemberAttributeMap
        {
            public TestMemberAttributeMap(MemberInfo memberInfo) : base(memberInfo) { }
        }

        private class AnotherMemberAttributeMap : MemberAttributeMap
        {
            public AnotherMemberAttributeMap(MemberInfo memberInfo) : base(memberInfo) { }
        }

        private class TestTypeAttributeMap : TypeAttributeMap
        {
            public TestTypeAttributeMap(Type type) : base(type) { }
        }

        [Fact]
        public void Add_ShouldAddMemberAttributeMap()
        {
            // Arrange
            var type = typeof(string);
            var typeAttributeMap = new TestTypeAttributeMap(type);
            var memberInfo = type.GetProperty("Length")!;
            var memberAttributeMap = new TestMemberAttributeMap(memberInfo);

            // Act
            var result = typeAttributeMap.Add(memberAttributeMap);

            // Assert
            Assert.True(result);
            Assert.Contains(memberAttributeMap, typeAttributeMap.MemberAttributeMaps);
        }

        [Fact]
        public void Add_ShouldReturnFalseIfMemberAlreadyExists()
        {
            // Arrange
            var type = typeof(string);
            var typeAttributeMap = new TestTypeAttributeMap(type);
            var memberInfo = type.GetProperty("Length")!;
            var memberAttributeMap = new TestMemberAttributeMap(memberInfo);
            typeAttributeMap.Add(memberAttributeMap);

            // Act
            var result = typeAttributeMap.Add(memberAttributeMap);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Get_ShouldReturnMemberAttributeMap()
        {
            // Arrange
            var type = typeof(string);
            var typeAttributeMap = new TestTypeAttributeMap(type);
            var memberInfo = type.GetProperty("Length")!;
            var memberAttributeMap = new TestMemberAttributeMap(memberInfo);
            typeAttributeMap.Add(memberAttributeMap);

            // Act
            var result = typeAttributeMap.Get("Length");

            // Assert
            Assert.Equal(memberAttributeMap, result);
        }

        [Fact]
        public void Get_ShouldThrowKeyNotFoundExceptionIfMemberDoesNotExist()
        {
            // Arrange
            var type = typeof(string);
            var typeAttributeMap = new TestTypeAttributeMap(type);

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => typeAttributeMap.Get("NonExistentMember"));
        }

        [Fact]
        public void TryGet_ShouldReturnTrueAndOutMemberAttributeMapIfExists()
        {
            // Arrange
            var type = typeof(string);
            var typeAttributeMap = new TestTypeAttributeMap(type);
            var memberInfo = type.GetProperty("Length")!;
            var memberAttributeMap = new TestMemberAttributeMap(memberInfo);
            typeAttributeMap.Add(memberAttributeMap);

            // Act
            var result = typeAttributeMap.TryGet("Length", out var retrievedMemberAttributeMap);

            // Assert
            Assert.True(result);
            Assert.Equal(memberAttributeMap, retrievedMemberAttributeMap);
        }

        [Fact]
        public void TryGet_ShouldReturnFalseIfMemberDoesNotExist()
        {
            // Arrange
            var type = typeof(string);
            var typeAttributeMap = new TestTypeAttributeMap(type);

            // Act
            var result = typeAttributeMap.TryGet("NonExistentMember", out var retrievedMemberAttributeMap);

            // Assert
            Assert.False(result);
            Assert.Null(retrievedMemberAttributeMap);
        }

        [Fact]
        public void GetGeneric_ShouldReturnTypedMemberAttributeMap()
        {
            // Arrange
            var type = typeof(string);
            var typeAttributeMap = new TestTypeAttributeMap(type);
            var memberInfo = type.GetProperty("Length")!;
            var memberAttributeMap = new TestMemberAttributeMap(memberInfo);
            typeAttributeMap.Add(memberAttributeMap);

            // Act
            var result = typeAttributeMap.Get<TestMemberAttributeMap>("Length");

            // Assert
            Assert.Equal(memberAttributeMap, result);
        }

        [Fact]
        public void GetGeneric_ShouldThrowInvalidCastExceptionIfTypeMismatch()
        {
            // Arrange
            var type = typeof(string);
            var typeAttributeMap = new TestTypeAttributeMap(type);
            var memberInfo = type.GetProperty("Length")!;
            var memberAttributeMap = new TestMemberAttributeMap(memberInfo);
            typeAttributeMap.Add(memberAttributeMap);

            // Act & Assert
            Assert.Throws<InvalidCastException>(() => typeAttributeMap.Get<AnotherMemberAttributeMap>("Length"));
        }

        [Fact]
        public void TryGetGeneric_ShouldReturnTrueAndOutTypedMemberAttributeMapIfExists()
        {
            // Arrange
            var type = typeof(string);
            var typeAttributeMap = new TestTypeAttributeMap(type);
            var memberInfo = type.GetProperty("Length")!;
            var memberAttributeMap = new TestMemberAttributeMap(memberInfo);
            typeAttributeMap.Add(memberAttributeMap);

            // Act
            var result = typeAttributeMap.TryGet<TestMemberAttributeMap>("Length", out var retrievedMemberAttributeMap);

            // Assert
            Assert.True(result);
            Assert.Equal(memberAttributeMap, retrievedMemberAttributeMap);
        }

        [Fact]
        public void TryGetGeneric_ShouldReturnFalseIfTypeMismatch()
        {
            // Arrange
            var type = typeof(string);
            var typeAttributeMap = new TestTypeAttributeMap(type);
            var memberInfo = type.GetProperty("Length")!;
            var memberAttributeMap = new TestMemberAttributeMap(memberInfo);
            typeAttributeMap.Add(memberAttributeMap);

            // Act
            var result = typeAttributeMap.TryGet<AnotherMemberAttributeMap>("Length", out var retrievedMemberAttributeMap);

            // Assert
            Assert.False(result);
            Assert.Null(retrievedMemberAttributeMap);
        }
    }
}
