using LChess.Util.Enums;

namespace LChess.Custom.UI.Unit;

/// <summary>
/// 체스보드 타일 컨트롤
/// </summary>
public class LChessBoardTile : Control
{
	static LChessBoardTile()
	{
		DefaultStyleKeyProperty.OverrideMetadata(typeof(LChessBoardTile), new FrameworkPropertyMetadata(typeof(LChessBoardTile)));
	}

	#region DP

	/// <summary>
	/// LChess 기물색상 타입 의존프로퍼티
	/// </summary>
	public static readonly DependencyProperty LChessPieceColorTypeProperty =
		DependencyProperty.Register(nameof(LChessPieceColorType), typeof(PieceColorType), typeof(LChessBoardTile), new FrameworkPropertyMetadata(PieceColorType.White));

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
		DependencyProperty.Register(nameof(LChessTileColorType), typeof(ChessTileColorType), typeof(LChessBoardTile), new FrameworkPropertyMetadata(ChessTileColorType.Light));

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
		DependencyProperty.Register(nameof(LChessUnitType), typeof(ChessUnitType), typeof(LChessBoardTile), new FrameworkPropertyMetadata(ChessUnitType.Empty));

	/// <summary>
	/// LChess 기물 타입
	/// </summary>
	public ChessUnitType LChessUnitType
	{
		get => (ChessUnitType)GetValue(LChessUnitTypeProperty);
		set => SetValue(LChessUnitTypeProperty, value);
    }

    /// <summary>
    /// 선택 여부 의존 프로퍼티
    /// </summary>
    public static readonly DependencyProperty IsSelectedProperty =
        DependencyProperty.Register(nameof(IsSelected), typeof(bool), typeof(LChessBoardTile), new FrameworkPropertyMetadata(false));

    /// <summary>
    /// 선택 여부
    /// </summary>
    public bool IsSelected
    {
        get => (bool)GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    /// <summary>
    /// 적기물 하이라이트 여부 의존 프로퍼티
    /// </summary>
    public static readonly DependencyProperty IsHighLightEnemyProperty =
        DependencyProperty.Register(nameof(IsHighLightEnemy), typeof(bool), typeof(LChessBoardTile), new FrameworkPropertyMetadata(false));

    /// <summary>
    /// 적기물 하이라이트 여부
    /// </summary>
    public bool IsHighLightEnemy
    {
        get => (bool)GetValue(IsHighLightEnemyProperty);
        set => SetValue(IsHighLightEnemyProperty, value);
    }

    /// <summary>
    /// 선택된 기물 이동경로 하이라이트 여부 의존 프로퍼티
    /// </summary>
    public static readonly DependencyProperty IsHighLightMoveProperty =
        DependencyProperty.Register(nameof(IsHighLightMove), typeof(bool), typeof(LChessBoardTile), new FrameworkPropertyMetadata(false));

    /// <summary>
    /// 선택된 기물 이동경로 하이라이트 여부
    /// </summary>
    public bool IsHighLightMove
    {
        get => (bool)GetValue(IsHighLightMoveProperty);
        set => SetValue(IsHighLightMoveProperty, value);
    }


    /// <summary>
    /// 위험상태 하이라이트 여부 의존 프로퍼티
    /// </summary>
    public static readonly DependencyProperty IsHighLightDangerProperty =
        DependencyProperty.Register(nameof(IsHighLightDanger), typeof(bool), typeof(LChessBoardTile), new FrameworkPropertyMetadata(false));

    /// <summary>
    /// 위험상태 하이라이트 여부
    /// </summary>
    public bool IsHighLightDanger
    {
        get => (bool)GetValue(IsHighLightDangerProperty);
        set => SetValue(IsHighLightDangerProperty, value);
    }

    #endregion
}
