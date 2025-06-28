using LChess.Abstract.Service;

namespace LChess.Service.Window;

/// <summary>
/// 윈도우 핸들링 서비스 구현부
/// </summary>
public class WindowHandlingService : IWindowHandlingService
{
	#region :: Constructor::

	/// <summary>
	/// 생성자
	/// </summary>
	public WindowHandlingService(IStockfishEngineService stockfishEngineService)
	{
		_stockfishEngineService = stockfishEngineService;
	}

	#endregion

	#region ::  Properties  ::

	/// <summary>
	/// 윈도우 기억
	/// </summary>
	private readonly System.Windows.Window _shellWindow = Application.Current.MainWindow;

	/// <summary>
	/// Sockfish 엔진 서비스
	/// </summary>
	private readonly IStockfishEngineService _stockfishEngineService;

	#endregion

	#region ::  Methods  ::

	/// <summary>
	/// 쉘 윈도우 최대화
	/// </summary>
	public void MaximizeOrRestore() => _shellWindow.WindowState = _shellWindow.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;

	/// <summary>
	/// 쉘 윈도우 숨김
	/// </summary>
	public void Minimize() => _shellWindow.WindowState = WindowState.Minimized;

	/// <summary>
	/// 쉘 윈도우 닫기
	/// </summary>
	public void Close()
	{
		_stockfishEngineService.StopEngine();
		_shellWindow.Close();
	}

	#endregion
}
