using CTS_Analytics.Filters;
using CTS_Analytics.Models;
using CTS_Analytics.Models.Mnemonic;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using System;
using System.Data.Entity;
using System.Globalization;
using System.Collections.Generic;
using PagedList;
using CTS_Models;
using CTS_Models.DBContext;
using System.IO;
using CTS_Core;
using CTS_Analytics.Factories;
using CTS_Analytics.Services;

namespace CTS_Analytics.Controllers
{
    [Culture]
    [Error]
	[CtsAuthorize(Roles = Roles.AnalyticsRoleName)]
	public partial class MnemonicController : Controller
    {
        private readonly CentralDBService _cdbService;

        public MnemonicController()
        {
            this._cdbService = new CentralDBService();
        }

        class WagonNumDate
        {
            public string WagonNum { get; set; }
            public DateTime Date { get; set; }
            public bool ManualFlag { get; set; }
            public int IdSostav { get; set; }
        }

        private string getUserLang(HttpCookie cookie)
        {
            string lang = "";

            if (cookie != null)
                lang = cookie.Value;
            else
                lang = "ru";

            return lang;
        }

        public ActionResult HseAlarm()
        {
            return View();
        }

        #region MineMethods

        public ActionResult shah()
        {
            var model = new shahModel();
            var factory = new MnemonicModelBuilder(getUserLang(Request.Cookies["lang"]));
            var fromDate = GetDateFromCookie("fromdate");
            var toDate = GetDateFromCookie("todate");
            model.DetailsViewName = "doc_detail_shah";
                factory.GetGeneralData(model, fromDate, toDate);
                model.Skip = factory.GetSkipModel(4, fromDate, toDate);
                model.Belt = factory.GetBeltScaleModel(5, fromDate, toDate);
                model.Vagon = factory.GetWagonScaleModel(7, fromDate, toDate);
                model.Sklad = factory.GetWarehouseModel(5, fromDate, toDate);
                model.RockUtil = factory.GetRockUtilModel(6, fromDate, toDate);
                model.Kotel = GetMineKotelModel("shah");
            return View(model);
        }

        public ActionResult sar1()
        {
            var model = new sar1Model();
            var factory = new MnemonicModelBuilder(getUserLang(Request.Cookies["lang"]));
            var fromDate = GetDateFromCookie("fromdate");
            var toDate = GetDateFromCookie("todate");
            model.DetailsViewName = "doc_detail_sar";
                factory.GetGeneralData(model, fromDate, toDate);
                model.Skip1 = factory.GetSkipModel(7, fromDate, toDate);
                model.Skip2 = factory.GetSkipModel(8, fromDate, toDate);
                model.BeltToVagon1 = factory.GetBeltScaleModel(11, fromDate, toDate);
                model.BeltToVagon2 = factory.GetBeltScaleModel(12, fromDate, toDate);
                model.BeltToBoiler = factory.GetBeltScaleModel(13, fromDate, toDate);
                model.Vagon = factory.GetWagonScaleModel(4, fromDate, toDate);
                model.Raspozn = GetRaspoznModel(9, fromDate, toDate, 5);
                model.Sklad = factory.GetWarehouseModel(6, fromDate, toDate);
                model.RockUtil = factory.GetRockUtilModel(9, fromDate, toDate); 
            model.Kotel = GetMineKotelModel("sar1");
            return View(model);
        }

        public ActionResult abay()
        {
            var model = new abayModel();
            var factory = new MnemonicModelBuilder(getUserLang(Request.Cookies["lang"]));
            var fromDate = GetDateFromCookie("fromdate");
            var toDate = GetDateFromCookie("todate");
            model.DetailsViewName = "doc_detail_abay";
                factory.GetGeneralData(model, fromDate, toDate);
                model.Skip1 = factory.GetSkipModel(17, fromDate, toDate);
                model.Skip2 = factory.GetSkipModel(18, fromDate, toDate);
                model.BeltPos1 = factory.GetBeltScaleModel(23, fromDate, toDate);
                model.BeltPos9 = factory.GetBeltScaleModel(22, fromDate, toDate);
                model.BeltBoiler = factory.GetBeltScaleModel(24, fromDate, toDate);
                model.Vagon = factory.GetWagonScaleModel(10, fromDate, toDate);
                model.Raspozn = GetRaspoznModel(1, fromDate, toDate, 5);
                model.Sklad = factory.GetWarehouseModel(4, fromDate, toDate);
                model.RockUtil = factory.GetRockUtilModel(5, fromDate, toDate);
            model.Kotel = GetMineKotelModel("abay");
            return View(model);
        }

