using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PapersPlease.Core.Tests
{
  public class PassengerGroupTests
  {
    [Theory]
    [MemberData(nameof(SampleGroups))]
    public void Constructor_GivenValidSampleGroupData_GivesExpectedValues(
      IEnumerable<string> groupAnswers,
      int expectedDistinctAnswerCount
    )
    {
      var sut = new PassengerGroup(groupAnswers);

      Assert.Equal(expectedDistinctAnswerCount, sut.DistinctAnswerCount);
    }

    [Theory]
    [MemberData(nameof(SampleSameAnswerGroups))]
    public void Constructor_GivenValidSampleGroupData_GivesExpectedSameAnswerValues(
      IEnumerable<string> groupAnswers,
      int expectedAllSameAnswerCount
    )
    {
      var sut = new PassengerGroup(groupAnswers);

      Assert.Equal(expectedAllSameAnswerCount, sut.AllSameAnswerCount);
    }

    public static IEnumerable<object[]> SampleGroups
    {
      get
      {
        yield return new object[]
        {
          new string[] { "abc" },
          3
        };

        yield return new object[]
        {
          new string[] { "a", "b", "c" },
          3
        };

        yield return new object[]
        {
          new string[] { "ab", "ac" },
          3
        };

        yield return new object[]
        {
          new string[] { "a", "a", "a", "a" },
          1
        };

        yield return new object[]
        {
          new string[] { "b" },
          1
        };
      }
    }


    public static IEnumerable<object[]> SampleSameAnswerGroups
    {
      get
      {
        yield return new object[]
        {
          new string[] { "abc" },
          3
        };

        yield return new object[]
        {
          new string[] { "a", "b", "c" },
          0
        };

        yield return new object[]
        {
          new string[] { "ab", "ac" },
          1
        };

        yield return new object[]
        {
          new string[] { "a", "a", "a", "a" },
          1
        };

        yield return new object[]
        {
          new string[] { "b" },
          1
        };
      }
    }
  }
}
