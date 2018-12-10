using CTS_Manual_Input.Models.Common;
using CTS_Manual_Input.Models.Dictionary;
using CTS_Models;
using CTS_Models.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CTS_Manual_Input.Helpers;
using PagedList;
using System.Data.Entity.Migrations;
using System.Web;
using System.IO;
using CTS_Manual_Input.Models.SkipModels;
using CTS_Core;

namespace CTS_Manual_Input.Controllers
{
	[ErrorAttribute]
	[CtsAuthorize(Roles = Roles.DictUserRoleName)]
	public class DictionaryController : Controller
	{
		private CtsDbContext _cdb = new CtsDbContext();

		#region WagonScales Dictionary 
		public ActionResult WagonScalesIndex()
		{
			List<WagonScale> wagonScales = new List<WagonScale>();
			wagonScales = _cdb.WagonScales.ToList();

			@ViewBag.Title = "Данные словаря вагонных весов";
			return View(wagonScales);
		}

		public ActionResult WagonScalesAdd()
		{
			WagonScalesLocations model = new WagonScalesLocations();
			WagonScale wagonScale = new WagonScale();
			wagonScale.Name = "Вагонные весы";
			wagonScale.NameEng = "Wagon weighbridge";
			wagonScale.NameKZ = "Вагондық таразы";
			model.WagonScale = wagonScale;
			model.Locations = new SelectList(_cdb.Locations, "ID", "LocationName");

			@ViewBag.Title = "Добавление вагонных весов";
			return View(model);
		}

		[HttpPost]
		public ActionResult WagonScalesAdd(WagonScalesLocations model)
		{
			if (String.IsNullOrEmpty(model.WagonScale.Name))
			{
				ModelState.AddModelError("WagonScale.Name", "Введите наименование - не может быть пустым");
			}
			if (String.IsNullOrEmpty(model.WagonScale.NameEng))
			{
				ModelState.AddModelError("WagonScale.NameEng", "Введите наименование - не может быть пустым");
			}
			if (String.IsNullOrEmpty(model.WagonScale.NameKZ))
			{
				ModelState.AddModelError("WagonScale.NameKZ", "Введите наименование - не может быть пустым");
			}
			if (ModelState.IsValid)
			{
				model.WagonScale.ID = _cdb.WagonScales.Max(x => x.ID) + 1;
				_cdb.WagonScales.Add(model.WagonScale);
				_cdb.SaveChanges();

				return RedirectToAction("WagonScalesIndex");
			}

			model.Locations = new SelectList(_cdb.Locations, "ID", "LocationName");
			@ViewBag.Title = "Добавление вагонных весов";
			return View("WagonScalesAdd", model);
		}

		public ActionResult WagonScalesEdit(int Id)
		{
			if (Id == 0)
			{
				return HttpNotFound();
			}

			WagonScale wagonScale = _cdb.WagonScales.Find(Id);
			if (wagonScale != null)
			{

				@ViewBag.Title = "Редактирование вагонных весов";
				return View("WagonScalesEdit", wagonScale);
			}
			return RedirectToAction("WagonScalesIndex");
		}

		[HttpPost]
		public ActionResult WagonScalesEdit(WagonScale model)
		{
			if (String.IsNullOrEmpty(model.Name))
			{
				ModelState.AddModelError("Name", "Введите наименование - не может быть пустым");
			}
			if (String.IsNullOrEmpty(model.NameEng))
			{
				ModelState.AddModelError("NameEng", "Введите наименование - не может быть пустым");
			}
			if (String.IsNullOrEmpty(model.NameKZ))
			{
				ModelState.AddModelError("NameKZ", "Введите наименование - не может быть пустым");
			}
			if (ModelState.IsValid)
			{
				WagonScale wagonScale = _cdb.WagonScales.Find(model.ID);

				wagonScale.Name = model.Name;
				wagonScale.NameEng = model.NameEng;
				wagonScale.NameKZ = model.NameKZ;
				_cdb.Entry(wagonScale).State = EntityState.Modified;
				_cdb.SaveChanges();

				return RedirectToAction("WagonScalesIndex");
			}

			@ViewBag.Title = "Редактирование вагонных весов";
			return View("WagonScalesEdit", model);
		}
		#endregion

