namespace NorthPoleAir.Core
{
  /// <summary>
  /// Represents a plane including data about it such as rows and seats
  /// </summary>
  public interface IPlane
  {
    /// <summary>
    /// Gets the number of rows on the plane
    /// </summary>
    public int Rows { get; }

    /// <summary>
    /// Gets the number of seats per row on the plane
    /// </summary>
    public int Seats { get; }
  }
}
