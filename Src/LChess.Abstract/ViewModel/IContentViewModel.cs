using LChess.Util.Enums;

namespace LChess.Abstract.ViewModel;

/// <summary>
/// LChess 컨텐츠 뷰모델 인터페이스
/// </summary>
public interface IContentViewModel
{
    /// <summary>
    /// Content Type 지정
    /// </summary>
    public LChessContentType ContentType { get; init; }

    /// <summary>
    /// 메신저 구독해제
    /// </summary>
    public void UnRegisterMessengers();
}
