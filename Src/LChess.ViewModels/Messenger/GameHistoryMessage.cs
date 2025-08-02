using LChess.Models.Result;

namespace LChess.ViewModels.Messenger;

/// <summary>
/// 게임 결과 메시지
/// </summary>
public partial class GameHistoryMessage : ValueChangedMessage<GameHistoryFileModel>
{
    public GameHistoryMessage(GameHistoryFileModel value) : base(value) { }
}
