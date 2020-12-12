using System.Collections;
using System.Collections.Generic;

namespace MVConway.Core.Life
{
  public interface IBoard
  {
    /// <summary>
    /// Gets the length of this board
    /// </summary>
    int Length { get; }

    /// <summary>
    /// Gets the height of this board
    /// </summary>
    int Height { get; }

    /// <summary>
    /// Gets the cells of the board in an indexable form
    /// </summary>
    IList<IList<ICell>> Cells { get; }
  }
}