        public ActionResult kost()
        {
            var model = new kostModel();
            var factory = new MnemonicModelBuilder(getUserLang(Request.Cookies["lang"]));
            var fromDate = GetDateFromCookie("fromdate");
            var toDate = GetDateFromCookie("todate");
            model.DetailsViewName = "doc_detail_kost";
                factory.GetGeneralData(model, fromDate, toDate);
                model.Skip6t = factory.GetSkipModel(5, fromDate, toDate);
                model.Skip9t = factory.GetSkipModel(6, fromDate, toDate);
                model.BeltFromSkip6t = factory.GetBeltScaleModel(6, fromDate, toDate);
                model.BeltFromSkip9t = factory.GetBeltScaleModel(7, fromDate, toDate);
                model.BeltToSklad = factory.GetBeltScaleModel(8, fromDate, toDate);
                model.BeltToVagon = factory.GetBeltScaleModel(9, fromDate, toDate);
                model.BeltToBoiler = factory.GetBeltScaleModel(10, fromDate, toDate);
                model.Vagon = factory.GetWagonScaleModel(2, fromDate, toDate);
                model.Sklad = factory.GetWarehouseModel(3, fromDate, toDate);
                model.Raspozn1 = GetRaspoznModel(4, fromDate, toDate, 5);
                model.Raspozn2 = GetRaspoznModel(4, fromDate, toDate, 5);
                model.RockUtil38 = factory.GetRockUtilModel(3, fromDate, toDate);
                model.RockUtil39 = factory.GetRockUtilModel(3, fromDate, toDate);
            model.Kotel = GetMineKotelModel("kost");
            return View(model);
        }

        public ActionResult kuz()
        {
            var model = new kuzModel();
            var factory = new MnemonicModelBuilder(getUserLang(Request.Cookies["lang"]));
            var fromDate = GetDateFromCookie("fromdate");
            var toDate = GetDateFromCookie("todate");
            model.DetailsViewName = "doc_detail_kuz";
                factory.GetGeneralData(model, fromDate, toDate);
                model.SkipPos1 = factory.GetSkipModel(3, fromDate, toDate);
                model.SkipPos2 = factory.GetSkipModel(2, fromDate, toDate);
                model.BeltPos44 = factory.GetBeltScaleModel(2, fromDate, toDate);
                model.BeltPos1L80_1 = factory.GetBeltScaleModel(3, fromDate, toDate);
                model.BeltPos1L80_2 = factory.GetBeltScaleModel(4, fromDate, toDate);
                model.Vagon = factory.GetWagonScaleModel(3, fromDate, toDate);
                model.Sklad = factory.GetWarehouseModel(2, fromDate, toDate);
                model.Raspozn = GetRaspoznModel(5, fromDate, toDate, 5);
                model.RockUtil = factory.GetRockUtilModel(2, fromDate, toDate);
            model.Kotel = GetMineKotelModel("kuz");
            return View(model);
        }

        public ActionResult tent()
        {
            var model = new tentModel();
            var factory = new MnemonicModelBuilder(getUserLang(Request.Cookies["lang"]));
            var fromDate = GetDateFromCookie("fromdate");
            var toDate = GetDateFromCookie("todate");
            model.DetailsViewName = "doc_detail_tent";
                factory.GetGeneralData(model, fromDate, toDate);
                model.SkipPos1 = factory.GetSkipModel(11, fromDate, toDate);
                model.SkipPos2 = factory.GetSkipModel(12, fromDate, toDate);
                model.BeltToTechComplex = factory.GetBeltScaleModel(21, fromDate, toDate);
                model.Vagon = factory.GetWagonScaleModel(6, fromDate, toDate);
                model.Sklad = factory.GetWarehouseModel(9, fromDate, toDate);

            model.Kotel = GetMineKotelModel("tent");
            return View(model);
        }

