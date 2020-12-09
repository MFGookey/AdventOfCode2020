namespace JumpMan.Core
{
  /// <summary>
  /// The supported OpCodes for the JumpMan vCPU
  /// </summary>
  public enum OpCode
  {
    /// <summary>
    /// Represents the nop opcode, where nothing is done and the argument is ignored
    /// </summary>
    nop = 0,

    /// <summary>
    /// Represents the acc opcode, where the value in the opcode argument is added or subtracted from the accumulator
    /// </summary>
    acc = 1,

    /// <summary>
    /// Represents the jmp opcode, where the value in the argument is added or subtracted from the program counter
    /// </summary>
    jmp = 2,

    /// <summary>
    /// Represents a debug opcode, used for testing
    /// </summary>
    debug = 3
  }
}
