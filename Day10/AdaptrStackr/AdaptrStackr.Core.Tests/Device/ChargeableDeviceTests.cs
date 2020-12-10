using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using AdaptrStackr.Core.Device;
using System.Linq;

namespace AdaptrStackr.Core.Tests.Device
{
  public class ChargeableDeviceTests
  {
    [Fact]
    public void BareCostructor_GivenNullAdapter_ThrowsException()
    {
      _ = Assert.Throws<ArgumentException>(() => new ChargeableDevice((IChargeableDevice)null));
    }

    [Fact]
    public void AdapterSelectorConstructor_ReturningNullAdapter_ThrowsException()
    {
      _ = Assert.Throws<ArgumentException>(
          () => new ChargeableDevice(
            null,
            (sockets) => (IChargeableDevice)null
          )
        );
    }

    [Theory]
    [MemberData(nameof(OutputSelectorTestData))]
    public void OutputSelectorConstructor_HavingNoSockets_ThrowsException(IEnumerable<ISocket> sockets)
    {
      _ = Assert.ThrowsAny<SystemException>(
          () => new ChargeableDevice(
            sockets,
            (sockets) => sockets.FirstOrDefault().Output
          )
        );
    }

    [Fact]
    public void SourceSelectorConstructor_FindingNoSource_ThrowsException()
    {
      _ = Assert.ThrowsAny<Exception>(() => new ChargeableDevice(null, (sockets) => (ISocket)null));
    }

    [Theory]
    [MemberData(nameof(OutputSelectorTestData))]
    public void SourcesSelector_HavingNoSources_ThrowsException(IEnumerable<ISocket> sockets)
    {
      _ = Assert.ThrowsAny<SystemException>(() => new ChargeableDevice(sockets));
    }

    [Theory]
    [MemberData(nameof(ChargeableDeviceData))]
    public void Constructor_GivenValidChargeableDevice_SetsPropertiesAsExpected(IChargeableDevice device, int expectedLowInput, int expectedHighInput, IDictionary<int, bool> testSocketData)
    {
      var sut = new ChargeableDevice(device);
      Assert.Equal(expectedLowInput, sut.LowInput);
      Assert.Equal(expectedHighInput, sut.HighInput);
      foreach (var kvp in testSocketData)
      {
        var testSocket = MockSocket(kvp.Key);

        Assert.Equal(kvp.Value, sut.CanAttach(testSocket));
      }
    }

    [Theory]
    [MemberData(nameof(SourceAndAdapterTestData))]
    public void Constructor_GivenSourcesAndAdapterSelector_SetsPropertiesAsExpected(
      IEnumerable<ISocket> sources,
      Func<IEnumerable<ISocket>, IChargeableDevice> selector,
      int expectedLowInput,
      int expectedHighInput,
      IDictionary<int, bool> testSocketData
    )
    {
      var sut = new ChargeableDevice(sources, selector);
      Assert.Equal(expectedLowInput, sut.LowInput);
      Assert.Equal(expectedHighInput, sut.HighInput);
      foreach (var kvp in testSocketData)
      {
        var testSocket = MockSocket(kvp.Key);

        Assert.Equal(kvp.Value, sut.CanAttach(testSocket));
      }
    }

    [Theory]
    [MemberData(nameof(SourceAndOutputSelectorTestData))]
    public void Constructor_GivenSourcesAndOutputSelector_SetsPropertiesAsExpected(
      IEnumerable<ISocket> sources,
      Func<IEnumerable<ISocket>, int> outputSelector,
      int expectedLowInput,
      int expectedHighInput,
      IDictionary<int, bool> testSocketData
    )
    {
      var sut = new ChargeableDevice(sources, outputSelector);
      Assert.Equal(expectedLowInput, sut.LowInput);
      Assert.Equal(expectedHighInput, sut.HighInput);
      foreach (var kvp in testSocketData)
      {
        var testSocket = MockSocket(kvp.Key);

        Assert.Equal(kvp.Value, sut.CanAttach(testSocket));
      }
    }

