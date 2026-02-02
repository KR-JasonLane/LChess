using FluentAssertions;
using LChess.Models.Chess.Route;
using LChess.Tests.Helpers;
using LChess.Util.Enums;

namespace LChess.Tests.Domain.Route;

public class KnightRouteModelTests
{
    [Fact]
    public void Knight_Center_Has8Moves()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.D4, 'N' },
            { ChessPosition.A1, 'K' },
            { ChessPosition.A8, 'k' },
        });

        var route = new KnightRouteModel(ChessPosition.D4, PieceColorType.White);
        var moves = route.GetMovablePositions(board);

        moves.Should().HaveCount(8);
        moves.Should().Contain(new[]
        {
            ChessPosition.C2, ChessPosition.E2,
            ChessPosition.B3, ChessPosition.F3,
            ChessPosition.B5, ChessPosition.F5,
            ChessPosition.C6, ChessPosition.E6,
        });
    }

    [Fact]
    public void Knight_Corner_Has2Moves()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.A1, 'N' },
            { ChessPosition.E1, 'K' },
            { ChessPosition.E8, 'k' },
        });

        var route = new KnightRouteModel(ChessPosition.A1, PieceColorType.White);
        var moves = route.GetMovablePositions(board);

        moves.Should().HaveCount(2);
        moves.Should().Contain(ChessPosition.B3);
        moves.Should().Contain(ChessPosition.C2);
    }

    [Fact]
    public void Knight_CanJumpOverPieces()
    {
        // Knight at B1 with pawns blocking B2, C2 — knight can still jump
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.B1, 'N' },
            { ChessPosition.B2, 'P' },
            { ChessPosition.C2, 'P' },
            { ChessPosition.A2, 'P' },
            { ChessPosition.E1, 'K' },
            { ChessPosition.E8, 'k' },
        });

        var route = new KnightRouteModel(ChessPosition.B1, PieceColorType.White);
        var moves = route.GetMovablePositions(board);

        moves.Should().Contain(ChessPosition.A3);
        moves.Should().Contain(ChessPosition.C3);
        moves.Should().Contain(ChessPosition.D2);
    }

    [Fact]
    public void Knight_CanCaptureEnemy()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.D4, 'N' },
            { ChessPosition.E6, 'p' },
            { ChessPosition.A1, 'K' },
            { ChessPosition.A8, 'k' },
        });

        var route = new KnightRouteModel(ChessPosition.D4, PieceColorType.White);
        var moves = route.GetMovablePositions(board);

        moves.Should().Contain(ChessPosition.E6);
    }

    [Fact]
    public void Knight_CannotCaptureFriendly()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.D4, 'N' },
            { ChessPosition.E6, 'P' },
            { ChessPosition.A1, 'K' },
            { ChessPosition.A8, 'k' },
        });

        var route = new KnightRouteModel(ChessPosition.D4, PieceColorType.White);
        var moves = route.GetMovablePositions(board);

        moves.Should().NotContain(ChessPosition.E6);
    }
}
