using System;
using System.Collections.Generic;
using System.Text;
using TobogganPlotter.Core.Map.Model;

namespace TobogganPlotter.Core.Map
{
  /// <summary>
  /// Represents a map containing free spaces and non-free spaces
  /// </summary>
  public interface IMap
  {
    /// <summary>
    /// Gets a value representing the size of the map's X axis
    /// </summary>
    int MapXSize
    {
      get;
    }

    /// <summary>
    /// Gets a value representing the size of the map's Y axis
    /// </summary>
    int MapYSize
    {
      get;
    }


    /// <summary>
    /// Load a map from a rectangular string array, one string per row, and the same number of characters in each string
    /// </summary>
    /// <param name="mapString">The map to load</param>
    void LoadMap(string[] mapString);

    /// <summary>
    /// Load a map from a single string, each row ending at newline, and the same number of characters in each row
    /// </summary>
    /// <param name="mapString">The map to load</param>
    void LoadMap(string mapString);

    /// <summary>
    /// Load a map from a file, each row ending at newline, and the same number of characters in each row
    /// </summary>
    /// <param name="path">The filepath to load</param>
    void LoadMapFile(string path);

    /// <summary>
    /// Retrieve the character on the map at a given point
    /// </summary>
    /// <param name="point">The point to check</param>
    /// <returns>The character located at the specified point</returns>
    char GetMapCell(Point point);
  }
}
