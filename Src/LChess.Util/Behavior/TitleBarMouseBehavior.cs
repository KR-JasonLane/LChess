namespace LChess.Util.Behavior;

/// <summary>
/// 타이틀바 마우스 동작을 처리하는 Behavior 클래스
/// </summary>
public class TitleBarMouseBehavior : Behavior<FrameworkElement>
{
    #region :: Attaching / Detaching ::

    /// <summary>
    /// 활성화
    /// </summary>
    protected override void OnAttached()
    {
        AssociatedObject.MouseLeftButtonDown += AssociatedObjectLeftButtonDown;
    }

    /// <summary>
    /// 비활성화
    /// </summary>
    protected override void OnDetaching()
    {
        AssociatedObject.MouseLeftButtonDown -= AssociatedObjectLeftButtonDown;
    }

    #endregion

    #region :: Constants ::

    /// <summary>
    /// 타이틀바 높이 보정값 (픽셀)
    /// </summary>
    private const double TitleBarHeightOffset = 20;

    #endregion

    #region :: Methods ::

    /// <summary>
    /// 메인 윈도우를 가져옵니다. 윈도우가 없으면 null을 반환합니다.
    /// </summary>
    private static Window? GetMainWindow()
    {
        return Application.Current.MainWindow;
    }

    /// <summary>
    /// 마우스 왼쪽 버튼 클릭 이벤트 핸들러
    /// </summary>
    /// <param name="sender">타이틀바 FrameworkElement</param>
    /// <param name="e">마우스 이벤트 파라미터</param>
    private void AssociatedObjectLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        var mainWindow = GetMainWindow();
        if (mainWindow is null)
            return;

        var windowState = mainWindow.WindowState;

        if (e.ClickCount == 2)
        {
            ToggleWindowState(mainWindow, windowState);
            return;
        }

        if (windowState == WindowState.Maximized)
        {
            RestoreWindowFromMaximized(mainWindow, e);
        }

        mainWindow.DragMove();
    }

    /// <summary>
    /// 창 상태를 토글합니다. 최대화된 창은 복원하고, 복원된 창은 최대화합니다.
    /// </summary>
    /// <param name="window">대상 윈도우</param>
    /// <param name="windowState">현재 윈도우 상태</param>
    private static void ToggleWindowState(Window window, WindowState windowState)
    {
        window.WindowState = windowState == WindowState.Maximized
            ? WindowState.Normal
            : WindowState.Maximized;
    }

    /// <summary>
    /// 최대화된 창을 복원하고 마우스 클릭 위치를 기준으로 창 위치를 조정합니다.
    /// </summary>
    /// <param name="window">대상 윈도우</param>
    /// <param name="e">마우스 이벤트 파라미터</param>
    private static void RestoreWindowFromMaximized(Window window, MouseButtonEventArgs e)
    {
        var mouseScreenPos = window.PointToScreen(e.GetPosition(window));
        var restoreWidth = window.RestoreBounds.Width;
        var screenWidth = SystemParameters.WorkArea.Width;

        var halfWidth = restoreWidth / 2;
        var targetLeft = Math.Clamp(mouseScreenPos.X - halfWidth, 0, screenWidth - restoreWidth);

        window.WindowState = WindowState.Normal;
        window.Left = targetLeft;
        window.Top = mouseScreenPos.Y - TitleBarHeightOffset;
    }

    #endregion
}