		#region BeltScales Dictionary 
		public ActionResult BeltScalesIndex()
		{
			List<BeltScale> beltScales = new List<BeltScale>();
			beltScales = _cdb.BeltScales.ToList();

			@ViewBag.Title = "Данные словаря конвейерных весов";
			return View(beltScales);
		}

		public ActionResult BeltScalesAdd()
		{
			BeltScalesLocations model = new BeltScalesLocations();
			BeltScale beltScale = new BeltScale();
			beltScale.Name = "Конвейерные весы";
			beltScale.NameEng = "Belt weigher";
			beltScale.NameKZ = "Конвейерлік таразы";
			model.BeltScale = beltScale;
			model.Locations = new SelectList(_cdb.Locations, "ID", "LocationName");

			@ViewBag.Title = "Добавление конвейерных весов";
			return View(model);
		}

		[HttpPost]
		public ActionResult BeltScalesAdd(BeltScalesLocations model)
		{
			var modelbadstate = false;
			if ((model.BeltScale.FromInnerDestID == null) || (model.BeltScale.ToInnerDestID == null))
			{
				modelbadstate = true;
				ViewBag.ErrorMessage = "Требуется указать начало и конец конвейера";
			}
			if (String.IsNullOrEmpty(model.BeltScale.Name))
			{
				modelbadstate = true;
				ViewBag.ErrorMessage = "Введите наименование - не может быть пустым - не может быть пустым";
				ModelState.AddModelError("BeltScale.Name", "Введите наименование - не может быть пустым - не может быть пустым");
			}
			if (String.IsNullOrEmpty(model.BeltScale.NameEng))
			{
				modelbadstate = true;
				ViewBag.ErrorMessage = "Введите наименование - не может быть пустым - не может быть пустым";
				ModelState.AddModelError("BeltScale.NameEng", "Введите наименование - не может быть пустым - не может быть пустым");
			}
			if (String.IsNullOrEmpty(model.BeltScale.NameKZ))
			{
				modelbadstate = true;
				ViewBag.ErrorMessage = "Введите наименование - не может быть пустым - не может быть пустым";
				ModelState.AddModelError("BeltScale.NameKZ", "Введите наименование - не может быть пустым - не может быть пустым");
			}
			if (modelbadstate)
			{
				Response.StatusCode = (int)HttpStatusCode.BadRequest;
				return Json(new { modelbadstate = modelbadstate, ErrorMessage = ViewBag.ErrorMessage });
			}
			if (ModelState.IsValid)
			{
				model.BeltScale.ID = _cdb.BeltScales.Max(x => x.ID) + 1;
				_cdb.BeltScales.Add(model.BeltScale);
				_cdb.SaveChanges();

				return RedirectToAction("BeltScalesIndex");
			}

			@ViewBag.Title = "Добавление конвейерных весов";
			return View("BeltScalesAdd", model);
		}

		public ActionResult BeltScalesEdit(int Id)
		{
			if (Id == 0)
			{
				return HttpNotFound();
			}

			BeltScale beltScale = _cdb.BeltScales.Find(Id);
			if (beltScale != null)
			{

				@ViewBag.Title = "Редактирование конвейерных весов";
				return View("BeltScalesEdit", beltScale);
			}
			return RedirectToAction("BeltScalesIndex");
		}

