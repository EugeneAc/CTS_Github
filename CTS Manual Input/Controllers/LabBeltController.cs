﻿using CTS_Core;
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
	public class LabBeltController : Controller
	{
		private CtsDbContext _cdb;
		public LabBeltController()
		{
			_cdb = new CtsDbContext();
		}

		public LabBeltController(CtsDbContext cdbcontext)
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

			var beltScales = EquipmentProvider.GetUserAuthorizedEquipment<BeltScale>(_cdb, User.Identity);
			var beltScalesArray = beltScales.Select(x => x.ID).ToArray();
			var transfers = _cdb.InternalTransfers.Where(t => beltScalesArray.Contains((int)t.EquipID))
				.Where(v => v.IsValid).Where(d => d.TransferTimeStamp >= DbFunctions.AddDays(System.DateTime.Now, -2));

			@ViewBag.Title = "Анализы для конвейерных весов";

			return View(new BeltTransfersView
			{
				BeltTransfers = transfers.OrderByDescending(t => t.TransferTimeStamp).ToPagedList(page, pagesize),
				CanEdit = CtsAuthorizeProvider.CanEditUser(User.Identity),
				CanDelete = CtsAuthorizeProvider.CanDeleteUser(User.Identity)
			});
		}

		public ActionResult AnalysisView(string TransferID)
		{
			BeltAnalysis analysis = new BeltAnalysis();

			if (TransferID != "")
			{
				try
				{
					analysis = _cdb.InternalTransfers.Where(x => x.ID == TransferID).FirstOrDefault().Analysis;
				}
				catch
				{

				}
			}

			ViewBag.TransferID = TransferID;
			@ViewBag.Title = "Данные проведенного анализа для конвейерных весов";
			return View(analysis);
		}

		[CtsAuthorize(Roles = Roles.AddUserRoleName)]
		public ActionResult AddBatchConfirm(string alltransfers)
		{
			if (alltransfers == "")
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
			BeltAnalysis model = new BeltAnalysis();

			@ViewBag.Title = "Добавление анализа для конвейерных весов";
			return View(model);
		}

		[HttpPost]
		[CtsAuthorize(Roles = Roles.AddUserRoleName)]
		public ActionResult Add(BeltAnalysis model, string alltransfers)
		{
			if (ModelState.IsValid)
			{
				//Get the list of transfers to add analysis
				string[] transfersArray = alltransfers.Split(',');
				List<BeltTransfer> transfers = new List<BeltTransfer>();

				foreach (var t in transfersArray)
				{
					if (t != "")
					{
						try
						{
							transfers.Add(_cdb.InternalTransfers.Where(x => x.ID == t).FirstOrDefault());
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
						BeltAnalysis analisys = new BeltAnalysis();
						analisys = _cdb.BeltAnalyzes.Where(x => x.ID == t.AnalysisID).FirstOrDefault();
						analisys.IsValid = false;
						_cdb.Entry(analisys).State = EntityState.Modified;
					}

					t.Analysis = model;
					_cdb.Entry(t).State = EntityState.Modified;
				}

				try
				{
					_cdb.SaveChanges();
				}
				catch
				(Exception)
				{ }
				return RedirectToAction("Index");
			}

			@ViewBag.alltransfers = alltransfers;
			@ViewBag.Title = "Добавление анализа для конвейерных весов";
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

			@ViewBag.Title = "Редактирование анализа для конвейерных весов";
			return View(_cdb.BeltAnalyzes.Find(id));
		}

		[HttpPost, ActionName("Edit")]
		[CtsAuthorize(Roles = Roles.EditUserRoleName)]
		public ActionResult EditConfirmed(BeltAnalysis model)
		{
			if (ModelState.IsValid)
			{
				BeltAnalysis beltAnalysis = _cdb.BeltAnalyzes.Find(model.ID);
				beltAnalysis.IsValid = false;
				beltAnalysis.LasEditDateTime = DateTime.Now;
				beltAnalysis.OperatorName = User.Identity.Name;
				_cdb.Entry(beltAnalysis).State = EntityState.Modified;

				model.LasEditDateTime = DateTime.Now;
				model.IsValid = true;
				model.OperatorName = User.Identity.Name;
				model.InheritedFrom = beltAnalysis.ID;

				List<BeltTransfer> transfers = new List<BeltTransfer>();
				transfers = _cdb.InternalTransfers.Where(x => x.AnalysisID == model.ID).ToList();
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

		[CtsAuthorize(Roles = Roles.DeleteUserRoleName)]
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return HttpNotFound();
			}

			BeltAnalysis analisys = _cdb.BeltAnalyzes.Find(id);
			if (analisys != null)
			{
				analisys.IsValid = false;
				analisys.LasEditDateTime = DateTime.Now;
				analisys.OperatorName = User.Identity.Name;

				_cdb.Entry(analisys).State = EntityState.Modified;
			}

			List<BeltTransfer> transfers = new List<BeltTransfer>();
			transfers = _cdb.InternalTransfers.Where(x => x.AnalysisID == id).ToList();
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