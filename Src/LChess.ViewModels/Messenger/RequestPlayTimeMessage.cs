namespace LChess.ViewModels.Messenger;

/// <summary>
/// 플레이시간 요청 메시지
/// </summary>
public class RequestPlayTimeMessage : RequestMessage<TimeSpan>
{
    public RequestPlayTimeMessage() { }
}
