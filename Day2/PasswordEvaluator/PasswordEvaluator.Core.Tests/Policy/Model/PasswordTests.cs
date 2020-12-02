using System;
using Xunit;
using sut = PasswordEvaluator.Core.Policy.Model;
using PasswordEvaluator.Core.Policy;

namespace PasswordEvaluator.Core.Tests.Policy.Model
{
  /// <summary>
  /// Tests for the PasswordEvaluator.Core.Policy.Model.Password class
  /// </summary>
  public class PasswordTests
  {
    [Fact]
    public void PasswordGranularConstructor_GivenValidItems_SetsAppropriately()
    {
      var policy = new sut.CharacterFrequencyPasswordPolicy('a', 1, 2);
      var sut = new sut.Password(policy, "a");
      Assert.Equal(policy, sut.PasswordPolicy);
      Assert.Equal("a", sut.PasswordCandidate);
    }

    [Fact]
    public void PasswordMultiStringConstructor_GivenValidItems_SetsAppropriately()
    {
      var policyString = "5-9 b";
      var password = "abbbbbc";

      var sut = new sut.Password(policyString, password);
      var policy = new sut.CharacterFrequencyPasswordPolicy(policyString);

      Assert.Equal(policy, sut.PasswordPolicy);
      Assert.Equal(password, sut.PasswordCandidate);
    }

    [Fact]
    public void PasswordMultiStringAndFactoryConstructor_GivenValidItems_SetsAppropriately()
    {
      var policyString = "5-9 b";
      var password = "abbbbbc";
      var factory = new CharacterPositionPasswordPolicyFactory();

      var sut = new sut.Password(policyString, password, factory);
      var policy = new sut.CharacterPositionPasswordPolicy(policyString);
      var nonMatchingPolicy = new sut.CharacterFrequencyPasswordPolicy(policyString);
      Assert.Equal(policy, sut.PasswordPolicy);
      Assert.NotEqual(nonMatchingPolicy, sut.PasswordPolicy);
      Assert.Equal(password, sut.PasswordCandidate);
    }

    [Fact]
    public void PasswordSingleStringConstructor_GivenValidString_SetsAppropriately()
    {
      var policyString = "1-3 a";
      var passwordCandidate = "abcde";
      var policy = new sut.CharacterFrequencyPasswordPolicy(policyString);

      var policyPasswordString = string.Format("{0}: {1}", policyString, passwordCandidate);

      var sut = new sut.Password(policyPasswordString);

      Assert.Equal(policy, sut.PasswordPolicy);
      Assert.Equal(passwordCandidate, sut.PasswordCandidate);
      Assert.Equal(policyPasswordString, sut.ToString());
    }

    [Theory]
    [InlineData("1-3 a: abcde", true)]
    [InlineData("1-3 b: cdefg", false)]
    [InlineData("2-9 c: ccccccccc", true)]
    public void IsValid_GivenPasswordAndPolicy_ReturnsExpectedValue(string policyPasswordCombo, bool expectedValue)
    {
      var sut = new sut.Password(policyPasswordCombo);
      Assert.Equal(expectedValue, sut.IsValid);
    }
  }
}
