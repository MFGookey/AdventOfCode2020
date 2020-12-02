using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using PasswordEvaluator.Core.Policy;

namespace PasswordEvaluator.Core.Policy.Model
{
  /// <inheritdoc/>
  public class Password : IPassword, IEquatable<Password>
  {

    private static readonly IPasswordPolicyFactory DefaultPolicyFactory = new CharacterFrequencyPasswordPolicyFactory();

    /// <inheritdoc/>
    public IPasswordPolicy PasswordPolicy
    {
      get; private set;
    }

    /// <inheritdoc/>
    public bool IsValid
    {
      get
      {
        return PasswordPolicy.Validate(PasswordCandidate);
      }
    }

    /// <inheritdoc/>
    public string PasswordCandidate
    {
      get;
      private set;
    }

    /// <summary>
    /// Initializes a new instance of the Password class with a given passwordPolicy and passwordCandidate
    /// </summary>
    /// <param name="passwordPolicy">The passwordPolicy to use</param>
    /// <param name="passwordCandidate">The passwordCandidate to use</param>
    public Password(IPasswordPolicy passwordPolicy, string passwordCandidate)
    {
      PasswordPolicy = passwordPolicy;
      PasswordCandidate = passwordCandidate;
    }

    /// <summary>
    /// Initializes a new instance of the Password class where we attempt to parse the policy from a string, and a candidate password
    /// </summary>
    /// <param name="passwordPolicy">The passwordPolicy string to attempt to parse into a policy</param>
    /// <param name="passwordCandidate">The passwordCandidate to use</param>
    public Password(string passwordPolicy, string passwordCandidate)
    {
      PasswordPolicy = Password.DefaultPolicyFactory.CreatePolicy(passwordPolicy);
      PasswordCandidate = passwordCandidate;
    }

    /// <summary>
    /// Initializes a new instance of the Password class where we attempt to parse the policy from a string, and a candidate password
    /// </summary>
    /// <param name="passwordPolicy">The passwordPolicy string to attempt to parse into a policy</param>
    /// <param name="passwordCandidate">The passwordCandidate to use</param>
    /// <param name="policyFactory">The passwordPolicyFactory to use</param>
    public Password(string passwordPolicy, string passwordCandidate, IPasswordPolicyFactory policyFactory)
    {
      PasswordPolicy = policyFactory.CreatePolicy(passwordPolicy);
      PasswordCandidate = passwordCandidate;
    }

    /// <summary>
    /// Initializes a new instance of the Password class where the passwordPolicy and passwordCandidate are combined in a formatted string of the format "{policy}: {passwordCandiate}"
    /// </summary>
    /// <param name="policyPasswordCombination">The password policy and password to parse as a single string</param>
    public Password(string policyPasswordCombination)
    {
      var match = Regex.Match(policyPasswordCombination, @"^([^:]*): (.*)$", RegexOptions.None);

      if (match.Success == false)
      {
        throw new ArgumentException("policyPasswordCombination does not match the required format of \"{policy}: {passwordCandidate}\"", nameof(policyPasswordCombination));
      }

      PasswordPolicy = Password.DefaultPolicyFactory.CreatePolicy(match.Groups[1].Value);
      PasswordCandidate = match.Groups[2].Value;
    }

    /// <summary>
    /// Initializes a new instance of the Password class where the passwordPolicy and passwordCandidate are combined in a formatted string of the format "{policy}: {passwordCandiate}"
    /// </summary>
    /// <param name="policyPasswordCombination">The password policy and password to parse as a single string</param>
    /// <param name="policyFactory">The passwordPolicyFactory to use</param>
    public Password(string policyPasswordCombination, IPasswordPolicyFactory passwordPolicyFactory)
    {
      var match = Regex.Match(policyPasswordCombination, @"^([^:]*): (.*)$", RegexOptions.None);

      if (match.Success == false)
      {
        throw new ArgumentException("policyPasswordCombination does not match the required format of \"{policy}: {passwordCandidate}\"", nameof(policyPasswordCombination));
      }

      PasswordPolicy = passwordPolicyFactory.CreatePolicy(match.Groups[1].Value);
      PasswordCandidate = match.Groups[2].Value;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
      return string.Format("{0}: {1}", PasswordPolicy.ToString(), PasswordCandidate);
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
        Password p = (Password)obj;
        return (
          this.PasswordPolicy.Equals(p.PasswordPolicy) &&
          this.PasswordCandidate.Equals(p.PasswordCandidate) &&
          this.IsValid == p.IsValid
          );
      }
    }

    public bool Equals(Password other)
    {
      return other != null &&
             EqualityComparer<IPasswordPolicy>.Default.Equals(PasswordPolicy, other.PasswordPolicy) &&
             IsValid == other.IsValid &&
             PasswordCandidate == other.PasswordCandidate;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
      return HashCode.Combine(PasswordPolicy, IsValid, PasswordCandidate);
    }

    public static bool operator ==(Password left, Password right)
    {
      return EqualityComparer<Password>.Default.Equals(left, right);
    }

    public static bool operator !=(Password left, Password right)
    {
      return !(left == right);
    }
  }
}
