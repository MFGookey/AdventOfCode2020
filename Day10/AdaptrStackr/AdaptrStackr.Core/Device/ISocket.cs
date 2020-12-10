using System;
using System.Collections.Generic;
using System.Text;

namespace AdaptrStackr.Core.Device
{
  /// <summary>
  /// Represents a "source" of Jolts, that is: a device which can emit Jolts
  /// </summary>
  public interface ISocket : IDevice
  {
    /// <summary>
    /// The nominal output of this adapter
    /// </summary>
    public int Output
    {
      get;
    }
  }
}
