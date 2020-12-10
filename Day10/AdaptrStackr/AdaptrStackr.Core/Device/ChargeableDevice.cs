using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AdaptrStackr.Core.Device
{
  /// <summary>
  /// Represents a device capable of charging using Jolts sources.  By definition it can receive any amount of jolts as input.
  /// </summary>
  public class ChargeableDevice : IChargeableDevice
  {
    
    private IChargeableDevice _internalAdapter;

    /// <inheritdoc/>
    public int LowInput
    {
      get
      {
        return _internalAdapter.LowInput;
      }
    }

    /// <inheritdoc/>
    public int HighInput
    {
      get {
        return _internalAdapter.HighInput;
      }
    }

    /// <inheritdoc/>
    public bool CanAttach(ISocket other)
    {
      return _internalAdapter.CanAttach(other);
    }

    /// <summary>
    /// Initializes a new chargable device that can accept the highest rated source by output from a list of sources as its lowest possible input
    /// </summary>
    /// <param name="sources">The sources to use when determining this device's input thresholds</param>
    public ChargeableDevice(IEnumerable<ISocket> sources) :
      this(
        sources,
        (sources) => 
        sources
          .OrderByDescending(
            source => source.Output
          )
          .FirstOrDefault()
      )
    { }

    /// <summary>
    /// Initializes a new chargable device that can accept the highest rated source by output from a list of sources as its lowest possible input
    /// </summary>
    /// <param name="sources">The sources to use when determining this device's input thresholds</param>
    /// <param name="sourceSelector">The rule to use to select the source from sources to use</param>
    public ChargeableDevice(
      IEnumerable<ISocket> sources,
      Func<IEnumerable<ISocket>, ISocket> sourceSelector
    ) :
      this(
        sources,
        (source) => sourceSelector(sources).Output + 3
      )
    { }

    /// <summary>
    /// Initialize a new chargable device that can accept the given source's output as its lowest possible input
    /// </summary>
    /// <param name="source">The source on which to base this device's input thresholds</param>
    /// <param name="ruleSelector">The rule to use to select the final output of the internal adapter from sources to use</param>
    /// <param name="internalAdapterOutputDeriver">The rule to use to derive the output of this device's internal adapter based on the selected source</param>
    public ChargeableDevice(
      IEnumerable<ISocket> sources,
      Func<IEnumerable<ISocket>, int> outputSelector
    ) :
      this(
        sources,
        (sources) => (IChargeableDevice)new Adapter(outputSelector(sources))
      )
    { }

    /// <summary>
    /// Initialize a new chargable device that can accept a given output as its lowest possible input
    /// </summary>
    /// <param name="sources">The sources to use to decide the inputs of this device</param>
    /// <param name="adapterSelector">A selector to map Sources to an IAdapter to use internally</param>
    public ChargeableDevice(
      IEnumerable<ISocket> sources,
      Func<IEnumerable<ISocket>, IChargeableDevice> adapterSelector
    ) : this(adapterSelector(sources))
    { }

    /// <summary>
    /// Initialize a new chareable device with a given internal adapter
    /// </summary>
    /// <param name="internalAdapter">The internal adapter to use</param>
    public ChargeableDevice(IChargeableDevice internalAdapter)
    {
      if (internalAdapter == null)
      {
        throw new ArgumentException("The internal adapter cannot be null", nameof(internalAdapter));
      }

      _internalAdapter = internalAdapter;
    }
  }
}
