using PasswordEvaluator.Core.Policy.Model;

namespace PasswordEvaluator.Core.Policy
{
  /// <inheritdoc/>
  public class CharacterFrequencyPasswordPolicyFactory : IPasswordPolicyFactory
  {
    /// <inheritdoc/>
    public IPasswordPolicy CreatePolicy(string policyString)
    {
      return new CharacterFrequencyPasswordPolicy(policyString);
    }

    /// <inheritdoc/>
    public IPasswordPolicy CreatePolicy(char requiredCharacter, int lowerNumber, int upperNumber)
    {
      return new CharacterFrequencyPasswordPolicy(requiredCharacter, lowerNumber, upperNumber);
    }
  }
}
