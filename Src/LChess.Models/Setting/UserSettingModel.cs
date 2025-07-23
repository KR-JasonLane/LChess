using LChess.Models.Setting.Sub;

namespace LChess.Models.Setting;

/// <summary>
/// 유저 환결설정 모델
/// </summary>
public partial class UserSettingModel : ObservableObject
{
    /// <summary>
    /// stockfish 설정 모델
    /// </summary>
    [ObservableProperty]
    private StockfishSettingModel _stockfishSetting = new();

    /// <summary>
    /// 시스템설정 모델
    /// </summary>
    [ObservableProperty]
    private SystemSettingModel _systemSetting = new();
}
