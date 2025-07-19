using LChess.Abstract.Service;

using LChess.Models.Result;

namespace LChess.Service.Engine;

/// <summary>
/// 스톡피쉬 엔진 서비스
/// </summary>
public class StockfishEngineService : IStockfishEngineService
{
	#region :: Properties ::

	/// <summary>
	/// 엔진 경로
	/// </summary>
	private readonly string _stockfishEngineDirectory;
	private readonly string _stockfishExecutablePath;

	/// <summary>
	/// Stockfish 엔진 프로세스
	/// </summary>
	private Process? _stockfishProcess;

	/// <summary>
	/// Stockfish 엔진 입력 스트림
	/// </summary>
	private StreamWriter? _stockfishInput;

	/// <summary>
	/// Stockfish 엔진 출력 스트림
	/// </summary>
	private StreamReader? _stockfishOutput;

	#endregion

	/// <summary>
	/// 생성자
	/// </summary>
	public StockfishEngineService()
	{
		// Stockfish 엔진 디렉터리 및 실행 파일 경로 설정
		_stockfishEngineDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Engine");
		_stockfishExecutablePath  = Path.Combine(_stockfishEngineDirectory, "Stockfish.exe");
	}

	#region :: Methods ::

	/// <summary>
	/// 엔진 시작 (비동기)
	/// </summary>
	/// <returns> 성공여부 </returns>
	public async Task<bool> StartEngineAsync()
	{
		Log.Information("Stockfish 엔진 시작...");

		//1. 엔진 프로세스 검사
		if(_stockfishProcess != null)
		{
			Log.Warning("엔진이 이미 실행 중");
			return true;
		}

		//2. 엔진 리소스 추출
		if (!ExtractStockfishExecutable())
		{
			Log.Error("엔진 리소스 추출 실패로 인한 엔진실행 실패.");
			return false;
		}

		//3. 엔진 프로세스 시작
		if (!await StartStockfishProcess())
		{
			Log.Error("엔진 프로세스 시작 실패.");
			return false;
		}

		//4. 엔진 초기화 커맨드 전송 후 결과 대기
		var response = await SendCommandAsync("uci", "uciok");

		return response?.Contains("uciok") == true;
	}

	/// <summary>
	/// 엔진 종료
	/// </summary>
	/// <returns> 성공여부 </returns>
	public bool StopEngine()
	{
		try
		{
			Log.Information("Stockfish 엔진 종료...");

			Log.Information("Stockfish 입력연결 해제");
			_stockfishInput?.Dispose();

			Log.Information("Stockfish 출력연결 해제");
			_stockfishOutput?.Dispose();

			Log.Information("Stockfish 프로세스 해제");
			_stockfishProcess?.Kill();
			_stockfishProcess?.Dispose();
		}
		catch(Exception ex)
		{
			Log.Error(ex, "Stockfish 엔진 종료 중 에러 발생");
			return false;
		}
		finally
		{
			Log.Information("Stockfish 관련 객체 초기화");
			_stockfishProcess = null;
			_stockfishInput   = null;
			_stockfishOutput  = null;
		}

		return true;
	}

