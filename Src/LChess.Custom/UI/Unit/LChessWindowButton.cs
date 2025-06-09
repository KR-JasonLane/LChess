namespace LChess.Custom.UI.Unit;

/// <summary>
/// LChess 커스텀 윈도우 버튼
/// </summary>
public class LChessWindowButton : Button
{
	static LChessWindowButton()
	{
		DefaultStyleKeyProperty.OverrideMetadata(typeof(LChessWindowButton), new FrameworkPropertyMetadata(typeof(LChessWindowButton)));
	}
}
