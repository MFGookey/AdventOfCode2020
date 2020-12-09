using System;
using System.Collections.Generic;
using System.Text;

namespace Bagr.Core.RuleParsing
{
  /// <summary>
  /// Represents a bag and its contents as strings and ints
  /// </summary>
  public class StringBag : IEquatable<StringBag>
  {
    /// <summary>
    /// Gets or sets color of this bag
    /// </summary>
    public string BagColor
    {
      get; set;
    }

    /// <summary>
    /// Gets or sets the color of a contained bag
    /// </summary>
    public string ContainedBag
    {
      get; set;
    }

    /// <summary>
    /// Gets or sets the quantity of the contained bag
    /// </summary>
    public int ContainedQuantity
    {
      get; set;
    }

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
      if (obj == null || obj is StringBag == false)
      {
        return false;
      }

      var otherBag = (StringBag)obj;

      if (otherBag == null)
      {
        return false;
      }

      return this.BagColor.Equals(otherBag.BagColor) &&
        this.ContainedBag.Equals(otherBag.ContainedBag) &&
        this.ContainedQuantity == otherBag.ContainedQuantity;
    }

    /// <inheritdoc/>
    public bool Equals(StringBag other)
    {
      return other != null &&
             BagColor.Equals(other.BagColor) &&
             ContainedBag.Equals(other.ContainedBag) &&
             ContainedQuantity == other.ContainedQuantity;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
      return HashCode.Combine(BagColor, ContainedBag, ContainedQuantity);
    }
  }
}
