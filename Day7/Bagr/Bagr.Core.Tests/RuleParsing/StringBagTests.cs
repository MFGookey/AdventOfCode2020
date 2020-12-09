using System;
using System.Collections.Generic;
using System.Text;
using Bagr.Core.RuleParsing;
using Xunit;

namespace Bagr.Core.Tests.RuleParsing
{
  public class StringBagTests
  {
    [Theory]
    [InlineData("invisible pink")]
    [InlineData("purplish green")]
    public void BagColor_WhenSet_IsProperlySet(string expected)
    {
      var sut = new StringBag();
      sut.BagColor = expected;

      Assert.Equal(expected, sut.BagColor);
    }

    [Theory]
    [InlineData("invisible pink")]
    [InlineData("purplish green")]
    public void ContainedBag_WhenSet_IsProperlySet(string expected)
    {
      var sut = new StringBag();
      sut.ContainedBag = expected;

      Assert.Equal(expected, sut.ContainedBag);
    }

    [Theory]
    [InlineData(23)]
    [InlineData(42)]
    public void ContainedQuantity_WhenSet_IsProperlySet(int expected)
    {
      var sut = new StringBag();
      sut.ContainedQuantity = expected;

      Assert.Equal(expected, sut.ContainedQuantity);
    }
  }
}