        public ActionResult kaz()
        {
            var model = new kazModel();
            var factory = new MnemonicModelBuilder(getUserLang(Request.Cookies["lang"]));
            var fromDate = GetDateFromCookie("fromdate");
            var toDate = GetDateFromCookie("todate");
            model.DetailsViewName = "doc_detail_kaz";
                factory.GetGeneralData(model, fromDate, toDate);
                model.SkipPos1 = factory.GetSkipModel(15, fromDate, toDate);
                model.SkipPos2 = factory.GetSkipModel(16, fromDate, toDate);
                model.Belt1 = factory.GetBeltScaleModel(18, fromDate, toDate);
                model.Belt2 = factory.GetBeltScaleModel(19, fromDate, toDate);
                model.Vagon = factory.GetWagonScaleModel(9, fromDate, toDate);
                model.Raspozn = GetRaspoznModel(3, fromDate, toDate, 5);
                model.RockUtil = factory.GetRockUtilModel(4, fromDate, toDate);

            model.Kotel = GetMineKotelModel("kaz");
            return View(model);
        }

        public ActionResult sar3()
        {
            var model = new sar3Model();
            var factory = new MnemonicModelBuilder(getUserLang(Request.Cookies["lang"]));
            var fromDate = GetDateFromCookie("fromdate");
            var toDate = GetDateFromCookie("todate");
            model.DetailsViewName = "doc_detail_sar";
                factory.GetGeneralData(model, fromDate, toDate);
                model.Skip = factory.GetSkipModel(10, fromDate, toDate);
                model.BeltToTech1 = factory.GetBeltScaleModel(14, fromDate, toDate);
                model.BeltToTech2 = factory.GetBeltScaleModel(15, fromDate, toDate);
                model.VagonObogatitel = factory.GetWagonScaleModel(5, fromDate, toDate);
                model.VagonSaburkhan = factory.GetWagonScaleModel(5, fromDate, toDate);
                model.Raspozn1 = GetRaspoznModel(8, fromDate, toDate, 5);
                model.Raspozn2 = GetRaspoznModel(10, fromDate, toDate, 5);
                model.Sklad = factory.GetWarehouseModel(7, fromDate, toDate);
                model.RockUtil = factory.GetRockUtilModel(10, fromDate, toDate);

            model.Kotel = GetMineKotelModel("sar3");
            return View(model);
        }

        public ActionResult len()
        {
            var model = new lenModel();
            var factory = new MnemonicModelBuilder(getUserLang(Request.Cookies["lang"]));
            var fromDate = GetDateFromCookie("fromdate");
            var toDate = GetDateFromCookie("todate");
            model.DetailsViewName = "doc_detail_len";
                factory.GetGeneralData(model, fromDate, toDate);
                model.SkipPos1 = factory.GetSkipModel(13, fromDate, toDate);
                model.SkipPos2 = factory.GetSkipModel(14, fromDate, toDate);
                model.Belt1 = factory.GetBeltScaleModel(16, fromDate, toDate);
                model.Belt2 = factory.GetBeltScaleModel(17, fromDate, toDate);
                model.Vagon = factory.GetWagonScaleModel(8, fromDate, toDate);
                model.Raspozn = GetRaspoznModel(6, fromDate, toDate, 5);
                model.Sklad = factory.GetWarehouseModel(8, fromDate, toDate);
                model.RockUtil = factory.GetRockUtilModel(7, fromDate, toDate);
            model.Kotel = GetMineKotelModel("len");
            return View(model);
        }

        #endregion

        #region Mine3rdLevel

        public ActionResult Mine_skip(string ID, int skipID, int? page, bool filterManualInput = false, bool orderByTransferTimeStampAsc = false)
        {
            var model = GetSkipModel(skipID);
            model.FilterManualInput = filterManualInput;
            if (filterManualInput)
            {
                model.SkipTransfers = model.SkipTransfers.Where(v => v.OperatorName != ProjectConstants.SystemPlarformOperatorName).ToList();
            }

            model.OrderByTransferTimeStampAsc = orderByTransferTimeStampAsc;
            if (orderByTransferTimeStampAsc)
            {
                model.SkipTransfers = model.SkipTransfers.OrderBy(t => t.TransferTimeStamp).ToList();
            }

            model.ReturnID = ID;
            model.SkipID = skipID;
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            model.PagedkipTransfers = model.SkipTransfers.ToPagedList(pageNumber, pageSize);
            return View(model);
        }

