using FluentAssertions;
using LChess.Models.Chess.Route;
using LChess.Tests.Helpers;
using LChess.Util.Enums;

namespace LChess.Tests.Domain.Route;

public class RookRouteModelTests
{
    [Fact]
    public void Rook_EmptyBoard_Center_Has14Moves()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.D4, 'R' },
            { ChessPosition.A1, 'K' },
            { ChessPosition.A8, 'k' },
        });

        var route = new RookRouteModel(ChessPosition.D4, PieceColorType.White);
        var moves = route.GetMovablePositions(board);

        // D열 7칸 + 4행 7칸 = 14 (겹치는 중심 제외)
        moves.Should().HaveCount(14);
    }

    [Fact]
    public void Rook_BlockedByFriendlyTop_StopsBeforeFriendly()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.D4, 'R' },
            { ChessPosition.D6, 'N' },
            { ChessPosition.A1, 'K' },
            { ChessPosition.A8, 'k' },
        });

        var route = new RookRouteModel(ChessPosition.D4, PieceColorType.White);
        var moves = route.GetMovablePositions(board);

        moves.Should().Contain(ChessPosition.D5);
        moves.Should().NotContain(ChessPosition.D6);
        moves.Should().NotContain(ChessPosition.D7);
    }

    [Fact]
    public void Rook_BlockedByEnemyTop_CanCaptureEnemy()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.D4, 'R' },
            { ChessPosition.D6, 'n' },
            { ChessPosition.A1, 'K' },
            { ChessPosition.A8, 'k' },
        });

        var route = new RookRouteModel(ChessPosition.D4, PieceColorType.White);
        var moves = route.GetMovablePositions(board);

        moves.Should().Contain(ChessPosition.D5);
        moves.Should().Contain(ChessPosition.D6);
        moves.Should().NotContain(ChessPosition.D7);
    }

    [Fact]
    public void Rook_Corner_Has14Moves()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.A1, 'R' },
            { ChessPosition.E1, 'K' },
            { ChessPosition.E8, 'k' },
        });

        var route = new RookRouteModel(ChessPosition.A1, PieceColorType.White);
        var moves = route.GetMovablePositions(board);

        // A열 위로 7칸 + 1행 오른쪽 7칸 = 14, 하지만 E1에 킹 → E1 이후 차단
        // A열: A2~A8 = 7, 1행: B1~D1 = 3 (E1에 아군 킹) = 10
        moves.Should().HaveCount(10);
    }

    [Fact]
    public void Rook_CanCaptureInAllDirections()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.D4, 'R' },
            { ChessPosition.D6, 'p' },
            { ChessPosition.D2, 'p' },
            { ChessPosition.B4, 'p' },
            { ChessPosition.F4, 'p' },
            { ChessPosition.A1, 'K' },
            { ChessPosition.A8, 'k' },
        });

        var route = new RookRouteModel(ChessPosition.D4, PieceColorType.White);
        var moves = route.GetMovablePositions(board);

        moves.Should().Contain(ChessPosition.D6);
        moves.Should().Contain(ChessPosition.D2);
        moves.Should().Contain(ChessPosition.B4);
        moves.Should().Contain(ChessPosition.F4);
    }

    [Fact]
    public void Rook_SurroundedByFriendly_NoMoves()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.D4, 'R' },
            { ChessPosition.D5, 'P' },
            { ChessPosition.D3, 'P' },
            { ChessPosition.C4, 'P' },
            { ChessPosition.E4, 'P' },
            { ChessPosition.A1, 'K' },
            { ChessPosition.A8, 'k' },
        });

        var route = new RookRouteModel(ChessPosition.D4, PieceColorType.White);
        var moves = route.GetMovablePositions(board);

        moves.Should().BeEmpty();
    }
}
