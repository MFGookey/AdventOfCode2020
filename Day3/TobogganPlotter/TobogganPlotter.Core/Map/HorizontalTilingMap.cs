using System;
using System.Collections.Generic;
using System.Text;
using TobogganPlotter.Core.Map.Model;
using Utilities.IO;

namespace TobogganPlotter.Core.Map
{
  public class HorizontalTilingMap : Map
  {
    /// <inheritdoc/>
    public HorizontalTilingMap() : base() { }

    /// <inheritdoc/>
    public HorizontalTilingMap(string[] map) : base(map) { }

    /// <inheritdoc/>
    public HorizontalTilingMap(IFileReader reader) : base(reader) { }

    /// <inheritdoc/>
    public HorizontalTilingMap(string[] map, IFileReader reader) : base(map, reader) { }

    /// <summary>
    /// Gets the cell value for the horizontally tiling map of the given point
    /// </summary>
    /// <param name="point">The point to find</param>
    /// <returns>The character at the point</returns>
    public override char GetMapCell(Point point)
    {
      return MapData[point.Y][point.X % MapXSize];
    }
  }
}
