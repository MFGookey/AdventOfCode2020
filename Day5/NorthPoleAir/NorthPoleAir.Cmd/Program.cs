using System;
using Common.Utilities.IO;
using NorthPoleAir;

namespace NorthPoleAir.Cmd
{
  /// <summary>
  /// The entrypoint for NorthPoleAir.Cmd
  /// </summary>
  public class Program
  {
    /// <summary>
    /// NorthPoleAir.Cmd entry point
    /// </summary>
    /// <param name="args">Command line arguments (not used)</param>
    static void Main(string[] args)
    {
      // I want there to be a better way to do this.
      var filePath = "../../../../../Data/input";
      var reader = new FileReader();
      Console.WriteLine(reader.ReadFile(filePath).Length);
      Console.WriteLine("Hello World!");
    }
  }
}
