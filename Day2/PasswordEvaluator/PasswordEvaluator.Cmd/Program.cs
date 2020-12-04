using PasswordEvaluator.Core.Database;
using PasswordEvaluator.Core.Policy;
using PasswordEvaluator.Core.Policy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Common.Utilities.IO;

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

      passwordDB = new Database(new List<IPassword>(), new FileReader(), new CharacterPositionPasswordPolicyFactory());
      passwordDB.Import(@"C:\Dev\GitHub\AdventOfCode2020\Day2\Data\input");
      goodPasswords = passwordDB.Passwords.Where(p => p.IsValid).Count();
      Console.WriteLine(goodPasswords);
    }
  }
}
