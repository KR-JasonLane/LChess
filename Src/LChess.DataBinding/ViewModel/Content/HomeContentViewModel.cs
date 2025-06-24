using LChess.Abstract.Service;
using LChess.Abstract.ViewModel;

using LChess.DataBinding.Messenger;

namespace LChess.DataBinding.ViewModel.Content;

/// <summary>
/// 홈 컨텐츠 뷰모델
/// </summary>
public partial class HomeContentViewModel : ObservableRecipient, IContentViewModel
{
	#region :: Constructor ::

	/// <summary>
	/// 생성자
	/// </summary>
	public HomeContentViewModel(IStockfishEngineService stockfishEngineService)
	{
		ContentType = LChessContentType.Home;

		_stockfishEngineService = stockfishEngineService;
	}

	#endregion

	#region :: Services ::

	#endregion

	#region :: Properties ::

	/// <summary>
	/// Content Type 지정
	/// </summary>
	public LChessContentType ContentType { get; init; }

	private readonly IStockfishEngineService _stockfishEngineService;

	#endregion

	#region :: Methods ::

	#endregion

	#region :: Commands ::

	/// <summary>
	/// 체스 게임으로 이동
	/// </summary>
	[RelayCommand]
	private void MoveToChessGame()
	{
		_stockfishEngineService.StartEngineAsync(1000);

		WeakReferenceMessenger.Default.Send(new MoveContentMessage(LChessContentType.ChessGame));
	}

	#endregion

}
