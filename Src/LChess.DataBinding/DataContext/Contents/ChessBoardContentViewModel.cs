using LChess.Abstract.Service;
using LChess.Abstract.ViewModel;

using LChess.Models.Chess;

using LChess.Util.Enums;
using LChess.ViewModels.Messenger;

namespace LChess.ViewModels.DataContext.Contents;

/// <summary>
/// 체스보드 Content 뷰모델
/// </summary>
public partial class ChessBoardContentViewModel : ObservableRecipient, IContentViewModel
{
	#region :: Constructor ::

	/// <summary>
	/// 생성자
	/// </summary>
	public ChessBoardContentViewModel(IStockfishEngineService stockfishEngineService)
    {
        ////////////////////////////////////////
        /// 서비스 등록
        ////////////////////////////////////////
        {
            _stockfishEngineService = stockfishEngineService;
        }


        ////////////////////////////////////////
        /// 타입 지정
        ////////////////////////////////////////
        {
            ContentType = LChessContentType.ChessBoard;
        }


        ////////////////////////////////////////
        /// 메시지 등록
        ////////////////////////////////////////
        {
            WeakReferenceMessenger.Default.Register<SelectUserPieceColorMessage>(this, (s, m) =>
            {
                ChessBoardSource = new ChessBoardModel(m.Value);

                InitBoardSource();
            });
        }
	}

    #endregion

    #region :: Services ::

    /// <summary>
    /// Stockfish 엔전 관리 서비스
    /// </summary>
    private readonly IStockfishEngineService _stockfishEngineService;

    #endregion

    #region :: Properties ::

    /// <summary>
    /// Content Type 지정
    /// </summary>
    public LChessContentType ContentType { get; init; }

	/// <summary>
	/// 체스보드 데이터 소스
	/// </summary>
	[ObservableProperty]
	private ChessBoardModel? _chessBoardSource;

    #endregion

    #region :: Methods ::

    private async void InitBoardSource()
    {
        var stockfishUnitCodes = await _stockfishEngineService.GetCurrentBoard();

        ChessBoardSource?.SetUnits(stockfishUnitCodes);
    }

	#endregion

	#region :: Commands ::

	/// <summary>
	/// 타일선택
	/// </summary>
	/// <param name="param"> 선택된 타일 모델 </param>
	[RelayCommand]
	private void SelectTile(object param)
	{
		if(param is ChessBoardUnitModel model && ChessBoardSource != null)
		{
            ChessBoardSource.SelectUnit(model);
        }
	}

	#endregion
}
