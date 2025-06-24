using LChess.Abstract.Service;
using System.Diagnostics;
using System.IO;
namespace LChess.Service.Engine;

/// <summary>
/// 스톡피쉬 엔진 서비스
/// </summary>
public class StockfishEngineService : IStockfishEngineService
{
	#region :: Properties ::

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

	public StockfishEngineService()
	{
		_stockfishEngineDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Engine");
		_stockfishExecutablePath = Path.Combine(_stockfishEngineDirectory, "Stockfish.exe");
	}

	#region :: Methods ::

	public async Task<bool> StartEngineAsync(int timeout)
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
		if (!StartStockfishProcess())
		{
			Log.Error("엔진 프로세스 시작 실패.");
			return false;
		}

		//4. 엔진 초기화 커맨드 전송 후 결과 대기
		var response = await SendCommandAsync("uci");

		return response?.Contains("uciok") ?? false;
	}

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
	private bool StartStockfishProcess()
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

			Log.Information("Stockfish 엔진 프로세스 시작.");
			return true;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "Stockfish 엔진 프로세스 시작 중 오류 발생");
			return false;
		}
	}

	/// <summary>
	/// 엔진에 커맨드 전송 (비동기)
	/// </summary>
	/// <param name="command"> 전송 할 커맨드 </param>
	public async Task<string?> SendCommandAsync(string command)
	{
		if(_stockfishInput == null || _stockfishOutput == null)
		{
			Log.Fatal("Stockfish 엔진 입력 스트림이 초기화되지 않음.");
			return null;
		}

		await _stockfishInput.WriteLineAsync(command);
		await _stockfishInput.FlushAsync();

		var output = await _stockfishOutput.ReadLineAsync();

		return output;
	}

	#endregion

}
