using CTS_Manual_Input.Helpers;
using CTS_Manual_Input.Models;
using CTS_Manual_Input.Models.Common;
using CTS_Models.DBContext;
using System.Web.Mvc;

namespace CTS_Manual_Input.Controllers
{
    [ErrorAttribute]
    public class HomeController : Controller
    {
		CtsDbContext db = new CtsDbContext();
        public ActionResult Index()
        {
            var model = new HomePageModel();
            var groups = UserHelper.GetUserDomainGroups(User.Identity.Name);
            model.CanEdit = UserHelper.CanEditUser(User.Identity.Name);
            model.CanDelete = UserHelper.CanDeleteUser(User.Identity.Name);
			model.Locations = EquipmentProvider.GetUserLocations(db, User.Identity.Name);

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
