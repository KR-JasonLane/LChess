using LChess.Util.Enums;
using LChess.Models.Chess.Unit.Base;

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
            // 이동 가능 여부 한번 더 확인
            if (IsMoveLegal(_selectedTileModel, selectedTile))
            {
                //2-1. 기물이동 기보 문자열 저장
                result = $"{_selectedTileModel.Position.Code}{selectedTile.Position.Code}";
            }

            //선택 해제시켜줌.
            _selectedTileModel = null;
        }

        // 3. 현재 선택한 타일에 사용자 기물이 있는 경우
        else if (!selectedTile.IsEmpty && selectedTile.Unit?.IsSameColor(UserPieceColor) == true)
        {
            // 3-1. 하이라이트 제거
            ClearAllHighLights();

            // 3-2. 선택된 타일에 해당하는 기물의 이동 가능 경로 하이라이트 (킹 안전 고려)
            HighlightLegalMoves(selectedTile);

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
        foreach (var tile in _tileMapper.Values) 
            tile.TurnOffHighLight();

        //기억중인 타일 제거
        _selectedTileModel = null;
    }

    public void ClearAllDangerHighLights()
    {
        //모든 하이라이트 상태 끄기
        foreach (var tile in _tileMapper.Values)
            tile.IsHighLightDanger = false;
    }

    //현재 체크상태인지 확인
    public bool IsCheckStatus(PieceColorType color)
    {
        return _tileMapper.Values.Any(x => x.IsHighLightDanger && x?.Unit?.ColorType == color);
    }

    /// <summary>
    /// 위치에 맞는 타일 찾기
    /// </summary>
    /// <param name="position"> 키 </param>
    /// <param name="tileModel"> 값 </param>
    /// <returns> 성공여부 </returns>
    public bool TryGetTile(ChessPosition position, out ChessBoardTileModel? tileModel) => _tileMapper.TryGetValue(position, out tileModel);

    /// <summary>
    /// 현재 색상의 킹 위치 타일을 반환
    /// </summary>
    /// <param name="color">킹 색상</param>
    /// <returns>킹이 위치한 타일</returns>
    private ChessBoardTileModel? GetKingTile(PieceColorType color)
    {
        return _tileMapper.Values.FirstOrDefault(t => !t.IsEmpty && t.Unit?.UnitType == ChessUnitType.King && t.Unit.ColorType == color);
    }

    /// <summary>
    /// 주어진 이동이 체크를 유발하는지 여부를 계산
    /// </summary>
    private bool IsMoveLegal(ChessBoardTileModel fromTile, ChessBoardTileModel toTile)
    {
        var movingUnit = fromTile.Unit;
        if (movingUnit == null) return false;

        //백업
        var originTargetUnit = toTile.Unit;

        //이동 기물 생성 (위치 갱신)
        var movedUnit = ChessUnitModelBase.CreateUnitModel(movingUnit.UnitType, movingUnit.ColorType, toTile.Position.Code);
        if (movedUnit == null) return false;

        //가상 이동
        fromTile.Unit = null;
        toTile.Unit = movedUnit;

        //킹 위치 찾기
        var kingTile = GetKingTile(movingUnit.ColorType);
        var kingPos = kingTile?.Position.Code ?? ChessPosition.Invalid;

        //적이 이동 가능한 모든 경로 계산
        var enemyRoute = GetEnemyRoute();

        //체크 여부 판정
        var inCheck = enemyRoute.Contains(kingPos);

        //원상복구
        toTile.Unit = originTargetUnit;
        fromTile.Unit = movingUnit;

        return !inCheck;
    }

    /// <summary>
    /// 선택된 기물의 이동 가능 경로를 하이라이트 (킹 안전 고려)
    /// </summary>
    /// <param name="selectedTile">선택된 타일</param>
    private void HighlightLegalMoves(ChessBoardTileModel selectedTile)
    {
        var unit = selectedTile.Unit;
        if (unit == null) return;

        var candidate = unit.RouteModel?.GetMovablePositions(this) ?? new List<ChessPosition>();

        foreach (var pos in candidate)
        {
            if (!TryGetTile(pos, out var target) || target == null) continue;

            if (!IsMoveLegal(selectedTile, target)) continue;

            if (target.IsEmpty)
                target.IsHighLightMove = true;
            else if (unit.IsEnemy(target.Unit))
                target.IsHighLightEnemy = true;
        }
    }

    /// <summary>
    /// 적이 이동가능한 모든 경로 
    /// </summary>
    /// <returns> 적이 이동 가능한 경로 </returns>
    public List<ChessPosition> GetEnemyRoute()
    {
        var allEnemyRoute = new List<ChessPosition>();

        foreach (var tile in _tileMapper.Values)
        {
            if (!tile.IsEmpty && tile.Unit?.IsSameColor(UserPieceColor) == false)
            {
                var currentMovableRoute = tile.Unit?.RouteModel?.GetMovablePositions(this);

                allEnemyRoute.AddRange(currentMovableRoute ?? new List<ChessPosition>());
            }
        }

        return allEnemyRoute;
    }

    /// <summary>
    /// 체크 상태일 때, 킹이 위험한 상태임을 표시
    /// </summary>
    /// <param name="checkerPositionCode"> 킹을 위협중인 기물의 위치 </param>
    public void KingInCheck(string checkerPositionCode)
    {
        //킹을 위협중인 기물 찾기
        var checker = _tileMapper.Values
            .FirstOrDefault(tile => tile.Position.Code.ToString().ToUpper() == checkerPositionCode.ToUpper())?.Unit;

        //킹 타일 찾기
        var kingTile = _tileMapper.Values
            .FirstOrDefault(tile => tile.Unit?.UnitType == ChessUnitType.King && tile.Unit?.IsSameColor(checker?.ColorType) == false);

        //킹 위험 하이라이트 
        if (kingTile != null)
            kingTile.IsHighLightDanger = true;
    }

    #endregion
}
