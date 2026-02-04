using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using FluentAssertions;
using LChess.Abstract.Service;
using LChess.Models.Result;
using LChess.Models.Setting;
using LChess.Util.Enums;
using LChess.ViewModels.DataContext.Contents;
using LChess.ViewModels.Messenger;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace LChess.Tests.ViewModel;

/// <summary>
/// ChessGameContentViewModel 유닛테스트
/// 커맨드 동작과 기보 저장 로직을 검증한다.
/// </summary>
[Collection("ViewModel")]
public class ChessGameContentViewModelTests : IDisposable
{
    private readonly Mock<IPopupWindowService> _mockPopupService;
    private readonly Mock<IChessGameService> _mockGameService;
    private readonly Mock<IUserSettingService> _mockUserSettingService;
    private readonly Mock<IJsonFileService> _mockJsonFileService;
    private readonly ChessGameContentViewModel _sut;

    /// <summary>
    /// Ioc.Default는 프로세스당 1회만 ConfigureServices 가능.
    /// 이미 구성되었으면 재구성을 건너뛴다.
    /// </summary>
    private static bool _iocConfigured;
    private static readonly object _iocLock = new();

    public ChessGameContentViewModelTests()
    {
        _mockPopupService = new Mock<IPopupWindowService>();
        _mockGameService = new Mock<IChessGameService>();
        _mockUserSettingService = new Mock<IUserSettingService>();
        _mockJsonFileService = new Mock<IJsonFileService>();

        EnsureIocConfigured();

        _sut = new ChessGameContentViewModel(
            _mockPopupService.Object,
            _mockGameService.Object,
            _mockUserSettingService.Object,
            _mockJsonFileService.Object);
    }

