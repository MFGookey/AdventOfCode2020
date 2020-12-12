using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TurtleShip.Core
{
  /// <summary>
  /// Represents navigation aboard the MVConway
  /// </summary>
  public class Ship
  {
    public int Heading
    {
      get; private set;
    }

    public double Longitude { get; private set; }

    public double Latitude { get; private set; }

    public Ship(string[] instructions)
    {
      Heading = 90; // Due east
      Longitude = 0;
      Latitude = 0;

      foreach (var instruction in instructions)
      {
        ProcessInstruction(instruction);
      }
    }

    public double ComputeManhattanDistanceToOrigin()
    {
      return Math.Abs(Longitude) + Math.Abs(Latitude);
    }

    /// <summary>
    /// Turn the ship either left or right.
    /// </summary>
    /// <param name="change">The number of degrees to add or subtract from our current heading</param>
    private void SetHeading(int change)
    {
      // North is 0
      // East is 90
      // South is 180
      // West is 270
      Heading = (((Heading + change) % 360) + 360) % 360;
    }

    /// <summary>
    /// Adjust our latitude directly.  North is addition, South is subtraction.
    /// </summary>
    /// <param name="toAdjust">The amount of units to adjust our latutudinal position.</param>
    private void AdjustLatitude(double toAdjust)
    {
      Latitude += toAdjust;
    }

    /// <summary>
    /// Adjust our longitude directly.  West is addition, East is subtraction.
    /// </summary>
    /// <param name="toAdjust">The amount of units to adjust our longitudinal position.</param>
    private void AdjustLongitude(double toAdjust)
    {
      Longitude += toAdjust;
    }

    /// <summary>
    /// Sail along our current heading.  Forward is positive, backward is negative.
    /// </summary>
    /// <param name="distance">The distance to sail on our current heading.</param>
    private void Sail(int distance)
    {
      // Here's the fun part.  Based on our heading, the change in position is split into two components.
      // Our longitudinal component is equivalent to the -sin() of our bearing
      // Our latitudinal component is equivalent to the cos() of our bearing.
      AdjustLatitude(-1*Math.Sin(Math.PI*Heading/180)*distance);

      // Here's the fun part.  Based on our heading, the change in position is split into two components.
      // Our longitudinal component is equivalent to the -sin() of our bearing
      // Our latitudinal component is equivalent to the cos() of our bearing.
      AdjustLatitude(Math.Cos(Math.PI * Heading / 180) * distance);
    }

    private void ProcessInstruction(string instruction)
    {
      var match = Regex.Match(instruction, @"^([NSEWLRF])(\d+)$");
      string operation = match.Groups[1].Value;
      int value = int.Parse(match.Groups[2].Value);

      switch (operation)
      {
        case "S":
          AdjustLatitude(-value);
          break;
        case "N":
          AdjustLatitude(value);
          break;
        case "E":
          AdjustLongitude(-value);
          break;
        case "W":
          AdjustLongitude(value);
          break;
        case "L":
          SetHeading(-value);
          break;
        case "R":
          SetHeading(value);
          break;
        case "F":
          Sail(value);
          break;
        default:
          throw new Exception("Shouldn't be here");
      }
    }
  }
}
