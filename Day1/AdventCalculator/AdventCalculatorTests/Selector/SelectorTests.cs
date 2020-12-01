using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using sut = AdventCalculator.Core.Selector;

namespace AdventCalculatorTests.Selector
{
  public class SelectorTests
  {
    [Fact]
    void SumFinder_TotalTermsIsZero_ReturnsNull()
    {
      var sut = new sut.Selector();
      var result = sut.SumFinder(new List<int>() { 1, 2, 3 }, 1, 0);
      Assert.Null(result);
    }

    [Fact]
    void SumFinder_TotalTermsIsNegative_ReturnsNull()
    {
      var sut = new sut.Selector();
      var result = sut.SumFinder(new List<int>() { 1, 2, 3 }, 1, int.MinValue);
      Assert.Null(result);
    }

    [Fact]
    void SumFinder_TotalTermsIsOneAndNoValidOptions_ReturnsNull()
    {
      var sut = new sut.Selector();
      var haystack = new List<int> { 1, 2, 3 };
      var needle = -1;
      var totalTerms = 1;

      var result = sut.SumFinder(haystack, needle, totalTerms);

      Assert.Null(result);
    }

    [Theory]
    [InlineData(null, 1, 1)]
    [InlineData(null, 1, 2)]
    [InlineData(null, 5, 4)]
    void SumFinder_TotalTermsIsGreaterThanZeroAndHaystackIsNull_ReturnsNull(IList<int> haystack, int needle, int totalTerms)
    {
      var sut = new sut.Selector();

      var result = sut.SumFinder(haystack, needle, totalTerms);

      Assert.Null(result);
    }

    [Fact]
    void SumFinder_TotalTermsIsOneAndValidOption_ReturnsValidOption()
    {
      var sut = new sut.Selector();
      var haystack = new List<int> { 1, 2, 3 };
      var needle = 2;
      var totalTerms = 1;
      var expectedResult = new List<int> { 2 };

      var result = sut.SumFinder(haystack, needle, totalTerms);

      Assert.Equal(expectedResult, result);
    }

    [Theory]
    [MemberData(nameof(ValidOptions))]
    void SumFinder_TotalTermsMoreThanOneAndValidOptions_ReturnsValidOptions(IList<int> haystack, int needle, int totalTerms)
    {
      var sut = new sut.Selector();
      var result = sut.SumFinder(haystack, needle, totalTerms);

      int resultSum = 0;
      foreach (var i in result)
      {
        resultSum += i;
      }

      Assert.Equal(totalTerms, result.Count);
      Assert.Equal(needle, resultSum);
    }

    public static IEnumerable<object[]> ValidOptions {
      get
      {
        yield return new object[] { new List<int> { 1, 2, 3 }, 4, 2 };
        yield return new object[] { new List<int> { -1, 1, 5, 6 }, 5, 2 };
        yield return new object[] { new List<int> { 1, 1, 1, 1, 1 }, 4, 4};
        yield return new object[] { new List<int> { 1721, 979, 366, 299, 675, 1456 }, 2020, 2 };
        yield return new object[] { new List<int> { 1721, 979, 366, 299, 675, 1456 }, 665, 2 };
        yield return new object[] { new List<int> { 1721, 979, 366, 299, -675, 1456 }, 670, 3 };
        yield return new object[] { new List<int> { 1721, 979, 366, -299, -675, 1456 }, -974, 2 };
      }
    }
  }
}
