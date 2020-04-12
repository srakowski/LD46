using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Coldsteel.DevTools
{
	public static class DevToolsExt
	{
		public static Engine Engine { get; private set; }

		public static void UseDevTools(this Engine engine)
		{
			if (Engine != null) return;
			Engine = engine;
			Task.Factory.StartNew(() =>
			{
				Host.CreateDefaultBuilder()
					.ConfigureWebHostDefaults(webBuilder =>
					{
						webBuilder.UseStartup<Startup>();
					})
					.Build()
					.Run();
			});
		}
	}
}
