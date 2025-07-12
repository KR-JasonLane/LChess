using LChess.Models.Chess.Board;

using LChess.Models.Chess.Route.Base;

using LChess.Util.Enums;

namespace LChess.Models.Chess.Route;

/// <summary>
/// 폰 이동경로 모델
/// </summary>
public class PawnRouteModel : ChessUnitRouteModelBase
{
    #region :: Constructor ::

    /// <summary>
    /// 생성자
    /// </summary>
    public PawnRouteModel(ChessPosition position, PieceColorType color) : base(position, color)
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
           !! 폰은 직진 시 적이든, 아군이든 유닛이 없을 때만 이동이 가능하며,
              대각선 방향으로는 적 유닛이 있을 때만 이동이 가능.
           
           !! 특수한 케이스로 앙파상이 있음.

           !! 상대 진영 끝까지 이동 시 승격 이벤트도 처리해야함.
         */

        var position = new ChessPositionModel(_currentPosition);

        if( _unitColor == PieceColorType.White )
        {
            //백색 폰 경로 활성화
            WhiteRoute(position, tileMapper);
        }
        else if( _unitColor == PieceColorType.Black )
        {
            //흑색 폰 경로 활성화
            BlackRoute(position, tileMapper);
        }
    }

    /// <summary>
    /// 화이트 폰 이동경로
    /// </summary>
    /// <param name="positionModel"> 현재 위치 모델 </param>
    /// <param name="tileMapper"> 기물정보 맵퍼 </param>
    private void WhiteRoute(ChessPositionModel positionModel, Dictionary<ChessPosition, ChessBoardTileModel> tileMapper)
    {
        var isFirstMove = positionModel.Code.ToString().Contains("2");

        List<ChessPosition> streight      = positionModel.GetTopLinePositions         (isFirstMove ? 2 : 1);
        List<ChessPosition> leftDiagonal  = positionModel.GetLeftTopDiagonalPositions (1                  ); 
        List<ChessPosition> rightDiagonal = positionModel.GetRightTopDiagonalPositions(1                  );

        TurnOn(streight, leftDiagonal, rightDiagonal, tileMapper);
    }

    /// <summary>
    /// 블랙 폰 이동경로
    /// </summary>
    /// <param name="positionModel"> 현재 위치 모델 </param>
    /// <param name="tileMapper"> 기물정보 맵퍼 </param>
    private void BlackRoute(ChessPositionModel positionModel, Dictionary<ChessPosition, ChessBoardTileModel> tileMapper)
    {
        var isFirstMove = positionModel.Code.ToString().Contains("7");

        List<ChessPosition> streight      = positionModel.GetBottomLinePositions         (isFirstMove ? 2 : 1);
        List<ChessPosition> leftDiagonal  = positionModel.GetLeftBottomDiagonalPositions (1                  ); 
        List<ChessPosition> rightDiagonal = positionModel.GetRightBottomDiagonalPositions(1                  );

        TurnOn(streight, leftDiagonal, rightDiagonal, tileMapper);
    }

    /// <summary>
    /// 위치정보 기반 활성화
    /// </summary>
    /// <param name="streight"> 직진위치정보 </param>
    /// <param name="leftDiagonal"> 왼쪽 대각선 정보 </param>
    /// <param name="rightDiagonal"> 우측 대각선 정보 </param>
    /// <param name="tileMapper"> 기물정보 맵퍼 </param>
    private void TurnOn(List<ChessPosition> streight, List<ChessPosition> leftDiagonal, List<ChessPosition> rightDiagonal, Dictionary<ChessPosition, ChessBoardTileModel> tileMapper)
    {
        // 직진
        foreach (var go in streight)
        {
            // 타일이 존재하고, 비어있으면
            if (tileMapper.TryGetValue(go, out var tile) && tile != null)
            {
                //이동가능.
                tile.IsHighLightMove = tile.IsEmpty;

                //다음단계로 진행
                continue;
            }

            //타일이 비어있지 않거나 유효하지 않으면
            //루프 탈출.
            break;
        }

        //좌측 대각선
        if (leftDiagonal.Count == 1 && tileMapper.TryGetValue(leftDiagonal.First(), out var leftTile) && leftTile != null)
        {
            leftTile.IsHighLightEnemy = !leftTile.Unit?.IsSameColor(_unitColor) ?? false;
        }

        //우측 대각선
        if (rightDiagonal.Count == 1 && tileMapper.TryGetValue(rightDiagonal.First(), out var rightTile) && rightTile != null)
        {
            rightTile.IsHighLightEnemy = !rightTile.Unit?.IsSameColor(_unitColor) ?? false;
        }
    }

    #endregion
}
