using System;
using Common.Utilities.IO;
using Common.Utilities.Formatter;
using PapersPlease.Core;
using System.Linq;

namespace PapersPlease.Cmd
{
  /// <summary>
  /// The entrypoint for PapersPlease.Cmd
  /// </summary>
  public class Program
  {
    /// <summary>
    /// PapersPlease.Cmd entry point
    /// </summary>
    /// <param name="args">Command line arguments (not used)</param>
    static void Main(string[] args)
    {
      var filePath = "./input";
      var reader = new FileReader();
      var formatter = new RecordFormatter(new FileReader());
      var fileString = reader.ReadFile(filePath).Replace("\r", string.Empty);
      var groupStrings = formatter
        .FormatRecord(
          fileString,
          "\n\n",
          true
        );
      var groups = formatter.FormatSubRecords(groupStrings, "\n", true)
        .Select(
          record => new PassengerGroup(record)
        );

      Console.WriteLine(groups.Select(group => group.DistinctAnswerCount).Sum());

      Console.WriteLine(groups.Select(group => group.AllSameAnswerCount).Sum());
    }
  }
}
