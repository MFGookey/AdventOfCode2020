namespace PasswordEvaluator.Core.Policy.Model
{
  /// <summary>
  /// Represents a password policy and provides a means by which a password can be validated by policy
  /// </summary>
  public interface IPasswordPolicy
  {
    /// <summary>
    /// Gets the character that is required by policy to appear within a password within a frequency range
    /// </summary>
    char RequiredCharacter
    {
      get;
    }

    /// <summary>
    /// Gets the lower number of the implementation-specific policy
    /// </summary>
    int LowerNumber
    {
      get;
    }

    /// <summary>
    /// Gets the upper number of the implementation-specific policy
    /// </summary>
    int UpperNumber
    {
      get;
    }

    /// <summary>
    /// Returns a value indicating whether or not the given password is allowed by this policy
    /// </summary>
    /// <param name="password">The password to validate</param>
    /// <returns>True if the password is valid by this policy, otherwise false</returns>
    bool Validate(string password);
  }
}
