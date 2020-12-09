using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;

namespace JumpMan.Core
{
  /// <inheritdoc cref="IOperation"/>
  public class Operation : IOperation
  {
    /// <inheritdoc/>
    public OpCode OpCode
    {
      get;
      private set;
    }

    /// <inheritdoc/>
    public int Argument
    {
      get;
      private set;
    }

    /// <summary>
    /// Initializes a new instance of the Operation class
    /// </summary>
    /// <param name="operation">The string to parse into this operation</param>
    public Operation(string operation)
    {
      var match = Regex.Match(operation ?? string.Empty, @"^(\w+)\s([+-]\d+)$");

      if (match.Success == false)
      {
        throw new ArgumentException($"Operation \"{operation ?? "(null)"}\" does not fit the format of " +
          $"\"{{opcode}} +\\-{{argument}}\"", nameof(operation));
      }

      if (Enum.TryParse<OpCode>(match.Groups[1].Value, out var opCode))
      {
        OpCode = opCode;
      }
      else
      {
        throw new ArgumentException($"\"{match.Groups[1].Value}\" is not a valid OpCode", nameof(operation));
      }

      Argument = int.Parse(match.Groups[2].Value);
    }

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
      if (obj == null || (obj is Operation == false))
      {
        return false;
      }

      var otherOperation = (IOperation)obj;
      return Equals(otherOperation);
    }

    /// <inheritdoc/>
    public bool Equals([AllowNull] IOperation other)
    {
      if (other == null)
      {
        return false;
      }

      return this.OpCode == other.OpCode
          && this.Argument == other.Argument;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
      return HashCode.Combine(OpCode, Argument);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
      var sign = (Argument >= 0) ? "+" : string.Empty;

      return $"{OpCode} {sign}{Argument}";
    }
  }
}
