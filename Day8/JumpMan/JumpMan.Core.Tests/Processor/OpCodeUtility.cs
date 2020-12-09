using System;
using System.Collections.Generic;
using System.Text;
using Moq;

namespace JumpMan.Core.Tests.Processor
{
  public static class OpCodeUtility
  {
    public static IOperation MockOperation(OpCode opcode, int argument)
    {
      var operation = new Mock<IOperation>();
      operation.SetupGet(op => op.OpCode).Returns(opcode);
      operation.SetupGet(op => op.Argument).Returns(argument);
      operation
        .Setup(operation => operation.Equals(It.IsAny<IOperation>()))
        .Returns(
          (IOperation other) =>
          {
            return other != null
              && other.OpCode == opcode;
          });

      return operation.Object;
    }
  }
}
