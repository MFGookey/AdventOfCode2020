using System;
using System.Collections.Generic;
using System.Text;
using Bagr.Core.RuleParsing;
using Xunit;
using System.Linq;

namespace Bagr.Core.Tests.RuleParsing
{
  public class RuleParserTests
  {
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("light red bags contains 1 bright white bag, 2 muted yellow bags.")]
    [InlineData("light red bags contain 1 bright white bagsn, 2 muted yellow bags.")]
    [InlineData("light red bags contain 1 bright white bags, 2 muted yellow bagsn.")]
    [InlineData("light red bags contain 1 bright white bags,.")]
    [InlineData("bags contain")]
    [InlineData("asdfawsdf")]
    [InlineData(".......")]
    public void ParseRule_GivenInvalidRule_ThrowsException(string rule)
    {
      var sut = new RuleParser();
      Assert.Throws<ArgumentException>(() => sut.ParseRule(rule));
    }

    [Theory]
    [MemberData(nameof(BagRules))]
    public void ParseRule_GivenValidRule_ReturnsExpectedValues(
      string rule,
      IEnumerable<StringBag> expectedResults
    )
    {
      var sut = new RuleParser();

      var results = sut.ParseRule(rule);

      Assert.Equal(
        expectedResults
          .OrderBy(bag => bag.BagColor)
          .ThenBy(bag => bag.ContainedBag),
        results
          .OrderBy(bag => bag.BagColor)
          .ThenBy(bag => bag.ContainedBag)
        );
    }

    [Theory]
    [MemberData(nameof(MultiBagRules))]
    public void ParseRules_GivenValidRules_ReturnsExpectedValues(
      IEnumerable<string> rules,
      IEnumerable<StringBag> expectedResults
      )
    {
      var sut = new RuleParser();

      var results = sut.ParseRules(rules);

      Assert.Equal(
        expectedResults
          .OrderBy(bag => bag.BagColor)
          .ThenBy(bag => bag.ContainedBag),
        results
          .OrderBy(bag => bag.BagColor)
          .ThenBy(bag => bag.ContainedBag)
      );
    }

    [Theory]
    [MemberData(nameof(FindBags))]
    public void FindBagsContainingBag(
      IEnumerable<StringBag> rules,
      string bagToFind,
      IEnumerable<StringBag> expectedResults
    )
    {
      var sut = new RuleParser();
      var results = sut.FindBagsContainingBag(rules, bagToFind);

      Assert.Equal(
        expectedResults
          .OrderBy(bag => bag.BagColor)
          .ThenBy(bag => bag.ContainedBag),
        results
          .OrderBy(bag => bag.BagColor)
          .ThenBy(bag => bag.ContainedBag)
      );
    }

    [Theory]
    [MemberData(nameof(FindBagColors))]
    public void FindBagsColorsContainingBag(
      IEnumerable<StringBag> rules,
      string bagToFind,
      IEnumerable<string> expectedResults
    )
    {
      var sut = new RuleParser();
      var results = sut.FindBagColorsContainingBag(rules, bagToFind);

      Assert.Equal(
        expectedResults
          .OrderBy(bag => bag),
        results
          .OrderBy(bag => bag)
      );
    }

