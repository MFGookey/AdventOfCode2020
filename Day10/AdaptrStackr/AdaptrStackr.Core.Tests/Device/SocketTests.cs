using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using AdaptrStackr.Core.Device;

namespace AdaptrStackr.Core.Tests.Device
{
  public class SocketTests
  {
    [Fact]
    public void Socket_Output_ReturnsZero()
    {
      var sut = new Socket();
      Assert.Equal(0, sut.Output);
    }
  }
}
