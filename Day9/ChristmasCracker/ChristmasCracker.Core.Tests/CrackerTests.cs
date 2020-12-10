using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ChristmasCracker.Core.Tests
{
  public class CrackerTests
  {
    [Fact]
    public void Crack_GivenZeroRequiredTerms_ThrowsException()
    {
      var sut = new Cracker();
      _ = Assert.Throws<ArgumentException>(() => sut.FindUnsummableNumbers(1, 0, new long[] { }));
    }

    [Fact]
    public void Crack_RequiredTermsLargerThanPreamble_ThrowsException()
    {
      var sut = new Cracker();
      _ = Assert.Throws<ArgumentException>(() => sut.FindUnsummableNumbers(10, 11, new long[] { }));
    }

    [Fact]
    public void Crack_PreambleSameSizeAsMessage_ThrowsException()
    {
      var sut = new Cracker();
      _ = Assert.Throws<ArgumentException>(() => sut.FindUnsummableNumbers(3, 2, new long[] { 1, 2, 3 }));
    }

    [Theory]
    [MemberData(nameof(XMASMessages))]
    public void Crack_GivenValidXMASMessages_ReturnsExpectedResults(int preambleLength, int requiredTerms, IEnumerable<long> message, IEnumerable<long> expectedResults)
    {
      var sut = new Cracker();
      var result = sut.FindUnsummableNumbers(preambleLength, requiredTerms, message);

      Assert.Equal(expectedResults, result);
    }

    [Fact]
    public void AttackUnsummableNumber_GivenValidMessageAndNumber_ReturnsExpectedResults()
    {
      var message = new long[]{
        35,
        20,
        15,
        25,
        47,
        40,
        62,
        55,
        65,
        95,
        102,
        117,
        150,
        182,
        127,
        219,
        299,
        277,
        309,
        576
      };

      var unSummableNumber = 127L;

      var expectedResults = new long[]
      {
        15,
        25,
        47,
        40
      };

      var sut = new Cracker();

      var result = sut.AttackUnsummableNumber(unSummableNumber, message);

      Assert.Equal(expectedResults, result);
    }

    public static IEnumerable<object[]> XMASMessages
    {
      get
      {
        yield return new object[]
        {
          2,
          2,
          new long[] {1, 2, 5},
          new long[] {5}
        };

        yield return new object[]
        {
          2,
          2,
          new long[] {1, 2, 3},
          new long[] {}
        };

        yield return new object[]
        {
          5,
          2,
          new long[]
          {
            35,
            20,
            15,
            25,
            47,
            40,
            62,
            55,
            65,
            95,
            102,
            117,
            150,
            182,
            127,
            219,
            299,
            277,
            309,
            576
          },
          new long[] { 127 }
        };
      }
    }
  }
}
