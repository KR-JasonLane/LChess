using LChess.Abstract.Model;
using LChess.Abstract.ViewModel;
using LChess.Util.Enums;

namespace LChess.DataBinding.ViewModel.Content;

public partial class ChessBoardContentViewModel : ObservableRecipient, IContentViewModel
{
	#region :: Constructor ::

	/// <summary>
	/// 생성자
	/// </summary>
	public ChessBoardContentViewModel()
	{
		ContentType = LChessContentType.ChessBoard;
	}

	#endregion

	#region :: Services ::

	#endregion

	#region :: Properties ::

	/// <summary>
	/// Content Type 지정
	/// </summary>
	public LChessContentType ContentType { get; init; }

	[ObservableProperty]
	private List<List<IChessBoardUnitModel>>? _chessBoardSource;

	#endregion


	#region :: Methods ::

	#endregion

	#region :: Commands ::

	#endregion
}
