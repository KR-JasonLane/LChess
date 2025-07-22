using LChess.Util.Enums;

namespace LChess.Models.Result;

/// <summary>
/// 게임결과 모델
/// </summary>
public partial class GameResultModel : ObservableObject
{
    #region :: Constructor ::

    /// <summary>
    /// 생성자
    /// </summary>
    public GameResultModel()
    {
        Notation = new();
    }

    #endregion

    #region :: Properties ::

    /// <summary>
    /// 경기 날짜
    /// </summary>
    [ObservableProperty]
    private DateTime _playDateTime;

    /// <summary>
    /// 경기시간
    /// </summary>
    [ObservableProperty]
    private TimeSpan _playTime;

    /// <summary>
    /// 게임종료 타입
    /// </summary>
    [ObservableProperty]
    private EndGameType _type;

    /// <summary>
    /// 승자
    /// </summary>
    [ObservableProperty]
    private PieceColorType? _winner;

    /// <summary>
    /// 기보
    /// </summary>
    [ObservableProperty]
    private List<string> _notation;

    #endregion
}
