using LChess.Abstract.Service;
using LChess.Abstract.ViewModel;

using LChess.Models.Chess;

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
	private ChessBoardModel? _chessBoardSource;

    #endregion

    #region :: Methods ::

    /// <summary>
    /// 보드 데이터 초기화
    /// </summary>
    /// <param name="color"> 기준 색상 </param>
    private async void InitBoardSource(PieceColorType color)
    {
        //보드데이터 없으면 생성
        ChessBoardSource ??= new ChessBoardModel(color);

        // 새 게임 시작
        var unitCodes = await _chessGameService.NewGame();
        ChessBoardSource.ParseCodes(unitCodes);

        // 유저기물이 흑색이면 백색인 AI부터 시작
        if(color == PieceColorType.Black)
        {
            // 1초 대기 (빠른진행을 막기 위함)
            await Task.Delay(1000);

            //AI 턴 처리
            var aiMove = await _chessGameService.ExecuteAIMove();
            ChessBoardSource.ParseCodes(aiMove);
        }
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
		if(param is ChessBoardTileModel model && ChessBoardSource != null)
		{
            // 체스보드 모델에 타일 선택 알려줌.
            ChessBoardSource.SelectTile(model, out string notation);

            // 이동 데이터가 있으면
            if (!string.IsNullOrEmpty(notation))
            {
                ////////////////////////////////////////
                /// 유저 기물이동 처리
                ////////////////////////////////////////
                {
                    var userMove = await _chessGameService.MovePiece(notation);
                    ChessBoardSource.ParseCodes(userMove);
                }


                ////////////////////////////////////////
                /// 1초 대기 (빠른진행을 막기 위함)
                ////////////////////////////////////////
                {
                    await Task.Delay(1000);
                }


                ////////////////////////////////////////
                /// ai 턴 처리
                ////////////////////////////////////////
                {
                    var aiMove = await _chessGameService.ExecuteAIMove();
                    ChessBoardSource.ParseCodes(aiMove);
                }
            }
        }
	}

	#endregion
}
