using LChess.Util.Enums;

namespace LChess.Custom.UI.Unit;

/// <summary>
/// 체스보드 유닛 컨트롤
/// </summary>
public class LChessBoardUnit : Control
{
	static LChessBoardUnit()
	{
		DefaultStyleKeyProperty.OverrideMetadata(typeof(LChessBoardUnit), new FrameworkPropertyMetadata(typeof(LChessBoardUnit)));
	}

	#region DP

	/// <summary>
	/// LChess 기물색상 타입 의존프로퍼티
	/// </summary>
	public static readonly DependencyProperty LChessPieceColorTypeProperty =
		DependencyProperty.Register(nameof(LChessPieceColorType), typeof(PieceColorType), typeof(LChessBoardUnit), new FrameworkPropertyMetadata(PieceColorType.White));

	/// <summary>
	/// LChess 기물색상 타입
	/// </summary>
	public PieceColorType LChessPieceColorType
	{
		get => (PieceColorType)GetValue(LChessPieceColorTypeProperty);
		set => SetValue(LChessPieceColorTypeProperty, value);
	}

	/// <summary>
	/// LChess 보드타일 색상 타입 의존프로퍼티
	/// </summary>
	public static readonly DependencyProperty LChessTileColorTypeProperty =
		DependencyProperty.Register(nameof(LChessTileColorType), typeof(ChessTileColorType), typeof(LChessBoardUnit), new FrameworkPropertyMetadata(ChessTileColorType.Light));

	/// <summary>
	/// LChess 보드타일 색상 타입
	/// </summary>
	public ChessTileColorType LChessTileColorType
	{
		get => (ChessTileColorType)GetValue(LChessTileColorTypeProperty);
		set => SetValue(LChessTileColorTypeProperty, value);
	}

	/// <summary>
	/// LChess 기물 타입 의존프로퍼티
	/// </summary>
	public static readonly DependencyProperty LChessUnitTypeProperty =
		DependencyProperty.Register(nameof(LChessUnitType), typeof(ChessUnitType), typeof(LChessBoardUnit), new FrameworkPropertyMetadata(ChessUnitType.Pawn));

	/// <summary>
	/// LChess 기물 타입
	/// </summary>
	public ChessUnitType LChessUnitType
	{
		get => (ChessUnitType)GetValue(LChessUnitTypeProperty);
		set => SetValue(LChessUnitTypeProperty, value);
    }

    /// <summary>
    /// 하이라이트 여부 의존 프로퍼티
    /// </summary>
    public static readonly DependencyProperty IsHighLightProperty =
        DependencyProperty.Register(nameof(IsHighLight), typeof(bool), typeof(LChessBoardUnit), new FrameworkPropertyMetadata(false));

    /// <summary>
    /// 하이라이트 여부
    /// </summary>
    public bool IsHighLight
    {
        get => (bool)GetValue(LChessUnitTypeProperty);
        set => SetValue(LChessUnitTypeProperty, value);
    }

    #endregion
}
