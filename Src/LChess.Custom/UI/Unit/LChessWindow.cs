namespace LChess.Custom.UI.Unit;

public class LChessWindow : Window
{
	static LChessWindow()
	{
		DefaultStyleKeyProperty.OverrideMetadata(typeof(LChessWindow), new FrameworkPropertyMetadata(typeof(LChessWindow)));
	}
}
