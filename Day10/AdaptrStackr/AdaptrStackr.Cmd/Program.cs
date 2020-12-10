using System;
using Common.Utilities.IO;
using AdaptrStackr.Core;
using AdaptrStackr.Core.Device;
using System.Linq;

namespace AdaptrStackr.Cmd
{
  /// <summary>
  /// The entrypoint for AdaptrStackr.Cmd
  /// </summary>
  public class Program
  {
    /// <summary>
    /// AdaptrStackr.Cmd entry point
    /// </summary>
    /// <param name="args">Command line arguments (not used)</param>
    static void Main(string[] args)
    {
      var filePath = "./input";
      var reader = new FileReader();
      var inputs = reader.ReadFileByLines(filePath);

      if (inputs.Where(input => int.TryParse(input, out var _) == false).Any())
      {
        throw new FormatException("Could not parse the input data.");
      }

      var adapters = inputs.Select(input => new Adapter(int.Parse(input)));
      var stacker = new DeviceStacker();
      var result = stacker.CreateStack(adapters);
      Console.WriteLine(result);
    }
  }
}
