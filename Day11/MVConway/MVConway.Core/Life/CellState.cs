using System;
using System.Collections.Generic;
using System.Text;

namespace MVConway.Core.Life
{
  /// <summary>
  /// Represents the potential states a cell can have in a Life game
  /// </summary>
  public enum CellState
  {
    /// <summary>
    /// The cell is unavailable for consideration.
    /// </summary>
    Unavailable = int.MinValue,

    /// <summary>
    /// The cell is empty
    /// </summary>
    Empty = 0,
    
    /// <summary>
    /// The cell is alive
    /// </summary>
    Alive = 1
  }
}
