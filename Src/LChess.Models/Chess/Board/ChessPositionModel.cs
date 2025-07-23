using LChess.Util.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace LChess.Models.Chess.Board;

/// <summary>
/// 체스 보드상의 기물 위치 모델
/// </summary>
public class ChessPositionModel
{
    #region :: Constructor ::

    /// <summary>
    /// 생성자
    /// </summary>
    public ChessPositionModel(int row, int column)
    {
        Column = column;
        Row    = row   ;

        Code = CalcPositionCode(row, column);
    }

    /// <summary>
    /// 생성자 오버로딩
    /// </summary>
    /// <param name="position"></param>
    public ChessPositionModel(ChessPosition position)
    {
        Row    = (int)position / 10;
        Column = (int)position % 10;

        Code = position;
    }

    #endregion

    #region :: Properties ::

    /// <summary>
    /// 행
    /// </summary>
    public readonly int Row;

    /// <summary>
    /// 열
    /// </summary>
    public readonly int Column;

    /// <summary>
    /// 포지션 코드
    /// </summary>
    public ChessPosition Code { get; init; }

    /// <summary>
    /// 보드의 세로 끝 지점인지 여부
    /// </summary>
    public bool IsEndPointColumnInBoard => Code.ToString().Contains("1") || Code.ToString().Contains("8");

    #endregion

    #region :: Methods ::

    /// <summary>
    /// 현재 위치 기준 위쪽방향 포지션들을 반환
    /// </summary>
    /// <param name="max"> 최대 라인 길이 </param>
    /// <returns> 현채 위치기준 좌표코드 리스트 </returns>
    public List<ChessPosition> GetTopLinePositions(int max = 8) => CreatePositionsByOffset(-1, 0, max);

    /// <summary>
    /// 현재 위치 기준 아래쪽방향 포지션들을 반환
    /// </summary>
    /// <param name="max"> 최대 라인 길이 </param>
    /// <returns> 현채 위치기준 좌표코드 리스트 </returns>
    public List<ChessPosition> GetBottomLinePositions(int max = 8) => CreatePositionsByOffset(1, 0, max);

    /// <summary>
    /// 현재 위치 기준 왼쪽방향 포지션들을 반환
    /// </summary>
    /// <param name="max"> 최대 라인 길이 </param>
    /// <returns> 현채 위치기준 좌표코드 리스트 </returns>
    public List<ChessPosition> GetLeftLinePositions(int max = 8) => CreatePositionsByOffset(0, -1, max);

    /// <summary>
    /// 현재 위치 기준 오른쪽방향 포지션들을 반환
    /// </summary>
    /// <param name="max"> 최대 라인 길이 </param>
    /// <returns> 현채 위치기준 좌표코드 리스트 </returns>
    public List<ChessPosition> GetRightLinePositions(int max = 8) => CreatePositionsByOffset(0, 1, max);

    /// <summary>
    /// 현재 위치 기준 좌측상단 대각선 포지션들을 반환
    /// </summary>
    /// <param name="max"> 최대 라인 길이 </param>
    /// <returns> 현채 위치기준 좌표코드 리스트 </returns>
    public List<ChessPosition> GetLeftTopDiagonalPositions(int max = 8) => CreatePositionsByOffset(-1, -1, max);

    /// <summary>
    /// 현재 위치 기준 좌측하단 대각선 포지션들을 반환
    /// </summary>
    /// <param name="max"> 최대 라인 길이 </param>
    /// <returns> 현채 위치기준 좌표코드 리스트 </returns>
    public List<ChessPosition> GetLeftBottomDiagonalPositions(int max = 8) => CreatePositionsByOffset(1, -1, max);

    /// <summary>
    /// 현재 위치 기준 우측하단 대각선 포지션들을 반환
    /// </summary>
    /// <param name="max"> 최대 라인 길이 </param>
    /// <returns> 현채 위치기준 좌표코드 리스트 </returns>
    public List<ChessPosition> GetRightBottomDiagonalPositions(int max = 8) => CreatePositionsByOffset(1, 1, max);

    /// <summary>
    /// 현재 위치 기준 우측상단 대각선 포지션들을 반환
    /// </summary>
    /// <param name="max"> 최대 라인 길이 </param>
    /// <returns> 현채 위치기준 좌표코드 리스트 </returns>
    public List<ChessPosition> GetRightTopDiagonalPositions(int max = 8) => CreatePositionsByOffset(-1, 1, max);

    /// <summary>
    /// 현재 위치 기준으로 주어진 방향으로 최대 길이만큼의 포지션 코드를 생성
    /// </summary>
    /// <param name="rowCorrection"> 행 보정값 </param>
    /// <param name="columnCorrection"> 열 보정값 </param>
    /// <param name="max"> 최대 라인 길이 </param>
    /// <returns> 보정값으로 계산한 좌표 코드 리스트 </returns>
    public List<ChessPosition> CreatePositionsByOffset(int rowCorrection, int columnCorrection, int max)
    {
        var positions = new List<ChessPosition>();
        for (int i = 1; i <= max; i++)
        {
            var position = CalcPositionCode(Row + rowCorrection * i, Column + columnCorrection * i);
            if (position == ChessPosition.Invalid) break;
            positions.Add(position);
        }
        return positions;
    }

    /// <summary>
    /// 체스보드 위치 변환
    /// </summary>
    /// <param name="row"> 행 </param>
    /// <param name="column"> 열 </param>
    /// <returns> 변환된 위치 </returns>
    public static ChessPosition CalcPositionCode(int row, int column)
    {
        var position = row * 10 + column;
        if (Enum.TryParse(typeof(ChessPosition), position.ToString(), out var parse) && Enum.IsDefined(typeof(ChessPosition), parse) && parse is ChessPosition result)
        {
            return result;
        }

        return ChessPosition.Invalid;
    }

    #endregion

}
