using CTS_Manual_Input.Helpers;
using CTS_Manual_Input.Models;
using CTS_Manual_Input.Models.Common;
using CTS_Models.DBContext;
using System.Web.Mvc;
using CTS_Core;

namespace CTS_Manual_Input.Controllers
{
    [ErrorAttribute]
	[CtsAuthorize]
    public class HomeController : Controller
    {
		CtsDbContext db = new CtsDbContext();
        public ActionResult Index()
        {
            var model = new HomePageModel();
            model.CanEdit = CtsAuthorizeProvider.CanEditUser(User.Identity);
            model.CanDelete = CtsAuthorizeProvider.CanDeleteUser(User.Identity);
			model.Locations = EquipmentProvider.GetUserLocations(db, User.Identity);

            @ViewBag.Title = "Главная страница";
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Ручной ввод данных ЦПО";

            @ViewBag.Title = "О программе";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Контакты";

            @ViewBag.Title = "Контакты";
            return View();
        }

        public ActionResult Logout()
        {
            //FormsAuthentication.SignOut();
            return Redirect("~/");
        }

        public ActionResult Failure(ViewDataDictionary exModel)
        {
            return View("~/Views/Home/Failure.cshtml", exModel);
        }

    }

}
