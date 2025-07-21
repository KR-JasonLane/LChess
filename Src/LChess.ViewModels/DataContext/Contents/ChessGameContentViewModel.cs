using LChess.Abstract.Service;
using LChess.Abstract.ViewModel;
using LChess.Models.Result;
using LChess.Util.Enums;

using LChess.ViewModels.Messenger;
using System.Collections.ObjectModel;

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
            History = new();
            IsEndGame = false;
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

                IsEndGame = true;
                Winner = m.Value.Winner;

                var endType = m.Value.Type switch
                {
                    EndGameType.CheckMate => "체크메이트! "   ,
                    EndGameType.Resign    => "기권! "        ,
                    EndGameType.Draw      => "무승부 입니다.",
                    _ => string.Empty
                };

                var winner = Winner switch
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

            WeakReferenceMessenger.Default.Register<MatchStatusMessage>(this, (s, m) =>
            {
                var statusModel = m.Value;

                CurrentTurn = statusModel.NextTurn;

                var currentTurnString = statusModel.CurrentTurn == PieceColorType.White ? "백" : "흑";

                var notation = statusModel.Notation;

                var history = $"{History.Count + 1}. {currentTurnString} : {notation.Substring(0, 2)} → {notation.Substring(2, 2)}";

                History.Add(history);
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

    /// <summary>
    /// 현재턴
    /// </summary>
    [ObservableProperty]
    private PieceColorType _currentTurn;

    /// <summary>
    /// 승자
    /// </summary>
    [ObservableProperty]
    private PieceColorType _winner;

    /// <summary>
    /// 이동 이력
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<string> _history;

    /// <summary>
    /// 현재 게임이 종료되었는지 여부
    /// </summary>
    [ObservableProperty]
    private bool _isEndGame;

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

    /// <summary>
    /// 기보를 저장할건지 여부에 따라 기보 저장
    /// </summary>
    private void SaveNotationIfNeeded()
    {
        WeakReferenceMessenger.Default.Send(new WindowDimmingMessage(true));

        var result = _popupWindowService.ShowMessagePopup("기보를 저장할까요?", "예", "아니요");

        if (result?.ButtonResult == true)
        {
            //TODO : 기보 저장
        }

        WeakReferenceMessenger.Default.Send(new WindowDimmingMessage(false));
    }

    #endregion

    #region :: Commands ::

    /// <summary>
    /// 홈으로 이동
    /// </summary>
    [RelayCommand]
    private void MoveToHome()
    {
        //게임이 끝났을 경우
        if(IsEndGame)
        {
            //기보를 저장할건지 여부를 물은뒤
            SaveNotationIfNeeded();

            //홈으로 이동
            WeakReferenceMessenger.Default.Send(new MoveContentMessage(LChessContentType.Home));

            return;
        }

        //Dim On
        WeakReferenceMessenger.Default.Send(new WindowDimmingMessage(true));

        //홈으로 이동할건지 입력받음
        var result = _popupWindowService.ShowMessagePopup("게임을 종료하고 홈으로 이동할까요?", "예", "아니요");

        //Dim Off
        WeakReferenceMessenger.Default.Send(new WindowDimmingMessage(false));

        // '예'를 선택했으면 홈으로 이동
        if (result?.ButtonResult == true)
            WeakReferenceMessenger.Default.Send(new MoveContentMessage(LChessContentType.Home));
    }

    /// <summary>
    /// 기권
    /// </summary>
    [RelayCommand]
    private void Resign()
    {
        //Dim On
        WeakReferenceMessenger.Default.Send(new WindowDimmingMessage(true));

        //기권여부 판단
        var result = _popupWindowService.ShowMessagePopup("기권할까요?", "예", "아니요");

        //'예'선택 시
        if(result?.ButtonResult == true)
        {
            //승자 설정
            Winner = CurrentTurn == PieceColorType.White ? PieceColorType.Black : PieceColorType.White;

            //게임종료 로깅
            Log.Information("======================= 게임종료 [기권] =======================");

            //게임종료 메시지
            WeakReferenceMessenger.Default.Send(new EndGameMessage(new GameResultModel(EndGameType.Resign, Winner)));
        }

        //Dim Off
        WeakReferenceMessenger.Default.Send(new WindowDimmingMessage(false));
    }

    /// <summary>
    /// 새게임
    /// </summary>
    [RelayCommand]
    private void NewGame()
    {
        //기보저장
        SaveNotationIfNeeded();

        //컬러선택으로 이동
        WeakReferenceMessenger.Default.Send(new MoveContentMessage(LChessContentType.ChoicePieceColor));
    }

    #endregion
}