    [Theory]
    [MemberData(nameof(SourceAndSourceSelectorTestData))]
    public void Constructor_GivenSourcesAndSourceSelector_SetsPropertiesAsExpected(
      IEnumerable<ISocket> sources,
      Func<IEnumerable<ISocket>, ISocket> sourceSelector,
      int expectedLowInput,
      int expectedHighInput,
      IDictionary<int, bool> testSocketData
    )
    {
      var sut = new ChargeableDevice(sources, sourceSelector);
      Assert.Equal(expectedLowInput, sut.LowInput);
      Assert.Equal(expectedHighInput, sut.HighInput);
      foreach (var kvp in testSocketData)
      {
        var testSocket = MockSocket(kvp.Key);

        Assert.Equal(kvp.Value, sut.CanAttach(testSocket));
      }
    }

    [Theory]
    [MemberData(nameof(SourcesTestData))]
    public void Constructor_GivenSources_SetsPropertiesAsExpected(
      IEnumerable<ISocket> sources,
      int expectedLowInput,
      int expectedHighInput,
      IDictionary<int, bool> testSocketData
    )
    {
      var sut = new ChargeableDevice(sources);
      Assert.Equal(expectedLowInput, sut.LowInput);
      Assert.Equal(expectedHighInput, sut.HighInput);
      foreach (var kvp in testSocketData)
      {
        var testSocket = MockSocket(kvp.Key);

        Assert.Equal(kvp.Value, sut.CanAttach(testSocket));
      }
    }

    public static IEnumerable<object[]> OutputSelectorTestData
    {
      get
      {
        yield return new object[]
        {
          null
        };

        yield return new object[]
        {
          new ISocket[]{ }
        };
      }
    }

    public static IEnumerable<object[]> ChargeableDeviceData
    {
      get
      {
        yield return new object[]
        {
          MockDevice(0, 0, false),
          0,
          0,
          new Dictionary<int, bool>
          {
            {int.MinValue, false },
            {-2, false },
            {-1, false },
            {0, false },
            {1, false },
            {2, false },
            {int.MaxValue, false }
          }
        };

        yield return new object[]
        {
          MockDevice(49, -54, true),
          49,
          -54,
          new Dictionary<int, bool>
          {
            {int.MinValue, true },
            {-2, true },
            {-1, true },
            {0, true },
            {1, true },
            {2, true },
            {int.MaxValue, true }
          }
        };

        yield return new object[]
        {
          MockDevice(17, 200, (s) => s.Output > 0),
          17,
          200,
          new Dictionary<int, bool>
          {
            {int.MinValue, false },
            {-2, false },
            {-1, false },
            {0, false },
            {1, true },
            {2, true },
            {int.MaxValue, true }
          }
        };
      }
    } 

    public static IEnumerable<object[]> SourceAndAdapterTestData
    {
      get
      {
        yield return new object[]
        {
          null,
          new Func<IEnumerable<ISocket>, IChargeableDevice>((sockets) => MockDevice(10, 20, false)),
          10,
          20,
          new Dictionary<int, bool>
          {
            {int.MinValue, false },
            {-2, false },
            {-1, false },
            {0, false },
            {1, false },
            {2, false },
            {int.MaxValue, false }
          }
        };

        yield return new object[]
        {
          null,
          new Func<IEnumerable<ISocket>, IChargeableDevice>((sockets) => MockDevice(49, -54, true)),
          49,
          -54,
          new Dictionary<int, bool>
          {
            {int.MinValue, true },
            {-2, true },
            {-1, true },
            {0, true },
            {1, true },
            {2, true },
            {int.MaxValue, true }
          }
        };

        yield return new object[]
        {
          null,
          new Func<IEnumerable<ISocket>, IChargeableDevice>((sockets) => MockDevice(17, 200, (s) => s.Output > 0)),
          17,
          200,
          new Dictionary<int, bool>
          {
            {int.MinValue, false },
            {-2, false },
            {-1, false },
            {0, false },
            {1, true },
            {2, true },
            {int.MaxValue, true }
          }
        };

        yield return new object[]
        {
          new ISocket[] { MockSocket(4) },
          new Func<IEnumerable<ISocket>, IChargeableDevice>(
            (sockets) => {
              var socket = sockets.First();
              return MockDevice(socket.Output, socket.Output * 2, (s) => s.Output > socket.Output);
            }),
          4,
          8,
          new Dictionary<int, bool>
          {
            {int.MinValue, false },
            {2, false },
            {3, false },
            {4, false },
            {5, true },
            {6, true },
            {int.MaxValue, true }
          }
        };

        yield return new object[]
        {
          new ISocket[] { MockSocket(3), MockSocket(2), MockSocket(4) },
          new Func<IEnumerable<ISocket>, IChargeableDevice>(
            (sockets) => {
              var socket = sockets.OrderBy(s => s.Output).First();
              return MockDevice(socket.Output, socket.Output * 2, (s) => s.Output <= socket.Output);
            }),
          2,
          4,
          new Dictionary<int, bool>
          {
            {int.MinValue, true },
            {2, true },
            {3, false },
            {4, false },
            {5, false },
            {6, false },
            {int.MaxValue, false }
          }
        };
      }
    }

