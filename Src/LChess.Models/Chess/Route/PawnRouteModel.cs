using LChess.Models.Chess.Board;

using LChess.Models.Chess.Route.Base;
using LChess.Models.Chess.Unit;
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
    /// <param name="managementModel"> 현재 위치/기물정보 맵퍼 </param>
    public override void TurnOnUnitRoute(BoardManagementModel managementModel)
    {
        /*
           !! 폰은 직진 시 적이든, 아군이든 유닛이 없을 때만 이동이 가능하며,
              대각선 방향으로는 적 유닛이 있을 때만 이동이 가능.
           
           !! 특수이동 : 앙파상
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
        MovablePositionsAtWhite(positionModel, managementModel, out var streight, out var leftDiagonal, out var rightDiagonal, out var enPassant);

        TurnOn(streight, leftDiagonal, rightDiagonal, enPassant, managementModel);
    }

    /// <summary>
    /// 블랙 폰 이동경로
    /// </summary>
    /// <param name="positionModel"> 현재 위치 모델 </param>
    /// <param name="managementModel"> 기물정보 맵퍼 </param>
    private void TurnOnBlackRoute(ChessPositionModel positionModel, BoardManagementModel managementModel)
    {
        MovablePositionsAtBlack(positionModel, managementModel, out var streight, out var leftDiagonal, out var rightDiagonal, out var enPassant);

        TurnOn(streight, leftDiagonal, rightDiagonal, enPassant, managementModel);
    }

    /// <summary>
    /// 위치정보 기반 활성화
    /// </summary>
    /// <param name="streight"> 직진위치정보 </param>
    /// <param name="leftDiagonal"> 왼쪽 대각선 정보 </param>
    /// <param name="rightDiagonal"> 우측 대각선 정보 </param>
    /// <param name="managementModel"> 기물정보 맵퍼 </param>
    private void TurnOn(List<ChessPosition> streight, List<ChessPosition> leftDiagonal, List<ChessPosition> rightDiagonal, ChessPosition enPassant, BoardManagementModel managementModel)
    {
        // 직진
        foreach (var go in streight)
        {
            // 타일이 존재하고, 비어있으면
            if (managementModel.TryGetTile(go, out var tile) && tile != null && tile.IsEmpty)
            {
                //이동가능.
                tile.IsHighLightMove = true;

                //다음단계로 진행
                continue;
            }

            //타일이 비어있지 않거나 유효하지 않으면
            //루프 탈출.
            break;
        }

        //앙파상 위치 활성화
        if(enPassant != ChessPosition.Invalid && managementModel.TryGetTile(enPassant, out var tile) && tile != null)
        {
            tile.IsHighLightMove = true;
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
        MovablePositionsAtWhite(positionModel, managementModel, out var streight, out var leftDiagonal, out var rightDiagonal, out var enPassant);

        return GetMovable(streight, leftDiagonal, rightDiagonal, enPassant, managementModel);
    }

    /// <summary>
    /// 블랙폰 이동 가능경로 반환
    /// </summary>
    /// <param name="positionModel"> 현재 위치 모델 </param>
    /// <param name="managementModel"> 기물정보 맵퍼 </param>
    /// <returns> 이동 가능한 경우의 수 </returns>
    private List<ChessPosition> MovableBlackRoute(ChessPositionModel positionModel, BoardManagementModel managementModel)
    {
        MovablePositionsAtBlack(positionModel, managementModel, out var streight, out var leftDiagonal, out var rightDiagonal, out var enPassant);

        return GetMovable(streight, leftDiagonal, rightDiagonal, enPassant, managementModel);
    }


    /// <summary>
    /// 이전 기보를 기반으로 앙파상 가능한 위치를 생성
    /// </summary>
    /// <param name="managementModel">보드 관리 모델</param>
    /// <returns>앙파상 이동 가능 위치 리스트</returns>
    private ChessPosition CreateEnPassantPosition(BoardManagementModel managementModel)
    {
        var result = ChessPosition.Invalid;

        //이전 기보 획득
        var notation = managementModel.PreviousNotation;

        // 앙파상 기보는 "e3e4" 형식으로, 최소 4글자 이상이어야 함
        if (string.IsNullOrEmpty(notation) || notation.Length < 4) 
            return result;

        //위치 PositionType으로 파싱
        if (!Enum.TryParse(notation.Substring(0, 2).ToUpper(), out ChessPosition from) ||
            !Enum.TryParse(notation.Substring(2, 2).ToUpper(), out ChessPosition to))
            return result;

        //파싱된 위치가 유효하지 않으면 진행불가.
        if (from == ChessPosition.Invalid || to == ChessPosition.Invalid) 
            return result;

        //적 폰 위치 확인
        if (managementModel.TryGetTile(to, out var movedTile) && 
            movedTile?.Unit is PawnModel enemyPawn && 
            enemyPawn.ColorType != _unitColor)
        {
            var fromPos = new ChessPositionModel(from);
            var toPos   = new ChessPositionModel(to  );

            // 앙파상 조건: 폰이 두 칸 전진했는지 확인
            if (Math.Abs(fromPos.Row - toPos.Row) != 2) 
                return result;

            var myPos = new ChessPositionModel(_currentPosition);

            // 현재 말이 상대 폰 옆에 있는지 확인
            if (myPos.Row != toPos.Row || Math.Abs(myPos.Column - toPos.Column) != 1) 
                return result;

            var rowOffset = _unitColor == PieceColorType.White ? -1 : 1;

            return ChessPositionModel.CalcPositionCode(toPos.Row + rowOffset, toPos.Column);
        }       

        return result;
    }


    /// <summary>
    /// 이동 가능한 경우의 수를 판단하여 반환
    /// </summary>
    /// <param name="streight"> 직진위치정보 </param>
    /// <param name="leftDiagonal"> 왼쪽 대각선 정보 </param>
    /// <param name="rightDiagonal"> 우측 대각선 정보 </param>
    /// <param name="managementModel"> 기물정보 맵퍼 </param>
    /// <returns> 이동 가능한 경우의 수 </returns>
    private List<ChessPosition> GetMovable(List<ChessPosition> streight, List<ChessPosition> leftDiagonal, List<ChessPosition> rightDiagonal, ChessPosition enPassant, BoardManagementModel managementModel)
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

        //앙파상 위치 포함
        if (enPassant != ChessPosition.Invalid)
        {
            result.Add(enPassant);
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

    /// <summary>
    /// 화이트일때 이동가능한 경로 구하기
    /// </summary>
    /// <param name="positionModel"> 포지션모델 </param>
    /// <param name="streight"> 직진 </param>
    /// <param name="leftDiagonal"> 좌측대각선 </param>
    /// <param name="rightDiagonal"> 우측대각선 </param>
    private void MovablePositionsAtWhite(ChessPositionModel positionModel, BoardManagementModel managementModel, out List<ChessPosition> streight, out List<ChessPosition> leftDiagonal, out List<ChessPosition> rightDiagonal, out ChessPosition enPassant)
    {
        var isFirstMove = positionModel.Code.ToString().Contains("2");

        streight      = positionModel.GetTopLinePositions         (isFirstMove ? 2 : 1);
        leftDiagonal  = positionModel.GetLeftTopDiagonalPositions (1                  ); 
        rightDiagonal = positionModel.GetRightTopDiagonalPositions(1                  );

        enPassant = CreateEnPassantPosition(managementModel);
    }

    /// <summary>
    /// 블랙일때 이동가능한 경로 구하기
    /// </summary>
    /// <param name="positionModel"> 포지션모델 </param>
    /// <param name="streight"> 직진 </param>
    /// <param name="leftDiagonal"> 좌측대각선 </param>
    /// <param name="rightDiagonal"> 우측대각선 </param>
    private void MovablePositionsAtBlack(ChessPositionModel positionModel, BoardManagementModel managementModel, out List<ChessPosition> streight, out List<ChessPosition> leftDiagonal, out List<ChessPosition> rightDiagonal, out ChessPosition enPassant)
    {
        var isFirstMove = positionModel.Code.ToString().Contains("7");

        streight      = positionModel.GetBottomLinePositions         (isFirstMove ? 2 : 1);
        leftDiagonal  = positionModel.GetLeftBottomDiagonalPositions (1                  ); 
        rightDiagonal = positionModel.GetRightBottomDiagonalPositions(1                  );

        enPassant = CreateEnPassantPosition(managementModel);
    }

    #endregion
}
