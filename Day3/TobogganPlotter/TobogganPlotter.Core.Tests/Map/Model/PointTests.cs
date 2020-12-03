using System;
using System.Collections.Generic;
using System.Text;
using sut = TobogganPlotter.Core.Map.Model;
using Xunit;

namespace TobogganPlotter.Core.Tests.Map.Model
{
  public class PointTests
  {
    [Fact]
    public void Point_GivenValues_SetsValuesCorrectly()
    {
      var x = 1;
      var y = 9999;
      var sut = new sut.Point(1, 9999);

      Assert.Equal(x, sut.X);
      Assert.Equal(y, sut.Y);
    }

    [Theory]
    [InlineData(1, 2, 3, 4, false)]
    [InlineData(1, 2, 1, 4, false)]
    [InlineData(1, 2, 3, 2, false)]
    [InlineData(1, 2, 1, 2, true)]
    public void Equals_GivenValues_ReturnsExpectedValues(int x1, int y1, int x2, int y2, bool expectedValue)
    {
      var sut1 = new sut.Point(x1, y1);
      var sut2 = new sut.Point(x2, y2);

      Assert.Equal(expectedValue, sut1.Equals(sut2));
      Assert.Equal(expectedValue, sut2.Equals(sut1));
    }

    [Fact]
    public void Equals_WhenComparingToSelf_ReturnsTrue()
    {
      var sut = new sut.Point(10, 20);
      Assert.True(sut.Equals(sut));
    }

    [Theory]
    [InlineData(1, 2, 1, 4)]
    [InlineData(1, 2, -1, -4)]
    [InlineData(0, 0, 0, 0)]
    public void PlusEquals_WhenAdding_ReturnsExpectedValue(int x1, int y1, int x2, int y2)
    {
      var sut = new sut.Point(x1, y1);
      sut += new sut.Point(x2, y2);
      Assert.Equal(new sut.Point(x1 + x2, y1 + y2), sut);
    }

    [Theory]
    [InlineData(1, 2, 1, 4)]
    [InlineData(1, 2, -1, -4)]
    [InlineData(0, 0, 0, 0)]
    public void MinusEquals_WhenSubtracting_ReturnsExpectedValue(int x1, int y1, int x2, int y2)
    {
      var sut = new sut.Point(x1, y1);
      sut -= new sut.Point(x2, y2);
      Assert.Equal(new sut.Point(x1 - x2, y1 - y2), sut);
    }

    [Theory]
    [InlineData(1, 2, 1, 4)]
    [InlineData(1, 2, -1, -4)]
    [InlineData(0, 0, 0, 0)]
    public void Plus_WhenAdding_ReturnsExpectedValue(int x1, int y1, int x2, int y2)
    {
      var sut = new sut.Point(x1, y1) + new sut.Point(x2, y2);
      Assert.Equal(new sut.Point(x1 + x2, y1 + y2), sut);
    }

    [Theory]
    [InlineData(1, 2, 1, 4)]
    [InlineData(1, 2, -1, -4)]
    [InlineData(0, 0, 0, 0)]
    public void Minus_WhenSubtracting_ReturnsExpectedValue(int x1, int y1, int x2, int y2)
    {
      var sut = new sut.Point(x1, y1) - new sut.Point(x2, y2);
      Assert.Equal(new sut.Point(x1 - x2, y1 - y2), sut);
    }
  }
}