    public static IEnumerable<object[]> SourceAndOutputSelectorTestData
    {
      get
      {
        yield return new object[] {
          null,
          new Func<IEnumerable<ISocket>, int>((s) => 5),
          2,
          4,
          new Dictionary<int, bool>
          {
            {int.MinValue, false },
            {-2, false },
            {-1, false },
            {0, false },
            {1, false },
            {2, true },
            {3, true },
            {4, true },
            {5, false },
            {int.MaxValue, false }
          }
        };

        yield return new object[] {
          new ISocket[] { MockSocket(4) },
          new Func<IEnumerable<ISocket>, int>((s) => s.First().Output),
          1,
          3,
          new Dictionary<int, bool>
          {
            {int.MinValue, false },
            {-2, false },
            {-1, false },
            {0, false },
            {1, true },
            {2, true },
            {3, true },
            {4, false },
            {5, false },
            {int.MaxValue, false }
          }
        };

        yield return new object[] {
          new ISocket[] { MockSocket(4), MockSocket(6), MockSocket(0) },
          new Func<IEnumerable<ISocket>, int>((s) => s.OrderByDescending(s=>s.Output).First().Output),
          3,
          5,
          new Dictionary<int, bool>
          {
            {int.MinValue, false },
            {-2, false },
            {-1, false },
            {0, false },
            {1, false },
            {2, false },
            {3, true },
            {4, true },
            {5, true },
            {6, false },
            {7, false},
            {int.MaxValue, false }
          }
        };
      }
    }

    public static IEnumerable<object[]> SourceAndSourceSelectorTestData
    {
      get
      {
        yield return new object[]
        {
          null,
          new Func<IEnumerable<ISocket>, ISocket>((s) => MockSocket(14)),
          14,
          16,
          new Dictionary<int, bool>
          {
            {int.MinValue, false },
            {11, false },
            {12, false },
            {13, false },
            {14, true },
            {15, true },
            {16, true },
            {17, false },
            {18, false },
            {19, false },
            {20, false},
            {int.MaxValue, false }
          }
        };

        yield return new object[]
        {
          new ISocket[] { MockSocket(-12) },
          new Func<IEnumerable<ISocket>, ISocket>((s) => s.First()),
          -12,
          -10,
          new Dictionary<int, bool>
          {
            {int.MaxValue, false },
            {-8, false },
            {-9, false },
            {-10, true },
            {-11, true },
            {-12, true },
            {-13, false },
            {-14, false },
            {-15, false },
            {-16, false },
            {-17, false },
            {-18, false },
            {-19, false },
            {-20, false},
            {int.MinValue, false }
          }
        };

        yield return new object[]
        {
          new ISocket[] { MockSocket(9), MockSocket(4), MockSocket(1234), MockSocket(6) },
          new Func<IEnumerable<ISocket>, ISocket>((s) => s.OrderByDescending(s => s.Output).First()),
          1234,
          1236,
          new Dictionary<int, bool>
          {
            {int.MinValue, false },
            {1231, false },
            {1232, false },
            {1233, false },
            {1234, true },
            {1235, true },
            {1236, true },
            {1237, false },
            {1238, false },
            {1239, false },
            {1240, false},
            {int.MaxValue, false }
          }
        };
        yield return new object[]
        {
          null,
          new Func<IEnumerable<ISocket>, ISocket>((s) => MockSocket(14)),
          14,
          16,
          new Dictionary<int, bool>
          {
            {int.MinValue, false },
            {11, false },
            {12, false },
            {13, false },
            {14, true },
            {15, true },
            {16, true },
            {17, false },
            {18, false },
            {19, false },
            {20, false},
            {int.MaxValue, false }
          }
        };
      }
    }

