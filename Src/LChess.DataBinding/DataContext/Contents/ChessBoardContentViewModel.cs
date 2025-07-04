using LChess.Abstract.Service;
using LChess.Abstract.ViewModel;

using LChess.Models.Chess;

using LChess.Util.Enums;

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
        /// 초기화
        ////////////////////////////////////////
        {
            DrawBoard();
        }
	}

    #endregion

    #region :: Services ::

    /// <summary>
    /// 체스게임 관리 서비스
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
	private List<List<ChessBoardUnitModel>>? _chessBoardSource;

	#endregion

	#region :: Methods ::

	/// <summary>
	/// 체스보드 그리기
	/// </summary>
	private async void DrawBoard() => ChessBoardSource = await _chessGameService.CurrentBoard();

	#endregion

	#region :: Commands ::

	/// <summary>
	/// 타일선택
	/// </summary>
	/// <param name="param"> 선택된 타일 모델 </param>
	[RelayCommand]
	private void SelectTile(object param)
	{
		if(param is ChessBoardUnitModel model)
		{

        }
	}

	#endregion
}
