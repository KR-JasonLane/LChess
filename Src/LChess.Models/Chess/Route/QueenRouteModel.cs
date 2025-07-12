using LChess.Models.Chess.Board;
using LChess.Models.Chess.Route.Base;
using LChess.Util.Enums;

namespace LChess.Models.Chess.Route;

public class QueenRouteModel : ChessUnitRouteModelBase
{
    #region :: Constructor ::

    /// <summary>
    /// 생성자
    /// </summary>
    public QueenRouteModel(ChessPosition position, PieceColorType color) : base(position, color)
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
    public override void TurnOnUnitRoute(Dictionary<ChessPosition, ChessBoardTileModel> tileMapper)
    {
        /*
          
           !! 퀸은 모든 방향 이동이 가능.       
         
         */

        var position = new ChessPositionModel(_currentPosition);

        //각 방향별 대각선 위치 생성
        List<List<ChessPosition>> positions = new List<List<ChessPosition>>()
        {
            position.GetTopLinePositions            (),
            position.GetLeftTopDiagonalPositions    (),
            position.GetLeftLinePositions           (),
            position.GetLeftBottomDiagonalPositions (),
            position.GetBottomLinePositions         (),
            position.GetRightTopDiagonalPositions   (),
            position.GetRightLinePositions          (),
            position.GetRightBottomDiagonalPositions(),
        };

        //방향별 위치 루프
        foreach (var line in positions)
        {
            //각개 위치 루프
            foreach (var go in line)
            {
                if (tileMapper.TryGetValue(go, out var tile) && tile != null)
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
                    tile.IsHighLightEnemy = !tile.Unit?.IsSameColor(_unitColor) ?? false;

                    // 루프 탈출
                    break;
                }
            }
        }
    }

    #endregion

}
