using FluentAssertions;
using LChess.Models.Chess.Board;
using LChess.Util.Enums;

namespace LChess.Tests.Domain.Board;

public class ChessPositionModelTests
{
    [Theory]
    [InlineData(0, 0, ChessPosition.A8)]
    [InlineData(0, 7, ChessPosition.H8)]
    [InlineData(7, 0, ChessPosition.A1)]
    [InlineData(7, 7, ChessPosition.H1)]
    [InlineData(4, 4, ChessPosition.E4)]
    [InlineData(6, 4, ChessPosition.E2)]
    public void Constructor_WithRowAndColumn_SetsCorrectCode(int row, int col, ChessPosition expected)
    {
        var position = new ChessPositionModel(row, col);

        position.Row.Should().Be(row);
        position.Column.Should().Be(col);
        position.Code.Should().Be(expected);
    }

    [Theory]
    [InlineData(ChessPosition.E4, 4, 4)]
    [InlineData(ChessPosition.A8, 0, 0)]
    [InlineData(ChessPosition.H1, 7, 7)]
    [InlineData(ChessPosition.A1, 7, 0)]
    public void Constructor_WithChessPosition_SetsRowAndColumn(ChessPosition pos, int expectedRow, int expectedCol)
    {
        var position = new ChessPositionModel(pos);

        position.Row.Should().Be(expectedRow);
        position.Column.Should().Be(expectedCol);
        position.Code.Should().Be(pos);
    }

    [Theory]
    [InlineData(0, 0, ChessPosition.A8)]
    [InlineData(7, 7, ChessPosition.H1)]
    [InlineData(6, 4, ChessPosition.E2)]
    public void CalcPositionCode_ValidCoordinates_ReturnsCorrectPosition(int row, int col, ChessPosition expected)
    {
        ChessPositionModel.CalcPositionCode(row, col).Should().Be(expected);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(8, 0)]
    [InlineData(0, 8)]
    [InlineData(0, -1)]
    [InlineData(9, 9)]
    public void CalcPositionCode_OutOfBounds_ReturnsInvalid(int row, int col)
    {
        ChessPositionModel.CalcPositionCode(row, col).Should().Be(ChessPosition.Invalid);
    }

    [Fact]
    public void GetTopLinePositions_FromE4_Returns4Positions()
    {
        var position = new ChessPositionModel(ChessPosition.E4);

        var top = position.GetTopLinePositions();

        top.Should().HaveCount(4);
        top.Should().ContainInOrder(ChessPosition.E5, ChessPosition.E6, ChessPosition.E7, ChessPosition.E8);
    }

    [Fact]
    public void GetTopLinePositions_WithMax1_ReturnsOnePosition()
    {
        var position = new ChessPositionModel(ChessPosition.E4);

        var top = position.GetTopLinePositions(1);

        top.Should().HaveCount(1);
        top.First().Should().Be(ChessPosition.E5);
    }

    [Fact]
    public void GetBottomLinePositions_FromE4_Returns3Positions()
    {
        var position = new ChessPositionModel(ChessPosition.E4);

        var bottom = position.GetBottomLinePositions();

        bottom.Should().HaveCount(3);
        bottom.Should().ContainInOrder(ChessPosition.E3, ChessPosition.E2, ChessPosition.E1);
    }

    [Fact]
    public void GetLeftLinePositions_FromE4_Returns4Positions()
    {
        var position = new ChessPositionModel(ChessPosition.E4);

        var left = position.GetLeftLinePositions();

        left.Should().HaveCount(4);
        left.Should().ContainInOrder(ChessPosition.D4, ChessPosition.C4, ChessPosition.B4, ChessPosition.A4);
    }

    [Fact]
    public void GetRightLinePositions_FromE4_Returns3Positions()
    {
        var position = new ChessPositionModel(ChessPosition.E4);

        var right = position.GetRightLinePositions();

        right.Should().HaveCount(3);
        right.Should().ContainInOrder(ChessPosition.F4, ChessPosition.G4, ChessPosition.H4);
    }

    [Fact]
    public void GetLeftTopDiagonalPositions_FromE4_Returns4Positions()
    {
        var position = new ChessPositionModel(ChessPosition.E4);

        var diag = position.GetLeftTopDiagonalPositions();

        diag.Should().HaveCount(4);
        diag.Should().ContainInOrder(ChessPosition.D5, ChessPosition.C6, ChessPosition.B7, ChessPosition.A8);
    }

    [Fact]
    public void GetRightTopDiagonalPositions_FromE4_Returns3Positions()
    {
        var position = new ChessPositionModel(ChessPosition.E4);

        var diag = position.GetRightTopDiagonalPositions();

        diag.Should().HaveCount(3);
        diag.Should().ContainInOrder(ChessPosition.F5, ChessPosition.G6, ChessPosition.H7);
    }

    [Fact]
    public void GetLeftBottomDiagonalPositions_FromE4_Returns3Positions()
    {
        var position = new ChessPositionModel(ChessPosition.E4);

        var diag = position.GetLeftBottomDiagonalPositions();

        diag.Should().HaveCount(3);
        diag.Should().ContainInOrder(ChessPosition.D3, ChessPosition.C2, ChessPosition.B1);
    }

    [Fact]
    public void GetRightBottomDiagonalPositions_FromE4_Returns3Positions()
    {
        var position = new ChessPositionModel(ChessPosition.E4);

        var diag = position.GetRightBottomDiagonalPositions();

        diag.Should().HaveCount(3);
        diag.Should().ContainInOrder(ChessPosition.F3, ChessPosition.G2, ChessPosition.H1);
    }

    [Fact]
    public void GetTopLinePositions_FromA8_ReturnsEmpty()
    {
        var position = new ChessPositionModel(ChessPosition.A8);

        position.GetTopLinePositions().Should().BeEmpty();
    }

    [Fact]
    public void GetLeftLinePositions_FromA4_ReturnsEmpty()
    {
        var position = new ChessPositionModel(ChessPosition.A4);

        position.GetLeftLinePositions().Should().BeEmpty();
    }

    [Fact]
    public void GetBottomLinePositions_FromH1_ReturnsEmpty()
    {
        var position = new ChessPositionModel(ChessPosition.H1);

        position.GetBottomLinePositions().Should().BeEmpty();
    }

    [Fact]
    public void GetRightLinePositions_FromH4_ReturnsEmpty()
    {
        var position = new ChessPositionModel(ChessPosition.H4);

        position.GetRightLinePositions().Should().BeEmpty();
    }

    [Theory]
    [InlineData(ChessPosition.A1, true)]
    [InlineData(ChessPosition.H1, true)]
    [InlineData(ChessPosition.A8, true)]
    [InlineData(ChessPosition.H8, true)]
    [InlineData(ChessPosition.E4, false)]
    [InlineData(ChessPosition.D5, false)]
    public void IsEndPointColumnInBoard_ReturnsCorrectResult(ChessPosition pos, bool expected)
    {
        var position = new ChessPositionModel(pos);

        position.IsEndPointColumnInBoard.Should().Be(expected);
    }

    [Fact]
    public void CreatePositionsByOffset_StopsAtBoardEdge()
    {
        var position = new ChessPositionModel(ChessPosition.G7);

        // 우측상단 대각선: G7 → H8 (1칸만 가능)
        var diag = position.CreatePositionsByOffset(-1, 1, 8);

        diag.Should().HaveCount(1);
        diag.First().Should().Be(ChessPosition.H8);
    }
}
