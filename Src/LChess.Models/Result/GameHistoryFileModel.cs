namespace LChess.Models.Result;

/// <summary>
/// 게임결과 파일 모델
/// </summary>
public partial class GameHistoryFileModel : ObservableObject
{
    #region :: Constructor ::

    /// <summary>
    /// 생성자
    /// </summary>
    public GameHistoryFileModel()
    {

    }

    #endregion

    #region :: Properties ::

    /// <summary>
    /// 파일이름
    /// </summary>
    [ObservableProperty]
    private string _fileName = string.Empty;

    /// <summary>
    /// 게임결과
    /// </summary>
    [ObservableProperty]
    private GameResultModel _gameResult = new();

    #endregion
}
