using LChess.Models.Chess.Route.Base;

using LChess.Models.Chess.Board;

using LChess.Util.Enums;

namespace LChess.Models.Chess.Unit.Base;

/// <summary>
/// 체스 기물 모델의 부모 클래스
/// </summary>
public abstract partial class ChessUnitModelBase : ObservableObject
{
    #region :: Constructor ::

    /// <summary>
    /// 생성자
    /// </summary>
    public ChessUnitModelBase(ChessUnitType unitType, PieceColorType pieceColorType, ChessPosition position, char originalCode)
    {
        ColorType = pieceColorType;
        UnitType  = unitType      ;

        RouteModel = ChessUnitRouteModelBase.CreateRouteModel(unitType, pieceColorType, position);

        OriginalCode = originalCode;
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

    /// <summary>
    /// 기물 경로모델
    /// </summary>
    public ChessUnitRouteModelBase? RouteModel { get; init; }

    /// <summary>
    /// 기물코드 원본
    /// </summary>
    public readonly char OriginalCode;

    #endregion

    #region :: Methods ::

    /// <summary>
    /// 현재 기물 색상과 같은 색상인지 판단
    /// </summary>
    /// <param name="color"> 비교할 색상 </param>
    /// <returns> 색상이 같은지 여부 </returns>
    public bool IsSameColor(PieceColorType? color) => ColorType == color;

    /// <summary>
    /// 타일에 적 기물이 있는지 판단
    /// </summary>
    /// <param name="otherTile"> 확인할 타일 </param>
    /// <returns> 적 기물 유무 </returns>
    public bool IsEnemy(ChessUnitModelBase? otherUnit) => otherUnit != null && otherUnit.ColorType != ColorType;

    /// <summary>
    /// 기물모델 생성
    /// </summary>
    /// <param name="unitCode"> Stockfish 기물코드 </param>
    /// <returns> 생성된 기물 모델 </returns>
    public static ChessUnitModelBase? CreateUnitModel(char unitCode, ChessPosition position)
    {
        if (unitCode == ' ') return null; // 빈 칸인 경우

        var unitColor = char.IsLower(unitCode) ? PieceColorType.Black : PieceColorType.White;

        return char.ToUpper(unitCode) switch
        {
            'P' => new PawnModel  (unitColor, position, unitCode),
            'R' => new RookModel  (unitColor, position, unitCode),
            'N' => new KnightModel(unitColor, position, unitCode),
            'B' => new BishopModel(unitColor, position, unitCode),
            'Q' => new QueenModel (unitColor, position, unitCode),
            'K' => new KingModel  (unitColor, position, unitCode),
            _   => null
        };
    }

    /// <summary>
    /// 기물타입과 색상을 기반으로 가상의 기물 모델 생성
    /// </summary>
    /// <param name="unitType">생성할 기물 타입</param>
    /// <param name="pieceColorType">기물 색상</param>
    /// <param name="position">기물이 위치할 좌표</param>
    /// <returns>생성된 가상의 기물 모델</returns>
    public static ChessUnitModelBase? CreateVirtualUnitModel(ChessUnitType unitType, PieceColorType pieceColorType, ChessPosition position)
    {
        return unitType switch
        {
            ChessUnitType.Pawn   => new PawnModel  (pieceColorType, position, ' '),
            ChessUnitType.Rook   => new RookModel  (pieceColorType, position, ' '),
            ChessUnitType.Knight => new KnightModel(pieceColorType, position, ' '),
            ChessUnitType.Bishop => new BishopModel(pieceColorType, position, ' '),
            ChessUnitType.Queen  => new QueenModel (pieceColorType, position, ' '),
            ChessUnitType.King   => new KingModel  (pieceColorType, position, ' '),
            _ => null
        };
    }

    #endregion
}
