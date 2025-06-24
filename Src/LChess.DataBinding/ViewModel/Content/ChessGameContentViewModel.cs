using LChess.Abstract.ViewModel;

namespace LChess.DataBinding.ViewModel.Content;
public class ChessGameContentViewModel : ObservableRecipient, IContentViewModel
{
	#region :: Constructor ::

	public ChessGameContentViewModel()
	{
		ContentType = LChessContentType.ChessGame;
	}

	#endregion

	#region :: Services ::

	#endregion

	#region :: Properties ::

	/// <summary>
	/// Content Type 지정
	/// </summary>
	public LChessContentType ContentType { get; init; }

	#endregion


	#region :: Methods ::

	#endregion

	#region :: Commands ::

	#endregion
}
