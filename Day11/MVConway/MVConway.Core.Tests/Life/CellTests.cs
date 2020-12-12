using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using MVConway.Core.Life;
using Xunit;
using Moq;

namespace MVConway.Core.Tests.Life
{
  public class CellTests
  {
    [Fact]
    public void SetNeighbors_WhenSet_ProperlySetsNeighbors()
    {
      var sut = new Cell(
        CellState.Empty,
        (n, s) => s,
        s => s.ToString()
      );

      Assert.Null(sut.Neighbors);

      sut.SetNeighbors(new [] { new ICell[] { }});

      Assert.Equal(new[] { new ICell[] { } }, sut.Neighbors);
    }

    [Fact]
    public void SetNeighbors_WhenSentNull_ThrowsArgumentNullException()
    {
      var sut = new Cell(
        CellState.Empty,
        (
          n,
          s) => s,
        (
          s) => s.ToString());

      Assert.Null(sut.Neighbors);

      Assert.Throws<ArgumentNullException>(() => sut.SetNeighbors(null));
    }

    [Fact]
    public void SetNeighbors_WhenSetTwice_ThrowsMemberAccessException()
    {
      var sut = new Cell(
        CellState.Empty,
        (
          n,
          s) => s,
        (
          s) => s.ToString());

      sut.SetNeighbors(new[] { new ICell[] { } });

      Assert.NotNull(sut.Neighbors);

      Assert.Throws<MemberAccessException>(() => sut.SetNeighbors(new[] { new ICell[] { } }));
    }

    [Fact]
    public void CalculateNextState_WhenCalledTwiceInARow_ThrowsException()
    {
      var sut = new Cell(
        CellState.Empty,
        (
          n,
          s) => s,
        (
          s) => s.ToString());
      sut.CalculateNextState();
      Assert.Throws<Exception>(() => sut.CalculateNextState());
    }

    [Fact]
    public void Step_WhenCalledWithoutCalculatingNextState_ThrowsException()
    {
      var sut = new Cell(
        CellState.Empty,
        (
          n,
          s) => s,
        (
          s) => s.ToString());
      Assert.Throws<Exception>(() => sut.Step());
    }

    [Fact]
    public void Constructor_WhenGivenNullNextStep_ThrowsArgumentNullException()
    {
      Assert.Throws<ArgumentNullException>(() => new Cell(
        CellState.Empty,
        null,
        (
          s) => s.ToString()));
    }

    [Fact]
    public void Constructor_WhenGivenNullDisplay_ThrowsArgumentNullException()
    {
      Assert.Throws<ArgumentNullException>(() => new Cell(
        CellState.Empty,
        (
          n,
          s) => s,
        null));
    }


    [Theory]
    [MemberData(nameof(ConstructorTestData))]
    public void Constructor_GivenValidData_SetsValuesAsExpected(
      CellState cellState,
      Func<IReadOnlyCollection<IReadOnlyCollection<ICell>>, CellState, CellState> stateTransitionFunc,
      Func<CellState, string> displayFunc,
      IReadOnlyCollection<IReadOnlyCollection<ICell>> neighbors,
      IEnumerable<CellState> expectedStates,
      IDictionary<CellState, string> expectedStringMapping,
      IReadOnlyCollection<IReadOnlyCollection<ICell>> expectedNeighbors
    )
    {
      var sut = new Cell(cellState, stateTransitionFunc, displayFunc);
      if (neighbors != null)
      {
        sut.SetNeighbors(neighbors);
      }
      Assert.Equal(sut.Neighbors, expectedNeighbors);

      foreach (var state in expectedStates)
      {
        Assert.Equal(state, sut.CurrentState);
        Assert.Equal(expectedStringMapping[state], sut.ToString());

        // We end up calling this one extra time, but I don't really care
        sut.CalculateNextState();
        sut.Step();
      }
    }

    public static IEnumerable<object[]> ConstructorTestData
    {
      get
      {
        yield return new object[]
        {
          CellState.Empty,
          new Func<IEnumerable<IEnumerable<ICell>>, CellState, CellState>((n, s) =>
          {
            Assert.Equal(new []{new ICell[]{ }}, n);
            return s;
          }),
          new Func<CellState, string>(s => "s"),
          new []{new ICell[]{ }},
          new[]
          {
            CellState.Empty,
            CellState.Empty
          },
          new Dictionary<CellState, string>
          {
            { CellState.Empty, "s"}
          },
          new []{new ICell[]{ }}
        };

        yield return new object[]
        {
          CellState.Empty,
          new Func<IEnumerable<IEnumerable<ICell>>, CellState, CellState>((n, s) =>
          {
            // Indirectly prove that the neighbors we provided get passed into this func
            Assert.Null(n);
            switch (s)
            {
              case CellState.Empty:
                return CellState.Alive;
              case CellState.Alive:
                return CellState.Unavailable;
              case CellState.Unavailable:
                return CellState.Empty;
              default:
                throw new Exception("This shouldn't happen");
            }
          }),
          new Func<CellState, string>(s =>
          {
            switch (s)
            {
              case CellState.Empty:
                return "L";
              case CellState.Alive:
                return "#";
              case CellState.Unavailable:
                return ".";
              default:
                throw new Exception("This shouldn't happen");
            }
          }),
          null,
          new[]
          {
            CellState.Empty,
            CellState.Alive,
            CellState.Unavailable,
            CellState.Empty
          },
          new Dictionary<CellState, string>
          {
            { CellState.Empty, "L"},
            { CellState.Alive, "#"},
            { CellState.Unavailable, "."}
          },
          null
        };
      }
    }
  }
}