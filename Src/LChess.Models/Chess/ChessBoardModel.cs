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
        // 유저색상 기억
        _userPieceColor = userPieceColor;

        //매퍼 생성
        _tilePositionMapper = new Dictionary<ChessPosition, ChessBoardTileModel>();
    }

    #endregion

    #region :: Properties ::

    /// <summary>
    /// 유저 기물색상
    /// </summary>
    private readonly PieceColorType _userPieceColor;

    /// <summary>
    /// 포지션과 타일 모델 매핑 딕셔너리
    /// </summary>
    private Dictionary<ChessPosition, ChessBoardTileModel> _tilePositionMapper;

    /// <summary>
    /// 선택된 모델 기억
    /// </summary>
    private ChessBoardTileModel? _oldSelectedTileModel;

    /// <summary>
    /// 체스 보드 타일 컬렉션
    /// </summary>
    [ObservableProperty]
    private List<List<ChessBoardTileModel>>? _tiles;

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

        // 1. 체스 보드 타일 컬렉션 초기화
        var result = new List<List<ChessBoardTileModel>>();

        _tilePositionMapper.Clear();

        // 2. 시작 타일색상 선택
        var tileColor = ChessTileColorType.Dark;

        // 3. 타일 행 생성 루프
        for (int i = 0; i < 8; i++)
        {
            // 3-1. 각 행의 타일 컬렉션 생성
            var line = new List<ChessBoardTileModel>();

            // 3-2. 사용자 기물 색상에 따라 행 인덱스 조정
            var row = _userPieceColor == PieceColorType.White ? i : 7 - i;

            // 3-3. 타일 열 생성 루프
            for (int j = 1; j <= 8; j++)
            {
                // 3-3-1. 사용자 기물 색상에 따라 열 인덱스 조정
                var column = _userPieceColor == PieceColorType.White ? j : 9 - j;

                // 3-3-2. 체스 보드 타일 모델 생성
                var current = new ChessBoardTileModel(tileColor, row, column - 1, unitCodes?[row][column * 4 - 1] ?? ' ');

                // 3-3-3. 모델 생성 후 삽입
                line.Add(current);

                // 3-3-4. 타일 위치를 매퍼에 추가
                _tilePositionMapper.Add(current.Position.Code, current);

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
        Tiles = result;
    }

    /// <summary>
    /// 타일 선택
    /// </summary>
    /// <param name="selectedModel"> 선택된 타일 </param>
    /// <param name="notation"> 기물 이동이 발생 한 경우 기보출력 </param>
    public void SelectTile(ChessBoardTileModel selectedModel, out string notation)
    {
        // 초기화
        notation = string.Empty;

        // 1. 보드 데이터가 없거나, 이미 선택한 모델이면 리턴
        if (Tiles == null || _oldSelectedTileModel == selectedModel) return;

        // 2. 이전에 선택된 타일에 기물이 있고, 현재 선택된 모델이 이동 가능한 위치 또는 공격 가능한 적일 경우
        if (_oldSelectedTileModel != null && !_oldSelectedTileModel.IsEmpty && selectedModel.IsMovableTarget)
        {
            //2-1. 기물이동 기보 문자열 저장
            notation = $"{_oldSelectedTileModel.Position.Code}{selectedModel.Position.Code}";
        }

        // 3. 현재 선택한 타일에 사용자 기물이 있는 경우
        if (!selectedModel.IsEmpty && selectedModel.IsSameColor(_userPieceColor))
        {
            // 3-1. 하이라이트 제거
            ClearAllHighLights();

            // 3-2. 선택된 타일에 해당하는 기물에 맞게 이동가능 경로 하이라이트
            HighLightMovablePositions(selectedModel);
        }
    }

    /// <summary>
    /// 선택된 타일에 있는 기물이 이동 가능한 위치를 하이라이트
    /// </summary>
    /// <param name="selectedModel"> 선택된 모델 </param>
    private void HighLightMovablePositions(ChessBoardTileModel selectedModel)
    {
        switch (selectedModel.UnitType)
        {
            case ChessUnitType.Rook:
                HighLightRookLoad(selectedModel);
                break;
            case ChessUnitType.Pawn:
                HighLightPawnLoad(selectedModel);
                break;
            case ChessUnitType.Bishop:
                HighLightBishopLoad(selectedModel);
                break;
            case ChessUnitType.Queen:
                HighLightQueenLoad(selectedModel);
                break;
            case ChessUnitType.Knight:
                HighLightKnightLoad(selectedModel);
                break;
            default:
                // 다른 기물 타입은 선택하지 않음
                break;
        }

        //선택유닛이 비어있지 않으면
        if(selectedModel.UnitType != ChessUnitType.Empty)
        {
            // 선택된 타일 모델에 하이라이트 표시
            selectedModel.IsSelected = true;
        }

        // 선택된 타일 모델을 기억
        _oldSelectedTileModel = selectedModel;
    }

    /// <summary>
    /// 현재 모든 칸의 하이라이트를 제거
    /// </summary>
    public void ClearAllHighLights()
    {
        //보드 데이터가 없으면 수행하지 않음
        if (Tiles == null) return;

        //모든 하이라이트 상태 끄기
        foreach (var line in Tiles)
        {
            foreach(var unit in line)
            {
                unit.TurnOffHighLight();
            }
        }

        //기억중인 타일 제거
        _oldSelectedTileModel = null;
    }

    /// <summary>
    /// 룩 경로 하이라이트
    /// </summary>
    /// <param name="rookModel"> 룩 모델 </param>
    private void HighLightRookLoad(ChessBoardTileModel rookModel)
    {
        var position = rookModel.Position;

        HighlightValidPositions(position.GetTopLinePositions   ());
        HighlightValidPositions(position.GetLeftLinePositions  ());
        HighlightValidPositions(position.GetBottomLinePositions());
        HighlightValidPositions(position.GetRightLinePositions ());
    }

    /// <summary>
    /// 폰 경로 하이라이트
    /// </summary>
    /// <param name="pawnModel"> 폰 모델 </param>
    private void HighLightPawnLoad(ChessBoardTileModel pawnModel)
    {
        var position = pawnModel.Position;  

        List<ChessPosition> streight     ;
        List<ChessPosition> leftDiagonal ;
        List<ChessPosition> rightDiagonal;

        if (_userPieceColor == PieceColorType.White)
        {
            int moveCount = pawnModel.Position.Code.ToString().Contains('2') ? 2 : 1;

            streight      = position.GetTopLinePositions         (moveCount);
            leftDiagonal  = position.GetLeftTopDiagonalPositions (1        );
            rightDiagonal = position.GetRightTopDiagonalPositions(1        );
        }
        else
        {
            int moveCount = pawnModel.Position.Code.ToString().Contains('7') ? 2 : 1;
            
            streight      = position.GetBottomLinePositions         (moveCount);
            leftDiagonal  = position.GetLeftBottomDiagonalPositions (1        );
            rightDiagonal = position.GetRightBottomDiagonalPositions(1        );
        }

        HighlightValidPositions(streight);

        // 폰은 대각선으로 이동할 때만 적군을 공격할 수 있으므로 적 기물 존재 여부에 따라 경로를 표시해준다.
        if(leftDiagonal.Count > 0 && _tilePositionMapper.TryGetValue(leftDiagonal.First(), out var leftTile) && pawnModel.IsEnemyTile(leftTile))
        {
            HighlightValidPositions(leftDiagonal);
        }
        if (rightDiagonal.Count > 0 && _tilePositionMapper.TryGetValue(rightDiagonal.First(), out var rightTile) && pawnModel.IsEnemyTile(rightTile))
        {
            HighlightValidPositions(rightDiagonal);
        }
    }

    /// <summary>
    /// 비숍 경로 하이라이트
    /// </summary>
    /// <param name="bishopModel"> 비숍모델 </param>
    private void HighLightBishopLoad(ChessBoardTileModel bishopModel)
    {
        var position = bishopModel.Position;

        HighlightValidPositions(position.GetLeftTopDiagonalPositions    ());
        HighlightValidPositions(position.GetLeftBottomDiagonalPositions ());
        HighlightValidPositions(position.GetRightBottomDiagonalPositions());
        HighlightValidPositions(position.GetRightTopDiagonalPositions   ());
    }

    /// <summary>
    /// 비숍 경로 하이라이트
    /// </summary>
    /// <param name="bishopModel"> 비숍모델 </param>
    private void HighLightQueenLoad(ChessBoardTileModel queenModel)
    {
        var position = queenModel.Position;

        HighlightValidPositions(position.GetTopLinePositions            ());
        HighlightValidPositions(position.GetLeftTopDiagonalPositions    ());
        HighlightValidPositions(position.GetLeftLinePositions           ());
        HighlightValidPositions(position.GetLeftBottomDiagonalPositions ());
        HighlightValidPositions(position.GetBottomLinePositions         ());
        HighlightValidPositions(position.GetRightBottomDiagonalPositions());
        HighlightValidPositions(position.GetRightLinePositions          ());
        HighlightValidPositions(position.GetRightTopDiagonalPositions   ());
    }

    private void HighLightKnightLoad(ChessBoardTileModel knightModel)
    {
        var position = knightModel.Position;

        HighlightValidPositions(position.GetKnightPositions(), true);
    }

    /// <summary>
    /// 유효한 위치를 하이라이트
    /// </summary>
    /// <param name="positions"> 하이라이트 대상 위치 리스트 </param>
    /// <param name="isKnight"> 나이트 인지 여부 </param>
    private void HighlightValidPositions(List<ChessPosition>? positions, bool isKnight = false)
    {
        if (positions == null) return;

        foreach (var position in positions)
        {
            // 1. 유효하지 않거나 매핑이 안되는 경우 다음으로
            if (position == ChessPosition.Invalid || !_tilePositionMapper.TryGetValue(position, out var tile) || tile == null) continue;

            // 2. 타일이 비어있으면 이동경로로 하이라이트
            if(tile.IsEmpty)
            {
                tile.IsHighLightMove = true;

                //계속 수행
                continue;
            }

            // 3. 적군기물을 만나면
            if (tile.IsEnemyColor(_userPieceColor))
            {
                // 적군 기물이 있는 타일 하이라이트
                tile.IsHighLightEnemy = true;
            }

            // 4. 나이트가 아닌 경우
            //    비어있지 않거나, 적군기물이 아니면 아군 기물이므로
            //    더이상 진행하지 않음.
            if (!isKnight) break;
        }
    }

    #endregion
}