    public static IEnumerable<object[]> SourcesTestData
    {
      get
      {
        yield return new object[]
        {
          new ISocket[] { MockSocket(4) },
          4,
          6,
          new Dictionary<int, bool>
          {
            {int.MinValue, false },
            {1, false },
            {2, false },
            {3, false },
            {4, true },
            {5, true },
            {6, true },
            {7, false },
            {8, false },
            {9, false },
            {10, false},
            {int.MaxValue, false }
          }
        };

        yield return new object[]
        {
          new ISocket[] { MockSocket(4), MockSocket(7) },
          7,
          9,
          new Dictionary<int, bool>
          {
            {int.MinValue, false },
            {4, false },
            {5, false },
            {6, false },
            {7, true },
            {8, true },
            {9, true },
            {10, false },
            {11, false },
            {12, false },
            {13, false },
            {int.MaxValue, false }
          }
        };

        yield return new object[]
        {
          new ISocket[] { MockSocket(4), MockSocket(40), MockSocket(7) },
          40,
          42,
          new Dictionary<int, bool>
          {
            {int.MinValue, false },
            {37, false },
            {38, false },
            {39, false },
            {40, true },
            {41, true },
            {42, true },
            {43, false },
            {44, false },
            {45, false },
            {46, false },
            {int.MaxValue, false }
          }
        };

        yield return new object[]
        {
          new ISocket[] { MockSocket(-4), MockSocket(-40), MockSocket(-7) },
          -4,
          -2,
          new Dictionary<int, bool>
          {
            {int.MinValue, false },
            {-7, false },
            {-6, false },
            {-5, false },
            {-4, true },
            {-3, true },
            {-2, true },
            {-1, false },
            {0, false },
            {1, false },
            {2, false },
            {int.MaxValue, false }
          }
        };

        yield return new object[]
        {
          new ISocket[] { MockSocket(19) },
          19,
          21,
          new Dictionary<int, bool>
          {
            {int.MinValue, false },
            {16, false },
            {17, false },
            {18, false },
            {19, true },
            {20, true },
            {21, true },
            {22, false },
            {23, false },
            {24, false },
            {25, false },
            {int.MaxValue, false }
          }
        };
      }
    }

    public static IChargeableDevice MockDevice(
      int desiredLowInput,
      int desiredHighInput,
      bool canAttachResult
    )
    {
      return MockDevice(desiredLowInput, desiredHighInput, (s) => canAttachResult);
    }

    public static IChargeableDevice MockDevice(
      int desiredLowInput,
      int desiredHighInput,
      Func<ISocket, bool> canAttach
    )
    {
      var deviceMock = new Mock<IChargeableDevice>();
      deviceMock.Setup(d => d.LowInput)
        .Returns(desiredLowInput);
      deviceMock.Setup(d => d.HighInput)
        .Returns(desiredHighInput);
      deviceMock.Setup(d => d.CanAttach(It.IsAny<ISocket>()))
        .Returns((ISocket socket) => canAttach(socket));
      return deviceMock.Object;
    }

    public static ISocket MockSocket(int desiredOutput)
    {
      var socketMock = new Mock<ISocket>();
      socketMock.Setup(s => s.Output).Returns(desiredOutput);

      return socketMock.Object;
    }
  }
}
