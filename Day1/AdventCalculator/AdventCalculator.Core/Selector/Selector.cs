using System.Collections.Generic;
using System.Linq;

namespace AdventCalculator.Core.Selector
{
  /// <inheritdoc/>
  public class Selector : ISelector
  {
    /// <inheritdoc/>
    public IList<int> SumFinder(IList<int> haystack, int needle, int totalTerms)
    {
      // Given a haystack, and a needle we can recursively work to find the sum using slices of the input haystack.
      if (totalTerms > 0 && (haystack is null) == false)
      {
        IList<int> result = new List<int>();
        // Iterate over the haystack once.  Let i be the current index in haystack
        for (var i = 0; i < haystack.Count; i++)
        {
          // If totalTerms == 1 && the current index in haystack == needle, we're done here!  Return a collection containing the current selected item as a collection
          if (totalTerms == 1)
          {
            if (haystack[i] == needle)
            {
              result.Add(haystack[i]);
              return result;
            }
          }
          else
          {
            // If totalTerms > 1: call SumFinder(haystack[i+1..end], needle-haystack[i], totalTerms-1);
            result = SumFinder(haystack.Skip(i).ToList(), needle - haystack[i], totalTerms - 1);

            // If that recursive call returns null, move on to the next i, else: append haystack[i] to the returned list and return that list.
            if (result is null == false)
            {
              result.Add(haystack[i]);
              return result;
            }
          }
        }
      }

      // If we are at the end of the haystack, return null
      return null;
    }
  }
}
