using System;
using System.Collections.Generic;
using System.Text;

namespace NorthPoleAir.Core
{
  /// <inheritdoc cref="IPlane"/>
  public class Plane : IPlane
  {
    /// <inheritdoc/>
    public int Rows
    {
      get; private set;
    }

    /// <inheritdoc/>
    public int Seats
    {
      get; private set;
    }

    /// <summary>
    /// Initializes a new instance of the Plane class with a given number of rows and seats
    /// </summary>
    /// <param name="rows">The number of rows on the plane</param>
    /// <param name="seats">The number of seats on the plane</param>
    public Plane(int rows, int seats)
    {
      if (rows <= 0)
      {
        throw new ArgumentException("Rows cannot be less than or equal to zero", nameof(rows));
      }

      if (seats <= 0)
      {
        throw new ArgumentException("Seats cannot be less than or equal to zero", nameof(seats));
      }

      Rows = rows;
      Seats = seats;
    }
  }
}
