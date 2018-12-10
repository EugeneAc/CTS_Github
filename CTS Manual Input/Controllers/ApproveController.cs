using CTS_Core;
using CTS_Manual_Input.Helpers;
using CTS_Manual_Input.Models.Common;
using CTS_Models;
using CTS_Models.DBContext;
using System.Web.Mvc;

namespace CTS_Manual_Input.Controllers
{
	[ErrorAttribute]
	[CtsAuthorize(Roles = Roles.ApproveUserRoleName)]
	public class ApproveController : Controller
	{
		private CtsDbContext _cdb;

		public ApproveController()
		{
			_cdb = new CtsDbContext();
		}

		[CtsAuthorize(Roles = Roles.SkipUserRoleName)]
		public ActionResult SkipIndex()
		{
			@ViewBag.Title = "Ожидают подтверждения (данные скиповых подъемов)";

			return View(Approver.GetTransfersToApprove<SkipTransfer, Skip>(_cdb, User.Identity));
		}

		[CtsAuthorize(Roles = Roles.SkipUserRoleName)]
		public ActionResult SkipApprove(string transfers, bool isApproved)
		{
			Approver.ChangeTransfersStatus<SkipTransfer>(transfers, isApproved, User.Identity);

			return RedirectToAction("SkipIndex");
		}

		[CtsAuthorize(Roles = Roles.BeltUserRoleName)]
		public ActionResult BeltIndex()
		{
			@ViewBag.Title = "Ожидают подтверждения (данные конвейерных весов)";

			return View(Approver.GetTransfersToApprove<BeltTransfer, BeltScale>(_cdb, User.Identity));
		}

		[CtsAuthorize(Roles = Roles.BeltUserRoleName)]
		public ActionResult BeltApprove(string transfers, bool isApproved)
		{
			Approver.ChangeTransfersStatus<BeltTransfer>(transfers, isApproved, User.Identity);

			return RedirectToAction("BeltIndex");
		}

		[CtsAuthorize(Roles = Roles.WagonUserRoleName)]
		public ActionResult WagonIndex()
		{
			@ViewBag.Title = "Ожидают подтверждения (данные вагонных весов)";

			return View(Approver.GetTransfersToApprove<WagonTransfer, WagonScale>(_cdb, User.Identity));
		}

		[CtsAuthorize(Roles = Roles.WagonUserRoleName)]
		public ActionResult WagonApprove(string transfers, bool isApproved)
		{
			Approver.ChangeTransfersStatus<WagonTransfer>(transfers, isApproved, User.Identity);

			return RedirectToAction("WagonIndex");
		}

		[CtsAuthorize(Roles = Roles.VehiUserRoleName)]
		public ActionResult VehiIndex()
		{
			@ViewBag.Title = "Ожидают подтверждения (данные автомобильных весов)";

			return View(Approver.GetTransfersToApprove<VehiTransfer, VehiScale>(_cdb, User.Identity));
		}

		[CtsAuthorize(Roles = Roles.VehiUserRoleName)]
		public ActionResult VehiApprove(string transfers, bool isApproved)
		{
			Approver.ChangeTransfersStatus<VehiTransfer>(transfers, isApproved, User.Identity);

			return RedirectToAction("VehiIndex");
		}

		[CtsAuthorize(Roles = Roles.RockUserRoleName)]
		public ActionResult RockIndex()
		{
			@ViewBag.Title = "Ожидают подтверждения (данные утилизации породы)";

			return View(Approver.GetTransfersToApprove<RockUtilTransfer, RockUtil>(_cdb, User.Identity));
		}

		[CtsAuthorize(Roles = Roles.RockUserRoleName)]
		public ActionResult RockApprove(string transfers, bool isApproved)
		{
			Approver.ChangeTransfersStatus<RockUtilTransfer>(transfers, isApproved, User.Identity);

			return RedirectToAction("RockIndex");
		}
	}
}