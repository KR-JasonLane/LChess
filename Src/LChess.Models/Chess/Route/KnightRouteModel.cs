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
    public override void TurnOnUnitRoute(Dictionary<ChessPosition, ChessBoardTileModel> tileMapper)
    {
        /*
          
           !! 나이트는 기물을 뛰어넘을 수 있고, 특수한 이동 형태를 가지고 있음.        
         
         */

        var position = new ChessPositionModel(_currentPosition);

        var positions = new List<List<ChessPosition>>()
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

        //방향별 위치 루프
        foreach (var line in positions)
        {
            //각개 위치 루프
            foreach (var go in line)
            {
                if (tileMapper.TryGetValue(go, out var tile) && tile != null)
                {
                    //이동경로 표시여부 판단
                    tile.IsHighLightMove = tile.IsEmpty;

                    //타일이 비어있지 않으면, 적군인지만 판단 후
                    tile.IsHighLightEnemy = !tile.IsEmpty && (!tile.Unit?.IsSameColor(_unitColor) ?? false);
                }
            }
        }
    }

    #endregion

}