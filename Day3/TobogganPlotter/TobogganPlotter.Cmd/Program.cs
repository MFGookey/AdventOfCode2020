using System;
using TobogganPlotter.Core;
using TobogganPlotter.Core.Map;
using TobogganPlotter.Core.Map.Model;

namespace TobogganPlotter.Cmd
{
  class Program
  {
    static void Main(string[] args)
    {
      var move = new Point(3, 1);
      var map = new HorizontalTilingMap();
      map.LoadMapFile(@"C:\Dev\GitHub\AdventOfCode2020\Day3\Data\input");
      var run = new TobogganRun(map);

      var possibleRuns = new Point[]
      {
        new Point(1,1),
        new Point(3,1),
        new Point(5,1),
        new Point(7,1),
        new Point(1,2)
      };

      var currentProduct = 1;

      foreach (var possibleRun in possibleRuns)
      {

        var treeCount = run.TreeCount(possibleRun);
        currentProduct *= treeCount;
        Console.WriteLine(treeCount);
      }

      Console.WriteLine(currentProduct);
    }
  }
}
