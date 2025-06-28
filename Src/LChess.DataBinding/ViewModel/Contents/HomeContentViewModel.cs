using LChess.Abstract.Service;
using LChess.Abstract.ViewModel;

using LChess.Util.Enums;

using LChess.ViewModels.Messenger;

namespace LChess.ViewModels.Contents;

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
	private async Task MoveToChessGame()
	{
		WeakReferenceMessenger.Default.Send(new WindowDimmingMessage(true));

		await Task.Run(() => _stockfishEngineService.StartEngineAsync());

		WeakReferenceMessenger.Default.Send(new MoveContentMessage(LChessContentType.ChessGame));

		WeakReferenceMessenger.Default.Send(new WindowDimmingMessage(false));
	}

	#endregion

}