	/// <summary>
	/// Stockfish 엔진 리소스 스트림을 어셈블리에서 획득.
	/// </summary>
	/// <returns> 엔진 리소스 스트림 </returns>
	private Stream? GetStockfishResourceStream()
	{
		try
		{
			//1. LChess.Util 어셈블리 접근 시도
			var assembly = AppDomain.CurrentDomain.GetAssemblies()
				.FirstOrDefault(a => a.FullName?.StartsWith("LChess.Util") == true);

			if(assembly == null)
			{
				Log.Fatal("LChess.Util 어셈블리 접근 실패.");
				return null;
			}

			//2. LChess.Util 어셈블리에서 Stockfish엔진 리소스 획득
			var resourceName = assembly.GetManifestResourceNames()
				.FirstOrDefault(name => name.EndsWith("Stockfish.exe"));

			//결과 반환
			return assembly.GetManifestResourceStream(resourceName ?? string.Empty);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "Stockfish 엔진 리소스 접근 중 오류 발생");
			return null;
		}
	}

	/// <summary>
	/// Stockfish 엔진 실행 파일을 어셈블리 리소스에서 추출하여 지정된 경로에 저장.
	/// </summary>
	/// <returns> 성공여부 </returns>
	private bool ExtractStockfishExecutable()
	{
		try
		{
			//1. 엔진 실행 파일 경로 설정 및 파일 존재여부 확인
			if(Path.Exists(_stockfishExecutablePath))
			{
				Log.Information($"이미 Stockfish 엔진이 존재하여 리소스 추출 생략.");
				return true;
			}

			//2. 엔진 리소스 획득
			using var stream = GetStockfishResourceStream();
			if (stream == null)
			{
				Log.Fatal($"Stockfish 엔진 리소스 획득 실패.");
				return false;
			}

			//3. 엔진 리소스 출력 디렉터리 검사 후 필요 시 생성
			if(!Directory.Exists(_stockfishEngineDirectory))
			{
				Directory.CreateDirectory(_stockfishEngineDirectory);
				Log.Information($"엔진 폴더 생성");
			}

			//4. 엔진 리소스 출력디렉터리로 복사
			using var fileStream = new FileStream(_stockfishExecutablePath, FileMode.Create, FileAccess.Write);
			stream.CopyTo(fileStream);

			Log.Information($"Stockfish 엔진 리소스 추출 성공.");

			// 성공
			return true;
		}
		catch (Exception ex)
		{
			Log.Error(ex, $"Stockfish 엔진 리소스 추출 중 오류 발생");
			return false;
		}
	}

	/// <summary>
	/// Stockfish 엔진 프로세스를 시작.
	/// </summary>
	/// <returns> 성공여부 </returns>
	private async Task<bool> StartStockfishProcess()
	{
		try
		{
			//1. 프로세스 시작 정보 설정
			var startInfo = new ProcessStartInfo
			{
				FileName = _stockfishExecutablePath,
				RedirectStandardInput = true,
				RedirectStandardOutput = true,
				CreateNoWindow = true,
				UseShellExecute = false
			};

			//2. 프로세스 시작
			_stockfishProcess = new Process { StartInfo = startInfo };
			_stockfishProcess.Start();

			//3. 입출력 스트림 기억
			_stockfishInput = _stockfishProcess.StandardInput;
			_stockfishOutput = _stockfishProcess.StandardOutput;

			var output = await _stockfishOutput.ReadLineAsync();

			Log.Information("Stockfish 엔진 프로세스 시작.");
			return output?.Contains("Stockfish") == true;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "Stockfish 엔진 프로세스 시작 중 오류 발생");
			return false;
		}
	}

	/// <summary>
	/// 엔진에 커맨드 전송 (비동기)
	/// Stockfish 엔진에 커맨드를 전송하고 응답을 기다립니다.
	/// </summary>
	/// <param name="command"> 전송 할 커맨드 </param>
	/// <param name="output"> stockfish 응답 </param>
	public async Task<string?> SendCommandAsync(string command, string output)
	{
		if(_stockfishInput == null || _stockfishOutput == null)
		{
			Log.Fatal("Stockfish 엔진 입력 스트림이 초기화되지 않음.");
			return null;
		}

		Log.Information($"커맨드 전송 : '{command}'");

		//1. 커맨드 전송
		await _stockfishInput.WriteLineAsync(command);
		await _stockfishInput.FlushAsync();

        //2. 응답이 지정되어있지 않으면 응답을 기다리지 않고 null 반환
        if (string.IsNullOrEmpty(output))
        {
            return null;
        }

        //3. Stockfish의 응답은 멀티라인이기 때문에,
        //   한줄씩 읽으면서 특정 단어가 들어가 있는 응답을 찾아야함.
        while (true)
		{
			//현재 라인
			var result = await _stockfishOutput.ReadLineAsync();

			// 응답을 찾았는지 확인
			if (result?.Contains(output) == true)
			{
				Log.Information($"응답 : '{result}'");
				return result;
			}
		}
	}

	/// <summary>
	/// 현재 보드 상태 반환
	/// </summary>
	/// <returns> StockFish 엔진 응답 </returns>
	public async Task<StockfishBoardCodeModel> GetCurrentBoard()
	{
		var result = new StockfishBoardCodeModel();

        if (_stockfishInput == null || _stockfishOutput == null)
		{
			Log.Fatal("Stockfish 엔진 입력 스트림이 초기화되지 않음.");
			return result;
		}

		Log.Information($"커맨드 전송 : 'd'");

		//1. 커맨드 전송
		await _stockfishInput.WriteLineAsync("d");
		await _stockfishInput.FlushAsync();

		var unitCodes = new List<string>();
		var checkers = new List<string>();

        //2. Stockfish의 응답은 멀티라인이기 때문에,
        while (true)
		{
			//현재 라인
			var line = await _stockfishOutput.ReadLineAsync();

			// 응답을 찾았는지 확인
			if (line?.Contains(@"|") == true)
			{
				Log.Information($"기물코드 : '{line}'");

                unitCodes.Add(line);
			}

            //마지막줄이면 정보 저장 후 루프 탈출
            if (line?.Contains("Checkers") == true)
            {
				Log.Information($"체크 정보 : '{line}'");

				var checkersArray = line.Split(' ');

				if (checkersArray.Length > 1)
				{
					checkers.AddRange(checkersArray.Skip(1));
                }

                break;
            }
        }

		result.SetTileCodeList(unitCodes);
		result.SetCheckerList (checkers );

        return result;
	}

	#endregion

}
