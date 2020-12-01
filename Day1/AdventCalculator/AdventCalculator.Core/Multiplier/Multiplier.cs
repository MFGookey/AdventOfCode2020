using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventCalculator.Core.Multiplier
{
  public class Multiplier : IMultiplier
  {
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
