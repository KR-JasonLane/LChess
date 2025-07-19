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

		_notations = new Queue<string>();
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
	private Queue<string> _notations;

    #endregion

    #region :: Methods ::

    /// <summary>
    /// 새게임 시작
    /// </summary>
    public async Task<StockfishBoardCodeModel?> NewGame()
    {
        _notations.Clear();
		await _stockfishEngineService.StartEngineAsync();
        await _stockfishEngineService.SendCommandAsync("ucinewgame", string.Empty);

        return await _stockfishEngineService.GetCurrentBoard();
    }

    /// <summary>
    /// AI 턴
    /// </summary>
    /// <returns> AI 판단 후 기물코드 반환 </returns>
    public async Task<StockfishBoardCodeModel?> ExecuteAIMove()
	{
        var aiMove = await BestMove();

        return await MovePiece(aiMove.BestMove);
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
		_notations.Enqueue(notation);

		StringBuilder commandBuilder = new StringBuilder("position startpos moves");

		foreach(var history in _notations)
		{
            commandBuilder.Append($" {history}");
        }

		await _stockfishEngineService.SendCommandAsync(commandBuilder.ToString(), string.Empty);

        var result = await _stockfishEngineService.GetCurrentBoard();

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

    #endregion
}
