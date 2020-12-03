using System;
using System.Collections.Generic;
using System.Text;
using TobogganPlotter.Core.Map;
using TobogganPlotter.Core.Map.Model;

namespace TobogganPlotter.Core
{
  /// <summary>
  /// Finds the number of trees hit on a map, given a particular move vector.
  /// </summary>
  public class TobogganRun
  {
    private IMap _map;

    /// <summary>
    /// Initialize an instance TobogganRun with a given map
    /// </summary>
    /// <param name="map">The map to use for the toboggan runs</param>
    public TobogganRun(IMap map)
    {
      _map = map;
    }

    /// <summary>
    /// Count the trees hit on a Toboggan Run given beginning at 0,0 and moving to a given point relative to your current point.
    /// </summary>
    /// <param name="move">The move to make</param>
    /// <returns>A count of trees encountered</returns>
    public int TreeCount(Point move)
    {
      return TreeCount(move, new Point(0, 0));
    }

    /// <summary>
    /// Count the trees hit on a Toboggan Run given beginning at a given point and moving to a given point relative to your current point.
    /// </summary>
    /// <param name="move">The move to make</param>
    /// <param name="startPosition">The position where the toboggan run began</param>
    /// <returns>A count of trees encountered</returns>
    public int TreeCount(Point move, Point startPosition)
    {
      if (move.Y <= 0)
      {
        throw new ArgumentException("move.Y must be greater than 0", nameof(move));
      }

      var treeCount = 0;

      var currentPoint = new Point(startPosition.X, startPosition.Y);

      while (currentPoint.Y < _map.MapYSize)
      {
        var symbol = _map.GetMapCell(currentPoint);

        if (symbol == '#')
        {
          treeCount++;
        }

        currentPoint += move;
      }

      return treeCount;
    }
  }
}
