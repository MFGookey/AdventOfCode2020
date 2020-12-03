using System;
using TobogganPlotter.Core.Map;
using TobogganPlotter.Core.Map.Model;

namespace TobogganPlotter.Cmd
{
  class Program
  {
    static void Main(string[] args)
    {
      var move = new Point(3, 1);
      var currentPoint = new Point(0, 0);
      var map = new HorizontalTilingMap();
      map.LoadMapFile(@"C:\Dev\GitHub\AdventOfCode2020\Day3\Data\input");
      var treeCount = 0;
      
      while (currentPoint.Y < map.MapYSize)
      {
        var symbol = map.GetMapCell(currentPoint);
        if (symbol == '#')
        {
          treeCount++;
        }

        currentPoint += move;
      }

      Console.WriteLine(treeCount);
    }
  }
}
