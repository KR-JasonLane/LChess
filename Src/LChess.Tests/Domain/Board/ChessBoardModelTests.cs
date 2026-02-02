using FluentAssertions;
using LChess.Models.Chess;
using LChess.Models.Chess.Board;
using LChess.Models.Result;
using LChess.Util.Enums;

namespace LChess.Tests.Domain.Board;

public class ChessBoardModelTests
{
    [Fact]
    public void Constructor_WhiteUser_SetsColors()
    {
        var board = new ChessBoardModel(PieceColorType.White);

        board.UserColor.Should().Be(PieceColorType.White);
        board.EnemyColor.Should().Be(PieceColorType.Black);
    }

    [Fact]
    public void Constructor_BlackUser_SetsColors()
    {
        var board = new ChessBoardModel(PieceColorType.Black);

        board.UserColor.Should().Be(PieceColorType.Black);
        board.EnemyColor.Should().Be(PieceColorType.White);
    }

    [Fact]
    public void ParseCodes_NullResult_NoException()
    {
        var board = new ChessBoardModel(PieceColorType.White);

        var act = () => board.ParseCodes(null);

        act.Should().NotThrow();
    }

    [Fact]
    public void ParseCodes_ValidResult_Creates8x8Board()
    {
        var board = new ChessBoardModel(PieceColorType.White);
        var result = CreateStartingBoardCodeModel();

        board.ParseCodes(result);

        board.Source.Should().NotBeNull();
        board.Source.Should().HaveCount(8);
        board.Source.All(row => row.Count == 8).Should().BeTrue();
    }

    [Fact]
    public void GameEnd_ClearsAllHighlights()
    {
        var board = new ChessBoardModel(PieceColorType.White);
        var result = CreateStartingBoardCodeModel();
        board.ParseCodes(result);

        board.GameEnd();

        // 모든 타일의 하이라이트가 해제되어야 함
        foreach (var row in board.Source)
        {
            foreach (var tile in row)
            {
                tile.IsHighLightMove.Should().BeFalse();
                tile.IsHighLightEnemy.Should().BeFalse();
            }
        }
    }

    private static StockfishBoardCodeModel CreateStartingBoardCodeModel()
    {
        var model = new StockfishBoardCodeModel();
        model.SetTileCodeList(new List<string>
        {
            " | r | n | b | q | k | b | n | r |",
            " | p | p | p | p | p | p | p | p |",
            " |   |   |   |   |   |   |   |   |",
            " |   |   |   |   |   |   |   |   |",
            " |   |   |   |   |   |   |   |   |",
            " |   |   |   |   |   |   |   |   |",
            " | P | P | P | P | P | P | P | P |",
            " | R | N | B | Q | K | B | N | R |",
        });
        return model;
    }
}
