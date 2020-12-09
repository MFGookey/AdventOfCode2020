using System;

namespace JumpMan.Core.Processor
{
  /// <summary>
  /// Represents the state of a CPU after a given IOperation has been run
  /// </summary>
  public interface ICPUState : IEquatable<ICPUState>
  {
    /// <summary>
    /// Gets the value of the ProgramCounter
    /// </summary>
    int ProgramCounter
    {
      get;
    }

    /// <summary>
    /// Gets the value of the Accumulator
    /// </summary>
    int Accumulator
    {
      get;
    }

    /// <summary>
    /// Gets or sets the Halt flag, indicating whether the next command should proceed.
    /// </summary>
    bool Halt
    {
      get; set;
    }

    /// <summary>
    /// Gets the last operation to have run, resulting in the CPU state
    /// </summary>
    IOperation LastOperation
    {
      get;
    }
  }
}