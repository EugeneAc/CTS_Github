using CTS_Analytics.Filters;
using System.Web.Mvc;

namespace CTS_Analytics.Controllers
{
	[Culture]
	public class ErrorController : CtsAnalController
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

        public ActionResult UnAuth401()
        {
            ViewBag.Title = "Нет доступа / Unathorized";
            return View("Index");
        }

        public ActionResult Exception(ViewDataDictionary exModel)
		{
			return View("~/Views/Home/Exception.cshtml", exModel);
		}
	}
}