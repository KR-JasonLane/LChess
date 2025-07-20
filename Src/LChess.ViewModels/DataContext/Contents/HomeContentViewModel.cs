using LChess.Abstract.ViewModel;

using LChess.Util.Enums;

using LChess.ViewModels.Messenger;

namespace LChess.ViewModels.DataContext.Contents;

/// <summary>
/// 홈 Content 뷰모델
/// </summary>
public partial class HomeContentViewModel : ObservableRecipient, IContentViewModel
{
	#region :: Constructor ::

	/// <summary>
	/// 생성자
	/// </summary>
	public HomeContentViewModel()
	{
        ////////////////////////////////////////
        /// 타입 지정
        ////////////////////////////////////////
        {
            ContentType = LChessContentType.Home;
        }
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

    /// <summary>
    /// 메신저 구독해제
    /// </summary>
    public void UnRegisterMessengers()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    #endregion

    #region :: Commands ::

    /// <summary>
    /// 체스 게임으로 이동
    /// </summary>
    [RelayCommand]
	private void MoveToChoicePieceColor() => WeakReferenceMessenger.Default.Send(new MoveContentMessage(LChessContentType.ChoicePieceColor));

    /// <summary>
    /// 체스 게임으로 이동
    /// </summary>
    [RelayCommand]
    private void MoveToUserSetting() => WeakReferenceMessenger.Default.Send(new MoveContentMessage(LChessContentType.UserSetting));
    #endregion

}
