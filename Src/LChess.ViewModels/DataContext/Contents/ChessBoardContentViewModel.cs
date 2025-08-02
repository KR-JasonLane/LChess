using LChess.Abstract.Service;
using LChess.Abstract.ViewModel;

using LChess.Models.Chess;
using LChess.Models.Chess.Board;
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
            _playTimeCorrection = new TimeSpan(0);
        }


        ////////////////////////////////////////
        /// 메시지 등록
        ////////////////////////////////////////
        {
            WeakReferenceMessenger.Default.Register<InitBoardMessage>(this, (s, m) =>
            {
                InitBoard(m.Value);
            });

            WeakReferenceMessenger.Default.Register<EndGameMessage>(this, (s, m) =>
            {
                BoardModel?.GameEnd();
                _currentMode = ChessBoardMode.Analysis;
            });

            WeakReferenceMessenger.Default.Register<RequestGameResultMessage>(this, (s, m) =>
            {
                var winner = CurrentTurn == PieceColorType.White ? PieceColorType.Black : PieceColorType.White;

                m.Reply(CreateGameResult(EndGameType.Init, winner));
            });

            WeakReferenceMessenger.Default.Register<ForceAnalysisMoveMessage>(this, (s, m) =>
            {
                ForceMoveForAnalysis(m.Value);
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
    /// 현재 턴 저장
    /// </summary>
    [ObservableProperty]
    private PieceColorType _currentTurn;

    /// <summary>
    /// 플레이타임을 재는 스탑워치
    /// </summary>
    private Stopwatch _playTimeWatch;

    /// <summary>
    /// 현재 체스보드 모드
    /// </summary>
    private ChessBoardMode _currentMode;

    /// <summary>
    /// 플레이타임 보정값(이어하기)
    /// </summary>
    private TimeSpan _playTimeCorrection;

    #endregion

    #region :: Methods ::

    /// <summary>
    /// 보드 초기화
    /// </summary>
    /// <param name="initPropertyModel"> 보드 초기화 모델 </param>
    /// <returns></returns>
    private async void InitBoard(ChessBoardInitPropertyModel initPropertyModel)
    {
        _currentMode = initPropertyModel.Mode;

        var initNotations = initPropertyModel.InitialNotations;

        if (initNotations != null && initNotations.Any())
        {
            await InitForceMove(initPropertyModel.UserColor, initNotations);

            return;
        }

        await InitWithoutForceMove(initPropertyModel.UserColor);
    }

    /// <summary>
    /// 강제이동과 함께 보드 초기화
    /// </summary>
    /// <param name="userColor"> 유저 색상 </param>
    /// <param name="notations"> 이동기보 </param>
    private async Task InitForceMove(PieceColorType userColor, List<NotationModel> notations)
    {
        await CreateBoardDataForceMove(userColor, notations);

        if (_currentMode == ChessBoardMode.Game)
        {
            await StartContine(notations);
        }
    }

    /// <summary>
    /// 강제이동없이 보드 초기화
    /// </summary>
    /// <param name="userColor"> 유저색상 </param>
    private async Task InitWithoutForceMove(PieceColorType userColor)
    {
        await CreateBoardDataWithoutForceMove(userColor);

        if (_currentMode == ChessBoardMode.Game)
        {
            await StartNewGameWithoutNotation();
        }
    }

    /// <summary>
    /// 보드 데이터생성
    /// </summary>
    /// <param name="userColor"></param>
    /// <returns></returns>
    private async Task CreateBoardDataWithoutForceMove(PieceColorType userColor)
    {
        //보드데이터 없으면 생성
        BoardModel ??= new ChessBoardModel(userColor);

        // 새 게임 시작
        StockfishBoardCodeModel? unitCodes = await _chessGameService.NewGame();

        //파싱
        BoardModel.ParseCodes(unitCodes);
    }

    /// <summary>
    /// 강제이동을 적용한 보드 데이터 생성
    /// </summary>
    /// <param name="userColor"> 유저 색상 </param>
    /// <param name="notations"> 이동 기보 </param>
    private async Task CreateBoardDataForceMove(PieceColorType userColor, List<NotationModel> notations)
    {
        //보드데이터 없으면 생성
        BoardModel ??= new ChessBoardModel(userColor);

        //맵 초기화 후
        await _chessGameService.NewGame();

        //강제이동 기보 적용
        StockfishBoardCodeModel? unitCodes = await _chessGameService.MovePiece(notations);

        //파싱
        BoardModel.ParseCodes(unitCodes);
    }

    /// <summary>
    /// 보드 데이터 초기화 후 새 게임 시작
    /// </summary>
    private async Task StartNewGameWithoutNotation()
    {
        if (BoardModel == null) return;

        _playTimeWatch = Stopwatch.StartNew();

        //기물이동이 없었기 때문에, Color는 null을 넣어준다(초기값)
        NotifyMatchStatus(new MatchStatusModel(_chessGameService.GetNotationList(), null, false));

        // 유저기물이 흑색이면 백색인 AI부터 시작
        if (BoardModel.UserColor == PieceColorType.Black) await ProcessInitialAiTurn();
    }

    /// <summary>
    /// 이어하기
    /// </summary>
    /// <param name="notations"> 이동 기보 </param>
    private async Task StartContine(List<NotationModel> notations)
    {
        if (BoardModel == null) return;

        _playTimeWatch = Stopwatch.StartNew();

        var lastTurn = notations.Count % 2 == 0 ? PieceColorType.Black : PieceColorType.White;

        //마지막 기물 이동 정보를 알려준다.
        NotifyMatchStatus(new MatchStatusModel(_chessGameService.GetNotationList(), lastTurn, false));

        if (BoardModel.UserColor == lastTurn) await ProcessInitialAiTurn();
    }

    private async Task ProcessInitialAiTurn()
    {
        if (BoardModel == null) return;

        WeakReferenceMessenger.Default.Send(new WindowDimmingMessage(true));

        // 1초 대기 (빠른진행을 막기 위함)
        await Task.Delay(1000);

        //AI 턴 처리
        var aiMove = await _chessGameService.ExecuteAIMove();
        BoardModel.ParseCodes(aiMove);

        NotifyMatchStatus(new MatchStatusModel(_chessGameService.GetNotationList(), CurrentTurn, false));

        WeakReferenceMessenger.Default.Send(new WindowDimmingMessage(false));
    }

    /// <summary>
    /// 분석을위한 강제이동 처리
    /// </summary>
    /// <param name="notation"> 이동기보 </param>
    private async void ForceMoveForAnalysis(List<NotationModel> notation)
    {
        if (BoardModel == null || _currentMode != ChessBoardMode.Analysis) return;

        WeakReferenceMessenger.Default.Send(new WindowDimmingMessage(true));

        var moveResult = await _chessGameService.MovePiece(notation);
        BoardModel.ParseCodes(moveResult);

        WeakReferenceMessenger.Default.Send(new WindowDimmingMessage(false));
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
    /// <param name="winner"> 승자 </param>
    private GameResultModel CreateGameResult(EndGameType type, PieceColorType? winner)
    {
        _playTimeWatch.Stop();

        return new GameResultModel()
        {
            Type = type,
            Winner = winner,
            Notation = new ObservableCollection<NotationModel>(_chessGameService.GetNotationList()),
            UserColor = BoardModel?.UserColor ?? PieceColorType.White,
            PlayTime = _playTimeWatch.Elapsed + _playTimeCorrection,
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
        if(param is ChessBoardTileModel model && BoardModel != null)
        {
            // 체스보드 모델에 타일 선택 알려줌.
            var result = BoardModel.SelectTile(model, _currentMode == ChessBoardMode.Analysis);

            var promotion = string.Empty;

            //승격여부 판단
            if (result.IsNeedToPromotion)
                promotion = _popupWindowService.ShowSelectPromotionPopup();

            //기보
            var notation = $"{result.Notation}{promotion}";

            // 이동 데이터가 있으면
            if (!string.IsNullOrEmpty(notation) && _currentMode == ChessBoardMode.Game)
            {
                ////////////////////////////////////////
                /// 유저 기물이동 처리
                ////////////////////////////////////////
                {
                    //유저 턴 설정
                    CurrentTurn = BoardModel.UserColor;

                    var userMoveResult = await _chessGameService.MovePiece(notation);
                    BoardModel.ParseCodes(userMoveResult);

                    if (userMoveResult == null)
                        return;

                    NotifyMatchStatus(new MatchStatusModel(_chessGameService.GetNotationList(), CurrentTurn, userMoveResult.IsCheck));

                    var bestMoveResult = await _chessGameService.BestMove();
                    if (CheckEndGame(userMoveResult, bestMoveResult)) return;
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

                //유저 턴 설정
                CurrentTurn = BoardModel.UserColor;
            }
        }
    }

    #endregion

    #region :: Events ::

    #endregion
}
