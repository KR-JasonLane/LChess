using LChess.Abstract.Service;
using LChess.Abstract.ViewModel;

using LChess.Models.Chess;
using LChess.Models.Chess.Match;
using LChess.Models.Result;

using LChess.Util.Enums;

using LChess.ViewModels.Messenger;

namespace LChess.ViewModels.DataContext.Contents;

/// <summary>
/// 체스보드 Content 뷰모델
/// </summary>
public partial class ChessBoardContentViewModel : ObservableRecipient, IContentViewModel
{
    #region :: Constructor ::

    /// <summary>
    /// 생성자
    /// </summary>
    public ChessBoardContentViewModel(IChessGameService chessGameService, IPopupWindowService popupWindowService)
    {
        ////////////////////////////////////////
        /// 서비스 등록
        ////////////////////////////////////////
        {
            _chessGameService   = chessGameService;
            _popupWindowService = popupWindowService;
        }


        ////////////////////////////////////////
        /// 타입 지정
        ////////////////////////////////////////
        {
            ContentType = LChessContentType.ChessBoard;
        }


        ////////////////////////////////////////
        /// 초기화
        ////////////////////////////////////////
        {
            _playTimeWatch = new();
        }


        ////////////////////////////////////////
        /// 메시지 등록
        ////////////////////////////////////////
        {
            WeakReferenceMessenger.Default.Register<SelectUserPieceColorMessage>(this, (s, m) =>
            {
                StartNewGame(m.Value);
            });

            WeakReferenceMessenger.Default.Register<EndGameMessage>(this, (s, m) =>
            {
                BoardModel?.GameEnd();
                _isEndGame = true;
            });

            WeakReferenceMessenger.Default.Register<RequestPlayTimeMessage>(this, (s, m) =>
            {
                _playTimeWatch.Stop();

                m.Reply(_playTimeWatch.Elapsed);
            });
        }
    }

    #endregion

    #region :: Services ::

    /// <summary>
    /// 체스 게임 서비스
    /// </summary>
    private readonly IChessGameService _chessGameService;

    /// <summary>
    /// PopupWindow 서비스
    /// </summary>
    private readonly IPopupWindowService _popupWindowService;

    #endregion

    #region :: Properties ::

    /// <summary>
    /// Content Type 지정
    /// </summary>
    public LChessContentType ContentType { get; init; }

    /// <summary>
    /// 체스보드 데이터 소스
    /// </summary>
    [ObservableProperty]
    private ChessBoardModel? _boardModel;

    /// <summary>
    /// 현재 게임이 종료되었는지 확인
    /// </summary>
    private bool _isEndGame;

    /// <summary>
    /// 현재 턴 저장
    /// </summary>
    [ObservableProperty]
    private PieceColorType _currentTurn;

    /// <summary>
    /// 플레이타임을 재는 스탑워치
    /// </summary>
    private Stopwatch _playTimeWatch;

    #endregion

    #region :: Methods ::

    /// <summary>
    /// 보드 데이터 초기화
    /// </summary>
    /// <param name="userColor"> 기준 색상 </param>
    private async void StartNewGame(PieceColorType userColor)
    {
        //보드데이터 없으면 생성
        BoardModel ??= new ChessBoardModel(userColor);

        // 새 게임 시작
        var unitCodes = await _chessGameService.NewGame();
        BoardModel.ParseCodes(unitCodes);

        //기물이동이 없었기 때문에, Color는 null을 넣어준다(초기값)
        NotifyMatchStatus(new MatchStatusModel(_chessGameService.GetNotationList(), null, false));

        // 유저기물이 흑색이면 백색인 AI부터 시작
        if (userColor == PieceColorType.Black)
        {
            WeakReferenceMessenger.Default.Send(new WindowDimmingMessage(true));

            // 1초 대기 (빠른진행을 막기 위함)
            await Task.Delay(1000);

            //AI 턴 처리
            var aiMove = await _chessGameService.ExecuteAIMove();
            BoardModel.ParseCodes(aiMove);

            NotifyMatchStatus(new MatchStatusModel(_chessGameService.GetNotationList(), CurrentTurn, false));

            WeakReferenceMessenger.Default.Send(new WindowDimmingMessage(false));
        }

        _isEndGame = false;

        _playTimeWatch = Stopwatch.StartNew();
    }

