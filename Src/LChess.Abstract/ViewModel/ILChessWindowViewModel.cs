namespace LChess.Abstract.ViewModel;

/// <summary>
/// LChess 윈도우 뷰모델 인터페이스
/// </summary>
public interface ILChessWindowViewModel
{
    /// <summary>
    /// 현재 Content 
    /// </summary>
    public IContentViewModel CurrentContent { get; set; }
}
