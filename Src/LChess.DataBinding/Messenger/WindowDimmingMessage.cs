namespace LChess.ViewModels.Messenger;

/// <summary>
/// 윈도우 Dim 처리 상호작용 메시지
/// </summary>
public class WindowDimmingMessage : ValueChangedMessage<bool>
{
	public WindowDimmingMessage(bool value) : base(value) { }
}
