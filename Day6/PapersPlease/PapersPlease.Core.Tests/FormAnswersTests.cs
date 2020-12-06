using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using PapersPlease.Core;

namespace PapersPlease.Core.Tests
{
  public class FormAnswersTests
  {
    [Theory]
    [InlineData("abcd\neft")]
    [InlineData("abcd\reft")]
    [InlineData("abcd\n\reft")]
    [InlineData("abcd\r\neft")]
    [InlineData("\r\nabceft")]
    [InlineData("\n\rabceft")]
    [InlineData("\nabceft")]
    [InlineData("\rabceft")]
    [InlineData("abcd\neft\n")]
    [InlineData("abcd\reft\n")]
    [InlineData("abcd\n\reft\n")]
    [InlineData("abcd\r\neft\n")]
    [InlineData("\r\nabceft\n")]
    [InlineData("\n\rabceft\n")]
    [InlineData("\nabceft\n")]
    [InlineData("\rabceft\n")]
    [InlineData("abcd\neft\r")]
    [InlineData("abcd\reft\r")]
    [InlineData("abcd\n\reft\r")]
    [InlineData("abcd\r\neft\r")]
    [InlineData("\r\nabceft\r")]
    [InlineData("\n\rabceft\r")]
    [InlineData("\nabceft\r")]
    [InlineData("\rabceft\r")]
    [InlineData("abceft\r")]
    [InlineData("abceft\r")]
    public void Constructor_GivenMultilineString_ThrowsException(string answers)
    {
      Assert.Throws<ArgumentException>(() => new FormAnswers(answers));
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("a")]
    [InlineData("b")]
    [InlineData("c")]
    [InlineData("ab")]
    [InlineData("ac")]
    public void Constructor_GivenValidString_SetsAnswersCorrectly(string answers)
    {
      var sut = new FormAnswers(answers);
      Assert.Equal(answers, sut.Answers);
    }

    [Theory]
    [InlineData("aababababababababa", "ab")]
    [InlineData("aaaaaaaaaaaaaa", "a")]
    [InlineData("bbbbbbbbbbbbb", "b")]
    public void Constructor_GivenStringWithDuplicates_MakesAnswersDistinct(string answers, string expectedAnswers)
    {
      var sut = new FormAnswers(answers);
      Assert.Equal(expectedAnswers, sut.Answers);
    }

    [Theory]
    [MemberData(nameof(AnswerLists))]
    public void Constructor_GivenValidString_SetsAnswerListCorrectly(string answers, IEnumerable<char> expectedAnswerList)
    {
      var sut = new FormAnswers(answers);
      Assert.Equal(expectedAnswerList, sut.AnswerList);
    }

    public static IEnumerable<object[]> AnswerLists
    {
      get
      {
        yield return new object[]
        {
          "abc",
          new char[]{'a', 'b', 'c'}
        };

        yield return new object[]
        {
          "a",
          new char[]{'a'}
        };

        yield return new object[]
        {
          "b",
          new char[]{'b'}
        };

        yield return new object[]
        {
          "c",
          new char[]{'c'}
        };

        yield return new object[]
        {
          "ab",
          new char[]{'a', 'b'}
        };

        yield return new object[]
        {
          "ac",
          new char[]{'a', 'c'}
        };
      }
    }
  }
}
