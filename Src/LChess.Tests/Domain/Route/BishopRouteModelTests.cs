using FluentAssertions;
using LChess.Models.Chess.Route;
using LChess.Tests.Helpers;
using LChess.Util.Enums;

namespace LChess.Tests.Domain.Route;

public class BishopRouteModelTests
{
    [Fact]
    public void Bishop_EmptyBoard_Center_Has13Moves()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.D4, 'B' },
            { ChessPosition.E1, 'K' },
            { ChessPosition.E8, 'k' },
        });

        var route = new BishopRouteModel(ChessPosition.D4, PieceColorType.White);
        var moves = route.GetMovablePositions(board);

        // D4 대각선: LT(C5,B6,A7)=3, RT(E5,F6,G7,H8)=4, LB(C3,B2,A1)=3, RB(E3,F2,G1)=3 = 13
        moves.Should().HaveCount(13);
    }

    [Fact]
    public void Bishop_EmptyBoard_Corner_Has7Moves()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.A8, 'B' },
            { ChessPosition.E1, 'K' },
            { ChessPosition.H8, 'k' },
        });

        var route = new BishopRouteModel(ChessPosition.A8, PieceColorType.White);
        var moves = route.GetMovablePositions(board);

        // A8에서 RB 대각선만 가능: B7,C6,D5,E4,F3,G2,H1 = 7
        moves.Should().HaveCount(7);
    }

    [Fact]
    public void Bishop_BlockedByFriendly_Stops()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.D4, 'B' },
            { ChessPosition.F6, 'P' },
            { ChessPosition.A1, 'K' },
            { ChessPosition.A8, 'k' },
        });

        var route = new BishopRouteModel(ChessPosition.D4, PieceColorType.White);
        var moves = route.GetMovablePositions(board);

        moves.Should().Contain(ChessPosition.E5);
        moves.Should().NotContain(ChessPosition.F6);
        moves.Should().NotContain(ChessPosition.G7);
    }

    [Fact]
    public void Bishop_CanCaptureEnemy_ThenStops()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.D4, 'B' },
            { ChessPosition.F6, 'p' },
            { ChessPosition.A1, 'K' },
            { ChessPosition.A8, 'k' },
        });

        var route = new BishopRouteModel(ChessPosition.D4, PieceColorType.White);
        var moves = route.GetMovablePositions(board);

        moves.Should().Contain(ChessPosition.E5);
        moves.Should().Contain(ChessPosition.F6);
        moves.Should().NotContain(ChessPosition.G7);
    }

    [Fact]
    public void Bishop_SurroundedByFriendly_NoMoves()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.D4, 'B' },
            { ChessPosition.C5, 'P' },
            { ChessPosition.E5, 'P' },
            { ChessPosition.C3, 'P' },
            { ChessPosition.E3, 'P' },
            { ChessPosition.A1, 'K' },
            { ChessPosition.A8, 'k' },
        });

        var route = new BishopRouteModel(ChessPosition.D4, PieceColorType.White);
        var moves = route.GetMovablePositions(board);

        moves.Should().BeEmpty();
    }
}
