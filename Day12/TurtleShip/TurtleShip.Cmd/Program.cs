using System;
using Common.Utilities.IO;
using TurtleShip.Core;

namespace TurtleShip.Cmd
{
  /// <summary>
  /// The entrypoint for TurtleShip.Cmd
  /// </summary>
  public class Program
  {
    /// <summary>
    /// TurtleShip.Cmd entry point
    /// </summary>
    /// <param name="args">Command line arguments (not used)</param>
    static void Main(string[] args)
    {
      var filePath = "./input";
      var reader = new FileReader();
      var instructions = reader.ReadFileByLines(filePath);
      var ship = new Ship(instructions);
      Console.WriteLine(Math.Round(ship.ComputeManhattanDistanceToOrigin()));

      var waypointShip = new WaypointShip(instructions);
      Console.WriteLine(waypointShip.ComputeManhattanDistanceToOrigin());
    }
  }
}
