namespace LChess.Models.Result;

/// <summary>
/// Stockfish 엔진에서 반환하는 BestMove 결과 모델
/// </summary>
public class StockfishBestMoveModel
{
    #region :: Constructor ::
    
    /// <summary>
    /// 생성자
    /// </summary>
    public StockfishBestMoveModel(string bestMove)
    {
        BestMove = bestMove;
    }
    
    #endregion
    
    #region :: Properties ::
    
    /// <summary>
    /// Stockfish 엔진에서 반환하는 최적의 이동 기보
    /// </summary>
    public readonly string BestMove;
    
    /// <summary>
    /// 움직일 수 있는 경우의 수가 있는지 여부 (즉, BestMove가 none이 아닌지 여부)
    /// </summary>
    public bool CanMove => !BestMove.Contains("none");
    
    #endregion
    
    #region :: Methods ::
    
    #endregion
    
    #region :: Commands ::
    
    #endregion
    
}
