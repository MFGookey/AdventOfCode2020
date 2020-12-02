using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PasswordEvaluator.Core.Policy.Model
{
  /// <inheritdoc/>
  public class CharacterFrequencyPasswordPolicy : PasswordPolicy, IEquatable<CharacterFrequencyPasswordPolicy>
  {
    /// <summary>
    /// Initializes a new instance of the CharacterFrequencyPasswordPolicy class with given RequiredCharacter, LowerNumber, and UpperNumber values.
    /// </summary>
    /// <param name="requiredCharacter">The character required by policy</param>
    /// <param name="minimumInstances">The minimum number of instances a character may be present in a password to be valid by this policy</param>
    /// <param name="maximumInstances">The maximum number of instances a character may be present in a password to be valid by this policy</param>
    public CharacterFrequencyPasswordPolicy(char requiredCharacter, int minimumInstances, int maximumInstances) : base(requiredCharacter, minimumInstances, maximumInstances)
    {

    }

    /// <summary>
    /// Initializes a new instance of the CharacterFrequencyPasswordPolicy class by parsing a string represntation of the policy
    /// </summary>
    /// <param name="passwordPolicyString">A string representing the password policy</param>
    public CharacterFrequencyPasswordPolicy(string passwordPolicyString) : base(passwordPolicyString)
    {
      
    }

    /// <summary>
    /// Prevent a default instance from being created.
    /// </summary>
    private CharacterFrequencyPasswordPolicy() : base()
    {
      throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override bool Validate(string password)
    {
      return Regex.IsMatch(password, string.Format("^(?:[^{0}]*{0}){{{1},{2}}}[^{0}]*$", RequiredCharacter, LowerNumber, UpperNumber), RegexOptions.None);
    }

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
      //Check for null and compare run-time types.
      if ((obj == null) || !this.GetType().Equals(obj.GetType()))
      {
        return false;
      }
      else
      {
        CharacterFrequencyPasswordPolicy p = (CharacterFrequencyPasswordPolicy)obj;
        return (
          this.RequiredCharacter == p.RequiredCharacter &&
          this.LowerNumber == p.LowerNumber &&
          this.UpperNumber == p.UpperNumber
        );
      }
    }

    /// <inheritdoc/>
    public bool Equals(CharacterFrequencyPasswordPolicy other)
    {
      return other != null &&
             RequiredCharacter == other.RequiredCharacter &&
             LowerNumber == other.LowerNumber &&
             UpperNumber == other.UpperNumber;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
      return HashCode.Combine(RequiredCharacter, LowerNumber, UpperNumber);
    }

    public static bool operator ==(CharacterFrequencyPasswordPolicy left, CharacterFrequencyPasswordPolicy right)
    {
      return EqualityComparer<CharacterFrequencyPasswordPolicy>.Default.Equals(left, right);
    }

    public static bool operator !=(CharacterFrequencyPasswordPolicy left, CharacterFrequencyPasswordPolicy right)
    {
      return !(left == right);
    }
  }
}
