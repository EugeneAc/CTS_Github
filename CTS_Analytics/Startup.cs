using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(CTS_Analytics.Startup))]
namespace CTS_Analytics
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			app.MapSignalR();
		}
	}
}