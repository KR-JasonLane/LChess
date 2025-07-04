using LChess.Abstract.Service;

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

		_notationBuilder = new StringBuilder();
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
	private StringBuilder _notationBuilder;

    #endregion

    #region :: Methods ::

	/// <summary>
	/// 게임 초기화
	/// </summary>
	/// <returns></returns>
	public void ClearMoves()
    {
		_notationBuilder.Clear();
        _stockfishEngineService.SendCommandAsync("ucinewgame", string.Empty);
    }

    #endregion
}
