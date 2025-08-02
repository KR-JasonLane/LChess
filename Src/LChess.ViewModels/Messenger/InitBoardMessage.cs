using LChess.Models.Chess.Board;
using LChess.Util.Enums;

namespace LChess.ViewModels.Messenger;

/// <summary>
/// 유저 기물 색상 선택 메시지
/// </summary>
public class InitBoardMessage : ValueChangedMessage<ChessBoardInitPropertyModel> { public InitBoardMessage(ChessBoardInitPropertyModel property) : base(property) { } }
