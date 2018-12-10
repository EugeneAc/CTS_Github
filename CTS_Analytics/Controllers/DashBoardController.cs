using CTS_Analytics.Filters;
using CTS_Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTS_Analytics.Controllers
{
	[Culture]
	[CtsAuthorize(Roles = Roles.AnalyticsRoleName)]
	public class DashBoardController : Controller
	{

		// GET: DashBoard
		public ActionResult Index()
		{
			
#if DEBUG
			ViewBag.HostName = "192.168.0.68";
#else
            ViewBag.HostName = Request.Url.Host;
#endif
			return View("Index");
		}

		public ActionResult Mine(string ID)
		{
			string viewID;
			switch (ID)
			{
				case "kuz":
					{
						viewID = "kuz";
                        ViewBag.LocationName = "Шахта им. Кузембаева / Kuzembayeva Mine";
                        break;
					}
				case "kost":
					{
						viewID = "kost";
                        ViewBag.LocationName = "Шахта им. Костенко / Kostenko Mine";
                        break;
					}
                case "abay":
                    {
                        viewID = "abay";
                        ViewBag.LocationName = "Шахта Абайская / Abay Mine";
                        break;
                    }
                case "len":
                    {
                        viewID = "len";
                        ViewBag.LocationName = "Шахта им. Ленина / Lenina Mine";
                        break;
                    }
                case "sar1":
					{
						viewID = "sar1";
                        ViewBag.LocationName = "Шахта Саранская - 1 / Saranskaya - 1 Mine";
                        break;
					}
                case "sar3":
                    {
                        viewID = "sar3";
                        ViewBag.LocationName = "Шахта Саранская - 3 / Saranskaya - 3 Mine";
                        break;
                    }
                case "kaz":
                    {
                        viewID = "kaz";
                        ViewBag.LocationName = "Шахта Казахстанская / Kazakhstanskaya Mine";
                        break;
                    }
                case "shah":
					{
						viewID = "shah";
                        ViewBag.LocationName = "Шахта Шахтинская / Shahtinskaya Mine";
                        break;
					}
                case "tent":
                    {
                        viewID = "tent";
                        ViewBag.LocationName = "Шахта Тентекская / Tentekstaya Mine";
                        break;
                    }
                default:
					viewID = "kuz";
					break;
			}
			ViewBag.LocationID = viewID;
			return View();
		}

        public ActionResult Alarm()
        {
            return View();
        }

        public ActionResult Alarm_other()
        {
            return View();
        }

        public ActionResult ChangeCulture(string lang)
		{
			string returnUrl = Request.UrlReferrer.AbsolutePath;
			// Список культур
			List<string> cultures = new List<string>() { "ru", "en", "kk" };
			if (!cultures.Contains(lang))
			{
				lang = "ru";
			}
			// Сохраняем выбранную культуру в куки
			HttpCookie cookie = Request.Cookies["lang"];
			if (cookie != null)
				cookie.Value = lang;   // если куки уже установлено, то обновляем значение
			else
			{
				cookie = new HttpCookie("lang");
				cookie.HttpOnly = false;
				cookie.Value = lang;
				cookie.Expires = DateTime.Now.AddYears(1);
			}
			Response.Cookies.Add(cookie);
			return Redirect(returnUrl);
		}

		private string getUserLang(HttpCookie cookie)
		{
			string lang = "";

			if (cookie != null)
				lang = cookie.Value;
			else
				lang = "ru";

			return lang;
		}
	}
}