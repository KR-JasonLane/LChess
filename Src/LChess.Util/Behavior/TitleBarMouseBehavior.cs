namespace LChess.Util.Behavior;

/// <summary>
/// 타이틀바 마우스 동작을 처리하는 Behavior 클래스
/// </summary>
public class TitleBarMouseBehavior : Behavior<FrameworkElement>
{
	#region :: Attaching / Detadhing ::

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

	#region ::Properties::

	/// <summary>
	/// 현재 애플리케이션의 메인 윈도우를 참조합니다.
	/// </summary>
	private readonly Window _mainWindow = Application.Current.MainWindow;

	#endregion

	#region :: Methods ::

	/// <summary>
	/// 마우스 왼쪽 버튼 클릭 이벤트 핸들러
	/// </summary>
	/// <param name="sender"> 타이틀바 FrameworkElement </param>
	/// <param name="e"> 마우스 이벤트 파라미터 </param>
	private void AssociatedObjectLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		//현재 윈도우 상태 기억
		var windowState = _mainWindow.WindowState;

		// 마우스 클릭이 더블 클릭 시 창상태 변경
		if (e.ClickCount == 2)
		{
			ToggleWindowState(windowState);
			return;
		}

		//최대화 일 때 마우스 위치에 해당하는 창 위치 계산 후 적용
		if (windowState == WindowState.Maximized)
		{
			RestoreWindowFromMaximized(e);
		}

		//최종 적으로 창을 드래그 이동 가능하게 함
		_mainWindow.DragMove();
	}

	/// <summary>
	/// 창 상태를 토글합니다. 최대화된 창은 복원하고, 복원된 창은 최대화합니다.
	/// </summary>
	/// <param name="windowState"> 현재 윈도우 상태 </param>
	private void ToggleWindowState(WindowState windowState)
	{
		_mainWindow.WindowState = windowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
	}

	/// <summary>
	/// 최대화된 창을 복원하고 마우스 클릭 위치를 기준으로 창 위치를 조정합니다.
	/// </summary>
	/// <param name="e"> 마우스 이벤트 파라미터 </param>
	private void RestoreWindowFromMaximized(MouseButtonEventArgs e)
	{
		// 현재 마우스 위치 (스크린 기준)
		var mouseScreenPos = _mainWindow.PointToScreen(e.GetPosition(_mainWindow));

		// 마우스 위치 좌표
		var mouseX = mouseScreenPos.X;
		var mouseY = mouseScreenPos.Y;

		// 최대화 이전의 창 크기
		var restoreBounds    = _mainWindow.RestoreBounds;
		var restoreWidth  = restoreBounds.Width;
		var restoreHeight = restoreBounds.Height;

		// 작업 영역 너비
		var screenWidth = SystemParameters.WorkArea.Width;

		// 좌측 위치 계산
		var halfWidth  = restoreWidth / 2;
		var targetLeft = Math.Clamp(mouseX - halfWidth, 0, screenWidth - restoreWidth);

		// 창 상태 복원
		_mainWindow.WindowState = WindowState.Normal;

		// 창 위치 조정 (Y는 타이틀바 클릭 위치 기준 보정)
		_mainWindow.Left = targetLeft;
		_mainWindow.Top  = mouseY - 20; // 타이틀바 높이 감안 
	}

	#endregion

}
