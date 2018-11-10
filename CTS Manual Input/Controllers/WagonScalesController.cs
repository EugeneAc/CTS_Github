using CTS_Manual_Input.Attributes;
using CTS_Manual_Input.Helpers;
using CTS_Manual_Input.Models;
using CTS_Manual_Input.Models.Common;
using CTS_Models;
using CTS_Models.DBContext;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace CTS_Manual_Input.Controllers
{
	public class MyStringifiedNumberComparer : IEqualityComparer<string>
	{
		public bool Equals(string x, string y)
		{
			return (Int32.Parse(x) == Int32.Parse(y));
		}

		public int GetHashCode(string obj)
		{
			return Int32.Parse(obj).ToString().GetHashCode();
		}
	}

	[ErrorAttribute]
	public class WagonScalesController : Controller
	{
		private CtsDbContext _cdb;

		public WagonScalesController()
		{
			_cdb = new CtsDbContext();
		}

		public WagonScalesController(CtsDbContext cdbcontext)
		{
			if (cdbcontext != null)
				_cdb = cdbcontext;
			else
				_cdb = new CtsDbContext();
		}

		[WagonUserAuthorization]
		public ActionResult Index(int page = 1)
		{
			string userName = User.Identity.Name ?? "";
			int pagesize = 20;

			var wagonScales = EquipmentProvider.GetUserAuthorizedEquipment<WagonScale>(_cdb, userName);
			var wagonScalesArray = wagonScales.Select(x => x.ID).ToArray();
			var transfers = _cdb.WagonTransfers.Where(t => wagonScalesArray.Contains((int)t.EquipID))
				.Where(d => d.TransferTimeStamp >= DbFunctions.AddDays(System.DateTime.Now, -2));

			@ViewBag.Title = "Данные вагонных весов";

			return View(new WagonScales_Transfers
			{
				WagonScales = wagonScales,
				Transfers = transfers.OrderByDescending(t => t.TransferTimeStamp).ToPagedList(page, pagesize),
				CanEdit = UserHelper.CanEditUser(userName),
				CanDelete = UserHelper.CanDeleteUser(userName)
			});
		}

		[CanAddRoleAuthorization]
		[WagonUserAuthorization]
		public ActionResult Add(int? scaleID, bool incomming = false)
		{
			if (scaleID == null)
			{
				return HttpNotFound();
			}
            
			var scale = EquipmentProvider.GetUserAuthorizedEquipment<WagonScale>(_cdb, User.Identity.Name ?? "").Where(s => s.ID == scaleID).SingleOrDefault();
			var model = new TransferList();
            model.Transfer = new WagonTransfer()
			{
				TransferTimeStamp = DateTime.Now,
				LotName = _cdb.WagonScales.Find(scaleID).Location.ID + "_" + DateTime.Now.ToString("yyMMddHHmmss"),
				ID = "W" + scaleID + (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds,
				EquipID = scaleID
			};
			GetDestinationsItemsAndScalesToVeiwBag();
            if (!incomming)
			    ViewBag.FromDestinations = new SelectList(_cdb.Locations
                    .Where(l => l.ID == scale.LocationID)
                    .ToList()
                    , "ID", "LocationName");
            else
            {
                ViewBag.FromDestinations = new SelectList(_cdb.Locations.ToList(), "ID", "LocationName");
                model.Transfer.ToDest = _cdb.Locations
                .Where(l => l.ID == scale.LocationID)
                .Select(m => m.LocationName)
                .FirstOrDefault();
            }

            model.Incomming = incomming;
            @ViewBag.Title = "Ввод данных вагонных весов";
			return View("Add", model);
		}

        public ActionResult AutocompleteToDest(string term)
        {
            var result = Json(_cdb.Locations
                .Where(l => l.LocationName.Contains(term))
                .Select(a => new { label = a.LocationName })
                .Concat(_cdb.WagonTransfers
                .Where(c => c.ToDest.Contains(term))
                .Where(d => d.TransferTimeStamp >= DbFunctions.AddDays(System.DateTime.Now, -14))
                .Select(a => new { label = a.ToDest }))
                .Distinct() 
                ,JsonRequestBehavior.AllowGet);
            return result;
        }

		[HttpPost]
		[CanAddRoleAuthorization]
		[WagonUserAuthorization]
		public ActionResult _AllTransfers(TransferList model)
		{
			model.Transfer.LasEditDateTime = DateTime.Now;
			model.Transfer.IsValid = false;
			model.Transfer.Status = 1;
			model.Transfer.OperatorName = UserHelper.GetOperatorName4DBInsertion(Request.UserHostName, User.Identity.Name);
			var oldid = model.Transfer.ID;
			model.Transfer.ID = "W" + model.Transfer.EquipID + (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

			if (Request.Form["One"] != null)
			{
                var errorMessages = new List<string>();
				var modelbadstate = false;
				if (oldid == model.Transfer.ID)
				{
                    errorMessages.Add("Слишком быстро - попробуйте еще раз");
					modelbadstate = true;
				}
				if (string.IsNullOrEmpty(model.Transfer.SublotName))
				{
                    errorMessages.Add("Необходимо указать номер вагона - поле не может быть пустым");
					modelbadstate = true;
				}

				if (string.IsNullOrEmpty(model.Transfer.LotName))
				{
                    errorMessages.Add("Необходимо указать номер партии - поле не может быть пустым");
					modelbadstate = true;
				}

				if (model.Transfer.Brutto <= 0 || model.Transfer.Brutto > 150)
				{
                    errorMessages.Add("Нет или неправильно указан вес вагона - должен быть больше 0 и меньше 150");
					modelbadstate = true;
				}

				if (model.Transfer.Tare <= 0 || model.Transfer.Tare > model.Transfer.Brutto)
				{
                    errorMessages.Add("Нет или неправильно указан вес тары - должен быть больше нуля и меньше брутто");
					modelbadstate = true;
				}

				if (modelbadstate)
				{
					Response.StatusCode = (int)HttpStatusCode.BadRequest;
					return Json(new { modelbadstate = modelbadstate, ErrorMessage = string.Join(", ", errorMessages)});
				}
				model.Transfers.Add(model.Transfer);

				return PartialView(model);
			}
			if (Request.Form["SaveAll"] != null)
			{

				foreach (var mod in model.Transfers)
				{
					var vc = new ValidationContext(mod, null, null);
					var vResults = new List<ValidationResult>();
					var isValid = Validator.TryValidateObject(mod, vc, vResults, true);

					_cdb.WagonTransfers.Add(mod);
				}
				string message;

				_cdb.SaveChanges();
				message = "Данные партии зафиксированы";

				return JavaScript("RedirectToIndex('" + message + "');");
			}
			return View();
		}


		[CanEditRoleAuthorization]
		[WagonUserAuthorization]
		public ActionResult Edit(string Id)
		{
			if (Id != null)
			{
				GetDestinationsItemsAndScalesToVeiwBag();
				@ViewBag.Title = "Редактирование данных вагонных весов";

				return View(_cdb.WagonTransfers.Find(Id));
			}
			return RedirectToAction("Index");
		}

		[HttpPost]
		[CanEditRoleAuthorization]
		[WagonUserAuthorization]
		[ValidateAntiForgeryToken]
		public ActionResult SaveChanges(WagonTransfer model)
		{
			if (ModelState.IsValid)
			{
				WagonTransfer transfer = _cdb.WagonTransfers.Find(model.ID);
				transfer.IsValid = true;
				transfer.Status = 3;
				transfer.LasEditDateTime = DateTime.Now;
				transfer.OperatorName = UserHelper.GetOperatorName4DBInsertion(Request.UserHostName, User.Identity.Name);
				_cdb.Entry(transfer).State = EntityState.Modified;
				_cdb.SaveChanges();

				model.LasEditDateTime = DateTime.Now;
				model.IsValid = false;
				model.Status = 2;
				model.OperatorName = UserHelper.GetOperatorName4DBInsertion(Request.UserHostName, User.Identity.Name);
				model.InheritedFrom = model.ID;
				model.ID = "W" + model.EquipID + (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
				_cdb.WagonTransfers.Add(model);
				_cdb.SaveChanges();

				return RedirectToAction("Index");
			}
			GetDestinationsItemsAndScalesToVeiwBag();

			@ViewBag.Title = "Редактирование данных вагонных весов";
			return View("Edit", model);
		}

		[CanDeleteRoleAuthorization]
		[WagonUserAuthorization]
		public ActionResult Delete(string Id)
		{
			if (Id == null)
			{
				return HttpNotFound();
			}
			var transfer = _cdb.WagonTransfers.Find(Id);
			if (transfer != null)
			{
				transfer.IsValid = true;
				transfer.Status = 4;
				transfer.LasEditDateTime = DateTime.Now;
				transfer.OperatorName = UserHelper.GetOperatorName4DBInsertion(Request.UserHostName, User.Identity.Name);
				_cdb.Entry(transfer).State = EntityState.Modified;
				_cdb.SaveChanges();
			}

			return RedirectToAction("Index", "WagonScales");
		}

		private void GetDestinationsItemsAndScalesToVeiwBag()
		{
            GetDestinationsItemsAndScalesToVeiwBag("");
        }

        private void GetDestinationsItemsAndScalesToVeiwBag(string locationID)
        {
            SelectList destinations = new SelectList(_cdb.Locations.Where(l=>l.ID.Contains(locationID)), "ID", "LocationName");
            SelectList items = new SelectList(EquipmentProvider.GetItems(_cdb, User.Identity.Name ?? ""), "ID", "Name");
            SelectList scales = new SelectList(EquipmentProvider.GetUserAuthorizedEquipment<WagonScale>(_cdb, User.Identity.Name ?? ""), "ID", "Name");

            ViewBag.Destinations = destinations;
            ViewBag.FromDestinations = destinations;
            ViewBag.Items = items;
            ViewBag.Scales = scales;
        }
    }
}