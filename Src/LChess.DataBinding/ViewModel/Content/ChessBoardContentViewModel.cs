using LChess.Abstract.ViewModel;
using LChess.DataBinding.Model;
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
	private ObservableCollection<ObservableCollection<ChessBoardUnitModel>>? _chessBoardSource;

	#endregion


	#region :: Methods ::

	#endregion

	#region :: Commands ::

	#endregion
}
