namespace LChess.Models.Result;

/// <summary>
/// 다이얼로그 결과 모델
/// </summary>
public class DialogResultModel
{
    #region :: Constructor ::

    /// <summary>
    /// 생성자
    /// </summary>
    public DialogResultModel(bool buttonResult, string response)
    {
        ButtonResult = buttonResult;

        Response = response;
    }

    #endregion

    #region :: Properties ::

    /// <summary>
    /// 버튼 결과 (OK : true, CANCEL : false)
    /// </summary>
    public readonly bool ButtonResult;

    /// <summary>
    /// 응답 (비어있을 수 있음.)
    /// </summary>
    public readonly string Response;

    #endregion
}
