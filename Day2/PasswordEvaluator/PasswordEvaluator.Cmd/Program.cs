using PasswordEvaluator.Core.Database;
using System;
using System.Linq;

namespace PasswordEvaluator.Cmd
{
  class Program
  {
    static void Main(string[] args)
    {
      var passwordDB = new Database();
      passwordDB.Import(@"C:\Dev\GitHub\AdventOfCode2020\Day2\Data\input");
      var goodPasswords = passwordDB.Passwords.Where(p => p.IsValid).Count();
      Console.WriteLine(goodPasswords);
    }
  }
}
