using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;
using Moq;
using MVConway.Core.Life;

namespace MVConway.Core.Tests.Life
{
  public class BoardTests
  {
    [Fact]
    public void Constructor_GivenNonRectangularData_ThrowsFormatException()
    {
      var data = new[] {
        "a",
        "ab",
        "b"
      };

      var criteria = new int[] { };

      var interpreter = new Func<char, CellState>((c) => CellState.Empty);

      Assert.Throws<FormatException>(() => new Board(data, criteria, criteria, interpreter, NeighborSelectionRule.SimpleConway));
    }

    [Fact]
    public void Constructor_GivenNullSurvivalCriteria_ThrowsArgumentNullException()
    {
      var data = new string[] { };

      var criteria = new int[] { };

      var interpreter = new Func<char, CellState>((c) => CellState.Empty);

      Assert.Throws<ArgumentNullException>(() => new Board(data, null, criteria, interpreter, NeighborSelectionRule.SimpleConway));
    }

    [Fact]
    public void Constructor_GivenNullBirthCriteria_ThrowsArgumentNullException()
    {
      var data = new string[] { };

      var criteria = new int[] { };

      var interpreter = new Func<char, CellState>((c) => CellState.Empty);

      Assert.Throws<ArgumentNullException>(() => new Board(data, criteria, null, interpreter, NeighborSelectionRule.SimpleConway));
    }

    [Fact]
    public void Constructor_GivenValidData_SetsCellsAsExpected()
    {
      var data = new[]
      {
        "L.LL.LL.LL",
        "LLLLLLL.LL",
        "L.L.L..L..",
        "LLLL.LL.LL",
        "L.LL.LL.LL",
        "L.LLLLL.LL",
        "..L.L.....",
        "LLLLLLLLLL",
        "L.LLLLLL.L",
        "L.LLLLL.LL"
      };

      var interpreter = new Func<char, CellState>((c) =>
        {
          switch (c)
          {
            case 'L':
              return CellState.Empty;
            case '.':
              return CellState.Unavailable;
            default:
              throw new FormatException($"Shouldn't have this: '{c}'");
          }
        }
      );

      var criteria = new int[] { };

      var expectedStringBoard =
        "L.LL.LL.LL\nLLLLLLL.LL\nL.L.L..L..\nLLLL.LL.LL\nL.LL.LL.LL\nL.LLLLL.LL\n..L.L.....\nLLLLLLLLLL\nL.LLLLLL.L\nL.LLLLL.LL";

      var sut = new Board(data, criteria, criteria, interpreter, NeighborSelectionRule.SimpleConway);
      Assert.Equal(expectedStringBoard, sut.ToString());
    }

