using LChess.Util.Enums;

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

    #region DP

    /// <summary>
    /// LChess 윈도우 버튼 타입 의존프로퍼티
    /// </summary>
    public static readonly DependencyProperty LChessWindowButtonTypeProperty =
        DependencyProperty.Register(nameof(LChessWindowButtonType), typeof(WindowButtonType), typeof(LChessWindowButton), new FrameworkPropertyMetadata(WindowButtonType.Hide));

    /// <summary>
    /// LChess 윈도우 버튼 타입
    /// </summary>
    public WindowButtonType LChessWindowButtonType
    {
        get => (WindowButtonType)GetValue(LChessWindowButtonTypeProperty);
        set => SetValue(LChessWindowButtonTypeProperty, value);
    }

    #endregion
}
