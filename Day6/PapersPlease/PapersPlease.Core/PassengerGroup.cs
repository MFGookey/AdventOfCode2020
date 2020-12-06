using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Common.Utilities.Formatter;

namespace PapersPlease.Core
{
  /// <inheritdoc cref="IPassengerGroup"/>
  public class PassengerGroup : IPassengerGroup
  {
    /// <summary>
    /// The enumerable of IFormAnswers from all passengers in the group
    /// </summary>
    private IEnumerable<IFormAnswers> PassengerAnswers
    {
      get;
      set;
    }

    /// <inheritdoc/>
    public int DistinctAnswerCount
    {
      get
      {
        return string
          .Join(
          "",
          PassengerAnswers
            .Select(
              passengerAnswer => passengerAnswer.Answers
            )
          )
          .ToCharArray()
          .Distinct()
          .Count();
      }
    }

    /// <inheritdoc/>
    public int AllSameAnswerCount
    {
      get
      {
        return string
          .Join(
            "",
            PassengerAnswers
              .Select(
                passengerAnswer => passengerAnswer.Answers
              )
          )
          .ToCharArray()
          .GroupBy(answer => answer)
          .Select(group => new
          {
            Answer = group.Key,
            Count = group.Count()
          })
          .Where(answer => answer.Count == PassengerAnswers.Count())
          .Count();
      }
    }

    /// <summary>
    /// Given a collection of answers for a group, create an object representing that group's answers to customs questions
    /// </summary>
    /// <param name="groupAnswers">The string of group answers to represent.</param>
    public PassengerGroup(IEnumerable<string> groupAnswers)
    {
      PassengerAnswers = groupAnswers.Select(answerString => new FormAnswers(answerString));
    }
  }
}
