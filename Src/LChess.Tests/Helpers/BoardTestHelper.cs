using LChess.Models.Chess;
using LChess.Models.Chess.Board;
using LChess.Util.Enums;
using LChess.Util.Extension;

namespace LChess.Tests.Helpers;

/// <summary>
/// 테스트용 체스보드를 간편하게 구성하는 헬퍼 클래스
/// </summary>
public static class BoardTestHelper
{
    /// <summary>
    /// 8x8 문자 배열로부터 BoardManagementModel을 생성한다.
    /// 각 문자열은 8자로 구성되며, Stockfish 기물코드를 사용한다.
    /// 대문자 = 백, 소문자 = 흑, 공백 = 빈칸
    /// </summary>
    /// <param name="ranks">rank 8(상단)부터 rank 1(하단) 순서의 8개 문자열</param>
    /// <param name="userColor">유저 기물 색상</param>
    /// <returns>타일이 배치된 BoardManagementModel</returns>
    public static BoardManagementModel CreateBoard(string[] ranks, PieceColorType userColor = PieceColorType.White)
    {
        if (ranks.Length != 8)
            throw new ArgumentException("8개의 rank 문자열이 필요합니다.", nameof(ranks));

        var board = new BoardManagementModel(userColor);
        var tileColor = ChessTileColorType.Light;

        for (int row = 0; row < 8; row++)
        {
            var rank = ranks[row].PadRight(8);

            for (int col = 0; col < 8; col++)
            {
                var unitCode = rank[col];
                var tile = new ChessBoardTileModel(tileColor, row, col, unitCode);
                board.AddTile(tile);

                tileColor = tileColor.ChangeColor();
            }

            tileColor = tileColor.ChangeColor();
        }

        return board;
    }

    /// <summary>
    /// 빈 보드에 특정 위치에만 기물을 배치한다.
    /// </summary>
    /// <param name="placements">위치-기물코드 쌍</param>
    /// <param name="userColor">유저 기물 색상</param>
    /// <returns>기물이 배치된 BoardManagementModel</returns>
    public static BoardManagementModel CreateBoardWithPieces(
        Dictionary<ChessPosition, char> placements,
        PieceColorType userColor = PieceColorType.White)
    {
        var ranks = new string[]
        {
            "        ",
            "        ",
            "        ",
            "        ",
            "        ",
            "        ",
            "        ",
            "        ",
        };

        foreach (var (position, unitCode) in placements)
        {
            int row = (int)position / 10;
            int col = (int)position % 10;

            var chars = ranks[row].ToCharArray();
            chars[col] = unitCode;
            ranks[row] = new string(chars);
        }

        return CreateBoard(ranks, userColor);
    }

    /// <summary>
    /// 특정 포지션의 타일을 조회한다.
    /// </summary>
    public static ChessBoardTileModel? GetTile(BoardManagementModel board, ChessPosition position)
    {
        board.TryGetTile(position, out var tile);
        return tile;
    }

    /// <summary>
    /// 특정 포지션에 기물 업데이트 이력을 기록한다.
    /// 캐슬링 테스트 시 킹/룩이 이동했음을 표시하는 데 사용한다.
    /// </summary>
    public static void MarkTileAsUpdated(BoardManagementModel board, ChessPosition position)
    {
        board.UpdateTileUnit(position, ' ');

        if (board.TryGetTile(position, out var tile) && tile != null)
        {
            var originalUnit = tile.Unit;
            board.UpdateTileUnit(position, originalUnit?.OriginalCode ?? ' ');
        }
    }
}
