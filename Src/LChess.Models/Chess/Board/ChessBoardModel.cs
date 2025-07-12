using LChess.Models.Chess.Board;

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
    public ChessBoardModel(PieceColorType userColor)
    {
        //보드 관리모델 생성
        _managementModel = new BoardManagementModel(userColor);
    }

    #endregion

    #region :: Properties ::

    /// <summary>
    /// 보드관리모델
    /// </summary>
    private BoardManagementModel _managementModel;

    /// <summary>
    /// 체스 보드 타일 컬렉션
    /// </summary>
    [ObservableProperty]
    private List<List<ChessBoardTileModel>>? _source;

    #endregion

    #region :: Methods ::

    /// <summary>
    /// 타일 세팅
    /// </summary>
    /// <param name="unitCodes"> Stockfish 엔진에서 얻은 기물코드 문자열 리스트 </param>
    public void ParseCodes(List<string>? unitCodes)
    {
        // 기물코드 문자가 비어있거나 null인 경우 초기화하지 않음
        if (unitCodes == null || unitCodes.Count == 0) return;

        if (Source == null)
            CreateBoardDatas(unitCodes);
        else
            UpdateTileModels(unitCodes);
    }

    /// <summary>
    /// 타일 선택
    /// </summary>
    /// <param name="selectedModel"> 선택된 타일 </param>
    /// <param name="notation"> 기물 이동이 발생 한 경우 기보출력 </param>
    public void SelectTile(ChessBoardTileModel selectedModel, out string notation)
    {
        // 초기화
        notation = _managementModel.SelectTileAndGetNotationIfNeeded(selectedModel);
    }

    /// <summary>
    /// 보드 데이터 생성
    /// </summary>
    /// <param name="unitCodes"></param>
    private void CreateBoardDatas(List<string> unitCodes)
    {
        // 1. 체스 보드 타일 컬렉션 초기화
        var result = new List<List<ChessBoardTileModel>>();

        _managementModel.Clear();

        // 2. 시작 타일색상 선택
        var tileColor = ChessTileColorType.Dark;

        // 3. 타일 행 생성 루프
        for (int i = 0; i < 8; i++)
        {
            // 3-1. 각 행의 타일 컬렉션 생성
            var line = new List<ChessBoardTileModel>();

            // 3-2. 사용자 기물 색상에 따라 행 인덱스 조정
            var row = _managementModel.UserPieceColor == PieceColorType.White ? i : 7 - i;

            // 3-3. 타일 열 생성 루프
            for (int j = 1; j <= 8; j++)
            {
                // 3-3-1. 사용자 기물 색상에 따라 열 인덱스 조정
                var column = _managementModel.UserPieceColor == PieceColorType.White ? j : 9 - j;

                // 3-3-2. 체스 보드 타일 모델 생성
                var current = new ChessBoardTileModel(tileColor, row, column - 1, unitCodes?[row][column * 4 - 1] ?? ' ');

                // 3-3-3. 모델 생성 후 삽입
                line.Add(current);

                // 3-3-4. 타일 위치를 매퍼에 추가
                _managementModel.AddTile(current);

                // 3-3-5. 필요 시 타일 색상 변경
                if (j != 8)
                {
                    tileColor = tileColor.ChangeColor();
                }
            }

            // 3-4. 결과 저장
            result.Add(line);
        }

        // 4. 체스 보드 타일 컬렉션에 결과 저장
        Source = result;
    }

    private void UpdateTileModels(List<string> unitCodes)
    {
        // 1. 행 루프
        for (int i = 0; i < 8; i++)
        {
            // 1-1. 행 인덱스 조정
            var row = _managementModel.UserPieceColor == PieceColorType.White ? i : 7 - i;

            // 1-2. 열 루프
            for (int j = 1; j <= 8; j++)
            {
                // 1-2-1. 열 인덱스 조절
                var column = _managementModel.UserPieceColor == PieceColorType.White ? j : 9 - j;

                // 1-2-2. 현재 위치 계산
                var currentPosition = ChessPositionModel.CalcPositionCode(row, column - 1);

                // 1-2-3. 파싱 데이터 적용
                _managementModel.UpdateTileUnit(currentPosition, unitCodes?[row][column * 4 - 1] ?? ' ');
            }
        }
    }

    #endregion
}
