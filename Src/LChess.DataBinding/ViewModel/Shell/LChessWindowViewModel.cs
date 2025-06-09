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
	public LChessWindowViewModel()
	{
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
		}
	}

	#endregion

	#region :: Services ::

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

	#endregion
}
