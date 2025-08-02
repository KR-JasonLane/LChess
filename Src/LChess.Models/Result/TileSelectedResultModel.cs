using LChess.Models.Chess.Match;

namespace LChess.Models.Result;

/// <summary>
/// 타일선택 결과 모델
/// </summary>
public class TileSelectedResultModel
{
    #region :: Constructor ::

    /// <summary>
    /// 생성자
    /// </summary>
    public TileSelectedResultModel()
    {
        Notation = string.Empty;

        IsNeedToPromotion = false;
    }

    #endregion
        
    #region :: Properties ::

    /// <summary>
    /// 기물이동 기보
    /// </summary>
    public string Notation { get; set; }

    /// <summary>
    /// 기물이동이 필요한지 여부
    /// </summary>
    public bool IsNeedToMove => !string.IsNullOrEmpty(Notation);

    /// <summary>
    /// 기물 승격이 필요한지 여부
    /// </summary>
    public bool IsNeedToPromotion { get; set; }

    #endregion

}
