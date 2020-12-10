using System;
using Common.Utilities.Selector;
using Common.Utilities.IO;
using AdventCalculator.Core.Multiplier;
using System.Collections.Generic;
using System.Linq;

namespace AdventCalculator
{
  /// <summary>
  /// Run the application for the Advent of Code
  /// </summary>
  public class Program
  {
    /// <summary>
    /// The main entry point
    /// </summary>
    /// <param name="args">Not currently used</param>
    static void Main(string[] args)
    {
      var filePath = "./input";
      var reader = new FileReader();
      var inputStrings = reader.ReadFileByLines(filePath);
      if (inputStrings.Select(s => int.TryParse(s, out var _)).Where(b => b == false).Any())
      {
        throw new FormatException("Input must be all integers!");
      }

      var inputs = inputStrings.Select(s => int.Parse(s));

      ISelector selector = new Selector();
      IMultiplier multiplier = new Multiplier();

      // Problem 1, find a pair of numbers that sum to 2020 and multiply them together
      var products = selector.SumFinder(inputs, 2020, 2);
      var result = multiplier.Multiply(products);
      Console.WriteLine(result);

      // Problem 2, find a trio of numbers that sum to 2020 and multiply them together
      products = selector.SumFinder(inputs, 2020, 3);
      result = multiplier.Multiply(products);
      Console.WriteLine(result);
    }
  }
}
