using LChess.Abstract.Service;
using LChess.Abstract.ViewModel;

using LChess.Models.Chess;

using LChess.Util.Enums;

namespace LChess.ViewModels.Contents;

public partial class ChessBoardContentViewModel : ObservableRecipient, IContentViewModel
{
	#region :: Constructor ::

	/// <summary>
	/// 생성자
	/// </summary>
	public ChessBoardContentViewModel(IChessGameService chessGameService)
	{
		ContentType = LChessContentType.ChessBoard;

		_chessGameService = chessGameService;

		DrawBoard();
	}

	#endregion

	#region :: Services ::

	#endregion

	#region :: Properties ::

	/// <summary>
	/// Content Type 지정
	/// </summary>
	public LChessContentType ContentType { get; init; }

	/// <summary>
	/// 체스게임 관리 서비스
	/// </summary>
	private readonly IChessGameService _chessGameService;

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
	private async void DrawBoard() => ChessBoardSource = await _chessGameService.DrawBoard();

	#endregion

	#region :: Commands ::

	#endregion
}
