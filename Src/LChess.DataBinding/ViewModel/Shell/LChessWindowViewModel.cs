using LChess.Abstract.ViewModel;
using LChess.DataBinding.ViewModel.Content;

namespace LChess.DataBinding.Shell;

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
		CurrentContent = Ioc.Default.GetService<HomeContentViewModel>();
	}

	#endregion

	#region :: Services ::

	#endregion

	#region :: Properties ::

	/// <summary>
	/// 현재 Content
	/// </summary>
	[ObservableProperty]
	IContentViewModel? _currentContent;

	#endregion


	#region :: Methods ::

	#endregion

	#region :: Commands ::

	#endregion
}
