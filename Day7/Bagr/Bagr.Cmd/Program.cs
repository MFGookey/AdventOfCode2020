using System;
using Common.Utilities.IO;
using Common.Utilities.Formatter;
using Bagr.Core.RuleParsing;
using System.Linq;

namespace Bagr.Cmd
{
  /// <summary>
  /// The entrypoint for Bagr.Cmd
  /// </summary>
  public class Program
  {
    /// <summary>
    /// Bagr.Cmd entry point
    /// </summary>
    /// <param name="args">Command line arguments (not used)</param>
    static void Main(string[] args)
    {
      var filePath = "./input";
      var reader = new FileReader();
      // Normalize line endings.  Bake this into the fileReader class soon.
      var contents = reader.ReadFile(filePath).Replace("\r\n", "\n");

      var formatter = new RecordFormatter(null);

      var ruleStrings = formatter.FormatRecord(contents, "\n", true);

      var parser = new RuleParser();

      var parsedRules = parser.ParseRules(ruleStrings);

      var bagColors = parser.FindBagColorsContainingBag(parsedRules, "shiny gold");

      Console.WriteLine(bagColors.Count());
    }
  }
}
