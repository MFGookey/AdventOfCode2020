using System;
using Xunit;
using sut = PasswordEvaluator.Core.Policy.Model;

namespace PasswordEvaluator.Core.Tests.Policy.Model
{
  /// <summary>
  /// Tests for the PasswordEvaluator.Core.Policy.Model.PasswordPolicy class
  /// </summary>
  public class PasswordPolicyTests
  {
    [Fact]
    public void PasswordPolicyConstructor_MinimumInstancesLessThanZero_ThrowsException()
    {
      var exception = Assert.Throws<ArgumentException>(() => { new sut.PasswordPolicy('a', -1, 0); });

      Assert.Contains("minimumInstances", exception.Message);
      Assert.Contains("less than 0", exception.Message);
    }

    [Fact]
    public void PasswordPolicyConstructor_MaximumInstancesLessThanMinimumInstances_ThrowsException()
    {
      var exception = Assert.Throws<ArgumentException>(() => { new sut.PasswordPolicy('a', 1, 0); });

      Assert.Contains("maximumInstances", exception.Message);
      Assert.Contains("less than minimumInstances", exception.Message);
    }

    [Theory]
    [InlineData("sadfg")]
    [InlineData("a 0-1")]
    [InlineData("1-2 a:")]
    [InlineData(" ")]
    [InlineData("a-b a")]
    public void PasswordPolicyStringConstructor_PasswordPolicyStringDoesNotMatchFormat_ThrowsException(string passwordPolicyString)
    {
      var exception = Assert.Throws<ArgumentException>(() => { new sut.PasswordPolicy(passwordPolicyString); });

      Assert.Contains("passwordPolicyString", exception.Message);
      Assert.Contains("\"{minimumInstances}-{maximumInstances} {requiredCharacter}\"", exception.Message);
    }

    [Fact]
    public void PasswordPolicyStringConstructor_MaximumInstancesLessThanMinimumInstances_ThrowsException()
    {
      var exception = Assert.Throws<ArgumentException>(() => { new sut.PasswordPolicy("2-1 a"); });

      Assert.Contains("maximumInstances", exception.Message);
      Assert.Contains("less than minimumInstances", exception.Message);
    }

    [Fact]
    public void PasswordPolicyConstructor_ValidPolicy_SetsPropertiesCorrectly()
    {
      var sut = new sut.PasswordPolicy('a', 10, 20);
      Assert.Equal('a', sut.RequiredCharacter);
      Assert.Equal(10, sut.MinimumInstances);
      Assert.Equal(20, sut.MaximumInstances);
    }

    [Fact]
    public void PasswordPolicyStringConstructor_ValidPolicy_ParsesAndSetsPropertiesCorrectly()
    {
      var sut = new sut.PasswordPolicy("2-9 g");
      Assert.Equal('g', sut.RequiredCharacter);
      Assert.Equal(2, sut.MinimumInstances);
      Assert.Equal(9, sut.MaximumInstances);
    }

    [Theory]
    [InlineData("1-3 a", "abcde")]
    [InlineData("2-9 c", "ccccccccc")]
    public void Validate_GivenValidPassword_ReturnsTrue(string passwordPolicyString, string password)
    {
      var sut = new sut.PasswordPolicy(passwordPolicyString);
      Assert.True(sut.Validate(password));
    }

    [Theory]
    [InlineData("1-3 b", "cdefg")]
    public void Validate_GivenInvalidPassword_ReturnsFalse(string passwordPolicyString, string password)
    {
      var sut = new sut.PasswordPolicy(passwordPolicyString);
      Assert.False(sut.Validate(password));
    }
  }
}
