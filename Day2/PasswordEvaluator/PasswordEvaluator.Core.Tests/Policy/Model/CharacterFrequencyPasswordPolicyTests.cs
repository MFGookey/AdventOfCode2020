using System;
using Xunit;
using sut = PasswordEvaluator.Core.Policy.Model;

namespace PasswordEvaluator.Core.Tests.Policy.Model
{
  /// <summary>
  /// Tests for the PasswordEvaluator.Core.Policy.Model.CharacterFrequencyPasswordPolicy class
  /// </summary>
  public class CharacterFrequencyPasswordPolicyTests
  {
    [Fact]
    public void CharacterFrequencyPasswordPolicyConstructor_LowerNumberLessThanZero_ThrowsException()
    {
      var exception = Assert.Throws<ArgumentException>(() => { new sut.CharacterFrequencyPasswordPolicy('a', -1, 0); });

      Assert.Contains("lowerNumber", exception.Message);
      Assert.Contains("less than 0", exception.Message);
    }

    [Fact]
    public void CharacterFrequencyPasswordPolicyConstructor_UpperNumberLessThanMinimumInstances_ThrowsException()
    {
      var exception = Assert.Throws<ArgumentException>(() => { new sut.CharacterFrequencyPasswordPolicy('a', 1, 0); });

      Assert.Contains("upperNumber", exception.Message);
      Assert.Contains("less than lowerNumber", exception.Message);
    }

    [Theory]
    [InlineData("sadfg")]
    [InlineData("a 0-1")]
    [InlineData("1-2 a:")]
    [InlineData(" ")]
    [InlineData("a-b a")]
    public void CharacterFrequencyPasswordPolicyStringConstructor_PasswordPolicyStringDoesNotMatchFormat_ThrowsException(string passwordPolicyString)
    {
      var exception = Assert.Throws<ArgumentException>(() => { new sut.CharacterFrequencyPasswordPolicy(passwordPolicyString); });

      Assert.Contains("passwordPolicyString", exception.Message);
      Assert.Contains("\"{lowerNumber}-{upperNumber} {requiredCharacter}\"", exception.Message);
    }

    [Fact]
    public void CharacterFrequencyPasswordPolicyStringConstructor_UpperNumberLessThanLowerNumber_ThrowsException()
    {
      var exception = Assert.Throws<ArgumentException>(() => { new sut.CharacterFrequencyPasswordPolicy("2-1 a"); });

      Assert.Contains("upperNumber", exception.Message);
      Assert.Contains("less than lowerNumber", exception.Message);
    }

    [Fact]
    public void CharacterFrequencyPasswordPolicyConstructor_ValidPolicy_SetsPropertiesCorrectly()
    {
      var sut = new sut.CharacterFrequencyPasswordPolicy('a', 10, 20);
      Assert.Equal('a', sut.RequiredCharacter);
      Assert.Equal(10, sut.LowerNumber);
      Assert.Equal(20, sut.UpperNumber);
    }

    [Fact]
    public void CharacterFrequencyPasswordPolicyStringConstructor_ValidPolicy_ParsesAndSetsPropertiesCorrectly()
    {
      var sut = new sut.CharacterFrequencyPasswordPolicy("2-9 g");
      Assert.Equal('g', sut.RequiredCharacter);
      Assert.Equal(2, sut.LowerNumber);
      Assert.Equal(9, sut.UpperNumber);
    }

    [Theory]
    [InlineData("1-3 a", "abcde")]
    [InlineData("2-9 c", "ccccccccc")]
    public void Validate_GivenValidPassword_ReturnsTrue(string passwordPolicyString, string password)
    {
      var sut = new sut.CharacterFrequencyPasswordPolicy(passwordPolicyString);
      Assert.True(sut.Validate(password));
    }

    [Theory]
    [InlineData("1-3 b", "cdefg")]
    public void Validate_GivenInvalidPassword_ReturnsFalse(string passwordPolicyString, string password)
    {
      var sut = new sut.CharacterFrequencyPasswordPolicy(passwordPolicyString);
      Assert.False(sut.Validate(password));
    }
  }
}
