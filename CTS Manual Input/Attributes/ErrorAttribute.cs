using CTS_Manual_Input.Helpers;
using System;
using System.Web.Mvc;

namespace CTS_Manual_Input.Models.Common
{
    public class ErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            Exception e = filterContext.Exception;

            //Log Exception e
            HandleException.LogException(e);

            filterContext.ExceptionHandled = true;
            var model = new HandleErrorInfo(filterContext.Exception, "Home", "Failure");
            filterContext.Result = new ViewResult()
            {
                ViewName = "~/Views/Home/Failure.cshtml",
                ViewData = new ViewDataDictionary(model)
            };
        }
    }
}