namespace LChess.Models.Setting.Sub;

/// <summary>
/// 시스템 설정 모델
/// </summary>
public partial class SystemSettingModel : ObservableObject
{
    [ObservableProperty]
    private string _notationSaveDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Notation");
}
