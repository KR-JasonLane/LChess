using LChess.Models.Chess.Board;

using LChess.Models.Chess.Route.Base;

using LChess.Util.Enums;
using System.Reflection.Metadata.Ecma335;

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
    /// <param name="managementModel"> 현재 위치/기물정보 맵퍼 </param>
    public override void TurnOnUnitRoute(BoardManagementModel managementModel)
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
            TurnOnWhiteRoute(position, managementModel);
        }
        else
        {
            //흑색 폰 경로 활성화
            TurnOnBlackRoute(position, managementModel);
        }
    }

    /// <summary>
    /// 화이트 폰 이동경로
    /// </summary>
    /// <param name="positionModel"> 현재 위치 모델 </param>
    /// <param name="managementModel"> 기물정보 맵퍼 </param>
    private void TurnOnWhiteRoute(ChessPositionModel positionModel, BoardManagementModel managementModel)
    {
        var isFirstMove = positionModel.Code.ToString().Contains("2");

        List<ChessPosition> streight      = positionModel.GetTopLinePositions         (isFirstMove ? 2 : 1);
        List<ChessPosition> leftDiagonal  = positionModel.GetLeftTopDiagonalPositions (1                  ); 
        List<ChessPosition> rightDiagonal = positionModel.GetRightTopDiagonalPositions(1                  );

        TurnOn(streight, leftDiagonal, rightDiagonal, managementModel);
    }

    /// <summary>
    /// 블랙 폰 이동경로
    /// </summary>
    /// <param name="positionModel"> 현재 위치 모델 </param>
    /// <param name="managementModel"> 기물정보 맵퍼 </param>
    private void TurnOnBlackRoute(ChessPositionModel positionModel, BoardManagementModel managementModel)
    {
        var isFirstMove = positionModel.Code.ToString().Contains("7");

        List<ChessPosition> streight      = positionModel.GetBottomLinePositions         (isFirstMove ? 2 : 1);
        List<ChessPosition> leftDiagonal  = positionModel.GetLeftBottomDiagonalPositions (1                  ); 
        List<ChessPosition> rightDiagonal = positionModel.GetRightBottomDiagonalPositions(1                  );

        TurnOn(streight, leftDiagonal, rightDiagonal, managementModel);
    }

    /// <summary>
    /// 위치정보 기반 활성화
    /// </summary>
    /// <param name="streight"> 직진위치정보 </param>
    /// <param name="leftDiagonal"> 왼쪽 대각선 정보 </param>
    /// <param name="rightDiagonal"> 우측 대각선 정보 </param>
    /// <param name="managementModel"> 기물정보 맵퍼 </param>
    private void TurnOn(List<ChessPosition> streight, List<ChessPosition> leftDiagonal, List<ChessPosition> rightDiagonal, BoardManagementModel managementModel)
    {
        // 직진
        foreach (var go in streight)
        {
            // 타일이 존재하고, 비어있으면
            if (managementModel.TryGetTile(go, out var tile) && tile != null)
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
        if (leftDiagonal.Count == 1 && managementModel.TryGetTile(leftDiagonal.First(), out var leftTile) && leftTile != null)
        {
            leftTile.IsHighLightEnemy = leftTile.Unit?.IsSameColor(_unitColor) == false;
        }

        //우측 대각선
        if (rightDiagonal.Count == 1 && managementModel.TryGetTile(rightDiagonal.First(), out var rightTile) && rightTile != null)
        {
            rightTile.IsHighLightEnemy = rightTile.Unit?.IsSameColor(_unitColor) == false;
        }
    }

    /// <summary>
    /// 움직일 수 있는 경우의 수를 반환
    /// </summary>
    /// <param name="managementModel"> 현재 위치/기물정보 맵퍼 </param>
    /// <returns> 유닛 경로 상 현재 움직일 수 있는 모든 경우의수 </returns>
    public override List<ChessPosition> GetMovablePositions(BoardManagementModel managementModel)
    {
        var position = new ChessPositionModel(_currentPosition);

        if (_unitColor == PieceColorType.White)
        {
            //백색 폰 경로 활성화
            return MovableWhiteRoute(position, managementModel);
        }
        else
        {
            //흑색 폰 경로 활성화
            return MovableBlackRoute(position, managementModel);
        }
    }

    /// <summary>
    /// 화이트폰 이동 가능경로 반환
    /// </summary>
    /// <param name="positionModel"> 현재 위치 모델 </param>
    /// <param name="managementModel"> 기물정보 맵퍼 </param>
    /// <returns> 이동 가능한 경우의 수 </returns>
    private List<ChessPosition> MovableWhiteRoute(ChessPositionModel positionModel, BoardManagementModel managementModel)
    {
        var isFirstMove = positionModel.Code.ToString().Contains("2");

        List<ChessPosition> streight      = positionModel.GetTopLinePositions         (isFirstMove ? 2 : 1);
        List<ChessPosition> leftDiagonal  = positionModel.GetLeftTopDiagonalPositions (1                  ); 
        List<ChessPosition> rightDiagonal = positionModel.GetRightTopDiagonalPositions(1                  );

        return GetMovable(streight, leftDiagonal, rightDiagonal, managementModel);
    }

    /// <summary>
    /// 블랙폰 이동 가능경로 반환
    /// </summary>
    /// <param name="positionModel"> 현재 위치 모델 </param>
    /// <param name="managementModel"> 기물정보 맵퍼 </param>
    /// <returns> 이동 가능한 경우의 수 </returns>
    private List<ChessPosition> MovableBlackRoute(ChessPositionModel positionModel, BoardManagementModel managementModel)
    {
        var isFirstMove = positionModel.Code.ToString().Contains("7");

        List<ChessPosition> streight      = positionModel.GetBottomLinePositions         (isFirstMove ? 2 : 1);
        List<ChessPosition> leftDiagonal  = positionModel.GetLeftBottomDiagonalPositions (1                  );
        List<ChessPosition> rightDiagonal = positionModel.GetRightBottomDiagonalPositions(1                  );

        return GetMovable(streight, leftDiagonal, rightDiagonal, managementModel);
    }

    /// <summary>
    /// 이동 가능한 경우의 수를 판단하여 반환
    /// </summary>
    /// <param name="streight"> 직진위치정보 </param>
    /// <param name="leftDiagonal"> 왼쪽 대각선 정보 </param>
    /// <param name="rightDiagonal"> 우측 대각선 정보 </param>
    /// <param name="managementModel"> 기물정보 맵퍼 </param>
    /// <returns> 이동 가능한 경우의 수 </returns>
    private List<ChessPosition> GetMovable(List<ChessPosition> streight, List<ChessPosition> leftDiagonal, List<ChessPosition> rightDiagonal, BoardManagementModel managementModel)
    {
        List<ChessPosition> result = new();
        // 직진
        foreach (var go in streight)
        {
            // 타일이 존재
            if (managementModel.TryGetTile(go, out var tile) && tile != null)
            {
                //비어있으면 경로에 추가 후 계속 진행
                if (tile.IsEmpty)
                {
                    result.Add(go);

                    continue;
                }

                //비어있지 않으면 루프 탈출.
                break;
            }
        }

        //좌측 대각선
        if (leftDiagonal.Count == 1 && managementModel.TryGetTile(leftDiagonal.First(), out var leftTile) && leftTile != null)
        {
            if (leftTile.Unit?.IsSameColor(_unitColor) == false)
            {
                result.Add(leftTile.Position.Code);
            }
        }

        //우측 대각선
        if (rightDiagonal.Count == 1 && managementModel.TryGetTile(rightDiagonal.First(), out var rightTile) && rightTile != null)
        {
            if (rightTile.Unit?.IsSameColor(_unitColor) == false)
            {
                result.Add(rightTile.Position.Code);
            }
        }
        return result;
    }

    #endregion
}
