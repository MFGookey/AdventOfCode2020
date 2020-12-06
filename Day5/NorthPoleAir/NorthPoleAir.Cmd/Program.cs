using System;
using Common.Utilities.IO;
using NorthPoleAir.Core;
using System.Linq;

namespace NorthPoleAir.Cmd
{
  /// <summary>
  /// The entrypoint for NorthPoleAir.Cmd
  /// </summary>
  public class Program
  {
    /// <summary>
    /// NorthPoleAir.Cmd entry point
    /// </summary>
    /// <param name="args">Command line arguments (not used)</param>
    static void Main(string[] args)
    {
      var filePath = "./input";
      var reader = new FileReader();
      var plane = new Plane(128, 8);
      var boardingPasses = reader.ReadFileByLines(filePath).Select(l => new BoardingPass(plane, l));
      var maxId = boardingPasses.Select(p => p.SeatId).Max();
      Console.WriteLine(maxId);
      var seatIds = boardingPasses.Select(pass => pass.SeatId);
      var idRange = Enumerable.Range(seatIds.Min(), seatIds.Max());

      var missingId = idRange.Except(seatIds).First();
      Console.WriteLine(missingId);
    }
  }
}
