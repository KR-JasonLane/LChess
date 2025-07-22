using LChess.Models.Chess.Board;

using LChess.Models.Chess.Route.Base;

using LChess.Util.Enums;

namespace LChess.Models.Chess.Route;

public class KnightRouteModel : ChessUnitRouteModelBase
{
    #region :: Constructor ::
    
    /// <summary>
    /// 생성자
    /// </summary>
    public KnightRouteModel(ChessPosition position, PieceColorType color) : base(position, color)
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
        
        !! 나이트는 기물을 뛰어넘을 수 있고, 특수한 이동 형태를 가지고 있음.
        
        */
        
        var position = new ChessPositionModel(_currentPosition);
        
        var positions = CreateAllRoutes();
        
        //방향별 위치 루프
        foreach (var line in positions)
        {
            //각개 위치 루프
            foreach (var go in line)
            {
                if (managementModel.TryGetTile(go, out var tile) && tile != null)
                {
                    //이동경로 표시여부 판단
                    tile.IsHighLightMove = tile.IsEmpty;
                    
                    //타일이 비어있지 않으면, 적군인지만 판단 후
                    tile.IsHighLightEnemy = !tile.IsEmpty && tile.Unit?.IsSameColor(_unitColor) == false;
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
                    if(tile.Unit?.IsSameColor(_unitColor) == false || tile.IsEmpty)
                    {
                        // 타일이 존재하고, 비어있거나 적군이면
                        result.Add(go);
                    }
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
            //좌측상단
            position.CreatePositionsByOffset(-2, -1, 1),
            position.CreatePositionsByOffset(-1, -2, 1),
            
            //우측상단
            position.CreatePositionsByOffset(-2, 1, 1),
            position.CreatePositionsByOffset(-1, 2, 1),
            
            //좌측하단
            position.CreatePositionsByOffset(1, -2, 1),
            position.CreatePositionsByOffset(2, -1, 1),
            
            //우측하단
            position.CreatePositionsByOffset(2, 1, 1),
            position.CreatePositionsByOffset(1, 2, 1)
            };
        }
        
        #endregion
        
    }