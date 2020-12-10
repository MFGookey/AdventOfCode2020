using System;
using System.Collections.Generic;
using System.Text;
using AdaptrStackr.Core.Device;
using Xunit;
using Moq;

namespace AdaptrStackr.Core.Tests.Device
{
  public class AdapterTests
  {
    [Fact]
    public void Constructor_GivenLowInputHigherThanHighInput_ThrowsException()
    {
      _ = Assert.Throws<ArgumentException>(()=> new Adapter(0, (input) => 1, (input) => 0));
    }

    [Theory]
    [MemberData(nameof(AdapterWithDerivers))]
    public void Constructor_GivenProperties_SetsValuesAsExpected(
      int output,
      Func<int, int> lowInputDeriver,
      Func<int, int> highInputDeriver,
      int expectedOutput,
      int expectedLowInput,
      int expectedHighInput
    )
    {
      var sut = new Adapter(output, lowInputDeriver, highInputDeriver);

      Assert.Equal(expectedOutput, sut.Output);
      Assert.Equal(expectedLowInput, sut.LowInput);
      Assert.Equal(expectedHighInput, sut.HighInput);
    }

    [Theory]
    [MemberData(nameof(AdapterOutputs))]
    public void Constructor_GivenOutput_SetsValuesAsExpected(int output, int expectedOutput, int expectedLowInput, int expectedHighInput)
    {
      var sut = new Adapter(output);
      Assert.Equal(expectedOutput, sut.Output);
      Assert.Equal(expectedLowInput, sut.LowInput);
      Assert.Equal(expectedHighInput, sut.HighInput);
    }

    [Theory]
    [MemberData(nameof(AdaptersAndSockets))]
    public void CanAttach_GivenSocket_ReturnsExpectedValue(
      int output,
      Func<int, int> lowInputDeriver,
      Func<int, int> highInputDeriver,
      ISocket socketToTest,
      bool expectedAttachResult)
    {
      var sut = new Adapter(output, lowInputDeriver, highInputDeriver);
      var result = sut.CanAttach(socketToTest);

      Assert.Equal(expectedAttachResult, result);
    }

    public static IEnumerable<object[]> AdapterWithDerivers
    {
      get
      {
        yield return new object[] {
          10,
          new Func<int, int>((input) => 5),
          new Func<int, int>((input) => 6),
          10,
          5,
          6
        };

        yield return new object[] {
          1,
          new Func<int, int>((input) => 1),
          new Func<int, int>((input) => 1),
          1,
          1,
          1
        };
        
        yield return new object[] {
          10,
          new Func<int, int>((input) => 2 * input),
          new Func<int, int>((input) => 3 * input),
          10,
          20,
          30
        };
      }
    }

    public static IEnumerable<object[]> AdapterOutputs
    {
      get
      {
        yield return new object[]
        {
          0, 0, -3, -1
        };

        yield return new object[]
        {
          5, 5, 2, 4
        };

        yield return new object[]
        {
          3, 3, 0, 2
        };

        yield return new object[]
        {
          1, 1, -2, 0
        };

        yield return new object[]
        {
          4, 4, 1, 3
        };

        yield return new object[]
        {
          5, 5, 2, 4
        };

        yield return new object[]
        {
          6, 6, 3, 5
        };

        yield return new object[]
        {
          7, 7, 4, 6
        };

        yield return new object[]
        {
          10, 10, 7, 9
        };

        yield return new object[]
        {
          11, 11, 8, 10
        };

        yield return new object[]
        {
          12, 12, 9, 11
        };

        yield return new object[]
        {
          15, 15, 12, 14
        };

        yield return new object[]
        {
          16, 16, 13, 15
        };

        yield return new object[]
        {
          19, 19, 16, 18
        };
      }
    }

