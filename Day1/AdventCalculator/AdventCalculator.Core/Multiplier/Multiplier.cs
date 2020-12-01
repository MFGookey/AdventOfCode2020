using System.Collections.Generic;
using System.Linq;

namespace AdventCalculator.Core.Multiplier
{
  /// <inheritdoc/>
  public class Multiplier : IMultiplier
  {
    /// <inheritdoc/>
    public int? Multiply(IList<int> multiplicands)
    {
      var result = multiplicands?.Cast<int?>().FirstOrDefault();

      if (multiplicands is null == false)
      {
        foreach (var multiplicand in multiplicands?.Skip(1))
        {
          result *= multiplicand;
        }
      }
      
      return result;
    }
  }
}
