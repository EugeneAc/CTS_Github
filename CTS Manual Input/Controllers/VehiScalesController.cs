using CTS_Manual_Input.Attributes;
using CTS_Manual_Input.Helpers;
using CTS_Manual_Input.Models.Common;
using CTS_Manual_Input.Models;
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
	[VehiUserAuthorization]
	[ErrorAttribute]
    public class VehiScalesController : Controller
    {
		CtsDbContext _cdb = new CtsDbContext();

        public ActionResult Index(int page = 1)
        {
			string userName = User.Identity.Name ?? "";
			int pagesize = 20;

			var vehiScales = EquipmentProvider.GetUserAuthorizedEquipment<VehiScale>(_cdb, userName);
			var vehiScalesArray = vehiScales.Select(x => x.ID).ToArray();
			var transfers = _cdb.VehiTransfers.Where(t => vehiScalesArray.Contains((int)t.EquipID))
				.Where(d => d.TransferTimeStamp >= DbFunctions.AddDays(System.DateTime.Now, -2));

			@ViewBag.Title = "Данные автомобильных весов";

			return View(new VehiScales_Transfers
			{
				VehiScales = vehiScales,
				Transfers = transfers.OrderByDescending(t => t.TransferTimeStamp).ToPagedList(page, pagesize),
				CanEdit = UserHelper.CanEditUser(userName),
				CanDelete = UserHelper.CanDeleteUser(userName)
			});
		}

		[CanAddRoleAuthorization]
        public ActionResult Add(int? scaleID)
        {
            if (scaleID == null)
            {
                return HttpNotFound();
            }

			var scale = EquipmentProvider.GetUserAuthorizedEquipment<VehiScale>(_cdb, User.Identity.Name ?? "").Where(s => s.ID == scaleID).SingleOrDefault();
			var model = new VehiTransfer()
			{
				TransferTimeStamp = DateTime.Now,
				ID = "V" + scaleID + (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds,
				EquipID = scaleID,
				FromDestID = scale.LocationID,
				FromDest = EquipmentProvider.GetUserLocations(_cdb, User.Identity.Name ?? "").Where(x => x.ID == scale.LocationID).FirstOrDefault()
			};
			GetDestinationsItemsAndScalesToVeiwBag(scale.LocationID);

			@ViewBag.Title = "Ввод данных автомобильных весов";
			return View("Add", model);
		}

		[HttpPost]
		[CanAddRoleAuthorization]
		[ValidateAntiForgeryToken]
		public ActionResult Add(VehiTransfer model, string name)
		{
			if (ModelState.IsValid)
			{
				model.LasEditDateTime = DateTime.Now;
				model.IsValid = false;
				model.Status = 1;
				model.OperatorName = UserHelper.GetOperatorName4DBInsertion(Request.UserHostName, User.Identity.Name);
				_cdb.VehiTransfers.Add(model);
				_cdb.SaveChanges();

				return RedirectToAction("Index");
			}

			ViewBag.Name = name;
			model.FromDest = EquipmentProvider.GetUserLocations(_cdb, User.Identity.Name ?? "").Where(x => x.ID == model.Equip.LocationID).FirstOrDefault();
			@ViewBag.Title = "Ввод данных автомобильных весов";
			return View("Add", model);
		}

		[CanEditRoleAuthorization]
		public ActionResult Edit(string ID)
		{
			if (ID != null)
			{
				var transfer = _cdb.VehiTransfers.Include(x => x.Equip).FirstOrDefault(x => x.ID == ID);
				GetDestinationsItemsAndScalesToVeiwBag(transfer.Equip.LocationID);
				@ViewBag.Title = "Редактирование данных автомобильных весов";
				return View("Edit", transfer);
			}

			return RedirectToAction("Index");
		}

		[HttpPost]
		[CanEditRoleAuthorization]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(VehiTransfer model)
		{
			if (ModelState.IsValid)
			{
				VehiTransfer transfer = _cdb.VehiTransfers.Find(model.ID);
				if (transfer == null) { transfer = _cdb.VehiTransfers.Find(model.ID); }
				transfer.IsValid = true;
				transfer.Status = 3;
				transfer.LasEditDateTime = DateTime.Now;
				transfer.OperatorName = UserHelper.GetOperatorName4DBInsertion(Request.UserHostName, User.Identity.Name);
				_cdb.Entry(transfer).State = EntityState.Modified;
				_cdb.SaveChanges();

				model.InheritedFrom = model.ID;
				model.ID = "V" + model.EquipID + (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
				model.OperatorName = UserHelper.GetOperatorName4DBInsertion(Request.UserHostName, User.Identity.Name);
				model.LasEditDateTime = DateTime.Now;
				model.IsValid = false;
				model.Status = 2;
				_cdb.VehiTransfers.Add(model);
				_cdb.SaveChanges();

				return RedirectToAction("Index");
			}

			@ViewBag.Title = "Редактирование данных автомобильных весов";
			return View("Edit", model);
		}

		[CanDeleteRoleAuthorization]
		public ActionResult Delete(string ID)
		{
			if (ID == null)
			{
				return HttpNotFound();
			}
			var transfer = _cdb.VehiTransfers.Find(ID);
			if (transfer != null)
			{
				transfer.IsValid = true;
				transfer.Status = 4;
				transfer.LasEditDateTime = DateTime.Now;
				transfer.OperatorName = UserHelper.GetOperatorName4DBInsertion(Request.UserHostName, User.Identity.Name);
				_cdb.Entry(transfer).State = EntityState.Modified;
				_cdb.SaveChanges();
			}

			return RedirectToAction("Index");
		}

		private void GetDestinationsItemsAndScalesToVeiwBag(string locationID)
		{
			SelectList destinations = new SelectList(_cdb.Locations.Where(l => l.ID.Contains(locationID)), "ID", "LocationName");
			SelectList items = new SelectList(EquipmentProvider.GetItems(_cdb, User.Identity.Name ?? ""), "ID", "Name");
			SelectList scales = new SelectList(EquipmentProvider.GetUserAuthorizedEquipment<VehiScale>(_cdb, User.Identity.Name ?? ""), "ID", "Name");

			ViewBag.Destinations = destinations;
			ViewBag.Items = items;
			ViewBag.Scales = scales;
		}
	}
}