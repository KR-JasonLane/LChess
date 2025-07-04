using LChess.Util.Enums;

namespace LChess.Models.Chess;

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

    #endregion

    #region :: Methods ::

    /// <summary>
    /// 현재 위치 기준 위쪽방향 포지션들을 반환
    /// </summary>
    /// <returns> 현재 위치로부터 보드 끝까지의 위치 리스트 </returns>
    public List<ChessPosition> GetTopLinePositions(int max = 8)
    {
        var positions = new List<ChessPosition>();
        for (int i = 1; i <= max; i++)
        {
            var topPosition = CalcPositionCode(Row - i, Column);

            if (topPosition == ChessPosition.Invalid) break;

            positions.Add(topPosition);
        }
        return positions;
    }

    /// <summary>
    /// 현재 위치 기준 아래쪽방향 포지션들을 반환
    /// </summary>
    /// <returns> 현재 위치로부터 보드 끝까지의 위치 리스트 </returns>
    public List<ChessPosition> GetBottomLinePositions(int max = 8)
    {
        var positions = new List<ChessPosition>();
        for (int i = 1; i <= max; i++)
        {
            var topPosition = CalcPositionCode(Row + i, Column);

            if (topPosition == ChessPosition.Invalid) break;

            positions.Add(topPosition);
        }
        return positions;
    }

    /// <summary>
    /// 현재 위치 기준 왼쪽방향 포지션들을 반환
    /// </summary>
    /// <returns> 현재 위치로부터 보드 끝까지의 위치 리스트 </returns>
    public List<ChessPosition> GetLeftLinePositions(int max = 8)
    {
        var positions = new List<ChessPosition>();
        for (int i = 1; i <= max; i++)
        {
            var topPosition = CalcPositionCode(Row, Column - i);

            if (topPosition == ChessPosition.Invalid) break;

            positions.Add(topPosition);
        }
        return positions;
    }

    /// <summary>
    /// 현재 위치 기준 오른쪽방향 포지션들을 반환
    /// </summary>
    /// <returns> 현재 위치로부터 보드 끝까지의 위치 리스트 </returns>
    public List<ChessPosition> GetRightLinePositions(int max = 8)
    {
        var positions = new List<ChessPosition>();
        for (int i = 1; i <= max; i++)
        {
            var topPosition = CalcPositionCode(Row, Column + i);

            if (topPosition == ChessPosition.Invalid) break;

            positions.Add(topPosition);
        }
        return positions;
    }

    /// <summary>
    /// 체스보드 위치 변환
    /// </summary>
    /// <param name="row"> 행 </param>
    /// <param name="column"> 열 </param>
    /// <returns> 변환된 위치 </returns>
    private static ChessPosition CalcPositionCode(int row, int column)
    {
        var position = row * 10 + column;
        if (Enum.TryParse(position.ToString(), out ChessPosition result))
        {
            return result;
        }

        return ChessPosition.Invalid;
    }

    #endregion

}
