using System;
using System.Linq;
using Common.Utilities.IO;
using MaskUp.Core;

namespace MaskUp.Cmd
{
  /// <summary>
  /// The entrypoint for MaskUp.Cmd
  /// </summary>
  public class Program
  {
    /// <summary>
    /// MaskUp.Cmd entry point
    /// </summary>
    /// <param name="args">Command line arguments (not used)</param>
    static void Main(string[] args)
    {
      var filePath = "./input";
      var reader = new FileReader();
      var instructions = reader.ReadFileByLines(filePath);
      var memory = new MaskableMemory();
      memory.ProcessInstructions(instructions);
      var memoryDump = memory.DumpMemory();
      var sum = memoryDump.Select(kvp => kvp.Value).Sum();
      Console.WriteLine(sum);
    }
  }
}
