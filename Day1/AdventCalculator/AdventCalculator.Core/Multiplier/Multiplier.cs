using System.Collections.Generic;
using System.Linq;

namespace AdventCalculator.Core.Multiplier
{
  /// <inheritdoc/>
  public class Multiplier : IMultiplier
  {
    /// <inheritdoc/>
    public int? Multiply(IEnumerable<int> multiplicands)
    {
      int? result = null;
      
      if (multiplicands is null == false && multiplicands.Count() >= 2)
      {
        result = multiplicands.First() * multiplicands.Skip(1).First();

        foreach (var multiplicand in multiplicands.Skip(2))
        {
          result *= multiplicand;
        }
      }
      
      return result;
    }
  }
}
