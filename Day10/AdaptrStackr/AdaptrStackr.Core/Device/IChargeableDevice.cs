using System;
using System.Collections.Generic;
using System.Text;

namespace AdaptrStackr.Core.Device
{
  /// <summary>
  /// Represents a "sink" of Jolts, that is: a device which can receive Jolts
  /// </summary>
  public interface IChargeableDevice : IDevice
  {
    /// <summary>
    /// The lowest input Jolts this sink can take
    /// </summary>
    public int LowInput
    {
      get;
    }

    /// <summary>
    /// The highest input jolts this sink can take
    /// </summary>
    public int HighInput
    {
      get;
    }

    /// <summary>
    /// Returns whether or not we can attach this sink onto a given source
    /// </summary>
    /// <param name="other">The jolts source to which we wish to attempt to attach</param>
    /// <returns>Whether or not this sink can be attached to the source</returns>
    public bool CanAttach(ISocket other);
  }
}
