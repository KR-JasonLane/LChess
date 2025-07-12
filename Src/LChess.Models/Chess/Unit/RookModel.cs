using LChess.Models.Chess.Unit.Base;

using LChess.Models.Chess.Board;

using LChess.Util.Enums;

namespace LChess.Models.Chess.Unit;

/// <summary>
/// Rook 기물 모델
/// </summary>
public partial class RookModel : ChessUnitModelBase
{
    #region :: Constructor ::

    public RookModel(PieceColorType pieceColorType, ChessPosition position) : base(ChessUnitType.Rook, pieceColorType, position)
    {

    }

    #endregion

    #region Methods


    #endregion
}
