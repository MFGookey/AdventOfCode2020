using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Utilities.IO
{
  /// <inheritdoc/>
  public class FileReader : IFileReader
  {
    /// <inheritdoc/>
    public string ReadFile(string path)
    {
      return File.ReadAllText(path);
    }

    /// <inheritdoc/>
    public string[] ReadFileByLines(string path)
    {
      return File.ReadAllLines(path);
    }
  }
}
