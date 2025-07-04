using LChess.Models.Chess;

using LChess.Util.Enums;

namespace LChess.Abstract.Service;

public interface IChessGameService
{
	/// <summary>
	/// 현재 보드상태
	/// </summary>
	/// <returns></returns>
	public Task<List<List<ChessBoardUnitModel>>?> CurrentBoard();

	/// <summary>
	/// 게임 초기화
	/// </summary>
	/// <returns></returns>
	public void ClearMoves();

	/// <summary>
	/// 사용자 기물색상 설정
	/// </summary>
	/// <param name="pieceColor"> 기물색상 타입 </param>
	public void SetUserPieceColor(PieceColorType pieceColor);

    /// <summary>
    /// 체스보드	유닛 선택
    /// </summary>
    /// <param name="selectedUnit"> 선택된 유닛 모델 </param>
    /// <returns> 유닛 선택 시 하이라이트 된 보드 반환 </returns>
    public List<List<ChessBoardUnitModel>> SelectUnit(ChessBoardUnitModel selectedUnit);
}
