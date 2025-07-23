using LChess.Abstract.Service;
using LChess.Models.Result;

namespace LChess.ViewModels.DataContext.Popup;

/// <summary>
/// 메시지 다이얼로그 팝업윈도우 뷰모델
/// </summary>
public partial class MessageDialogPopupWindowViewModel : ObservableRecipient
{    
    #region :: Constructor ::

    /// <summary>
    /// 생성자
    /// </summary>
    public MessageDialogPopupWindowViewModel(IPopupWindowService popupWindowService)
    {
        ////////////////////////////////////////
        // 서비스 등록
        ////////////////////////////////////////
        {
            _popupWindowService = popupWindowService;
        }

        ////////////////////////////////////////
        // 속성 초기화
        ////////////////////////////////////////
        { 
            Message = string.Empty;

            OkButtonContent     = "확인";
            CancelButtonContent = "취소";
        }
    }

    #endregion

    #region :: Services ::

    /// <summary>
    /// 팝업윈도우 서비스
    /// </summary>
    private readonly IPopupWindowService _popupWindowService;

    #endregion

    #region :: Properties ::

    /// <summary>
    /// 다이얼로그 결과
    /// </summary>
    public DialogResultModel? Result { get; private set; }

    /// <summary>
    /// 보여줄 메시지
    /// </summary>
    [ObservableProperty]
    private string _message;

    /// <summary>
    /// 확인버튼
    /// </summary>
    [ObservableProperty]
    private string _okButtonContent;

    /// <summary>
    /// 취소버튼
    /// </summary>
    [ObservableProperty]
    private string _cancelButtonContent;

    #endregion


    #region :: Methods ::

    #endregion

    #region :: Commands ::

    /// <summary>
    /// 확인버튼 클릭
    /// </summary>
    [RelayCommand]
    private void Ok()
    {
        Result = new DialogResultModel(true, string.Empty);

        _popupWindowService.CloseCurrentPopupWinodw();
    }

    /// <summary>
    /// 취소버튼 클릭
    /// </summary>
    [RelayCommand]
    private void Cancel()
    {
        Result = new DialogResultModel(false, string.Empty);

        _popupWindowService.CloseCurrentPopupWinodw();
    }

    #endregion
}
