using LChess.Abstract.Service;
using LChess.Abstract.ViewModel;

using LChess.Models.Chess.Match;
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

    public ChessGameContentViewModel(IPopupWindowService popupWindowService, IChessGameService chessGameService, IUserSettingService userSettingService, IJsonFileService jsonFileService)
    {
        ////////////////////////////////////////
        /// 서비스 등록
        ////////////////////////////////////////
        {
            _popupWindowService = popupWindowService;
            _userSettingService = userSettingService;
            _jsonFileService    = jsonFileService   ;
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
            GameResult = null;

            MatchStatus = new MatchStatusModel(new(), PieceColorType.White, false);
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

                GameResult = m.Value;

                var endType = GameResult.Type switch
                {
                    EndGameType.CheckMate => "체크메이트! "   ,
                    EndGameType.Resign    => "기권! "        ,
                    EndGameType.Draw      => "무승부 입니다.",
                    _ => string.Empty
                };

                var winner = GameResult.Winner switch
                {
                    PieceColorType.Black => "흑이 승리했습니다.",
                    PieceColorType.White => "백이 승리했습니다.",
                    _ => string.Empty
                };

                _popupWindowService.ShowMessagePopup($"{endType}{(m.Value.Type != EndGameType.Draw ? winner : string.Empty)}", "확인", string.Empty);

                WeakReferenceMessenger.Default.Send(new WindowDimmingMessage(false));
            });

            WeakReferenceMessenger.Default.Register<MatchStatusMessage>(this, (s, m) =>
            {
                MatchStatus = m.Value;
            });
        }
    }

    #endregion

    #region :: Services ::

    /// <summary>
    /// 팝업윈도우 서비스
    /// </summary>
    private readonly IPopupWindowService _popupWindowService;

    /// <summary>
    /// 유저설정 서비스
    /// </summary>
    private readonly IUserSettingService _userSettingService;

    /// <summary>
    /// Json 파일 서비스
    /// </summary>
    private readonly IJsonFileService _jsonFileService;

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
    /// 매치상태를 알려줌.
    /// (기물 이동기준이기 때문에, NextTurn이 현재 진행해야하는 턴이다.)
    /// </summary>
    [ObservableProperty]
    private MatchStatusModel _matchStatus;

    /// <summary>
    /// 게임 결과 모델
    /// </summary>
    [ObservableProperty]
    private GameResultModel? _gameResult;

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
        if (GameResult == null) return;

        var result = _popupWindowService.ShowMessagePopup("기보를 저장할까요?", "예", "아니요");
        
        if (result?.ButtonResult == true)
        {
            SaveNotation();
        }
    }

    /// <summary>
    /// 기보저장
    /// </summary>
    /// <returns> 저장 된 모델 반환 </returns>
    private GameHistoryFileModel? SaveNotation()
    {
        if (GameResult == null) return null;

        var setting = _userSettingService.GetUserSetting();
        var path = setting.SystemSetting.NotationSaveDirectory;

        if (!string.IsNullOrEmpty(path))
        {
            var gameHistoryFile = new GameHistoryFileModel()
            {
                FileName = $"{GameResult.PlayDateTime:yyyy년MM월dd일HH시mm분}_{GameResult.PlayTime.Minutes}분{GameResult.PlayTime.Seconds}초경기.chess",
                GameResult = GameResult,
            };

            _jsonFileService.SaveJsonProperties(gameHistoryFile, Path.Combine(path, gameHistoryFile.FileName));

            return gameHistoryFile;
        }

        return null;
    }

    #endregion

    #region :: Commands ::

    /// <summary>
    /// 홈으로 이동
    /// </summary>
    [RelayCommand]
    private void MoveToHome()
    {
        //Dim On
        WeakReferenceMessenger.Default.Send(new WindowDimmingMessage(true));

        //홈으로 이동할건지 입력받음
        var result = _popupWindowService.ShowMessagePopup("게임을 종료하고 홈으로 이동할까요?", "예", "아니요");

        // '예'를 선택
        if (result?.ButtonResult == true)
        {
            //게임이 끝났으면 기보저장 여부 판단 후
            if (GameResult != null) SaveNotationIfNeeded();

            //홈으로 이동
            WeakReferenceMessenger.Default.Send(new MoveContentMessage(LChessContentType.Home));
        }

        //Dim Off
        WeakReferenceMessenger.Default.Send(new WindowDimmingMessage(false));
    }

    /// <summary>
    /// 분석화면 이동
    /// </summary>
    [RelayCommand]
    private void MoveToAnalysis()
    {
        //Dim On
        WeakReferenceMessenger.Default.Send(new WindowDimmingMessage(true));

        //분석 페이지로 이동할건지 여부 확인
        var result = _popupWindowService.ShowMessagePopup("현재 게임을 저장하고 분석 화면으로 이동합니다.", "예", "아니요");

        if(result?.ButtonResult == true)
        {
            var historyModel = SaveNotation();

            if(historyModel != null)
            {
                //분석화면 이동
                WeakReferenceMessenger.Default.Send(new MoveContentMessage(LChessContentType.Analysis));

                //게임결과 전송
                WeakReferenceMessenger.Default.Send(new GameHistoryMessage(historyModel));
            }
        }

        //Dim Off
        WeakReferenceMessenger.Default.Send(new WindowDimmingMessage(false));
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
            //게임종료 로깅
            Log.Information("======================= 게임종료 [기권] =======================");

            var requestMessage = new RequestGameResultMessage();

            //게임결과 요청
            WeakReferenceMessenger.Default.Send(requestMessage);

            //게임결과 받아오기
            var gameResult = requestMessage.Response;

            //게임 종료 타입 변경
            gameResult.Type = EndGameType.Resign;

            //게임종료 메시지
            WeakReferenceMessenger.Default.Send(new EndGameMessage(gameResult));
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
        //Dim On
        WeakReferenceMessenger.Default.Send(new WindowDimmingMessage(true));

        //새게임 할건지 입력받음
        var result = _popupWindowService.ShowMessagePopup("새로운 게임으로 이동 할까요?", "예", "아니요");

        // '예'를 선택
        if (result?.ButtonResult == true)
        {
            //게임이 끝났으면 기보저장 여부 판단 후
            if (GameResult != null) SaveNotationIfNeeded();

            //컬러선택으로 이동
            WeakReferenceMessenger.Default.Send(new MoveContentMessage(LChessContentType.ChoicePieceColor));
        }

        //Dim Off
        WeakReferenceMessenger.Default.Send(new WindowDimmingMessage(false));
    }

    #endregion
}
