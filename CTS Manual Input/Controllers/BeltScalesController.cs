using CTS_Manual_Input.Attributes;
using CTS_Manual_Input.Helpers;
using CTS_Manual_Input.Models;
using CTS_Manual_Input.Models.Common;
using CTS_Models;
using CTS_Models.DBContext;
using PagedList;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using CTS_Core;

namespace CTS_Manual_Input.Controllers
{
	[ErrorAttribute]
	[CtsAuthorize(Roles = Roles.BeltUserRoleName)]
	public class BeltScalesController : Controller
	{
		private CtsDbContext _cdb;

		public BeltScalesController()
		{
			_cdb = new CtsDbContext();
		}

		public BeltScalesController(CtsDbContext cdbcontext)
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

			var beltScales = EquipmentProvider.GetUserAuthorizedEquipment<BeltScale>(_cdb, userName);
			var beltScalesArray = beltScales.Select(x => x.ID).ToArray();
			var transfers = _cdb.InternalTransfers.Where(t => beltScalesArray.Contains((int)t.EquipID))
				.Where(d => d.TransferTimeStamp >= DbFunctions.AddDays(System.DateTime.Now, -2));

			@ViewBag.Title = "Данные конвейерных весов";

			return View(new BeltScale_Transfer
			{
				BeltScales = beltScales,
				InternalTransfers = transfers.OrderByDescending(t => t.TransferTimeStamp).ToPagedList(page, pagesize),
				CanEdit = UserHelper.CanEditUser(userName),
				CanDelete = UserHelper.CanDeleteUser(userName)
			});
		}

		[CtsAuthorize(Roles = Roles.AddUserRoleName)]
		public ActionResult Add(int? scaleId, string name)
		{
			if (scaleId == null)
			{
				return HttpNotFound();
			}
			string userName = User.Identity.Name ?? "";
			var model = new BeltTransfer();
			model.EquipID = scaleId;
			model.LasEditDateTime = DateTime.Now;
			model.TransferTimeStamp = System.DateTime.Now;
			if (model.Equip != null)
				model.LotName = model.Equip.Name.ToString() + "-" + DateTime.Now.ToString("yyMMddHHmmss");
			model.ID = "B" + scaleId + (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

			ViewBag.Name = name;
			@ViewBag.Title = "Ввод данных конвейерных весов";
			return View("Add", model);
		}

		[HttpPost]
		[CtsAuthorize(Roles = Roles.AddUserRoleName)]
		[ValidateAntiForgeryToken]
		public ActionResult Add(BeltTransfer model, string name)
		{
			if (!((model.LotQuantity != null) && ((float)model.LotQuantity > 0)))
			{
				ModelState.AddModelError("LotQuantity", "Некорректный вес - должен быть больше нуля");
			}

			if (ModelState.IsValid)
			{
				var lastTransfer = _cdb.InternalTransfers.Where(x => x.EquipID == model.EquipID).Where(x => x.IsValid)
					.OrderByDescending(x => x.TransferTimeStamp).Take(1).FirstOrDefault();
				if (lastTransfer != null)
				{
					model.TotalQuantity = lastTransfer.TotalQuantity - lastTransfer.LotQuantity + model.LotQuantity;
				}
				else
				{
					model.TotalQuantity = model.LotQuantity;
				}

				model.LasEditDateTime = DateTime.Now;
				model.IsValid = false;
				model.Status = 1;
				model.OperatorName = UserHelper.GetOperatorName4DBInsertion(Request.UserHostName, User.Identity.Name);
				_cdb.InternalTransfers.Add(model);
				_cdb.SaveChanges();

				return RedirectToAction("Index");
			}

			ViewBag.Name = name;
			@ViewBag.Title = "Ввод данных конвейерных весов";
			return View("Add", model);
		}

		[CtsAuthorize(Roles = Roles.EditUserRoleName)]
		public ActionResult Edit(string ID)
		{
			if (ID != null)
			{
				@ViewBag.Title = "Редактирование данных конвейерных весов";
				return View("Edit", _cdb.InternalTransfers.Find(ID));
			}

			return RedirectToAction("Index");
		}

		[HttpPost]
		[CtsAuthorize(Roles = Roles.EditUserRoleName)]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(BeltTransfer model)
		{
			if (!((model.LotQuantity != null) && ((float)model.LotQuantity > 0)))
			{
				ModelState.AddModelError("LotQuantity", "Некорректный вес - должен быть больше нуля");
			}

			if (ModelState.IsValid)
			{
				BeltTransfer transfer = _cdb.InternalTransfers.Find(model.ID);
				if (transfer == null) { transfer = _cdb.InternalTransfers.Find(model.ID); }
				transfer.IsValid = true;
				transfer.Status = 3;
				transfer.LasEditDateTime = DateTime.Now;
				transfer.OperatorName = UserHelper.GetOperatorName4DBInsertion(Request.UserHostName, User.Identity.Name);
				_cdb.Entry(transfer).State = EntityState.Modified;
				_cdb.SaveChanges();

				model.InheritedFrom = model.ID;
				model.ID = "B" + model.EquipID + (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
				model.OperatorName = UserHelper.GetOperatorName4DBInsertion(Request.UserHostName, User.Identity.Name);
				string name = Request.UserHostName;
				model.LasEditDateTime = DateTime.Now;
				model.IsValid = false;
				model.Status = 2;
				model.TotalQuantity = transfer.TotalQuantity - transfer.LotQuantity + model.LotQuantity;
				_cdb.InternalTransfers.Add(model);
				_cdb.SaveChanges();

				return RedirectToAction("Index");
			}

			@ViewBag.Title = "Редактирование данных конвейерных весов";
			return View("Edit", model);
		}

		[CtsAuthorize(Roles = Roles.DeleteUserRoleName)]
		public ActionResult Delete(string ID)
		{
			if (ID == null)
			{
				return HttpNotFound();
			}
			var transfer = _cdb.InternalTransfers.Find(ID);
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
	}
}


