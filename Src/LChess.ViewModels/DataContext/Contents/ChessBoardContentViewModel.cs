using LChess.Abstract.Service;
using LChess.Abstract.ViewModel;

using LChess.Models.Chess;
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
	public ChessBoardContentViewModel(IChessGameService chessGameService)
    {
        ////////////////////////////////////////
        /// 서비스 등록
        ////////////////////////////////////////
        {
            _chessGameService = chessGameService;
        }


        ////////////////////////////////////////
        /// 타입 지정
        ////////////////////////////////////////
        {
            ContentType = LChessContentType.ChessBoard;
        }

        ////////////////////////////////////////
        /// 메시지 등록
        ////////////////////////////////////////
        {
            WeakReferenceMessenger.Default.Register<SelectUserPieceColorMessage>(this, (s, m) =>
            {
                InitBoardSource(m.Value);
            });

            WeakReferenceMessenger.Default.Register<EndGameMessage>(this, (s, m) =>
            {
                BoardModel?.GameEnd();
                _isEndGame = true;
            });
        }
	}

    #endregion

    #region :: Services ::

    /// <summary>
    /// Stockfish 엔전 관리 서비스
    /// </summary>
    private readonly IChessGameService _chessGameService;

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

    #endregion

    #region :: Methods ::

    /// <summary>
    /// 보드 데이터 초기화
    /// </summary>
    /// <param name="userColor"> 기준 색상 </param>
    private async void InitBoardSource(PieceColorType userColor)
    {
        //보드데이터 없으면 생성
        BoardModel ??= new ChessBoardModel(userColor);

        // 새 게임 시작
        var unitCodes = await _chessGameService.NewGame();
        BoardModel.ParseCodes(unitCodes);

        //백색부터 시작
        CurrentTurn = PieceColorType.White;

        // 유저기물이 흑색이면 백색인 AI부터 시작
        if(userColor == PieceColorType.Black)
        {
            WeakReferenceMessenger.Default.Send(new WindowDimmingMessage(true));

            // 1초 대기 (빠른진행을 막기 위함)
            await Task.Delay(1000);

            //AI 턴 처리
            var aiMove = await _chessGameService.ExecuteAIMove();
            BoardModel.ParseCodes(aiMove);

            WeakReferenceMessenger.Default.Send(new WindowDimmingMessage(false));
        }

        _isEndGame = false;
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

            WeakReferenceMessenger.Default.Send(new EndGameMessage(new GameResultModel(EndGameType.Draw, CurrentTurn)));

            return true;
        }

        //체크메이트 처리
        if (moveResult.IsCheck && !bestMoveResult.CanMove)
        {
            Log.Information("======================= 게임종료 [체크메이트] =======================");

            WeakReferenceMessenger.Default.Send(new EndGameMessage(new GameResultModel(EndGameType.CheckMate, CurrentTurn)));

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

            //TODO : 여기서 승격이 필요하면 승격처리 로직을 추가해야함.
            // 승격처리 후 q, n, b, r 등등 유닛코드를 notation 뒤에 넣어줘야함.

            //기보
            var notation = $"{result.Notation}";

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

                    var bestMoveResult = await _chessGameService.BestMove();
                    CheckEndGame(aiMoveResult, bestMoveResult);

                }
            }
        }
	}

    #endregion

    #region :: Events ::

    /// <summary>
    /// 현재 차례 변경 이벤트
    /// </summary>
    /// <param name="value"> 변경된 색상 </param>
    partial void OnCurrentTurnChanged(PieceColorType value) => WeakReferenceMessenger.Default.Send(new CurrentTurnChangedMessage(value));

    #endregion
}