    [Fact]
    public void Step_WhenValidData_SetsCellsAsExpected()
    {
      var data = new[]
      {
        "L.LL.LL.LL",
        "LLLLLLL.LL",
        "L.L.L..L..",
        "LLLL.LL.LL",
        "L.LL.LL.LL",
        "L.LLLLL.LL",
        "..L.L.....",
        "LLLLLLLLLL",
        "L.LLLLLL.L",
        "L.LLLLL.LL"
      };

      var interpreter = new Func<char, CellState>((c) =>
        {
          switch (c)
          {
            case 'L':
              return CellState.Empty;
            case '.':
              return CellState.Unavailable;
            default:
              throw new FormatException($"Shouldn't have this: '{c}'");
          }
        }
      );

      var birthCriteria = new int[] { 0 };
      var survivalCriteria = new int[] { 0, 1, 2, 3 };

      var expectedStringBoard =
        "L.LL.LL.LL\nLLLLLLL.LL\nL.L.L..L..\nLLLL.LL.LL\nL.LL.LL.LL\nL.LLLLL.LL\n..L.L.....\nLLLLLLLLLL\nL.LLLLLL.L\nL.LLLLL.LL";

      var boards = new object[][]
      {
        new object[]{
          "#.##.##.##\n#######.##\n#.#.#..#..\n####.##.##\n#.##.##.##\n#.#####.##\n..#.#.....\n##########\n#.######.#\n#.#####.##", true
        },

        new object[]{
          "#.LL.L#.##\n#LLLLLL.L#\nL.L.L..L..\n#LLL.LL.L#\n#.LL.LL.LL\n#.LLLL#.##\n..L.L.....\n#LLLLLLLL#\n#.LLLLLL.L\n#.#LLLL.##", true
        },

        new object[]{
          "#.##.L#.##\n#L###LL.L#\nL.#.#..#..\n#L##.##.L#\n#.##.LL.LL\n#.###L#.##\n..#.#.....\n#L######L#\n#.LL###L.L\n#.#L###.##",
          true
        },

        new object[]{
          "#.#L.L#.##\n#LLL#LL.L#\nL.L.L..#..\n#LLL.##.L#\n#.LL.LL.LL\n#.LL#L#.##\n..L.L.....\n#L#LLLL#L#\n#.LLLLLL.L\n#.#L#L#.##",
          true
        },

        new object[]{
          "#.#L.L#.##\n#LLL#LL.L#\nL.#.L..#..\n#L##.##.L#\n#.#L.LL.LL\n#.#L#L#.##\n..L.L.....\n#L#L##L#L#\n#.LLLLLL.L\n#.#L#L#.##",
          true
        },

        new object[]{
          "#.#L.L#.##\n#LLL#LL.L#\nL.#.L..#..\n#L##.##.L#\n#.#L.LL.LL\n#.#L#L#.##\n..L.L.....\n#L#L##L#L#\n#.LLLLLL.L\n#.#L#L#.##",
          false
        }
      };

      var sut = new Board(data, survivalCriteria, birthCriteria, interpreter, NeighborSelectionRule.SimpleConway);


      Assert.Equal(expectedStringBoard, sut.ToString());

      foreach (var board in boards)
      {
        var willChange = sut.Step();
        Assert.Equal((string)board[0], sut.ToString());
        Assert.Equal((bool)board[1], willChange);
      }
    }

    [Fact]
    public void StepWhileChange_WhenValidData_SetsCellsAsExpected()
    {
      var data = new[]
      {
        "L.LL.LL.LL",
        "LLLLLLL.LL",
        "L.L.L..L..",
        "LLLL.LL.LL",
        "L.LL.LL.LL",
        "L.LLLLL.LL",
        "..L.L.....",
        "LLLLLLLLLL",
        "L.LLLLLL.L",
        "L.LLLLL.LL"
      };

      var interpreter = new Func<char, CellState>(
        (
          c) =>
        {
          switch (c)
          {
            case 'L':
              return CellState.Empty;
            case '.':
              return CellState.Unavailable;
            default:
              throw new FormatException($"Shouldn't have this: '{c}'");
          }
        }
      );

      var birthCriteria = new int[] {0};
      var survivalCriteria = new int[] {0, 1, 2, 3};

      var expectedStringBoard =
        "L.LL.LL.LL\nLLLLLLL.LL\nL.L.L..L..\nLLLL.LL.LL\nL.LL.LL.LL\nL.LLLLL.LL\n..L.L.....\nLLLLLLLLLL\nL.LLLLLL.L\nL.LLLLL.LL";
      var finalExpectedBoard =
        "#.#L.L#.##\n#LLL#LL.L#\nL.#.L..#..\n#L##.##.L#\n#.#L.LL.LL\n#.#L#L#.##\n..L.L.....\n#L#L##L#L#\n#.LLLLLL.L\n#.#L#L#.##";
      var sut = new Board(data, survivalCriteria, birthCriteria, interpreter, NeighborSelectionRule.SimpleConway);


      Assert.Equal(expectedStringBoard, sut.ToString());

      sut.StepWhileChange(
        new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token
        );

      Assert.Equal(finalExpectedBoard, sut.ToString());

    }
  }
}
