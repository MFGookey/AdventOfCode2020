using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace MVConway.Core.Life
{
  /// <inheritdoc cref="ICell" />
  public class Cell : ICell
  {
    /// <inheritdoc />
    public CellState CurrentState { get; private set; }

    /// <summary>
    /// Given the neighbors and the current cell state, return a cell state.
    /// </summary>
    private readonly Func<IReadOnlyCollection<IReadOnlyCollection<ICell>>, CellState, CellState> _nextStep;

    /// <summary>
    /// We need to precalculate the state so we can apply state changes to the entire board at once.  Store the precalculated state.
    /// </summary>
    private CellState? CalculatedNextState { get; set; }

    /// <summary>
    /// Gets a flag indicating whether or not the cell will change upon the next step.
    /// If the next step has not been calculated, returns null.
    /// </summary>
    public bool? WillChange { get; private set; }

    /// <summary>
    /// Provide a method to lockout the calculation and application of cell states.
    /// </summary>
    private readonly SemaphoreSlim _lockout;

    /// <inheritdoc />
    public IReadOnlyCollection<IReadOnlyCollection<ICell>> Neighbors { get; private set; }

    /// <summary>
    /// Given a cell state, return a string to represent this cell
    /// </summary>
    private readonly Func<CellState, string> _display;

    /// <summary>
    /// Initializes a new instance of Cell with a given state, rules to transition from the current state to another state, and rules to format the cells tate for display
    /// </summary>
    /// <param name="currentState">The initial state of this cell</param>
    /// <param name="nextStep">A func to calculate state transitions based on a cell state, and neighboring cells</param>
    /// <param name="display">A func to map cell states to strings</param>
    public Cell(
      CellState currentState,
      Func<IReadOnlyCollection<IReadOnlyCollection<ICell>>, CellState, CellState> nextStep,
      Func<CellState, string> display
    )
    {
      _lockout = new SemaphoreSlim(1);
      CalculatedNextState = null;
      WillChange = null;
      CurrentState = currentState;
      Neighbors = null;
      _nextStep = nextStep ?? throw new ArgumentNullException(nameof(nextStep),"Must provide the next step transition func");
      _display = display ?? throw new ArgumentNullException(nameof(display), "Must provide the mapping from CellState to string");
    }

    /// <inheritdoc />
    public void SetNeighbors(IReadOnlyCollection<IReadOnlyCollection<ICell>> neighbors)
    {
      if (Neighbors != null)
      {
        throw new MemberAccessException("Neighbors may only be set once");
      }

      Neighbors = neighbors ?? throw new ArgumentNullException(nameof(neighbors), "neighbors cannot be null");
    }

    /// <inheritdoc />
    public void CalculateNextState()
    {
      _lockout.Wait();

      try{
        if (CalculatedNextState != null)
        {
          throw new Exception(
            "The next state has been calculated and must be applied before a new state can be calculated.");

        }
        else
        {
          CalculatedNextState = _nextStep(Neighbors, CurrentState);
          WillChange = CalculatedNextState != CurrentState;
        }
      }
      finally
      {
        _lockout.Release();
      }
      
    }

    /// <inheritdoc />
    public void Step()
    {
      _lockout.Wait();
      try
      {

        CurrentState = CalculatedNextState ??
                       throw new Exception("Next state must be calculated before Step can be applied");
        CalculatedNextState = null;
        WillChange = null;
      }
      finally
      {
        _lockout.Release();
      }
    }

    /// <inheritdoc />
    public override string ToString()
    {
      return _display(CurrentState);
    }
  }
}