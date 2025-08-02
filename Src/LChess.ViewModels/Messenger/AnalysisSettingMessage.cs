using LChess.Models.Result;

namespace LChess.ViewModels.Messenger;

/// <summary>
/// 분석화면 세팅 메시지
/// </summary>
public class AnalysisSettingMessage : ValueChangedMessage<GameHistoryFileModel>
{
    public AnalysisSettingMessage(GameHistoryFileModel model) : base(model) { }
}
