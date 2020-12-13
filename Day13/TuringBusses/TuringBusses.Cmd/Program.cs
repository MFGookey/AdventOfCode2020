using System;
using System.Linq;
using Common.Utilities.Formatter;
using Common.Utilities.IO;
using TuringBusses.Core;

//using TuringBusses.Core;

namespace TuringBusses.Cmd
{
  /// <summary>
  /// The entrypoint for TuringBusses.Cmd
  /// </summary>
  public class Program
  {
    /// <summary>
    /// TuringBusses.Cmd entry point
    /// </summary>
    /// <param name="args">Command line arguments (not used)</param>
    static void Main(string[] args)
    {
      var filePath = "./input";
      var reader = new FileReader();
      var formatter = new RecordFormatter(reader);
      var records = formatter.FormatFile(filePath, "\n", true);

      var timeStamp = int.Parse(records.First());

      var scheduler = new BusScheduler(formatter);

      var product = scheduler.GetNextBusProduct(timeStamp, records.Skip(1).First());

      Console.WriteLine(product);
    }
  }
}
