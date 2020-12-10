using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdaptrStackr.Core.Device;

namespace AdaptrStackr.Core
{
  /// <summary>
  /// Implement a method to use all IDevices in a single stack beginning with a Socket and ending with a ChargeableDevice
  /// </summary>
  public interface IDeviceStacker
  {
    /// <summary>
    /// Create a stack of the devices, and then process it
    /// </summary>
    /// <param name="toStack">The devices to stack</param>
    /// <returns>The product of the count of devices that are 1 unit apart times the ones that are 3 units apart</returns>
    int CreateStack(IEnumerable<IDevice> toStack);
  }
}
