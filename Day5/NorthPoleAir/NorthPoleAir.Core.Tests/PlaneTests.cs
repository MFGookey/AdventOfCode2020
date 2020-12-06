using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using NorthPoleAir.Core;

namespace NorthPoleAir.Core.Tests
{
  public class PlaneTests
  {
    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    [InlineData(-1, 1)]
    [InlineData(1, -1)]
    public void Plane_ConstructorGivenInvalidValues_ThrowsArgumentException(int rows, int seats)
    {
      Assert.Throws<ArgumentException>(() => { var sut = new Plane(rows, seats); });
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(10, 1)]
    [InlineData(10, 10)]
    [InlineData(1, 10)]
    public void Constructor_GivenValidValues_SetsValuesAsExpected(int rows, int seats)
    {
      var sut = new Plane(rows, seats);

      Assert.Equal(rows, sut.Rows);
      Assert.Equal(seats, sut.Seats);
    }
  }
}
