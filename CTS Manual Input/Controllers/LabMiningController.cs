using CTS_Manual_Input.Attributes;
using CTS_Manual_Input.Helpers;
using CTS_Manual_Input.Models.Common;
using CTS_Models.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CTS_Manual_Input.Models.LabModels;
using PagedList;
using CTS_Models;

namespace CTS_Manual_Input.Controllers
{
	[ErrorAttribute]
	[LabUserAuthorization]
	public class LabMiningController : Controller
    {
		private CtsDbContext _cdb = new CtsDbContext();

		[LabUserAuthorization]
		public ActionResult Index(int page = 1)
		{
			string userName = User.Identity.Name ?? "";
			int pagesize = 20;
			var locations = EquipmentProvider.GetUserLocations(_cdb, userName);
			var locationsArray = locations.Select(x => x.ID).ToArray();
			var analysis = _cdb.MiningAnalyzes.Where(t => locationsArray.Contains(t.LocationID)).Where(v => v.IsValid).ToList();
			@ViewBag.Title = "Анализы по добыче";

			return View(new MiningAnalysisView
			{
				Locations = locations,
				MiningAnalysis = analysis.OrderByDescending(t => t.AnalysisTimeStamp).ToPagedList(page, pagesize),
				CanEdit = UserHelper.CanEditUser(userName),
				CanDelete = UserHelper.CanDeleteUser(userName)
			});
		}

		public ActionResult AnalysisView(int? Id)
		{
			var analysis = new MiningAnalysis();

			if (Id != null)
			{
				try
				{
					analysis = _cdb.MiningAnalyzes.Where(x => x.ID == Id).FirstOrDefault();
				}
				catch
				{

				}
			}

			@ViewBag.Title = "Данные анализа по добыче";
			return View(analysis);
		}

		[HttpGet]
		[CanAddRoleAuthorization]
		public ActionResult Add(string LocationID)
		{
			var model = new MiningAnalysis();
			model.LocationID = LocationID;
			model.Location = EquipmentProvider.GetUserLocations(_cdb, User.Identity.Name ?? "").Where(x => x.ID == LocationID).FirstOrDefault();

			@ViewBag.Title = "Добавление анализа по добыче";
			return View(model);
		}

		[HttpPost]
		[CanAddRoleAuthorization]
		public ActionResult Add(MiningAnalysis model)
		{
			if (ModelState.IsValid)
			{
				model.LasEditDateTime = DateTime.Now;
				model.IsValid = true;
				model.OperatorName = UserHelper.GetOperatorName4DBInsertion(Request.UserHostName, User.Identity.Name);
				_cdb.MiningAnalyzes.Add(model);
				_cdb.SaveChanges();

				return RedirectToAction("Index");
			}

			model.Location = EquipmentProvider.GetUserLocations(_cdb, User.Identity.Name ?? "").Where(x => x.ID == model.LocationID).FirstOrDefault();
			@ViewBag.Title = "Добавление анализа по добыче";
			return View("Add", model);
		}

		[HttpGet]
		[CanEditRoleAuthorization]
		public ActionResult Edit(int? Id)
		{
			if (Id == null)
			{
				return HttpNotFound();
			}

			@ViewBag.Title = "Редактирование анализа по добыче";
			return View(_cdb.MiningAnalyzes.Include(x => x.Location).FirstOrDefault(x => x.ID == Id));
		}

		[HttpPost, ActionName("Edit")]
		[CanEditRoleAuthorization]
		public ActionResult EditConfirmed(MiningAnalysis model)
		{
			if (ModelState.IsValid)
			{
				MiningAnalysis analysis = _cdb.MiningAnalyzes.Find(model.ID);
				analysis.IsValid = false;
				analysis.LasEditDateTime = DateTime.Now;
				analysis.OperatorName = UserHelper.GetOperatorName4DBInsertion(Request.UserHostName, User.Identity.Name);
				_cdb.Entry(analysis).State = EntityState.Modified;

				model.LasEditDateTime = DateTime.Now;
				model.IsValid = true;
				model.OperatorName = UserHelper.GetOperatorName4DBInsertion(Request.UserHostName, User.Identity.Name);
				model.InheritedFrom = analysis.ID;
				_cdb.MiningAnalyzes.Add(model);

				_cdb.SaveChanges();

				return RedirectToAction("Index");
			}

			model.Location = EquipmentProvider.GetUserLocations(_cdb, User.Identity.Name ?? "").Where(x => x.ID == model.LocationID).FirstOrDefault();
			@ViewBag.Title = "Редактирование анализа по добыче";
			return View("Edit", model);
		}

		[CanDeleteRoleAuthorization]
		public ActionResult Delete(int? Id)
		{
			if (Id == null)
			{
				return HttpNotFound();
			}

			MiningAnalysis analisys = _cdb.MiningAnalyzes.Find(Id);
			if (analisys != null)
			{
				analisys.IsValid = false;
				analisys.LasEditDateTime = DateTime.Now;
				analisys.OperatorName = UserHelper.GetOperatorName4DBInsertion(Request.UserHostName, User.Identity.Name);

				_cdb.Entry(analisys).State = EntityState.Modified;
			}

			_cdb.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}