namespace LChess.Models.Stockfish;

/// <summary>
/// Stockfish 엔진에서 반환하는 결과 모델
/// </summary>
public class StockfishResultModel
{
    #region :: Constructor ::

    /// <summary>
    /// 생성자
    /// </summary>
    public StockfishResultModel()
    {
        TileCodeList = new();
        CheckerList  = new();

        BestMove     = string.Empty;
    }

    #endregion

    #region :: Services ::

    #endregion

    #region :: Properties ::

    /// <summary>
    /// Stockfish 엔진에서 반환하는 타일 코드 리스트
    /// </summary>
    public List<string> TileCodeList { get; init; }

    /// <summary>
    /// Stockfish 엔진에서 반환하는 체크중인 기물 리스트
    /// </summary>
    public List<string> CheckerList { get; init; }

    /// <summary>
    /// Stockfish 엔진에서 반환하는 최적의 이동 기보
    /// </summary>
    public string? BestMove { get; set; }

    /// <summary>
    /// 무승부 여부
    /// </summary>
    public bool IsDraw => BestMove?.Contains("none") == true && !IsCheck;

    /// <summary>
    /// 체크메이트 여부
    /// </summary>
    public bool IsCheckMate => BestMove?.Contains("none") == true && IsCheck;

    /// <summary>
    /// 현재 체크상태인지 여부
    /// </summary>
    public bool IsCheck => CheckerList.Count > 0 && !string.IsNullOrEmpty(CheckerList.First());

    #endregion


    #region :: Methods ::

    /// <summary>
    /// 타일 코드리스트 새로 설정
    /// </summary>
    /// <param name="tileCodeList"> 설정대상 </param>
    public void SetTileCodeList(List<string> tileCodeList)
    {
        TileCodeList.Clear();
        TileCodeList.AddRange(tileCodeList);
    }

    /// <summary>
    /// 체크 기물 리스트 새로 설정
    /// </summary>
    /// <param name="checkerList"> 설정대상 </param>
    public void SetCheckerList(List<string> checkerList)
    {
        CheckerList.Clear();
        CheckerList.AddRange(checkerList);
    }

    #endregion

    #region :: Commands ::

    #endregion
}