    /// <summary>
    /// 게임종료상태 검사
    /// </summary>
    /// <param name="moveResult"> 기물움직임 결과 </param>
    /// <returns> 게임종료 여부 </returns>
    private bool CheckEndGame(StockfishBoardCodeModel? moveResult, StockfishBestMoveModel bestMoveResult)
    {
        if (moveResult == null) return true;

        //무승부 처리
        if (!moveResult.IsCheck && !bestMoveResult.CanMove)
        {
            Log.Information("======================= 게임종료 [무승부] =======================");

            WeakReferenceMessenger.Default.Send(new EndGameMessage(CreateGameResult(EndGameType.Draw, null)));

            return true;
        }

        //체크메이트 처리
        if (moveResult.IsCheck && !bestMoveResult.CanMove)
        {
            Log.Information("======================= 게임종료 [체크메이트] =======================");

            WeakReferenceMessenger.Default.Send(new EndGameMessage(CreateGameResult(EndGameType.CheckMate, CurrentTurn)));

            return true;
        }

        return false;
    }

    /// <summary>
    /// 메신저 구독해제
    /// </summary>
    public void UnRegisterMessengers()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    /// <summary>
    /// 매치상태 전송
    /// </summary>
    public void NotifyMatchStatus(MatchStatusModel model) => WeakReferenceMessenger.Default.Send(new MatchStatusMessage(model));

    /// <summary>
    /// 게임결과 전송
    /// </summary>
    /// <param name="type"> 게임종료 타입 </param>
    /// <param name="Winner"> 승자 </param>
    private GameResultModel CreateGameResult(EndGameType type, PieceColorType? Winner)
    {
        _playTimeWatch.Stop();

        return new GameResultModel()
        {
            Type = EndGameType.Draw,
            Winner = null,
            Notation = _chessGameService.GetNotationList(),
            PlayTime = _playTimeWatch.Elapsed,
            PlayDateTime = DateTime.Now
        };
    }

    #endregion

    #region :: Commands ::

    /// <summary>
    /// 타일선택
    /// </summary>
    /// <param name="param"> 선택된 타일 모델 </param>
    [RelayCommand]
    private async Task SelectTile(object param)
    {
        if(param is ChessBoardTileModel model && BoardModel != null && !_isEndGame)
        {
            // 체스보드 모델에 타일 선택 알려줌.
            var result = BoardModel.SelectTile(model);

            var promotion = string.Empty;

            //승격여부 판단
            if (result.IsNeedToPromotion)
                promotion = _popupWindowService.ShowSelectPromotionPopup();

            //기보
            var notation = $"{result.Notation}{promotion}";

            //유저 턴 설정
            CurrentTurn = BoardModel.UserColor;

            // 이동 데이터가 있으면
            if (!string.IsNullOrEmpty(notation))
            {
                ////////////////////////////////////////
                /// 유저 기물이동 처리
                ////////////////////////////////////////
                {
                    var userMoveResult = await _chessGameService.MovePiece(notation);
                    BoardModel.ParseCodes(userMoveResult);

                    if (userMoveResult == null)
                        return;

                    NotifyMatchStatus(new MatchStatusModel(_chessGameService.GetNotationList(), CurrentTurn, userMoveResult.IsCheck));

                    var bestMoveResult = await _chessGameService.BestMove();
                    if (CheckEndGame(userMoveResult, bestMoveResult)) return;
                }


                ////////////////////////////////////////
                /// 0.5초 대기 (빠른진행을 막기 위함)
                ////////////////////////////////////////
                {
                    await Task.Delay(500);
                }


                ////////////////////////////////////////
                /// ai 턴 처리
                ////////////////////////////////////////
                {
                    CurrentTurn = BoardModel.EnemyColor;

                    WeakReferenceMessenger.Default.Send(new WindowDimmingMessage(true));

                    var aiMoveResult = await _chessGameService.ExecuteAIMove();
                    BoardModel.ParseCodes(aiMoveResult);

                    WeakReferenceMessenger.Default.Send(new WindowDimmingMessage(false));

                    NotifyMatchStatus(new MatchStatusModel(_chessGameService.GetNotationList(), CurrentTurn, aiMoveResult?.IsCheck ?? false));

                    var bestMoveResult = await _chessGameService.BestMove();
                    CheckEndGame(aiMoveResult, bestMoveResult);
                }
            }
        }
    }

    #endregion

    #region :: Events ::

    #endregion
}
