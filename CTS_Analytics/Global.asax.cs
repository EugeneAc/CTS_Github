//using ArchestrA.MxAccess;
using CTS_Analytics.App_Start;
using CTS_Analytics.Controllers;
using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CTS_Analytics
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);

			BundleConfig.RegisterBundles(BundleTable.Bundles);
		}

		protected void Application_BeginRequest()
		{
			// чтобы работали query запросы к api контроллерам
			if (Request.Headers.AllKeys.Contains("Origin", StringComparer.OrdinalIgnoreCase) &&
				Request.HttpMethod == "OPTIONS")
			{
				Response.Flush();
			}
		}

		protected void Session_Start(object sender, EventArgs e)
		{
			
		}

		protected void Application_Dispose()
		{
			
		}

	}
}
