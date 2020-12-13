using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using TurtleShip.Core;

namespace TurtleShip.Core.Tests
{
  public class WaypointShipTests
  {
    [Fact]
    public void WaypointShip_North_IncreasesWaypointRelativeLatitude()
    {
      var sut = new WaypointShip(new []{"N10"});
      Assert.Equal(0, sut.Latitude);
      Assert.Equal(0, sut.Longitude);
      Assert.Equal(0, sut.ComputeManhattanDistanceToOrigin());
      // Waypoint starts 1 unit north.
      Assert.Equal(10 + 1, sut.WaypointRelativeLatitude);
      Assert.Equal(-10, sut.WaypointRelativeLongitude);
    }

    [Fact]
    public void WaypointShip_South_DecreasesWaypointRelativeLatitude()
    {
      var sut = new WaypointShip(new[] { "S20" });
      Assert.Equal(0, sut.Latitude);
      Assert.Equal(0, sut.Longitude);
      Assert.Equal(0, sut.ComputeManhattanDistanceToOrigin());
      Assert.Equal(-20 + 1, sut.WaypointRelativeLatitude);
      Assert.Equal(-10, sut.WaypointRelativeLongitude);
    }

    [Fact]
    public void WaypointShip_West_IncreasesWaypointRelativeLongitude()
    {
      var sut = new WaypointShip(new[] { "W30" });
      Assert.Equal(0, sut.Latitude);
      Assert.Equal(0, sut.Longitude);
      Assert.Equal(0, sut.ComputeManhattanDistanceToOrigin());
      Assert.Equal(1, sut.WaypointRelativeLatitude);
      Assert.Equal(-10 + 30, sut.WaypointRelativeLongitude);
    }

    [Fact]
    public void WaypointShip_East_DecreasesWaypointRelativeLongitude()
    {
      var sut = new WaypointShip(new[] { "E40" });
      Assert.Equal(0, sut.Latitude);
      Assert.Equal(0, sut.Longitude);
      Assert.Equal(0, sut.ComputeManhattanDistanceToOrigin());
      Assert.Equal(1, sut.WaypointRelativeLatitude);
      Assert.Equal(-10 + -40, sut.WaypointRelativeLongitude);
    }

    [Fact]
    public void WaypointShip_TurnRight_MovesWaypointAsExpected()
    {
      var sut = new WaypointShip(new[] { "R90" });
      Assert.Equal(0, sut.Latitude);
      Assert.Equal(0, sut.Longitude);
      Assert.Equal(0, sut.ComputeManhattanDistanceToOrigin());
      Assert.Equal(-10, sut.WaypointRelativeLatitude);
      Assert.Equal(-1, sut.WaypointRelativeLongitude);
    }

    [Fact]
    public void WaypointShip_TurnLeft_MovesWaypointAsExpected()
    {
      var sut = new WaypointShip(new[] { "L90" });
      Assert.Equal(0, sut.Latitude);
      Assert.Equal(0, sut.Longitude);
      Assert.Equal(0, sut.ComputeManhattanDistanceToOrigin());
      Assert.Equal(10, Math.Round(sut.WaypointRelativeLatitude));
      Assert.Equal(1, Math.Round(sut.WaypointRelativeLongitude));
    }

    [Fact]
    public void WaypointShip_GivenInstructions_IncreasesDistanceAsExpected()
    {
      var sut = new WaypointShip(new[]
      {
        "F10",
        "N3",
        "F7",
        "R90",
        "F11"
      });

      Assert.Equal(286, sut.ComputeManhattanDistanceToOrigin());
      Assert.Equal(-214, sut.Longitude);
      Assert.Equal(-72, sut.Latitude);
    }

    [Theory]
    [InlineData("R0", 1, -10)]
    [InlineData("R90", -10, -1)]
    [InlineData("R180", -1, 10)]
    [InlineData("R270", 10, 1)]
    [InlineData("R360", 1, -10)]
    public void WayPointShip_GivenTurns_MovesWaypointAsExpected(string instruction, int expectedLatitude, int expectedLongitude)
    {
      // Waypoint begins at -10 long, +1 lat
      var sut = new WaypointShip(new[] {instruction});
      Assert.Equal(expectedLatitude, Math.Round(sut.WaypointRelativeLatitude));
      Assert.Equal(expectedLongitude, Math.Round(sut.WaypointRelativeLongitude));
    }
  }
}
