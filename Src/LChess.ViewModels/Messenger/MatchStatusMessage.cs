using LChess.Models.Chess.Match;

namespace LChess.ViewModels.Messenger;

/// <summary>
/// 체스게임 상태변경 메시지
/// </summary>
public class MatchStatusMessage : ValueChangedMessage<MatchStatusModel>
{
    public MatchStatusMessage(MatchStatusModel value) : base(value) { }
}
