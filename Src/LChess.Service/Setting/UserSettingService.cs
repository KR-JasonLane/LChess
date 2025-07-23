using LChess.Abstract.Service;
using LChess.Models.Setting;

using LChess.Service.Json;

namespace LChess.Service.Setting;

/// <summary>
/// 유저 환경설정 서비스 구현부
/// </summary>
public class UserSettingService : IUserSettingService
{
    #region :: Constructor ::

    /// <summary>
    /// 생성자
    /// </summary>
    public UserSettingService(IJsonFileService jsonFileService)
    {
        _jsonFileService = jsonFileService;

        _settingFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Setting", "LChessSetting.config");
    }

    #endregion

    #region :: Properties ::

    private readonly string _settingFilePath;

    private readonly IJsonFileService _jsonFileService;

    #endregion


    #region :: Methods ::

    /// <summary>
    /// 사용자 설정 모델을 불러옴.
    /// </summary>
    /// <returns> 사용자설정 모델 </returns>
    public UserSettingModel GetUserSetting()
    {
        _jsonFileService.TryParseJsonProperties(_settingFilePath, out UserSettingModel? result);

        return result ?? new();
    }

    /// <summary>
    /// 사용자 설정을 저장
    /// </summary>
    /// <param name="userSettingModel"> 저장할 모델 </param>
    /// <returns> 저장 성공 여부 </returns>
    public bool SaveUserSettingModel(UserSettingModel userSettingModel)
    {
        return _jsonFileService.SaveJsonProperties(userSettingModel, _settingFilePath);
    }

    #endregion

    #region :: Commands ::

    #endregion

}
