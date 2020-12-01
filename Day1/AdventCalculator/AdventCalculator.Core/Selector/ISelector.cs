﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AdventCalculator.Core.Selector
{
  public interface ISelector
  {
    /// <summary>
    /// Given a list of ints, a target sum, and the number of terms to include in the final list, return an enumerable of those ints
    /// </summary>
    /// <param name="haystack">The list of ints to search for a sum equalling the target value</param>
    /// <param name="needle">The target value to which the returned list must sum</param>
    /// <param name="totalTerms">The number of ints that ought to be in the final listing</param>
    /// <returns>An enumerable of integers that sum to a target value</returns>
    IList<int> SumFinder(IList<int> haystack, int needle, int totalTerms);
  }
}