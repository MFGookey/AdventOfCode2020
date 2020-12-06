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

      // Looking for an unclaimed seat with neighbors
      // Means we're looking for a pair of seats where one does not exist between them.
      var missingId = boardingPasses
        // self join to boarding passes where the seatID is one higher
        .GroupJoin(
          boardingPasses,
          p1 => p1.SeatId,
          p2 => p2.SeatId - 1,
          (lowerPass, upperPassEnumerable) => new { lowerPass, upperPassEnumerable }
        )
        
        // flatten the upperPass IEnumerable, and default to null if the enumerable is empty
        // map into a lower pass and an upper pass
        .SelectMany(
          pair => pair.upperPassEnumerable.DefaultIfEmpty(),
          (lowerPair, upperPass) => new { lowerPair.lowerPass, upperPass }
        )
        
        // Preemptively filter out anything where we have a matching pair.  We only want ones where the upperPass is not found
        .Where(pair => pair.upperPass is null)

        // Join again to boarding passes, on passes whose seat IDs are 2 higher than our pair's lower seatId
        .GroupJoin(
          boardingPasses,
          p1 => p1.lowerPass.SeatId,
          p2 => p2.SeatId - 2,
          (lowerPair, upperPassEnumerable) => new { lowerPair.lowerPass, middlePass = lowerPair.upperPass, upperPassEnumerable }
        )

        // Once again flatten the upperPassEnumerable and default to null if it is empty
        // Map into a trio of passes
        .SelectMany(
          passTrio => passTrio.upperPassEnumerable.DefaultIfEmpty(),
          (triple, upperPass) => new { triple.lowerPass, triple.middlePass, upperPass }
        )

        // Filter to only cases where we have a non-null set of outer passes, and a null middle pass.
        .Where(passTrio => (passTrio.lowerPass != null && passTrio.middlePass is null && passTrio.upperPass != null))

        // Derive the missing ID from the upper and lower SeatIds
        .Select(p => new { missingId = (p.upperPass.SeatId + p.lowerPass.SeatId) / 2 } )

        // Give the first entry in the final set
        .FirstOrDefault();

      Console.WriteLine(missingId);
    }
  }
}
