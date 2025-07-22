using LChess.Models.Result;

namespace LChess.Abstract.Service;

/// <summary>
/// 팝업윈도우 서비스 인터페이스
/// </summary>
public interface IPopupWindowService
{
    /// <summary>
    /// 메시지 팝업윈도우 띄우기
    /// 취소버튼 텍스트가 비어있으면 확인버튼만 표시.
    /// </summary>
    /// <param name="message"> 전달할 메시지 </param>
    /// <param name="okButtonContent"> 확인버튼 텍스트 </param>
    /// <param name="cancelButtonContent"> 취소버튼 텍스트 </param>
    public DialogResultModel? ShowMessagePopup(string message, string okButtonContent, string cancelButtonContent);
    
    /// <summary>
    /// 현재 띄워져있는 팝업 윈도우 닫기
    /// </summary>
    public void CloseCurrentPopupWinodw();
}
