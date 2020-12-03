using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using sut = TobogganPlotter.Core;
using TobogganPlotter.Core.Map;
using TobogganPlotter.Core.Map.Model;

namespace TobogganPlotter.Core.Tests
{
  public class TobogganRunTests
  {
    [Theory]
    [InlineData(0, 1, 3)]
    [InlineData(3, 1, 7)]
    [InlineData(3, 2, 2)]
    [InlineData(6, 2, 4)]
    public void TreeCount_ValidMoveFromOrigin_ReturnsExpectedValue(int moveX, int moveY, int expectedTrees)
    {
      var map = new HorizontalTilingMap();
      map.LoadMap(@"..##.......
#...#...#..
.#....#..#.
..#.#...#.#
.#...##..#.
..#.##.....
.#.#.#....#
.#........#
#.##...#...
#...##....#
.#..#...#.#");

      var move = new Point(moveX, moveY);

      var sut = new sut.TobogganRun(map);

      var result = sut.TreeCount(move);

      Assert.Equal(expectedTrees, result);
    }

    [Theory]
    [InlineData(0, 1, 1, 0, 5)]
    [InlineData(3, 1, 10, 0, 2)]
    [InlineData(3, 1, 0, 10, 0)]
    [InlineData(3, 2, 7, 0, 1)]
    public void TreeCount_ValidMoveFromArbitraryPoint_ReturnsExpectedValue(int moveX, int moveY, int startX, int startY, int expectedTrees)
    {
      var map = new HorizontalTilingMap();
      map.LoadMap(@"..##.......
#...#...#..
.#....#..#.
..#.#...#.#
.#...##..#.
..#.##.....
.#.#.#....#
.#........#
#.##...#...
#...##....#
.#..#...#.#");

      var move = new Point(moveX, moveY);
      var start = new Point(startX, startY);
      var sut = new sut.TobogganRun(map);

      var result = sut.TreeCount(move, start);

      Assert.Equal(expectedTrees, result);
    }


  }
}
