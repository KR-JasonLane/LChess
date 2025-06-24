namespace LChess.Util.Enums;

/// <summary>
/// 체스보드 요소 타입
/// </summary>
public enum ChessUnitType
{
	// 빈 칸
	Empty = 0, 
	// 하이라이트 (움직일 수 있는 범위 표시)
	HighLight,
	// 폰
	Pawn,
	// 나이트
	Knight,
	// 비숍
	Bishop,
	// 룩
	Rook,
	// 퀸
	Queen,
	// 킹
	King, 
}
