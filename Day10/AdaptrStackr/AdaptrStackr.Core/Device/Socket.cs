using System;
using System.Collections.Generic;
using System.Text;

namespace AdaptrStackr.Core.Device
{
  /// <summary>
  /// Represents the charging socket which is the baseline
  /// </summary>
  public class Socket : ISocket
  {
    /// <inheritdoc/>
    public int Output => 0;
  }
}
