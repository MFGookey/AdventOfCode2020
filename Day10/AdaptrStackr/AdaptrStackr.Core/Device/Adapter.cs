using System;
using System.Collections.Generic;
using System.Text;

namespace AdaptrStackr.Core.Device
{
  /// <inheritdoc cref="IAdapter"/>
  public class Adapter : IAdapter
  {
    /// <inheritdoc/>
    public int Output
    {
      get;
      private set;
    }

    /// <inheritdoc/>
    public int LowInput
    {
      get;
      private set;
    }

    /// <inheritdoc/>
    public int HighInput
    {
      get;
      private set;
    }

    /// <summary>
    /// Initialize a new Adapter following a default set of rules for allowed inputs
    /// </summary>
    /// <param name="output">The output jolts provided by this adapter</param>
    public Adapter(int output) : this(output, (output) => output - 3, (output) => output - 1)
    { }

    /// <summary>
    /// Initialize a new Adapter using an output, and rules to derive the input thresholds
    /// </summary>
    /// <param name="output">The output for this adapter</param>
    /// <param name="lowInputDeriver">Given an int, the lower threshold of jolts that can be supported as an input to this adapter</param>
    /// <param name="highInputDeriver">Given an int, the higher threshold of jolts that can be supported as an input to this adapter</param>
    public Adapter(int output, Func<int, int> lowInputDeriver, Func<int, int> highInputDeriver)
    {

      var lowCandidate = lowInputDeriver(output);
      var highCandidate = highInputDeriver(output);

      if (lowCandidate > highCandidate)
      {
        throw new ArgumentException($"The detected low input \"{lowCandidate}\" cannot be greater than the detected high input \"{highCandidate}\"");
      }

      Output = output;
      LowInput = lowCandidate;
      HighInput = highCandidate;
    }

    /// <inheritdoc/>
    public bool CanAttach(ISocket other)
    {
      if (other == null)
      {
        return false;
      }

      return (other.Output >= LowInput && other.Output <= HighInput);
    }
  }
}