		[HttpPost]
		public ActionResult BeltScalesEdit(BeltScale model)
		{
			var modelbadstate = false;
			if ((model.FromInnerDestID == null) || (model.ToInnerDestID == null))
			{
				modelbadstate = true;
				ViewBag.ErrorMessage = "Требуется указать начало и конец конвейера";
			}
			if (String.IsNullOrEmpty(model.Name))
			{
				modelbadstate = true;
				ViewBag.ErrorMessage = "Введите наименование - не может быть пустым";
				ModelState.AddModelError("BeltScale.Name", "Введите наименование - не может быть пустым");
			}
			if (String.IsNullOrEmpty(model.NameEng))
			{
				modelbadstate = true;
				ViewBag.ErrorMessage = "Введите наименование - не может быть пустым";
				ModelState.AddModelError("BeltScale.NameEng", "Введите наименование - не может быть пустым");
			}
			if (String.IsNullOrEmpty(model.NameKZ))
			{
				modelbadstate = true;
				ViewBag.ErrorMessage = "Введите наименование - не может быть пустым";
				ModelState.AddModelError("BeltScale.NameKZ", "Введите наименование - не может быть пустым");
			}
			if (modelbadstate)
			{
				Response.StatusCode = (int)HttpStatusCode.BadRequest;
				return Json(new { modelbadstate = modelbadstate, ErrorMessage = ViewBag.ErrorMessage });
			}
			if (ModelState.IsValid)
			{
				BeltScale beltScale = _cdb.BeltScales.Find(model.ID);

				beltScale.Name = model.Name;
				beltScale.NameEng = model.NameEng;
				beltScale.NameKZ = model.NameKZ;
				_cdb.Entry(beltScale).State = EntityState.Modified;
				_cdb.SaveChanges();

				return RedirectToAction("BeltScalesIndex");
			}

			@ViewBag.Title = "Редактирование конвейерных весов";
			return View("BeltScalesEdit", model);
		}

