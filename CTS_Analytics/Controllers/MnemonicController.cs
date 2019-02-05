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
	public partial class MnemonicController : CtsAnalController
    {
        private readonly CentralDBService _cdbService;
        private readonly WagonDbService _wagDbService;
        private MnemonicModelBuilder _builder;
        private MnemonicModelBuilder Builder
        {
            get
            {
                if (_builder == null) 
                    _builder = new MnemonicModelBuilder(getUserLang(Request.Cookies["lang"]), GetDateFromCookie("fromdate"), GetDateFromCookie("todate"));

                return _builder;
            }
        }

        public MnemonicController()
        {
            this._cdbService = new CentralDBService();
            this._wagDbService = new WagonDbService();
        }

        public class WagonNumDate
        {
            public string WagonNum { get; set; }
            public DateTime Date { get; set; }
            public bool ManualFlag { get; set; }
            public int IdSostav { get; set; }
        }

        public ActionResult HseAlarm()
        {
            return View();
        }

        public ActionResult Home()
        {
            return View();
        }

        #region MineMethods

        [CtsAuthorize(Roles = Roles.MineShahRoleName)]
        public ActionResult shah()
        {
            var model = new shahModel();

            model.DetailsViewName = "doc_detail_shah";
                Builder.GetGeneralData(model);
                model.Skip = Builder.GetSkipModel(4);
                model.Belt = Builder.GetBeltScaleModel(5);
                model.Vagon = Builder.GetWagonScaleModel(7);
                model.Sklad = Builder.GetWarehouseModel(5);
                model.RockUtil = Builder.GetRockUtilModel(6);
                model.Kotel = GetMineKotelModel("shah");
            return View(model);
        }

        [CtsAuthorize(Roles = Roles.MineSar1RoleName)]
        public ActionResult sar1()
        {
            var model = new sar1Model();
            model.DetailsViewName = "doc_detail_sar";
                Builder.GetGeneralData(model);
                model.Skip1 = Builder.GetSkipModel(7);
                model.Skip2 = Builder.GetSkipModel(8);
                model.BeltToVagon1 = Builder.GetBeltScaleModel(11);
                model.BeltToVagon2 = Builder.GetBeltScaleModel(12);
                model.BeltToBoiler = Builder.GetBeltScaleModel(13);
                model.Vagon = Builder.GetWagonScaleModel(4);
                model.Raspozn = Builder.GetRaspoznModel(9, 5);
                model.Sklad = Builder.GetWarehouseModel(6);
                model.RockUtil = Builder.GetRockUtilModel(9); 
            model.Kotel = GetMineKotelModel("sar1");
            return View(model);
        }

        [CtsAuthorize(Roles = Roles.MineAbayRoleName)]
        public ActionResult abay()
        {
            var model = new abayModel();
            model.DetailsViewName = "doc_detail_abay";
                Builder.GetGeneralData(model);
                model.Skip1 = Builder.GetSkipModel(17);
                model.Skip2 = Builder.GetSkipModel(18);
                model.BeltPos1 = Builder.GetBeltScaleModel(23);
                model.BeltPos9 = Builder.GetBeltScaleModel(22);
                model.BeltBoiler = Builder.GetBeltScaleModel(24);
                model.Vagon = Builder.GetWagonScaleModel(10);
                model.Raspozn = Builder.GetRaspoznModel(1, 5);
                model.Sklad = Builder.GetWarehouseModel(4);
                model.RockUtil = Builder.GetRockUtilModel(5);
            model.Kotel = GetMineKotelModel("abay");
            return View(model);
        }

        [CtsAuthorize(Roles = Roles.MineKostRoleName)]
        public ActionResult kost()
        {
            var model = new kostModel();
            model.DetailsViewName = "doc_detail_kost";
                Builder.GetGeneralData(model);
                model.Skip6t = Builder.GetSkipModel(5);
                model.Skip9t = Builder.GetSkipModel(6);
                model.BeltFromSkip6t = Builder.GetBeltScaleModel(6);
                model.BeltFromSkip9t = Builder.GetBeltScaleModel(7);
                model.BeltToSklad = Builder.GetBeltScaleModel(8);
                model.BeltToVagon = Builder.GetBeltScaleModel(9);
                model.BeltToBoiler = Builder.GetBeltScaleModel(10);
                model.Vagon = Builder.GetWagonScaleModel(2);
                model.Sklad = Builder.GetWarehouseModel(3);
                model.Raspozn1 = Builder.GetRaspoznModel(4, 5);
                model.Raspozn2 = Builder.GetRaspoznModel(4, 5);
                model.RockUtil38 = Builder.GetRockUtilModel(3);
                model.RockUtil39 = Builder.GetRockUtilModel(3);
            model.Kotel = GetMineKotelModel("kost");
            return View(model);
        }

        [CtsAuthorize(Roles = Roles.MineKuzRoleName)]
        public ActionResult kuz()
        {
            var model = new kuzModel();
            model.DetailsViewName = "doc_detail_kuz";
                Builder.GetGeneralData(model);
                model.SkipPos1 = Builder.GetSkipModel(3);
                model.SkipPos2 = Builder.GetSkipModel(2);
                model.BeltPos44 = Builder.GetBeltScaleModel(2);
                model.BeltPos1L80_1 = Builder.GetBeltScaleModel(3);
                model.BeltPos1L80_2 = Builder.GetBeltScaleModel(4);
                model.Vagon = Builder.GetWagonScaleModel(3);
                model.Sklad = Builder.GetWarehouseModel(2);
                model.Raspozn = Builder.GetRaspoznModel(5, 5);
                model.RockUtil = Builder.GetRockUtilModel(2);
            model.Kotel = GetMineKotelModel("kuz");
            return View(model);
        }

        [CtsAuthorize(Roles = Roles.MineTentRoleName)]
        public ActionResult tent()
        {
            var model = new tentModel();
            model.DetailsViewName = "doc_detail_tent";
                Builder.GetGeneralData(model);
                model.SkipPos1 = Builder.GetSkipModel(11);
                model.SkipPos2 = Builder.GetSkipModel(12);
                model.BeltToTechComplex = Builder.GetBeltScaleModel(21);
                model.Vagon = Builder.GetWagonScaleModel(6);
                model.Sklad = Builder.GetWarehouseModel(9);

            model.Kotel = GetMineKotelModel("tent");
            return View(model);
        }

        [CtsAuthorize(Roles = Roles.MineKazRoleName)]
        public ActionResult kaz()
        {
            var model = new kazModel();
            model.DetailsViewName = "doc_detail_kaz";
                Builder.GetGeneralData(model);
                model.SkipPos1 = Builder.GetSkipModel(15);
                model.SkipPos2 = Builder.GetSkipModel(16);
                model.Belt1 = Builder.GetBeltScaleModel(18);
                model.Belt2 = Builder.GetBeltScaleModel(19);
                model.Vagon = Builder.GetWagonScaleModel(9);
                model.Raspozn = Builder.GetRaspoznModel(3, 5);
                model.RockUtil = Builder.GetRockUtilModel(4);

            model.Kotel = GetMineKotelModel("kaz");
            return View(model);
        }

        [CtsAuthorize(Roles = Roles.MineSar3RoleName)]
        public ActionResult sar3()
        {
            var model = new sar3Model();
            model.DetailsViewName = "doc_detail_sar";
                Builder.GetGeneralData(model);
                model.Skip = Builder.GetSkipModel(10);
                model.BeltToTech1 = Builder.GetBeltScaleModel(14);
                model.BeltToTech2 = Builder.GetBeltScaleModel(15);
                model.VagonObogatitel = Builder.GetWagonScaleModel(5);
                model.VagonSaburkhan = Builder.GetWagonScaleModel(5);
                model.Raspozn1 = Builder.GetRaspoznModel(8, 5);
                model.Raspozn2 = Builder.GetRaspoznModel(10, 5);
                model.Sklad = Builder.GetWarehouseModel(7);
                model.RockUtil = Builder.GetRockUtilModel(10);

            model.Kotel = GetMineKotelModel("sar3");
            return View(model);
        }

        [CtsAuthorize(Roles = Roles.MineLenRoleName)]
        public ActionResult len()
        {
            var model = new lenModel();
            model.DetailsViewName = "doc_detail_len";
                Builder.GetGeneralData(model);
                model.SkipPos1 = Builder.GetSkipModel(13);
                model.SkipPos2 = Builder.GetSkipModel(14);
                model.Belt1 = Builder.GetBeltScaleModel(16);
                model.Belt2 = Builder.GetBeltScaleModel(17);
                model.Vagon = Builder.GetWagonScaleModel(8);
                model.Raspozn = Builder.GetRaspoznModel(6, 5);
                model.Sklad = Builder.GetWarehouseModel(8);
                model.RockUtil = Builder.GetRockUtilModel(7);
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
                model.RaspoznTable = Builder.GetRaspoznModel(raspoznID,pageSize,(pageNumber-1)*pageSize);
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
            return Builder.GetSkipModel(skipID); ;

        }

        private Mine_konv GetBeltScaleModel(int beltID)
        {
                return Builder.GetBeltScaleModel(beltID);
        }

        private Mine_vagon GetWagonScaleModel(int wagonScaleID, int pageSize = 20, int page = 1)
        {
                return Builder.GetWagonScaleModel(wagonScaleID);

        }

        private Mine_sklad GetWarehouseModel(int warehouseID)
        {
                return Builder.GetWarehouseModel(warehouseID);
        }

        #endregion

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
                    Photo = _wagDbService.GetWagonPhoto(wt.SublotName, wt.TransferTimeStamp),
                    WagonTransfer = wt
                })
                .ToList();

           return new StaticPagedList<WagonTransfersAndPhoto>(transfersAndPictures, pageNumber, pageSize, elementCount);
        }
    }
}