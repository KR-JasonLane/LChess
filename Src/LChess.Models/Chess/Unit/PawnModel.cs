using LChess.Models.Chess.Unit.Base;

using LChess.Util.Enums;

namespace LChess.Models.Chess.Unit;

/// <summary>
/// Pawn 기물 모델
/// </summary>
public partial class PawnModel : ChessUnitModelBase
{
    #region :: Constructor ::

    public PawnModel(PieceColorType pieceColorType, ChessPosition position) : base(ChessUnitType.Pawn, pieceColorType, position)
    {

    }

    #endregion

    #region Methods

    #endregion
}
