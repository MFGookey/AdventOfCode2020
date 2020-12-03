using System;
using System.Collections.Generic;
using System.Text;
using TobogganPlotter.Core.Map.Model;
using Utilities.IO;
using System.Linq;

namespace TobogganPlotter.Core.Map
{
  /// <summary>
  /// Abstract class to represent a Map and handle the common tasks for me
  /// </summary>
  public abstract class Map : IMap
  {
    private IFileReader _reader;
    private string[] _map;

    /// <summary>
    /// Gets or sets a copy of the rectangular map data
    /// </summary>
    protected string[] MapData
    {
      get
      {
        return (string[])_map.Clone();
      }

      private set
      {
        if (value is null)
        {
          throw new ArgumentNullException("Map Data may not be null");
        }

        if (value.Select(x => x.Length).Distinct().Count() > 1)
        {
          throw new FormatException("Map Data must be rectangular, found more than one distinct line length");
        }

        _map = (string[])value.Clone();
      }
    }

    /// <inheritdoc/>
    public int MapXSize
    {
      get
      {
        var firstValue = _map.FirstOrDefault();
        
        if (firstValue is null)
        {
          firstValue = string.Empty;
        }

        return firstValue.Length;
      }

      private set
      {
        throw new NotImplementedException();
      }
    }

    /// <inheritdoc/>
    public int MapYSize
    {
      get
      {
        return _map.Length;
      }

      private set
      {
        throw new NotImplementedException();
      }
    }

    /// <summary>
    /// Initializes a new Map with default settings
    /// </summary>
    public Map() : this(new string[] { })
    { }

    /// <summary>
    /// Initializes a new map with a given string array of map data
    /// </summary>
    /// <param name="map">The map data to initialize with</param>
    public Map(string[] map) : this(map, new FileReader())
    { }

    /// <summary>
    /// Initializes a new map with default map data and a given IFileReader for file IO
    /// </summary>
    /// <param name="reader">The IFileReader to use for file access</param>
    public Map(IFileReader reader) : this(new string[] { }, reader)
    { }

    /// <summary>
    /// Initialize a new map with a given string array of map data, and a given IFileReader for file access
    /// </summary>
    /// <param name="map">The map data to initialize with</param>
    /// <param name="map">The map data to initialize with</param>
    public Map(string[] map, IFileReader reader)
    {
      MapData = map;
      _reader = reader;
    }

    /// <inheritdoc/>
    public abstract char GetMapCell(Point point);

    /// <inheritdoc/>
    public void LoadMap(string[] mapStrings)
    {
      MapData = mapStrings;
    }

    /// <inheritdoc/>
    public void LoadMap(string mapString)
    {
      MapData = mapString.Split('\n');
    }

    /// <inheritdoc/>
    public void LoadMapFile(string path)
    {
      MapData = _reader.ReadFileByLines(path);
    }
  }
}
