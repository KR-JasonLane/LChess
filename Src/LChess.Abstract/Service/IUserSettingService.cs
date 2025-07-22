using LChess.Models.Setting;

namespace LChess.Abstract.Service;

/// <summary>
/// 사용자 설정 서비스 추상화 인터페이스
/// </summary>
public interface IUserSettingService
{
    /// <summary>
    /// 사용자 설정 모델을 불러옴.
    /// </summary>
    /// <returns> 사용자설정 모델 </returns>
    public UserSettingModel GetUserSettingModel();
    
    /// <summary>
    /// 사용자 설정을 저장
    /// </summary>
    /// <param name="userSettingModel"> 저장할 모델 </param>
    /// <returns> 저장 성공 여부 </returns>
    public bool SaveUserSettingModel(UserSettingModel userSettingModel);
}
