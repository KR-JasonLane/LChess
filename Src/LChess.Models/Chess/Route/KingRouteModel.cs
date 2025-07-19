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
        
           !! 특수이동 : 캐슬링
         
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
                    if (tile.Unit?.IsSameColor(_unitColor) == false) result.Add(go);

                    // 루프 탈출
                    break;
                }
            }
        }

        //캐슬링 가능 위치까지 포함시켜줌
        result.AddRange(CreateCastlingPosition(managementModel));

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

    /// <summary>
    /// 캐슬링 포지션 생성
    /// </summary>
    /// <param name="managementModel"> 보드 관리 매니저 </param>
    /// <returns> 캐슬링 가능한 위치리스트 반환 </returns>
    private List<ChessPosition> CreateCastlingPosition(BoardManagementModel managementModel)
    {
        // 결과생성
        var result = new List<ChessPosition>();

        //킹이 한번이라도 이동했을 경우 수행하지 않음.
        if (managementModel.IsTileUpdated(_currentPosition)) return result;

        // 포지션 모델 생성
        var position = new ChessPositionModel(_currentPosition);

        //왼쪽 경로 생성
        var left = position.GetLeftLinePositions();

        //캐슬링 가능 위치 추출
        var leftCastlingPosition = ExtractAbleToCastlingPosition(position, left, managementModel);

        //유효성 검사
        if(leftCastlingPosition != ChessPosition.Invalid)
        {
            // 캐슬링 가능한 위치가 있다면, 리스트에 추가
            result.Add(leftCastlingPosition);
        }

        //오른쪽 경로 생성
        var right = position.GetRightLinePositions();

        //캐슬링 가능 위치 추출
        var rightCastlingPosition = ExtractAbleToCastlingPosition(position, right, managementModel);

        //유효성 검사
        if (rightCastlingPosition != ChessPosition.Invalid)
        {
            // 캐슬링 가능한 위치가 있다면, 리스트에 추가
            result.Add(rightCastlingPosition);
        }

        return result;
    }

    /// <summary>
    /// 캐슬링 가능한 위치 추출
    /// </summary>
    /// <param name="currentPosition"> 현재 포지션 모델 </param>
    /// <param name="positionList"> 검사할 포지션 리스트 </param>
    /// <param name="managementModel"> 보드 관리모델 </param>
    /// <returns> 캐슬링 가능한 위치리스트 반환 </returns>
    private static ChessPosition ExtractAbleToCastlingPosition(ChessPositionModel currentPosition, List<ChessPosition> positionList, BoardManagementModel managementModel)
    {
        //경로 검사
        for (var i = 0; i < positionList.Count; i++)
        {
            // 타일이 비어있지 않을때
            if (managementModel.TryGetTile(positionList[i], out var tile) && tile != null && !tile.IsEmpty)
            {
                //현재 끝까지 검사 하였고, 그 위치에 해당하는 타일이 Rook이며 이동이력이 없을 경우
                if (i == positionList.Count - 1 && tile.Unit?.UnitType == ChessUnitType.Rook && !managementModel.IsTileUpdated(tile.Position.Code))
                {
                    // 1번 인덱스에 해당하는 위치가 캐슬링이 가능한 위치임.
                    // 이미 킹이 이동한 경우는 제외했으므로, 리스트의 길이검사는 하지않음.
                    return positionList[1];
                }
            }
        }

        return ChessPosition.Invalid;
    }

    #endregion
}
