using System.Collections.Generic;
using PasswordEvaluator.Core.Policy.Model;

namespace PasswordEvaluator.Core.Database
{
  /// <summary>
  /// Represents a database of passwords
  /// </summary>
  public interface IDatabase
  {
    /// <summary>
    /// Gets a read only list of the passwords in the database
    /// </summary>
    public IReadOnlyList<IPassword> Passwords
    {
      get;
    }

    /// <summary>
    /// Given a filepath, import the contents of the file as password entries
    /// </summary>
    /// <param name="filePath">The path to the file of passwords to import</param>
    public void Import(string filePath);

    /// <summary>
    /// Given an array of password and policy string representations, import them into the Passwords list
    /// </summary>
    /// <param name="passwordPolicyCombinations"></param>
    public void Import(string[] passwordPolicyCombinations);

    /// <summary>
    /// Add a single Password to the database
    /// </summary>
    /// <param name="password">The password to add</param>
    public void Add(Password password);

    /// <summary>
    /// Add a single passwordPolicyCombo to the database
    /// </summary>
    /// <param name="passwordPolicyCombo">The pasword policy combo to add.</param>
    public void Add(string passwordPolicyCombo);
  }
}
