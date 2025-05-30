namespace LChess.Custom.UI.Unit;

public class LChessButton : Button
{
    static LChessButton()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(LChessButton), new FrameworkPropertyMetadata(typeof(LChessButton)));
    }
}
