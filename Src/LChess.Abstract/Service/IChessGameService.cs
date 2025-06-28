using LChess.Models.Chess;
using System.Collections.ObjectModel;

namespace LChess.Abstract.Service;

public interface IChessGameService
{
	/// <summary>
	/// 현재 보드상태
	/// </summary>
	/// <returns></returns>
	public Task<List<List<ChessBoardUnitModel>>?> DrawBoard();

	/// <summary>
	/// 게임 초기화
	/// </summary>
	/// <returns></returns>
	public void ClearMoves();
}
