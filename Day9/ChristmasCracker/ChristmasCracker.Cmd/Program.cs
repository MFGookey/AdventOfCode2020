using System;
using Common.Utilities.IO;
using ChristmasCracker.Core;
using System.Linq;

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
      var inputStrings = reader.ReadFileByLines(filePath);

      if (inputStrings.Where(input => long.TryParse(input, out var _) == false).Any())
      {
        throw new FormatException("Input file must be only integers");
      }

      var inputs = inputStrings.Select(input => long.Parse(input));
      var preambleLength = 25;
      var cracker = new Cracker();

      var results = cracker.FindUnsummableNumbers(preambleLength, 2, inputs);

      var unSummableNumber = results.First();

      Console.WriteLine(unSummableNumber);

      var attackResults = cracker.AttackUnsummableNumber(unSummableNumber, inputs).OrderBy(s => s);
      
      var key = attackResults.First() + attackResults.Last();

      Console.WriteLine(key);
    }
  }
}
