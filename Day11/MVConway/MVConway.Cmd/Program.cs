using System;
using System.Linq;
using System.Threading;
using Common.Utilities.IO;
using MVConway.Core.Life;


namespace MVConway.Cmd
{
  /// <summary>
  /// The entrypoint for MVConway.Cmd
  /// </summary>
  public class Program
  {
    /// <summary>
    /// MVConway.Cmd entry point
    /// </summary>
    /// <param name="args">Command line arguments (not used)</param>
    static void Main(string[] args)
    {
      var filePath = "./input";
      var reader = new FileReader();
      var board = reader.ReadFileByLines(filePath);

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
            case '#':
              return CellState.Alive;
            default:
              throw new FormatException($"Shouldn't have this: '{c}'");
          }
        }
      );

      var birthCriteria = new int[] { 0 };
      var survivalCriteria = new int[] { 0, 1, 2, 3 };

      var gameOfLife = new Board(board, survivalCriteria, birthCriteria, interpreter, NeighborSelectionRule.SimpleConway);

      gameOfLife.StepWhileChange(new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token);

      var finalBoard = gameOfLife.ToString();

      var occupiedSeats = finalBoard.ToCharArray().Count(c => c == '#');

      Console.WriteLine(occupiedSeats);
    }
  }
}
