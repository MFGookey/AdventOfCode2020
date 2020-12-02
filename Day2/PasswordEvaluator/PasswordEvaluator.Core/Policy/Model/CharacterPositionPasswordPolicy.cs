using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PasswordEvaluator.Core.Policy.Model
{
  /// <inheritdoc/>
  public class CharacterPositionPasswordPolicy : PasswordPolicy, IEquatable<CharacterPositionPasswordPolicy>
  {
    /// <summary>
    /// Initializes a new instance of the CharacterPositionPasswordPolicy class with given RequiredCharacter, LowerNumber, and UpperNumber values.
    /// </summary>
    /// <param name="requiredCharacter">The character required by policy</param>
    /// <param name="minimumInstances">The minimum number of instances a character may be present in a password to be valid by this policy</param>
    /// <param name="maximumInstances">The maximum number of instances a character may be present in a password to be valid by this policy</param>
    public CharacterPositionPasswordPolicy(char requiredCharacter, int minimumInstances, int maximumInstances) : base(requiredCharacter, minimumInstances, maximumInstances)
    {

    }

    /// <summary>
    /// Initializes a new instance of the CharacterPositionPasswordPolicy class by parsing a string represntation of the policy
    /// </summary>
    /// <param name="passwordPolicyString">A string representing the password policy</param>
    public CharacterPositionPasswordPolicy(string passwordPolicyString) : base(passwordPolicyString)
    {

    }

    /// <summary>
    /// Prevent a default instance from being created.
    /// </summary>
    private CharacterPositionPasswordPolicy()
    {
      throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override bool Validate(string password)
    {
      // The password must have the requiredCharacter in either the LowerNumber or the UpperNumber index, of the password were 1-based.
      return password.Length >= UpperNumber &&
        (
          (
            password[LowerNumber - 1] == RequiredCharacter &&
            password[UpperNumber - 1] != RequiredCharacter
          ) ||
          (
            password[LowerNumber - 1] != RequiredCharacter &&
            password[UpperNumber - 1] == RequiredCharacter
          )
        );
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
        CharacterPositionPasswordPolicy p = (CharacterPositionPasswordPolicy)obj;
        return (
          this.RequiredCharacter == p.RequiredCharacter &&
          this.LowerNumber == p.LowerNumber &&
          this.UpperNumber == p.UpperNumber
        );
      }
    }

    /// <inheritdoc/>
    public bool Equals(CharacterPositionPasswordPolicy other)
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

    public static bool operator ==(CharacterPositionPasswordPolicy left, CharacterPositionPasswordPolicy right)
    {
      return EqualityComparer<CharacterPositionPasswordPolicy>.Default.Equals(left, right);
    }

    public static bool operator !=(CharacterPositionPasswordPolicy left, CharacterPositionPasswordPolicy right)
    {
      return !(left == right);
    }
  }
}
