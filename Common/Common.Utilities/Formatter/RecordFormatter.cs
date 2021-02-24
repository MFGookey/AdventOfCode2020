using System;
using System.Collections.Generic;
using System.Linq;
using Common.Utilities.IO;
using System.Text.RegularExpressions;

namespace Common.Utilities.Formatter
{
  public class RecordFormatter : IRecordFormatter
  {
    private IFileReader _fileReader;

    /// <summary>
    /// Initializes a new default instance of the RecordFormatter class without an IFileReader
    /// </summary>
    public RecordFormatter() : this(null) { }

    /// <summary>
    /// Initializes a new instance of the RecordFormatter class with a given IFileReader
    /// </summary>
    /// <param name="fileReader">The file reader to use for file IO</param>
    public RecordFormatter(IFileReader fileReader)
    {
      _fileReader = fileReader;
    }

    /// <inheritdoc/>
    public IEnumerable<string> FormatFile(string filePath, string recordDelimiter, bool removeBlankRecords)
    {
      return FormatFile(filePath, recordDelimiter, removeBlankRecords, false);
    }

    /// <inheritdoc/>
    public IEnumerable<string> FormatFile(string filePath, string recordDelimiter, bool removeBlankRecords, bool normalizeLineEndings)
    {
      var fileContent = ReadFile(filePath);
      return FormatRecord(fileContent, recordDelimiter, removeBlankRecords, normalizeLineEndings);
    }

    /// <inheritdoc/>
    public IList<Match> FormatFileWithRegex(string filePath, string pattern)
    {
      return FormatFileWithRegex(filePath, pattern, RegexOptions.None);
    }

    /// <inheritdoc/>
    public IList<Match> FormatFileWithRegex(string filePath, string pattern, RegexOptions options)
    {
      var fileContent = ReadFile(filePath);
      return FormatRecordWithRegex(fileContent, pattern, RegexOptions.None);
    }



    /// <inheritdoc/>
    public IEnumerable<string> FormatRecord(string records, string recordDelimiter, bool removeBlankRecords)
    {
      return FormatRecord(records, recordDelimiter, removeBlankRecords, false);
    }

    /// <inheritdoc/>
    public IEnumerable<string> FormatRecord(string records, string recordDelimiter, bool removeBlankRecords, bool normalizeLineEndings)
    {
      if (normalizeLineEndings)
      {
        records = NormalizeLineEndings(records);
      }

      return records.Split(recordDelimiter, removeBlankRecords ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None).Where(r => { return removeBlankRecords ? string.IsNullOrWhiteSpace(r) == false : true; });
    }

    /// <inheritdoc/>
    public IList<Match> FormatRecordWithRegex(string records, string pattern)
    {
      return FormatRecordWithRegex(records, pattern, RegexOptions.None);
    }

    /// <inheritdoc/>
    public IList<Match> FormatRecordWithRegex(string records, string pattern, RegexOptions options)
    {
      records = NormalizeLineEndings(records);
      return Regex.Matches(records, pattern, options);
    }

    /// <inheritdoc/>
    public IEnumerable<IEnumerable<string>> FormatSubRecords(IEnumerable<string> records, string subRecordDelimiter, bool removeBlankRecords)
    {
      return FormatSubRecords(records, subRecordDelimiter, removeBlankRecords, false);
    }

    /// <inheritdoc/>
    public IEnumerable<IEnumerable<string>> FormatSubRecords(IEnumerable<string> records, string subRecordDelimiter, bool removeBlankRecords, bool normalizeLineEndings)
    {
      return records.Where(r => { return removeBlankRecords ? string.IsNullOrWhiteSpace(r) == false : true; }).Select(r => FormatRecord(r, subRecordDelimiter, removeBlankRecords, normalizeLineEndings));
    }

    /// <summary>
    /// Convert all line endings to a particular style
    /// </summary>
    /// <param name="toNormalize">The string to normalize</param>
    /// <param name="normalizeTo">The line ending to normalize to, default /n</param>
    /// <returns></returns>
    private string NormalizeLineEndings(string toNormalize, string normalizeTo = "\n")
    {
      return Regex.Replace(toNormalize, "\r\n|\r|\n\r|\n", normalizeTo);
    }

    /// <summary>
    /// Read a file given a filepath
    /// </summary>
    /// <param name="filePath">The filepath to read</param>
    /// <returns>The string contents of the file</returns>
    private string ReadFile(string filePath)
    {
      return _fileReader.ReadFile(filePath) ?? throw new NullReferenceException("No File Reader has been provided");
    }
  }
}
