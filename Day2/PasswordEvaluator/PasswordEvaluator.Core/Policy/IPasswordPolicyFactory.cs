using PasswordEvaluator.Core.Policy.Model;

namespace PasswordEvaluator.Core.Policy
{
/// <summary>
/// Factory to create IPasswordPolicy objects of various types
/// </summary>
  public interface IPasswordPolicyFactory
  {
    /// <summary>
    /// Parses a policyString and returns an object representing the policy
    /// </summary>
    /// <param name="policyString">The policyString to parse</param>
    /// <returns>An IPasswordPolicy represented by the policy string</returns>
    public IPasswordPolicy CreatePolicy(string policyString);

    /// <summary>
    /// Given a lowerNumber, upperNumber, and requiredCharacter, create a passwordPolicy
    /// </summary>
    /// <param name="requiredCharacter">The required character of the policy</param>
    /// <param name="lowerNumber">The lower number of the policy</param>
    /// <param name="upperNumber">The upper number of the policy</param>
    /// <returns>An IPasswordPolicy represented by the policy string</returns>
    public IPasswordPolicy CreatePolicy(char requiredCharacter, int lowerNumber, int upperNumber);
  }
}
