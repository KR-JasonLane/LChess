using LChess.Models.Chess.Unit.Base;

using LChess.Models.Chess.Board;

using LChess.Util.Enums;

namespace LChess.Models.Chess.Unit;

/// <summary>
/// Bishop 기물 모델
/// </summary>
public partial class BishopModel : ChessUnitModelBase
{
    #region :: Constructor ::

    public BishopModel(PieceColorType pieceColorType, ChessPosition position) : base(ChessUnitType.Bishop, pieceColorType, position)
    {

    }

    #endregion

    #region Methods

    #endregion

}
