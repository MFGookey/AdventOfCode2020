using System;
using Xunit;
using sut = PasswordEvaluator.Core.Policy;
using PasswordEvaluator.Core.Policy.Model;

namespace PasswordEvaluator.Core.Tests.Policy
{
  /// <summary>
  /// Tests for the PasswordEvaluator.Core.Policy.CharacterPositionPasswordPolicyFactory class
  /// </summary>
  public class CharacterPositionPasswordPolicyFactoryTests
  {
    [Fact]
    public void CreatePolicy_LowerNumberLessThanZero_ThrowsException()
    {
      var sut = new sut.CharacterPositionPasswordPolicyFactory();

      var exception = Assert.Throws<ArgumentException>(() => { sut.CreatePolicy('a', -1, 0); });

      Assert.Contains("lowerNumber", exception.Message);
      Assert.Contains("less than 0", exception.Message);
    }

    [Fact]
    public void CreatePolicy_UpperNumberLessThanLowerNumber_ThrowsException()
    {
      var sut = new sut.CharacterPositionPasswordPolicyFactory();

      var exception = Assert.Throws<ArgumentException>(() => { sut.CreatePolicy('a', 1, 0); });
      
      Assert.Contains("upperNumber", exception.Message);
      Assert.Contains("less than lowerNumber", exception.Message);
    }

    [Theory]
    [InlineData("sadfg")]
    [InlineData("a 0-1")]
    [InlineData("1-2 a:")]
    [InlineData(" ")]
    [InlineData("a-b a")]
    public void CreatePolicy_PasswordPolicyStringDoesNotMatchFormat_ThrowsException(string passwordPolicyString)
    {
      var sut = new sut.CharacterPositionPasswordPolicyFactory();
      var exception = Assert.Throws<ArgumentException>(() => { sut.CreatePolicy(passwordPolicyString); });

      Assert.Contains("passwordPolicyString", exception.Message);
      Assert.Contains("\"{lowerNumber}-{upperNumber} {requiredCharacter}\"", exception.Message);
    }

    [Fact]
    public void CreatePolicyFromString_UpperNumberLessThanLowerNumber_ThrowsException()
    {
      var sut = new sut.CharacterPositionPasswordPolicyFactory();
      var exception = Assert.Throws<ArgumentException>(() => { sut.CreatePolicy("2-1 a"); });

      Assert.Contains("upperNumber", exception.Message);
      Assert.Contains("less than lowerNumber", exception.Message);
    }

    [Fact]
    public void CreatePolicy_ValidPolicy_SetsPropertiesCorrectly()
    {
      var sut = new sut.CharacterPositionPasswordPolicyFactory();
      var result = sut.CreatePolicy('a', 10, 20);
      Assert.IsType<CharacterPositionPasswordPolicy>(result);
      Assert.Equal('a', result.RequiredCharacter);
      Assert.Equal(10, result.LowerNumber);
      Assert.Equal(20, result.UpperNumber);
    }

    [Fact]
    public void CreatePolicyFromString_ValidPolicy_ParsesAndSetsPropertiesCorrectly()
    {
      var sut = new sut.CharacterPositionPasswordPolicyFactory();
      var result = sut.CreatePolicy("2-9 g");
      Assert.IsType<CharacterPositionPasswordPolicy>(result);
      Assert.Equal('g', result.RequiredCharacter);
      Assert.Equal(2, result.LowerNumber);
      Assert.Equal(9, result.UpperNumber);
    }
  }
}
