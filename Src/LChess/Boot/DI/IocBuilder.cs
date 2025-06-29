﻿using LChess.Abstract.Service;

using LChess.DataBinding.ViewModel.Shell;
using LChess.DataBinding.ViewModel.Content;

using LChess.Service.Setting;
using LChess.Service.Window;
using LChess.Service.Engine;

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
			services.AddSingleton<IStockfishEngineService, StockfishEngineService>();
			services.AddSingleton<IWindowHandlingService , WindowHandlingService >();
			services.AddSingleton<IUserSettingService    , UserSettingService    >();
		}


		////////////////////////////////////////
		/// ViewModel 등록
		////////////////////////////////////////
		{
			services.AddTransient<LChessWindowViewModel    >();
			services.AddTransient<HomeContentViewModel     >();
			services.AddTransient<ChessGameContentViewModel>();
		}

		return services.BuildServiceProvider();
	}
}
