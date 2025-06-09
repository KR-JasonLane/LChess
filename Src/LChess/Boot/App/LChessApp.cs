using LChess.Custom.UI.Unit;

using LChess.DataBinding.ViewModel.Shell;

namespace LChess.Boot.App;

/// <summary>
/// LChess 어플리케이션 몸체
/// </summary>
public class LChessApp : Application
{
	/// <summary>
	/// 어플리케이션 시작 시 호출
	/// </summary>
	/// <param name="e"> 시작 이벤트 파라미터 </param>
	protected override void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);

		////////////////////////////////////////
		/// Initialization
		////////////////////////////////////////
		{
			this.MergedDictionaries();
			this.CreateWindow();
		}
	}

	/// <summary>
	/// 리소스 사전 병합
	/// </summary>
	private void MergedDictionaries()
	{
		//데이터템플릿 리소스 사전 병합
		this.Resources.MergedDictionaries.Add(new ResourceDictionary
		{
			Source = new Uri("pack://application:,,,/LChess;component/Themes/Generic.xaml")
		});
	}

	/// <summary>
	/// 윈도우 생성
	/// </summary>
	private void CreateWindow()
	{
		this.MainWindow = new LChessWindow() { DataContext = Ioc.Default.GetService<LChessWindowViewModel>() };
		this.MainWindow.Show();
	}
}
