using LChess.Abstract.Service;
using LChess.Abstract.ViewModel;

using LChess.Models.Setting;

using LChess.Util.Enums;
using LChess.ViewModels.Messenger;

namespace LChess.ViewModels.DataContext.Contents;

/// <summary>
/// 사용자설정 화면 뷰모델
/// </summary>
public partial class UserSettingContentViewModel : ObservableRecipient, IContentViewModel
{    
    
    #region :: Constructor ::
    
    /// <summary>
    /// 생성자
    /// </summary>
    public UserSettingContentViewModel(IUserSettingService userSettingService, IPopupWindowService popupWindowService)
    {
        ////////////////////////////////////////
        // 서비스 등록
        ////////////////////////////////////////
        {
            _userSettingService = userSettingService;
            _popupWindowService = popupWindowService;
        }

        ////////////////////////////////////////
        // 사용자 설정 모델 불러오기
        ////////////////////////////////////////
        { 
            UserSetting = userSettingService.GetUserSetting();
        }

        ////////////////////////////////////////
        // 콘텐츠 타입 지정
        ////////////////////////////////////////
        {
            ContentType = LChessContentType.UserSetting;
        }
    }

    #endregion

    #region :: Services ::

    /// <summary>
    /// 사용자설정 핸들링서비스
    /// </summary>
    private readonly IUserSettingService _userSettingService;

    private readonly IPopupWindowService _popupWindowService;

    #endregion

    #region :: Properties ::

    /// <summary>
    /// 사용자설정 모델
    /// </summary>
    [ObservableProperty]
    private UserSettingModel _userSetting;

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
    /// 홈으로 이동
    /// </summary>
    [RelayCommand]
    private void MoveToHome() => WeakReferenceMessenger.Default.Send(new MoveContentMessage(LChessContentType.Home));

    /// <summary>
    /// 기보저장 폴더 선택
    /// </summary>
    [RelayCommand]
    private void SelectNotationSaveDirectory()
    {
        var directory = _popupWindowService.ShowSelectFolderPopup();

        if(!string.IsNullOrEmpty(directory))
        {
            UserSetting.SystemSetting.NotationSaveDirectory = directory;
        }

    }

    /// <summary>
    /// 사용자설정 저장
    /// </summary>
    [RelayCommand]
    private void SaveSetting()
    {
        var result = _userSettingService.SaveUserSettingModel(UserSetting);

        var message = result ? "설정이 저장되었습니다." : "설정 저장에 실패하였습니다.";

        WeakReferenceMessenger.Default.Send(new WindowDimmingMessage(true));

        _popupWindowService.ShowMessagePopup(message, "확인", string.Empty);

        WeakReferenceMessenger.Default.Send(new WindowDimmingMessage(false));
    }

    #endregion

}
