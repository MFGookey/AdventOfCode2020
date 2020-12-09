using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using JumpMan.Core.Processor;
using System.Linq;

namespace JumpMan.Core.Tests.Processor
{
  public class JumpmanCPUTests
  {
    [Fact]
    public void Constructor_GivenProgramWithInvalidOpcode_ThrowsException()
    {
      var program = new IOperation[]
      {
        OpCodeUtility.MockOperation(OpCode.debug, 0)
      };

      var exception = Assert.Throws<ArgumentException>(() => new JumpmanCPU(program));
      Assert.StartsWith("The Opcodes: \"debug\" are not supported.", exception.Message);
    }

    [Theory]
    [MemberData(nameof(ValidPrograms))]
    public void Constructor_GivenValidProgram_SetsValuesAsExpected(IEnumerable<IOperation> program)
    {
      var sut = new JumpmanCPU(program);
      Assert.Equal(program, sut.Program);
      Assert.Equal(0, sut.ProgramCounter);
      Assert.Equal(0, sut.Accumulator);
      Assert.False(sut.Halt);
      Assert.Null(sut.LastOperation);
      Assert.Empty(sut.Trace);
    }

    [Theory]
    [MemberData(nameof(HaltingRules))]
    public void AddHaltingRule_GivenValidRule_HaltsInExpectedState(
      Func<IJumpmanCPU, bool> rule,
      IEnumerable<IOperation> program,
      int expectedProgramCounter,
      int expectedAccumulator,
      bool expectedHalt,
      IOperation expectedLastOperation,
      IEnumerable<ICPUState> expectedTrace,
      bool expectedTermination
    )
    {
      var sut = new JumpmanCPU(program);

      Assert.Equal(program, sut.Program);

      sut.AddHaltingRule(rule);
      sut.RunProgram();

      Assert.Equal(expectedProgramCounter, sut.ProgramCounter);
      Assert.Equal(expectedAccumulator, sut.Accumulator);
      Assert.Equal(expectedHalt, sut.Halt);
      Assert.Equal(expectedLastOperation, sut.LastOperation);
      Assert.Equal(expectedTermination, sut.TerminatedNormally);
      Assert.Equal(expectedTrace.Count(), sut.Trace.Count());

      for (var i = 0; i < expectedTrace.Count(); i++)
      {
        Assert.Equal(expectedTrace.Skip(i).First(), sut.Trace.Skip(i).First());
      }
    }

    [Fact]
    public void LoadNewProgram_GivenInvalidProgram_ThrowsException()
    {
      var sut = new JumpmanCPU(new IOperation[] { });

      var program = new IOperation[]
      {
        OpCodeUtility.MockOperation(OpCode.debug, 0)
      };

      var exception = Assert.Throws<ArgumentException>(() => sut.LoadNewProgram(program));
      Assert.StartsWith("The Opcodes: \"debug\" are not supported.", exception.Message);
    }

    [Theory]
    [MemberData(nameof(ValidPrograms))]
    public void LoadNewProgram_GivenValidProgram_SetsProgramAsExpected(IEnumerable<IOperation> program)
    {
      var sut = new JumpmanCPU(new IOperation[] { });

      sut.LoadNewProgram(program);

      Assert.Equal(program, sut.Program);
    }

    public static IEnumerable<object[]> ValidPrograms
    {
      get
      {
        yield return new object[]
        {
          new IOperation[]{ }
        };

        yield return new object[]
        {
          new IOperation[]{
            OpCodeUtility.MockOperation(OpCode.nop, 0)
          }
        };

        yield return new object[]
        {
          new IOperation[]{
            OpCodeUtility.MockOperation(OpCode.jmp, 0)
          }
        };

        yield return new object[]
        {
          new IOperation[]{
            OpCodeUtility.MockOperation(OpCode.acc, 0)
          }
        };

        yield return new object[]
        {
          new IOperation[]{
            OpCodeUtility.MockOperation(OpCode.nop, 0),
            OpCodeUtility.MockOperation(OpCode.acc, 1),
            OpCodeUtility.MockOperation(OpCode.jmp, 4),
            OpCodeUtility.MockOperation(OpCode.acc, 3),
            OpCodeUtility.MockOperation(OpCode.jmp, -3),
            OpCodeUtility.MockOperation(OpCode.acc, -99),
            OpCodeUtility.MockOperation(OpCode.acc, 1),
            OpCodeUtility.MockOperation(OpCode.jmp, -4),
            OpCodeUtility.MockOperation(OpCode.acc, 6)
          }
        };
      }
    }

