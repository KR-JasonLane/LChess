using LChess.Util.Enums;

namespace LChess.ViewModels.Messenger;

/// <summary>
/// 현재 턴 변경 메시지
/// </summary>
public class CurrentTurnChangedMessage : ValueChangedMessage<PieceColorType>
{
    public CurrentTurnChangedMessage(PieceColorType turn) : base(turn) { }
}
