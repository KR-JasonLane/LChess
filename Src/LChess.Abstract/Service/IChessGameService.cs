using System.Text;

namespace LChess.Abstract.Service;

/// <summary>
/// 체스 게임 관리 서비스 인터페이스
/// </summary>
public interface IChessGameService
{
    /// <summary>
    /// 새게임 시작
    /// </summary>
    public Task<List<string>?> NewGame();

    /// <summary>
    /// AI 턴
    /// </summary>
    /// <returns></returns>
    public Task<List<string>?> ExecuteAIMove();

    /// <summary>
    /// 기물이동
    /// </summary>
    /// <param name="notation"> 기물이동 기보 문자열 </param>
    /// <returns> Stockfish 기물 코드 </returns>
    public Task<List<string>?> MovePiece(string notation);
}
