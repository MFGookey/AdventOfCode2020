using System;
using System.Collections.Generic;
using System.Text;

namespace AdventCalculator.Core.Multiplier
{
  public interface IMultiplier
  {
    /// <summary>
    /// Given a list of integers, multiply all of them together
    /// </summary>
    /// <param name="multiplicands">The list of integers to multiply</param>
    /// <returns>The product of the list of multiplicands, or null for an empty or null list</returns>
    int? Multiply(IList<int> multiplicands);
  }
}
