using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using Common.Utilities.Formatter;

namespace TuringBusses.Core
{
  public class BusScheduler
  {
    private IRecordFormatter _formatter;

    public BusScheduler(IRecordFormatter formatter)
    {
      _formatter = formatter;
    }

    /// <summary>
    /// Get the product of the id of the bus you need to catch, and how long you will wait for it.
    /// </summary>
    /// <param name="timeStamp">The soonest you can get onto a bus</param>
    /// <param name="busses">A comma delimited list of bus IDs</param>
    /// <returns>The product of the bus ID and how many minutes you must wait.</returns>
    public int GetNextBusProduct(int timeStamp, string busses)
    {
      return _formatter.FormatRecord(busses, ",", true)
        .Where(bus => int.TryParse(bus, out _))
        .Select(
          bus =>
          {
            var busId = int.Parse(bus);
            var modulo = timeStamp % busId;
            var wait = busId - modulo;
            return new
            {
              product = wait * busId,
              wait
            };
          })
        .OrderBy(result => result.wait)
        .First()
        .product;
    }

    public int WinContest(string busses)
    {
      throw new NotImplementedException();
    }
  }
}
