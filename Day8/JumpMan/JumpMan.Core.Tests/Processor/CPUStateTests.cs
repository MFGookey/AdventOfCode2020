using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using JumpMan.Core.Processor;

namespace JumpMan.Core.Tests.Processor
{
  public class CPUStateTests
  {
    [Theory]
    [MemberData(nameof(CPUStates))]
    public void Constructor_GivenValidValues_SetsValuesAsExpected(
      ICPUState copyFrom,
      int expectedProgramCounter,
      int expectedAccumulator,
      bool expectedHalt,
      IOperation expectedLastOperation
    )
    {
      var sut = new CPUState(copyFrom);

      Assert.Equal(expectedProgramCounter, sut.ProgramCounter);
      Assert.Equal(expectedAccumulator, sut.Accumulator);
      Assert.Equal(expectedHalt, sut.Halt);
      Assert.Equal(expectedLastOperation, sut.LastOperation);
    }

    public static IEnumerable<object[]> CPUStates
    {
      get
      {
        yield return new object[]
        {
          CPUStateUtility.MockCPUState(0, 0, false, null),
          0,
          0,
          false,
          null
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(23, 0, false, null),
          23,
          0,
          false,
          null
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(0, 42, false, null),
          0,
          42,
          false,
          null
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(19, -97, false, null),
          19,
          -97,
          false,
          null
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(0, 0, true, null),
          0,
          0,
          true,
          null
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(-23, 0, true, null),
          -23,
          0,
          true,
          null
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(0, -42, true, null),
          0,
          -42,
          true,
          null
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(-19, 97, true, null),
          -19,
          97,
          true,
          null
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(0, 0, false, OpCode.debug, 23),
          0,
          0,
          false,
          OpCodeUtility.MockOperation(OpCode.debug, 23)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(23, 0, false, OpCode.debug, 23),
          23,
          0,
          false,
          OpCodeUtility.MockOperation(OpCode.debug, 23)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(0, 42, false, OpCode.debug, 23),
          0,
          42,
          false,
          OpCodeUtility.MockOperation(OpCode.debug, 23)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(19, -97, false, OpCode.debug, 23),
          19,
          -97,
          false,
          OpCodeUtility.MockOperation(OpCode.debug, 23)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(0, 0, true, OpCode.debug, 23),
          0,
          0,
          true,
          OpCodeUtility.MockOperation(OpCode.debug, 23)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(-23, 0, true, OpCode.debug, 23),
          -23,
          0,
          true,
          OpCodeUtility.MockOperation(OpCode.debug, 23)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(0, -42, true, OpCode.debug, 23),
          0,
          -42,
          true,
          OpCodeUtility.MockOperation(OpCode.debug, 23)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(-19, 97, true, OpCode.debug, 23),
          -19,
          97,
          true,
          OpCodeUtility.MockOperation(OpCode.debug, 23)
        };
        yield return new object[]
      {
          CPUStateUtility.MockCPUState(0, 0, false, OpCode.debug, 23),
          0,
          0,
          false,
          OpCodeUtility.MockOperation(OpCode.debug, 23)
      };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(23, 0, false, OpCode.debug, 23),
          23,
          0,
          false,
          OpCodeUtility.MockOperation(OpCode.debug, 23)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(0, 42, false, OpCode.debug, 23),
          0,
          42,
          false,
          OpCodeUtility.MockOperation(OpCode.debug, 23)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(19, -97, false, OpCode.debug, 23),
          19,
          -97,
          false,
          OpCodeUtility.MockOperation(OpCode.debug, 23)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(0, 0, true, OpCode.debug, 23),
          0,
          0,
          true,
          OpCodeUtility.MockOperation(OpCode.debug, 23)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(-23, 0, true, OpCode.debug, 23),
          -23,
          0,
          true,
          OpCodeUtility.MockOperation(OpCode.debug, 23)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(0, -42, true, OpCode.debug, 23),
          0,
          -42,
          true,
          OpCodeUtility.MockOperation(OpCode.debug, 23)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(-19, 97, true, OpCode.debug, 23),
          -19,
          97,
          true,
          OpCodeUtility.MockOperation(OpCode.debug, 23)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(0, 0, false, OpCode.nop, 42),
          0,
          0,
          false,
          OpCodeUtility.MockOperation(OpCode.nop, 42)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(23, 0, false, OpCode.nop, 42),
          23,
          0,
          false,
          OpCodeUtility.MockOperation(OpCode.nop, 42)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(0, 42, false, OpCode.nop, 42),
          0,
          42,
          false,
          OpCodeUtility.MockOperation(OpCode.nop, 42)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(19, -97, false, OpCode.nop, 42),
          19,
          -97,
          false,
          OpCodeUtility.MockOperation(OpCode.nop, 42)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(0, 0, true, OpCode.nop, 42),
          0,
          0,
          true,
          OpCodeUtility.MockOperation(OpCode.nop, 42)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(-23, 0, true, OpCode.nop, 42),
          -23,
          0,
          true,
          OpCodeUtility.MockOperation(OpCode.nop, 42)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(0, -42, true, OpCode.nop, 42),
          0,
          -42,
          true,
          OpCodeUtility.MockOperation(OpCode.nop, 42)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(-19, 97, true, OpCode.nop, 42),
          -19,
          97,
          true,
          OpCodeUtility.MockOperation(OpCode.nop, 42)
        };
        yield return new object[]
      {
          CPUStateUtility.MockCPUState(0, 0, false, OpCode.nop, 42),
          0,
          0,
          false,
          OpCodeUtility.MockOperation(OpCode.nop, 42)
      };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(23, 0, false, OpCode.nop, 42),
          23,
          0,
          false,
          OpCodeUtility.MockOperation(OpCode.nop, 42)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(0, 42, false, OpCode.nop, 42),
          0,
          42,
          false,
          OpCodeUtility.MockOperation(OpCode.nop, 42)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(19, -97, false, OpCode.nop, 42),
          19,
          -97,
          false,
          OpCodeUtility.MockOperation(OpCode.nop, 42)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(0, 0, true, OpCode.nop, 42),
          0,
          0,
          true,
          OpCodeUtility.MockOperation(OpCode.nop, 42)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(-23, 0, true, OpCode.nop, 42),
          -23,
          0,
          true,
          OpCodeUtility.MockOperation(OpCode.nop, 42)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(0, -42, true, OpCode.nop, 42),
          0,
          -42,
          true,
          OpCodeUtility.MockOperation(OpCode.nop, 42)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(-19, 97, true, OpCode.nop, 42),
          -19,
          97,
          true,
          OpCodeUtility.MockOperation(OpCode.nop, 42)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(0, 0, false, OpCode.acc, -837),
          0,
          0,
          false,
          OpCodeUtility.MockOperation(OpCode.acc, -837)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(23, 0, false, OpCode.acc, -837),
          23,
          0,
          false,
          OpCodeUtility.MockOperation(OpCode.acc, -837)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(0, 42, false, OpCode.acc, -837),
          0,
          42,
          false,
          OpCodeUtility.MockOperation(OpCode.acc, -837)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(19, -97, false, OpCode.acc, -837),
          19,
          -97,
          false,
          OpCodeUtility.MockOperation(OpCode.acc, -837)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(0, 0, true, OpCode.acc, -837),
          0,
          0,
          true,
          OpCodeUtility.MockOperation(OpCode.acc, -837)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(-23, 0, true, OpCode.acc, -837),
          -23,
          0,
          true,
          OpCodeUtility.MockOperation(OpCode.acc, -837)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(0, -42, true, OpCode.acc, -837),
          0,
          -42,
          true,
          OpCodeUtility.MockOperation(OpCode.acc, -837)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(-19, 97, true, OpCode.acc, -837),
          -19,
          97,
          true,
          OpCodeUtility.MockOperation(OpCode.acc, -837)
        };
        yield return new object[]
      {
          CPUStateUtility.MockCPUState(0, 0, false, OpCode.acc, -837),
          0,
          0,
          false,
          OpCodeUtility.MockOperation(OpCode.acc, -837)
      };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(23, 0, false, OpCode.acc, -837),
          23,
          0,
          false,
          OpCodeUtility.MockOperation(OpCode.acc, -837)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(0, 42, false, OpCode.acc, -837),
          0,
          42,
          false,
          OpCodeUtility.MockOperation(OpCode.acc, -837)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(19, -97, false, OpCode.acc, -837),
          19,
          -97,
          false,
          OpCodeUtility.MockOperation(OpCode.acc, -837)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(0, 0, true, OpCode.acc, -837),
          0,
          0,
          true,
          OpCodeUtility.MockOperation(OpCode.acc, -837)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(-23, 0, true, OpCode.acc, -837),
          -23,
          0,
          true,
          OpCodeUtility.MockOperation(OpCode.acc, -837)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(0, -42, true, OpCode.acc, -837),
          0,
          -42,
          true,
          OpCodeUtility.MockOperation(OpCode.acc, -837)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(-19, 97, true, OpCode.acc, -837),
          -19,
          97,
          true,
          OpCodeUtility.MockOperation(OpCode.acc, -837)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(0, 0, false, OpCode.jmp, 74),
          0,
          0,
          false,
          OpCodeUtility.MockOperation(OpCode.jmp, 74)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(23, 0, false, OpCode.jmp, 74),
          23,
          0,
          false,
          OpCodeUtility.MockOperation(OpCode.jmp, 74)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(0, 42, false, OpCode.jmp, 74),
          0,
          42,
          false,
          OpCodeUtility.MockOperation(OpCode.jmp, 74)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(19, -97, false, OpCode.jmp, 74),
          19,
          -97,
          false,
          OpCodeUtility.MockOperation(OpCode.jmp, 74)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(0, 0, true, OpCode.jmp, 74),
          0,
          0,
          true,
          OpCodeUtility.MockOperation(OpCode.jmp, 74)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(-23, 0, true, OpCode.jmp, 74),
          -23,
          0,
          true,
          OpCodeUtility.MockOperation(OpCode.jmp, 74)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(0, -42, true, OpCode.jmp, 74),
          0,
          -42,
          true,
          OpCodeUtility.MockOperation(OpCode.jmp, 74)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(-19, 97, true, OpCode.jmp, 74),
          -19,
          97,
          true,
          OpCodeUtility.MockOperation(OpCode.jmp, 74)
        };
        yield return new object[]
      {
          CPUStateUtility.MockCPUState(0, 0, false, OpCode.jmp, 74),
          0,
          0,
          false,
          OpCodeUtility.MockOperation(OpCode.jmp, 74)
      };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(23, 0, false, OpCode.jmp, 74),
          23,
          0,
          false,
          OpCodeUtility.MockOperation(OpCode.jmp, 74)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(0, 42, false, OpCode.jmp, 74),
          0,
          42,
          false,
          OpCodeUtility.MockOperation(OpCode.jmp, 74)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(19, -97, false, OpCode.jmp, 74),
          19,
          -97,
          false,
          OpCodeUtility.MockOperation(OpCode.jmp, 74)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(0, 0, true, OpCode.jmp, 74),
          0,
          0,
          true,
          OpCodeUtility.MockOperation(OpCode.jmp, 74)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(-23, 0, true, OpCode.jmp, 74),
          -23,
          0,
          true,
          OpCodeUtility.MockOperation(OpCode.jmp, 74)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(0, -42, true, OpCode.jmp, 74),
          0,
          -42,
          true,
          OpCodeUtility.MockOperation(OpCode.jmp, 74)
        };

        yield return new object[]
        {
          CPUStateUtility.MockCPUState(-19, 97, true, OpCode.jmp, 74),
          -19,
          97,
          true,
          OpCodeUtility.MockOperation(OpCode.jmp, 74)
        };
      }
    }
  }
}
