
﻿using CTS_Analytics.Models;
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
