using System;
using System.Collections.Generic;
using System.Text;
using Common.Utilities.Selector;
using System.Linq;

namespace ChristmasCracker.Core
{
  /// <inheritdoc cref="ICracker"/>
  public class Cracker : ICracker
  {
    private readonly ISelector _selector;

    /// <summary>
    /// Initializes a new instance of the Cracker class with a default ISelector
    /// </summary>
    public Cracker() : this(new Selector()) { }
    
    /// <summary>
    /// Initializes a new instance of the Cracker class with a given ISelector
    /// </summary>
    /// <param name="selector">The ISelector to use for cracking</param>
    public Cracker(ISelector selector)
    {
      _selector = selector;
    }

    /// <inheritdoc />
    public IEnumerable<long> FindUnsummableNumbers(int preambleLength, int requiredTerms, IEnumerable<long> message)
    {
      if (requiredTerms <= 0)
      {
        throw new ArgumentException("Required Terms must be greater than 0", nameof(requiredTerms));
      }

      if (requiredTerms > preambleLength)
      {
        throw new ArgumentException($"Preamble length \"{preambleLength}\" must be longer than the number of required terms: \"{requiredTerms}\"", nameof(preambleLength));
      }

      if (preambleLength >= message.Count())
      {
        throw new ArgumentException($"Preamble length \"{preambleLength}\" must be shorter than the number of numbers in the message: \"{message.Count()}\"", nameof(preambleLength));
      }

      // For every number in message, beginning at the preambleLength + 1th term, find requiredTerms integers that add up to the preambleLength + 1th term from the previous preambleLength terms.
      return Enumerable
        .Range(0, message.Count() - preambleLength)
        .Select(
          k => new
          {
            needle = message.Skip(k + preambleLength).First(),
            haystack = message.Skip(k).Take(preambleLength),
            terms = 2
          }
        )
        .Select(
          run => new
          {
            run.needle,
            foundSum = _selector.SumFinder(run.haystack, run.needle, run.terms)
          }
        )
        .Where(result => result.foundSum == null)
        .Select(result => result.needle);
    }
  }
}
