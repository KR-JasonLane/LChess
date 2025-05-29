using LChess.Boot.App;
using LChess.Boot.DI;

namespace LChess.Boot;

/// <summary>
/// 프로그램 시작점
/// </summary>
internal class BootStrapper
{
	/// <summary>
	/// 프로그램 시작 메서드
	/// </summary>
	[STAThread]
	public static void Main()
	{
		// Ioc 컨테이너 빌드
		IocBuilder.Build(); 

		// LChessApp 인스턴스를 생성하고 실행
		var app = new LChessApp();
		app.Run();
	}
}
