namespace LChess.Custom.UI.Unit;

/// <summary>
/// LChess 커스텀윈도우
/// </summary>
public class LChessWindow : Window
{
	static LChessWindow()
	{
		DefaultStyleKeyProperty.OverrideMetadata(typeof(LChessWindow), new FrameworkPropertyMetadata(typeof(LChessWindow)));
	}
}
