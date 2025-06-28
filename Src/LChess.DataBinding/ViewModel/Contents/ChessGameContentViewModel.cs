using LChess.Abstract.ViewModel;

using LChess.Util.Enums;

namespace LChess.ViewModels.Contents;
public partial class ChessGameContentViewModel : ObservableRecipient, IContentViewModel
{
	#region :: Constructor ::

	public ChessGameContentViewModel()
	{
		ContentType = LChessContentType.ChessGame;

		ChessBoardContent = Ioc.Default.GetService<ChessBoardContentViewModel>();
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
	/// 체스보드 
	/// </summary>
	[ObservableProperty]
	private IContentViewModel? _chessBoardContent;

	#endregion


	#region :: Methods ::

	#endregion

	#region :: Commands ::

	#endregion
}
