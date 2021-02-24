using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using TurtleShip.Core;

namespace TurtleShip.Core.Tests
{
  public class ShipTests
  {
    [Fact]
    public void Ship_GoesNorth_IncreasesLatitude()
    {
      var sut = new Ship(new []{"N10"});
      Assert.Equal(10, sut.Latitude);
      Assert.Equal(0, sut.Longitude);
      Assert.Equal(10, sut.ComputeManhattanDistanceToOrigin());
      Assert.Equal(90, sut.Heading);
    }

    [Fact]
    public void Ship_GoesSouth_DecreasesLatitude()
    {
      var sut = new Ship(new[] { "S20" });
      Assert.Equal(-20, sut.Latitude);
      Assert.Equal(0, sut.Longitude);
      Assert.Equal(20, sut.ComputeManhattanDistanceToOrigin());
      Assert.Equal(90, sut.Heading);
    }

    [Fact]
    public void Ship_GoesWest_IncreasesLongitude()
    {
      var sut = new Ship(new[] { "W30" });
      Assert.Equal(0, sut.Latitude);
      Assert.Equal(30, sut.Longitude);
      Assert.Equal(30, sut.ComputeManhattanDistanceToOrigin());
      Assert.Equal(90, sut.Heading);
    }

    [Fact]
    public void Ship_GoesEast_DecreasesLongitude()
    {
      var sut = new Ship(new[] { "E40" });
      Assert.Equal(0, sut.Latitude);
      Assert.Equal(-40, sut.Longitude);
      Assert.Equal(40, sut.ComputeManhattanDistanceToOrigin());
      Assert.Equal(90, sut.Heading);
    }

    [Fact]
    public void Ship_TurnRight_IncreasesHeading()
    {
      var sut = new Ship(new[] { "R50" });
      Assert.Equal(0, sut.Latitude);
      Assert.Equal(0, sut.Longitude);
      Assert.Equal(0, sut.ComputeManhattanDistanceToOrigin());
      Assert.Equal(140, sut.Heading);
    }

    [Fact]
    public void Ship_TurnLeft_DecreasesHeading()
    {
      var sut = new Ship(new[] { "L60" });
      Assert.Equal(0, sut.Latitude);
      Assert.Equal(0, sut.Longitude);
      Assert.Equal(0, sut.ComputeManhattanDistanceToOrigin());
      Assert.Equal(30, sut.Heading);
    }

    [Fact]
    public void Ship_Sail_IncreasesDistanceAsExpected()
    {
      var sut = new Ship(new []{"L30", "F10"});
      Assert.Equal(60, sut.Heading);

      Assert.True(sut.ComputeManhattanDistanceToOrigin() > 0);
    }

    [Fact]
    public void Ship_GivenInstructions_IncreasesDistanceAsExpected()
    {
      var sut = new Ship(new[]
      {
        "F10",
        "N3",
        "F7",
        "R90",
        "F11"
      });

      Assert.Equal(25, sut.ComputeManhattanDistanceToOrigin());
      Assert.Equal(-17, sut.Longitude);
      Assert.Equal(-8, sut.Latitude);
    }
  }
}
