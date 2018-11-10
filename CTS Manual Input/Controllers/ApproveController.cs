using CTS_Manual_Input.Attributes;
using CTS_Manual_Input.Helpers;
using CTS_Manual_Input.Models.ApproveModels;
using CTS_Manual_Input.Models.Common;
using CTS_Models;
using CTS_Models.DBContext;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace CTS_Manual_Input.Controllers
{
	[ErrorAttribute]
	[CanApproveRoleAuthorization]
	public class ApproveController : Controller
	{
		private CtsDbContext _cdb;

		public ApproveController()
		{
			_cdb = new CtsDbContext();
		}

		[SkipUserAuthorization]
		public ActionResult SkipIndex()
		{
			@ViewBag.Title = "Ожидают подтверждения (данные скиповых подъемов)";

			return View(Approver.GetTransfersToApprove<SkipTransfer, Skip>(_cdb, User.Identity.Name ?? ""));
		}

		[SkipUserAuthorization]
		public ActionResult SkipApprove(string transfers, bool isApproved)
		{
			Approver.ChangeTransfersStatus<SkipTransfer>(transfers, isApproved, User.Identity.Name ?? "");

			return RedirectToAction("SkipIndex");
		}

		[BeltUserAuthorization]
		public ActionResult BeltIndex()
		{
			@ViewBag.Title = "Ожидают подтверждения (данные конвейерных весов)";

			return View(Approver.GetTransfersToApprove<BeltTransfer, BeltScale>(_cdb, User.Identity.Name ?? ""));
		}

		[BeltUserAuthorization]
		public ActionResult BeltApprove(string transfers, bool isApproved)
		{
			Approver.ChangeTransfersStatus<BeltTransfer>(transfers, isApproved, User.Identity.Name ?? "");

			return RedirectToAction("BeltIndex");
		}

		[WagonUserAuthorization]
		public ActionResult WagonIndex()
		{
			@ViewBag.Title = "Ожидают подтверждения (данные вагонных весов)";

			return View(Approver.GetTransfersToApprove<WagonTransfer, WagonScale>(_cdb, User.Identity.Name ?? ""));
		}

		[WagonUserAuthorization]
		public ActionResult WagonApprove(string transfers, bool isApproved)
		{
			Approver.ChangeTransfersStatus<WagonTransfer>(transfers, isApproved, User.Identity.Name ?? "");

			return RedirectToAction("WagonIndex");
		}

		[WagonUserAuthorization]
		public ActionResult VehiIndex()
		{
			@ViewBag.Title = "Ожидают подтверждения (данные автомобильных весов)";

			return View(Approver.GetTransfersToApprove<VehiTransfer, VehiScale>(_cdb, User.Identity.Name ?? ""));
		}

		[WagonUserAuthorization]
		public ActionResult VehiApprove(string transfers, bool isApproved)
		{
			Approver.ChangeTransfersStatus<VehiTransfer>(transfers, isApproved, User.Identity.Name ?? "");

			return RedirectToAction("VehiIndex");
		}

		[RockUserAuthorization]
		public ActionResult RockIndex()
		{
			@ViewBag.Title = "Ожидают подтверждения (данные утилизации породы)";

			return View(Approver.GetTransfersToApprove<RockUtilTransfer, RockUtil>(_cdb, User.Identity.Name ?? ""));
		}

		[RockUserAuthorization]
		public ActionResult RockApprove(string transfers, bool isApproved)
		{
			Approver.ChangeTransfersStatus<RockUtilTransfer>(transfers, isApproved, User.Identity.Name ?? "");

			return RedirectToAction("RockIndex");
		}

		//[LabUserAuthorization]
		//public ActionResult LabBeltIndex()
		//{
		//	@ViewBag.Title = "Ожидают подтверждения (данные анализов по конвейерным весам)";

		//	return View(Approver.GetAnalysisToApprove<BeltAnalysis>(_cdb, User.Identity.Name ?? ""));
		//}

		//[LabUserAuthorization]
		//public ActionResult LabBeltApprove(string analysis, bool isApproved)
		//{
		//	Approver.ChangeAnalysisStatus<BeltAnalysis>(analysis, isApproved, User.Identity.Name ?? "");

		//	return RedirectToAction("LabBeltIndex");
		//}

		//[LabUserAuthorization]
		//public ActionResult LabSkipIndex()
		//{
		//	@ViewBag.Title = "Ожидают подтверждения (данные анализов по скиповым подъемам)";

		//	return View(Approver.GetAnalysisToApprove<SkipAnalysis>(_cdb, User.Identity.Name ?? ""));
		//}

		//[LabUserAuthorization]
		//public ActionResult LabSkipApprove(string analysis, bool isApproved)
		//{
		//	Approver.ChangeAnalysisStatus<SkipAnalysis>(analysis, isApproved, User.Identity.Name ?? "");

		//	return RedirectToAction("LabSkipIndex");
		//}

		//[LabUserAuthorization]
		//public ActionResult LabWagonIndex()
		//{
		//	@ViewBag.Title = "Ожидают подтверждения (данные анализов по вагонным весам)";

		//	return View(Approver.GetAnalysisToApprove<WagonAnalysis>(_cdb, User.Identity.Name ?? ""));
		//}

		//[LabUserAuthorization]
		//public ActionResult LabWagonApprove(string analysis, bool isApproved)
		//{
		//	Approver.ChangeAnalysisStatus<WagonAnalysis>(analysis, isApproved, User.Identity.Name ?? "");

		//	return RedirectToAction("WagonSkipIndex");
		//}
	}
}