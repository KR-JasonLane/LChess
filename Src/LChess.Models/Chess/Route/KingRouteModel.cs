using LChess.Models.Chess.Board;
using LChess.Models.Chess.Route.Base;
using LChess.Util.Enums;

namespace LChess.Models.Chess.Route;

/// <summary>
/// 킹 이동경로 모델
/// </summary>
public class KingRouteModel : ChessUnitRouteModelBase
{
    #region :: Constructor ::

    /// <summary>
    /// 생성자
    /// </summary>
    public KingRouteModel(ChessPosition position, PieceColorType color) : base(position, color)
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
        var result = GetSlidingMovablePositions(CreateAllRoutes(), managementModel);

        //캐슬링 가능 위치까지 포함시켜줌
        result.AddRange(CreateCastlingPosition(managementModel));

        return result;
    }

    /// <summary>
    /// 전 방향 1칸 경로 생성
    /// </summary>
    /// <returns>생성된 경로</returns>
    private List<List<ChessPosition>> CreateAllRoutes()
    {
        var position = new ChessPositionModel(_currentPosition);

        return new List<List<ChessPosition>>()
        {
            position.GetTopLinePositions            (1),
            position.GetLeftTopDiagonalPositions    (1),
            position.GetLeftLinePositions           (1),
            position.GetLeftBottomDiagonalPositions (1),
            position.GetBottomLinePositions         (1),
            position.GetRightTopDiagonalPositions   (1),
            position.GetRightLinePositions          (1),
            position.GetRightBottomDiagonalPositions(1),
        };
    }

    /// <summary>
    /// 캐슬링 포지션 생성
    /// </summary>
    /// <param name="managementModel"> 보드 관리 매니저 </param>
    /// <returns> 캐슬링 가능한 위치리스트 반환 </returns>
    private List<ChessPosition> CreateCastlingPosition(BoardManagementModel managementModel)
    {
        var result = new List<ChessPosition>();

        //킹이 한번이라도 이동했을 경우 수행하지 않음.
        if (managementModel.IsTileUpdated(_currentPosition)) return result;

        var position = new ChessPositionModel(_currentPosition);

        //왼쪽 캐슬링 검사
        var leftCastling = ExtractAbleToCastlingPosition(position, position.GetLeftLinePositions(), managementModel);
        if (leftCastling != ChessPosition.Invalid)
            result.Add(leftCastling);

        //오른쪽 캐슬링 검사
        var rightCastling = ExtractAbleToCastlingPosition(position, position.GetRightLinePositions(), managementModel);
        if (rightCastling != ChessPosition.Invalid)
            result.Add(rightCastling);

        return result;
    }

    /// <summary>
    /// 캐슬링 가능한 위치 추출
    /// </summary>
    /// <param name="currentPosition"> 현재 포지션 모델 </param>
    /// <param name="positionList"> 검사할 포지션 리스트 </param>
    /// <param name="managementModel"> 보드 관리모델 </param>
    /// <returns> 캐슬링 가능한 위치 반환 </returns>
    private static ChessPosition ExtractAbleToCastlingPosition(ChessPositionModel currentPosition, List<ChessPosition> positionList, BoardManagementModel managementModel)
    {
        for (var i = 0; i < positionList.Count; i++)
        {
            if (managementModel.TryGetTile(positionList[i], out var tile) && tile != null && !tile.IsEmpty)
            {
                //끝까지 검사하였고, 그 위치가 Rook이며 이동이력이 없을 경우
                if (i == positionList.Count - 1 && tile.Unit?.UnitType == ChessUnitType.Rook && !managementModel.IsTileUpdated(tile.Position.Code))
                {
                    return positionList[1];
                }

                break;
            }
        }

        return ChessPosition.Invalid;
    }

    #endregion
}
