using LChess.Models.Chess.Unit.Base;

using LChess.Models.Chess.Board;

using LChess.Util.Enums;

namespace LChess.Models.Chess;

/// <summary>
/// 체스보드 요소 모델
/// </summary>
public partial class ChessBoardTileModel : ObservableObject
{
    #region :: Constructor ::

    /// <summary>
    /// 생성자
    /// </summary>
    public ChessBoardTileModel(ChessTileColorType tileColor, int row, int column, char unitCode)
    {
        Position = new ChessPositionModel(row, column);

        Unit = ChessUnitModelBase.CreateUnitModel(unitCode, Position.Code);

        TileColorType  = tileColor ;
    }

    #endregion

    #region :: Properties ::

    /// <summary>
    /// 체스판 색상
    /// </summary>
    [ObservableProperty]
    private ChessTileColorType _tileColorType;

    /// <summary>
    /// 기물
    /// </summary>
    [ObservableProperty]
    private ChessUnitModelBase? _unit;

    /// <summary>
    /// 타일 선택 여부
    /// </summary>
    [ObservableProperty]
    private bool _isSelected;

    /// <summary>
    /// 기물 이동경로 하이라이트 여부
    /// </summary>
    [ObservableProperty]
    private bool _isHighLightMove;

    /// <summary>
    /// 적 기물 하이라이트 여부
    /// </summary>
    [ObservableProperty]
    private bool _isHighLightEnemy;

    /// <summary>
    /// 위험상태 하이라이트 여부
    /// </summary>
    [ObservableProperty]
    private bool _isHighLightDanger;

    /// <summary>
    /// 해당 모델의 위치
    /// </summary>
    public readonly ChessPositionModel Position;

    /// <summary>
    /// 다른 기물이 해당 타일로 이동 가능한 상태인지 여부
    /// </summary>
    public bool IsMovableTarget => IsHighLightEnemy || IsHighLightMove;

    /// <summary>
    /// 타일이 빈 칸인지 여부
    /// </summary>
    public bool IsEmpty => Unit is null;

    #endregion

    #region :: Methods ::

    /// <summary>
    /// 기물 변경
    /// </summary>
    /// <param name="unitCode"> 변경할 기물 코드 </param>
    public void UpdateUnit(char unitCode)
    {
        // 하이라이트 초기화
        TurnOffHighLight();

        //유닛 변경
        Unit = ChessUnitModelBase.CreateUnitModel(unitCode, Position.Code);
    }

    /// <summary>
    /// 모든 하이라이트 끄기
    /// </summary>
    public void TurnOffHighLight()
    {
        IsHighLightMove  = false;
        IsHighLightEnemy = false;
        IsSelected       = false;
    }

    #endregion
}