        public ActionResult Mine_konv(string ID, int beltScaleID, int? page, bool filterManualInput = false, bool orderByTransferTimeStampAsc = false)
        {
            var model = GetBeltScaleModel(beltScaleID);
            model.FilterManualInput = filterManualInput;
            if (filterManualInput)
            {
                model.BeltTransfers = model.BeltTransfers.Where(v => v.OperatorName != ProjectConstants.SystemPlarformOperatorName).ToList();
            }

            model.OrderByTransferTimeStampAsc = orderByTransferTimeStampAsc;
            if (orderByTransferTimeStampAsc)
            {
                model.BeltTransfers = model.BeltTransfers.OrderBy(t => t.TransferTimeStamp).ToList();
            }

            model.ReturnID = ID;
            model.BeltID = beltScaleID;
            int pageSize = 24;
            int pageNumber = (page ?? 1);
            model.PagedBeltTransfers = model.BeltTransfers.ToPagedList(pageNumber, pageSize);
            return View(model);
        }

        public ActionResult Mine_sklad(string ID, int warehouseID, int? page)
        {
            var model = GetWarehouseModel(warehouseID);
            model.ReturnID = ID;
            model.WarehouseID = warehouseID;
            int pageSize = 24;
            int pageNumber = (page ?? 1);
            model.PagedWarehouseTransfers = model.WarehouseTransfers.ToPagedList(pageNumber, pageSize);
            return View(model);
        }

        public ActionResult Mine_vagon(string ID, int wagonScaleID, int? page, bool filterManualInput = false, bool orderByTransferTimeStampAsc = false, string wagonNumberFilter = "")
        {
            var model = GetWagonScaleModel(wagonScaleID);

            model.FilterManualInput = filterManualInput;
            if (filterManualInput)
            {
                model.WagonTransfers = model.WagonTransfers.Where(v => v.OperatorName != ProjectConstants.DbSyncOperatorName).ToList();
            }

            model.OrderByTransferTimeStampAsc = orderByTransferTimeStampAsc;
            if (orderByTransferTimeStampAsc)
            {
                model.WagonTransfers = model.WagonTransfers.OrderBy(t=>t.TransferTimeStamp).ToList();
            }

            model.WagonNumberFilter = wagonNumberFilter;
            if (!string.IsNullOrEmpty(wagonNumberFilter))
            {
                model.WagonTransfers = model.WagonTransfers.Where(w => w.SublotName.Contains(wagonNumberFilter)).ToList();
            }

            model.PagedWagonTrasnfersAndPhotos = GetPagedWagonTransfersAndPhotos(page, model.WagonTransfers);

            return View(model);
        }

        public ActionResult Mine_raspozn(int raspoznID, int? page, string wagonNumberFilter="")
        {
            var fromDate = GetDateFromCookie("fromdate");
            var toDate = GetDateFromCookie("todate");
            string locationID = "test";
            using (var wagdb = new WagonDBcontext())
            {
                locationID = wagdb.recogn.Where(w => w.id == raspoznID).First().name;
            }

            int pageSize = 24;
            int pageNumber = (page ?? 1);
            var model = new Mine_raspozn(locationID);
                model.RaspoznTable = GetRaspoznModel(raspoznID, fromDate, toDate,pageSize,(pageNumber-1)*pageSize);
            if (!string.IsNullOrEmpty(wagonNumberFilter))
            {
                model.RaspoznTable.RaspoznList = model.RaspoznTable.RaspoznList.Where(w => w.WagonNumber.Contains(wagonNumberFilter)).ToList();
            };

            model.PagedRaspoznTable = model.RaspoznTable.RaspoznList.ToPagedList(pageNumber, pageSize);
            model.RaspoznID = raspoznID;
            model.LastTrainDateTime = model.RaspoznTable.RaspoznList.FirstOrDefault()?.Date;
            model.WagonsPassed = model.RaspoznTable.RaspoznList.Count();
            model.LastTrainWagonCount = model.RaspoznTable.RaspoznList.GroupBy(g => g.IdSostav).FirstOrDefault()?.Count() ?? 0;
            model.MineName = GetLocationNameOnCurrentLanguate(locationID);
            model.WagonNumberFilter = wagonNumberFilter;
            

            return View(model);
        }

        #endregion

        public string GetLocationNameOnCurrentLanguate(string locationID)
        {
            using (var db = new CtsDbContext())
            {
                var location = db.Locations.Find(locationID);
                var name = location.LocationName;
                if (getUserLang(Request.Cookies["lang"]) == "en")
                {
                    name = location.LocationNameEng;
                }

                if (getUserLang(Request.Cookies["lang"]) == "kk")
                {
                    name = location.LocationNameKZ;
                }
                return name;
            }
        }

