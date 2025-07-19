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

    public BishopModel(PieceColorType pieceColorType, ChessPosition position, char originalCode) : base(ChessUnitType.Bishop, pieceColorType, position, originalCode)
    {

    }

    #endregion

    #region Methods

    #endregion

}
