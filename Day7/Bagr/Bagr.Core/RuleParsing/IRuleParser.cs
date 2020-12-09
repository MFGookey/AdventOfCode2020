using System;
using System.Collections.Generic;
using System.Text;

namespace Bagr.Core.RuleParsing
{
  /// <summary>
  /// Given a string[] of rules, parse those rules and return a list of bag-containedBag-containedBagQuantity
  /// </summary>
  public interface IRuleParser
  {
    /// <summary>
    /// Parse a set of rules into a set of stingbags
    /// </summary>
    /// <param name="rules">The rules to parse</param>
    /// <returns>The stringbags that represent the parsed rules</returns>
    IEnumerable<StringBag> ParseRules(IEnumerable<string> rules);

    /// <summary>
    /// Parse a rule into a set of stingbags
    /// </summary>
    /// <param name="rule">The rule to parse</param>
    /// <returns>The stringbags that represent the parsed rule</returns>
    IEnumerable<StringBag> ParseRule(string rule);

    /// <summary>
    /// Given a list of bag rules, a bag to find, and a minimum depth: find all bags that could contain the bag to find at least as deep as the minimumLevelsDeep
    /// </summary>
    /// <param name="bagRules">The rules to use in finding the bag</param>
    /// <param name="bagToFind">The bag to find</param>
    /// <returns>A list of bags that could contain the bagToFind at least minimum levels deep</returns>
    IEnumerable<StringBag> FindBagsContainingBag(IEnumerable<StringBag> bagRules, string bagToFind);

    /// <summary>
    /// Get a list of just the bagColors that could contain a particular bag
    /// </summary>
    /// <param name="rules">The rules to use in finding the bag</param>
    /// <param name="bagToFind">The bag to find</param>
    /// <returns>A list of distinct bag colors that could contain the bagToFind at least minimum levels deep</returns>
    IEnumerable<string> FindBagColorsContainingBag(IEnumerable<StringBag> rules, string bagToFind);
  }
}
