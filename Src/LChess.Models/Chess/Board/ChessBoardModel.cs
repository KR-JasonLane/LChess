using LChess.Models.Chess.Board;
using LChess.Models.Result;
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
        _managementModel = new(userColor);
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
    
    /// <summary>
    /// 유저 기물색상 필드
    /// </summary>
    public PieceColorType UserColor => _managementModel.UserPieceColor;
    
    /// <summary>
    /// 적 기물 색상 필드
    /// </summary>
    public PieceColorType EnemyColor => UserColor == PieceColorType.White ? PieceColorType.Black : PieceColorType.White;
    
    #endregion
    
    #region :: Methods ::
    
    /// <summary>
    /// 타일 세팅
    /// </summary>
    /// <param name="unitCodes"> Stockfish 엔진에서 얻은 기물코드 문자열 리스트 </param>
    public void ParseCodes(StockfishBoardCodeModel? resultModel)
    {
        // 결과모델이 null인 경우 초기화하지 않음
        if (resultModel == null) return;
        
        if (Source == null)
        CreateBoardDatas(resultModel.TileCodeList);
        else
        UpdateTileModels(resultModel);
    }
    
    /// <summary>
    /// 타일 선택
    /// </summary>
    /// <param name="selectedModel"> 선택된 타일 </param>
    /// <returns> 기물 이동이 발생 한 경우 기보출력 </returns>
    public TileSelectedResultModel SelectTile(ChessBoardTileModel selectedModel)
    {
        return _managementModel.SelectTileAndGetNotationIfNeeded(selectedModel);
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
    
    /// <summary>
    /// 타일 모델상태를 업데이트
    /// </summary>
    /// <param name="resultModel"></param>
    private void UpdateTileModels(StockfishBoardCodeModel resultModel)
    {
        var unitCodes = resultModel.TileCodeList;
        
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
                
                // 1-2-3. 파싱 데이터 적용 후 기물 변경이 일어났으면 히스토리 저장
                _managementModel.UpdateTileUnit(currentPosition, unitCodes?[row][column * 4 - 1] ?? ' ');
            }
        }
        
        //체크상태면
        if (resultModel.IsCheck)
        {
            //킹 위협 상태 하이라이트
            _managementModel.KingInCheck(resultModel.CheckerList);
            
            return;
        }
        
        //킹 하이라이트가 있다면 삭제해줌.
        _managementModel.ClearKingHighLightsIfNeeded();
    }
    
    /// <summary>
    /// 모든 하이라이트 정리
    /// </summary>
    public void GameEnd() => _managementModel.ClearAllHighLights();
    
    #endregion
}
