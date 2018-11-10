using System;
using System.Web.Mvc;

namespace CTS_Analytics.Models
{
	public class ErrorAttribute : HandleErrorAttribute
	{
		public override void OnException(ExceptionContext filterContext)
		{
			Exception e = filterContext.Exception;

			filterContext.ExceptionHandled = true;
			var model = new HandleErrorInfo(filterContext.Exception, "Exception", "Index");
			filterContext.Result = new ViewResult()
			{
				ViewName = "~/Views/Error/Exception.cshtml",
				ViewData = new ViewDataDictionary(model)
			};
		}
	}
}