using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;


namespace NorthPoleAir.Core
{
  /// <inheritdoc cref="IBoardingPass"/>
  public class BoardingPass : IBoardingPass
  {
    /// <inheritdoc/>
    public string SeatPath
    {
      get; private set;
    }

    /// <inheritdoc/>
    public int Row
    {
      get; private set;
    }

    /// <inheritdoc/>
    public int Seat
    {
      get; private set;
    }

    /// <inheritdoc/>
    public IPlane Plane
    {
      get; private set;
    }

    public int SeatId
    {
      get
      {
        return Row * Plane.Seats + Seat;
      }
    }

    /// <summary>
    /// Initializes a new instance of the BoardingPass class, for a plane of a given size and with a known path to the seat
    /// </summary>
    /// <param name="plane">The plane to which this boarding pass applies</param>
    /// <param name="seatPath">The path to the boarding pass holder's seat</param>
    public BoardingPass(IPlane plane, string seatPath)
    {
      Plane = plane;
      SeatPath = seatPath;

      FindSeat();
    }

    /// <summary>
    /// Process SeatPath in order to find the user's seat.
    /// </summary>
    private void FindSeat()
    {
      var rowCharacters = (int)Math.Ceiling(Math.Log2(Plane.Rows));
      var seatCharacters = (int)Math.Ceiling(Math.Log2(Plane.Seats));
      
      if (Regex.IsMatch(SeatPath, $"[FB]{{{rowCharacters}}}[LR]{{{seatCharacters}}}") == false)
      {
        throw new FormatException($"Seatpath \"{SeatPath}\" is invalid on a plane with {Plane.Rows} rows and {Plane.Seats} seats per row!  Expecting {rowCharacters} characters of \"F\" and \"B\" followed by {seatCharacters} characters of \"L\" and \"R\"");
      }

      Row = BinaryWalk(SeatPath.Substring(0, rowCharacters), Plane.Rows - 1, 'F', 'B');
      Seat = BinaryWalk(SeatPath.Substring(rowCharacters), Plane.Seats - 1, 'L', 'R');
    }

    /// <summary>
    /// Use a binary walk to determine a position in a range using a given path
    /// </summary>
    /// <param name="path">The path to walk</param>
    /// <param name="initialUpperBound">The initial number of possible positions</param>
    /// <param name="lower">The character indicating using the lower half of the path</param>
    /// <param name="upper">The character indicating using the upper half of the path</param>
    /// <returns>The final position, using a 0 based index</returns>
    private int BinaryWalk(string path, int initialUpperBound, char lower, char upper)
    {
      var lowerBound = 0;
      var upperBound = initialUpperBound;

      if (Regex.IsMatch(path, $"[^{lower}{upper}]", RegexOptions.IgnoreCase))
      {
        throw new FormatException($"Path {path} is invalid because it is not only \"{lower}\" or \"{upper}\" characters");
      }

      for (var i = 0; i < path.Length; i++)
      {
        if (path[i].Equals(lower))
        {
          // If we are in the lower half, leave lowerBound alone, and reduce upperBound
          upperBound = ((upperBound + lowerBound + 1) / 2) - 1;
        }
        else
        {
          // If we are in the upper half, leave upperBound alone, and increase lowerBound
          lowerBound = (upperBound + lowerBound + 1) / 2;
        }
      }

      if (lowerBound != upperBound)
      {
        throw new FormatException($"Could not uniquely identify a position from {initialUpperBound} options using the path \"{path}\".  Final Lower Bound: {lowerBound}  Final Upper Bound: {upperBound}");
      }

      return lowerBound;
    }
  }
}
