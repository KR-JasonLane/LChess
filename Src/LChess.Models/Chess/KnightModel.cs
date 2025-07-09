using LChess.Models.Chess.Base;

using LChess.Util.Enums;

namespace LChess.Models.Chess;

/// <summary>
/// Knight 기물 모델
/// </summary>
public partial class KnightModel : ChessUnitModelBase
{
    #region :: Constructor ::

    public KnightModel(PieceColorType pieceColorType) : base(ChessUnitType.Knight, pieceColorType)
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
