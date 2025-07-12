using LChess.Models.Chess.Unit.Base;

using LChess.Models.Chess.Board;

using LChess.Util.Enums;

namespace LChess.Models.Chess.Unit;

/// <summary>
/// Knight 기물 모델
/// </summary>
public partial class KnightModel : ChessUnitModelBase
{
    #region :: Constructor ::

    public KnightModel(PieceColorType pieceColorType, ChessPosition position) : base(ChessUnitType.Knight, pieceColorType, position)
    {

    }

    #endregion

    #region Methods

    #endregion

}
