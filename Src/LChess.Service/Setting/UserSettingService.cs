using LChess.Abstract.Service;
using LChess.Models.Setting;

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
    public UserSettingService()
    {

    }

    #endregion

    #region :: Properties ::

    #endregion


    #region :: Methods ::

    /// <summary>
    /// 사용자 설정 모델을 불러옴.
    /// </summary>
    /// <returns> 사용자설정 모델 </returns>
    public UserSettingModel GetUserSettingModel()
    {
        return new();
    }

    /// <summary>
    /// 사용자 설정을 저장
    /// </summary>
    /// <param name="userSettingModel"> 저장할 모델 </param>
    /// <returns> 저장 성공 여부 </returns>
    public bool SaveUserSettingModel(UserSettingModel userSettingModel)
    {
        return true;
    }

    #endregion

    #region :: Commands ::

    #endregion

}
