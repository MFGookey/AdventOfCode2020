using System;
using System.Collections.Generic;
using System.Text;

namespace PapersPlease.Core
{
  /// <summary>
  /// Represents affirmative answers on a customs form
  /// </summary>
  public interface IFormAnswers
  {
    /// <summary>
    /// A string representation of the question identifiers on the form where the answer was yes
    /// </summary>
    string Answers
    {
      get;
    }

    /// <summary>
    /// An enumerable of the questions where the answer given was yes
    /// </summary>
    IEnumerable<char> AnswerList
    {
      get;
    }
  }
}
