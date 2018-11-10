using CTS_Analytics.Models;
using CTS_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CTS_Analytics.Filters;
using CTS_Models.DBContext;

namespace CTS_Analytics.Controllers
{
	[Culture]
	public class HomeController : Controller
	{
		CtsDbContext cdb = new CtsDbContext();
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult GetScales(string Locations)
		{
			string[] locations = Locations.Split(Convert.ToChar(@","));
			var scales = new List<WagonScale>();
			foreach (var l in locations)
			{
				scales.AddRange(cdb.WagonScales.Where(s => s.LocationID == l));
			}
			ViewBag.Scales = scales.Select(N => new SelectListItem { Text = N.Name, Value = N.ID.ToString() }); ;
			ViewBag.Message = "Your application description page.";

			return PartialView("_ScalesDynDropDown");
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}

		public ActionResult ChangeCulture(string lang)
		{
			string returnUrl = Request.UrlReferrer.AbsoluteUri;
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
			string s = lang + "lang";
			ViewBag.ActiveLanguage = s;
			return Redirect(returnUrl);
		}
	}
}