    public static IEnumerable<object[]> HaltingRules
    {
      get
      {
        yield return new object[]
        {
          new Func<IJumpmanCPU, bool>((cpu) => false),
          new IOperation[] { },
          0,
          0,
          true,
          null,
          new ICPUState[]
          {
            CPUStateUtility.MockCPUState(0, 0, false, null),
            CPUStateUtility.MockCPUState(0, 0, true, null)
          },
          true
        };

        yield return new object[]
        {
          new Func<IJumpmanCPU, bool>((cpu) => true),
          new IOperation[]
          {
            OpCodeUtility.MockOperation(OpCode.nop, 0)
          },
          1,
          0,
          true,
          OpCodeUtility.MockOperation(OpCode.nop, 0),
          new ICPUState[]
          {
            CPUStateUtility.MockCPUState(0, 0, false, null),
            CPUStateUtility.MockCPUState(1, 0, true, OpCodeUtility.MockOperation(OpCode.nop, 0))
          },
          false
        };

        yield return new object[]
        {
          new Func<IJumpmanCPU, bool>((cpu) => true),
          new IOperation[]
          {
            OpCodeUtility.MockOperation(OpCode.acc, 42)
          },
          1,
          42,
          true,
          OpCodeUtility.MockOperation(OpCode.acc, 42),
          new ICPUState[]
          {
            CPUStateUtility.MockCPUState(0, 0, false, null),
            CPUStateUtility.MockCPUState(1, 42, true, OpCodeUtility.MockOperation(OpCode.acc, 42))
          },
          false
        };

        yield return new object[]
        {
          new Func<IJumpmanCPU, bool>((cpu) => true),
          new IOperation[]
          {
            OpCodeUtility.MockOperation(OpCode.jmp, -120)
          },
          -120,
          0,
          true,
          OpCodeUtility.MockOperation(OpCode.jmp, -120),
          new ICPUState[]
          {
            CPUStateUtility.MockCPUState(0, 0, false, null),
            CPUStateUtility.MockCPUState(-120, 0, true, OpCodeUtility.MockOperation(OpCode.jmp, -120))
          },
          false
        };

        yield return new object[]
        {
          new Func<IJumpmanCPU, bool>((cpu) => false),
          new IOperation[]
          {
            OpCodeUtility.MockOperation(OpCode.jmp, -120)
          },
          -120,
          0,
          true,
          null,
          new ICPUState[]
          {
            CPUStateUtility.MockCPUState(0, 0, false, null),
            CPUStateUtility.MockCPUState(-120, 0, false, OpCodeUtility.MockOperation(OpCode.jmp, -120)),
            CPUStateUtility.MockCPUState(-120, 0, true, null)
          },
          true
        };

        yield return new object[]
        {
          new Func<IJumpmanCPU, bool>((cpu) => {
            return cpu
              .Trace
              .Select(trace => trace.ProgramCounter)
              .Where(counter => counter == cpu.ProgramCounter)
              .Count()
              > 0;
          }),
          new IOperation[]
          {
            OpCodeUtility.MockOperation(OpCode.nop, 0),
            OpCodeUtility.MockOperation(OpCode.acc, 1),
            OpCodeUtility.MockOperation(OpCode.jmp, 4),
            OpCodeUtility.MockOperation(OpCode.acc, 3),
            OpCodeUtility.MockOperation(OpCode.jmp, -3),
            OpCodeUtility.MockOperation(OpCode.acc, -99),
            OpCodeUtility.MockOperation(OpCode.acc, 1),
            OpCodeUtility.MockOperation(OpCode.jmp, -4),
            OpCodeUtility.MockOperation(OpCode.acc, 6)
          },
          1,
          5,
          true,
          OpCodeUtility.MockOperation(OpCode.jmp, -3),
          new ICPUState[]
          {
            CPUStateUtility.MockCPUState(0, 0, false, null),
            CPUStateUtility.MockCPUState(1, 0, false, OpCode.nop, 0),
            CPUStateUtility.MockCPUState(2, 1, false, OpCode.acc, 1),
            CPUStateUtility.MockCPUState(6, 1, false, OpCode.jmp, 3),
            CPUStateUtility.MockCPUState(7, 2, false, OpCode.acc, 1),
            CPUStateUtility.MockCPUState(3, 2, false, OpCode.jmp, -4),
            CPUStateUtility.MockCPUState(4, 5, false, OpCode.acc, 3),
            CPUStateUtility.MockCPUState(1, 5, true, OpCode.jmp, -3)
          },
          false
        };
      }
    }
  }
}
