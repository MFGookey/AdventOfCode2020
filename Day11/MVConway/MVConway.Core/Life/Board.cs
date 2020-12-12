using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MVConway.Core.Life
{
  /// <inheritdoc cref="ICell"/>
  public class Board : IBoard
  {
    /// <inheritdoc />
    public int Length {
      get
      {
        return Cells.Count;
      }
    }

    /// <inheritdoc />
    public int Height
    {
      get
      {
        return Cells.First().Count;
      }
    }

    private readonly int[] _survivalCriteria;

    private readonly int[] _birthCriteria;

    /// <inheritdoc />
    public IList<IList<ICell>> Cells { get; private set; }

    /// <summary>
    /// Initializes a new board from a string of board data, a list of possible numbers of live neighbors a live cell must have to survive, and how many neighbors an empty cell must have to become live
    /// </summary>
    /// <param name="boardData">A list of strings to represent the initial board state, one character per cell.  Must be rectangular.</param>
    /// <param name="survivalCriteria">A list of the number of living neighbors a living cell must have in order to persist from one step to the next</param>
    /// <param name="birthCriteria">A list of the number of living neighbors an empty cell must have in order to become alive</param>
    /// <param name="boardInterpreter">A delegate to map a character to a CellState</param>
    /// <param name="neighborRule">Determine the rules by which neighbors of cells are chosen</param>
    public Board(
      string[] boardData,
      int[] survivalCriteria,
      int[] birthCriteria,
      Func<char, CellState> boardInterpreter,
      NeighborSelectionRule neighborRule
    )
    {
      _survivalCriteria = survivalCriteria ?? throw new ArgumentNullException(nameof(survivalCriteria));
      _birthCriteria = birthCriteria ?? throw new ArgumentNullException(nameof(birthCriteria));

      // Ensure the board is rectangular
      if (boardData.Select(row => row.Length).Distinct().Count() != 1)
      {
        throw new FormatException("boardData must be rectangular");
      }

      Cells = Parse(boardData, boardInterpreter);
      switch (neighborRule)
      {
        case NeighborSelectionRule.SimpleConway:
          AssignNeighborGroups(SimpleConwayNeighborList);
          break;
        case NeighborSelectionRule.QueensMove:
          AssignNeighborGroups(QueensMoveNeighborList);
          break;
        default:
          throw new Exception("Should not get here");
      }
      
    }

    /// <summary>
    /// Given a list of neighbors and a current cell state, determine the next state the cell will have
    /// </summary>
    /// <param name="neighborGroup">Group of neighbors to be evaluated for whether or not to live or die</param>
    /// <param name="currentState">The cell's current state</param>
    /// <returns>The next CellState</returns>
    private CellState CalculateNextCellState(IEnumerable<ICell> neighborGroup, CellState currentState)
    {
      if (currentState == CellState.Unavailable)
      {
        return CellState.Unavailable;
      }

      // Get a count of the neighbors that are Alive
      // For the purposes of survival, we treat Unavailable cells as empty
      var livingNeighbors = neighborGroup.Count(n => n.CurrentState == CellState.Alive);

      if (currentState == CellState.Alive)
      {
        if (_survivalCriteria.Contains(livingNeighbors))
        {
          return CellState.Alive;
        }
        
        return CellState.Empty;
      }

      if (currentState == CellState.Empty)
      {
        if (_birthCriteria.Contains(livingNeighbors))
        {
          return CellState.Alive;
        }

        return CellState.Empty;
      }

      throw new Exception("This should not be reachable.");
    }

    /// <summary>
    /// Given a CellState, return a string
    /// </summary>
    /// <param name="currentState">The state to represent</param>
    /// <returns>A string to represent the given state</returns>
    private static string CellString(CellState currentState)
    {
      switch (currentState)
      {
        case CellState.Unavailable:
          return ".";
        case CellState.Empty:
          return "L";
        case CellState.Alive:
          return "#";
        default:
          throw new Exception("This should be unreachable");
      }
    }

    /// <summary>
    /// Given an interpreter that maps a char to a cellstate, parse a string array into an IEnumerable of IEnumerables of ICell
    /// </summary>
    /// <param name="boardState">The stringified board state to parse</param>
    /// <param name="interpreter">The interpretation rules to turn chars into CellStates</param>
    /// <returns>The parsed board, as an IEnumerable of IEnumerables of ICell</returns>
    private IList<IList<ICell>> Parse(
      string[] boardState,
      Func<char, CellState> interpreter
    )
    {
      return boardState.Select(
          (
            // For every string in the state array
            // Select the string and what its array index is
            rowData,
            rowNumber
          ) =>
          new
            {
              // map the index to rowNumber
              rowNumber,

              // turn the string into a char array and parse it to a cell
              // before casting the whole thing as an IReadOnlyList of ICell
              rowData = (IList<ICell>)rowData.ToCharArray()
                .Select(
                  (
                    // for every char in the string
                    // Select the char and its index in the string
                    c,
                    columnNumber
                  ) => new
                  {
                    // map the index to columnNumber
                    columnNumber,

                    // run the char through interpreter to get a CellState
                    // and then along with some local methods, new up a Cell
                    cell = new Cell(
                      interpreter(c),
                      CalculateNextCellState,
                      CellString
                    )
                  }
                )
                // order the cells within a row by their column number
                .OrderBy(c => c.columnNumber)
                // return just the cell from the anonymous object
                .Select(r => r.cell)
                .ToList<ICell>()
            }
          )
        // Order the rows by their row number
        .OrderBy(r => r.rowNumber)
        // then select just the IReadOnlyList<ICell>s in the rowData
        // from the anonymous object we no longer need.
        .Select(r => r.rowData)
        .ToList();
    }

    /// <summary>
    /// To use as a Func to generate and assign immediate neighbors for a given cell.
    /// </summary>
    /// <returns>An IEnumerable of tuples holding an ICell and a collection of ICells representing the cell's neighbors</returns>
    private IEnumerable<Tuple<ICell,IReadOnlyCollection<ICell>>> SimpleConwayNeighborList()
    {
      return Cells.SelectMany((cellRow, yIndex) =>
        // For every cell within that row
        cellRow.Select((cell, xIndex) => {
          var neighbors =
            // calculate the neighbors based of the current cell's indices
            // neighbors are within 1 of the cell on both the x and y axes
            // except for when we are off the edge of the board
            // or when the ranges align such that we are looking at ourselves
            // Ignore both of those cases for each pair of calculated points.

            // Create a range of x values from the current x index - 1 that is 3 long (x-1, x, x+1)
            Enumerable.Range(xIndex - 1, 3)
              .SelectMany(
                rangeX =>
                {
                    // Create a range of y values from the current y index - 1 that is 3 long (y-1, y, y+1)
                    return Enumerable.Range(yIndex - 1, 3)
                    .Select(
                      rangeY => new
                      {
                          // Create an x, y pair using the ranges
                          x = rangeX,
                        y = rangeY
                      }
                    );
                }
              )
              .Where(
                resultingPoint =>
                  //the candidate neighbor point must have an x between 0 and the row length -1
                  resultingPoint.x >= 0 &&
                  resultingPoint.x < Cells.First().Count &&

                  //the point must have a y between 0 and the enumerable length -1
                  resultingPoint.y >= 0 &&
                  resultingPoint.y < Cells.Count &&

                  // and the candidate neighbor point cannot reference the point for which we are selecting neighbors
                  (resultingPoint.x == xIndex && resultingPoint.y == yIndex) == false
              )
              //for each of those points, select the cell at the point's coordinates
              .Select(coordinates => Cells[coordinates.y][coordinates.x])

              //turn this into a readonly list of ICells
              .ToList();

          return new Tuple<ICell, IReadOnlyCollection<ICell>>(cell, neighbors);
        }
        )
      );
    }

    /// <summary>
    /// To use as a Func to generate and assign neighbors.  If a chess queen can move to a cell, consider it a neighbor.
    /// </summary>
    /// <returns>An IEnumerable of tuples holding an ICell and a collection of collections of ICells representing the cell's neighbors</returns>
    private IEnumerable<Tuple<ICell, IReadOnlyCollection<ICell>>> QueensMoveNeighborList()
    {
      return Cells.SelectMany((cellRow, yIndex) =>
        // For every cell within that row
        cellRow.Select((cell, xIndex) => {
          var neighbors =
            // calculate the neighbors based of the current cell's indices
            // neighbors are any cell a chess queen could move to from the current cell
            // except for when we are off the edge of the board
            // or when the ranges align such that we are looking at ourselves
            // Ignore both of those cases for each pair of calculated points.

            // Create a range of x values from 0 to Cells[0].Length
            Enumerable.Range(0, Cells.First().Count)
              .SelectMany(
                rangeX =>
                {
                  // Create a range of y values from from 0 to Cells[0].Length
                  return Enumerable.Range(0, Cells.Count)
                    .Select(
                      rangeY => new
                      {
                        // Create an x, y pair using the ranges
                        x = rangeX,
                        y = rangeY
                      }
                    );
                }
              )
              .Where(
                resultingPoint =>
                  /*//the candidate neighbor point must have an x between 0 and the row length -1
                  resultingPoint.x >= 0 &&
                  resultingPoint.x < Cells.First().Count &&

                  //the point must have a y between 0 and the enumerable length -1
                  resultingPoint.y >= 0 &&
                  resultingPoint.y < Cells.Count &&*/

                  // and the candidate neighbor point cannot reference the point for which we are selecting neighbors
                  (resultingPoint.x == xIndex && resultingPoint.y == yIndex) == false && (
                    resultingPoint.y == yIndex || // same column
                    resultingPoint.x == xIndex || // same row
                    resultingPoint.x - xIndex == resultingPoint.y - yIndex ||
                    resultingPoint.x - xIndex == yIndex - resultingPoint.y
                  )
              )
              .GroupBy(
                // group the points so that we can hve each group be an enumerable of cells.
                point =>
                {
                  if (point.x == xIndex && point.y <= yIndex)
                    return 1; // column above this point
                  if (point.x == xIndex && point.y > yIndex)
                    return 2; // column below this point
                  if (point.x <= xIndex && point.y == yIndex)
                    return 3; // row left of this point
                  if (point.x >= xIndex && point.y == yIndex)
                    return 4; // row right of this point
                  if (point.x <= xIndex && point.y <= yIndex)
                    return 5; // top left to this point
                  if (point.x >= xIndex && point.y > yIndex)
                    return 6; // this point to bottom right
                  if (point.x < xIndex && point.y >= yIndex)
                    return 7; // bottom left to this point
                  return 8; // this point to top right
                }
              )
              //for each of those points, select the cell at the point's coordinates
              .Select(group => group.Select(
                coordinate => new {
                    cell = Cells[coordinate.y][coordinate.x],
                    x = coordinate.x,
                    y = coordinate.y
                  }
              )
              .Where(cellInfo => cellInfo.cell.CurrentState != CellState.Unavailable)
              .OrderBy(cellInfo => Math.Pow(xIndex - cellInfo.x, 2) + Math.Pow(yIndex - cellInfo.y, 2))
              .FirstOrDefault()
              ?.cell
            )
              .Where(c => c != null)

            //turn this into a readonly list of ICells
            .ToList();

          return new Tuple<ICell, IReadOnlyCollection<ICell>>(cell, neighbors);
        }
        )
      );
    }

    private void AssignNeighborGroups(Func<IEnumerable<Tuple<ICell, IReadOnlyCollection<ICell>>>> neighborSelector)
    {
      // every cell has up to eight neighbors
      // except for the ones around the outside edge
      // but we don't care about simulating neighbors outside the board.  They always need to count as empty anyway.

      // here we go.
      // For every row
      var cellNeighborMaps = neighborSelector();

      foreach (var cellNeighborMap in cellNeighborMaps)
      {
        cellNeighborMap.Item1.SetNeighbors(cellNeighborMap.Item2);
      }
    }

    /// <inheritdoc />
    public override string ToString()
    {
      return string.Join("\n", Cells.Select(row => string.Join("", row.Select(cell => cell.ToString()))));
    }

    public bool Step()
    {
      var cells = Cells.SelectMany(row => row.Select(cell => cell));
      foreach (var cell in cells)
      {
        cell.CalculateNextState();
      }

      if (cells.Any(cell => cell.WillChange.HasValue == false))
      {
        throw new Exception("Cells ought to have WillChange set by now");
      }

      var anyChange = cells.Any(cell => cell.WillChange.GetValueOrDefault());

      foreach (var cell in cells)
      {
        cell.Step();
      }

      return anyChange;
    }

    /// <summary>
    /// Run until step stops
    /// </summary>
    public void StepWhileChange(CancellationToken ct)
    {
      while (ct.IsCancellationRequested == false && Step())
        ;
    }
  }
}