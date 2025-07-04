using LChess.Util.Enums;
using LChess.Util.Extension;

namespace LChess.Models.Chess;

/// <summary>
/// 체스보드 모델
/// </summary>
public partial class ChessBoardModel : ObservableObject
{
    #region :: Constructor ::

    /// <summary>
    /// 생성자
    /// </summary>
    public ChessBoardModel(PieceColorType userPieceColor)
    {
        _userPieceColor = userPieceColor;
    }

    #endregion

    #region :: Properties ::

    /// <summary>
    /// 유저 기물색상
    /// </summary>
    private readonly PieceColorType _userPieceColor;

    /// <summary>
    /// 체스 보드 유닛 컬렉션
    /// </summary>
    [ObservableProperty]
    private List<List<ChessBoardUnitModel>>? _units;

    #endregion

    #region :: Methods ::

    /// <summary>
    /// 유닛 세팅
    /// </summary>
    /// <param name="unitCodes"> Stockfish 엔진에서 얻은 기물코드 문자열 리스트 </param>
    public void SetUnits(List<string>? unitCodes)
    {
        // 유닛 코드가 비어있거나 null인 경우 초기화하지 않음
        if (unitCodes == null || unitCodes.Count == 0)
        {
            return;
        }

        // 1. 체스 보드 유닛 컬렉션 초기화
        var result = new List<List<ChessBoardUnitModel>>();

        // 2. 시작 타일색상 선택
        var tileColor = ChessTileColorType.Dark;

        // 3. 타일 행 생성 루프
        for (int i = 0; i < 8; i++)
        {
            // 3-1. 각 행의 유닛 컬렉션 생성
            var line = new List<ChessBoardUnitModel>();

            // 3-2. 사용자 기물 색상에 따라 행 인덱스 조정
            var row = _userPieceColor == PieceColorType.White ? i : 7 - i;

            // 3-3. 타일 색상 변경
            for (int j = 1; j <= 8; j++)
            {
                // 3-3-1. 사용자 기물 색상에 따라 열 인덱스 조정
                var column = _userPieceColor == PieceColorType.White ? j : 9 - j;

                // 3-3-2. 모델 생성 후 삽입
                line.Add(new ChessBoardUnitModel(tileColor, row, column, unitCodes?[row][column * 4 - 1] ?? ' '));

                // 3-3-3. 필요 시 타일 색상 변경
                if (column != 8)
                {
                    tileColor = tileColor.ChangeColor();
                }
            }

            // 3-4. 결과 저장
            result.Add(line);
        }

        // 4. 체스 보드 유닛 컬렉션에 결과 저장
        Units = result;
    }

    /// <summary>
    /// 유닛 선택
    /// </summary>
    /// <param name="selectedModel"> 사용자에 의해 선택 된 유닛 </param>
    public void SelectUnit(ChessBoardUnitModel selectedModel)
    {
        //TODO: selectedModel의 IsHighLight 속성을 토글하는 로직 추가
    }

    #endregion
}
