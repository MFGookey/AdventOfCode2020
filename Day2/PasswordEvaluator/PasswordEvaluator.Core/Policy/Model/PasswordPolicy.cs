using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PasswordEvaluator.Core.Policy.Model
{
  /// <inheritdoc/>
  public class PasswordPolicy : IPasswordPolicy, IEquatable<PasswordPolicy>
  {
    /// <inheritdoc/>
    public char RequiredCharacter
    {
      get; private set;
    }

    /// <inheritdoc/>
    public int MinimumInstances
    {
      get; private set;
    }

    /// <inheritdoc/>
    public int MaximumInstances
    {
      get; private set;
    }

    /// <summary>
    /// Initializes a new instance of the PasswordPolicy class with given RequiredCharacter, MinimumInstances, and MaximumInstances values.
    /// </summary>
    /// <param name="requiredCharacter">The character required by policy</param>
    /// <param name="minimumInstances">The minimum number of instances a character may be present in a password to be valid by this policy</param>
    /// <param name="maximumInstances">The maximum number of instances a character may be present in a password to be valid by this policy</param>
    public PasswordPolicy(char requiredCharacter, int minimumInstances, int maximumInstances)
    {
      PasswordPolicy.ValidateParameters(requiredCharacter, minimumInstances, maximumInstances);

      RequiredCharacter = requiredCharacter;
      MinimumInstances = minimumInstances;
      MaximumInstances = maximumInstances;
    }

    /// <summary>
    /// Initializes a new instance of the PasswordPolicy class by parsing a string represntation of the policy
    /// </summary>
    /// <param name="passwordPolicyString"></param>
    public PasswordPolicy(string passwordPolicyString)
    {
      var match = Regex.Match(passwordPolicyString, @"^(\d+)-(\d+) (.)$", RegexOptions.None);

      if (match.Success == false)
      {
        throw new ArgumentException("passwordPolicyString does not match the required format of \"{minimumInstances}-{maximumInstances} {requiredCharacter}\"", nameof(passwordPolicyString));
      }

      var minimumInstances = int.Parse(match.Groups[1].Value);
      var maximumInstances = int.Parse(match.Groups[2].Value);
      var requiredCharacter = char.Parse(match.Groups[3].Value);
      
      PasswordPolicy.ValidateParameters(requiredCharacter, minimumInstances, maximumInstances);

      RequiredCharacter = requiredCharacter;
      MinimumInstances = minimumInstances;
      MaximumInstances = maximumInstances;
    }

    /// <summary>
    /// Prevent a default instance from being created.
    /// </summary>
    private PasswordPolicy()
    {
      throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public bool Validate(string password)
    {
      return Regex.IsMatch(password, string.Format("^(?:[^{0}]*{0}){{{1},{2}}}[^{0}]*$", RequiredCharacter, MinimumInstances, MaximumInstances), RegexOptions.None);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
      return string.Format("{0}-{1} {2}", MinimumInstances, MaximumInstances, RequiredCharacter);
    }

    /// <summary>
    /// Throws an ArgumentException if the requiredCharacter, minimumInstances, and maximumInstances do not follow sensible rules
    /// </summary>
    /// <param name="requiredCharacter">The character required by policy</param>
    /// <param name="minimumInstances">The minimum number of instances a character may be present in a password to be valid by this policy</param>
    /// <param name="maximumInstances">The maximum number of instances a character may be present in a password to be valid by this policy</param>
    private static void ValidateParameters(char requiredCharacter, int minimumInstances, int maximumInstances)
    {
      if (minimumInstances < 0)
      {
        throw new ArgumentException("minimumInstances may not be less than 0", nameof(minimumInstances));
      }

      if (maximumInstances < minimumInstances)
      {
        throw new ArgumentException("maximumInstances may not be less than minimumInstances", nameof(maximumInstances));
      }
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
        PasswordPolicy p = (PasswordPolicy)obj;
        return (
          this.RequiredCharacter == p.RequiredCharacter &&
          this.MinimumInstances == p.MinimumInstances &&
          this.MaximumInstances == p.MaximumInstances
        );
      }
    }

    /// <inheritdoc/>
    public bool Equals(PasswordPolicy other)
    {
      return other != null &&
             RequiredCharacter == other.RequiredCharacter &&
             MinimumInstances == other.MinimumInstances &&
             MaximumInstances == other.MaximumInstances;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
      return HashCode.Combine(RequiredCharacter, MinimumInstances, MaximumInstances);
    }

    public static bool operator ==(PasswordPolicy left, PasswordPolicy right)
    {
      return EqualityComparer<PasswordPolicy>.Default.Equals(left, right);
    }

    public static bool operator !=(PasswordPolicy left, PasswordPolicy right)
    {
      return !(left == right);
    }
  }
}
