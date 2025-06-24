using LChess.Abstract.Service;
using LChess.Abstract.ViewModel;

using LChess.DataBinding.Messenger;
using LChess.DataBinding.ViewModel.Content;


namespace LChess.DataBinding.ViewModel.Shell;

/// <summary>
/// LChess 윈도우 뷰모델
/// </summary>
public partial class LChessWindowViewModel : ObservableRecipient, ILChessWindowViewModel
{
	#region :: Constructor ::

	/// <summary>
	/// 생성자
	/// </summary>
	public LChessWindowViewModel(IWindowHandlingService windowHandlingService)
	{ 
		////////////////////////////////////////
		/// 서비스
		////////////////////////////////////////
		{
			_windowHandlingService = windowHandlingService;
		}

		////////////////////////////////////////
		/// Content 초기화
		////////////////////////////////////////
		{
			CurrentContent = Ioc.Default.GetService<HomeContentViewModel>();
		}


		////////////////////////////////////////
		/// 메신저 등록
		////////////////////////////////////////
		{
			// 윈도우 Dim 처리 메시지
			WeakReferenceMessenger.Default.Register<WindowDimmingMessage>(this, (r, m) =>
			{
				IsVisibleDimming = m.Value;
			});

			// Content 이동 메시지
			WeakReferenceMessenger.Default.Register<MoveContentMessage>(this, (r, m) =>
			{
				if (CurrentContent?.ContentType == m.Value) return;

				IsVisibleDimming = true;

				CurrentContent = m.Value switch
				{
					LChessContentType.Home => Ioc.Default.GetService<HomeContentViewModel>(),
					LChessContentType.ChessGame => Ioc.Default.GetService<ChessGameContentViewModel>(),
					//LChessContentType.Settings  => Ioc.Default.GetService<SettingContentViewModel>(),
					_ => null
				};

				IsVisibleDimming = false;
			});
		}
	}

	#endregion

	#region :: Services ::

	/// <summary>
	/// 윈도우 핸들링 서비스
	/// </summary>
	private readonly IWindowHandlingService _windowHandlingService;

	#endregion

	#region :: Properties ::

	/// <summary>
	/// 현재 Content
	/// </summary>
	[ObservableProperty]
	private IContentViewModel? _currentContent;

	/// <summary>
	/// 윈도우 Dim처리
	/// </summary>
	[ObservableProperty]
	private bool _isVisibleDimming;

	#endregion

	#region :: Methods ::

	#endregion

	#region :: Commands ::

	/// <summary>
	/// 윈도우 숨김
	/// </summary>
	[RelayCommand]
	private void WindowMinimize() => _windowHandlingService.Minimize();

	/// <summary>
	/// 윈도우 최대화
	/// </summary>
	[RelayCommand]
	private void WindowMaximize() => _windowHandlingService.MaximizeOrRestore();

	/// <summary>
	/// 윈도우 닫기
	/// </summary>
	[RelayCommand]
	private void WindowClose() => _windowHandlingService.Close();

	#endregion
}
