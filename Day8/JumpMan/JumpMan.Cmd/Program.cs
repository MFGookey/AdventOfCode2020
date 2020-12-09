using System;
using Common.Utilities.IO;
using JumpMan.Core;
using JumpMan.Core.Processor;
using System.Linq;

namespace JumpMan.Cmd
{
  /// <summary>
  /// The entrypoint for JumpMan.Cmd
  /// </summary>
  public class Program
  {
    /// <summary>
    /// JumpMan.Cmd entry point
    /// </summary>
    /// <param name="args">Command line arguments (not used)</param>
    static void Main(string[] args)
    {
      var filePath = "./input";
      var reader = new FileReader();
      var program = reader
        .ReadFileByLines(filePath)
        .Select(operation => new Operation(operation));
      var cpu = new JumpmanCPU(program);
      cpu.AddHaltingRule(
        (cpu) => cpu
              .Trace
              .Select(trace => trace.ProgramCounter)
              .Where(counter => counter == cpu.ProgramCounter)
              .Count()
              > 0
        );

      cpu.RunProgram();

      Console.WriteLine(cpu.Accumulator);

      // We can patch the program by editing one instruction from a nop to a jump or vice versa.
      // The program ends normaly by having the ProgramCounter exceed the length of the program.
      // So detect when that happens.
      var arrayProgram = program.ToArray<IOperation>();
      IOperation oldOperation;
      for (var i = 0; i < arrayProgram.Length; i++)
      {
        if (arrayProgram[i].OpCode == OpCode.jmp || arrayProgram[i].OpCode == OpCode.nop)
        {
          oldOperation = arrayProgram[i];
          var sign = oldOperation.Argument >= 0 ? "+" : string.Empty;
          if (arrayProgram[i].OpCode == OpCode.jmp)
          {
            arrayProgram[i] = new Operation(string.Format("nop {0}{1}", sign, oldOperation.Argument));
          }
          else
          {
            arrayProgram[i] = new Operation(string.Format("jmp {0}{1}", sign, oldOperation.Argument));
          }

          cpu.LoadNewProgram(arrayProgram);
          cpu.RunProgram();
          if (cpu.TerminatedNormally)
          {
            // we did it!
            break;
          }
          else
          {
            arrayProgram[i] = oldOperation;
          }
        }
      }

      Console.WriteLine(cpu.Accumulator);
    }
  }
}
