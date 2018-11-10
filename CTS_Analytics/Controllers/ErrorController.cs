using CTS_Analytics.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTS_Analytics.Controllers
{
	[Culture]
	public class ErrorController : Controller
    {
		public ActionResult Index()
		{
			ViewBag.Title = "Regular Error";
			return View();
		}

		public ActionResult NotFound404()
		{
			ViewBag.Title = "Error 404 - File not Found";
			return View("Index");
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

		public ActionResult Exception(ViewDataDictionary exModel)
		{
			return View("~/Views/Home/Exception.cshtml", exModel);
		}
	}
}