    public static IEnumerable<object[]> AdaptersAndSockets
    {
      get
      {
        yield return new object[]
        {
          10,
          new Func<int, int>((input) => int.MinValue),
          new Func<int, int>((input) => int.MaxValue),
          null,
          false
        };

        yield return new object[]
        {
          10,
          new Func<int, int>((input) => int.MinValue),
          new Func<int, int>((input) => int.MaxValue),
          MockSocket(int.MinValue),
          true
        };

        yield return new object[]
        {
          10,
          new Func<int, int>((input) => int.MinValue),
          new Func<int, int>((input) => int.MaxValue),
          MockSocket(int.MaxValue),
          true
        };

        yield return new object[]
        {
          10,
          new Func<int, int>((input) => int.MinValue),
          new Func<int, int>((input) => 0),
          MockSocket(1),
          false
        };

        yield return new object[]
        {
          10,
          new Func<int, int>((input) => int.MinValue),
          new Func<int, int>((input) => 0),
          MockSocket(0),
          true
        };

        yield return new object[]
        {
          10,
          new Func<int, int>((input) => 0),
          new Func<int, int>((input) => int.MaxValue),
          MockSocket(-1),
          false
        };

        yield return new object[]
        {
          10,
          new Func<int, int>((input) => 0),
          new Func<int, int>((input) => int.MaxValue),
          MockSocket(0),
          true
        };

        yield return new object[]
        {
          10,
          new Func<int, int>((input) => 0),
          new Func<int, int>((input) => int.MaxValue),
          MockSocket(1),
          true
        };

        yield return new object[]
        {
          10,
          new Func<int, int>((input) => 1),
          new Func<int, int>((input) => 1),
          MockSocket(1),
          true
        };

        yield return new object[]
        {
          10,
          new Func<int, int>((input) => 0),
          new Func<int, int>((input) => 2),
          MockSocket(1),
          true
        };

        yield return new object[]
        {
          10,
          new Func<int, int>((input) => input * 2),
          new Func<int, int>((input) => input * 3),
          MockSocket(10),
          false
        };

        yield return new object[]
        {
          10,
          new Func<int, int>((input) => input * 2),
          new Func<int, int>((input) => input * 3),
          MockSocket(19),
          false
        };

        yield return new object[]
        {
          10,
          new Func<int, int>((input) => input * 2),
          new Func<int, int>((input) => input * 3),
          MockSocket(20),
          true
        };

        yield return new object[]
        {
          10,
          new Func<int, int>((input) => input * 2),
          new Func<int, int>((input) => input * 3),
          MockSocket(29),
          true
        };

        yield return new object[]
        {
          10,
          new Func<int, int>((input) => input * 2),
          new Func<int, int>((input) => input * 3),
          MockSocket(30),
          true
        };

        yield return new object[]
        {
          10,
          new Func<int, int>((input) => input * 2),
          new Func<int, int>((input) => input * 3),
          MockSocket(31),
          false
        };

        yield return new object[]
        {
          1,
          new Func<int, int>((input) => input - 3),
          new Func<int, int>((input) => input - 1),
          MockSocket(0),
          true
        };

        yield return new object[]
        {
          4,
          new Func<int, int>((input) => input - 3),
          new Func<int, int>((input) => input - 1),
          MockSocket(1),
          true
        };

        yield return new object[]
        {
          5,
          new Func<int, int>((input) => input - 3),
          new Func<int, int>((input) => input - 1),
          MockSocket(4),
          true
        };

        yield return new object[]
        {
          6,
          new Func<int, int>((input) => input - 3),
          new Func<int, int>((input) => input - 1),
          MockSocket(5),
          true
        };

        yield return new object[]
        {
          7,
          new Func<int, int>((input) => input - 3),
          new Func<int, int>((input) => input - 1),
          MockSocket(6),
          true
        };

        yield return new object[]
        {
          10,
          new Func<int, int>((input) => input - 3),
          new Func<int, int>((input) => input - 1),
          MockSocket(7),
          true
        };

        yield return new object[]
        {
          11,
          new Func<int, int>((input) => input - 3),
          new Func<int, int>((input) => input - 1),
          MockSocket(10),
          true
        };

        yield return new object[]
        {
          12,
          new Func<int, int>((input) => input - 3),
          new Func<int, int>((input) => input - 1),
          MockSocket(11),
          true
        };

        yield return new object[]
        {
          15,
          new Func<int, int>((input) => input - 3),
          new Func<int, int>((input) => input - 1),
          MockSocket(12),
          true
        };

        yield return new object[]
        {
          16,
          new Func<int, int>((input) => input - 3),
          new Func<int, int>((input) => input - 1),
          MockSocket(15),
          true
        };

        yield return new object[]
        {
          19,
          new Func<int, int>((input) => input - 3),
          new Func<int, int>((input) => input - 1),
          MockSocket(16),
          true
        };

        yield return new object[]
        {
          22,
          new Func<int, int>((input) => input - 3),
          new Func<int, int>((input) => input - 1),
          MockSocket(19),
          true
        };
      }
    }

    public static ISocket MockSocket(int desiredOutput)
    {
      var socketMock = new Mock<ISocket>();
      socketMock.Setup(s => s.Output).Returns(desiredOutput);
      return socketMock.Object;
    }
  }
}
