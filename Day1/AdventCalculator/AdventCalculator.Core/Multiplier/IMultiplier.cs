using System.Collections.Generic;

namespace AdventCalculator.Core.Multiplier
{
  /// <summary>
  /// Given a list of at least two ints, multiply them together
  /// </summary>
  public interface IMultiplier
  {
    /// <summary>
    /// Given a list of integers, multiply all of them together
    /// </summary>
    /// <param name="multiplicands">The list of integers to multiply</param>
    /// <returns>The product of the list of multiplicands, or null for a list that is null or contains fewer than 2 multiplicands.</returns>
    int? Multiply(IList<int> multiplicands);
  }
}
