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

    /// <summary>
    /// Given a comma delimited string of busses, find a timestamp on the bus schedule where: each bus leaves in order in its sequence one after the next.  Some busses are out of service and only take up a time slot.
    /// </summary>
    /// <param name="busses">A comma delimited string of busIDs for busses in service, and x for busses not in service</param>
    /// <returns>The timestamp when the sequence begins.</returns>
    public long WinContest(string busses)
    {
      var busIDs = _formatter.FormatRecord(busses, ",", true);
      return FindEarliestTimestamp(busIDs);
    }

    long FindEarliestTimestamp(IEnumerable<string> schedule)
    {
      var processedBusIDs = schedule.Select(
                  (
                    busId,
                    busScheduleIndex) =>
                  {
                    int? nullableId = null;
                    if (int.TryParse(busId, out int parsedId))
                    {
                      nullableId = parsedId;
                    }

                    return new
                    {
                      index = busScheduleIndex,
                      busId = nullableId
                    };
                  }
                  )
                  .Where(record => record.busId.HasValue)
                  .ToDictionary(record => (long)record.busId.Value, record => (long)(record.busId.Value - record.index) % (long)record.busId.Value);
      return ChineseRemainderTheorem.Solve(processedBusIDs.Keys, processedBusIDs.Values);
    }
  }
}
