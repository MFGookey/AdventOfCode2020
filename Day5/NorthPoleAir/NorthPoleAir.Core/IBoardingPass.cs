using System;
using System.Collections.Generic;
using System.Text;

namespace NorthPoleAir.Core
{
  public interface IBoardingPass
  {
    /// <summary>
    /// Gets a path to a particular seat on the plane
    /// </summary>
    public string SeatPath { get; }

    /// <summary>
    /// Gets the zero-based row number for the boarding pass
    /// </summary>
    public int Row { get; }

    /// <summary>
    /// Gets the zero-based seat number for the boarding pass
    /// </summary>
    public int Seat { get; }

    /// <summary>
    /// Gets a representation of the plane's rows and seats
    /// </summary>
    public IPlane Plane { get; }
  }
}
