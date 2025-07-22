using LChess.Util.Enums;

namespace LChess.Models.Chess.Match;

/// <summary>
/// 체스 매치 진행상태를 저장하는 모델
/// </summary>
public class MatchStatusModel
{
    #region :: Constructor ::
    
    /// <summary>
    /// 생성자
    /// </summary>
    public MatchStatusModel(string notation, PieceColorType currentTurn, bool isCheck)
    {
        Notation = notation;
        
        CurrentTurn = currentTurn;
        
        IsCheck = isCheck;
    }
    
    #endregion
    
    #region :: Properties ::
    
    /// <summary>
    /// 기보
    /// </summary>
    public readonly string Notation;
    
    /// <summary>
    /// 현재 턴
    /// </summary>
    public readonly PieceColorType CurrentTurn;
    
    /// <summary>
    /// 다음 턴
    /// </summary>
    public PieceColorType NextTurn => CurrentTurn == PieceColorType.White ? PieceColorType.Black : PieceColorType.White;
    
    /// <summary>
    /// 현재 체크상태인지 여부
    /// </summary>
    public bool IsCheck;
    
    #endregion
    
    
    #region :: Methods ::
    
    #endregion
}
