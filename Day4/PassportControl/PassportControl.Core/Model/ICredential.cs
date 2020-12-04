namespace PassportControl.Core.Model
{
  /// <summary>
  /// Represents credentials
  /// </summary>
  public interface ICredential
  {
    /// <summary>
    /// Indicates whether or not the given credential is valid
    /// </summary>
    public bool Valid
    {
      get;
    }

    /// <summary>
    /// The credential holder's birth year
    /// </summary>
    public string byr
    {
      get;
    }

    /// <summary>
    /// The credential's issued year
    /// </summary>
    public string iyr
    {
      get;
    }

    /// <summary>
    /// The credential's expiration year
    /// </summary>
    public string eyr
    {
      get;
    }

    /// <summary>
    /// The credential holder's height
    /// </summary>
    public string hgt
    {
      get;
    }

    /// <summary>
    /// The credential holder's hair color
    /// </summary>
    public string hcl
    {
      get;
    }

    /// <summary>
    /// The credential holder's eye color
    /// </summary>
    public string ecl
    {
      get;
    }

    /// <summary>
    /// The credential's control number
    /// </summary>
    public string pid
    {
      get;
    }
  }
}
