using LChess.Util.Enums;

namespace LChess.ViewModels.Messenger;

/// <summary>
/// 게임종료 메시지 (체크메이트, 무승부)
/// </summary>
public class EndGameMessage : ValueChangedMessage<EndGameType>
{
    public EndGameMessage(EndGameType type) : base(type) { }
}
