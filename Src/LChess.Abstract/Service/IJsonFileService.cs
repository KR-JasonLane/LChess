namespace LChess.Abstract.Service;

/// <summary>
/// Json파일 서비스 추상화 인터페이스
/// </summary>
public interface IJsonFileService
{
    /// <summary>
    /// 객체를 Json직렬화 하여 저장
    /// </summary>
    /// <typeparam name="T"> 객체 타입 </typeparam>
    /// <param name="jsonObject"> 직렬화 할 객체 </param>
    /// <param name="jsonPath"> 저장 할 위치 </param>
    /// <returns> 저장여부 </returns>
    public bool SaveJsonProperties<T>(T jsonObject, string jsonPath) where T : new();

    /// <summary>
    /// Json파일 파싱. 없으면 해당 경로에 새 객체를 저장
    /// </summary>
    /// <typeparam name="T"> 객체 타입 </typeparam>
    /// <param name="jsonPath"> 파싱할 파일 경로 </param>
    /// <returns> 파싱데이터 </returns>
    public bool TryParseJsonProperties<T>(string jsonPath, out T? result) where T : new();
}
