using LChess.Abstract.ViewModel;
using System.Collections.ObjectModel;

namespace LChess.DataBinding.ViewModel.Content;

public partial class ChessBoardContentViewModel : IContentViewModel
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
	private ObservableCollection<ObservableCollection<>>

	#endregion


	#region :: Methods ::

	#endregion

	#region :: Commands ::

	#endregion
}
