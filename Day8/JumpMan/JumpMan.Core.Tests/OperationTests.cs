using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using JumpMan.Core;

namespace JumpMan.Core.Tests
{
  public class OperationTests
  {
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("sad asd")]
    [InlineData("asd !42")]
    [InlineData("asd 42")]
    public void Constructor_GivenInvalidOperationFormat_ThrowsException(string operation)
    {
      var exception = Assert.Throws<ArgumentException>(() => new Operation(operation));
      Assert.Contains("{opcode} +\\-{argument}", exception.Message);
    }

    [Theory]
    [InlineData("asd +42")]
    [InlineData("jmmp -42")]
    public void Constructor_GivenInvalidOpcode_ThrowsException(string operation)
    {
      var exception = Assert.Throws<ArgumentException>(() => new Operation(operation));
      Assert.Contains("\" is not a valid OpCode", exception.Message);
    }

    [Theory]
    [InlineData("nop +0", OpCode.nop, 0)]
    [InlineData("acc +1", OpCode.acc, 1)]
    [InlineData("jmp +4", OpCode.jmp, 4)]
    [InlineData("acc +3", OpCode.acc, 3)]
    [InlineData("jmp -3", OpCode.jmp, -3)]
    [InlineData("acc -99", OpCode.acc, -99)]
    [InlineData("jmp -4", OpCode.jmp, -4)]
    [InlineData("acc +6", OpCode.acc, 6)]
    [InlineData("debug +42", OpCode.debug, 42)]
    public void Constructor_GivenValidOperation_SetsValuesAsExpected(string operation, OpCode expectedOpCode, int expectedArgument)
    {
      var sut = new Operation(operation);
      Assert.Equal(expectedOpCode, sut.OpCode);
      Assert.Equal(expectedArgument, sut.Argument);
    }
  }
}
