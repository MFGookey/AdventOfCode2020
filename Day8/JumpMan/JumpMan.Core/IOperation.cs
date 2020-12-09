using System;
using System.Collections.Generic;
using System.Text;

namespace JumpMan.Core
{
  public interface IOperation : IEquatable<IOperation>
  {
    OpCode OpCode
    {
      get;
    }

    public int Argument
    {
      get;
    }
  }
}