    public static IEnumerable<object[]> BagRules
    {
      get
      {
        yield return new object[]
        {
          "light red bags contain 1 bright white bag, 2 muted yellow bags.",
          new StringBag[]
          {
            new StringBag{
              BagColor = "light red",
              ContainedBag = "bright white",
              ContainedQuantity = 1
            },

            new StringBag{
              BagColor = "light red",
              ContainedBag = "muted yellow",
              ContainedQuantity = 2
            }
          }
        };

        yield return new object[]
        {
          "dark orange bags contain 3 bright white bags, 4 muted yellow bags.",
          new StringBag[]
          {
            new StringBag{
              BagColor = "dark orange",
              ContainedBag = "bright white",
              ContainedQuantity = 3
            },

            new StringBag{
              BagColor = "dark orange",
              ContainedBag = "muted yellow",
              ContainedQuantity = 4
            }
          }
        };

        yield return new object[]
        {
          "bright white bags contain 1 shiny gold bag.",
          new StringBag[]
          {
            new StringBag{
              BagColor = "bright white",
              ContainedBag = "shiny gold",
              ContainedQuantity = 1
            }
          }
        };

        yield return new object[]
        {
          "muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.",
          new StringBag[]
          {
            new StringBag{
              BagColor = "muted yellow",
              ContainedBag = "shiny gold",
              ContainedQuantity = 2
            },

            new StringBag{
              BagColor = "muted yellow",
              ContainedBag = "faded blue",
              ContainedQuantity = 9
            }
          }
        };

        yield return new object[]
        {
          "shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.",
          new StringBag[]
          {
            new StringBag{
              BagColor = "shiny gold",
              ContainedBag = "dark olive",
              ContainedQuantity = 1
            },

            new StringBag{
              BagColor = "shiny gold",
              ContainedBag = "vibrant plum",
              ContainedQuantity = 2
            }
          }
        };

        yield return new object[]
        {
          "dark olive bags contain 3 faded blue bags, 4 dotted black bags.",
          new StringBag[]
          {
            new StringBag{
              BagColor = "dark olive",
              ContainedBag = "faded blue",
              ContainedQuantity = 3
            },

            new StringBag{
              BagColor = "dark olive",
              ContainedBag = "dotted black",
              ContainedQuantity = 4
            }
          }
        };

        yield return new object[]
        {
          "vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.",
          new StringBag[]
          {
            new StringBag{
              BagColor = "vibrant plum",
              ContainedBag = "faded blue",
              ContainedQuantity = 5
            },

            new StringBag{
              BagColor = "vibrant plum",
              ContainedBag = "dotted black",
              ContainedQuantity = 6
            }
          }
        };

        yield return new object[]
        {
          "faded blue bags contain no other bags.",
          new StringBag[] { }
        };

        yield return new object[]
        {
          "dotted black bags contain no other bags.",
          new StringBag[] { }
        };
      }
    }

