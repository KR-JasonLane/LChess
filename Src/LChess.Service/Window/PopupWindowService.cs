using LChess.Abstract.Service;

using LChess.Models.Result;

using LChess.ViewModels.DataContext.Popup;

using LChess.Views.Popup;

namespace LChess.Service.Window;

/// <summary>
/// 팝업윈도우 서비스
/// </summary>
public class PopupWindowService : IPopupWindowService
{
    #region :: Constructor ::
    
    /// <summary>
    /// 생성자
    /// </summary>
    public PopupWindowService()
    {
        
    }
    
    #endregion
    
    #region :: Properties ::
    
    private System.Windows.Window? _currnetPopup;
    
    #endregion
    
    #region :: Methods ::
    
    /// <summary>
    /// 메시지 팝업윈도우 띄우기
    /// 취소버튼 텍스트가 비어있으면 확인버튼만 표시.
    /// </summary>
    /// <param name="message"> 전달할 메시지 </param>
    /// <param name="okButtonContent"> 확인버튼 텍스트 </param>
    /// <param name="cancelButtonContent"> 취소버튼 텍스트 </param>
    public DialogResultModel? ShowMessagePopup(string message, string okButtonContent, string cancelButtonContent)
    {
        if(_currnetPopup != null)
        {
            CloseCurrentPopupWinodw();
        }
        
        var popupViewModel = Ioc.Default.GetRequiredService<MessageDialogPopupWindowViewModel>();
        
        popupViewModel.Message             = message            ;
        popupViewModel.OkButtonContent     = okButtonContent    ;
        popupViewModel.CancelButtonContent = cancelButtonContent;
        
        var popupView = new MessageDialogPopupWindowView()
        {
            DataContext = popupViewModel,
            Owner = Application.Current.MainWindow
            };
            
            _currnetPopup = popupView;
            
            popupView.ShowDialog();
            
            return popupViewModel.Result;
        }
        
        /// <summary>
        /// 현재 띄워져있는 팝업 윈도우 닫기
        /// </summary>
        public void CloseCurrentPopupWinodw()
        {
            _currnetPopup?.Close();
            _currnetPopup = null;
        }
        
        #endregion
    }
