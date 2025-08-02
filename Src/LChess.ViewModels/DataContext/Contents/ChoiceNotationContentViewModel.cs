using LChess.Abstract.Service;
using LChess.Abstract.ViewModel;
using LChess.Models.Chess;
using LChess.Models.Result;
using LChess.Util.Converter;
using LChess.Util.Enums;
using LChess.ViewModels.Messenger;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;

namespace LChess.ViewModels.DataContext.Contents;

/// <summary>
/// 기보선택 콘텐츠 뷰모델 
/// </summary>
public partial class ChoiceNotationContentViewModel : ObservableRecipient, IContentViewModel
{
    #region :: Constructor ::

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="popupWindowService"></param>
    public ChoiceNotationContentViewModel(IPopupWindowService popupWindowService, IJsonFileService jsonFileService, IUserSettingService userSettingService)
    {
        ////////////////////////////////////////
        // 서비스 등록
        ////////////////////////////////////////
        {
            _jsonFileService    = jsonFileService   ;
            _popupWindowService = popupWindowService;
            _userSettingService = userSettingService;
        }

        ////////////////////////////////////////
        // 멤버 초기화
        ////////////////////////////////////////
        {
            ContentType = LChessContentType.ChoiceNotation;

            GameHistoryList = new();

            LoadGameHistoryList();
        }
    }

    #endregion

    #region :: Services ::

    /// <summary>
    /// Json파일 서비스
    /// </summary>
    private readonly IJsonFileService _jsonFileService;

    /// <summary>
    /// 팝업윈도우 핸들링 서비스
    /// </summary>
    private readonly IPopupWindowService _popupWindowService;

    /// <summary>
    /// 유저설정 서비스
    /// </summary>
    private readonly IUserSettingService _userSettingService;

    #endregion

    #region :: Properties ::

    /// <summary>
    /// 기보 파일 리스트
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<GameHistoryFileModel> _gameHistoryList;

    /// <summary>
    /// 콘텐츠 타입
    /// </summary>
    public LChessContentType ContentType { get; init; }

    #endregion

    #region :: Methods ::

    /// <summary>
    /// 기보 리스트 가져오기
    /// </summary>
    private void LoadGameHistoryList()
    {
        GameHistoryList.Clear();

        //사용자 설정에서 기보 저장 경로를 불러옴.
        var notationDirectory = _userSettingService.GetUserSetting().SystemSetting.NotationSaveDirectory;

        //경로 파싱
        if(_jsonFileService.TryParseJsonPropertiesInDirectory(notationDirectory, out List<GameHistoryFileModel> result))
        {
            //파싱결과 순회
            foreach(var history in result)
            {
                //파일이름 속성이 비어있으면 잘못된 파일로 간주.
                if (string.IsNullOrEmpty(history.FileName)) continue;

                //유효한 파일만 목록에 추가
                GameHistoryList.Add(history);
            }
        }
    }

    /// <summary>
    /// 메신저 구독해제
    /// </summary>
    public void UnRegisterMessengers()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    #endregion

    #region :: Commands ::

    /// <summary>
    /// 홈으로 이동
    /// </summary>
    [RelayCommand]
    private void MoveToHome() => WeakReferenceMessenger.Default.Send(new MoveContentMessage(LChessContentType.Home));

    /// <summary>
    /// 기보 불러오기
    /// </summary>
    [RelayCommand]
    private void LoadHistory()
    {
        //불러온 파일
        var sourceFilePath = _popupWindowService.ShowSelectFilePopup();

        //1. 파일 유효성 검사
        if(string.IsNullOrEmpty(sourceFilePath) || !File.Exists(sourceFilePath)) return;

        //2. 복사할 경로
        var pasteDirectory = _userSettingService.GetUserSetting().SystemSetting.NotationSaveDirectory;

        //3. 불러온 파일이름 추출
        var fileName = Path.GetFileName(sourceFilePath);

        //4. 복사할 경로 + 불러온 파일이름 추출
        var pasteFilePath = Path.Combine(pasteDirectory, fileName);

        //5. 같은 위치면 수행하지 않음.
        if (sourceFilePath == pasteFilePath) return;

        //6. 파일 복사
        File.Copy(sourceFilePath, pasteFilePath);
    }

    /// <summary>
    /// 기보분석 화면으로 이동
    /// </summary>
    /// <param name="param"> 현재 선택된 기보 </param>
    [RelayCommand]
    private void MoveToAnalysis(object param)
    {
        if(param is GameHistoryFileModel model)
        {
            //화면전환 후
            WeakReferenceMessenger.Default.Send(new MoveContentMessage(LChessContentType.Analysis));

            //게임결과 전송
            WeakReferenceMessenger.Default.Send(new GameHistoryMessage(model));
        }
    }

    #endregion
}
