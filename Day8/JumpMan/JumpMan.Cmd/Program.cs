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
    }
  }
}
