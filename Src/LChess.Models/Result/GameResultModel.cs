using LChess.Util.Enums;

namespace LChess.Models.Result;

/// <summary>
/// 게임결과 모델
/// </summary>
public class GameResultModel
{
    #region :: Constructor ::
    
    /// <summary>
    /// 생성자
    /// </summary>
    public GameResultModel(EndGameType type, PieceColorType winner)
    {
        Type = type;
        Winner = winner;
    }
    
    #endregion
    
    #region :: Properties ::
    
    /// <summary>
    /// 게임종료 타입
    /// </summary>
    public EndGameType Type;
    
    /// <summary>
    /// 승자
    /// </summary>
    public PieceColorType Winner;
    
    #endregion
}
