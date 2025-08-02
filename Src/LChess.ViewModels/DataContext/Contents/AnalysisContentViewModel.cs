using LChess.Abstract.Service;
using LChess.Abstract.ViewModel;

using LChess.Models.Chess.Board;
using LChess.Models.Chess.Match;
using LChess.Models.Result;

using LChess.Util.Enums;

using LChess.ViewModels.Messenger;

namespace LChess.ViewModels.DataContext.Contents;

/// <summary>
/// 기보분석 콘텐츠 뷰모델
/// </summary>
public partial class AnalysisContentViewModel : ObservableRecipient, IContentViewModel
{
    #region :: Constructor ::

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="popupWindowService"></param>
    public AnalysisContentViewModel(IPopupWindowService popupWindowService)
    {
        ////////////////////////////////////////
        // 서비스 등록
        ////////////////////////////////////////
        {
            _popupWindowService = popupWindowService;
        }

        ////////////////////////////////////////
        // 멤버 초기화
        ////////////////////////////////////////
        {
            ContentType = LChessContentType.ChoiceNotation;
        }


        ////////////////////////////////////////
        /// 체스보드 Content 뷰모델 생성
        ////////////////////////////////////////
        {
            ChessBoardContent = Ioc.Default.GetService<ChessBoardContentViewModel>();
        }


        ////////////////////////////////////////
        /// 메신저 등록
        ////////////////////////////////////////
        {
            WeakReferenceMessenger.Default.Register<GameHistoryMessage>(this, (s, m) =>
            {
                CurrentAnalysisFile = m.Value;

                //기보 분석 시작 메시지
                WeakReferenceMessenger.Default.Send(new InitBoardMessage(new ChessBoardInitPropertyModel(ChessBoardMode.Analysis, CurrentAnalysisFile.GameResult.UserColor, null, new TimeSpan(0))));
            });
        }
    }

    #endregion

    #region :: Services ::

    /// <summary>
    /// 팝업윈도우 핸들링 서비스
    /// </summary>
    private readonly IPopupWindowService _popupWindowService;

    #endregion

    #region :: Properties ::

    /// <summary>
    /// 콘텐츠 타입
    /// </summary>
    public LChessContentType ContentType { get; init; }

    /// <summary>
    /// 체스보드 
    /// </summary>
    [ObservableProperty]
    private IContentViewModel? _chessBoardContent;

    /// <summary>
    /// 현재 분석중인 파일 기억
    /// </summary>
    [ObservableProperty]
    private GameHistoryFileModel _currentAnalysisFile = new();

    private List<NotationModel> _lastestNotationList = new();

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
    /// 선택한 기보까지 이동
    /// </summary>
    /// <param name="param"> 기보 데이터 </param>
    [RelayCommand]
    private void MoveNotation(object param)
    {
        if(param is NotationModel notation)
        {
            var result = new List<NotationModel>();

            foreach (var item in CurrentAnalysisFile.GameResult.Notation)
            {
                result.Add(item);

                if (item == notation) break;
            }

            WeakReferenceMessenger.Default.Send(new ForceAnalysisMoveMessage(result));

            //리스트를 메시지로 보내는 순간 ChessGameService의 _notation객체가 되므로,
            //새로운 객체를 만들어 줘야함.
            _lastestNotationList = new(result);
        }
    }

    /// <summary>
    /// 현재 보드 상태로 게임 이동
    /// </summary>
    [RelayCommand]
    private void MoveToGame()
    {
        if (CurrentAnalysisFile == null) return;

        //딤 켜기
        WeakReferenceMessenger.Default.Send(new WindowDimmingMessage(true));

        var result = _popupWindowService.ShowMessagePopup("현재 단계부터 이어서 게임을 진행할까요?", "네", "아니요");

        //딤 끄기
        WeakReferenceMessenger.Default.Send(new WindowDimmingMessage(false));

        if (result?.ButtonResult != true) return;

        var gameResult = CurrentAnalysisFile.GameResult;

        //1. 딤 켜기
        WeakReferenceMessenger.Default.Send(new WindowDimmingMessage(true));

        //2. 체스 게임 컨텐츠로 이동
        WeakReferenceMessenger.Default.Send(new MoveContentMessage(LChessContentType.ChessGame));

        //3. 보드 초기화 메시지
        WeakReferenceMessenger.Default.Send(new InitBoardMessage(new ChessBoardInitPropertyModel(ChessBoardMode.Game, gameResult.UserColor, _lastestNotationList, gameResult.PlayTime)));

        //4. 딤 끄기
        WeakReferenceMessenger.Default.Send(new WindowDimmingMessage(false));
    }

    #endregion
}
