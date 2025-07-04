using LChess.Models.Chess;

using LChess.Util.Enums;

namespace LChess.Abstract.Service;

public interface IChessGameService
{
	/// <summary>
	/// 게임 초기화
	/// </summary>
	/// <returns></returns>
	public void ClearMoves();
}
