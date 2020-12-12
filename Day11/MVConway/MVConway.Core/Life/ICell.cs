using System;
using System.Collections.Generic;
using System.Text;

namespace MVConway.Core.Life
{
  /// <summary>
  /// Represents a cell in a game of life board
  /// </summary>
  public interface ICell
  {
    /// <summary>
    /// Represents the current state of the cell
    /// </summary>
    public CellState CurrentState { get; }

    /// <summary>
    /// A listing of the neighboring cells to this one
    /// </summary>
    public IReadOnlyCollection<ICell> Neighbors { get; }

    /// <summary>
    /// Calculate the state this cell will have in the next iteration
    /// </summary>
    /// <returns>The next state of the cell</returns>
    public void CalculateNextState();

    /// <summary>
    /// Transition from the current cell state to the next state.
    /// </summary>
    public void Step();

    /// <summary>
    /// Gets a flag indicating whether or not this cell will change its state once the next state has been calculated.
    /// </summary>
    public bool? WillChange { get; }

    /// <summary>
    /// Set the collection of neighbors of this cell
    /// </summary>
    /// <param name="neighbors">The neighbors of the current cell.</param>
    public void SetNeighbors(IReadOnlyCollection<ICell> neighbors);
  }
}
