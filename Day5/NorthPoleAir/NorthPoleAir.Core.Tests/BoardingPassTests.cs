using System;
using System.Collections.Generic;
using System.Text;
using NorthPoleAir.Core;
using Xunit;
using Moq;

namespace NorthPoleAir.Core.Tests
{
  public class BoardingPassTests
  {
    [Theory]
    [InlineData(0, 0, "FFFRRR")]
    [InlineData(8, -8, "FFFRRR")]
    [InlineData(-8, -8, "FFFRRR")]
    [InlineData(-8, 8, "FFFRRR")]
    [InlineData(0, 0, "BBBRRR")]
    [InlineData(8, -8, "BBBRRR")]
    [InlineData(-8, -8, "BBBRRR")]
    [InlineData(-8, 8, "BBBRRR")]
    [InlineData(0, 0, "FFFLLL")]
    [InlineData(8, -8, "FFFLLL")]
    [InlineData(-8, -8, "FFFLLL")]
    [InlineData(-8, 8, "FFFLLL")]
    [InlineData(0, 0, "BBBLLL")]
    [InlineData(8, -8, "BBBLLL")]
    [InlineData(-8, -8, "BBBLLL")]
    [InlineData(-8, 8, "BBBLLL")]
    public void Constructor_GivenInvalidPlane_ThrowsFormatException(int rows, int seats, string seatPath)
    {
      var plane = new Mock<IPlane>();
      plane.SetupGet(p => p.Rows).Returns(rows);
      plane.SetupGet(p => p.Seats).Returns(seats);
      Assert.Throws<FormatException>(() => { var sut = new BoardingPass(plane.Object, seatPath); });
    }

    [Theory]
    [InlineData(8, 8, "")]
    [InlineData(1, 1, "FFFRRR")]
    [InlineData(8, 8, "asdffdsa")]
    [InlineData(8, 8, "FFRFRR")]
    [InlineData(8, 8, "RRRFFF")]
    [InlineData(1, 1, "BBBRRR")]
    [InlineData(8, 8, "BBRBRR")]
    [InlineData(8, 8, "RRRBBB")]
    [InlineData(1, 1, "FFFLLL")]
    [InlineData(8, 8, "FFLFLL")]
    [InlineData(8, 8, "LLLFFF")]
    [InlineData(1, 1, "BBBLLL")]
    [InlineData(8, 8, "BBLBLL")]
    [InlineData(8, 8, "LLLBBB")]
    public void Constructor_GivenValidPlaneAndInvalidPath_ThrowsFormatException(int rows, int seats, string seatPath)
    {
      var plane = new Mock<IPlane>();
      plane.SetupGet(p => p.Rows).Returns(rows);
      plane.SetupGet(p => p.Seats).Returns(seats);
      Assert.Throws<FormatException>(() => { var sut = new BoardingPass(plane.Object, seatPath); });
    }

    [Theory]
    [InlineData(8, 8, "FFFRRR")]
    [InlineData(4, 4, "FBRL")]
    [InlineData(2, 2, "FL")]
    [InlineData(8, 8, "BBBLLL")]
    [InlineData(4, 4, "BFLR")]
    [InlineData(2, 2, "BR")]
    public void Constructor_GivenValidInformation_SetsPlaneAppropriately(int rows, int seats, string seatPath)
    {
      var plane = new Mock<IPlane>();
      plane.SetupGet(p => p.Rows).Returns(rows);
      plane.SetupGet(p => p.Seats).Returns(seats);
      var sut = new BoardingPass(plane.Object, seatPath);

      Assert.Equal(plane.Object.Rows, sut.Plane.Rows);
      Assert.Equal(plane.Object.Seats, sut.Plane.Seats);
    }

    [Theory]
    [InlineData(8, 8, "FFFRRR")]
    [InlineData(4, 4, "FBRL")]
    [InlineData(2, 2, "FL")]
    [InlineData(8, 8, "BBBLLL")]
    [InlineData(4, 4, "BFLR")]
    [InlineData(2, 2, "BR")]
    public void Constructor_GivenValidInformation_SetsSeatPathAppropriately(int rows, int seats, string seatPath)
    {
      var plane = new Mock<IPlane>();
      plane.SetupGet(p => p.Rows).Returns(rows);
      plane.SetupGet(p => p.Seats).Returns(seats);
      var sut = new BoardingPass(plane.Object, seatPath);

      Assert.Equal(seatPath, sut.SeatPath);
    }

    [Theory]
    [InlineData(128, 8, "FBFBBFFRLR", 44, 5, 357)]
    [InlineData(128, 8, "BFFFBBFRRR", 70, 7, 567)]
    [InlineData(128, 8, "FFFBBBFRRR", 14, 7, 119)]
    [InlineData(128, 8, "BBFFBBFRLL", 102, 4, 820)]
    public void Constructor_GivenValidInformation_SetsRowSeatAndSeatIdAppropriately(int rows, int seats, string seatPath, int expectedRow, int expectedSeat, int expectedSeatId)
    {
      var plane = new Mock<IPlane>();
      plane.SetupGet(p => p.Rows).Returns(rows);
      plane.SetupGet(p => p.Seats).Returns(seats);
      var sut = new BoardingPass(plane.Object, seatPath);

      Assert.Equal(expectedRow, sut.Row);
      Assert.Equal(expectedSeat, sut.Seat);
      Assert.Equal(expectedSeatId, sut.SeatId);
    }
  }
}
