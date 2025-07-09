using LChess.Models.Chess.Base;

using LChess.Util.Enums;

namespace LChess.Models.Chess;

/// <summary>
/// Rook 기물 모델
/// </summary>
public partial class RookModel : ChessUnitModelBase
{
    #region :: Constructor ::

    public RookModel(PieceColorType pieceColorType) : base(ChessUnitType.Rook, pieceColorType)
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
        return new List<List<ChessPosition>>()
        {
            position.GetTopLinePositions   (),
            position.GetLeftLinePositions  (),
            position.GetBottomLinePositions(),
            position.GetRightLinePositions ()
        };
    }

    #endregion
}
