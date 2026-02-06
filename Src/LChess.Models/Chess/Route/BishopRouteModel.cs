using LChess.Models.Chess.Board;

using LChess.Models.Chess.Route.Base;

using LChess.Util.Enums;

namespace LChess.Models.Chess.Route;

/// <summary>
/// 비숍 이동경로 모델
/// </summary>
public class BishopRouteModel : ChessUnitRouteModelBase
{
    #region :: Constructor ::

    /// <summary>
    /// 생성자
    /// </summary>
    public BishopRouteModel(ChessPosition position, PieceColorType color) : base(position, color)
    {
    }

    #endregion

    #region :: Methods ::

    /// <summary>
    /// 기물의 이동경로를 활성화
    /// </summary>
    /// <param name="managementModel"> 현재 위치/기물정보 맵퍼 </param>
    public override void TurnOnUnitRoute(BoardManagementModel managementModel)
    {
        TurnOnSlidingRoute(CreateAllRoutes(), managementModel);
    }

    /// <summary>
    /// 움직일 수 있는 경우의 수를 반환
    /// </summary>
    /// <param name="managementModel"> 현재 위치/기물정보 맵퍼 </param>
    /// <returns> 유닛 경로 상 현재 움직일 수 있는 모든 경우의수 </returns>
    public override List<ChessPosition> GetMovablePositions(BoardManagementModel managementModel)
    {
        return GetSlidingMovablePositions(CreateAllRoutes(), managementModel);
    }

    /// <summary>
    /// 대각선 4방향 경로 생성
    /// </summary>
    /// <returns>생성된 경로</returns>
    private List<List<ChessPosition>> CreateAllRoutes()
    {
        var position = new ChessPositionModel(_currentPosition);

        return new List<List<ChessPosition>>()
        {
            position.GetLeftTopDiagonalPositions    (),
            position.GetLeftBottomDiagonalPositions (),
            position.GetRightTopDiagonalPositions   (),
            position.GetRightBottomDiagonalPositions(),
        };
    }

    #endregion
}
