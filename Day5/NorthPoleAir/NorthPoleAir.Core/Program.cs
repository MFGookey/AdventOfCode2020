using System;
using Common.Utilities.IO;

namespace NorthPoleAir.Cmd
{
  public class Program
  {
    static void Main(string[] args)
    {
      var filePath = "../../../../../Data/input";
      var reader = new FileReader();
      Console.WriteLine(reader.ReadFile(filePath).Length);
      Console.WriteLine("Hello World!");
    }
  }
}
