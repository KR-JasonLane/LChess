namespace LChess.Abstract.Service;

/// <summary>
/// 윈도우 핸들링 서비스 추상화 인터페이스
/// </summary>
public interface IWindowHandlingService
{
	/// <summary>
	/// 쉘 윈도우 최대화
	/// </summary>
	public void MaximizeShellWindow();

	/// <summary>
	/// 쉘 윈도우 크기 복구
	/// </summary>
	public void RestoreShellWindow();

	/// <summary>
	/// 쉘 윈도우 닫기
	/// </summary>
	public void CloseShellWindow();
}
