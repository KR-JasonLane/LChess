using FluentAssertions;
using LChess.Models.Chess.Unit;
using LChess.Models.Chess.Unit.Base;
using LChess.Util.Enums;

namespace LChess.Tests.Domain.Unit;

public class ChessUnitModelBaseTests
{
    [Theory]
    [InlineData('P', typeof(PawnModel), PieceColorType.White)]
    [InlineData('p', typeof(PawnModel), PieceColorType.Black)]
    [InlineData('R', typeof(RookModel), PieceColorType.White)]
    [InlineData('r', typeof(RookModel), PieceColorType.Black)]
    [InlineData('N', typeof(KnightModel), PieceColorType.White)]
    [InlineData('n', typeof(KnightModel), PieceColorType.Black)]
    [InlineData('B', typeof(BishopModel), PieceColorType.White)]
    [InlineData('b', typeof(BishopModel), PieceColorType.Black)]
    [InlineData('Q', typeof(QueenModel), PieceColorType.White)]
    [InlineData('q', typeof(QueenModel), PieceColorType.Black)]
    [InlineData('K', typeof(KingModel), PieceColorType.White)]
    [InlineData('k', typeof(KingModel), PieceColorType.Black)]
    public void CreateUnitModel_ValidCode_ReturnsCorrectTypeAndColor(char code, Type expectedType, PieceColorType expectedColor)
    {
        var unit = ChessUnitModelBase.CreateUnitModel(code, ChessPosition.E4);

        unit.Should().NotBeNull();
        unit.Should().BeOfType(expectedType);
        unit!.ColorType.Should().Be(expectedColor);
    }

    [Fact]
    public void CreateUnitModel_Space_ReturnsNull()
    {
        ChessUnitModelBase.CreateUnitModel(' ', ChessPosition.E4).Should().BeNull();
    }

    [Theory]
    [InlineData('X')]
    [InlineData('0')]
    [InlineData('.')]
    public void CreateUnitModel_InvalidChar_ReturnsNull(char code)
    {
        ChessUnitModelBase.CreateUnitModel(code, ChessPosition.E4).Should().BeNull();
    }

    [Theory]
    [InlineData(ChessUnitType.Pawn, typeof(PawnModel))]
    [InlineData(ChessUnitType.Rook, typeof(RookModel))]
    [InlineData(ChessUnitType.Knight, typeof(KnightModel))]
    [InlineData(ChessUnitType.Bishop, typeof(BishopModel))]
    [InlineData(ChessUnitType.Queen, typeof(QueenModel))]
    [InlineData(ChessUnitType.King, typeof(KingModel))]
    public void CreateVirtualUnitModel_AllTypes_ReturnsCorrectType(ChessUnitType unitType, Type expectedType)
    {
        var unit = ChessUnitModelBase.CreateVirtualUnitModel(unitType, PieceColorType.White, ChessPosition.E4);

        unit.Should().NotBeNull();
        unit.Should().BeOfType(expectedType);
        unit!.ColorType.Should().Be(PieceColorType.White);
    }

    [Fact]
    public void IsSameColor_SameColor_ReturnsTrue()
    {
        var unit = ChessUnitModelBase.CreateUnitModel('P', ChessPosition.E2)!;

        unit.IsSameColor(PieceColorType.White).Should().BeTrue();
    }

    [Fact]
    public void IsSameColor_DifferentColor_ReturnsFalse()
    {
        var unit = ChessUnitModelBase.CreateUnitModel('P', ChessPosition.E2)!;

        unit.IsSameColor(PieceColorType.Black).Should().BeFalse();
    }

    [Fact]
    public void IsSameColor_Null_ReturnsFalse()
    {
        var unit = ChessUnitModelBase.CreateUnitModel('P', ChessPosition.E2)!;

        unit.IsSameColor(null).Should().BeFalse();
    }

    [Fact]
    public void IsEnemy_DifferentColor_ReturnsTrue()
    {
        var white = ChessUnitModelBase.CreateUnitModel('P', ChessPosition.E2)!;
        var black = ChessUnitModelBase.CreateUnitModel('p', ChessPosition.E7)!;

        white.IsEnemy(black).Should().BeTrue();
    }

    [Fact]
    public void IsEnemy_SameColor_ReturnsFalse()
    {
        var white1 = ChessUnitModelBase.CreateUnitModel('P', ChessPosition.E2)!;
        var white2 = ChessUnitModelBase.CreateUnitModel('R', ChessPosition.A1)!;

        white1.IsEnemy(white2).Should().BeFalse();
    }

    [Fact]
    public void IsEnemy_Null_ReturnsFalse()
    {
        var unit = ChessUnitModelBase.CreateUnitModel('P', ChessPosition.E2)!;

        unit.IsEnemy(null).Should().BeFalse();
    }

    [Fact]
    public void OriginalCode_IsPreserved()
    {
        var unit = ChessUnitModelBase.CreateUnitModel('P', ChessPosition.E2)!;

        unit.OriginalCode.Should().Be('P');
    }

    [Fact]
    public void RouteModel_IsNotNull_WhenCreated()
    {
        var unit = ChessUnitModelBase.CreateUnitModel('N', ChessPosition.B1)!;

        unit.RouteModel.Should().NotBeNull();
    }

    [Theory]
    [InlineData('P', ChessUnitType.Pawn)]
    [InlineData('R', ChessUnitType.Rook)]
    [InlineData('N', ChessUnitType.Knight)]
    [InlineData('B', ChessUnitType.Bishop)]
    [InlineData('Q', ChessUnitType.Queen)]
    [InlineData('K', ChessUnitType.King)]
    public void CreateUnitModel_SetsCorrectUnitType(char code, ChessUnitType expectedType)
    {
        var unit = ChessUnitModelBase.CreateUnitModel(code, ChessPosition.E4)!;

        unit.UnitType.Should().Be(expectedType);
    }
}
