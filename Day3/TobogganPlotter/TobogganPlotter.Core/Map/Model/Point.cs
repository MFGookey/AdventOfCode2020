using System;

namespace TobogganPlotter.Core.Map.Model
{
  /// <summary>
  /// Represents a point on the cartesian plane
  /// </summary>
  public class Point
  {
    /// <summary>
    /// The point's X coordinate
    /// </summary>
    public int X {
      get; private set;
    }

    /// <summary>
    /// The point's Y coordinate
    /// </summary>
    public int Y {
      get; private set;
    }

    /// <summary>
    /// Initializes a new point at the given x and y coordinates
    /// </summary>
    /// <param name="x">The X coordinate of the point</param>
    /// <param name="y">The Y coordinate of the point</param>
    public Point(int x, int y)
    {
      X = x;
      Y = y;
    }

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
      //Check for null and compare run-time types.
      if ((obj == null) || !this.GetType().Equals(obj.GetType()))
      {
        return false;
      }
      else
      {
        Point p = (Point)obj;
        return (
          this.X.Equals(p.X) &&
          this.Y.Equals(p.Y)
          );
      }
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
      return HashCode.Combine(X, Y);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
      return string.Format("({0}, {1})", X, Y);
    }

    /// <inheritdoc/>
    public static Point operator +(Point left, Point right)
    {
      return new Point(left.X + right.X, left.Y + right.Y);
    }

    /// <inheritdoc/>
    public static Point operator -(Point left, Point right)
    {
      return new Point(left.X - right.X, left.Y - right.Y);
    }
  }
}
