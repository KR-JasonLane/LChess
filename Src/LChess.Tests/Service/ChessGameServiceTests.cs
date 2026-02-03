using FluentAssertions;
using LChess.Abstract.Service;
using LChess.Models.Chess.Match;
using LChess.Models.Result;
using LChess.Models.Setting;
using LChess.Service.Game;
using Moq;

namespace LChess.Tests.Service;

/// <summary>
/// ChessGameService 유닛테스트
/// IStockfishEngineService와 IUserSettingService를 Mock하여 게임 로직을 검증한다.
/// </summary>
public class ChessGameServiceTests
{
    private readonly Mock<IStockfishEngineService> _mockEngine;
    private readonly Mock<IUserSettingService> _mockUserSetting;
    private readonly ChessGameService _sut;

    public ChessGameServiceTests()
    {
        _mockEngine = new Mock<IStockfishEngineService>();
        _mockUserSetting = new Mock<IUserSettingService>();
        _sut = new ChessGameService(_mockEngine.Object, _mockUserSetting.Object);
    }

    #region :: Helpers ::

    private static StockfishBoardCodeModel CreateBoard(params string[] tileCodes)
    {
        var board = new StockfishBoardCodeModel();
        board.SetTileCodeList(tileCodes.ToList());
        return board;
    }

    private static readonly string[] StartingTiles =
    [
        " | r | n | b | q | k | b | n | r |",
        " | p | p | p | p | p | p | p | p |",
        " |   |   |   |   |   |   |   |   |",
        " |   |   |   |   |   |   |   |   |",
        " |   |   |   |   |   |   |   |   |",
        " |   |   |   |   |   |   |   |   |",
        " | P | P | P | P | P | P | P | P |",
        " | R | N | B | Q | K | B | N | R |"
    ];

    private static readonly string[] MovedTiles =
    [
        " | r | n | b | q | k | b | n | r |",
        " | p | p | p | p | p | p | p | p |",
        " |   |   |   |   |   |   |   |   |",
        " |   |   |   |   |   |   |   |   |",
        " |   |   |   |   | P |   |   |   |",
        " |   |   |   |   |   |   |   |   |",
        " | P | P | P | P |   | P | P | P |",
        " | R | N | B | Q | K | B | N | R |"
    ];