    public static IEnumerable<object[]> MultiBagRules
    {
      get
      {
        yield return new object[]
        {
          new string[]
          {
            "dotted black bags contain no other bags."
          },

          new StringBag[] { }
        };

        yield return new object[]
        {
          new string[]
          {
            "faded blue bags contain no other bags.",
            "dotted black bags contain no other bags."
          },

          new StringBag[] { }
        };

        yield return new object[]
        {
          new string[]
          {
            "vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.",
            "faded blue bags contain no other bags.",
            "dotted black bags contain no other bags."
          },

          new StringBag[] {
            new StringBag{
              BagColor = "vibrant plum",
              ContainedBag = "faded blue",
              ContainedQuantity = 5
            },

            new StringBag{
              BagColor = "vibrant plum",
              ContainedBag = "dotted black",
              ContainedQuantity = 6
            }
          }
        };

        yield return new object[]
        {
          new string[]
          {
            "dark olive bags contain 3 faded blue bags, 4 dotted black bags.",
            "vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.",
            "faded blue bags contain no other bags.",
            "dotted black bags contain no other bags."
          },

          new StringBag[] {

            new StringBag{
              BagColor = "vibrant plum",
              ContainedBag = "faded blue",
              ContainedQuantity = 5
            },

            new StringBag{
              BagColor = "vibrant plum",
              ContainedBag = "dotted black",
              ContainedQuantity = 6
            },

            new StringBag{
              BagColor = "dark olive",
              ContainedBag = "faded blue",
              ContainedQuantity = 3
            },

            new StringBag{
              BagColor = "dark olive",
              ContainedBag = "dotted black",
              ContainedQuantity = 4
            }
          }
        };

        yield return new object[]
        {
          new string[]
          {
            "shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.",
            "dark olive bags contain 3 faded blue bags, 4 dotted black bags.",
            "vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.",
            "faded blue bags contain no other bags.",
            "dotted black bags contain no other bags."
          },

          new StringBag[] {

            new StringBag{
              BagColor = "vibrant plum",
              ContainedBag = "faded blue",
              ContainedQuantity = 5
            },

            new StringBag{
              BagColor = "vibrant plum",
              ContainedBag = "dotted black",
              ContainedQuantity = 6
            },

            new StringBag{
              BagColor = "dark olive",
              ContainedBag = "faded blue",
              ContainedQuantity = 3
            },

            new StringBag{
              BagColor = "dark olive",
              ContainedBag = "dotted black",
              ContainedQuantity = 4
            },

            new StringBag{
              BagColor = "shiny gold",
              ContainedBag = "dark olive",
              ContainedQuantity = 1
            },

            new StringBag{
              BagColor = "shiny gold",
              ContainedBag = "vibrant plum",
              ContainedQuantity = 2
            }
          }
        };

        yield return new object[]
        {
          new string[]
          {
            "muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.",
            "shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.",
            "dark olive bags contain 3 faded blue bags, 4 dotted black bags.",
            "vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.",
            "faded blue bags contain no other bags.",
            "dotted black bags contain no other bags."
          },

          new StringBag[] {

            new StringBag{
              BagColor = "vibrant plum",
              ContainedBag = "faded blue",
              ContainedQuantity = 5
            },

            new StringBag{
              BagColor = "vibrant plum",
              ContainedBag = "dotted black",
              ContainedQuantity = 6
            },

            new StringBag{
              BagColor = "dark olive",
              ContainedBag = "faded blue",
              ContainedQuantity = 3
            },

            new StringBag{
              BagColor = "dark olive",
              ContainedBag = "dotted black",
              ContainedQuantity = 4
            },

            new StringBag{
              BagColor = "shiny gold",
              ContainedBag = "dark olive",
              ContainedQuantity = 1
            },

            new StringBag{
              BagColor = "shiny gold",
              ContainedBag = "vibrant plum",
              ContainedQuantity = 2
            },

            new StringBag{
              BagColor = "muted yellow",
              ContainedBag = "shiny gold",
              ContainedQuantity = 2
            },

            new StringBag{
              BagColor = "muted yellow",
              ContainedBag = "faded blue",
              ContainedQuantity = 9
            }
          }
        };

        yield return new object[]
        {
          new string[]
          {
            "bright white bags contain 1 shiny gold bag.",
            "muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.",
            "shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.",
            "dark olive bags contain 3 faded blue bags, 4 dotted black bags.",
            "vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.",
            "faded blue bags contain no other bags.",
            "dotted black bags contain no other bags."
          },

          new StringBag[] {

            new StringBag{
              BagColor = "vibrant plum",
              ContainedBag = "faded blue",
              ContainedQuantity = 5
            },

            new StringBag{
              BagColor = "vibrant plum",
              ContainedBag = "dotted black",
              ContainedQuantity = 6
            },

            new StringBag{
              BagColor = "dark olive",
              ContainedBag = "faded blue",
              ContainedQuantity = 3
            },

            new StringBag{
              BagColor = "dark olive",
              ContainedBag = "dotted black",
              ContainedQuantity = 4
            },

            new StringBag{
              BagColor = "shiny gold",
              ContainedBag = "dark olive",
              ContainedQuantity = 1
            },

            new StringBag{
              BagColor = "shiny gold",
              ContainedBag = "vibrant plum",
              ContainedQuantity = 2
            },

            new StringBag{
              BagColor = "muted yellow",
              ContainedBag = "shiny gold",
              ContainedQuantity = 2
            },

            new StringBag{
              BagColor = "muted yellow",
              ContainedBag = "faded blue",
              ContainedQuantity = 9
            },

            new StringBag{
              BagColor = "bright white",
              ContainedBag = "shiny gold",
              ContainedQuantity = 1
            }
          }
        };

        yield return new object[]
        {
          new string[]
          {
            "dark orange bags contain 3 bright white bags, 4 muted yellow bags.",
            "bright white bags contain 1 shiny gold bag.",
            "muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.",
            "shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.",
            "dark olive bags contain 3 faded blue bags, 4 dotted black bags.",
            "vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.",
            "faded blue bags contain no other bags.",
            "dotted black bags contain no other bags."
          },

          new StringBag[] {

            new StringBag{
              BagColor = "vibrant plum",
              ContainedBag = "faded blue",
              ContainedQuantity = 5
            },

            new StringBag{
              BagColor = "vibrant plum",
              ContainedBag = "dotted black",
              ContainedQuantity = 6
            },

            new StringBag{
              BagColor = "dark olive",
              ContainedBag = "faded blue",
              ContainedQuantity = 3
            },

            new StringBag{
              BagColor = "dark olive",
              ContainedBag = "dotted black",
              ContainedQuantity = 4
            },

            new StringBag{
              BagColor = "shiny gold",
              ContainedBag = "dark olive",
              ContainedQuantity = 1
            },

            new StringBag{
              BagColor = "shiny gold",
              ContainedBag = "vibrant plum",
              ContainedQuantity = 2
            },

            new StringBag{
              BagColor = "muted yellow",
              ContainedBag = "shiny gold",
              ContainedQuantity = 2
            },

            new StringBag{
              BagColor = "muted yellow",
              ContainedBag = "faded blue",
              ContainedQuantity = 9
            },

            new StringBag{
              BagColor = "bright white",
              ContainedBag = "shiny gold",
              ContainedQuantity = 1
            },

            new StringBag{
              BagColor = "dark orange",
              ContainedBag = "bright white",
              ContainedQuantity = 3
            },

            new StringBag{
              BagColor = "dark orange",
              ContainedBag = "muted yellow",
              ContainedQuantity = 4
            }
          }
        };

        yield return new object[]
        {
          new string[]
          {
            "light red bags contain 1 bright white bag, 2 muted yellow bags.",
            "dark orange bags contain 3 bright white bags, 4 muted yellow bags.",
            "bright white bags contain 1 shiny gold bag.",
            "muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.",
            "shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.",
            "dark olive bags contain 3 faded blue bags, 4 dotted black bags.",
            "vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.",
            "faded blue bags contain no other bags.",
            "dotted black bags contain no other bags."
          },

          new StringBag[] {

            new StringBag{
              BagColor = "vibrant plum",
              ContainedBag = "faded blue",
              ContainedQuantity = 5
            },

            new StringBag{
              BagColor = "vibrant plum",
              ContainedBag = "dotted black",
              ContainedQuantity = 6
            },

            new StringBag{
              BagColor = "dark olive",
              ContainedBag = "faded blue",
              ContainedQuantity = 3
            },

            new StringBag{
              BagColor = "dark olive",
              ContainedBag = "dotted black",
              ContainedQuantity = 4
            },

            new StringBag{
              BagColor = "shiny gold",
              ContainedBag = "dark olive",
              ContainedQuantity = 1
            },

            new StringBag{
              BagColor = "shiny gold",
              ContainedBag = "vibrant plum",
              ContainedQuantity = 2
            },

            new StringBag{
              BagColor = "muted yellow",
              ContainedBag = "shiny gold",
              ContainedQuantity = 2
            },

            new StringBag{
              BagColor = "muted yellow",
              ContainedBag = "faded blue",
              ContainedQuantity = 9
            },

            new StringBag{
              BagColor = "bright white",
              ContainedBag = "shiny gold",
              ContainedQuantity = 1
            },

            new StringBag{
              BagColor = "dark orange",
              ContainedBag = "bright white",
              ContainedQuantity = 3
            },

            new StringBag{
              BagColor = "dark orange",
              ContainedBag = "muted yellow",
              ContainedQuantity = 4
            },

            new StringBag{
              BagColor = "light red",
              ContainedBag = "bright white",
              ContainedQuantity = 1
            },

            new StringBag{
              BagColor = "light red",
              ContainedBag = "muted yellow",
              ContainedQuantity = 2
            }
          }
        };
      }
    }

