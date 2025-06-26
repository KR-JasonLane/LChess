using LChess.Util.Enums;

namespace LChess.DataBinding.Model;

/// <summary>
/// 체스보드 요소 모델
/// </summary>
public partial class ChessBoardUnitModel : ObservableObject
{
	#region :: Properties ::
	/// <summary>
	/// 기물 색상
	/// </summary>
	[ObservableProperty]
	private PieceColorType _pieceColorType;

	/// <summary>
	/// 체스판 색상
	/// </summary>
	[ObservableProperty]
	private ChessTileColorType _tileColorType;

	/// <summary>
	/// 말
	/// </summary>
	[ObservableProperty]
	private ChessUnitType _unitType;

	#endregion
}
