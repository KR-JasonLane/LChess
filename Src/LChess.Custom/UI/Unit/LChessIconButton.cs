using LChess.Util.Enums;

namespace LChess.Custom.UI.Unit;

/// <summary>
/// LChess 아이콘 버튼 컨트롤
/// </summary>
public class LChessIconButton : Button
{
    /// <summary>
    /// 생성자
    /// </summary>
    static LChessIconButton()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(LChessIconButton), new FrameworkPropertyMetadata(typeof(LChessIconButton)));
    }


    #region :: DP ::

    /// <summary>
    /// 아이콘 타입 의존프로퍼티
    /// </summary>
    public static readonly DependencyProperty IconTypeProperty =
        DependencyProperty.Register(nameof(IconType), typeof(LChessIconType), typeof(LChessIconButton), new FrameworkPropertyMetadata(LChessIconType.None));

    /// <summary>
    /// 아이콘 타입
    /// </summary>
    public LChessIconType IconType
    {
        get => (LChessIconType)GetValue(IconTypeProperty);
        set => SetValue(IconTypeProperty, value);
    }

    #endregion
}
