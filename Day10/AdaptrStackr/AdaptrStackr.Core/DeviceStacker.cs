using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdaptrStackr.Core.Device;

namespace AdaptrStackr.Core
{
  public class DeviceStacker : IDeviceStacker
  {
    public int CreateStack(IEnumerable<IDevice> toStack)
    {
      var chargeables = toStack.Where(device => device is ChargeableDevice).Count();

      if (chargeables > 1)
      {
        throw new ArgumentException("toStack may only have at most one ChargeableDevice", nameof(toStack));
      }

      var chargingPorts = toStack.Where(device => device is Socket).Count();

      if (chargingPorts > 1)
      {
        throw new ArgumentException("toStack may have at most one Socket", nameof(toStack));
      }

      if (chargeables == 0)
      {
        // Need to create our own, I guess.
        var toCharge = new ChargeableDevice(toStack.Where(device => device is ISocket).Select(socket => (ISocket)socket));
        toStack = toStack.Append(toCharge);
      }

      if (chargingPorts == 0)
      {
        // Need ot create our own, I guess.
        var chargingPort = new Socket();
        toStack = toStack.Append(chargingPort);
      }

      // Sort the source devices by their output
      var sources = toStack.Where(device => device is ISocket).Select(device => device as ISocket).OrderBy(device => device.Output);

      // Sort the sink devices by their lower input
      var sinks = toStack.Where(device => device is IChargeableDevice).Select(device => device as IChargeableDevice).OrderBy(device => device.LowInput);

      // If all is correct, the elements in sources and sinks OUGHT to be matched such that we can pair them up trivially and all will satisfy IChargeableDevice.CanAttach
      var stack = sources.Zip(sinks, (source, sink) => new { Socket = source, Plug = sink });

      // Sanity check
      if (stack.Where(pair => pair.Plug.CanAttach(pair.Socket) == false).Any())
      {
        throw new ArgumentException("Could not pair up sockets and plugs");
      }

      var threes = stack.Where(pair => pair.Plug.LowInput == pair.Socket.Output).Count();
      var ones = stack.Where(pair => pair.Plug.HighInput == pair.Socket.Output).Count();
      return threes * ones;
    }

    /// <inheritdoc/>
    public long Permute(IEnumerable<ISocket> toPermute, ChargeableDevice toCharge)
    {
      // combine our lists.  Did this to ensure only one chargeable device at the end of all of this.
      var devices = toPermute.Select(adapter => adapter as IDevice).Append(toCharge);
      var chargingPorts = devices.Where(device => device is Socket).Count();

      if (chargingPorts > 1)
      {
        throw new ArgumentException("toPermute may have at most one Socket", nameof(toPermute));
      }
      
      if (chargingPorts == 0)
      {
        // Need to create our own, I guess.
        var chargingPort = new Socket();
        devices = devices.Append(chargingPort);
      }

      var socketsWithPlugs = devices
        .Where(device => device is ISocket)
        .Select(device => (ISocket)device)
        .OrderBy(device => device.Output)
        .Select(
          socket => new
          {
            socketVoltage = socket.Output,
            plugs = devices
              .Where(device => device is IChargeableDevice)
              .Select(device => ((IChargeableDevice)device))
              .Where(device => device.CanAttach(socket))
              .OrderByDescending(device => device.LowInput)
          }
        );

      var waysToGetThere = new Dictionary<int, long>();
      // creating a running list of how many ways it takes to get to the goal LowInput from each previous spot
      waysToGetThere.Add(toCharge.HighInput + 1, 1);

      foreach (var socketPlug in socketsWithPlugs.OrderByDescending(sp => sp.socketVoltage))
      {
        long waysThereFromHere = 0;
        foreach (var plug in socketPlug.plugs)
        {
          waysThereFromHere += waysToGetThere[plug.HighInput + 1]; // fake out the output
        }
        waysToGetThere.Add(socketPlug.socketVoltage, waysThereFromHere);
      }

      return waysToGetThere.Values.Max();
    }
  }
}
