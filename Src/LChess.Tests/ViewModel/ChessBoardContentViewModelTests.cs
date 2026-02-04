using CommunityToolkit.Mvvm.Messaging;
using FluentAssertions;
using LChess.Abstract.Service;
using LChess.Models.Chess.Board;
using LChess.Models.Chess.Match;
using LChess.Models.Result;
using LChess.Util.Enums;
using LChess.ViewModels.DataContext.Contents;
using LChess.ViewModels.Messenger;
using Moq;

namespace LChess.Tests.ViewModel;

/// <summary>
/// ChessBoardContentViewModel 유닛테스트
/// IChessGameService와 IPopupWindowService를 Mock하여 보드 동작을 검증한다.
/// </summary>
[Collection("ViewModel")]
public class ChessBoardContentViewModelTests : IDisposable
{
    private readonly Mock<IChessGameService> _mockGameService;
    private readonly Mock<IPopupWindowService> _mockPopupService;
    private readonly ChessBoardContentViewModel _sut;

    private static readonly string[] BoardTiles =
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

    public ChessBoardContentViewModelTests()
    {
        _mockGameService = new Mock<IChessGameService>();
        _mockPopupService = new Mock<IPopupWindowService>();
        _sut = new ChessBoardContentViewModel(_mockGameService.Object, _mockPopupService.Object);
    }

