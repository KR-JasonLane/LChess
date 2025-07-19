namespace LChess.Models.Setting.Sub;

/// <summary>
/// 스톡피쉬 설정 모델
/// </summary>
public partial class StockfishSettingModel : ObservableObject
{
    #region :: Constructor ::

    /// <summary>
    /// 생성자
    /// </summary>
    public StockfishSettingModel()
    {

    }

    #endregion

    #region :: Properties ::

    /// <summary>
    /// 스톡피쉬 판단 깊이 (클수록 난이도가 높고 오래걸림)
    /// </summary>
    [ObservableProperty]
    private string _stockfishTinkingDepth = "10";


    #endregion
}
