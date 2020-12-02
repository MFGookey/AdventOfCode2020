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
    /// Gets the minimum number of instances of the required character that must be present in a password in order to be valid by policy
    /// </summary>
    int MinimumInstances
    {
      get;
    }

    /// <summary>
    /// Gets the maximum number of instances of the required character that must be present in a password in order to be valid by policy
    /// </summary>
    int MaximumInstances
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
