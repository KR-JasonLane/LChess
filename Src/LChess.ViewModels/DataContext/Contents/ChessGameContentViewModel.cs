using LChess.Abstract.Service;
using LChess.Abstract.ViewModel;
using LChess.Models.Result;
using LChess.Util.Enums;

using LChess.ViewModels.Messenger;

namespace LChess.ViewModels.DataContext.Contents;

/// <summary>
/// 체스게임 Content 뷰모델
/// </summary>
public partial class ChessGameContentViewModel : ObservableRecipient, IContentViewModel
{
	#region :: Constructor ::

	public ChessGameContentViewModel(IPopupWindowService popupWindowService, IChessGameService chessGameService)
    {
        ////////////////////////////////////////
        /// 서비스 등록
        ////////////////////////////////////////
        {
            _popupWindowService = popupWindowService;
        }


        ////////////////////////////////////////
        /// 타입 지정
        ////////////////////////////////////////
        {
            ContentType = LChessContentType.ChessGame;
        }


        ////////////////////////////////////////
        /// 초기화
        ////////////////////////////////////////
        {
            //CurrentTurn = string.Empty;
        }


        ////////////////////////////////////////
        /// 체스보드 Content 뷰모델 생성
        ////////////////////////////////////////
        {
            ChessBoardContent = Ioc.Default.GetService<ChessBoardContentViewModel>();
		}


        ////////////////////////////////////////
        /// 메신저 구독
        ////////////////////////////////////////
        {
            WeakReferenceMessenger.Default.Register<EndGameMessage>(this, (s, m) =>
            {
                WeakReferenceMessenger.Default.Send(new WindowDimmingMessage(true));

                var endType = m.Value.Type switch
                {
                    EndGameType.CheckMate => "체크메이트! "   ,
                    EndGameType.Resign    => "기권! "        ,
                    EndGameType.Draw      => "무승부 입니다.",
                    _ => string.Empty
                };

                var winner = m.Value.Winner switch
                {
                    PieceColorType.Black => "흑이 승리했습니다.",
                    PieceColorType.White => "백이 승리했습니다.",
                    _ => string.Empty
                };

                _popupWindowService.ShowMessagePopup($"{endType}{(m.Value.Type != EndGameType.Draw ? winner : string.Empty)}", "확인", string.Empty);

                WeakReferenceMessenger.Default.Send(new WindowDimmingMessage(false));
            });

            WeakReferenceMessenger.Default.Register<CurrentTurnChangedMessage>(this, (s, m) =>
            {
                CurrentTurn = m.Value;
            });
        }
    }

    #endregion

    #region :: Services ::

    /// <summary>
    /// 팝업윈도우 서비스
    /// </summary>
    private readonly IPopupWindowService _popupWindowService;

    #endregion

    #region :: Properties ::

    /// <summary>
    /// Content Type 지정
    /// </summary>
    public LChessContentType ContentType { get; init; }

	/// <summary>
	/// 체스보드 
	/// </summary>
	[ObservableProperty]
	private IContentViewModel? _chessBoardContent;

    [ObservableProperty]
    private PieceColorType _currentTurn;

    #endregion

    #region :: Methods ::


    /// <summary>
    /// 메신저 구독해제
    /// </summary>
    public void UnRegisterMessengers()
    {
        ChessBoardContent?.UnRegisterMessengers();
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    #endregion

    #region :: Commands ::

    /// <summary>
    /// 홈으로 이동
    /// </summary>
    [RelayCommand]
    private void MoveToHome() => WeakReferenceMessenger.Default.Send(new MoveContentMessage(LChessContentType.Home));

    /// <summary>
    /// 기권
    /// </summary>
    [RelayCommand]
    private void Resign()
    {
        var winner = CurrentTurn == PieceColorType.White ? PieceColorType.Black : PieceColorType.White;

        WeakReferenceMessenger.Default.Send(new EndGameMessage(new GameResultModel(EndGameType.Resign, winner)));
    }

    #endregion
}
