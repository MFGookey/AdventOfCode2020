using System.Collections.Generic;
using PasswordEvaluator.Core.Policy.Model;
using PasswordEvaluator.Core.Policy;
using Utilities.IO;

namespace PasswordEvaluator.Core.Database
{
  /// <inheritdoc/>
  public class Database : IDatabase
  {
    /// <inheritdoc/>
    public IReadOnlyList<IPassword> Passwords
    {
      get
      {
        return _passwords;
      }
    }

    private List<IPassword> _passwords;
    private IFileReader _fileReader;
    private IPasswordPolicyFactory _passwordPolicyFactory;

    /// <summary>
    /// Initializes a new instance of the database with an empty Passwords collection
    /// </summary>
    public Database()
    {
      _passwords = new List<IPassword>();
      _fileReader = new FileReader();
      _passwordPolicyFactory = new CharacterFrequencyPasswordPolicyFactory();
    }

    /// <summary>
    /// Initializes a new instance of the database with a given Passwords collection, file reader, and password policy factory
    /// </summary>
    /// <param name="passwords">The list of passwords to automatically include</param>
    /// <param name="fileReader">The fileReader the database should use for imports</param>
    /// <param name="passwordPolicyFactory">The fileReader the database should use for imports</param>
    public Database(
      List<IPassword> passwords,
      IFileReader fileReader,
      IPasswordPolicyFactory passwordPolicyFactory
    )
    {
      _passwords = passwords;
      _fileReader = fileReader;
      _passwordPolicyFactory = passwordPolicyFactory;
    }

    /// <inheritdoc/>
    public void Add(Password password)
    {
      _passwords.Add(password);
    }

    /// <inheritdoc/>
    public void Add(string passwordPolicyCombo)
    {
      this.Add(new Password(passwordPolicyCombo, _passwordPolicyFactory));
    }

    /// <inheritdoc/>
    public void Import(string filePath)
    {
      var passwordPolicyCombinations = _fileReader.ReadFileByLines(filePath);
      Import(passwordPolicyCombinations);
    }

    /// <inheritdoc/>
    public void Import(string[] passwordPolicyCombinations)
    {
      var pendingList = new List<Password>();

      // Add this atomically.  If any fail to parse, abandon the work.
      foreach (var combination in passwordPolicyCombinations)
      {
        pendingList.Add(new Password(combination, _passwordPolicyFactory));
      }

      _passwords.AddRange(pendingList);
    }
  }
}
