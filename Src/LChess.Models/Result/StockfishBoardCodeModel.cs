namespace LChess.Models.Result;

/// <summary>
/// Stockfish 엔진에서 반환하는 Draw 결과 모델
/// </summary>
public class StockfishBoardCodeModel
{
    #region :: Constructor ::

    /// <summary>
    /// 생성자
    /// </summary>
    public StockfishBoardCodeModel()
    {
        TileCodeList = new();
        CheckerList  = new();
    }

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