    public static IEnumerable<object[]> FindBags
    {
      get
      {
        yield return new object[]
        {
          new StringBag[] {
            new StringBag{
              BagColor = "vibrant plum",
              ContainedBag = "faded blue",
              ContainedQuantity = 5
            },

            new StringBag{
              BagColor = "vibrant plum",
              ContainedBag = "dotted black",
              ContainedQuantity = 6
            },

            new StringBag{
              BagColor = "dark olive",
              ContainedBag = "faded blue",
              ContainedQuantity = 3
            },

            new StringBag{
              BagColor = "dark olive",
              ContainedBag = "dotted black",
              ContainedQuantity = 4
            },

            new StringBag{
              BagColor = "muted yellow",
              ContainedBag = "faded blue",
              ContainedQuantity = 9
            },

            new StringBag{
              BagColor = "muted yellow",
              ContainedBag = "shiny gold",
              ContainedQuantity = 2
            }
          },
          "faded blue",
          new StringBag[]
          {
            new StringBag{
              BagColor = "muted yellow",
              ContainedBag = "faded blue",
              ContainedQuantity = 9
            },

            new StringBag{
              BagColor = "dark olive",
              ContainedBag = "faded blue",
              ContainedQuantity = 3
            },

            new StringBag{
              BagColor = "vibrant plum",
              ContainedBag = "faded blue",
              ContainedQuantity = 5
            }
          }
        };

        yield return new object[]
        {
          new StringBag[] {
            new StringBag{
              BagColor = "vibrant plum",
              ContainedBag = "faded blue",
              ContainedQuantity = 5
            },

            new StringBag{
              BagColor = "vibrant plum",
              ContainedBag = "dotted black",
              ContainedQuantity = 6
            },

            new StringBag{
              BagColor = "dark olive",
              ContainedBag = "faded blue",
              ContainedQuantity = 3
            },

            new StringBag{
              BagColor = "dark olive",
              ContainedBag = "dotted black",
              ContainedQuantity = 4
            },

            new StringBag{
              BagColor = "muted yellow",
              ContainedBag = "faded blue",
              ContainedQuantity = 9
            },

            new StringBag{
              BagColor = "shiny gold",
              ContainedBag = "dark olive",
              ContainedQuantity = 1
            },

            new StringBag{
              BagColor = "shiny gold",
              ContainedBag = "vibrant plum",
              ContainedQuantity = 2
            }
          },
          "faded blue",
          new StringBag[]
          {
            new StringBag{
              BagColor = "muted yellow",
              ContainedBag = "faded blue",
              ContainedQuantity = 9
            },

            new StringBag{
              BagColor = "dark olive",
              ContainedBag = "faded blue",
              ContainedQuantity = 3
            },

            new StringBag{
              BagColor = "vibrant plum",
              ContainedBag = "faded blue",
              ContainedQuantity = 5
            },

            new StringBag{
              BagColor = "shiny gold",
              ContainedBag = "dark olive",
              ContainedQuantity = 1
            },

            new StringBag{
              BagColor = "shiny gold",
              ContainedBag = "vibrant plum",
              ContainedQuantity = 2
            }
          }
        };

        yield return new object[]
        {
          new StringBag[] {

            new StringBag{
              BagColor = "vibrant plum",
              ContainedBag = "faded blue",
              ContainedQuantity = 5
            },

            new StringBag{
              BagColor = "vibrant plum",
              ContainedBag = "dotted black",
              ContainedQuantity = 6
            },

            new StringBag{
              BagColor = "dark olive",
              ContainedBag = "faded blue",
              ContainedQuantity = 3
            },

            new StringBag{
              BagColor = "dark olive",
              ContainedBag = "dotted black",
              ContainedQuantity = 4
            },

            new StringBag{
              BagColor = "shiny gold",
              ContainedBag = "dark olive",
              ContainedQuantity = 1
            },

            new StringBag{
              BagColor = "shiny gold",
              ContainedBag = "vibrant plum",
              ContainedQuantity = 2
            },

            new StringBag{
              BagColor = "muted yellow",
              ContainedBag = "shiny gold",
              ContainedQuantity = 2
            },

            new StringBag{
              BagColor = "muted yellow",
              ContainedBag = "faded blue",
              ContainedQuantity = 9
            },

            new StringBag{
              BagColor = "bright white",
              ContainedBag = "shiny gold",
              ContainedQuantity = 1
            },

            new StringBag{
              BagColor = "dark orange",
              ContainedBag = "bright white",
              ContainedQuantity = 3
            },

            new StringBag{
              BagColor = "dark orange",
              ContainedBag = "muted yellow",
              ContainedQuantity = 4
            },

            new StringBag{
              BagColor = "light red",
              ContainedBag = "bright white",
              ContainedQuantity = 1
            },

            new StringBag{
              BagColor = "light red",
              ContainedBag = "muted yellow",
              ContainedQuantity = 2
            }
          },
          "shiny gold",
          new StringBag[]
          {
            new StringBag{
              BagColor = "bright white",
              ContainedBag = "shiny gold",
              ContainedQuantity = 1
            },

            new StringBag{
              BagColor = "muted yellow",
              ContainedBag = "shiny gold",
              ContainedQuantity = 2
            },

            new StringBag{
              BagColor = "dark orange",
              ContainedBag = "bright white",
              ContainedQuantity = 3
            },

            new StringBag{
              BagColor = "dark orange",
              ContainedBag = "muted yellow",
              ContainedQuantity = 4
            },

            new StringBag{
              BagColor = "light red",
              ContainedBag = "bright white",
              ContainedQuantity = 1
            },

            new StringBag{
              BagColor = "light red",
              ContainedBag = "muted yellow",
              ContainedQuantity = 2
            }
          }
        };
      }
    }

