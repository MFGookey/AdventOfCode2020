using System;
using System.Collections.Generic;
using System.Text;
using sut = TobogganPlotter.Core.Map;
using TobogganPlotter.Core.Map.Model;
using Common.Utilities.IO;
using Xunit;
using System.Linq;
using Moq;

namespace TobogganPlotter.Core.Tests.Map
{
  public class HorizontalTilingMapTests
  {
    #region BaseMapTests
    [Fact]
    public void HorizontalTilingMap_EmptyConstructor_InitializesEmptyMap()
    {
      var sut = new sut.HorizontalTilingMap();
      Assert.Equal(0, sut.MapXSize);
      Assert.Equal(0, sut.MapYSize);
    }

    [Theory]
    [MemberData(nameof(MapArrayGenerator))]
    public void HorizontalTilingMap_StringArrayConstructor_InitializesWithStringArrayOfCorrectSize(string[] mapData)
    {
      var sut = new sut.HorizontalTilingMap(mapData);

      var first = mapData.FirstOrDefault();
      first = first ?? string.Empty;

      Assert.Equal(first.Length, sut.MapXSize);
      Assert.Equal(mapData.Length, sut.MapYSize);
    }
    [Theory]
    [MemberData(nameof(MapNonRectangularArrayGenerator))]
    public void HorizontalTilingMap_StringArrayConstructorWithNonRectangularArray_ThrowsFormatException(string[] mapData)
    {
      _ = Assert.Throws<FormatException>(() => { var sut = new sut.HorizontalTilingMap(mapData); });
    }

    [Theory]
    [MemberData(nameof(MapArrayGenerator))]
    public void HorizontalTilingMap_StringArrayAndIFileReaderConstructor_InitializesWithStringArrayOfCorrectSize(string[] mapData)
    {
      var sut = new sut.HorizontalTilingMap(mapData, Mock.Of<IFileReader>());

      var first = mapData.FirstOrDefault();
      first = first ?? string.Empty;

      Assert.Equal(first.Length, sut.MapXSize);
      Assert.Equal(mapData.Length, sut.MapYSize);
    }

    [Theory]
    [MemberData(nameof(MapNonRectangularArrayGenerator))]
    public void HorizontalTilingMap_StringArrayAndIFileReaderConstructorWithNonRectangularArray_ThrowsFormatException(string[] mapData)
    {
      _ = Assert.Throws<FormatException>(() => { var sut = new sut.HorizontalTilingMap(mapData, Mock.Of<IFileReader>()); });
    }

    [Theory]
    [MemberData(nameof(MapArrayGenerator))]
    public void LoadMap_ValidStringArray_LoadsArrayOfCorrectSize(string[] mapData)
    {
      var sut = new sut.HorizontalTilingMap();

      sut.LoadMap(mapData);

      var first = mapData.FirstOrDefault();
      first = first ?? string.Empty;

      Assert.Equal(first.Length, sut.MapXSize);
      Assert.Equal(mapData.Length, sut.MapYSize);
    }

    [Theory]
    [MemberData(nameof(MapNonRectangularArrayGenerator))]
    public void LoadMap_InvalidStringArray_ThrowsFormatException(string[] mapData)
    {
      var sut = new sut.HorizontalTilingMap();

      Assert.Throws<FormatException>(() => { sut.LoadMap(mapData); });

      Assert.Equal(0, sut.MapXSize);
      Assert.Equal(0, sut.MapYSize);
    }

    [Theory]
    [MemberData(nameof(MapArrayGenerator))]
    public void LoadMapFile_ValidMap_LoadsArrayOfCorrectSize(string[] mapData)
    {
      var mockedReader = new Mock<IFileReader>();
      mockedReader.Setup(r => r.ReadFileByLines(It.IsAny<string>())).Returns(mapData);

      var sut = new sut.HorizontalTilingMap(mockedReader.Object);

      sut.LoadMapFile(string.Empty);


      var first = mapData.FirstOrDefault();
      first ??= string.Empty;

      Assert.Equal(first.Length, sut.MapXSize);
      Assert.Equal(mapData.Length, sut.MapYSize);
    }

    [Theory]
    [MemberData(nameof(MapNonRectangularArrayGenerator))]
    public void LoadMapFile_InvalidMap_ThrowsFormatException(string[] mapData)
    {
      var mockedReader = new Mock<IFileReader>();
      mockedReader.Setup(r => r.ReadFileByLines(It.IsAny<string>())).Returns(mapData);

      var sut = new sut.HorizontalTilingMap(mockedReader.Object);

      _ = Assert.Throws<FormatException>(() => { sut.LoadMapFile(string.Empty); });

      Assert.Equal(0, sut.MapXSize);
      Assert.Equal(0, sut.MapYSize);
    }

    public static IEnumerable<object[]> MapArrayGenerator
    {
      get
      {
        yield return new object[]
        {
          new string[]
          { }
        };

        yield return new object[]
        {
          new string[]
          {
            string.Empty,
            string.Empty,
            string.Empty
          }
        };

        yield return new object[]
        {
          new string[]
          {
            "1234"
          }
        };

        yield return new object[]
        {
          new string[]
          {
            "1",
            "2",
            "3",
            "4"
          }
        };

        yield return new object[]
        {
          new string[]
          {
            "0123",
            "4567",
            "89AB",
            "CDEF"
          }
        };

        // From problem statement itself
        yield return new object[]
        {
          new string[]
          {
            "..##.......",
            "#...#...#..",
            ".#....#..#.",
            "..#.#...#.#",
            ".#...##..#.",
            "..#.##.....",
            ".#.#.#....#",
            ".#........#",
            "#.##...#...",
            "#...##....#",
            ".#..#...#.#"
          }
        };
      }
    }

    public static IEnumerable<object[]> MapNonRectangularArrayGenerator
    {
      get
      {
        yield return new object[]
        {
          new string[]
          {
            "1234",
            "123"
          }
        };

        yield return new object[]
        {
          new string[]
          {
            "1234",
            "12345"
          }
        };
      }
    }
    #endregion

    [Theory]
    [MemberData(nameof(MapArrayGenerator))]
    public void GetMapCell_ValidMapData_MatchesHorizontallyDoubledMapData(string[] mapData)
    {
      var sut = new sut.HorizontalTilingMap(mapData);

      var first = mapData.FirstOrDefault();
      first ??= string.Empty;

      for (var x = 0; x < first.Length; x++)
      {
        for (var y = 0; y < mapData.Length; y++)
        {
          Assert.Equal(mapData[y][x], sut.GetMapCell(new Point(x, y)));
          Assert.Equal(mapData[y][x], sut.GetMapCell(new Point(x+sut.MapXSize, y)));
        }
      }

      if (mapData.Length > 0 && first.Length > 0)
      {
        Assert.Equal(mapData[0][int.MaxValue % first.Length], sut.GetMapCell(new Point(int.MaxValue, 0)));
      }

      Assert.Throws<IndexOutOfRangeException>(() => sut.GetMapCell(new Point(0, sut.MapYSize)));
    }
  }
}