    /// <summary>
    /// NewGame를 호출하고 MovePiece가 다른 보드를 반환하도록 SetupSequence를 구성한다.
    /// </summary>
    private void SetupNewGameAndMove()
    {
        _mockEngine.Setup(e => e.StartEngineAsync()).ReturnsAsync(true);
        _mockEngine.Setup(e => e.SendCommandAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((string?)null);

        _mockEngine.SetupSequence(e => e.GetCurrentBoard())
            .ReturnsAsync(CreateBoard(StartingTiles))
            .ReturnsAsync(CreateBoard(MovedTiles));
    }

    /// <summary>
    /// NewGame를 호출하고 MovePiece가 동일한 보드를 반환하도록 설정한다.
    /// </summary>
    private void SetupNewGameAndSameBoard()
    {
        _mockEngine.Setup(e => e.StartEngineAsync()).ReturnsAsync(true);
        _mockEngine.Setup(e => e.SendCommandAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((string?)null);

        _mockEngine.SetupSequence(e => e.GetCurrentBoard())
            .ReturnsAsync(CreateBoard(StartingTiles))
            .ReturnsAsync(CreateBoard(StartingTiles));
    }

    #endregion

    #region :: NewGame ::

    [Fact]
    public async Task NewGame_WhenCalled_StartsEngine()
    {
        // Arrange
        _mockEngine.Setup(e => e.StartEngineAsync()).ReturnsAsync(true);
        _mockEngine.Setup(e => e.SendCommandAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((string?)null);
        _mockEngine.Setup(e => e.GetCurrentBoard()).ReturnsAsync(CreateBoard(StartingTiles));

        // Act
        await _sut.NewGame();

        // Assert
        _mockEngine.Verify(e => e.StartEngineAsync(), Times.Once);
    }

    [Fact]
    public async Task NewGame_WhenCalled_SendsStartPositionCommand()
    {
        // Arrange
        _mockEngine.Setup(e => e.StartEngineAsync()).ReturnsAsync(true);
        _mockEngine.Setup(e => e.SendCommandAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((string?)null);
        _mockEngine.Setup(e => e.GetCurrentBoard()).ReturnsAsync(CreateBoard(StartingTiles));

        // Act
        await _sut.NewGame();

        // Assert
        _mockEngine.Verify(
            e => e.SendCommandAsync("position startpos", string.Empty),
            Times.Once);
    }

    [Fact]
    public async Task NewGame_WhenCalled_ReturnsBoardFromEngine()
    {
        // Arrange
        var expectedBoard = CreateBoard(StartingTiles);
        _mockEngine.Setup(e => e.StartEngineAsync()).ReturnsAsync(true);
        _mockEngine.Setup(e => e.SendCommandAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((string?)null);
        _mockEngine.Setup(e => e.GetCurrentBoard()).ReturnsAsync(expectedBoard);

        // Act
        var result = await _sut.NewGame();

        // Assert
        result.Should().BeSameAs(expectedBoard);
    }

    #endregion

    #region :: MovePiece(string?) ::

    [Fact]
    public async Task MovePiece_NullNotation_ReturnsNull()
    {
        // Act
        var result = await _sut.MovePiece((string?)null);

        // Assert
        result.Should().BeNull();
        _mockEngine.Verify(
            e => e.SendCommandAsync(It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task MovePiece_EmptyString_ReturnsNull()
    {
        // Act
        var result = await _sut.MovePiece(string.Empty);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task MovePiece_ContainsNone_ReturnsNull()
    {
        // Act
        var result = await _sut.MovePiece("(none)");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task MovePiece_ValidNotation_AddsToNotationList()
    {
        // Arrange
        SetupNewGameAndMove();
        await _sut.NewGame();

        // Act
        await _sut.MovePiece("e2e4");

        // Assert
        var notations = _sut.GetNotationList();
        notations.Should().HaveCount(1);
        notations[0].Notation.Should().Be("e2e4");
        notations[0].TurnCount.Should().Be(1);
    }

    [Fact]
    public async Task MovePiece_MultipleValidMoves_BuildsCorrectCommandString()
    {
        // Arrange
        _mockEngine.Setup(e => e.StartEngineAsync()).ReturnsAsync(true);
        _mockEngine.Setup(e => e.SendCommandAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((string?)null);

        var board1 = CreateBoard(StartingTiles);
        var board2 = CreateBoard(MovedTiles);
        var board3 = CreateBoard(
            " | r | n | b | q | k | b | n | r |",
            " | p | p | p |   | p | p | p | p |",
            " |   |   |   |   |   |   |   |   |",
            " |   |   |   | p |   |   |   |   |",
            " |   |   |   |   | P |   |   |   |",
            " |   |   |   |   |   |   |   |   |",
            " | P | P | P | P |   | P | P | P |",
            " | R | N | B | Q | K | B | N | R |"
        );

        _mockEngine.SetupSequence(e => e.GetCurrentBoard())
            .ReturnsAsync(board1)
            .ReturnsAsync(board2)
            .ReturnsAsync(board3);

        await _sut.NewGame();
        await _sut.MovePiece("e2e4");

        // Act
        await _sut.MovePiece("d7d5");

        // Assert
        _mockEngine.Verify(
            e => e.SendCommandAsync(
                "position startpos moves e2e4 d7d5",
                string.Empty),
            Times.Once);
    }

    [Fact]
    public async Task MovePiece_BoardUnchanged_RemovesLastNotationAndReturnsNull()
    {
        // Arrange
        SetupNewGameAndSameBoard();
        await _sut.NewGame();

        // Act
        var result = await _sut.MovePiece("e2e2");

        // Assert
        result.Should().BeNull();
        _sut.GetNotationList().Should().BeEmpty();
    }

    #endregion

    #region :: MovePiece(List<NotationModel>) ::

    [Fact]
    public async Task MovePiece_WithNotationList_OverwritesInternalList()
    {
        // Arrange
        _mockEngine.Setup(e => e.StartEngineAsync()).ReturnsAsync(true);
        _mockEngine.Setup(e => e.SendCommandAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((string?)null);

        _mockEngine.SetupSequence(e => e.GetCurrentBoard())
            .ReturnsAsync(CreateBoard(StartingTiles))
            .ReturnsAsync(CreateBoard(MovedTiles));

        await _sut.NewGame();

        var notations = new List<NotationModel>
        {
            new() { Notation = "e2e4", TurnCount = 1 },
            new() { Notation = "e7e5", TurnCount = 2 }
        };

        // Act
        await _sut.MovePiece(notations);

        // Assert
        _sut.GetNotationList().Should().HaveCount(2);
        _sut.GetNotationList()[0].Notation.Should().Be("e2e4");
        _sut.GetNotationList()[1].Notation.Should().Be("e7e5");
    }

    #endregion

    #region :: ExecuteAIMove ::

    [Fact]
    public async Task ExecuteAIMove_WhenCalled_CallsBestMoveThenMovePiece()
    {
        // Arrange
        _mockEngine.Setup(e => e.StartEngineAsync()).ReturnsAsync(true);
        _mockEngine.Setup(e => e.SendCommandAsync("position startpos", string.Empty))
            .ReturnsAsync((string?)null);
        _mockEngine.Setup(e => e.SendCommandAsync(It.Is<string>(s => s.StartsWith("go depth")), "best"))
            .ReturnsAsync("bestmove e2e4 ponder d7d5");
        _mockEngine.Setup(e => e.SendCommandAsync(It.Is<string>(s => s.StartsWith("position startpos moves")), string.Empty))
            .ReturnsAsync((string?)null);

        _mockEngine.SetupSequence(e => e.GetCurrentBoard())
            .ReturnsAsync(CreateBoard(StartingTiles))
            .ReturnsAsync(CreateBoard(MovedTiles));

        _mockUserSetting.Setup(s => s.GetUserSetting()).Returns(new UserSettingModel());

        await _sut.NewGame();

        // Act
        var result = await _sut.ExecuteAIMove();

        // Assert
        result.Should().NotBeNull();
        _mockEngine.Verify(
            e => e.SendCommandAsync(It.Is<string>(s => s.StartsWith("go depth")), "best"),
            Times.Once);
        _mockEngine.Verify(
            e => e.SendCommandAsync(It.Is<string>(s => s.Contains("moves e2e4")), string.Empty),
            Times.Once);
    }

    [Fact]
    public async Task ExecuteAIMove_WhenCalled_SetsCurrentMoveOnResult()
    {
        // Arrange
        _mockEngine.Setup(e => e.StartEngineAsync()).ReturnsAsync(true);
        _mockEngine.Setup(e => e.SendCommandAsync("position startpos", string.Empty))
            .ReturnsAsync((string?)null);
        _mockEngine.Setup(e => e.SendCommandAsync(It.Is<string>(s => s.StartsWith("go depth")), "best"))
            .ReturnsAsync("bestmove e2e4 ponder d7d5");
        _mockEngine.Setup(e => e.SendCommandAsync(It.Is<string>(s => s.StartsWith("position startpos moves")), string.Empty))
            .ReturnsAsync((string?)null);

        _mockEngine.SetupSequence(e => e.GetCurrentBoard())
            .ReturnsAsync(CreateBoard(StartingTiles))
            .ReturnsAsync(CreateBoard(MovedTiles));

        _mockUserSetting.Setup(s => s.GetUserSetting()).Returns(new UserSettingModel());

        await _sut.NewGame();

        // Act
        var result = await _sut.ExecuteAIMove();

        // Assert
        result.Should().NotBeNull();
        result!.CurrentMove.Should().Be("e2e4");
    }

    #endregion

    #region :: BestMove ::

    [Fact]
    public async Task BestMove_ValidResponse_ParsesBestMoveCorrectly()
    {
        // Arrange
        _mockUserSetting.Setup(s => s.GetUserSetting()).Returns(new UserSettingModel());
        _mockEngine.Setup(e => e.SendCommandAsync("go depth 20", "best"))
            .ReturnsAsync("bestmove e2e4 ponder d7d5");

        // Act
        var result = await _sut.BestMove();

        // Assert
        result.BestMove.Should().Be("e2e4");
        result.CanMove.Should().BeTrue();
    }

    [Fact]
    public async Task BestMove_NullResponse_ReturnsNoneMove()
    {
        // Arrange
        _mockUserSetting.Setup(s => s.GetUserSetting()).Returns(new UserSettingModel());
        _mockEngine.Setup(e => e.SendCommandAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((string?)null);

        // Act
        var result = await _sut.BestMove();

        // Assert
        result.BestMove.Should().Be("(none)");
        result.CanMove.Should().BeFalse();
    }

    #endregion
}