    public static IEnumerable<object[]> FindBagColors
    {
      get
      {
        yield return new object[]
        {
          new StringBag[] {
            new StringBag{
              BagColor = "vibrant plum",
              ContainedBag = "faded blue",
              ContainedQuantity = 5
            },

            new StringBag{
              BagColor = "vibrant plum",
              ContainedBag = "dotted black",
              ContainedQuantity = 6
            },

            new StringBag{
              BagColor = "dark olive",
              ContainedBag = "faded blue",
              ContainedQuantity = 3
            },

            new StringBag{
              BagColor = "dark olive",
              ContainedBag = "dotted black",
              ContainedQuantity = 4
            },

            new StringBag{
              BagColor = "muted yellow",
              ContainedBag = "faded blue",
              ContainedQuantity = 9
            },

            new StringBag{
              BagColor = "muted yellow",
              ContainedBag = "shiny gold",
              ContainedQuantity = 2
            }
          },
          "faded blue",
          new string[]
          {
            "vibrant plum",
            "dark olive",
            "muted yellow"
          }
        };

        yield return new object[]
        {
          new StringBag[] {
            new StringBag{
              BagColor = "vibrant plum",
              ContainedBag = "faded blue",
              ContainedQuantity = 5
            },

            new StringBag{
              BagColor = "vibrant plum",
              ContainedBag = "dotted black",
              ContainedQuantity = 6
            },

            new StringBag{
              BagColor = "dark olive",
              ContainedBag = "faded blue",
              ContainedQuantity = 3
            },

            new StringBag{
              BagColor = "dark olive",
              ContainedBag = "dotted black",
              ContainedQuantity = 4
            },

            new StringBag{
              BagColor = "muted yellow",
              ContainedBag = "faded blue",
              ContainedQuantity = 9
            },

            new StringBag{
              BagColor = "shiny gold",
              ContainedBag = "dark olive",
              ContainedQuantity = 1
            },

            new StringBag{
              BagColor = "shiny gold",
              ContainedBag = "vibrant plum",
              ContainedQuantity = 2
            }
          },
          "faded blue",
          new string[]
          {
            "muted yellow",
            "dark olive",
            "vibrant plum",
            "shiny gold"
          }
        };

        yield return new object[]
        {
          new StringBag[] {

            new StringBag{
              BagColor = "vibrant plum",
              ContainedBag = "faded blue",
              ContainedQuantity = 5
            },

            new StringBag{
              BagColor = "vibrant plum",
              ContainedBag = "dotted black",
              ContainedQuantity = 6
            },

            new StringBag{
              BagColor = "dark olive",
              ContainedBag = "faded blue",
              ContainedQuantity = 3
            },

            new StringBag{
              BagColor = "dark olive",
              ContainedBag = "dotted black",
              ContainedQuantity = 4
            },

            new StringBag{
              BagColor = "shiny gold",
              ContainedBag = "dark olive",
              ContainedQuantity = 1
            },

            new StringBag{
              BagColor = "shiny gold",
              ContainedBag = "vibrant plum",
              ContainedQuantity = 2
            },

            new StringBag{
              BagColor = "muted yellow",
              ContainedBag = "shiny gold",
              ContainedQuantity = 2
            },

            new StringBag{
              BagColor = "muted yellow",
              ContainedBag = "faded blue",
              ContainedQuantity = 9
            },

            new StringBag{
              BagColor = "bright white",
              ContainedBag = "shiny gold",
              ContainedQuantity = 1
            },

            new StringBag{
              BagColor = "dark orange",
              ContainedBag = "bright white",
              ContainedQuantity = 3
            },

            new StringBag{
              BagColor = "dark orange",
              ContainedBag = "muted yellow",
              ContainedQuantity = 4
            },

            new StringBag{
              BagColor = "light red",
              ContainedBag = "bright white",
              ContainedQuantity = 1
            },

            new StringBag{
              BagColor = "light red",
              ContainedBag = "muted yellow",
              ContainedQuantity = 2
            }
          },
          "shiny gold",
          new string[]
          {
            "bright white",
            "muted yellow",
            "dark orange",
            "light red"
          }
        };
      }
    }
  }
}