    public void Dispose()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
        _sut.UnRegisterMessengers();
    }

    private void EnsureIocConfigured()
    {
        lock (_iocLock)
        {
            if (_iocConfigured) return;

            try
            {
                var services = new ServiceCollection();
                services.AddSingleton(_mockGameService.Object);
                services.AddSingleton(_mockPopupService.Object);
                services.AddTransient<ChessBoardContentViewModel>();
                Ioc.Default.ConfigureServices(services.BuildServiceProvider());
                _iocConfigured = true;
            }
            catch (InvalidOperationException)
            {
                // 이미 구성됨 - 무시
                _iocConfigured = true;
            }
        }
    }

    #region :: SaveNotation (via MoveToHome) ::

    [Fact]
    public void MoveToHome_GameResultNull_NoSaveNotationCall()
    {
        // Arrange - GameResult는 기본 null
        _mockPopupService
            .Setup(p => p.ShowMessagePopup(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new DialogResultModel(true, string.Empty));

        // Act
        _sut.MoveToHomeCommand.Execute(null);

        // Assert - GameResult가 null이므로 SaveNotationIfNeeded에서 조기 반환
        // JsonFileService가 호출되지 않아야 함
        _mockJsonFileService.Verify(
            j => j.SaveJsonProperties(It.IsAny<GameHistoryFileModel>(), It.IsAny<string>()),
            Times.Never);
    }

    [Fact]
    public void SaveNotation_EmptyPath_DoesNotSave()
    {
        // Arrange - GameResult 설정 (EndGameMessage 수신 시뮬레이션)
        var gameResult = new GameResultModel
        {
            Type = EndGameType.CheckMate,
            Winner = PieceColorType.White,
            UserColor = PieceColorType.White
        };

        // EndGameMessage를 보내서 GameResult 설정
        WeakReferenceMessenger.Default.Send(new EndGameMessage(gameResult));

        // MoveToAnalysis에서 SaveNotation 호출 → 빈 경로
        var emptyPathSetting = new UserSettingModel();
        emptyPathSetting.SystemSetting.NotationSaveDirectory = string.Empty;
        _mockUserSettingService.Setup(s => s.GetUserSetting()).Returns(emptyPathSetting);

        _mockPopupService
            .Setup(p => p.ShowMessagePopup(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new DialogResultModel(true, string.Empty));

        // Act - MoveToAnalysis는 SaveNotation을 직접 호출
        _sut.MoveToAnalysisCommand.Execute(null);

        // Assert - 경로가 비어있으므로 저장하지 않음
        _mockJsonFileService.Verify(
            j => j.SaveJsonProperties(It.IsAny<GameHistoryFileModel>(), It.IsAny<string>()),
            Times.Never);
    }

    #endregion

    #region :: MoveToHome ::

    [Fact]
    public void MoveToHome_UserConfirm_SendsMoveContentMessage()
    {
        // Arrange
        MoveContentMessage? receivedMessage = null;
        WeakReferenceMessenger.Default.Register<MoveContentMessage>(this, (s, m) =>
        {
            receivedMessage = m;
        });

        _mockPopupService
            .Setup(p => p.ShowMessagePopup(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new DialogResultModel(true, string.Empty));

        // Act
        _sut.MoveToHomeCommand.Execute(null);

        // Assert
        receivedMessage.Should().NotBeNull();
        receivedMessage!.Value.Should().Be(LChessContentType.Home);
    }

    [Fact]
    public void MoveToHome_UserCancel_NoNavigation()
    {
        // Arrange
        MoveContentMessage? receivedMessage = null;
        WeakReferenceMessenger.Default.Register<MoveContentMessage>(this, (s, m) =>
        {
            receivedMessage = m;
        });

        _mockPopupService
            .Setup(p => p.ShowMessagePopup(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new DialogResultModel(false, string.Empty));

        // Act
        _sut.MoveToHomeCommand.Execute(null);

        // Assert
        receivedMessage.Should().BeNull();
    }

    #endregion

    #region :: Resign ::

    [Fact]
    public void Resign_UserConfirm_SendsEndGameMessageWithResign()
    {
        // Arrange
        EndGameMessage? receivedEndGame = null;

        // Ioc에서 생성된 ChessBoardContentVM의 메신저 해제
        // (RequestGameResultMessage Reply 시 GetNotationList null 방지)
        _sut.ChessBoardContent?.UnRegisterMessengers();

        // RequestGameResultMessage에 Reply할 핸들러 등록
        WeakReferenceMessenger.Default.Register<RequestGameResultMessage>(this, (s, m) =>
        {
            m.Reply(new GameResultModel
            {
                Type = EndGameType.Init,
                Winner = PieceColorType.Black,
                UserColor = PieceColorType.White
            });
        });

        // SUT의 EndGameMessage 핸들러가 팝업을 호출하므로 Mock 설정
        _mockPopupService
            .SetupSequence(p => p.ShowMessagePopup(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new DialogResultModel(true, string.Empty))  // Resign 확인
            .Returns(new DialogResultModel(true, string.Empty)); // EndGame 알림

        // EndGameMessage를 캡처
        WeakReferenceMessenger.Default.Register<EndGameMessage>(this, (s, m) =>
        {
            receivedEndGame = m;
        });

        // Act
        _sut.ResignCommand.Execute(null);

        // Assert
        receivedEndGame.Should().NotBeNull();
        receivedEndGame!.Value.Type.Should().Be(EndGameType.Resign);
    }

    [Fact]
    public void Resign_UserCancel_NoEndGameMessage()
    {
        // Arrange
        EndGameMessage? receivedEndGame = null;
        WeakReferenceMessenger.Default.Register<EndGameMessage>(this, (s, m) =>
        {
            receivedEndGame = m;
        });

        _mockPopupService
            .Setup(p => p.ShowMessagePopup(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new DialogResultModel(false, string.Empty));

        // Act
        _sut.ResignCommand.Execute(null);

        // Assert
        receivedEndGame.Should().BeNull();
    }

    #endregion

    #region :: NewGame ::

    [Fact]
    public void NewGame_UserConfirm_NavigatesToChoicePieceColor()
    {
        // Arrange
        MoveContentMessage? receivedMessage = null;
        WeakReferenceMessenger.Default.Register<MoveContentMessage>(this, (s, m) =>
        {
            receivedMessage = m;
        });

        _mockPopupService
            .Setup(p => p.ShowMessagePopup(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new DialogResultModel(true, string.Empty));

        // Act
        _sut.NewGameCommand.Execute(null);

        // Assert
        receivedMessage.Should().NotBeNull();
        receivedMessage!.Value.Should().Be(LChessContentType.ChoicePieceColor);
    }

    #endregion
}
