using System;
using System.Collections.Generic;
using System.Text;
using JumpMan.Core.Processor;
using Moq;

namespace JumpMan.Core.Tests.Processor
{
  public static class CPUStateUtility
  {
    public static ICPUState MockCPUState(int programCounter, int accumulator, bool halt, OpCode lastOpCode, int lastArgument)
    {
      var lastOp = OpCodeUtility.MockOperation(lastOpCode, lastArgument);
      return MockCPUState(programCounter, accumulator, halt, lastOp);
    }

    public static ICPUState MockCPUState(int programCounter, int accumulator, bool halt, IOperation lastOperation)
    {
      var state = new Mock<ICPUState>();
      state.SetupGet(state => state.ProgramCounter).Returns(programCounter);
      state.SetupGet(state => state.Accumulator).Returns(accumulator);
      state.SetupGet(state => state.Halt).Returns(halt);
      state.SetupGet(state => state.LastOperation).Returns(lastOperation);
      state
        .Setup(state => state.Equals(It.IsAny<ICPUState>()))
        .Returns(
          (ICPUState other) =>
          {
            if (other == null)
            {
              return false;
            }

            return programCounter == other.ProgramCounter
              && accumulator == other.Accumulator
              && halt == other.Halt;
          }
        );

      return state.Object;
    }
  }
}
