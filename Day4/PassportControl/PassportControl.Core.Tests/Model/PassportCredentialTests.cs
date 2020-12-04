using System;
using System.Collections.Generic;
using System.Text;
using sut = PassportControl.Core.Model;
using Xunit;

namespace PassportControl.Core.Tests.Model
{
  public class PassportCredentialTests
  {
    [Fact]
    public void PassportCredential_GivenValidCredentialString_SetsPropertiesAppropriately()
    {
      var byr = "1937";
      var iyr = "2017";
      var eyr = "2020";
      var hgt = "183cm";
      var hcl = "#fffffd";
      var ecl = "gry";
      var pid = "860033327";
      var cid = "147";
      var sut = new sut.PassportCredential($@"ecl:{ecl} pid:{pid} eyr:{eyr} hcl:{hcl}
byr:{byr} iyr:{iyr} cid:{cid} hgt:{hgt}");

      Assert.Equal(sut.byr, byr);
      Assert.Equal(sut.iyr, iyr);
      Assert.Equal(sut.eyr, eyr);
      Assert.Equal(sut.hgt, hgt);
      Assert.Equal(sut.hcl, hcl);
      Assert.Equal(sut.ecl, ecl);
      Assert.Equal(sut.pid, pid);
      Assert.Equal(sut.cid, cid);
      Assert.True(sut.Valid);
    }

    [Theory]
    [InlineData(@"ecl:gry pid:860033327 eyr:2020 hcl:#fffffd
byr:1937 iyr:2017 cid:147 hgt:183cm", true)]
    [InlineData(@"iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884
hcl:#cfa07d byr:1929", false)]
    [InlineData(@"hcl:#ae17e1 iyr:2013
eyr:2024
ecl:brn pid:760753108 byr:1931
hgt:179cm", false)]
    [InlineData(@"hcl:#cfa07d eyr:2025 pid:166559648
iyr:2011 ecl:brn hgt:59in", false)]
    public void Valid_GivenCredentialString_ReturnsExpectedValue(string credentialString, bool expectedValue)
    {
      var sut = new sut.PassportCredential(credentialString);

      Assert.Equal(expectedValue, sut.Valid);
    }

    [Theory]
    [InlineData(@"ecl:gry pid:860033327 eyr:2020 hcl:#fffffd
byr:1937 iyr:2017 cid:147 hgt:183cm", true, true)]
    [InlineData(@"iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884
hcl:#cfa07d byr:1929", false, false)]
    [InlineData(@"hcl:#ae17e1 iyr:2013
eyr:2024
ecl:brn pid:760753108 byr:1931
hgt:179cm", false, true)]
    [InlineData(@"hcl:#cfa07d eyr:2025 pid:166559648
iyr:2011 ecl:brn hgt:59in", false, false)]
    public void Valid_CastingToNorthPoleCredental_ReevaluatesValidity(string credentialString, bool passportValidExpectedValue, bool northPoleValidExpectedValue)
    {
      var sut = new sut.PassportCredential(credentialString);

      Assert.Equal(passportValidExpectedValue, sut.Valid);
      Assert.Equal(northPoleValidExpectedValue, ((sut.NorthPoleCredential)sut).Valid);
    }
  }
}
