using LChess.Models.Chess.Base;

using LChess.Util.Enums;

namespace LChess.Models.Chess;

/// <summary>
/// King 기물 모델
/// </summary>
public partial class KingModel : ChessUnitModelBase
{
    #region :: Constructor ::

    public KingModel(PieceColorType pieceColorType) : base(ChessUnitType.King, pieceColorType)
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


        return result;
    }

    #endregion

}
