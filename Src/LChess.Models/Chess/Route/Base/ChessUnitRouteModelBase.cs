using LChess.Models.Chess.Board;

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
    /// <param name="managementModel"> 현재 위치/기물정보 맵퍼 </param>
    public abstract void TurnOnUnitRoute(BoardManagementModel managementModel);

    /// <summary>
    /// 움직일 수 있는 경우의 수를 반환
    /// </summary>
    /// <param name="managementModel"> 현재 위치/기물정보 맵퍼 </param>
    /// <returns> 유닛 경로 상 현재 움직일 수 있는 모든 경우의수 </returns>
    public abstract List<ChessPosition> GetMovablePositions(BoardManagementModel managementModel);

    /// <summary>
    /// 슬라이딩 기물(룩, 비숍, 퀸, 킹)의 이동경로를 하이라이트
    /// </summary>
    /// <param name="routes">방향별 이동 경로 리스트</param>
    /// <param name="managementModel">보드 관리 모델</param>
    protected void TurnOnSlidingRoute(List<List<ChessPosition>> routes, BoardManagementModel managementModel)
    {
        foreach (var line in routes)
        {
            foreach (var position in line)
            {
                if (!managementModel.TryGetTile(position, out var tile) || tile == null)
                    continue;

                if (tile.IsEmpty)
                {
                    tile.IsHighLightMove = true;
                    continue;
                }

                tile.IsHighLightEnemy = tile.Unit?.IsSameColor(_unitColor) == false;
                break;
            }
        }
    }

    /// <summary>
    /// 슬라이딩 기물(룩, 비숍, 퀸, 킹)의 이동 가능한 위치를 수집
    /// </summary>
    /// <param name="routes">방향별 이동 경로 리스트</param>
    /// <param name="managementModel">보드 관리 모델</param>
    /// <returns>이동 가능한 위치 리스트</returns>
    protected List<ChessPosition> GetSlidingMovablePositions(List<List<ChessPosition>> routes, BoardManagementModel managementModel)
    {
        var result = new List<ChessPosition>();

        foreach (var line in routes)
        {
            foreach (var position in line)
            {
                if (!managementModel.TryGetTile(position, out var tile) || tile == null)
                    continue;

                if (tile.IsEmpty)
                {
                    result.Add(position);
                    continue;
                }

                if (tile.Unit?.IsSameColor(_unitColor) == false)
                    result.Add(position);

                break;
            }
        }

        return result;
    }

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
