﻿using LChess.Util.Enums;

namespace LChess.ViewModels.Messenger;

/// <summary>
/// Content 이동 메시지
/// </summary>
public class MoveContentMessage : ValueChangedMessage<LChessContentType> { public MoveContentMessage(LChessContentType value) : base(value) { } }
