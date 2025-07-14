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

    #region :: Properties ::

    #endregion

    #region :: Methods ::

    /// <summary>
    /// 기물의 이동경로를 활성화
    /// </summary>
    /// <param name="tileMapper"> 현재 위치/기물정보 맵퍼 </param>
    public override void TurnOnUnitRoute(BoardManagementModel managementModel)
    {
        /*
          
           !! 킹은 모든 방향으로 한칸씩 이동이 가능. 
        
           !! TODO : 캐슬링 구현

           !! TODO : 체크, 체크메이트 상태 대응
         
         */

        //움직일 수 있는 모든 경로 생성
        var positions = CreateAllRoutes();

        //방향별 위치 루프
        foreach (var line in positions)
        {
            //각개 위치 루프
            foreach (var go in line)
            {
                if (managementModel.TryGetTile(go, out var tile) && tile != null)
                {
                    // 타일이 존재하고, 비어있으면
                    if (tile.IsEmpty)
                    {
                        // 이동경로 하이라이트
                        tile.IsHighLightMove = true;

                        //계속진행
                        continue;
                    }

                    //타일이 비어있지 않으면, 적군인지만 판단 후
                    tile.IsHighLightEnemy = tile.Unit?.IsSameColor(_unitColor) == false;

                    // 루프 탈출
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 움직일 수 있는 경우의 수를 반환
    /// </summary>
    /// <param name="tileMapper"> 현재 위치/기물정보 맵퍼 </param>
    /// <returns> 유닛 경로 상 현재 움직일 수 있는 모든 경우의수 </returns>
    public override List<ChessPosition> GetMovablePositions(BoardManagementModel managementModel)
    {
        //반환 리스트 생성
        var result = new List<ChessPosition>();

        //움직일 수 있는 모든 경로 생성
        var positions = CreateAllRoutes();

        //방향별 위치 루프
        foreach (var line in positions)
        {
            //각개 위치 루프
            foreach (var go in line)
            {
                if (managementModel.TryGetTile(go, out var tile) && tile != null)
                {
                    // 타일이 존재하고, 비어있으면
                    if (tile.IsEmpty)
                    {
                        result.Add(go);

                        //계속진행
                        continue;
                    }

                    //타일이 비어있지 않으면, 적군일 경우에만 리스트에 담아줌.
                    if (tile.Unit?.IsSameColor(_unitColor) == false)
                    {
                        result.Add(go);
                    }

                    // 루프 탈출
                    break;
                }
            }
        }

        //결과 반환
        return result;
    }

    // 이동 가능한 모든 경로 생성
    private List<List<ChessPosition>> CreateAllRoutes()
    {
        var position = new ChessPositionModel(_currentPosition);

        //각 방향별 대각선 위치 생성
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

    #endregion
}
