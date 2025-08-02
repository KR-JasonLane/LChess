using LChess.Models.Chess.Match;

namespace LChess.ViewModels.Messenger;

/// <summary>
/// 기보대로 강제 이동시키라는 메시지
/// </summary>
public class ForceAnalysisMoveMessage : ValueChangedMessage<List<NotationModel>>
{
    public ForceAnalysisMoveMessage(List<NotationModel> notations) : base(notations) { }
}
