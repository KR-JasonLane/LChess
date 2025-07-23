using LChess.Abstract.Service;
using LChess.Models.Result;

namespace LChess.ViewModels.DataContext.Popup;

/// <summary>
/// 폰 승격 선택 팝업 윈도우 뷰모델
/// </summary>
public partial class SelectPromotionUnitPopupWindowViewModel : ObservableRecipient
{
    #region :: Constructor ::

    public SelectPromotionUnitPopupWindowViewModel(IPopupWindowService popupWindowService)
    {
        ////////////////////////////////////////
        // 서비스 등록
        ////////////////////////////////////////
        {
            _popupWindowService = popupWindowService;
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

    #endregion


    #region :: Methods ::

    #endregion

    #region :: Commands ::

    /// <summary>
    /// 폰 승격선택
    /// </summary>
    /// <param name="param"> 승격 코드 </param>
    [RelayCommand]
    private void SelectPromotion(object param)
    {
        if(param is string promotionCode)
        {
            Result = new DialogResultModel(true, promotionCode);

            _popupWindowService.CloseCurrentPopupWinodw();
        }
    }

    #endregion
}