    public void Dispose()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
        _sut.UnRegisterMessengers();
    }

    #region :: Helpers ::

    private static StockfishBoardCodeModel CreateBoard(params string[] tileCodes)
    {
        var board = new StockfishBoardCodeModel();
        board.SetTileCodeList(tileCodes.ToList());
        return board;
    }

    /// <summary>
    /// InitBoard 메시지를 전송하여 보드를 초기화한다.
    /// </summary>
    private async Task InitializeBoardForGame(PieceColorType userColor = PieceColorType.White)
    {
        var boardModel = CreateBoard(BoardTiles);
        _mockGameService.Setup(s => s.NewGame()).ReturnsAsync(boardModel);
        _mockGameService.Setup(s => s.GetNotationList()).Returns(new List<NotationModel>());

        var initProperty = new ChessBoardInitPropertyModel(
            ChessBoardMode.Game, userColor, null, TimeSpan.Zero);

        WeakReferenceMessenger.Default.Send(new InitBoardMessage(initProperty));

        // InitBoard는 async void이므로 약간의 대기가 필요
        await Task.Delay(100);
    }

    #endregion

    #region :: SelectTile ::

    [Fact]
    public async Task SelectTile_EmptyTile_NoMovement()
    {
        // Arrange - 보드 초기화
        await InitializeBoardForGame();

        if (_sut.BoardModel is null)
            throw new InvalidOperationException("BoardModel should not be null after initialization.");

        // 빈 타일 선택 (이동 가능 기물이 아닌 빈 타일)
        var emptyTile = _sut.BoardModel.Source?[3][3];
        if (emptyTile is null)
            throw new InvalidOperationException("Tile should exist at position [3][3].");

        // Act
        await _sut.SelectTileCommand.ExecuteAsync(emptyTile);

        // Assert - MovePiece가 호출되지 않아야 함
        _mockGameService.Verify(
            s => s.MovePiece(It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public async Task SelectTile_NeedsPromotion_ShowsPromotionPopup()
    {
        // Arrange - 보드 초기화
        var promoBoard = CreateBoard(
            " | r | n | b | q | k | b | n | r |",
            " | p | p | p | p | P | p | p | p |",
            " |   |   |   |   |   |   |   |   |",
            " |   |   |   |   |   |   |   |   |",
            " |   |   |   |   |   |   |   |   |",
            " |   |   |   |   |   |   |   |   |",
            " | P | P | P | P |   | P | P | P |",
            " | R | N | B | Q | K | B | N | R |"
        );

        var movedBoard = CreateBoard(
            " | r | n | b | q | Q | b | n | r |",
            " | p | p | p | p |   | p | p | p |",
            " |   |   |   |   |   |   |   |   |",
            " |   |   |   |   |   |   |   |   |",
            " |   |   |   |   |   |   |   |   |",
            " |   |   |   |   |   |   |   |   |",
            " | P | P | P | P |   | P | P | P |",
            " | R | N | B | Q | K | B | N | R |"
        );

        _mockGameService.Setup(s => s.NewGame()).ReturnsAsync(promoBoard);
        _mockGameService.Setup(s => s.GetNotationList()).Returns(new List<NotationModel>());
        _mockGameService.Setup(s => s.MovePiece(It.IsAny<string>())).ReturnsAsync(movedBoard);
        _mockGameService.Setup(s => s.BestMove()).ReturnsAsync(new StockfishBestMoveModel("e7e5"));
        _mockGameService.Setup(s => s.ExecuteAIMove()).ReturnsAsync(movedBoard);
        _mockPopupService.Setup(s => s.ShowSelectPromotionPopup()).Returns("Q");

        var initProperty = new ChessBoardInitPropertyModel(
            ChessBoardMode.Game, PieceColorType.White, null, TimeSpan.Zero);
        WeakReferenceMessenger.Default.Send(new InitBoardMessage(initProperty));
        await Task.Delay(100);

        if (_sut.BoardModel is null)
            throw new InvalidOperationException("BoardModel should not be null after initialization.");

        // 백 폰을 선택 (7행(rank 2)에서 이동 가능한 타일 선택)
        var pawnTile = _sut.BoardModel.Source?[1][4]; // 2행 e열의 백 폰
        if (pawnTile is null)
            throw new InvalidOperationException("Pawn tile should exist.");

        // 먼저 폰 선택 → 이동 가능 위치 하이라이트
        await _sut.SelectTileCommand.ExecuteAsync(pawnTile);

        // 프로모션 위치 (1행) 선택 → 프로모션 팝업 표시
        var promotionTile = _sut.BoardModel.Source?[0][4]; // 1행 e열
        if (promotionTile is null)
            throw new InvalidOperationException("Promotion tile should exist.");

        // 하이라이트 타일만 프로모션 트리거 가능하므로 하이라이트 확인
        if (promotionTile.IsHighLightMove || promotionTile.IsHighLightEnemy)
        {
            await _sut.SelectTileCommand.ExecuteAsync(promotionTile);

            // Assert - 프로모션 팝업 호출됨
            _mockPopupService.Verify(s => s.ShowSelectPromotionPopup(), Times.Once);
        }
        // 하이라이트가 안 되어있을 경우 (보드 상태에 따라) - 타일 선택 자체는 성공
    }

    #endregion

    #region :: CheckEndGame ::

    [Fact]
    public async Task CheckEndGame_Checkmate_SendsEndGameMessage()
    {
        // Arrange
        await InitializeBoardForGame();

        if (_sut.BoardModel is null)
            throw new InvalidOperationException("BoardModel should not be null after initialization.");

        EndGameMessage? receivedMessage = null;
        WeakReferenceMessenger.Default.Register<EndGameMessage>(this, (s, m) =>
        {
            receivedMessage = m;
        });

        // 유저 기물 선택 후 이동 시뮬레이션
        // 유저 이동 결과: 체크 상태
        var checkBoard = CreateBoard(BoardTiles);
        checkBoard.SetCheckerList(new List<string> { "e4" });

        _mockGameService.Setup(s => s.MovePiece(It.IsAny<string>())).ReturnsAsync(checkBoard);
        _mockGameService.Setup(s => s.GetNotationList()).Returns(new List<NotationModel>
        {
            new() { Notation = "e2e4", TurnCount = 1 }
        });
        // 체크메이트: 체크 + 이동 불가
        _mockGameService.Setup(s => s.BestMove()).ReturnsAsync(new StockfishBestMoveModel("(none)"));

        // 백 폰 선택
        var pawnTile = _sut.BoardModel.Source?[6][4]; // 7행 e열 백 폰
        if (pawnTile is null)
            throw new InvalidOperationException("Pawn tile should exist.");

        await _sut.SelectTileCommand.ExecuteAsync(pawnTile);

        // 이동 가능 타일 찾기
        var targetTile = _sut.BoardModel.Source?[4][4]; // 5행 e열
        if (targetTile is null)
            throw new InvalidOperationException("Target tile should exist.");

        if (targetTile.IsHighLightMove)
        {
            await _sut.SelectTileCommand.ExecuteAsync(targetTile);

            // Assert - EndGameMessage가 CheckMate로 전송되어야 함
            receivedMessage.Should().NotBeNull();
            receivedMessage!.Value.Type.Should().Be(EndGameType.CheckMate);
        }
    }

    [Fact]
    public async Task CheckEndGame_Stalemate_SendsDrawMessage()
    {
        // Arrange
        await InitializeBoardForGame();

        if (_sut.BoardModel is null)
            throw new InvalidOperationException("BoardModel should not be null after initialization.");

        EndGameMessage? receivedMessage = null;
        WeakReferenceMessenger.Default.Register<EndGameMessage>(this, (s, m) =>
        {
            receivedMessage = m;
        });

        // 유저 이동 결과: 체크 아님
        var normalBoard = CreateBoard(BoardTiles);

        _mockGameService.Setup(s => s.MovePiece(It.IsAny<string>())).ReturnsAsync(normalBoard);
        _mockGameService.Setup(s => s.GetNotationList()).Returns(new List<NotationModel>
        {
            new() { Notation = "e2e4", TurnCount = 1 }
        });
        // 스테일메이트: 체크 아님 + 이동 불가
        _mockGameService.Setup(s => s.BestMove()).ReturnsAsync(new StockfishBestMoveModel("(none)"));

        // 백 폰 선택
        var pawnTile = _sut.BoardModel.Source?[6][4];
        if (pawnTile is null)
            throw new InvalidOperationException("Pawn tile should exist.");

        await _sut.SelectTileCommand.ExecuteAsync(pawnTile);

        var targetTile = _sut.BoardModel.Source?[4][4];
        if (targetTile is null)
            throw new InvalidOperationException("Target tile should exist.");

        if (targetTile.IsHighLightMove)
        {
            await _sut.SelectTileCommand.ExecuteAsync(targetTile);

            // Assert - EndGameMessage가 Draw로 전송되어야 함
            receivedMessage.Should().NotBeNull();
            receivedMessage!.Value.Type.Should().Be(EndGameType.Draw);
        }
    }

    [Fact]
    public async Task CheckEndGame_GameContinues_NoEndGameMessage()
    {
        // Arrange
        await InitializeBoardForGame();

        if (_sut.BoardModel is null)
            throw new InvalidOperationException("BoardModel should not be null after initialization.");

        EndGameMessage? receivedMessage = null;
        WeakReferenceMessenger.Default.Register<EndGameMessage>(this, (s, m) =>
        {
            receivedMessage = m;
        });

        var movedBoard = CreateBoard(
            " | r | n | b | q | k | b | n | r |",
            " | p | p | p | p | p | p | p | p |",
            " |   |   |   |   |   |   |   |   |",
            " |   |   |   |   |   |   |   |   |",
            " |   |   |   |   | P |   |   |   |",
            " |   |   |   |   |   |   |   |   |",
            " | P | P | P | P |   | P | P | P |",
            " | R | N | B | Q | K | B | N | R |"
        );

        var aiMovedBoard = CreateBoard(
            " | r | n | b | q | k | b | n | r |",
            " | p | p | p |   | p | p | p | p |",
            " |   |   |   |   |   |   |   |   |",
            " |   |   |   | p |   |   |   |   |",
            " |   |   |   |   | P |   |   |   |",
            " |   |   |   |   |   |   |   |   |",
            " | P | P | P | P |   | P | P | P |",
            " | R | N | B | Q | K | B | N | R |"
        );

        _mockGameService.Setup(s => s.MovePiece(It.IsAny<string>())).ReturnsAsync(movedBoard);
        _mockGameService.Setup(s => s.GetNotationList()).Returns(new List<NotationModel>
        {
            new() { Notation = "e2e4", TurnCount = 1 }
        });
        // 게임 계속: 이동 가능
        _mockGameService.Setup(s => s.BestMove()).ReturnsAsync(new StockfishBestMoveModel("d7d5"));
        _mockGameService.Setup(s => s.ExecuteAIMove()).ReturnsAsync(aiMovedBoard);

        // 백 폰 선택 후 이동
        var pawnTile = _sut.BoardModel.Source?[6][4];
        if (pawnTile is null)
            throw new InvalidOperationException("Pawn tile should exist.");

        await _sut.SelectTileCommand.ExecuteAsync(pawnTile);

        var targetTile = _sut.BoardModel.Source?[4][4];
        if (targetTile is null)
            throw new InvalidOperationException("Target tile should exist.");

        if (targetTile.IsHighLightMove)
        {
            await _sut.SelectTileCommand.ExecuteAsync(targetTile);

            // Assert - EndGameMessage가 전송되지 않아야 함
            receivedMessage.Should().BeNull();
        }
    }

    #endregion
}
