using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CTS_Manual_Input.Models.SkipModels;
using CTS_Manual_Input.Helpers;
using PagedList;
using System.Data.Entity;
using CTS_Manual_Input.Models.Common;
using CTS_Models;
using CTS_Models.DBContext;
using System.ComponentModel.DataAnnotations;
using CTS_Core;

namespace CTS_Manual_Input.Controllers
{
	[ErrorAttribute]
	[CtsAuthorize(Roles = Roles.SkipUserRoleName)]
	public class SkipController : Controller
	{
		private CtsDbContext _cdb;

		public SkipController()
		{
			_cdb = new CtsDbContext();
		}

		public SkipController(CtsDbContext cdbcontext)
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
				.Where(d => d.TransferTimeStamp >= DbFunctions.AddDays(System.DateTime.Now, -2));

			@ViewBag.Title = "Данные скиповых подъемов";

			return View(new SkipsAndTransfersModel
			{
				Skips = skips,
				SkipTransfers = transfers.OrderByDescending(t => t.TransferTimeStamp).ToPagedList(page, pagesize),
				Counters = new Dictionary<string, string>(),
				CanEdit = CtsAuthorizeProvider.CanEditUser(User.Identity),
				CanDelete = CtsAuthorizeProvider.CanDeleteUser(User.Identity)
			});
		}

		[CtsAuthorize(Roles = Roles.AddUserRoleName)]
		public ActionResult Add(int? skipID, string name)
		{
			if (skipID == null)
			{
				return HttpNotFound();
			}
			string userName = User.Identity.Name ?? "";
			var model = new SkipTransferFormModel();
			model.EquipID = skipID;
			model.Equip = EquipmentProvider.GetUserAuthorizedEquipment<Skip>(_cdb, User.Identity).Where(s => s.ID == skipID).SingleOrDefault();
			model.SkipWeight = model.Equip.Weight;
			model.TransferTimeStamp = System.DateTime.Now;
			model.ID = "S" + skipID + (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

			ViewBag.Name = name;
			@ViewBag.Title = "Добавление данных скиповых подъемов";
            return View("Add", model);
		}

		[HttpPost]
		[CtsAuthorize(Roles = Roles.AddUserRoleName)]
		[ValidateAntiForgeryToken]
        public ActionResult Add(SkipTransferFormModel skipTransfer, string name)
		{
			if (ModelState.IsValid)
			{
                var transfer = new SkipTransfer();
			    transfer.ID = skipTransfer.ID;
			    transfer.EquipID = skipTransfer.EquipID;
			    transfer.LiftingID = skipTransfer.LiftingID;
			    transfer.TransferTimeStamp = skipTransfer.TransferTimeStamp;
			    transfer.LasEditDateTime = DateTime.Now;
			    transfer.TransferTimeStamp = skipTransfer.TransferTimeStamp;
			    transfer.IsValid = false;
			    transfer.Status = 1;
			    transfer.OperatorName =  User.Identity.Name;
				_cdb.SkipTransfers.Add(transfer);
				_cdb.SaveChanges();

                return RedirectToAction("Index");
			}

			ViewBag.Name = name;
			@ViewBag.Title = "Добавление данных скиповых подъемов";
			return View("Add", skipTransfer);
		}

		[HttpPost]
		[ValidateInput(false)]
		[CtsAuthorize(Roles = Roles.AddUserRoleName)]
		[ValidateAntiForgeryToken]
		public ActionResult FastAdd(FormCollection form)
		{
			string Counter = Convert.ToString(form["Counter"]);
			string SkipId = Convert.ToString(form["EquipID"]);
			var skipTransfer = new SkipTransfer();

			skipTransfer.LiftingID = Counter;
			skipTransfer.EquipID = Convert.ToInt32(SkipId);
			skipTransfer.LasEditDateTime = DateTime.Now;
			skipTransfer.TransferTimeStamp = DateTime.Now;
			skipTransfer.IsValid = false;
			skipTransfer.Status = 1;
			skipTransfer.SkipWeight = _cdb.Skips.FirstOrDefault(x => x.ID == skipTransfer.EquipID).Weight ;
			skipTransfer.OperatorName = User.Identity.Name;
			skipTransfer.ID = "S" + SkipId + (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

			var vc = new ValidationContext(skipTransfer, null, null);
			var vResults = new List<ValidationResult>();
			var isValid = Validator.TryValidateObject(skipTransfer, vc, vResults, true);
			if (isValid)
			{
				_cdb.SkipTransfers.Add(skipTransfer);
				_cdb.SaveChanges();
			}

			return RedirectToAction("Index");
		}

		[HttpGet]
		[CtsAuthorize(Roles = Roles.EditUserRoleName)]
		public ActionResult Edit(string ID)
		{
			if (ID != null)
			{
				@ViewBag.Title = "Редактирование данных скиповых подъемов";
			    var skipTransfer = _cdb.SkipTransfers.Find(ID);
			    if (skipTransfer != null)
			    {
			        var transfer = new SkipTransferFormModel(skipTransfer);

			        return View("Edit", transfer);
                }
			}

			return RedirectToAction("Index");
		}

		[HttpPost]
		[CtsAuthorize(Roles = Roles.EditUserRoleName)]
		[ValidateAntiForgeryToken]
        public ActionResult Edit(SkipTransferFormModel model)
		{
			if (ModelState.IsValid)
			{
				SkipTransfer editedTransfer = _cdb.SkipTransfers.Find(model.ID);
				editedTransfer.IsValid = true;
				editedTransfer.Status = 3;
				editedTransfer.LasEditDateTime = DateTime.Now;
				editedTransfer.OperatorName =  User.Identity.Name;
                _cdb.Entry(editedTransfer).State = EntityState.Modified;

				var transfer = new SkipTransfer();
				transfer.InheritedFrom = model.ID;
				transfer.ID = "S" + model.EquipID + (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
				transfer.LiftingID = model.LiftingID;
				transfer.LasEditDateTime = DateTime.Now;
				transfer.TransferTimeStamp = model.TransferTimeStamp;
				transfer.EquipID = model.EquipID;
				transfer.SkipWeight = model.SkipWeight;
				transfer.OperatorName =  User.Identity.Name;
				transfer.IsValid = false;
				transfer.Status = 2;

				_cdb.SkipTransfers.Add(transfer);
				_cdb.SaveChanges();

				return RedirectToAction("Index");
			}

            @ViewBag.Title = "Редактирование данных скиповых подъемов";
            return View("Edit", model);
		}

		[CtsAuthorize(Roles = Roles.DeleteUserRoleName)]
		public ActionResult Delete(string Id)
		{
			if (Id == null)
			{
				return HttpNotFound();
			}
			SkipTransfer transfer = _cdb.SkipTransfers.Find(Id);
			if (transfer != null)
			{
				transfer.IsValid = true;
				transfer.Status = 4;
				transfer.LasEditDateTime = DateTime.Now;
				transfer.OperatorName = User.Identity.Name;
				_cdb.Entry(transfer).State = EntityState.Modified;
				_cdb.SaveChanges();
			}

			return RedirectToAction("Index");
		}
	}
}