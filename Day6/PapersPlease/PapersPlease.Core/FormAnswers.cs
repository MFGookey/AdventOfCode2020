using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PapersPlease.Core
{
  /// <inheritdoc cref="IFormAnswers"/>
  public class FormAnswers : IFormAnswers
  {
    /// <inheritdoc/>
    public string Answers
    {
      get;
      private set;
    }

    /// <inheritdoc/>
    public IEnumerable<char> AnswerList
    {
      get
      {
        return Answers.ToCharArray().OrderBy(c=>c);
      }
    }

    /// <summary>
    /// Initializes a new instance of FormAnswers from a string of undistinctified answers
    /// </summary>
    /// <param name="undistinctifiedAnswers">The list of form answers to represent.</param>
    public FormAnswers(string undistinctifiedAnswers)
    {
      if (undistinctifiedAnswers.Contains('\n') || undistinctifiedAnswers.Contains('\r'))
      {
        throw new ArgumentException("Answers must be on a single line", nameof(undistinctifiedAnswers));
      }

      Answers = string.Join("", undistinctifiedAnswers.ToLowerInvariant().Distinct());
    }
  }
}
