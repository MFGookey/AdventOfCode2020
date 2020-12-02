using sut = PasswordEvaluator.Core.Database;
using PasswordEvaluator.Core.Policy.Model;
using Xunit;
using System.Collections.Generic;
using Utilities.IO;
using System;
using Moq;
using PasswordEvaluator.Core.Policy;

namespace PasswordEvaluator.Core.Tests.Database
{
  /// <summary>
  /// Tests for the PasswordEvaluator.Core.Database.Database class
  /// </summary>
  public class DatabaseTests
  {
    [Fact]
    public void Database_DefaultConstructor_AssignsPasswordsList()
    {
      var sut = new sut.Database();
      Assert.NotNull(sut.Passwords);
      Assert.Empty(sut.Passwords);
    }

    [Fact]
    public void Database_SpecificConstructor_ContainsProvidedList()
    {
      var passwordList = new List<IPassword>();
      passwordList.Add(new Password("1-2 a: aabuiyoiuy"));

      var sut = new sut.Database(
        passwordList,
        new FileReader(),
        new CharacterFrequencyPasswordPolicyFactory()
      );

      foreach (var password in passwordList)
      {
        Assert.Contains(password, sut.Passwords);
      }

      foreach (var password in sut.Passwords)
      {
        Assert.Contains(password, passwordList);
      }
    }

    [Fact]
    public void Add_Password_AddsPassword()
    {
      var password = new Password("4-5 d: lkjlkddddoiuoiuou");
      var sut = new sut.Database();

      Assert.Empty(sut.Passwords);

      sut.Add(password);

      Assert.Contains(password, sut.Passwords);
    }

    [Fact]
    public void Add_GoodString_AddsPassword()
    {
      var passwordString = "4-5 d: lkjlkddddoiuoiuou";
      var password = new Password(passwordString);
      var sut = new sut.Database();

      Assert.Empty(sut.Passwords);

      sut.Add(passwordString);

      Assert.Contains(password, sut.Passwords);
    }

    [Fact]
    public void Add_BadString_ThrowsExceptionAndDoesNotAdd()
    {
      var passwordString = "asdf";
      var sut = new sut.Database();

      Assert.Empty(sut.Passwords);

      Assert.Throws<ArgumentException>(()=> { sut.Add(passwordString); });

      Assert.Empty(sut.Passwords);
    }

    [Fact]
    public void ImportFile_GoodStrings_AddsPasswords()
    {
      var list = new List<IPassword>();
      var reader = new Mock<IFileReader>();
      var factory = new CharacterPositionPasswordPolicyFactory();

      reader.Setup(r => r.ReadFileByLines(It.IsAny<string>())).Returns(new string[]
      {
        "1-3 a: abcde",
        "1-3 b: cdefg",
        "2-9 c: ccccccccc"
      });

      var sut = new sut.Database(list, reader.Object, factory);

      Assert.Empty(sut.Passwords);

      sut.Import(string.Empty);

      Assert.NotEmpty(sut.Passwords);
      Assert.Equal(3, sut.Passwords.Count);
      Assert.Contains(new Password("1-3 a: abcde", factory), sut.Passwords);
      Assert.Contains(new Password("1-3 b: cdefg", factory), sut.Passwords);
      Assert.Contains(new Password("2-9 c: ccccccccc", factory), sut.Passwords);
    }

    [Fact]
    public void ImportFile_BadStrings_AddsNoPasswords()
    {
      var list = new List<IPassword>();
      var reader = new Mock<IFileReader>();
      var factory = new CharacterFrequencyPasswordPolicyFactory();
      reader.Setup(r => r.ReadFileByLines(It.IsAny<string>())).Returns(new string[]
      {
        "1-3 a: abcde",
        "1-3 b: cdefg",
        "2-9 c: ccccccccc",
        "asdasfgasfasdfas"
      });

      var sut = new sut.Database(list, reader.Object, factory);

      Assert.Empty(sut.Passwords);

      Assert.Throws<ArgumentException>(() => { sut.Import(string.Empty); });

      Assert.Empty(sut.Passwords);
    }

    [Fact]
    public void ImportStrings_GoodStrings_AddsPasswords()
    {
      var sut = new sut.Database();

      Assert.Empty(sut.Passwords);

      sut.Import(new string[]
      {
        "1-3 a: abcde",
        "1-3 b: cdefg",
        "2-9 c: ccccccccc"
      });

      Assert.NotEmpty(sut.Passwords);
      Assert.Equal(3, sut.Passwords.Count);
      Assert.Contains(new Password("1-3 a: abcde"), sut.Passwords);
      Assert.Contains(new Password("1-3 b: cdefg"), sut.Passwords);
      Assert.Contains(new Password("2-9 c: ccccccccc"), sut.Passwords);

      var factory = new CharacterPositionPasswordPolicyFactory();

      Assert.DoesNotContain(new Password("1-3 a: abcde", factory), sut.Passwords);
      Assert.DoesNotContain(new Password("1-3 b: cdefg", factory), sut.Passwords);
      Assert.DoesNotContain(new Password("2-9 c: ccccccccc", factory), sut.Passwords);
    }

    [Fact]
    public void ImportStrings_BadStrings_AddsNoPasswords()
    {
      var sut = new sut.Database();

      Assert.Empty(sut.Passwords);

      Assert.Throws<ArgumentException>(
          () => {
            sut.Import(
              new string[]
              {
                "1-3 a: abcde",
                "1-3 b: cdefg",
                "2-9 c: ccccccccc",
                "asdasfgasfasdfas"
              }
            );
          }
        );

      Assert.Empty(sut.Passwords);
    }
  }
}
