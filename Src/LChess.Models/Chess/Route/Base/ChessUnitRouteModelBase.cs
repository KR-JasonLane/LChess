using LChess.Util.Enums;

namespace LChess.Models.Chess.Route.Base;

/// <summary>
/// 체스 기물 이동경로 모델 상속 클래스
/// </summary>
public abstract class ChessUnitRouteModelBase
{
    #region :: Constructor ::

    /// <summary>
    /// 생성자
    /// </summary>
    public ChessUnitRouteModelBase(ChessPosition position, PieceColorType color)
    {
        //위치 기억
        _currentPosition = position;

        // 색상 기억
        _unitColor = color;
    }

    #endregion

    #region :: Properties ::

    /// <summary>
    /// 현재위치
    /// </summary>
    protected readonly ChessPosition _currentPosition;

    /// <summary>
    /// 기물색상
    /// </summary>
    protected readonly PieceColorType _unitColor;

    #endregion

    #region :: Methods ::

    /// <summary>
    /// 기물의 이동경로를 활성화
    /// </summary>
    /// <param name="tileMapper"> 현재 위치/기물정보 맵퍼 </param>
    public abstract void TurnOnUnitRoute(Dictionary<ChessPosition, ChessBoardTileModel> tileMapper);

    /// <summary>
    /// 유닛경로 모델 생성
    /// </summary>
    /// <param name="unitType"> 기물타입 </param>
    /// <param name="color"> 기물색상 </param>
    /// <param name="position"> 기물의위치 </param>
    /// <returns> 해당되는 경로모델 </returns>
    public static ChessUnitRouteModelBase? CreateRouteModel(ChessUnitType unitType, PieceColorType color, ChessPosition position)
    {
        return unitType switch
        {
            ChessUnitType.Pawn   => new PawnRouteModel  (position, color),
            ChessUnitType.King   => new KingRouteModel  (position, color),
            ChessUnitType.Queen  => new QueenRouteModel (position, color),
            ChessUnitType.Rook   => new RookRouteModel  (position, color),
            ChessUnitType.Bishop => new BishopRouteModel(position, color),
            ChessUnitType.Knight => new KnightRouteModel(position, color),
            _ => null
        };
    }

    #endregion
}
