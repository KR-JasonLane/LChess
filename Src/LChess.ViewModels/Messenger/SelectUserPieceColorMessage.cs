using LChess.Util.Enums;

namespace LChess.ViewModels.Messenger;

/// <summary>
/// 유저 기물 색상 선택 메시지
/// </summary>
public class SelectUserPieceColorMessage : ValueChangedMessage<PieceColorType> { public SelectUserPieceColorMessage(PieceColorType type) : base(type) { } }
