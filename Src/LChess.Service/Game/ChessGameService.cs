using LChess.Abstract.Service;

using LChess.Models.Result;

namespace LChess.Service.Game;

/// <summary>
/// 게임 관리 서비스
/// </summary>
public class ChessGameService : IChessGameService
{
    #region :: Constructure ::
    
    /// <summary>
    /// 생성자
    /// </summary>
    public ChessGameService(IStockfishEngineService stockfishEngineService)
    {
        _stockfishEngineService = stockfishEngineService;
        
        _notations = new List<string>();
    }
    
    #endregion
    
    #region :: Services ::
    
    /// <summary>
    /// 엔진관리 서비스
    /// </summary>
    private readonly IStockfishEngineService _stockfishEngineService;
    
    #endregion
    
    #region :: Properties ::
    
    /// <summary>
    /// 기물 움직임 기보
    /// </summary>
    private List<string> _notations;
    
    /// <summary>
    /// 직전 보드 상태
    /// </summary>
    private StockfishBoardCodeModel? _previousBoard;
    
    #endregion
    
    #region :: Methods ::
    
    /// <summary>
    /// 새게임 시작
    /// </summary>
    public async Task<StockfishBoardCodeModel?> NewGame()
    {
        _notations.Clear();
        
        await _stockfishEngineService.StartEngineAsync();
        await _stockfishEngineService.SendCommandAsync("position startpos", string.Empty);
        
        _previousBoard = await _stockfishEngineService.GetCurrentBoard();
        
        return _previousBoard;
    }
    
    /// <summary>
    /// AI 턴
    /// </summary>
    /// <returns> AI 판단 후 기물코드 반환 </returns>
    public async Task<StockfishBoardCodeModel?> ExecuteAIMove()
    {
        var aiMove = await BestMove();
        
        var result = await MovePiece(aiMove.BestMove);
        
        if(result != null)
        {
            result.CurrentMove = aiMove.BestMove;
        }
        
        return result;
    }
    
    /// <summary>
    /// 기물이동
    /// </summary>
    /// <param name="notation"> 기물이동 기보 문자열 </param>
    /// <returns> Stockfish 기물 코드 </returns>
    public async Task<StockfishBoardCodeModel?> MovePiece(string? notation)
    {
        if (string.IsNullOrEmpty(notation) || notation.Contains("none")) return null;
        
        // 기보 저장
        _notations.Add(notation);
        
        StringBuilder commandBuilder = new StringBuilder("position startpos moves");
        
        foreach(var history in _notations)
        {
            commandBuilder.Append($" {history}");
        }
        
        await _stockfishEngineService.SendCommandAsync(commandBuilder.ToString(), string.Empty);
        
        var result = await _stockfishEngineService.GetCurrentBoard();
        
        // 보드 상태가 변하지 않았으면 마지막 기보를 삭제하고 null 반환
        if (_previousBoard != null && IsSameBoard(_previousBoard, result))
        {
            if (_notations.Count > 0)
            _notations.RemoveAt(_notations.Count - 1);
            
            return null;
        }
        
        result.CurrentMove = notation;
        
        _previousBoard = result;
        
        return result;
    }
    
    /// <summary>
    /// AI가 판단하는 BestMove 획득
    /// </summary>
    /// <returns> BestMove 처리결과 </returns>
    public async Task<StockfishBestMoveModel> BestMove()
    {
        var bestMove = await _stockfishEngineService.SendCommandAsync("go depth 20", "best");
        
        return new StockfishBestMoveModel(bestMove?.Split(' ').ElementAt(1) ?? "(none)");
    }
    
    /// <summary>
    /// 두 보드 상태가 동일한지 비교
    /// </summary>
    private static bool IsSameBoard(StockfishBoardCodeModel a, StockfishBoardCodeModel b)
    {
        if (a.TileCodeList.Count != b.TileCodeList.Count)
        return false;
        
        for (int i = 0; i < a.TileCodeList.Count; i++)
        {
            if (a.TileCodeList[i] != b.TileCodeList[i])
            return false;
        }
        
        return true;
    }
    
    #endregion
}
