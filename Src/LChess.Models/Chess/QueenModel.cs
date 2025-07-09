using LChess.Models.Chess.Base;

using LChess.Util.Enums;

namespace LChess.Models.Chess;

/// <summary>
/// Queen 기물 모델
/// </summary>
public partial class QueenModel : ChessUnitModelBase
{
    #region :: Constructor ::

    public QueenModel(PieceColorType pieceColorType) : base(ChessUnitType.Queen, pieceColorType)
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
            position.GetTopLinePositions            (),
            position.GetLeftTopDiagonalPositions    (),
            position.GetLeftLinePositions           (),
            position.GetLeftBottomDiagonalPositions (),
            position.GetBottomLinePositions         (),
            position.GetRightBottomDiagonalPositions(),
            position.GetRightLinePositions          (),
            position.GetRightTopDiagonalPositions   ()
        };
    }

    #endregion

}
