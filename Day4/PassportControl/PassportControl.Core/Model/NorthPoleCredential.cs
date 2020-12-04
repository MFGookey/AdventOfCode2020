using System;
using System.Collections.Generic;
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
    public bool ExtendedValidation
    {
      get
      {
        if (Valid)
        {
          bool currentlyValid = true;
          foreach (var validator in _fieldValidators.Values)
          {
            currentlyValid &= validator();
          }

          return currentlyValid;
        }

        return false;
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

    protected Dictionary<string, Func<bool>> _fieldValidators;

    /// <summary>
    /// Given a space delimited string of colon delimited key value pairs, set the properties of the credential
    /// </summary>
    /// <param name="credentialString"></param>
    public NorthPoleCredential(string credentialString)
    {
      _fieldValidators = new Dictionary<string, Func<bool>>()
      {
        { "byr", BirthYearValidator},
        { "iyr", IssuedYearValidator},
        { "eyr", ExpirationYearValidator},
        { "hgt", HeightValidator},
        { "hcl", HairColorValidator},
        { "ecl", EyeColorValidator},
        { "pid", ControlNumberValidator},
      };

      foreach (var field in _fieldValidators.Keys)
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

    /// <summary>
    /// byr (Birth Year) - four digits; at least 1920 and at most 2002.
    /// </summary>
    /// <returns>True if the birth year is valid, otherwise false</returns>
    private bool BirthYearValidator()
    {
      if (byr.Length == 4 && int.TryParse(byr, out var parsedInt))
      {
        return parsedInt >= 1920 && parsedInt <= 2020;
      }
      else
      {
        return false;
      }
    }

    /// <summary>
    /// iyr (Issue Year) - four digits; at least 2010 and at most 2020.
    /// </summary>
    /// <returns>True if the issued year is valid, otherwise false</returns>
    private bool IssuedYearValidator()
    {
      if (iyr.Length == 4 && int.TryParse(iyr, out var parsedInt))
      {
        return parsedInt >= 2010 && parsedInt <= 2020;
      }
      else
      {
        return false;
      }
    }

    /// <summary>
    /// eyr (Expiration Year) - four digits; at least 2020 and at most 2030.
    /// </summary>
    /// <returns>True if the expiration year is valid, otherwise false</returns>
    private bool ExpirationYearValidator()
    {
      if (eyr.Length == 4 && int.TryParse(eyr, out var parsedInt))
      {
        return parsedInt >= 2020 && parsedInt <= 2030;
      }
      else
      {
        return false;
      }
    }

    /// <summary>
    /// hgt (Height) - a number followed by either cm or in:
    ///     If cm, the number must be at least 150 and at most 193.
    ///     If in, the number must be at least 59 and at most 76.
    /// </summary>
    /// <returns>True if the height is valid, otherwise false</returns>
    private bool HeightValidator()
    {
      var match = Regex.Match(hgt, @"^(\d+)(in|cm)");
      if (match.Success && int.TryParse(match.Groups[1].Value, out int parsedInt))
      {
        if (match.Groups[2].Value.Equals("in", StringComparison.InvariantCultureIgnoreCase))
        {
          return parsedInt >= 59 && parsedInt <= 76;
        }
        else
        {
          if (match.Groups[2].Value.Equals("cm", StringComparison.InvariantCultureIgnoreCase))
          {
            return parsedInt >= 150 && parsedInt <= 193;
          }
        }
      }

      return false;
    }

    /// <summary>
    /// hcl (Hair Color) - a # followed by exactly six characters 0-9 or a-f.
    /// </summary>
    /// <returns>True if the hair color is valid, otherwise false</returns>
    private bool HairColorValidator()
    {
      return Regex.IsMatch(hcl, @"#[a-f0-9]{6}", RegexOptions.IgnoreCase);
    }

    /// <summary>
    /// ecl (Eye Color) - exactly one of: amb blu brn gry grn hzl oth.
    /// </summary>
    /// <returns>True if the eye color is valid, otherwise false</returns>
    private bool EyeColorValidator()
    {
      var allowedEyeColors = new string[]
      {
        "amb",
        "blu",
        "brn",
        "gry",
        "grn",
        "hzl",
        "oth"
      };

      var pattern = $"^{string.Join('|', allowedEyeColors)}$";

      return Regex.IsMatch(ecl, pattern, RegexOptions.IgnoreCase);
    }

    /// <summary>
    /// pid (Passport ID) - a nine-digit number, including leading zeroes.
    /// </summary>
    /// <returns>True if the control number is valid, otherwise false</returns>
    private bool ControlNumberValidator()
    {
      return pid.Length == 9;
    }
  }
}