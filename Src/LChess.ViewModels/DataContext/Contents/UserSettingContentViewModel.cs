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
    public UserSettingContentViewModel(IUserSettingService userSettingService)
    {
        ////////////////////////////////////////
        // 서비스 등록
        ////////////////////////////////////////
        {
            _userSettingService = userSettingService;
        }

        ////////////////////////////////////////
        // 사용자 설정 모델 불러오기
        ////////////////////////////////////////
        { 
            UserSettingModel = userSettingService.GetUserSettingModel();
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

    #endregion

    #region :: Properties ::

    /// <summary>
    /// 사용자설정 모델
    /// </summary>
    [ObservableProperty]
    private UserSettingModel _userSettingModel;

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
    /// 사용자설정 저장
    /// </summary>
    [RelayCommand]
    private void SaveSetting() => _userSettingService.SaveUserSettingModel(UserSettingModel);

    #endregion

}
