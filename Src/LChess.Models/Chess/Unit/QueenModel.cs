using LChess.Models.Chess.Unit.Base;

using LChess.Models.Chess.Board;

using LChess.Util.Enums;

namespace LChess.Models.Chess.Unit;

/// <summary>
/// Queen 기물 모델
/// </summary>
public partial class QueenModel : ChessUnitModelBase
{
    #region :: Constructor ::

    public QueenModel(PieceColorType pieceColorType, ChessPosition position) : base(ChessUnitType.Queen, pieceColorType, position)
    {

    }

    #endregion

    #region Methods

    #endregion

}
