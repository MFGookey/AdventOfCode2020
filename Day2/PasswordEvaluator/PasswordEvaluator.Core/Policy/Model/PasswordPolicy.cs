using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PasswordEvaluator.Core.Policy.Model
{
  /// <inheritdoc/>
  public abstract class PasswordPolicy : IPasswordPolicy
  {
    /// <inheritdoc/>
    public char RequiredCharacter
    {
      get; private set;
    }

    /// <inheritdoc/>
    public int LowerNumber
    {
      get; private set;
    }

    /// <inheritdoc/>
    public int UpperNumber
    {
      get; private set;
    }

    /// <summary>
    /// Initializes a new instance of the PasswordPolicy class with given RequiredCharacter, LowerNumber, and UpperNumber values.
    /// </summary>
    /// <param name="requiredCharacter">The character required by policy</param>
    /// <param name="minimumInstances">The minimum number of instances a character may be present in a password to be valid by this policy</param>
    /// <param name="maximumInstances">The maximum number of instances a character may be present in a password to be valid by this policy</param>
    public PasswordPolicy(char requiredCharacter, int minimumInstances, int maximumInstances)
    {
      PasswordPolicy.ValidateParameters(requiredCharacter, minimumInstances, maximumInstances);

      RequiredCharacter = requiredCharacter;
      LowerNumber = minimumInstances;
      UpperNumber = maximumInstances;
    }

    /// <summary>
    /// Initializes a new instance of the CharacterFrequencyPasswordPolicy class by parsing a string represntation of the policy
    /// </summary>
    /// <param name="passwordPolicyString">A string representing the password policy</param>
    public PasswordPolicy(string passwordPolicyString)
    {
      var match = Regex.Match(passwordPolicyString, @"^(\d+)-(\d+) (.)$", RegexOptions.None);

      if (match.Success == false)
      {
        throw new ArgumentException("passwordPolicyString does not match the required format of \"{lowerNumber}-{upperNumber} {requiredCharacter}\"", nameof(passwordPolicyString));
      }

      var minimumInstances = int.Parse(match.Groups[1].Value);
      var maximumInstances = int.Parse(match.Groups[2].Value);
      var requiredCharacter = char.Parse(match.Groups[3].Value);

      PasswordPolicy.ValidateParameters(requiredCharacter, minimumInstances, maximumInstances);

      RequiredCharacter = requiredCharacter;
      LowerNumber = minimumInstances;
      UpperNumber = maximumInstances;
    }

    /// <summary>
    /// Prevent a default instance from being created.
    /// </summary>
    protected PasswordPolicy()
    {
      throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public abstract bool Validate(string password);

    /// <summary>
    /// Throws an ArgumentException if the requiredCharacter, minimumInstances, and maximumInstances do not follow sensible rules
    /// </summary>
    /// <param name="requiredCharacter">The character required by policy</param>
    /// <param name="minimumInstances">The minimum number of instances a character may be present in a password to be valid by this policy</param>
    /// <param name="maximumInstances">The maximum number of instances a character may be present in a password to be valid by this policy</param>
    protected static void ValidateParameters(char requiredCharacter, int minimumInstances, int maximumInstances)
    {
      if (minimumInstances < 0)
      {
        throw new ArgumentException("lowerNumber may not be less than 0", nameof(minimumInstances));
      }

      if (maximumInstances < minimumInstances)
      {
        throw new ArgumentException("upperNumber may not be less than lowerNumber", nameof(maximumInstances));
      }
    }

    /// <inheritdoc/>
    public override string ToString()
    {
      return string.Format("{0}-{1} {2}", LowerNumber, UpperNumber, RequiredCharacter);
    }
  }
}
