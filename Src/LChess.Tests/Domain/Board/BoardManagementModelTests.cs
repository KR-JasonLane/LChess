using FluentAssertions;
using LChess.Models.Chess.Board;
using LChess.Tests.Helpers;
using LChess.Util.Enums;

namespace LChess.Tests.Domain.Board;

public class BoardManagementModelTests
{
    #region :: Tile Selection Flow ::

    [Fact]
    public void SelectTile_EmptyTile_NoResult()
    {
        var board = BoardTestHelper.CreateBoard(BoardStringBuilder.StartingPosition);
        var emptyTile = BoardTestHelper.GetTile(board, ChessPosition.E4)!;

        var result = board.SelectTileAndGetNotationIfNeeded(emptyTile, false);

        result.IsNeedToMove.Should().BeFalse();
    }

    [Fact]
    public void SelectTile_FriendlyPiece_HighlightsRoutes()
    {
        var board = BoardTestHelper.CreateBoard(BoardStringBuilder.StartingPosition);
        var pawnTile = BoardTestHelper.GetTile(board, ChessPosition.E2)!;

        var result = board.SelectTileAndGetNotationIfNeeded(pawnTile, false);

        result.IsNeedToMove.Should().BeFalse();

        // E3, E4 should be highlighted
        var e3 = BoardTestHelper.GetTile(board, ChessPosition.E3)!;
        var e4 = BoardTestHelper.GetTile(board, ChessPosition.E4)!;
        e3.IsHighLightMove.Should().BeTrue();
        e4.IsHighLightMove.Should().BeTrue();
    }

    [Fact]
    public void SelectTile_MoveTo_HighlightedEmpty_ReturnsNotation()
    {
        var board = BoardTestHelper.CreateBoard(BoardStringBuilder.StartingPosition);
        var pawnTile = BoardTestHelper.GetTile(board, ChessPosition.E2)!;

        // 1단계: 기물 선택
        board.SelectTileAndGetNotationIfNeeded(pawnTile, false);

        // 2단계: 이동할 위치 선택
        var targetTile = BoardTestHelper.GetTile(board, ChessPosition.E4)!;
        var result = board.SelectTileAndGetNotationIfNeeded(targetTile, false);

        result.IsNeedToMove.Should().BeTrue();
        result.Notation.Should().Be("E2E4");
    }

    [Fact]
    public void SelectTile_SameTile_NoResult()
    {
        var board = BoardTestHelper.CreateBoard(BoardStringBuilder.StartingPosition);
        var pawnTile = BoardTestHelper.GetTile(board, ChessPosition.E2)!;

        board.SelectTileAndGetNotationIfNeeded(pawnTile, false);
        var result = board.SelectTileAndGetNotationIfNeeded(pawnTile, false);

        result.IsNeedToMove.Should().BeFalse();
    }

    [Fact]
    public void SelectTile_EnemyPiece_WhenNotAllowed_NoResult()
    {
        var board = BoardTestHelper.CreateBoard(BoardStringBuilder.StartingPosition);
        var enemyTile = BoardTestHelper.GetTile(board, ChessPosition.E7)!;

        var result = board.SelectTileAndGetNotationIfNeeded(enemyTile, false);

        result.IsNeedToMove.Should().BeFalse();
    }

    [Fact]
    public void SelectTile_EnemyPiece_WhenAllowed_HighlightsRoutes()
    {
        var board = BoardTestHelper.CreateBoard(BoardStringBuilder.StartingPosition);
        var enemyTile = BoardTestHelper.GetTile(board, ChessPosition.E7)!;

        var result = board.SelectTileAndGetNotationIfNeeded(enemyTile, true);

        result.IsNeedToMove.Should().BeFalse();
        // Enemy pawn should have highlighted moves (E6, E5)
        var e6 = BoardTestHelper.GetTile(board, ChessPosition.E6)!;
        e6.IsHighLightMove.Should().BeTrue();
    }

    #endregion

    #region :: Check Detection ::

