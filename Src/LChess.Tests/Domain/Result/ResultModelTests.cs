using FluentAssertions;
using LChess.Models.Chess.Match;
using LChess.Models.Result;
using LChess.Util.Enums;

namespace LChess.Tests.Domain.Result;

public class ResultModelTests
{
    #region :: StockfishBoardCodeModel ::

    [Fact]
    public void StockfishBoardCodeModel_IsCheck_WhenCheckersNotEmpty_ReturnsTrue()
    {
        var model = new StockfishBoardCodeModel();
        model.SetCheckerList(new List<string> { "E4" });

        model.IsCheck.Should().BeTrue();
    }

    [Fact]
    public void StockfishBoardCodeModel_IsCheck_WhenCheckersEmpty_ReturnsFalse()
    {
        var model = new StockfishBoardCodeModel();

        model.IsCheck.Should().BeFalse();
    }

    [Fact]
    public void StockfishBoardCodeModel_IsCheck_WhenCheckersHasEmptyString_ReturnsFalse()
    {
        var model = new StockfishBoardCodeModel();
        model.SetCheckerList(new List<string> { "" });

        model.IsCheck.Should().BeFalse();
    }

    [Fact]
    public void StockfishBoardCodeModel_SetTileCodeList_ReplacesExisting()
    {
        var model = new StockfishBoardCodeModel();
        model.SetTileCodeList(new List<string> { "a", "b" });
        model.SetTileCodeList(new List<string> { "c" });

        model.TileCodeList.Should().HaveCount(1);
        model.TileCodeList.First().Should().Be("c");
    }

    #endregion

    #region :: StockfishBestMoveModel ::

    [Fact]
    public void StockfishBestMoveModel_CanMove_WhenNotNone_ReturnsTrue()
    {
        var model = new StockfishBestMoveModel("e2e4");

        model.CanMove.Should().BeTrue();
        model.BestMove.Should().Be("e2e4");
    }

    [Fact]
    public void StockfishBestMoveModel_CanMove_WhenNone_ReturnsFalse()
    {
        var model = new StockfishBestMoveModel("(none)");

        model.CanMove.Should().BeFalse();
    }

    #endregion

    #region :: TileSelectedResultModel ::

    [Fact]
    public void TileSelectedResultModel_IsNeedToMove_WhenNotationSet_ReturnsTrue()
    {
        var model = new TileSelectedResultModel { Notation = "E2E4" };

        model.IsNeedToMove.Should().BeTrue();
    }

    [Fact]
    public void TileSelectedResultModel_IsNeedToMove_WhenEmpty_ReturnsFalse()
    {
        var model = new TileSelectedResultModel();

        model.IsNeedToMove.Should().BeFalse();
    }

    [Fact]
    public void TileSelectedResultModel_IsNeedToPromotion_DefaultFalse()
    {
        var model = new TileSelectedResultModel();

        model.IsNeedToPromotion.Should().BeFalse();
    }

    #endregion

    #region :: MatchStatusModel ::

    [Fact]
    public void MatchStatusModel_WhiteTurn_NextTurnIsBlack()
    {
        var model = new MatchStatusModel(new List<NotationModel>(), PieceColorType.White, false);

        model.CurrentTurn.Should().Be(PieceColorType.White);
        model.NextTurn.Should().Be(PieceColorType.Black);
    }

    [Fact]
    public void MatchStatusModel_BlackTurn_NextTurnIsWhite()
    {
        var model = new MatchStatusModel(new List<NotationModel>(), PieceColorType.Black, false);

        model.CurrentTurn.Should().Be(PieceColorType.Black);
        model.NextTurn.Should().Be(PieceColorType.White);
    }

    [Fact]
    public void MatchStatusModel_NullTurn_DefaultsToBlack()
    {
        var model = new MatchStatusModel(new List<NotationModel>(), null, false);

        model.CurrentTurn.Should().Be(PieceColorType.Black);
        model.NextTurn.Should().Be(PieceColorType.White);
    }

    [Fact]
    public void MatchStatusModel_IsCheck_IsSet()
    {
        var model = new MatchStatusModel(new List<NotationModel>(), PieceColorType.White, true);

        model.IsCheck.Should().BeTrue();
    }

    #endregion

    #region :: NotationModel ::

    [Fact]
    public void NotationModel_IsEmpty_WhenNoNotation_ReturnsTrue()
    {
        var model = new NotationModel();

        model.IsEmpty.Should().BeTrue();
    }

    [Fact]
    public void NotationModel_IsEmpty_WhenHasNotation_ReturnsFalse()
    {
        var model = new NotationModel { Notation = "e2e4", TurnCount = 1 };

        model.IsEmpty.Should().BeFalse();
        model.TurnCount.Should().Be(1);
    }

    #endregion

    #region :: DialogResultModel ::

    [Fact]
    public void DialogResultModel_PreservesValues()
    {
        var model = new DialogResultModel(true, "OK");

        model.ButtonResult.Should().BeTrue();
        model.Response.Should().Be("OK");
    }

    [Fact]
    public void DialogResultModel_Cancel_PreservesValues()
    {
        var model = new DialogResultModel(false, "");

        model.ButtonResult.Should().BeFalse();
        model.Response.Should().BeEmpty();
    }

    #endregion
}
