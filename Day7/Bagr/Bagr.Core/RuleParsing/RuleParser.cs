using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bagr.Core.RuleParsing
{
  public class RuleParser : IRuleParser
  {
    /// <inheritdoc />
    public IEnumerable<StringBag> ParseRules(IEnumerable<string> rules)
    {
      return rules.SelectMany(rule => ParseRule(rule));
    }

    /// <inheritdoc />
    public IEnumerable<StringBag> ParseRule(string rule)
    {
      var matches = Regex.Match(rule ?? string.Empty, @"^(.+?)\sbags\scontain\s(?:no other bags|\d+\s[^\.,]+\sbags?(?:,\s\d+[^\.,]+\sbags?)*)\.$");

      if (matches.Success == false)
      {
        throw new ArgumentException($"Bag rule \"{rule ?? "(null)" }\" could not be parsed!", nameof(rule));
      }

      var bagColor = matches.Groups[1].Value;

      return Regex.Matches(rule, @"(?:contain|,)\s(\d+)\s([^,\.]+)\sbags?")
        .Cast<Match>()
        .Select(
          match => new StringBag
          {
            BagColor = bagColor,
            ContainedBag = match.Groups[2].Value,
            ContainedQuantity = int.Parse(match.Groups[1].Value)
          }
        );
    }

    /// <inheritdoc/>
    public IEnumerable<StringBag> FindBagsContainingBag(IEnumerable<StringBag> bagRules, string bagToFind)
    {
      var bagsContainingBagToFind = bagRules
        // Find all bags in the current level that contain a bag we care about
        .Where(bag => bag.ContainedBag.Equals(bagToFind));

      // Then find all bags that could contain those bags
      var bagsContainingThoseBags = bagsContainingBagToFind
        .SelectMany(
          bag => FindBagsContainingBag(bagRules, bag.BagColor)
        );

      return bagsContainingBagToFind.Union(bagsContainingThoseBags);
    }

    /// <inheritdoc/>
    public IEnumerable<string> FindBagColorsContainingBag(IEnumerable<StringBag> rules, string bagToFind)
    {
      return FindBagsContainingBag(rules, bagToFind)
        .Select(bag => bag.BagColor)
        .Distinct();
    }
  }
}
