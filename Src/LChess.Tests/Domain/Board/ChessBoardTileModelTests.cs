using FluentAssertions;
using LChess.Models.Chess;
using LChess.Util.Enums;

namespace LChess.Tests.Domain.Board;

public class ChessBoardTileModelTests
{
    [Fact]
    public void Constructor_WithUnit_IsEmptyFalse()
    {
        var tile = new ChessBoardTileModel(ChessTileColorType.Light, 6, 4, 'P');

        tile.IsEmpty.Should().BeFalse();
        tile.Unit.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithSpace_IsEmptyTrue()
    {
        var tile = new ChessBoardTileModel(ChessTileColorType.Dark, 4, 4, ' ');

        tile.IsEmpty.Should().BeTrue();
        tile.Unit.Should().BeNull();
    }

    [Fact]
    public void Position_IsCorrectlySet()
    {
        var tile = new ChessBoardTileModel(ChessTileColorType.Light, 6, 4, 'P');

        tile.Position.Row.Should().Be(6);
        tile.Position.Column.Should().Be(4);
        tile.Position.Code.Should().Be(ChessPosition.E2);
    }

    [Fact]
    public void TileColorType_IsSet()
    {
        var tile = new ChessBoardTileModel(ChessTileColorType.Dark, 0, 0, ' ');

        tile.TileColorType.Should().Be(ChessTileColorType.Dark);
    }

    [Fact]
    public void UpdateUnit_DifferentCode_ReturnsTrue()
    {
        var tile = new ChessBoardTileModel(ChessTileColorType.Light, 6, 4, 'P');

        tile.UpdateUnit('R').Should().BeTrue();
        tile.Unit.Should().NotBeNull();
        tile.Unit!.UnitType.Should().Be(ChessUnitType.Rook);
    }

    [Fact]
    public void UpdateUnit_SameCode_ReturnsFalse()
    {
        var tile = new ChessBoardTileModel(ChessTileColorType.Light, 6, 4, 'P');

        tile.UpdateUnit('P').Should().BeFalse();
    }

    [Fact]
    public void UpdateUnit_FromUnitToEmpty_ReturnsTrue()
    {
        var tile = new ChessBoardTileModel(ChessTileColorType.Light, 6, 4, 'P');

        tile.UpdateUnit(' ').Should().BeTrue();
        tile.IsEmpty.Should().BeTrue();
    }

    [Fact]
    public void UpdateUnit_FromEmptyToUnit_ReturnsTrue()
    {
        var tile = new ChessBoardTileModel(ChessTileColorType.Light, 4, 4, ' ');

        tile.UpdateUnit('P').Should().BeTrue();
        tile.IsEmpty.Should().BeFalse();
    }

    [Fact]
    public void UpdateUnit_ClearsHighLights()
    {
        var tile = new ChessBoardTileModel(ChessTileColorType.Light, 4, 4, ' ');
        tile.IsHighLightMove = true;
        tile.IsHighLightEnemy = true;
        tile.IsSelected = true;

        tile.UpdateUnit('P');

        tile.IsHighLightMove.Should().BeFalse();
        tile.IsHighLightEnemy.Should().BeFalse();
        tile.IsSelected.Should().BeFalse();
    }

    [Fact]
    public void TurnOffHighLight_ClearsAllFlags()
    {
        var tile = new ChessBoardTileModel(ChessTileColorType.Light, 4, 4, 'P');
        tile.IsHighLightMove = true;
        tile.IsHighLightEnemy = true;
        tile.IsSelected = true;

        tile.TurnOffHighLight();

        tile.IsHighLightMove.Should().BeFalse();
        tile.IsHighLightEnemy.Should().BeFalse();
        tile.IsSelected.Should().BeFalse();
    }

    [Fact]
    public void IsMovableTarget_WhenHighLightMove_True()
    {
        var tile = new ChessBoardTileModel(ChessTileColorType.Light, 4, 4, ' ');
        tile.IsHighLightMove = true;

        tile.IsMovableTarget.Should().BeTrue();
    }

    [Fact]
    public void IsMovableTarget_WhenHighLightEnemy_True()
    {
        var tile = new ChessBoardTileModel(ChessTileColorType.Light, 4, 4, 'p');
        tile.IsHighLightEnemy = true;

        tile.IsMovableTarget.Should().BeTrue();
    }

    [Fact]
    public void IsMovableTarget_WhenNoHighLight_False()
    {
        var tile = new ChessBoardTileModel(ChessTileColorType.Light, 4, 4, 'P');

        tile.IsMovableTarget.Should().BeFalse();
    }
}
