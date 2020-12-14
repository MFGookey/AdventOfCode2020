using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TuringBusses.Core
{
  /// <summary>
  /// Implementation of the Chinese Remainder Theorem retrieved from Rosetta Code
  /// https://rosettacode.org/wiki/Chinese_remainder_theorem#C.23
  /// Modified to use long instead of int
  /// </summary>
  static class ChineseRemainderTheorem
  {
    public static long Solve(IEnumerable<long> n, IEnumerable<long> a)
    {
      long prod = n.Aggregate(1, (long i, long j) => i * j);
      long p;
      long sm = 0;
      for (long i = 0; i < n.Count(); i++)
      {
        p = prod / n.Skip((int)i).First();
        sm += a.Skip((int)i).First() * ModularMultiplicativeInverse(p, n.Skip((int)i).First()) * p;
      }
      return sm % prod;
    }

    private static long ModularMultiplicativeInverse(long a, long mod)
    {
      long b = a % mod;
      for (long x = 1; x < mod; x++)
      {
        if ((b * x) % mod == 1)
        {
          return x;
        }
      }
      return 1;
    }
  }
}
