using System;
using Common.Utilities.IO;
//using ChristmasCracker.Core;

namespace ChristmasCracker.Cmd
{
  /// <summary>
  /// The entrypoint for ChristmasCracker.Cmd
  /// </summary>
  public class Program
  {
    /// <summary>
    /// ChristmasCracker.Cmd entry point
    /// </summary>
    /// <param name="args">Command line arguments (not used)</param>
    static void Main(string[] args)
    {
      var filePath = "./input";
      var reader = new FileReader();
      Console.WriteLine(reader.ReadFile(filePath).Length);
      Console.WriteLine("Hello World!");
    }
  }
}
