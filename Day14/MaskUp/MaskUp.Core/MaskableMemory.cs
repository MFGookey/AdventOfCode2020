using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MaskUp.Core
{
  /// <summary>
  /// Represents an addressable memory store, where a mask can be pre-applied to writes to the memory
  /// </summary>
  public class MaskableMemory
  {
    
    private readonly Dictionary<long, long> _internalStore;

    private Dictionary<int, bool> _mask;

    /// <summary>
    /// Gets or sets the length of the words this memory stores.
    /// </summary>
    private int WordLength
    {
      get; set;
    }

    /// <summary>
    /// Gets the current bitmask in use
    /// </summary>
    public string Mask
    {
      get {
        return string.Join("", Enumerable.Range(0, WordLength).OrderByDescending(x => x).Select(x => _mask.Keys.Contains(x) ? _mask[x] ? '1' : '0' : 'X'));
      }
    }

    /// <summary>
    /// Directly get and set memory addresses
    /// </summary>
    /// <param name="index">The memory address to get or set</param>
    /// <returns>The value at the given address</returns>
    public long this[long index]
    {
      get
      {
        return _internalStore[index];
      }

      set
      {
        SetMemoryLocation(_mask, _internalStore, index, value);
      }
    }

    /// <summary>
    /// Given a mask, a dictionary representing memory, an index, and a value, set the relevant memory addresses
    /// </summary>
    /// <param name="mask">The mask to apply to the inputs</param>
    /// <param name="memory">The memory container</param>
    /// <param name="index">The index in memory to attempt to set</param>
    /// <param name="value">The value to attempt to set</param>
    private void SetMemoryLocation(Dictionary<int, bool> mask, Dictionary<long, long> memory, long index, long value)
    {
      memory[index] = ApplyMask(mask, value);
    }

    /// <summary>
    /// Apply the current mask to a given value
    /// </summary>
    /// <param name="value">The value to which the mask must be applied</param>
    /// <returns>A new value based on the given value with a mask applied</returns>
    private long ApplyMask(Dictionary<int, bool> mask, long value)
    {
      var valArray = new BitArray(BitConverter.GetBytes(value));
      return valArray.Cast<bool>().Select((bit, index) =>
      {
        // if the condensed mask has our index in it, force the bit to match the mask's
        if (mask.Keys.Contains(index))
        {
          return mask[index];
        }
        return bit;
      })
      .Select((bit, index) => bit ? (long)Math.Pow(2, index) : 0)
      .Sum();
    }

    /// <summary>
    /// Initializes a new instance of the MaskableMemory class
    /// </summary>
    public MaskableMemory()
    {
      _internalStore = new Dictionary<long, long>();
      _mask = new Dictionary<int, bool>();
      WordLength = 36;

      SetMask(string.Empty.PadLeft(WordLength, 'X'));
    }

    /// <summary>
    /// Set the mask for writing to the memory store
    /// </summary>
    /// <param name="mask">The mask to set.</param>
    public void SetMask(string mask)
    {
      if (Regex.IsMatch(mask ?? string.Empty, $"^[01X]{{{WordLength}}}$"))
      {
        var paddedMask = mask.PadLeft(64, '0');
        _mask = paddedMask
          .ToCharArray()
          .Select(
            (maskChar, index) =>
            new {
              maskChar,
              bitNo = paddedMask.Length - index - 1 
            }
          )
          .Where(charInfo => charInfo.maskChar != 'X')
          .ToDictionary(
            charInfo => charInfo.bitNo,
            charInfo => charInfo.maskChar == '1'
          );
      }
      else
      {
        throw new FormatException($"Mask is {WordLength} characters long of 0, 1, and X");
      }
    }

    /// <summary>
    /// Given a list of instructions to set masks and store memory, do so
    /// </summary>
    /// <param name="instructions">The instructions to execute</param>
    public void ProcessInstructions(string[] instructions)
    {
      var maskRegex = new Regex($"^mask\\s=\\s([X10]{{{WordLength}}})$", RegexOptions.Multiline | RegexOptions.Compiled);

      var memoryRegex = new Regex("^mem\\[(\\d+)\\]\\s=\\s(\\d+)$", RegexOptions.Multiline | RegexOptions.Compiled);

      Match match;

      foreach (var instruction in instructions)
      {
        match = maskRegex.Match(instruction);
        if (match.Success)
        {
          SetMask(match.Groups[1].Value);
        }
        else
        {
          match = memoryRegex.Match(instruction);
          if (match.Success)
          {
            this[long.Parse(match.Groups[1].Value)] = long.Parse(match.Groups[2].Value);
          }
          else
          {
            throw new FormatException($"Could not parse the instruction \"{instruction}\"");
          }
        }
      }
    }

    /// <summary>
    /// Dump the contents of the memory that has been touched to an enumerable of addresses and their values
    /// </summary>
    /// <returns>An enumerable of memory address keys with values</returns>
    public IEnumerable<KeyValuePair<long, long>> DumpMemory()
    {
      return _internalStore.Select(kvp => kvp).OrderBy(kvp => kvp.Key);
    }
  }
}
