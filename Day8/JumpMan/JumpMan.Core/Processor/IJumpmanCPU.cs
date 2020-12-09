using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace JumpMan.Core.Processor
{
  /// <summary>
  /// A CPU implementation
  /// </summary>
  public interface IJumpmanCPU : ICPUState
  {
    /// <summary>
    /// Gets the program currently loaded on the CPU
    /// </summary>
    ImmutableArray<IOperation> Program
    {
      get;
    }

    /// <summary>
    /// Gets a trace of all of the commands that have been run
    /// </summary>
    IEnumerable<ICPUState> Trace
    {
      get;
    }

    /// <summary>
    /// Gets a flag indicating whether or not the program terminated normally.
    /// </summary>
    bool TerminatedNormally
    {
      get;
    }

    /// <summary>
    /// Run the loaded program the IOperations represent until it halts
    /// </summary>
    void RunProgram();

    /// <summary>
    /// Add a rule under which the program will halt
    /// </summary>
    /// <param name="rule">A function that inspects the current state of the CPU and returns whether or not the program should halt</param>
    void AddHaltingRule(Func<IJumpmanCPU, bool> rule);

    /// <summary>
    /// Reset the program counter, accumulator, trace, halt, and last operation
    /// </summary>
    void Reset();

    /// <summary>
    /// Loads a new program into the cpu and resets
    /// </summary>
    /// <param name="newProgram">The new program to load</param>
    void LoadNewProgram(IEnumerable<IOperation> newProgram);
  }
}
