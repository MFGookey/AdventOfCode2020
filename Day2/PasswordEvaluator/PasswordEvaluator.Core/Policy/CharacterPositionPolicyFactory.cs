using System;
using PasswordEvaluator.Core.Policy.Model;

namespace PasswordEvaluator.Core.Policy
{
  /// <inheritdoc/>
  public class CharacterPositionPolicyFactory : IPasswordPolicyFactory
  {
    /// <inheritdoc/>
    public IPasswordPolicy CreatePolicy(string policyString)
    {
      throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public IPasswordPolicy CreatePolicy(char requiredCharacter, int lowerNumber, int upperNumber)
    {
      throw new NotImplementedException();
    }
  }
}
