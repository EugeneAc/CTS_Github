using CTS_Manual_Input.Attributes;
using CTS_Manual_Input.Helpers;
using CTS_Manual_Input.Models.Common;
using CTS_Manual_Input.Models.LabModels;
using CTS_Models;
using CTS_Models.DBContext;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace CTS_Manual_Input.Controllers
{
	[ErrorAttribute]
	public class LabWagonController : Controller
	{
		private CtsDbContext _cdb;
		public LabWagonController()
		{
			_cdb = new CtsDbContext();
		}

		public LabWagonController(CtsDbContext cdbcontext)
		{
			if (cdbcontext != null)
				_cdb = cdbcontext;
			else
				_cdb = new CtsDbContext();
		}

		[LabUserAuthorization]
		public ActionResult Index(int page = 1)
		{
			string userName = User.Identity.Name ?? "";
			int pagesize = 20;

			var wagonScales = EquipmentProvider.GetUserAuthorizedEquipment<WagonScale>(_cdb, userName);
			var wagonScalesArray = wagonScales.Select(x => x.ID).ToArray();
			var transfers = _cdb.WagonTransfers.Where(t => wagonScalesArray.Contains((int)t.EquipID))
				.Where(v => v.IsValid).Where(d => d.TransferTimeStamp >= DbFunctions.AddDays(System.DateTime.Now, -2));

			transfers.OrderByDescending(o => o.TransferTimeStamp);
			var groups = transfers.GroupBy(l => l.LotName).ToList();
			List<WagonBatch> batches = new List<WagonBatch>();

			foreach (var g in groups)
			{

				WagonBatch batch = new WagonBatch();
				float netto = transfers.Where(b => b.LotName == g.Key.ToString()).Sum(s => s.Brutto) - transfers.Where(b => b.LotName == g.Key.ToString()).Sum(s => s.Tare);

				batch.LotName = g.Key.ToString();
				batch.TransferTimeStamp = g.First().TransferTimeStamp;
				batch.WagonQuantity = g.Count();
				batch.FromDest = g.First().FromDest.LocationName;
				batch.ToDest = g.First().ToDest;
				batch.Item = g.First().Item.Name;
				batch.Netto = netto;
				batch.WagonAnalysisID = g.First().AnalysisID;
				batch.WagonAnalysis = g.First().Analysis;

				batches.Add(batch);
			}

			@ViewBag.Title = "Анализы для вагонных весов";

			return View(new WagonBatchView
			{
				WagonBatches = batches.ToPagedList(page, pagesize),
				CanEdit = UserHelper.CanEditUser(User.Identity.Name),
				CanDelete = UserHelper.CanDeleteUser(User.Identity.Name)
			});
		}

		[LabUserAuthorization]
		public ActionResult AnalysisView(WagonBatch batch)
		{
			if (batch.WagonAnalysisID != null)
			{
				try
				{
					batch.WagonAnalysis = _cdb.WagonAnalyzes.Where(x => x.ID == batch.WagonAnalysisID).FirstOrDefault();
				}
				catch
				{

				}
			}

			@ViewBag.Title = "Данные проведенного анализа для вагонных весов";
			return View(batch);
		}

		[LabUserAuthorization]
		[CanAddRoleAuthorization]
		public ActionResult Add(string lotName)
		{
			@ViewBag.LotName = lotName;
			WagonAnalysis model = new WagonAnalysis();

			@ViewBag.Title = "Добавление анализа для вагонных весов";
			return View(model);
		}

		[HttpPost]
		[LabUserAuthorization]
		[CanAddRoleAuthorization]
		public ActionResult Add(WagonAnalysis model, string lotName)
		{
			if (ModelState.IsValid)
			{
				model.LasEditDateTime = DateTime.Now;
				model.IsValid = true;
				model.OperatorName = UserHelper.GetOperatorName4DBInsertion(Request.UserHostName, User.Identity.Name);
				//cdb.WagonAnalyzes.Add(model);

				List<WagonTransfer> transfers = new List<WagonTransfer>();
				transfers = _cdb.WagonTransfers.Where(x => x.LotName == lotName).ToList();
				transfers = _cdb.WagonTransfers.Where(x => x.LotName == lotName).ToList();

				foreach (var t in transfers)
				{
					t.Analysis = model;
					_cdb.Entry(t).State = EntityState.Modified;
				}

				_cdb.SaveChanges();

				return RedirectToAction("Index");
			}

			@ViewBag.LotName = lotName;

			@ViewBag.Title = "Добавление анализа для вагонных весов";
			return View("Add", model);
		}


		[HttpGet]
		[LabUserAuthorization]
		[CanEditRoleAuthorization]
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}

			@ViewBag.Title = "Редактирование анализа для вагонных весов";
			return View(_cdb.WagonAnalyzes.Find(id));
		}

		[HttpPost, ActionName("Edit")]
		[LabUserAuthorization]
		[CanEditRoleAuthorization]
		public ActionResult EditConfirmed(WagonAnalysis model)
		{
			if (ModelState.IsValid)
			{
				WagonAnalysis wagonAnalysis = _cdb.WagonAnalyzes.Find(model.ID);
				wagonAnalysis.IsValid = false;
				wagonAnalysis.LasEditDateTime = DateTime.Now;
				wagonAnalysis.OperatorName = UserHelper.GetOperatorName4DBInsertion(Request.UserHostName, User.Identity.Name);
				_cdb.Entry(wagonAnalysis).State = EntityState.Modified;

				model.LasEditDateTime = DateTime.Now;
				model.IsValid = true;
				model.OperatorName = UserHelper.GetOperatorName4DBInsertion(Request.UserHostName, User.Identity.Name);
				model.InheritedFrom = wagonAnalysis.ID;

				List<WagonTransfer> transfers = new List<WagonTransfer>();
				transfers = _cdb.WagonTransfers.Where(x => x.AnalysisID == model.ID).ToList();
				if (transfers != null)
				{
					foreach (var t in transfers)
					{
						t.Analysis = model;
						_cdb.Entry(t).State = EntityState.Modified;
					}
				}

				_cdb.SaveChanges();

				return RedirectToAction("Index");
			}

			@ViewBag.Title = "Редактирование анализа для конвейерных весов";
			return View("Edit", model);
		}

		[LabUserAuthorization]
		[CanDeleteRoleAuthorization]
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}

			WagonAnalysis analisys = _cdb.WagonAnalyzes.Find(id);
			if (analisys != null)
			{
				analisys.IsValid = false;
				analisys.LasEditDateTime = DateTime.Now;
				analisys.OperatorName = UserHelper.GetOperatorName4DBInsertion(Request.UserHostName, User.Identity.Name);

				_cdb.Entry(analisys).State = EntityState.Modified;
			}

			List<WagonTransfer> transfers = new List<WagonTransfer>();
			transfers = _cdb.WagonTransfers.Where(x => x.AnalysisID == id).ToList();
			if (transfers != null)
			{
				foreach (var t in transfers)
				{
					t.AnalysisID = null;
					_cdb.Entry(t).State = EntityState.Modified;
				}
			}

			_cdb.SaveChanges();
			return RedirectToAction("Index");
		}

	}
}