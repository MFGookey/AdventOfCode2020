using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace JumpMan.Core.Processor
{
  /// <inheritdoc cref="ICPUState"/>
  public class CPUState : ICPUState
  {
    /// <inheritdoc/>
    public int ProgramCounter
    {
      get; private set;
    }

    /// <inheritdoc/>
    public int Accumulator
    {
      get; private set;
    }

    /// <inheritdoc/>
    public bool Halt
    {
      get; set;
    }

    /// <inheritdoc/>
    public IOperation LastOperation
    {
      get; private set;
    }

    /// <summary>
    /// Copies a new CPU state based on an existing CPU state
    /// </summary>
    /// <param name="cpuState">The CPU state to copy</param>
    public CPUState(ICPUState cpuState)
    {
      ProgramCounter = cpuState.ProgramCounter;
      Accumulator = cpuState.Accumulator;
      Halt = cpuState.Halt;
      LastOperation = cpuState.LastOperation;
    }

    /// <inheritdoc/>
    public bool Equals([AllowNull] ICPUState other)
    {
      if (other == null)
      {
        return false;
      }

      return this.ProgramCounter == other.ProgramCounter
        && this.Accumulator == other.Accumulator
        && this.Halt == other.Halt
        && (
          this.LastOperation == null && other.LastOperation == null
          || this.LastOperation.Equals(other.LastOperation)
        );
    }
  }
}
