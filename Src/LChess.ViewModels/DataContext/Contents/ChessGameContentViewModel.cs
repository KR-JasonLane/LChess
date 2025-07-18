﻿using LChess.Abstract.ViewModel;

using LChess.Util.Enums;
using LChess.ViewModels.Messenger;

namespace LChess.ViewModels.DataContext.Contents;

/// <summary>
/// 체스게임 Content 뷰모델
/// </summary>
public partial class ChessGameContentViewModel : ObservableRecipient, IContentViewModel
{
	#region :: Constructor ::

	public ChessGameContentViewModel()
    {
        ////////////////////////////////////////
        /// 타입 지정
        ////////////////////////////////////////
        {
            ContentType = LChessContentType.ChessGame;
        }


        ////////////////////////////////////////
        /// 체스보드 Content 뷰모델 생성
        ////////////////////////////////////////
        {
            ChessBoardContent = Ioc.Default.GetService<ChessBoardContentViewModel>();
		}


        ////////////////////////////////////////
        /// 체스보드 Content 뷰모델 생성
        ////////////////////////////////////////
        {
            WeakReferenceMessenger.Default.Register<EndGameMessage>(this, (s, e) =>
            {
                // TODO : 게임 종료 메시지 수신 처리
            });
        }
    }

    #endregion

    #region :: Services ::

    #endregion

    #region :: Properties ::

    /// <summary>
    /// Content Type 지정
    /// </summary>
    public LChessContentType ContentType { get; init; }

	/// <summary>
	/// 체스보드 
	/// </summary>
	[ObservableProperty]
	private IContentViewModel? _chessBoardContent;

	#endregion

	#region :: Methods ::

	#endregion

	#region :: Commands ::

	#endregion
}
