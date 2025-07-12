using LChess.Models.Chess.Unit.Base;

using LChess.Models.Chess.Board;

using LChess.Util.Enums;

namespace LChess.Models.Chess.Unit;

/// <summary>
/// King 기물 모델
/// </summary>
public partial class KingModel : ChessUnitModelBase
{
    #region :: Constructor ::

    public KingModel(PieceColorType pieceColorType, ChessPosition position) : base(ChessUnitType.King, pieceColorType, position)
    {

    }

    #endregion

    #region Methods


    #endregion

}
