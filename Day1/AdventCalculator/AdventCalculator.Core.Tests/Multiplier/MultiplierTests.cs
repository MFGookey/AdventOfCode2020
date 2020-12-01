using System.Collections.Generic;
using Xunit;
using sut = AdventCalculator.Core.Multiplier;

namespace AdventCalculator.Core.Tests.Multiplier
{
  public class MultiplierTests
  {
    [Fact]
    void Multiply_NullMultiplicands_ReturnsNull()
    {
      var sut = new sut.Multiplier();
      var result = sut.Multiply(null);
      Assert.Null(result);
    }

    [Fact]
    void Multiply_EmptyMultiplicands_ReturnsNull()
    {
      var sut = new sut.Multiplier();
      var result = sut.Multiply(new List<int>());
      Assert.Null(result);
    }

    [Fact]
    void Multiply_SingleMultiplicand_ReturnsNull()
    {
      var sut = new sut.Multiplier();
      var result = sut.Multiply(new List<int> { 42 });
      Assert.Null(result);
    }

    [Theory]
    [MemberData(nameof(ValidOptions))]
    void Multiply_ValidMultiplicands_ReturnsProduct(
      IList<int> multiplicands,
      int expectedProduct
    )
    {
      var sut = new sut.Multiplier();
      var result = sut.Multiply(multiplicands);
      Assert.NotNull(result);
      Assert.Equal(expectedProduct, result.Value);
    }

    public static IEnumerable<object[]> ValidOptions
    {
      get
      {
        yield return new object[] {
          new List<int> { 1, 2, 3 },
          6
        };

        yield return new object[] {
          new List<int> { 2, 3 },
          6
        };

        yield return new object[] {
          new List<int> { -1, 1, 5, 6 },
          -30
        };

        yield return new object[] {
          new List<int> { 1, 1, 1, 1, 1 },
          1
        };

        // Provided by the Day 1 problem 1 description itself
        yield return new object[] {
          new List<int> { 1721, 299 },
          514579
        };
        
        yield return new object[] {
          new List<int> { 1721, 979, 366 },
          616658394
        };

        yield return new object[] {
          new List<int> { 1, -1 },
          -1
        };

        yield return new object[] {
          new List<int> { -2, -2 },
          4
        };

        yield return new object[] {
          new List<int> { -2, -2, -5 },
          -20
        };

        // Provided by the Day 1 problem 2 description itself
        yield return new object[] {
          new List<int> { 979, 366, 675 },
          241861950
        };
      }
    }
  }
}
