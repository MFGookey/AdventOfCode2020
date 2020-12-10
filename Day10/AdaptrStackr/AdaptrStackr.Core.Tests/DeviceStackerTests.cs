using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using AdaptrStackr.Core.Device;

namespace AdaptrStackr.Core.Tests
{
  public class DeviceStackerTests
  {
    [Theory]
    [MemberData(nameof(GoodStackData))]
    public void CreateStack_GivenGoodData_ReturnsExpectedResults(IEnumerable<IDevice> toStack, int expectedResult)
    {
      var sut = new DeviceStacker();
      var result = sut.CreateStack(toStack);
      Assert.Equal(expectedResult, result);
    }

    [Theory]
    [MemberData(nameof(BadStackData))]
    public void CreateStack_GivenBadData_ThrowsArgumentExceptions(IEnumerable<IDevice> toStack)
    {
      var sut = new DeviceStacker();
      Assert.ThrowsAny<SystemException>(() => sut.CreateStack(toStack));
    }

    public static IEnumerable<object[]> GoodStackData {
      get
      {
        yield return new object[]
        {
          new IDevice[]
          {
            new Adapter(16),
            new Adapter(10),
            new Adapter(15),
            new Adapter(5),
            new Adapter(1),
            new Adapter(11),
            new Adapter(7),
            new Adapter(19),
            new Adapter(6),
            new Adapter(12),
            new Adapter(4)
          },
          35
        };

        yield return new object[]
        {
          new IDevice[]
          {
            new Adapter(28),
            new Adapter(33),
            new Adapter(18),
            new Adapter(42),
            new Adapter(31),
            new Adapter(14),
            new Adapter(46),
            new Adapter(20),
            new Adapter(48),
            new Adapter(47),
            new Adapter(24),
            new Adapter(23),
            new Adapter(49),
            new Adapter(45),
            new Adapter(19),
            new Adapter(38),
            new Adapter(39),
            new Adapter(11),
            new Adapter(1),
            new Adapter(32),
            new Adapter(25),
            new Adapter(35),
            new Adapter(8),
            new Adapter(17),
            new Adapter(7),
            new Adapter(9),
            new Adapter(4),
            new Adapter(2),
            new Adapter(34),
            new Adapter(10),
            new Adapter(3)
          },
          220
        };

        yield return new object[]
        {
          new IDevice[]
          {
            new Adapter(16),
            new Adapter(10),
            new Adapter(15),
            new Adapter(5),
            new Adapter(1),
            new Adapter(11),
            new Socket(),
            new Adapter(7),
            new Adapter(19),
            new Adapter(6),
            new Adapter(12),
            new Adapter(4)
          },
          35
        };

        yield return new object[]
        {
          new IDevice[]
          {
            new Adapter(28),
            new Adapter(33),
            new Adapter(18),
            new Adapter(42),
            new Adapter(31),
            new Adapter(14),
            new Adapter(46),
            new Adapter(20),
            new Adapter(48),
            new Adapter(47),
            new Adapter(24),
            new Adapter(23),
            new Adapter(49),
            new Adapter(45),
            new Socket(),
            new Adapter(19),
            new Adapter(38),
            new Adapter(39),
            new Adapter(11),
            new Adapter(1),
            new Adapter(32),
            new Adapter(25),
            new Adapter(35),
            new Adapter(8),
            new Adapter(17),
            new Adapter(7),
            new Adapter(9),
            new Adapter(4),
            new Adapter(2),
            new Adapter(34),
            new Adapter(10),
            new Adapter(3)
          },
          220
        };
        yield return new object[]
      {
          new IDevice[]
          {
            new Adapter(16),
            new Adapter(10),
            new Adapter(15),
            new Adapter(5),
            new Adapter(1),
            new Adapter(11),
            new Adapter(7),
            new Adapter(19),
            new ChargeableDevice(new Adapter(22)),
            new Adapter(6),
            new Adapter(12),
            new Adapter(4)
          },
          35
      };

        yield return new object[]
        {
          new IDevice[]
          {
            new Adapter(28),
            new Adapter(33),
            new Adapter(18),
            new Adapter(42),
            new Adapter(31),
            new Adapter(14),
            new Adapter(46),
            new Adapter(20),
            new Adapter(48),
            new Adapter(47),
            new Adapter(24),
            new Adapter(23),
            new Adapter(49),
            new Adapter(45),
            new Adapter(19),
            new Adapter(38),
            new Adapter(39),
            new Adapter(11),
            new Adapter(1),
            new Adapter(32),
            new Adapter(25),
            new Adapter(35),
            new Adapter(8),
            new Adapter(17),
            new Adapter(7),
            new Adapter(9),
            new ChargeableDevice(new Adapter(52)),
            new Adapter(4),
            new Adapter(2),
            new Adapter(34),
            new Adapter(10),
            new Adapter(3)
          },
          220
        };
        yield return new object[]
      {
          new IDevice[]
          {
            new Adapter(16),
            new Adapter(10),
            new Adapter(15),
            new Adapter(5),
            new ChargeableDevice(new Adapter(22)),
            new Adapter(1),
            new Adapter(11),
            new Adapter(7),
            new Socket(),
            new Adapter(19),
            new Adapter(6),
            new Adapter(12),
            new Adapter(4)
          },
          35
      };

        yield return new object[]
        {
          new IDevice[]
          {
            new Adapter(28),
            new Adapter(33),
            new Adapter(18),
            new Adapter(42),
            new Adapter(31),
            new Adapter(14),
            new ChargeableDevice(new Adapter(52)),
            new Adapter(46),
            new Adapter(20),
            new Adapter(48),
            new Adapter(47),
            new Adapter(24),
            new Adapter(23),
            new Adapter(49),
            new Adapter(45),
            new Adapter(19),
            new Adapter(38),
            new Adapter(39),
            new Adapter(11),
            new Socket(),
            new Adapter(1),
            new Adapter(32),
            new Adapter(25),
            new Adapter(35),
            new Adapter(8),
            new Adapter(17),
            new Adapter(7),
            new Adapter(9),
            new Adapter(4),
            new Adapter(2),
            new Adapter(34),
            new Adapter(10),
            new Adapter(3)
          },
          220
        };
      }
    }

    public static IEnumerable<object[]> BadStackData
    {
      get
      {
        yield return new object[]
        {
          null
        };

        yield return new object[]
        {
          new IDevice[]{ }
        };

        yield return new object[]
        {
          new IDevice[]{
            new Socket(),
            new Socket()
          }
        };

        yield return new object[]
        {
          new IDevice[]{
            new ChargeableDevice(new Adapter(5)),
            new ChargeableDevice(new Adapter(5))
          }
        };

        yield return new object[]
        {
          new IDevice[]{
            new Adapter(7)
          }
        };

        yield return new object[]
        {
          new IDevice[]{
            new ChargeableDevice(new Adapter(7)),
            new Socket()
          }
        };
      }
    }
  }
}
