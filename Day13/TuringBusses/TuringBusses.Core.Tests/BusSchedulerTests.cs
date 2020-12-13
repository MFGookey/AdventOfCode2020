using System;
using System.Collections.Generic;
using System.Text;
using Common.Utilities.Formatter;
using Xunit;

namespace TuringBusses.Core.Tests
{
  public class BusSchedulerTests
  {
    [Fact]
    public void GetNextBusProduct_GivenValidTimestampAndBusses_ReturnsExpectedResult()
    {
      var sut = new BusScheduler(new RecordFormatter(null));
      var result = sut.GetNextBusProduct(939, "7,13,x,x,59,x,31,19");

      Assert.Equal(295, result);
    }
  }
}
