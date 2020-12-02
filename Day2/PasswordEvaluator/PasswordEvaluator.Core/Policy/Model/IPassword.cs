using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordEvaluator.Core.Policy.Model
{
  /// <summary>
  /// Represents the combination of a password and an associated password policy
  /// </summary>
  public interface IPassword
  {
    /// <summary>
    /// The password policy against which the password ought to be validated
    /// </summary>
    public IPasswordPolicy PasswordPolicy
    {
      get;
    }

    /// <summary>
    /// The password to validate against the policy
    /// </summary>
    public string PasswordCandidate
    {
      get;
    }

    /// <summary>
    /// Gets a value indicating whether or not the Password is valid, per the policy
    /// </summary>
    public bool IsValid
    {
      get;
    }
  }
}
