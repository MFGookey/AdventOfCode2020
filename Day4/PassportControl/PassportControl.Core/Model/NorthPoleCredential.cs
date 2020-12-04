using System;
using System.Text.RegularExpressions;

namespace PassportControl.Core.Model
{
  public class NorthPoleCredential : ICredential
  {
    /// <inheritdoc/>
    public bool Valid
    {
      get
      {
        return
          (
            string.IsNullOrEmpty(byr) == false &&
            string.IsNullOrEmpty(iyr) == false &&
            string.IsNullOrEmpty(eyr) == false &&
            string.IsNullOrEmpty(hgt) == false &&
            string.IsNullOrEmpty(hcl) == false &&
            string.IsNullOrEmpty(ecl) == false &&
            string.IsNullOrEmpty(pid) == false
          );
      }
    }

    /// <inheritdoc/>
    public string byr
    {
      get; protected set;
    }

    /// <inheritdoc/>
    public string iyr
    {
      get; protected set;
    }

    /// <inheritdoc/>
    public string eyr
    {
      get; protected set;
    }

    /// <inheritdoc/>
    public string hgt
    {
      get; protected set;
    }

    /// <inheritdoc/>
    public string hcl
    {
      get; protected set;
    }

    /// <inheritdoc/>
    public string ecl
    {
      get; protected set;
    }

    /// <inheritdoc/>
    public string pid
    {
      get; protected set;
    }

    /// <summary>
    /// Given a space delimited string of colon delimited key value pairs, set the properties of the credential
    /// </summary>
    /// <param name="credentialString"></param>
    public NorthPoleCredential(string credentialString)
    {
      var supportedFields = new string[]
      {
        "byr",
        "iyr",
        "eyr",
        "hgt",
        "hcl",
        "ecl",
        "pid"
      };

      foreach (var field in supportedFields)
      {
        SetPropertyByString(field, FindPropertyValue(field, credentialString));
      }
    }

    /// <summary>
    /// Use reflection to set the property value by the property's name
    /// </summary>
    /// <param name="propertyName">The name of the property to set</param>
    /// <param name="value">The value to set the property</param>
    protected void SetPropertyByString(string propertyName, string value)
    {
      this.GetType().GetProperty(propertyName).SetValue(this, value);
    }

    /// <summary>
    /// Search a string formatted as a space delimited series of colon delimited key value pairs for a key matching the desired property name
    /// </summary>
    /// <param name="propertyName">The name of the desired property</param>
    /// <param name="credentialString">The credential string to search</param>
    /// <returns>The value of the desired property</returns>
    protected string FindPropertyValue(string propertyName, string credentialString)
    {

      var matches = Regex.Match(credentialString, $"(?:^|\\s){propertyName}:([^\\s]+)");

      if (matches.Success)
      {
        return matches.Groups[1].Value;
      }

      return null;
    }
  }
}