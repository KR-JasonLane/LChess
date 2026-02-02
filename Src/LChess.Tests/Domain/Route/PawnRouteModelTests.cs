using FluentAssertions;
using LChess.Models.Chess.Board;
using LChess.Models.Chess.Route;
using LChess.Tests.Helpers;
using LChess.Util.Enums;

namespace LChess.Tests.Domain.Route;

public class PawnRouteModelTests
{
    #region :: White Pawn - Basic Movement ::

    [Fact]
    public void WhitePawn_InitialPosition_CanMoveTwoSquares()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.E2, 'P' },
            { ChessPosition.E1, 'K' },
            { ChessPosition.E8, 'k' },
        });

        var route = new PawnRouteModel(ChessPosition.E2, PieceColorType.White);
        var moves = route.GetMovablePositions(board);

        moves.Should().Contain(ChessPosition.E3);
        moves.Should().Contain(ChessPosition.E4);
    }

    [Fact]
    public void WhitePawn_AfterFirstMove_CanMoveOneSquare()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.E3, 'P' },
            { ChessPosition.E1, 'K' },
            { ChessPosition.E8, 'k' },
        });

        var route = new PawnRouteModel(ChessPosition.E3, PieceColorType.White);
        var moves = route.GetMovablePositions(board);

        moves.Should().Contain(ChessPosition.E4);
        moves.Should().NotContain(ChessPosition.E5);
    }

    [Fact]
    public void WhitePawn_BlockedByFriendly_CannotMove()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.E2, 'P' },
            { ChessPosition.E3, 'N' },
            { ChessPosition.E1, 'K' },
            { ChessPosition.E8, 'k' },
        });

        var route = new PawnRouteModel(ChessPosition.E2, PieceColorType.White);
        var moves = route.GetMovablePositions(board);

        moves.Should().NotContain(ChessPosition.E3);
        moves.Should().NotContain(ChessPosition.E4);
    }

    [Fact]
    public void WhitePawn_BlockedByEnemy_CannotMoveStraight()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.E4, 'P' },
            { ChessPosition.E5, 'p' },
            { ChessPosition.E1, 'K' },
            { ChessPosition.E8, 'k' },
        });

        var route = new PawnRouteModel(ChessPosition.E4, PieceColorType.White);
        var moves = route.GetMovablePositions(board);

        moves.Should().NotContain(ChessPosition.E5);
    }

    [Fact]
    public void WhitePawn_DoubleBlockedOnFirstMove_OnlyE3()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.E2, 'P' },
            { ChessPosition.E4, 'p' },
            { ChessPosition.E1, 'K' },
            { ChessPosition.E8, 'k' },
        });

        var route = new PawnRouteModel(ChessPosition.E2, PieceColorType.White);
        var moves = route.GetMovablePositions(board);

        moves.Should().Contain(ChessPosition.E3);
        moves.Should().NotContain(ChessPosition.E4);
    }

    #endregion

    #region :: White Pawn - Capture ::

    [Fact]
    public void WhitePawn_CanCaptureLeftDiagonal()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.E4, 'P' },
            { ChessPosition.D5, 'p' },
            { ChessPosition.E1, 'K' },
            { ChessPosition.E8, 'k' },
        });

        var route = new PawnRouteModel(ChessPosition.E4, PieceColorType.White);
        var moves = route.GetMovablePositions(board);

        moves.Should().Contain(ChessPosition.D5);
    }

    [Fact]
    public void WhitePawn_CanCaptureRightDiagonal()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.E4, 'P' },
            { ChessPosition.F5, 'n' },
            { ChessPosition.E1, 'K' },
            { ChessPosition.E8, 'k' },
        });

        var route = new PawnRouteModel(ChessPosition.E4, PieceColorType.White);
        var moves = route.GetMovablePositions(board);

        moves.Should().Contain(ChessPosition.F5);
    }

    [Fact]
    public void WhitePawn_CannotCaptureFriendly()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.E4, 'P' },
            { ChessPosition.D5, 'N' },
            { ChessPosition.E1, 'K' },
            { ChessPosition.E8, 'k' },
        });

        var route = new PawnRouteModel(ChessPosition.E4, PieceColorType.White);
        var moves = route.GetMovablePositions(board);

        moves.Should().NotContain(ChessPosition.D5);
    }

    [Fact]
    public void WhitePawn_NoCaptureOnEmptyDiagonal()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.E4, 'P' },
            { ChessPosition.E1, 'K' },
            { ChessPosition.E8, 'k' },
        });

        var route = new PawnRouteModel(ChessPosition.E4, PieceColorType.White);
        var moves = route.GetMovablePositions(board);

        moves.Should().NotContain(ChessPosition.D5);
        moves.Should().NotContain(ChessPosition.F5);
    }

    #endregion

    #region :: White Pawn - En Passant ::

    [Fact]
    public void WhitePawn_EnPassant_RightSide()
    {
        // White pawn on E5, black pawn just moved F7->F5
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.E5, 'P' },
            { ChessPosition.F5, 'p' },
            { ChessPosition.E1, 'K' },
            { ChessPosition.E8, 'k' },
        });

        // Simulate previous notation "F7F5"
        // PreviousNotation is set via SelectTileAndGetNotationIfNeeded, so we set it manually
        // by using reflection or by making a move. Let's use the board's property setter approach.
        // The PreviousNotation is on BoardManagementModel - it's a private set property.
        // We need to use the selection flow to set it.
        // Alternative: create a board, make a move that sets PreviousNotation
        // For simplicity, let's test through the board management flow
        SetPreviousNotation(board, "F7F5");

        var route = new PawnRouteModel(ChessPosition.E5, PieceColorType.White);
        var moves = route.GetMovablePositions(board);

        moves.Should().Contain(ChessPosition.F6);
    }

    [Fact]
    public void WhitePawn_EnPassant_LeftSide()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.E5, 'P' },
            { ChessPosition.D5, 'p' },
            { ChessPosition.E1, 'K' },
            { ChessPosition.E8, 'k' },
        });

        SetPreviousNotation(board, "D7D5");

        var route = new PawnRouteModel(ChessPosition.E5, PieceColorType.White);
        var moves = route.GetMovablePositions(board);

        moves.Should().Contain(ChessPosition.D6);
    }

    [Fact]
    public void WhitePawn_NoEnPassant_WhenPawnMovedOneSquare()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.E5, 'P' },
            { ChessPosition.F5, 'p' },
            { ChessPosition.E1, 'K' },
            { ChessPosition.E8, 'k' },
        });

        SetPreviousNotation(board, "F6F5");

        var route = new PawnRouteModel(ChessPosition.E5, PieceColorType.White);
        var moves = route.GetMovablePositions(board);

        moves.Should().NotContain(ChessPosition.F6);
    }

    [Fact]
    public void WhitePawn_NoEnPassant_WhenNotAdjacent()
    {
        // 백 폰(E5)과 흑 폰(G5)이 인접하지 않으면 앙파상 불가
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.E5, 'P' },
            { ChessPosition.G5, 'p' },
            { ChessPosition.E1, 'K' },
            { ChessPosition.E8, 'k' },
        });

        SetPreviousNotation(board, "G7G5");

        var route = new PawnRouteModel(ChessPosition.E5, PieceColorType.White);
        var moves = route.GetMovablePositions(board);

        moves.Should().NotContain(ChessPosition.G6);
    }

    #endregion

    #region :: White Pawn - Edge Cases ::

    [Fact]
    public void WhitePawn_OnAFile_NoDiagonalLeft()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.A4, 'P' },
            { ChessPosition.E1, 'K' },
            { ChessPosition.E8, 'k' },
        });

        var route = new PawnRouteModel(ChessPosition.A4, PieceColorType.White);
        var moves = route.GetMovablePositions(board);

        // Should only have forward move, no out-of-bounds diagonals
        moves.Should().Contain(ChessPosition.A5);
    }

    #endregion

    #region :: Black Pawn - Basic Movement ::

    [Fact]
    public void BlackPawn_InitialPosition_CanMoveTwoSquares()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.E7, 'p' },
            { ChessPosition.E1, 'K' },
            { ChessPosition.E8, 'k' },
        });

        var route = new PawnRouteModel(ChessPosition.E7, PieceColorType.Black);
        var moves = route.GetMovablePositions(board);

        moves.Should().Contain(ChessPosition.E6);
        moves.Should().Contain(ChessPosition.E5);
    }

    [Fact]
    public void BlackPawn_AfterFirstMove_CanMoveOneSquare()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.E6, 'p' },
            { ChessPosition.E1, 'K' },
            { ChessPosition.E8, 'k' },
        });

        var route = new PawnRouteModel(ChessPosition.E6, PieceColorType.Black);
        var moves = route.GetMovablePositions(board);

        moves.Should().Contain(ChessPosition.E5);
        moves.Should().NotContain(ChessPosition.E4);
    }

    [Fact]
    public void BlackPawn_CanCaptureLeftDiagonal()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.E5, 'p' },
            { ChessPosition.D4, 'P' },
            { ChessPosition.E1, 'K' },
            { ChessPosition.E8, 'k' },
        });

        var route = new PawnRouteModel(ChessPosition.E5, PieceColorType.Black);
        var moves = route.GetMovablePositions(board);

        moves.Should().Contain(ChessPosition.D4);
    }

    [Fact]
    public void BlackPawn_EnPassant_RightSide()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.D4, 'p' },
            { ChessPosition.E4, 'P' },
            { ChessPosition.E1, 'K' },
            { ChessPosition.E8, 'k' },
        });

        SetPreviousNotation(board, "E2E4");

        var route = new PawnRouteModel(ChessPosition.D4, PieceColorType.Black);
        var moves = route.GetMovablePositions(board);

        moves.Should().Contain(ChessPosition.E3);
    }

    #endregion

    #region :: Helpers ::

    /// <summary>
    /// BoardManagementModel의 PreviousNotation을 설정하기 위한 헬퍼.
    /// SelectTileAndGetNotationIfNeeded를 통해 이동을 시뮬레이션하여 PreviousNotation을 설정한다.
    /// </summary>
    private static void SetPreviousNotation(BoardManagementModel board, string notation)
    {
        // PreviousNotation has private set, so we use reflection
        var prop = typeof(BoardManagementModel).GetProperty("PreviousNotation");
        prop?.SetValue(board, notation);
    }

    #endregion
}
