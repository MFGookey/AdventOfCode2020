using System.Collections.Generic;
using System.Linq;
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
    void SumFinder_TotalTermsMoreThanOneAndValidOptions_ReturnsValidOptions(IList<int> haystack, IList<int> needles, int totalTerms)
    {
      var sut = new sut.Selector();
      var result = sut.SumFinder(haystack, needles.Sum(), totalTerms);

      Assert.Equal(totalTerms, result.Count);
      Assert.Equal(needles.Count, result.Count);
      Assert.Equal(needles.Sum(), result.Sum());
      Assert.Equal(needles.OrderBy(i => i), result.OrderBy(i => i));
    }

    public static IEnumerable<object[]> ValidOptions {
      get
      {
        yield return new object[] {
          new List<int> { 1, 2, 3 },
          new List<int> { 1, 3 },
          2
        };

        // Same as above, needles in reversed order
        yield return new object[] {
          new List<int> { 1, 2, 3 },
          new List<int> { 3, 1 },
          2
        };

        yield return new object[] {
          new List<int> { -1, 1, 5, 6 },
          new List<int> { 5, 1 },
          2
        };

        yield return new object[] {
          new List<int> { 1, 1, 1, 1, 1 },
          new List<int>{ 1, 1, 1, 1 },
          4
        };
        
        // Provided by the Day 1 problem 1 description itself
        yield return new object[] {
          new List<int> { 1721, 979, 366, 299, 675, 1456 },
          new List<int> { 1721, 299 },
          2
        };
        
        yield return new object[] {
          new List<int> { 1721, 979, 366, 299, 675, 1456 },
          new List<int> { 299, 366 },
          2
        };

        yield return new object[] {
          new List<int> { 1721, 979, 366, 299, -675, 1456 },
          new List<int> { 979, -675, 366 },
          3
        };
        
        yield return new object[] {
          new List<int> { 1721, 979, 366, -299, -675, 1456 },
          new List<int> {-299, -675 },
          2
        };

        // Provided by the Day 2 problem 2 description itself
        yield return new object[] {
          new List<int> { 1721, 979, 366, 299, 675, 1456 },
          new List<int> { 979, 366, 675 },
          3
        };
      }
    }
  }
}