    [Fact]
    public void IsCheckStatus_WhenKingHighlighted_ReturnsTrue()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.E1, 'K' },
            { ChessPosition.E8, 'k' },
            { ChessPosition.E3, 'r' },
        });

        board.KingInCheck(new List<string> { "E3" });

        board.IsCheckStatus(PieceColorType.White).Should().BeTrue();
    }

    [Fact]
    public void ClearKingHighLightsIfNeeded_ClearsHighlight()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.E1, 'K' },
            { ChessPosition.E8, 'k' },
            { ChessPosition.E3, 'r' },
        });

        board.KingInCheck(new List<string> { "E3" });
        board.ClearKingHighLightsIfNeeded();

        board.IsCheckStatus(PieceColorType.White).Should().BeFalse();
    }

    #endregion

    #region :: Promotion Detection ::

    [Fact]
    public void SelectTile_PawnReachesEndRow_NeedsPromotion()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.E7, 'P' },
            { ChessPosition.E1, 'K' },
            { ChessPosition.A8, 'k' },
        });

        var pawnTile = BoardTestHelper.GetTile(board, ChessPosition.E7)!;
        board.SelectTileAndGetNotationIfNeeded(pawnTile, false);

        var targetTile = BoardTestHelper.GetTile(board, ChessPosition.E8)!;
        var result = board.SelectTileAndGetNotationIfNeeded(targetTile, false);

        result.IsNeedToMove.Should().BeTrue();
        result.IsNeedToPromotion.Should().BeTrue();
    }

    [Fact]
    public void SelectTile_PawnNotAtEnd_NoPromotion()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.E5, 'P' },
            { ChessPosition.E1, 'K' },
            { ChessPosition.A8, 'k' },
        });

        var pawnTile = BoardTestHelper.GetTile(board, ChessPosition.E5)!;
        board.SelectTileAndGetNotationIfNeeded(pawnTile, false);

        var targetTile = BoardTestHelper.GetTile(board, ChessPosition.E6)!;
        var result = board.SelectTileAndGetNotationIfNeeded(targetTile, false);

        result.IsNeedToMove.Should().BeTrue();
        result.IsNeedToPromotion.Should().BeFalse();
    }

    #endregion

    #region :: Tile Update Tracking ::

    [Fact]
    public void UpdateTileUnit_ChangedPiece_TracksUpdate()
    {
        var board = BoardTestHelper.CreateBoard(BoardStringBuilder.StartingPosition);

        board.UpdateTileUnit(ChessPosition.E2, ' ');

        board.IsTileUpdated(ChessPosition.E2).Should().BeTrue();
    }

    [Fact]
    public void UpdateTileUnit_SamePiece_NotTracked()
    {
        var board = BoardTestHelper.CreateBoard(BoardStringBuilder.StartingPosition);

        board.UpdateTileUnit(ChessPosition.E2, 'P');

        board.IsTileUpdated(ChessPosition.E2).Should().BeFalse();
    }

    [Fact]
    public void Clear_ResetsAllState()
    {
        var board = BoardTestHelper.CreateBoard(BoardStringBuilder.StartingPosition);
        board.UpdateTileUnit(ChessPosition.E2, ' ');

        board.Clear();

        board.IsTileUpdated(ChessPosition.E2).Should().BeFalse();
        board.PreviousNotation.Should().BeEmpty();
    }

    #endregion

    #region :: ClearAllHighLights ::

    [Fact]
    public void ClearAllHighLights_RemovesAllHighlights()
    {
        var board = BoardTestHelper.CreateBoard(BoardStringBuilder.StartingPosition);
        var pawnTile = BoardTestHelper.GetTile(board, ChessPosition.E2)!;
        board.SelectTileAndGetNotationIfNeeded(pawnTile, false);

        board.ClearAllHighLights();

        var e3 = BoardTestHelper.GetTile(board, ChessPosition.E3)!;
        var e4 = BoardTestHelper.GetTile(board, ChessPosition.E4)!;
        e3.IsHighLightMove.Should().BeFalse();
        e4.IsHighLightMove.Should().BeFalse();
    }

    #endregion

    #region :: GetEnemyRoute ::

    [Fact]
    public void GetEnemyRoute_ReturnsAllEnemyMoves()
    {
        var board = BoardTestHelper.CreateBoardWithPieces(new()
        {
            { ChessPosition.E1, 'K' },
            { ChessPosition.E8, 'k' },
            { ChessPosition.D7, 'p' },
        });

        var enemyRoute = board.GetEnemyRoute();

        // 흑 킹과 흑 폰의 이동 가능 위치가 포함되어야 함
        enemyRoute.Should().NotBeEmpty();
    }

    #endregion
}
