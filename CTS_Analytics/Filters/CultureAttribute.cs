using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace CTS_Analytics.Filters
{
  public class CultureAttribute : FilterAttribute, IActionFilter
  {
    public void OnActionExecuted(ActionExecutedContext filterContext)
    {

    }

    public void OnActionExecuting(ActionExecutingContext filterContext)
    {
      string cultureName = null;
      // Получаем куки из контекста, которые могут содержать установленную культуру
      HttpCookie cultureCookie = filterContext.HttpContext.Request.Cookies["lang"];
      if (cultureCookie != null)
        cultureName = cultureCookie.Value;
      else
        cultureName = "ru";

      // Список культур
      List<string> cultures = new List<string>() { "ru", "en", "kk"};
      if (!cultures.Contains(cultureName))
      {
        cultureName = "ru";
      }
      Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureName);
      Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(cultureName);
    }

  }
}
