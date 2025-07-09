using LChess.Models.Chess.Base;

using LChess.Util.Enums;

namespace LChess.Models.Chess;

/// <summary>
/// Bishop 기물 모델
/// </summary>
public partial class BishopModel : ChessUnitModelBase
{
    #region :: Constructor ::

    public BishopModel(PieceColorType pieceColorType) : base(ChessUnitType.Bishop, pieceColorType)
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
            position.GetLeftTopDiagonalPositions    (),
            position.GetLeftBottomDiagonalPositions (),
            position.GetRightBottomDiagonalPositions(),
            position.GetRightTopDiagonalPositions   ()
        };
    }

    #endregion

}
