using CTS_Core;
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
	[CtsAuthorize(Roles = Roles.LabUserRoleName)]
	public class LabSkipController : Controller
	{
		private CtsDbContext _cdb;
		public LabSkipController()
		{
			_cdb = new CtsDbContext();
		}

		public LabSkipController(CtsDbContext cdbcontext)
		{
			if (cdbcontext != null)
				_cdb = cdbcontext;
			else
				_cdb = new CtsDbContext();
		}

		public ActionResult Index(int page = 1)
		{
			string userName = User.Identity.Name ?? "";
			int pagesize = 20;

			var skips = EquipmentProvider.GetUserAuthorizedEquipment<Skip>(_cdb, User.Identity);
			var skipsArray = skips.Select(x => x.ID).ToArray();
			var transfers = _cdb.SkipTransfers.Where(t => skipsArray.Contains((int)t.EquipID))
				.Where(v => v.IsValid).Where(d => d.TransferTimeStamp >= DbFunctions.AddDays(System.DateTime.Now, -2));

			@ViewBag.Title = "Анализы для скиповых подъемов";

			return View(new SkipTransfersView
			{
				SkipTransfers = transfers.OrderByDescending(t => t.TransferTimeStamp).ToPagedList(page, pagesize),
				CanEdit = CtsAuthorizeProvider.CanEditUser(User.Identity),
				CanDelete = CtsAuthorizeProvider.CanDeleteUser(User.Identity)
			});
		}

		public ActionResult AnalysisView(string TransferID)
		{
			SkipAnalysis analysis = new SkipAnalysis();

			if (TransferID != "")
			{
				try
				{
					analysis = _cdb.SkipTransfers.Where(x => x.ID == TransferID).FirstOrDefault().Analysis;
				}
				catch
				{

				}
			}

			ViewBag.TransferID = TransferID;

			@ViewBag.Title = "Данные проведенного анализа для скипового подъема";
			return View(analysis);
		}

		[CtsAuthorize(Roles = Roles.AddUserRoleName)]
		public ActionResult AddBatchConfirm(string alltransfers)
		{
			if(alltransfers == "")
			{
				return RedirectToAction("Index");
			}

			ViewBag.alltransfers = alltransfers;

			@ViewBag.Title = "Добавление анализа к нескольким партиям";
			return View("AddBatchConfirm");
		}

		[HttpGet]
		[CtsAuthorize(Roles = Roles.AddUserRoleName)]
		public ActionResult Add(string alltransfers)
		{
			@ViewBag.alltransfers = alltransfers;
			SkipAnalysis model = new SkipAnalysis();

			@ViewBag.Title = "Добавление анализа для скипового подъема";
			return View(model);
		}

		[HttpPost]
		[CtsAuthorize(Roles = Roles.AddUserRoleName)]
		public ActionResult Add(SkipAnalysis model, string alltransfers)
		{
			if (ModelState.IsValid)
			{
				//Get the list of transfers to add analysis
				string[] transfersArray = alltransfers.Split(',');
				List<SkipTransfer> transfers = new List<SkipTransfer>();

				foreach (var t in transfersArray)
				{
					if (t != "")
					{
						try
						{
							transfers.Add(_cdb.SkipTransfers.Where(x => x.ID == t).FirstOrDefault());
						}
						catch
						{

						}
					}
				}

				//Bind model
				model.LasEditDateTime = DateTime.Now;
				model.IsValid = true;
				model.OperatorName = User.Identity.Name;

				foreach (var t in transfers)
				{
					if (t.Analysis != null)
					{
						SkipAnalysis analisys = new SkipAnalysis();
						analisys = _cdb.SkipAnalyzes.Where(x => x.ID == t.AnalysisID).FirstOrDefault();
						analisys.IsValid = false;
						_cdb.Entry(analisys).State = EntityState.Modified;
					}

					t.Analysis = model;
					_cdb.Entry(t).State = EntityState.Modified;
				}

				_cdb.SaveChanges();

				return RedirectToAction("Index");
			}

			@ViewBag.alltransfers = alltransfers;

			@ViewBag.Title = "Добавление анализа для скипового подъема";
			return View("Add", model);
		}

		[HttpGet]
		[CtsAuthorize(Roles = Roles.EditUserRoleName)]
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}

			@ViewBag.Title = "Редактирование анализа для скипового подъема";
			return View(_cdb.SkipAnalyzes.Find(id));
		}

		[HttpPost, ActionName("Edit")]
		[CtsAuthorize(Roles = Roles.EditUserRoleName)]
		public ActionResult EditConfirmed(SkipAnalysis model)
		{
			if (ModelState.IsValid)
			{
				SkipAnalysis skipAnalysis = _cdb.SkipAnalyzes.Find(model.ID);
				skipAnalysis.IsValid = false;
				skipAnalysis.LasEditDateTime = DateTime.Now;
				skipAnalysis.OperatorName = User.Identity.Name;
				_cdb.Entry(skipAnalysis).State = EntityState.Modified;

				model.LasEditDateTime = DateTime.Now;
				model.IsValid = true;
				model.OperatorName = User.Identity.Name;
				model.InheritedFrom = skipAnalysis.ID;

				List<SkipTransfer> transfers = new List<SkipTransfer>();
				transfers = _cdb.SkipTransfers.Where(x => x.AnalysisID == model.ID).ToList();
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

			@ViewBag.Title = "Редактирование анализа для скипового подъема";
			return View("Edit", model);
		}

		[CtsAuthorize(Roles = Roles.DeleteUserRoleName)]
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}

			SkipAnalysis analisys = _cdb.SkipAnalyzes.Find(id);
			if (analisys != null)
			{
				analisys.IsValid = false;
				analisys.LasEditDateTime = DateTime.Now;
				analisys.OperatorName = User.Identity.Name;

				_cdb.Entry(analisys).State = EntityState.Modified;
			}

			List<SkipTransfer> transfers = new List<SkipTransfer>();
			transfers = _cdb.SkipTransfers.Where(x => x.AnalysisID == id).ToList();
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