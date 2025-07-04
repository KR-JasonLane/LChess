using LChess.Abstract.Service;
using LChess.Abstract.ViewModel;

using LChess.Util.Enums;

using LChess.ViewModels.Messenger;

namespace LChess.ViewModels.DataContext.Contents;

/// <summary>
/// 기물색상 선택 Content 뷰모델
/// </summary>
public partial class ChoicePieceColorContentViewModel : ObservableRecipient, IContentViewModel
{
    #region :: Constructor ::

    /// <summary>
    /// 생성자
    /// </summary>
    public ChoicePieceColorContentViewModel(IStockfishEngineService stockfishEngineService, IChessGameService chessGameService)
    {
        ////////////////////////////////////////
        /// 서비스 등록
        ////////////////////////////////////////
        {
            _stockfishEngineService = stockfishEngineService;
            _chessGameService       = chessGameService      ;
        }
        
        ////////////////////////////////////////
        /// 타입 지정
        ////////////////////////////////////////
        {
            ContentType = LChessContentType.ChoicePieceColor;
        }
    }

    #endregion

    #region :: Services ::

    /// <summary>
    /// Stockfish 엔진 관리 서비스
    /// </summary>
    private readonly IStockfishEngineService _stockfishEngineService;

    /// <summary>
    /// 체스게임 관리 서비스
    /// </summary>
    private readonly IChessGameService _chessGameService;

    #endregion

    #region :: Properties ::

    /// <summary>
    /// Content Type 지정
    /// </summary>
    public LChessContentType ContentType { get; init; }

    #endregion

    #region :: Methods ::

    private async Task MoveToChessGame(PieceColorType color)
    {
        //1. 딤 켜기
        WeakReferenceMessenger.Default.Send(new WindowDimmingMessage(true));

        //2. 비동기 엔진 시작
        await Task.Run(_stockfishEngineService.StartEngineAsync);

        //3. 체스 게임 컨텐츠로 이동
        WeakReferenceMessenger.Default.Send(new MoveContentMessage(LChessContentType.ChessGame));

        //4. 사용자 색상 전달
        WeakReferenceMessenger.Default.Send(new SelectUserPieceColorMessage(color));

        //5. 딤 끄기
        WeakReferenceMessenger.Default.Send(new WindowDimmingMessage(false));
    }

    #endregion

    #region :: Commands ::

    /// <summary>
    /// 기물 색상 선택
    /// </summary>
    [RelayCommand]
    private async Task SelectColor(PieceColorType color)
    {
        await MoveToChessGame(color);
    }

    #endregion
}