		[OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
		public ActionResult GetFromInner(string Locations)
		{
			string[] locations = Locations.Split(',');
			var model = new InnerDestDropDownModel();
			var dests = new List<InnerDestination>();
			foreach (var l in locations)
			{
				dests.AddRange(_cdb.InnerDestinations.Where(s => s.LocationID == l));
			}

			model.InnerDests = dests.Select(N => new SelectListItem { Text = string.Concat(string.Concat(N.Name, " / "), N.Location.LocationName), Value = N.ID.ToString() }); ;

			return PartialView("_FromInnerDynDropDown", model);
		}

		[OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
		public ActionResult GetToInner(string Locations)
		{
			string[] locations = Locations.Split(',');
			var model = new InnerDestDropDownModel();
			var dests = new List<InnerDestination>();
			foreach (var l in locations)
			{
				dests.AddRange(_cdb.InnerDestinations.Where(s => s.LocationID == l));
			}

			dests.Reverse();
			model.InnerDests = dests.Select(N => new SelectListItem { Text = string.Concat(string.Concat(N.Name, " / "), N.Location.LocationName), Value = N.ID.ToString() }); ;

			return PartialView("_ToInnerDynDropDown", model);
		}
		#endregion

		#region Skip Dictionary 
		public ActionResult SkipsIndex()
		{
			List<Skip> skips = new List<Skip>();
			skips = _cdb.Skips.ToList();

			@ViewBag.Title = "Данные словаря скипов";
			return View(skips);
		}

		public ActionResult SkipsAdd()
		{
			SkipsLocations model = new SkipsLocations();
			Skip skip = new Skip();
			skip.Name = "Скиповой подъем";
			skip.NameEng = "Skip";
			skip.NameKZ = "Өткелді көтергіш";
			skip.Weight = 6;
			model.Skip = skip;
			model.Locations = new SelectList(_cdb.Locations, "ID", "LocationName");

			@ViewBag.Title = "Добавление скипов";
			return View(model);
		}

		[HttpPost]
		public ActionResult SkipsAdd(SkipsLocations model)
		{
			if (String.IsNullOrEmpty(model.Skip.Name))
			{
				ModelState.AddModelError("Skip.Name", "Введите наименование - не может быть пустым");
			}
			if (String.IsNullOrEmpty(model.Skip.NameEng))
			{
				ModelState.AddModelError("Skip.NameEng", "Введите наименование - не может быть пустым");
			}
			if (String.IsNullOrEmpty(model.Skip.NameKZ))
			{
				ModelState.AddModelError("Skip.NameKZ", "Введите наименование - не может быть пустым");
			}
			if (!(model.Skip.Weight > 0) || (model.Skip.Weight == 0))
			{
				ModelState.AddModelError("Skip.Weight", "Неверный вес - должен быть больше нуля");
			}
			if (ModelState.IsValid)
			{
				model.Skip.ID = _cdb.Skips.Max(x => x.ID) + 1;
				_cdb.Skips.Add(model.Skip);
				_cdb.SaveChanges();

				return RedirectToAction("SkipsIndex");
			}

			model.Locations = new SelectList(_cdb.Locations, "ID", "LocationName");
			@ViewBag.Title = "Добавление скипов";
			return View("SkipsAdd", model);
		}

		public ActionResult SkipsEdit(int Id)
		{
			if (Id == 0)
			{
				return HttpNotFound();
			}

			Skip skip = _cdb.Skips.Find(Id);
			if (skip != null)
			{

				@ViewBag.Title = "Редактирование скипов";
				return View("SkipsEdit", skip);
			}

			return RedirectToAction("SkipsIndex");
		}

		[HttpPost]
		public ActionResult SkipsEdit(Skip model)
		{
			if (String.IsNullOrEmpty(model.Name))
			{
				ModelState.AddModelError("Name", "Введите наименование - не может быть пустым");
			}
			if (String.IsNullOrEmpty(model.NameEng))
			{
				ModelState.AddModelError("NameEng", "Введите наименование - не может быть пустым");
			}
			if (String.IsNullOrEmpty(model.NameKZ))
			{
				ModelState.AddModelError("NameKZ", "Введите наименование - не может быть пустым");
			}
			if (ModelState.IsValid)
			{
				Skip skip = _cdb.Skips.Find(model.ID);

				skip.Name = model.Name;
				skip.NameEng = model.NameEng;
				skip.NameKZ = model.NameKZ;
				skip.Weight = model.Weight;
				_cdb.Entry(skip).State = EntityState.Modified;
				_cdb.SaveChanges();

				return RedirectToAction("SkipsIndex");
			}

			@ViewBag.Title = "Редактирование скипов";
			return View("SkipsEdit", model);
		}
		#endregion

		#region SkipWeight Dictionary

		public ActionResult SkipWeightsIndex(int page = 1)
		{
			int pagesize = 20;
			List<Skip> skips = EquipmentProvider.GetUserAuthorizedEquipment<Skip>(_cdb, User.Identity);
			List<SkipWeight> skipWeights = _cdb.SkipWeights.ToList();
			var skipWeightsWithActs = new List<SkipWeightWithAct>();
			foreach (var s in skipWeights)
			{
				skipWeightsWithActs.Add(new SkipWeightWithAct(s));
			}

			@ViewBag.Title = "Акты перевески скипов";
			return View(new SkipsSkipWeights
			{
				Skips = skips,
				SkipWeightsActs = skipWeightsWithActs.OrderByDescending(t => t.SkipWeight.LasEditDateTime).ToPagedList(page, pagesize),
			});
		}

		public ActionResult SkipWeightsAdd(int skipID, string name)
		{
			SkipWeight model = new SkipWeight();
			model.SkipID = skipID;
			model.ValidFrom = System.DateTime.Now;

			@ViewBag.Title = "Добавление акта перевески";
			return View(model);
		}

		[HttpPost]
		public ActionResult SkipWeightsAdd(SkipWeight model, HttpPostedFileBase upload)
		{
			if ((model.Weight <= 0) || (model.Weight > 25))
			{
				ModelState.AddModelError("Weight", "Вес скипа должен быть больше 0 и меньше 25 т");
				ModelState.AddModelError("StudentName", "Неправильная дата - вес скипа не может быть изменен в прошлом");
			}
			if (model.ValidFrom < System.DateTime.Today)
			{
				ModelState.AddModelError("ValidFrom", "Неправильная дата - вес скипа не может быть изменен в прошлом");
			}
			if (ModelState.IsValid)
			{
				model.OperatorName = User.Identity.Name;
				model.LasEditDateTime = System.DateTime.Now;
				var ffddf = _cdb.SkipWeights.DefaultIfEmpty();
				model.ID = _cdb.SkipWeights.DefaultIfEmpty().Max(p => p == null ? 1 : p.ID + 1);
				_cdb.SkipWeights.Add(model);
				_cdb.SaveChanges();

				if (upload != null)
				{
					string extension = Path.GetExtension(upload.FileName);
					if ((extension == ".jpeg") || (extension == ".jpg") || (extension == ".png") || (extension == ".pdf"))
					{
						upload.SaveAs(Path.Combine(CTS_Core.ProjectConstants.SkipActFolderPath, model.ID.ToString() + extension));
					}
				}

				return RedirectToAction("SkipWeightsIndex");
			}

			@ViewBag.Title = "Добавление акта перевески";
			return View(model);
		}

		public ActionResult SkipWeightsEdit(int Id)
		{
			if (Id == 0)
			{
				return HttpNotFound();
			}

			SkipWeight skipWeight = _cdb.SkipWeights.Find(Id);
			if (skipWeight != null)
			{
				@ViewBag.HasFile = !string.IsNullOrEmpty(Directory.EnumerateFiles(CTS_Core.ProjectConstants.SkipActFolderPath, skipWeight.ID + ".*").FirstOrDefault());
				@ViewBag.Title = "Редактирование акта перевески";
				return View("SkipWeightsEdit", skipWeight);
			}

			return RedirectToAction("SkipWeightsIndex");
		}

		[HttpPost]
		public ActionResult SkipWeightsEdit(SkipWeight model, HttpPostedFileBase upload)
		{
			string pathToAct = Directory.EnumerateFiles(CTS_Core.ProjectConstants.SkipActFolderPath, model.ID + ".*").FirstOrDefault();

			if ((model.Weight <= 0) || (model.Weight > 25))
			{
				ModelState.AddModelError("Weight", "Вес скипа должен быть больше 0 и меньше 25 т");
			}
			if (model.ValidFrom < System.DateTime.Today)
			{
				ModelState.AddModelError("ValidFrom", "Неправильная дата - вес скипа не может быть изменен в прошлом");
			}
			if (ModelState.IsValid)
			{
				SkipWeight skipWeight = _cdb.SkipWeights.Find(model.ID);
				skipWeight.Weight = model.Weight;
				skipWeight.ValidFrom = model.ValidFrom;
				skipWeight.OrderNo = model.OrderNo;
				skipWeight.OperatorName = User.Identity.Name;
				skipWeight.LasEditDateTime = System.DateTime.Now;
				_cdb.Entry(skipWeight).State = EntityState.Modified;
				_cdb.SaveChanges();

				if (upload != null)
				{
					string extension = Path.GetExtension(upload.FileName);
					if ((extension == ".jpeg") || (extension == ".jpg") || (extension == ".png") || (extension == ".pdf"))
					{
						if (!string.IsNullOrEmpty(pathToAct) && System.IO.File.Exists(pathToAct))
						{
							System.IO.File.Delete(pathToAct);
						}

						upload.SaveAs(Path.Combine(CTS_Core.ProjectConstants.SkipActFolderPath, model.ID.ToString() + extension));
					}
				}

				return RedirectToAction("SkipWeightsIndex");
			}

			@ViewBag.HasFile = !string.IsNullOrEmpty(pathToAct);
			@ViewBag.Title = "Редактирование акта перевески";
			return View("SkipWeightsEdit", model);
		}

		public FileResult OpenSkipAct(string filePath)
		{
			var fileStream = new FileStream(filePath,
								 FileMode.Open,
								 FileAccess.Read
							   );
			var fsResult = new FileStreamResult(fileStream, MimeMapping.GetMimeMapping(filePath));
			return fsResult;
		}
		#endregion

		#region Shifts Dictionary
		public ActionResult ShiftsIndex()
		{
			List<Shift> shifts = _cdb.Shifts.ToList();
			var locations = _cdb.Locations.Where(x => x.LocationName.StartsWith("ш")).ToList();
			var locationsShifts = new List<ShiftsConsolidated>();

			foreach (var loc in locations)
			{
				var locShifts = new ShiftsConsolidated()
				{
					LocationID = loc.ID,
					Location = loc,
					TimeStartFirstShift = shifts.Where(x => x.LocationID == loc.ID).Where(s => s.ShiftNum == 1).Select(m => m.TimeStart).SingleOrDefault(),
					TimeStartSecondShift = shifts.Where(x => x.LocationID == loc.ID).Where(s => s.ShiftNum == 2).Select(m => m.TimeStart).SingleOrDefault(),
					TimeStartThirdShift = shifts.Where(x => x.LocationID == loc.ID).Where(s => s.ShiftNum == 3).Select(m => m.TimeStart).SingleOrDefault(),
					TimeStartFourthShift = shifts.Where(x => x.LocationID == loc.ID).Where(s => s.ShiftNum == 4).Select(m => m.TimeStart).SingleOrDefault()
				};
				locationsShifts.Add(locShifts);
			}

			@ViewBag.Title = "Данные времени смен на шахтах";
			return View(locationsShifts);
		}

		public ActionResult ShiftsEdit(string Id, string Name)
		{
			if (Id == null)
			{
				return HttpNotFound();
			}

			Shift shift = _cdb.Shifts.Find(1, Id);
			if (shift != null)
			{
				shift.Location.LocationName = Name;
				@ViewBag.Name = Name;
				@ViewBag.Title = "Редактирование данных времени смен на шахтах";
				return View("ShiftsEdit", shift);
			}

			return RedirectToAction("SkipWeightsIndex");
		}

		[HttpPost]
		public ActionResult ShiftsEdit(Shift model, string Name)
		{
			if (ModelState.IsValid)
			{
				List<Shift> shifts = _cdb.Shifts.ToList();
				var shift1 = shifts.Where(x => x.LocationID == model.LocationID).Where(s => s.ShiftNum == 1).FirstOrDefault();
				var shift2 = shifts.Where(x => x.LocationID == model.LocationID).Where(s => s.ShiftNum == 2).FirstOrDefault();
				var shift3 = shifts.Where(x => x.LocationID == model.LocationID).Where(s => s.ShiftNum == 3).FirstOrDefault();
				var shift4 = shifts.Where(x => x.LocationID == model.LocationID).Where(s => s.ShiftNum == 4).FirstOrDefault();
				var shiftDuration = new TimeSpan(6, 0, 0);
				shift1.TimeStart = new TimeSpan(model.TimeStart.Hours, 0, 0);
				shift2.TimeStart = new TimeSpan(model.TimeStart.Add(shiftDuration).Hours, 0, 0);
				shift3.TimeStart = new TimeSpan(model.TimeStart.Add(shiftDuration).Add(shiftDuration).Hours, 0, 0);
				shift4.TimeStart = new TimeSpan(model.TimeStart.Add(shiftDuration).Add(shiftDuration).Add(shiftDuration).Hours, 0, 0);

				_cdb.Entry(shift1).State = EntityState.Modified;
				_cdb.Entry(shift2).State = EntityState.Modified;
				_cdb.Entry(shift3).State = EntityState.Modified;
				_cdb.Entry(shift4).State = EntityState.Modified;
				_cdb.SaveChanges();

				return RedirectToAction("ShiftsIndex");
			}

			@ViewBag.Name = Name;
			@ViewBag.Title = "Редактирование данных времени смен на шахтах";
			return View("ShiftsEdit", model);
		}
		#endregion
	}
}