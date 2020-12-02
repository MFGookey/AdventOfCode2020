using PasswordEvaluator.Core.Policy.Model;

namespace PasswordEvaluator.Core.Policy
{
  /// <inheritdoc/>
  public class CharacterPositionPasswordPolicyFactory : IPasswordPolicyFactory
  {
    /// <inheritdoc/>
    public IPasswordPolicy CreatePolicy(string policyString)
    {
      return new CharacterPositionPasswordPolicy(policyString);
    }

    /// <inheritdoc/>
    public IPasswordPolicy CreatePolicy(char requiredCharacter, int lowerNumber, int upperNumber)
    {
      return new CharacterPositionPasswordPolicy(requiredCharacter, lowerNumber, upperNumber);
    }
  }
}
