using System;
using Common.Utilities.Formatter;
using Common.Utilities.IO;
using PassportControl.Core.Model;
using System.Linq;

namespace PassportControl.Cmd
{
  public class Program
  {
    static void Main(string[] args)
    {
      // Doing a thing.
      var formatter = new RecordFormatter(new FileReader());
      var records = formatter.FormatFile(@"C:\Dev\GitHub\AdventOfCode2020\Day4\Data\input", "\n\n", true).Select(s => new NorthPoleCredential(s));

      var validRecords = records.Where(r => r.Valid).Count();

      Console.WriteLine(validRecords);

      var extendedValidRecords = records.Where(r => r.ExtendedValidation).Count();

      Console.WriteLine(extendedValidRecords);
    }
  }
}
