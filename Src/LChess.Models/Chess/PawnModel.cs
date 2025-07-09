using LChess.Models.Chess.Base;

using LChess.Util.Enums;

namespace LChess.Models.Chess;

/// <summary>
/// Pawn 기물 모델
/// </summary>
public partial class PawnModel : ChessUnitModelBase
{
    #region :: Constructor ::

    public PawnModel(PieceColorType pieceColorType) : base(ChessUnitType.Pawn, pieceColorType)
    {

    }

    #endregion

    #region Methods

    /// <summary>
    /// 기물이 움직일 수 있는 영역을 반환
    /// </summary>
    /// <returns> 각 방향마다의 포지션 리스트를 담은 리스트 </returns>
    public override List<List<ChessPosition>> GetAvailablePositions(ChessPositionModel position, Dictionary<ChessPosition, ChessBoardTileModel> mapper)
    {
        var result = new List<List<ChessPosition>>();

        List<ChessPosition> leftDiagonal ;
        List<ChessPosition> rightDiagonal;

        if (ColorType == PieceColorType.White)
        {
            int moveCount = position.Code.ToString().Contains('2') ? 2 : 1;

            result.Add(position.GetTopLinePositions(moveCount));

            leftDiagonal  = position.GetLeftTopDiagonalPositions (1);
            rightDiagonal = position.GetRightTopDiagonalPositions(1);
        }
        else
        {
            int moveCount = position.Code.ToString().Contains('7') ? 2 : 1;

            result.Add(position.GetBottomLinePositions(moveCount));

            leftDiagonal  = position.GetLeftBottomDiagonalPositions (1);
            rightDiagonal = position.GetRightBottomDiagonalPositions(1);
        }

        // 폰은 대각선으로 이동할 때만 적군을 공격할 수 있으므로 적 기물 존재 여부에 따라 경로를 표시해준다.
        if (leftDiagonal.Count > 0 && mapper.TryGetValue(leftDiagonal.First(), out var leftTile) && IsEnemy(leftTile.Unit))
        {
            result.Add(leftDiagonal);
        }
        if (rightDiagonal.Count > 0 && mapper.TryGetValue(rightDiagonal.First(), out var rightTile) && IsEnemy(rightTile.Unit))
        {
            result.Add(rightDiagonal);
        }

        return result;
    }

    #endregion
}
