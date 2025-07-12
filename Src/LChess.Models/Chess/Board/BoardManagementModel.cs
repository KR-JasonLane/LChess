using LChess.Util.Enums;

namespace LChess.Models.Chess.Board;

/// <summary>
/// 체스보드관리 모델
/// </summary>
public class BoardManagementModel
{
    #region :: Constructor ::

    public BoardManagementModel(PieceColorType userColor)
    {
        //매퍼 생성
        _tileMapper = new Dictionary<ChessPosition, ChessBoardTileModel>();

        //유저 기물색상 설정
        UserPieceColor = userColor;
    }

    #endregion

    #region :: Properties ::

    /// <summary>
    /// 포지션과 타일 모델 매핑 딕셔너리
    /// </summary>
    private readonly Dictionary<ChessPosition, ChessBoardTileModel> _tileMapper;

    /// <summary>
    /// 유저 기물 색상
    /// </summary>
    public PieceColorType UserPieceColor { get; init; }

    /// <summary>
    /// 선택된 모델 기억
    /// </summary>
    private ChessBoardTileModel? _selectedTileModel;

    #endregion

    #region :: Methods ::

    /// <summary>
    /// 타일 정보 업데이트
    /// </summary>
    public void UpdateTileUnit(ChessPosition position, char unitCode) 
    {
        if (_tileMapper.TryGetValue(position, out var tile)) tile.UpdateUnit(unitCode);
    }

    /// <summary>
    /// 타일 맵핑정보 초기화
    /// </summary>
    public void Clear() => _tileMapper.Clear();

    /// <summary>
    /// 타일 맵핑정보 추가
    /// </summary>
    /// <param name="tileModel"> 추가할 타일 </param>
    public void AddTile(ChessBoardTileModel tileModel) => _tileMapper.Add(tileModel.Position.Code, tileModel);

    /// <summary>
    /// 타일 선택 및 필요 시 기물 이동 기보 반환
    /// </summary>
    /// <param name="selectedTile"> 선택된 타일 </param>
    /// <returns> 기물 이동기보 (이동이 없을 시 빈 문자열) </returns>
    public string SelectTileAndGetNotationIfNeeded(ChessBoardTileModel selectedTile)
    {
        var result = string.Empty;

        // 1. 이미 선택한 모델이면 수행하지 않음.
        if (_selectedTileModel == selectedTile) return result;

        // 2. 이전에 선택된 타일에 기물이 있고, 현재 선택된 모델이 이동 가능한 상태이면
        if (_selectedTileModel != null && selectedTile.IsMovableTarget)
        {
            //2-1. 기물이동 기보 문자열 저장
            result = $"{_selectedTileModel.Position.Code}{selectedTile.Position.Code}";

            //선택 해제시켜줌.
            _selectedTileModel = null;
        }

        // 3. 현재 선택한 타일에 사용자 기물이 있는 경우
        else if (!selectedTile.IsEmpty && (selectedTile.Unit?.IsSameColor(UserPieceColor) ?? false))
        {
            // 3-1. 하이라이트 제거
            ClearAllHighLights();

            // 3-2. 선택된 타일에 해당하는 기물에 맞게 이동가능 경로 하이라이트
            selectedTile.Unit?.RouteModel?.TurnOnUnitRoute(_tileMapper);

            // 3-3. 선택된 타일 모델 기억
            _selectedTileModel = selectedTile;
        }

        return result;
    }


    /// <summary>
    /// 현재 모든 칸의 하이라이트를 제거
    /// </summary>
    public void ClearAllHighLights()
    {
        //모든 하이라이트 상태 끄기
        foreach (var unit in _tileMapper.Values) 
            unit.TurnOffHighLight();

        //기억중인 타일 제거
        _selectedTileModel = null;
    }

    #endregion
}
