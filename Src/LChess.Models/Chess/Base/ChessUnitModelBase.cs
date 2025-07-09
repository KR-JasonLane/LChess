using LChess.Util.Enums;

namespace LChess.Models.Chess.Base;

/// <summary>
/// 체스 기물 모델의 부모 클래스
/// </summary>
public abstract partial class ChessUnitModelBase : ObservableObject
{
    #region :: Constructor ::

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="unitType"></param>
    /// <param name="pieceColorType"></param>
    public ChessUnitModelBase(ChessUnitType unitType, PieceColorType pieceColorType)
    {
        ColorType = pieceColorType;
        UnitType  = unitType      ;
    }

    #endregion

    #region :: Properties ::

    /// <summary>
    /// 기물 색상
    /// </summary>
    [ObservableProperty]
    private PieceColorType _ColorType;
    /// <summary>
    /// 기물 타입
    /// </summary>
    [ObservableProperty]
    private ChessUnitType _unitType;

    #endregion

    #region :: Methods ::

    /// <summary>
    /// 현재 기물 색상과 같은 색상인지 판단
    /// </summary>
    /// <param name="color"> 비교할 색상 </param>
    /// <returns> 색상이 같은지 여부 </returns>
    public bool IsSameColor(PieceColorType color) => ColorType == color;

    /// <summary>
    /// 타일에 적 기물이 있는지 판단
    /// </summary>
    /// <param name="otherTile"> 확인할 타일 </param>
    /// <returns> 적 기물 유무 </returns>
    public bool IsEnemy(ChessUnitModelBase? otherUnit) => otherUnit != null && otherUnit.ColorType != ColorType;

    /// <summary>
    /// 기물이 움직일 수 있는 영역을 반환
    /// </summary>
    /// <returns> 각 방향마다의 포지션 리스트를 담은 리스트 </returns>
    public abstract List<List<ChessPosition>> GetAvailablePositions(ChessPositionModel position, Dictionary<ChessPosition, ChessBoardTileModel> mapper);

    /// <summary>
    /// 기물모델 생성
    /// </summary>
    /// <param name="unitCode"> Stockfish 기물코드 </param>
    /// <returns> 생성된 기물 모델 </returns>
    public static ChessUnitModelBase? CreateUnitModel(char unitCode)
    {
        if (unitCode == ' ') return null; // 빈 칸인 경우

        var unitColor = char.IsLower(unitCode) ? PieceColorType.Black : PieceColorType.White;

        return char.ToUpper(unitCode) switch
        {
            'P' => new PawnModel  (unitColor),
            'R' => new RookModel  (unitColor),
            'N' => new KnightModel(unitColor),
            'B' => new BishopModel(unitColor),
            'Q' => new QueenModel (unitColor),
            'K' => new KingModel  (unitColor),
            _   => null
        };
    }

    #endregion
}
