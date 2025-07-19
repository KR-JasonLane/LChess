namespace LChess.Custom.UI.Unit;

/// <summary>
/// LChess 커스텀 버튼
/// </summary>
public class LChessButton : Button
{
    static LChessButton()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(LChessButton), new FrameworkPropertyMetadata(typeof(LChessButton)));
    }
}
