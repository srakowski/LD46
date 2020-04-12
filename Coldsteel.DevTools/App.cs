using System.Threading;
using System.Windows;

namespace Coldsteel.DevTools
{
	public static class App
	{
		private static Application app;

		public static bool IsOpen { get; private set; } = false;

		public static void OpenDevTools(this Engine engine)
		{
			if (IsOpen) return;

			IsOpen = true;

			var thread = new Thread(() =>
			{
				app = new Application();
				app.Run(new MainWindow());
				app = null;
				IsOpen = false;
			});

			thread.SetApartmentState(ApartmentState.STA);
			thread.Start();
		}

		public static void CloseDevTools(this Engine engine)
		{
			if (!IsOpen) return;
			app.Dispatcher.Invoke(() => app.Shutdown());
		}
	}
}
