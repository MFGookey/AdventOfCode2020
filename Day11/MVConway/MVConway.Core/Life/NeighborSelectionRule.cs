namespace MVConway.Core.Life
{
  /// <summary>
  /// Represent different rule sets we can use for determining which cell is a neighbor of another one
  /// </summary>
  public enum NeighborSelectionRule
  {
    /// <summary>
    /// Represents selection using direct adjacency
    /// </summary>
    SimpleConway = 0,
    
    /// <summary>
    /// Represents selection using queen's moves
    /// </summary>
    QueensMove = 1
  }
}