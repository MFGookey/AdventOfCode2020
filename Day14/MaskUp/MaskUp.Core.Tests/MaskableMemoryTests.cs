using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace MaskUp.Core.Tests
{
  public class MaskableMemoryTests
  {
    [Fact]
    public void Constructor_Constructs_SetsMaskAsExpected()
    {
      var sut = new MaskableMemory();
      Assert.Equal("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX", sut.Mask);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("XXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X")]
    [InlineData("XXXX2XXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X")]
    public void SetMask_GivenMalformedString_FormatException(string badString)
    {
      var sut = new MaskableMemory();
      Assert.Throws<FormatException>(() => sut.SetMask(badString));
    }

    [Theory]
    [InlineData("XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X", 8, 11, 73)]
    [InlineData("XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X", 8, 101, 101)]
    [InlineData("XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X", 8, 0, 64)]
    [InlineData("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX", 8, 11, 11)]
    [InlineData("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX", 8, 101, 101)]
    [InlineData("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX", 8, 0, 0)]
    public void Index_GivenMaskAddressAndValue_SetsValueAsExpected(string mask, long address, long value, long expectedValue)
    {
      var sut = new MaskableMemory();
      sut.SetMask(mask);
      sut[address] = value;

      Assert.Equal(expectedValue, sut[address]);
    }

    [Fact]
    public void ProcessInstructions_GivenInstructions_SetsMemoryAddressesAsExpected()
    {
      var instructions = new[]
      {
        "mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X",
        "mem[8] = 11",
        "mem[9] = 11",
        "mem[7] = 101",
        "mem[8] = 0"
      };

      var sut = new MaskableMemory();
      sut.ProcessInstructions(instructions);

      Assert.Equal(64, sut[8]);
      Assert.Equal(101, sut[7]);
      Assert.Equal(73, sut[9]);
    }

    [Fact]
    public void Dump_WhenMemoryHasBeenSet_ReturnsExpectedValues()
    {
      var instructions = new[]
      {
        "mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X",
        "mem[8] = 11",
        "mem[9] = 11",
        "mem[7] = 101",
        "mem[8] = 0"
      };

      var expectedMemory = new[]
      {
        new KeyValuePair<long, long>(7, 101),
        new KeyValuePair<long, long>(8, 64),
        new KeyValuePair<long, long>(9, 73),
      };

      var sut = new MaskableMemory();
      sut.ProcessInstructions(instructions);

      var memDump = sut.DumpMemory();

      Assert.Equal(expectedMemory, memDump);
      var sum = memDump.Select(kvp => kvp.Value).Sum();
      Assert.Equal(238, sum);
    }
  }
}
