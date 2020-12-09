using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace JumpMan.Core.Processor
{
  /// <inheritdoc cref="IJumpmanCPU" />
  public class JumpmanCPU : IJumpmanCPU
  {
    private IList<IOperation> _program;

    /// <inheritdoc/>
    public ImmutableArray<IOperation> Program
    {
      get
      {
        return _program.ToImmutableArray();
      }
    }

    private IList<ICPUState> _trace;

    /// <inheritdoc/>
    public IEnumerable<ICPUState> Trace
    {
      get {
        return _trace;
      }
    }

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

    private IList<Func<IJumpmanCPU, bool>> _haltingRules;

    /// <inheritdoc/>
    public IOperation LastOperation
    {
      get; private set;
    }

    /// <inheritdoc/>
    public bool TerminatedNormally
    {
      get;
      private set;
    }

    private Dictionary<OpCode, Action<IOperation>> _supportedOperations;

    /// <summary>
    /// Initializes a new instance of the JumpmanCPU with a given program to run
    /// </summary>
    /// <param name="program">The program to run</param>
    public JumpmanCPU(IEnumerable<IOperation> program)
    {
      _haltingRules = new List<Func<IJumpmanCPU, bool>>();

      _supportedOperations = new Dictionary<OpCode, Action<IOperation>>
      {
        {
          OpCode.nop,
          (operation) => IncrementProgramCounter()
        },
        { 
          OpCode.acc,
          (operation) => {
            Accumulator += operation.Argument;
            IncrementProgramCounter();
          }
        },
        {
          OpCode.jmp,
          (operation) => SetProgramCounter(ProgramCounter + operation.Argument)
        }
      };


      ValidateProgram(program);

      _program = program.ToList();

      Reset();
    }

    /// <inheritdoc/>
    public void AddHaltingRule(Func<IJumpmanCPU, bool> rule)
    {
      _haltingRules.Add(rule);
    }

    /// <inheritdoc/>
    public void RunProgram()
    {
      while (false == Halt)
      {
        _trace.Add(new CPUState(this));
        var operation = Fetch();
        Execute(operation);
        LastOperation = operation;
        
        var shouldHalt = _haltingRules
          .Select(rule => rule(this))
          .Where(result => result == true)
          .Count()
          > 0;

        Halt |= shouldHalt;
      }

      // capture the last operation that triggered the halt.
      _trace.Add(new CPUState(this));
    }

    /// <inheritdoc/>
    public void Reset()
    {
      SetProgramCounter(0);
      Accumulator = 0;
      Halt = false;
      LastOperation = null;
      _trace = new List<ICPUState>();
      TerminatedNormally = false;
    }

    /// <inheritdoc/>
    public void LoadNewProgram(IEnumerable<IOperation> newProgram)
    {
      ValidateProgram(newProgram);

      _program = newProgram.ToList();

      Reset();
    }

    private void ValidateProgram(IEnumerable<IOperation> program)
    {
      // If _program contains any OpCodes not found in _supportedOperations, we have a problem and should bail immediately
      var unsupportedOpCodes = program
        .Select((operation) => operation.OpCode)
        .Distinct()
        .Except(_supportedOperations.Keys);

      if (unsupportedOpCodes.Count() > 0)
      {
        var message = string.Format(
          "The Opcodes: \"{0}\" are not supported.",
          string.Join(
            "\", \"",
            unsupportedOpCodes.Select(
              operation => operation.ToString()
            )
          )
        );

        throw new ArgumentException(message, nameof(program));
      }
    }

    /// <summary>
    /// Fetch the operation from the program at the ProgramCounter's index
    /// </summary>
    /// <returns>The operation found at the programCounter's index, or null</returns>
    private IOperation Fetch()
    {
      if (_program.Count > ProgramCounter && ProgramCounter >= 0)
      {
        return _program[ProgramCounter];
      }
      TerminatedNormally = true;
      return null;
    }

    /// <summary>
    /// Given an operation, execute its registered delegate
    /// </summary>
    /// <param name="toExecute">The operation to execute.</param>
    private void Execute(IOperation toExecute)
    {
      // If there is no operation to execute, halt the CPU
      if (toExecute == null)
      {
        Halt = true;
        return;
      }

      // Otherwise call the delegate for the operation
      if (_supportedOperations.ContainsKey(toExecute.OpCode))
      {
        _supportedOperations[toExecute.OpCode](toExecute);
      }
      else
      {
        // If the operation is not registered, throw an exception.
        throw new ArgumentException($"Opcode \"{toExecute.OpCode}\" is not supported!", nameof(toExecute));
      }
    }

    /// <summary>
    /// Increment the program counter by one step
    /// </summary>
    private void IncrementProgramCounter()
    {
      SetProgramCounter(ProgramCounter + 1);
    }

    /// <summary>
    /// Set the program counter to the given value
    /// </summary>
    /// <param name="newValue">The value to which to set the ProgramCounter</param>
    private void SetProgramCounter(int newValue)
    {
      ProgramCounter = newValue;
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