        #region GetMineEquipmentMethods

        private Mine_skip GetSkipModel(int skipID)
        {
            var fromDate = GetDateFromCookie("fromdate");
            var toDate = GetDateFromCookie("todate");
            var factory = new MnemonicModelBuilder(getUserLang(Request.Cookies["lang"]));
            return factory.GetSkipModel(skipID, fromDate, toDate); ;

        }

        private Mine_konv GetBeltScaleModel(int beltID)
        {
            var fromDate = GetDateFromCookie("fromdate");
            var toDate = GetDateFromCookie("todate");
            var factory = new MnemonicModelBuilder(getUserLang(Request.Cookies["lang"]));
                return factory.GetBeltScaleModel(beltID, fromDate, toDate);
        }

        private Mine_vagon GetWagonScaleModel(int wagonScaleID, int pageSize = 20, int page = 1)
        {
            var fromDate = GetDateFromCookie("fromdate");
            var toDate = GetDateFromCookie("todate");
            var factory = new MnemonicModelBuilder(getUserLang(Request.Cookies["lang"]));
                return factory.GetWagonScaleModel(wagonScaleID, fromDate, toDate);

        }

        private Mine_sklad GetWarehouseModel(int warehouseID)
        {
            var fromDate = GetDateFromCookie("fromdate");
            var toDate = GetDateFromCookie("todate");
            var factory = new MnemonicModelBuilder(getUserLang(Request.Cookies["lang"]));
                return factory.GetWarehouseModel(warehouseID, fromDate, toDate); ;
        }

        #endregion

        private IEnumerable<WagonTransfer> GetDataFromWagonDB(DateTime fromDate, DateTime toDate, string locationID)
        {
            using (var wagdb = new WagonDBcontext())
            {
                var vagons = wagdb.ves_vagon.Where(v => v.scales.objects.name == locationID).
                    Where(t => t.date_time_brutto >= fromDate && t.date_time_brutto <= toDate).
                    Select(s => new WagonTransfer()
                    {
                        ID = s.id.ToString(),
                        Brutto = s.ves_brutto / 1000,
                        SublotName = s.vagon_num,
                        LotName = s.nakladn,
                        Tare = s.ves_tara / 1000,
                        TransferTimeStamp = s.date_time_brutto,
                        ToDest = s.poluch.display_name,
                        FromDestID = s.otpravl.name,
                        IsValid = true,
                    }
                    ).ToArray();
                return vagons.OrderByDescending(t => t.TransferTimeStamp);
            }
        }

        private DateTime GetDateFromCookie(string cookieName)
        {
            HttpCookie dateCoookie = Request.Cookies[cookieName];
            DateTime returnDate = new DateTime();
            if (dateCoookie != null)
                return DateTime.ParseExact(HttpUtility.UrlDecode(dateCoookie.Value), "ddd MMM dd yyyy HH:mm:ss 'GMT'K", CultureInfo.InvariantCulture);
            else if (cookieName != "todate")
                returnDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            else
                returnDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddMinutes(-1);

            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.HttpOnly = false;
            cookie.Value = returnDate.AddHours(DateTimeOffset.Now.Offset.Hours*-1).ToString("ddd MMM dd yyyy HH:mm:ss 'GMT'K" , CultureInfo.InvariantCulture);
            cookie.Expires = DateTime.Now.AddYears(1);
            Response.Cookies.Add(cookie);
            return returnDate;
        }

        private RaspoznItem GetWagonPhoto(string wagonNumber, DateTime recognDateTime, bool manualFlag = false)
        {
            using (var wagdb = new WagonDBcontext())
            {
                var timeFrom = recognDateTime.AddSeconds(-2);
                var timeTo = recognDateTime.AddSeconds(2);
                var photosDb = wagdb.vagon_nums
                    .Where(t => t.date_time >= timeFrom & t.date_time <= timeTo)
                    .Where(v => ((v.number == wagonNumber) || (v.number_operator == wagonNumber)))
                    .ToList();
                var photos = photosDb
                    .Select(s => s.img != null ? string.Format("data:image/jpg;base64,{0}", System.Convert.ToBase64String(s.img)) : null)
                    .ToList();
                photos.AddRange(photosDb
                    .Select(s => s.img2 != null ? string.Format("data:image/jpg;base64,{0}", System.Convert.ToBase64String(s.img2)) : null));

                if (photos == null || !photos.Any())
                {
                    return new RaspoznItem();
                }

                var item = new RaspoznItem()
                {
                    Date = recognDateTime,
                    WagonNumber = wagonNumber,
                    GallleryName = "G" + wagonNumber + (int)recognDateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds,
                    PictureGallery = photos.Where(p => !string.IsNullOrEmpty(p)).ToList(),
                    ManualRecogn = manualFlag,
                };
                return item;
            }
        }

