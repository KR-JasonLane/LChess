using FluentAssertions;
using LChess.Models.Chess.Route;
using LChess.Tests.Helpers;
using LChess.Util.Enums;

namespace LChess.Tests.Domain.Route;

public class KingRouteModelTests
{
    #region :: Basic Movement ::

    [Fact]
    public void King_Center_Has8Moves()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.E4, 'K' },
            { ChessPosition.A8, 'k' },
        });

        var route = new KingRouteModel(ChessPosition.E4, PieceColorType.White);
        var moves = route.GetMovablePositions(board);

        moves.Should().HaveCount(8);
    }

    [Fact]
    public void King_Corner_Has3Moves()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.A1, 'K' },
            { ChessPosition.H8, 'k' },
        });

        var route = new KingRouteModel(ChessPosition.A1, PieceColorType.White);
        var moves = route.GetMovablePositions(board);

        moves.Should().HaveCount(3);
        moves.Should().Contain(new[] { ChessPosition.A2, ChessPosition.B2, ChessPosition.B1 });
    }

    [Fact]
    public void King_CanCaptureEnemy()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.E4, 'K' },
            { ChessPosition.E5, 'p' },
            { ChessPosition.A8, 'k' },
        });

        var route = new KingRouteModel(ChessPosition.E4, PieceColorType.White);
        var moves = route.GetMovablePositions(board);

        moves.Should().Contain(ChessPosition.E5);
    }

    [Fact]
    public void King_CannotCaptureFriendly()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.E4, 'K' },
            { ChessPosition.E5, 'P' },
            { ChessPosition.A8, 'k' },
        });

        var route = new KingRouteModel(ChessPosition.E4, PieceColorType.White);
        var moves = route.GetMovablePositions(board);

        moves.Should().NotContain(ChessPosition.E5);
    }

    #endregion

    #region :: Castling ::

    [Fact]
    public void King_KingSideCastling_Available()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.E1, 'K' },
            { ChessPosition.H1, 'R' },
            { ChessPosition.E8, 'k' },
        });

        var route = new KingRouteModel(ChessPosition.E1, PieceColorType.White);
        var moves = route.GetMovablePositions(board);

        // G1 = 킹사이드 캐슬링 위치
        moves.Should().Contain(ChessPosition.G1);
    }

    [Fact]
    public void King_QueenSideCastling_Available()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.E1, 'K' },
            { ChessPosition.A1, 'R' },
            { ChessPosition.E8, 'k' },
        });

        var route = new KingRouteModel(ChessPosition.E1, PieceColorType.White);
        var moves = route.GetMovablePositions(board);

        // C1 = 퀸사이드 캐슬링 위치
        moves.Should().Contain(ChessPosition.C1);
    }

    [Fact]
    public void King_NoCastling_WhenKingMoved()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.E1, 'K' },
            { ChessPosition.H1, 'R' },
            { ChessPosition.E8, 'k' },
        });

        // 킹 이동 이력 설정 (같은 코드로 UpdateTileUnit 하면 추적 안됨 → MarkTileAsUpdated 사용)
        BoardTestHelper.MarkTileAsUpdated(board, ChessPosition.E1);

        var route = new KingRouteModel(ChessPosition.E1, PieceColorType.White);
        var moves = route.GetMovablePositions(board);

        moves.Should().NotContain(ChessPosition.G1);
        moves.Should().NotContain(ChessPosition.C1);
    }

    [Fact]
    public void King_NoCastling_WhenRookMoved()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.E1, 'K' },
            { ChessPosition.H1, 'R' },
            { ChessPosition.E8, 'k' },
        });

        // 룩 이동 이력 설정
        BoardTestHelper.MarkTileAsUpdated(board, ChessPosition.H1);

        var route = new KingRouteModel(ChessPosition.E1, PieceColorType.White);
        var moves = route.GetMovablePositions(board);

        moves.Should().NotContain(ChessPosition.G1);
    }

    [Fact]
    public void King_NoCastling_WhenPieceBetween()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.E1, 'K' },
            { ChessPosition.F1, 'B' },
            { ChessPosition.H1, 'R' },
            { ChessPosition.E8, 'k' },
        });

        var route = new KingRouteModel(ChessPosition.E1, PieceColorType.White);
        var moves = route.GetMovablePositions(board);

        moves.Should().NotContain(ChessPosition.G1);
    }

    [Fact]
    public void King_BothSideCastling_Available()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.E1, 'K' },
            { ChessPosition.A1, 'R' },
            { ChessPosition.H1, 'R' },
            { ChessPosition.E8, 'k' },
        });

        var route = new KingRouteModel(ChessPosition.E1, PieceColorType.White);
        var moves = route.GetMovablePositions(board);

        moves.Should().Contain(ChessPosition.C1);
        moves.Should().Contain(ChessPosition.G1);
    }

    [Fact]
    public void King_BlackKingSideCastling_Available()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.E8, 'k' },
            { ChessPosition.H8, 'r' },
            { ChessPosition.E1, 'K' },
        }, PieceColorType.Black);

        var route = new KingRouteModel(ChessPosition.E8, PieceColorType.Black);
        var moves = route.GetMovablePositions(board);

        moves.Should().Contain(ChessPosition.G8);
    }

    #endregion
}
