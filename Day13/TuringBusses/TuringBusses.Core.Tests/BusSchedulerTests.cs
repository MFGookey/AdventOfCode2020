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

    [Theory]
    [InlineData("7,13,x,x,59,x,31,19", 1068781)]
    [InlineData("17,x,13,19", 3417)]
    [InlineData("67,7,59,61", 754018)]
    [InlineData("67,x,7,59,61", 779210)]
    [InlineData("67,7,x,59,61", 1261476)]
    [InlineData("1789,37,47,1889", 1202161486)]
    public void WinContest_GivenSchedule_ReturnsExpectedValue(string schedule, long expectedValue)
    {
      var sut = new BusScheduler(new RecordFormatter(null));
      var result = sut.WinContest(schedule);
      Assert.Equal(expectedValue, result);
    }
  }
}
