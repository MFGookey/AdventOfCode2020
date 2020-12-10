using System;
using System.Collections.Generic;
using System.Text;

namespace AdaptrStackr.Core.Device
{
  /// <summary>
  /// Represents an Adapter that accepts a range of "jolts" and emits a particular number of "jolts"
  /// </summary>
  public interface IAdapter : ISocket, IChargeableDevice
  {
  }
}
