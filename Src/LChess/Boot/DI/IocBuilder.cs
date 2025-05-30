using LChess.DataBinding.Shell;
using LChess.DataBinding.ViewModel.Content;

namespace LChess.Boot.DI;

/// <summary>
/// Ioc 빌더
/// </summary>
internal class IocBuilder
{
	/// <summary>
	/// Ioc 컨테이너를 빌드
	/// </summary>
	public static void Build()
	{
		var service = ConfigureService();
		
		Ioc.Default.ConfigureServices(service);
	}

	/// <summary>
	/// 의존성 주입
	/// </summary>
	/// <returns></returns>
	private static IServiceProvider ConfigureService()
	{
		var services = new ServiceCollection();

		////////////////////////////////////////
		/// Service 등록
		////////////////////////////////////////
		{

		}


		////////////////////////////////////////
		/// ViewModel 등록
		////////////////////////////////////////
		{
			services.AddTransient<LChessWindowViewModel>();
			services.AddTransient<HomeContentViewModel >();
		}

		return services.BuildServiceProvider();
	}
}
