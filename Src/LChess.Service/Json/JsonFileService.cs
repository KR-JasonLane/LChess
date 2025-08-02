using LChess.Abstract.Service;
using Newtonsoft.Json;

namespace LChess.Service.Json;

public class JsonFileService : IJsonFileService
{
    /// <summary>
    /// 객체를 Json직렬화 하여 저장
    /// </summary>
    /// <typeparam name="T"> 객체 타입 </typeparam>
    /// <param name="jsonObject"> 직렬화 할 객체 </param>
    /// <param name="jsonPath"> 저장 할 위치 </param>
    /// <returns> 저장여부 </returns>
    public bool SaveJsonProperties<T>(T jsonObject, string jsonPath) where T : new()
    {
        var directoryPath = Path.GetDirectoryName(jsonPath);

        ////////////////////////////////////////
        // 폴더경로, 파일경로 검사
        ////////////////////////////////////////
        {
            if(string.IsNullOrEmpty(directoryPath) || string.IsNullOrEmpty(jsonPath))
            {
                Log.Error($"======== Json 저장 실패");
                Log.Error($"Json 경로가 비어있음.");

                return false;
            }
        }

        ////////////////////////////////////////
        // 폴더생성 시도
        ////////////////////////////////////////
        {
            try
            {
                Directory.CreateDirectory(directoryPath);
            }
            catch(Exception e)
            {
                Log.Error($"======== Json 저장 실패");
                Log.Error(e, "폴더 생성 중 예외 발생");

                return false;
            }
        }

        ////////////////////////////////////////
        // 파일 생성 시도
        ////////////////////////////////////////
        {
            try
            {
                var jsonText = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);

                File.WriteAllText(jsonPath, jsonText);
            }
            catch (Exception e)
            {
                Log.Error($"======== Json 저장 실패");
                Log.Error(e, "파일 작성 중 예외 발생");

                return false;
            }
        }

        return File.Exists(jsonPath);
    }

    /// <summary>
    /// Json파일 파싱. 없으면 해당 경로에 새 객체를 저장
    /// </summary>
    /// <typeparam name="T"> 객체 타입 </typeparam>
    /// <param name="jsonPath"> 파싱할 파일 경로 </param>
    /// <returns> 파싱데이터 </returns>
    public bool TryParseJsonProperties<T>(string jsonPath, out T? result) where T : new()
    {
        result = default;

        ////////////////////////////////////////
        // 경로 검사 후 경로에 파일이 없으면
        // 생성해서 반환
        ////////////////////////////////////////
        {
            if (!File.Exists(jsonPath))
            {
                result = new T();

                var saveResult = SaveJsonProperties(result, jsonPath);

                return saveResult;
            }
        }

        ////////////////////////////////////////
        // 데이터 파싱
        ////////////////////////////////////////
        {
            try
            {
                var readText = File.ReadAllText(jsonPath);

                result = JsonConvert.DeserializeObject<T>(readText);
            }
            catch (Exception e)
            {
                Log.Error($"======== Json 파싱 실패");
                Log.Error(e, "파일 읽는 중 예외발생");

                result = default;
            }
        }

        return result != null;
    }

    /// <summary>
    /// 폴더에 있는 모든 파일을 파싱.
    /// </summary>
    /// <typeparam name="T"> 파싱 타입 </typeparam>
    /// <param name="directoryPath"> 폴더 경로 </param>
    /// <param name="result"> 파싱결과 </param>
    /// <returns> 성공여부 </returns>
    public bool TryParseJsonPropertiesInDirectory<T>(string directoryPath, out List<T> result) where T : new()
    {
        result = new();

        if (!Directory.Exists(directoryPath))  return false;

        string[] files = Directory.GetFiles(directoryPath);

        foreach(var file in files)
        {
            if (!TryParseJsonProperties(file, out T? parsed) || parsed == null) continue;

            result.Add(parsed);
        }

        return result.Count != 0;
    }
}