        private RaspoznItem GetMockWagonPhoto(string wagonNumber)
        {
            using (var fileReader = new FileStream(Server.MapPath("~/Content/Mnemo/pic/mockWagonNumber.jpg"), FileMode.Open, FileAccess.Read))
            {
                var item = new RaspoznItem()
                {
                    Date = System.DateTime.Now,
                    WagonNumber = wagonNumber,
                    GallleryName = "Gallery1",
                    PictureGallery = new List<string>()
                };
                try
                {
                    byte[] bytes = new byte[fileReader.Length];
                    int numBytesToRead = (int)fileReader.Length;
                    int n = fileReader.Read(bytes, 0, numBytesToRead);
                    item.PictureGallery.Add(string.Format("data:image/jpg;base64,{0}", System.Convert.ToBase64String(bytes)));
                }
                catch (Exception)
                {
                    item.PictureGallery.Add(string.Empty);
                }
                return item;
            }
        }

        private RaspoznModel GetFakeRaspoznModel(string MineID)
        {
            return new RaspoznModel()
            {
                RaspoznList = new List<RaspoznItem>()
                {
                   GetMockWagonPhoto("1111 1234"),
                   GetMockWagonPhoto("2222 1234"),
                   GetMockWagonPhoto("3333 1234"),
                   GetMockWagonPhoto("4444 1234"),
                   GetMockWagonPhoto("5555 1234")
                }
            };
        }

        private RaspoznModel GetRaspoznModel(int recognID, DateTime fromDate, DateTime toDate, int takeCount = int.MaxValue, int offset = 0)
        {
            var model = new RaspoznModel();
            var dbvagonNums = GetWagonNumsAndDates(recognID, fromDate, toDate);
            model.RaspoznID = recognID;
            model.RaspoznList = dbvagonNums.Select(s => new RaspoznItem
            {
                Date = s.Date,
                WagonNumber = s.WagonNum,
                ManualRecogn = s.ManualFlag,
                IdSostav = s.IdSostav
            })
            .ToList();
            for (int i = 0; i < model.RaspoznList.Count(); i++)
            {
                if (i>= offset && i<= offset+takeCount)
                {
                    var idSostav = model.RaspoznList[i].IdSostav; // this is PAIN
                    model.RaspoznList[i] = GetWagonPhoto(model.RaspoznList[i].WagonNumber, model.RaspoznList[i].Date, model.RaspoznList[i].ManualRecogn);
                    model.RaspoznList[i].IdSostav = idSostav;
                }
            }

            return model;
        }

        private List<WagonNumDate> GetWagonNumsAndDates(int recognID, DateTime fromDate, DateTime toDate)
        {
            using (var wagdb = new WagonDBcontext())
            {
                var dbvagonNums = wagdb.vagon_nums
                    .Where(d => d.date_time >= fromDate && d.date_time <= toDate)
                    .Where(f => !string.IsNullOrEmpty(f.number) || !string.IsNullOrEmpty(f.number_operator))
                    .Where(i => i.recognid == recognID)
                    .Select(s => new WagonNumDate
                    {
                        WagonNum = s.number != null ? s.number :
                        s.number_operator != null ? s.number_operator : "0",
                        Date = s.date_time,
                        IdSostav = s.id_sostav ?? 0
                    })
                     .OrderByDescending(d => d.Date)
                    .ToList();

                return dbvagonNums;
            }
        }

        private StaticPagedList<WagonTransfersAndPhoto> GetPagedWagonTransfersAndPhotos(int? page, List<WagonTransfer> wagonTranfers)
        {
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            var elementCount = wagonTranfers.Count;
            var transfersAndPictures = wagonTranfers
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(wt => new WagonTransfersAndPhoto()
                {
                    Photo = GetWagonPhoto(wt.SublotName, wt.TransferTimeStamp),
                    WagonTransfer = wt
                })
                .ToList();

           return new StaticPagedList<WagonTransfersAndPhoto>(transfersAndPictures, pageNumber, pageSize, elementCount);
        }
    }
}