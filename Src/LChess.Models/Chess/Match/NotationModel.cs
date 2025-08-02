using LChess.Util.Enums;

namespace LChess.Models.Chess.Match;

/// <summary>
/// 기보 모델
/// </summary>
public class NotationModel
{
    #region :: Constructor ::

    /// <summary>
    /// 생성자
    /// </summary>
    public NotationModel()
    {

    }

    #endregion

    #region :: Properties ::

    /// <summary>
    /// 기보문자열
    /// </summary>
    public string Notation { get; set; } = string.Empty;

    /// <summary>
    /// 순서 번호
    /// </summary>
    public int TurnCount { get; set; }

    /// <summary>
    /// 비어있는지 여부
    /// </summary>
    public bool IsEmpty => string.IsNullOrEmpty(Notation);

    #endregion
}
