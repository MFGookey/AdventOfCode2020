using System;
using System.Collections.Generic;
using System.Text;

namespace ChristmasCracker.Core
{
  /// <summary>
  /// Cracker of the eXchange-Masking Addition System (XMAS) protocol
  /// </summary>
  public interface ICracker
  {
    /// <summary>
    /// Given a preamble length, a number of required terms and a message (including preamble),
    /// return the first int that cannot be made up by adding {requiredTerms} terms together when looking at the preceeding {preamble} digits
    /// </summary>
    /// <param name="preambleLength">The length of the message preamble</param>
    /// <param name="requiredTerms">The number of terms to search the preceeding numbers to sum up to the target number in the message</param>
    /// <param name="message">The numbers to process, including the preamble</param>
    /// <returns>The ints in message that are not a sum of {requiredTerms} terms from the preceeding {preambleLength} numbers in message.</returns>
    public IEnumerable<long> FindUnsummableNumbers(int preambleLength, int requiredTerms, IEnumerable<long> message);
  }
}
