using LChess.Util.Enums;

namespace LChess.Abstract.Model;

/// <summary>
/// 체스보드 요소 모델 인터페이스
/// </summary>
public interface IChessBoardUnitModel
{
	/// <summary>
	/// 기물 색상
	/// </summary>
	public PieceColorType PieceColorType { get; set; }

	/// <summary>
	/// 체스판 색상
	/// </summary>
	public ChessTileColorType TileColorType { get; set; }

	/// <summary>
	/// 말
	/// </summary>
	public ChessUnitType UnitType { get; set; }
}
