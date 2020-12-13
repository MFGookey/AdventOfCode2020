using System;
using System.Collections.Generic;
using System.Text;

namespace TurtleShip.Core
{
  public class WaypointShip : Ship
  {

    public double WaypointRelativeLatitude { get; protected set; }

    public double WaypointRelativeLongitude { get; protected set; }

    /// <inheritdoc />
    public WaypointShip(string[] instructions)
    {
      WaypointRelativeLatitude = 1; // 1 unit north

      WaypointRelativeLongitude = -10; // 10 units east

      ProcessInstructions(instructions);
    }

    /// <summary>
    /// Adjust the relative longitude of the waypoint
    /// </summary>
    /// <param name="toAdjust">The number of units by which the longitude should be adjusted.</param>
    protected override void AdjustLongitude(double toAdjust)
    {
      WaypointRelativeLongitude += toAdjust;
    }

    /// <summary>
    /// Adjust the relative latitude of the waypoint
    /// </summary>
    /// <param name="toAdjust">The number of units by which the latitude should be adjusted.</param>
    protected override void AdjustLatitude(double toAdjust)
    {
      WaypointRelativeLatitude += toAdjust;
    }

    protected override void Sail(int distance)
    {
      Latitude += Math.Round(WaypointRelativeLatitude) * distance;
      Longitude += Math.Round(WaypointRelativeLongitude) * distance;
    }

    /// <summary>
    /// Rotate the waypoint around the ship, by the number of degrees specified by change
    /// </summary>
    /// <param name="change">The number of degrees to change.  Clockwise is positive, counterclockwise is negative.</param>
    protected override void SetHeading(int change)
    {
      double cosine = Math.Cos((90 - change) * Math.PI / 180);
      double sine = Math.Sin((90 - change) * Math.PI / 180);
      
      var newLongitude = WaypointRelativeLongitude * sine;
      newLongitude -= WaypointRelativeLatitude * cosine;

      var newLatitude = WaypointRelativeLongitude * cosine;
      newLatitude += WaypointRelativeLatitude * sine;

      WaypointRelativeLatitude = Math.Round(newLatitude);
      WaypointRelativeLongitude = Math.Round(newLongitude);
    }
  }
}
