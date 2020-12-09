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

    /// <inheritdoc/>
    public IEnumerable<StringBag> FindContents(IEnumerable<StringBag> rules, string bagToFind)
    {
      // Find the rules for the contextual bag
      var rulesForCurrentBag = rules.Where(bag => bag.BagColor.Equals(bagToFind));

      // Find the contents of the bags contained in the rules
      var containedBags = rulesForCurrentBag
        .SelectMany(
          bag => FindContents(rules, bag.ContainedBag)
        );

      return rulesForCurrentBag.Union(containedBags).Distinct();
    }

    /// <inheritdoc/>
    public int FindSumOfContents(
      IEnumerable<StringBag> rules,
      string bagToFind
    )
    {
      // Find the bags contained by the current bag to find
      // Call FindSumOfContents for all of those bags then multiply by the ContainedQuantity in the current bag
      // Then add 1 to the sum of all of those.

      var rulesForCurrentBag = rules.Where(bag => bag.BagColor.Equals(bagToFind));

      var containedBags = rulesForCurrentBag
        .Select(
          bag => FindSumOfContents(rules, bag.ContainedBag) * bag.ContainedQuantity
        );

      return containedBags.Sum() + 1;
    }
  }
}
