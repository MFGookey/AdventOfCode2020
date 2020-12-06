using System;
using System.Collections.Generic;
using System.Text;

namespace PapersPlease.Core
{
  /// <summary>
  /// Represents the collection of form answers by a single group of passengers
  /// </summary>
  public interface IPassengerGroup
  {
    /// <summary>
    /// The count of distinct yes answers from the group
    /// </summary>
    int DistinctAnswerCount
    {
      get;
    }

    /// <summary>
    /// The count of questions where everyone in the group had the same answer
    /// </summary>
    int AllSameAnswerCount
    {
      get;
    }
  }
}
