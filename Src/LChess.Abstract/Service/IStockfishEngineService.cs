using LChess.Models.Result;

namespace LChess.Abstract.Service;

/// <summary>
/// 스톡피쉬 엔진 서비스 인터페이스
/// </summary>
public interface IStockfishEngineService
{
	/// <summary>
	/// 엔진 시작 (비동기)
	/// </summary>
	/// <returns> 성공여부 </returns>
	public Task<bool> StartEngineAsync();

	/// <summary>
	/// 엔진 종료
	/// </summary>
	/// <returns> 성공여부 </returns>
	public bool StopEngine();

	/// <summary>
	/// 엔진 커맨드 전송 (비동기)
	/// </summary>
	/// <param name="command"> 전송 할 커맨드 </param>
	/// <returns> 엔진 응답 </returns>
	public Task<string?> SendCommandAsync(string command, string output);

	/// <summary>
	/// 현재 보드 상태 반환
	/// </summary>
	/// <returns> StockFish 엔진 응답 </returns>
	public Task<StockfishBoardCodeModel> GetCurrentBoard();
}
