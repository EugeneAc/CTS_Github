using CTS_Core;
using CTS_Manual_Input.Attributes;
using CTS_Manual_Input.Helpers;
using CTS_Manual_Input.Models;
using CTS_Manual_Input.Models.Common;
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
	[CtsAuthorize(Roles = Roles.WarehouseUserRoleName)]
	public class WarehouseController : Controller
	{
		private CtsDbContext _cdb = new CtsDbContext();

		public ActionResult Index(int page = 1)
		{
			int pagesize = 20;
			List<WarehouseMeasure> measures = _cdb.WarehouseMeasures.ToList();
			List<Warehouse> warehouses = new List<Warehouse>();

			var locations = EquipmentProvider.GetUserLocations(_cdb, User.Identity.Name ?? "");
			if (locations != null)
			{
				var locationsArray = locations.Select(x => x.ID).ToList();
				warehouses.AddRange(_cdb.Warehouses.Where(n => locationsArray.Contains(n.LocationID)).ToList());
			}

			@ViewBag.Title = "Данные маркшейдерских замеров";
			return View(new WarehousesAndTransfersModel
			{
				Warehouses = warehouses,
				WarehouseMeasures = measures.OrderByDescending(t => t.LasEditDateTime).ToPagedList(page, pagesize),
			});
		}

		[CtsAuthorize(Roles = Roles.AddUserRoleName)]
		public ActionResult Add(int warehouseID, string name)
		{
			WarehouseMeasure model = new WarehouseMeasure();
			model.WarehouseID = warehouseID;
			model.MeasureDate = System.DateTime.Today;

			ViewBag.Name = name;
			@ViewBag.Title = "Добавление данных маркшейдерского замера";
			return View(model);
		}

		[HttpPost]
		[CtsAuthorize(Roles = Roles.AddUserRoleName)]
		public ActionResult Add(WarehouseMeasure model, string name)
		{
			if ((model.TotalMeasured <= 0) || (model.TotalMeasured == null))
			{
				ModelState.AddModelError("TotalMeasured", "Количество на складе должно быть больше 0");
			}
			if (model.MeasureDate > System.DateTime.Now)
			{
				ModelState.AddModelError("MeasureDate", "Неправильная дата - замер не может быть произведен в будущем");
			}
			if (ModelState.IsValid)
			{
				model.OperatorName = UserHelper.GetOperatorName4DBInsertion(Request.UserHostName, User.Identity.Name);
				model.LasEditDateTime = System.DateTime.Now;
				model.MeasureDate = model.MeasureDate.Date;

				_cdb.WarehouseMeasures.Add(model);
				_cdb.SaveChanges();

				return RedirectToAction("Index");
			}

			ViewBag.Name = name;
			@ViewBag.Title = "Добавление данных маркшейдерского замера";
			return View(model);
		}

		[CtsAuthorize(Roles = Roles.EditUserRoleName)]
		public ActionResult Edit(int Id)
		{
			if (Id == 0)
			{
				return HttpNotFound();
			}

			WarehouseMeasure measure = _cdb.WarehouseMeasures.Find(Id);
			if (measure != null)
			{

				return View("Edit", measure);
			}

			@ViewBag.Title = "Редактирование данных маркшейдерского замера";
			return RedirectToAction("Index");
		}

		[HttpPost]
		[CtsAuthorize(Roles = Roles.EditUserRoleName)]
		public ActionResult Edit(WarehouseMeasure model)
		{
			if (model.TotalMeasured <= 0)
			{
				ModelState.AddModelError("TotalMeasured", "Количество на складе должно быть больше 0");
			}
			if (model.MeasureDate > System.DateTime.Today)
			{
				ModelState.AddModelError("MeasureDate", "Неправильная дата - замер не может быть произведен в будущем");
			}
			if (ModelState.IsValid)
			{
				WarehouseMeasure measure = _cdb.WarehouseMeasures.Find(model.ID);
				measure.TotalMeasured = model.TotalMeasured;
				measure.Comment = model.Comment;
				measure.OperatorName = UserHelper.GetOperatorName4DBInsertion(Request.UserHostName, User.Identity.Name);
				measure.LasEditDateTime = System.DateTime.Now;
				_cdb.Entry(measure).State = EntityState.Modified;
				_cdb.SaveChanges();

				return RedirectToAction("Index");
			}

			@ViewBag.Title = "Редактирование данных маркшейдерского замера";
			return View("Edit", model);
		}

		[CtsAuthorize(Roles = Roles.WarehouseSetUserRoleName)]
		public ActionResult SetNewBalance(int? Id, double Quantity)
		{
			if (Id == null)
			{
				return HttpNotFound();
			}

			_cdb.WarehouseTransfers.Add(new WarehouseTransfer
			{
				WarehouseID = Id,
				TotalQuantity = Quantity,
				TransferTimeStamp = DateTime.Now,
				IsManual = true,
				LotQuantity = 0,
				ID = "WH" + Id + (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds
			});

			_cdb.SaveChanges();

			return RedirectToAction("Index");
		}
	}
}