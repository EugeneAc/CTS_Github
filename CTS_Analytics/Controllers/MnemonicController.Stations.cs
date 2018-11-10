using CTS_Analytics.Models.Mnemonic;
using CTS_Models.DBContext;
using System;
using System.Linq;
using System.Web.Mvc;

namespace CTS_Analytics.Controllers
{
    public partial class MnemonicController : Controller
    {
        public ActionResult cofv()
        {
            var model = new cofvModel("cofv");
            var fromDate = GetDateFromCookie("fromdate");
            var toDate = GetDateFromCookie("todate");
            using (var db = new CtsDbContext())
            {
                GetGeneralStationData(model, fromDate, toDate, db);
            }

            return View(model);
        }

        public ActionResult sdub()
        {
            var model = new sdubModel("sdub");
            var fromDate = GetDateFromCookie("fromdate");
            var toDate = GetDateFromCookie("todate");
            using (var db = new CtsDbContext())
            {
                GetGeneralStationData(model, fromDate, toDate, db);
            }

            return View(model);
        }

        public ActionResult sabay()
        {
            var model = new sabayModel("sabay");
            var fromDate = GetDateFromCookie("fromdate");
            var toDate = GetDateFromCookie("todate");
            using (var db = new CtsDbContext())
            {
                GetGeneralStationData(model, fromDate, toDate, db);
            }

            return View(model);
        }

        public ActionResult sprd()
        {
            var model = new sprdModel("sprd");
            var fromDate = GetDateFromCookie("fromdate");
            var toDate = GetDateFromCookie("todate");
            using (var db = new CtsDbContext())
            {
                GetGeneralStationData(model, fromDate, toDate, db);
                model.Sklad = GetWarehouseModel(1, db, fromDate, toDate); // TODO поменять ID когда заведем склад в базе
            }

            return View(model);
        }

        public ActionResult sugl()
        {
            var model = new suglModel("sugl");
            var fromDate = GetDateFromCookie("fromdate");
            var toDate = GetDateFromCookie("todate");
            using (var db = new CtsDbContext())
            {
                GetGeneralStationData(model, fromDate, toDate, db);
            }

            return View(model);
        }

        public ActionResult srasp()
        {
            var model = new sraspModel("srasp");
            var fromDate = GetDateFromCookie("fromdate");
            var toDate = GetDateFromCookie("todate");
            using (var db = new CtsDbContext())
            {
                GetGeneralStationData(model, fromDate, toDate, db);
                model.Sklad = GetWarehouseModel(10, db, fromDate, toDate); 
            }

            return View(model);
        }

        private void GetStationWagonScalesData(Station model, DateTime fromDate, DateTime toDate, CtsDbContext db)
        {
            var location = db.Locations.Find(model.LocationID);
            model.WagonScales = new Station.Station_wagon();
            model.WagonScales.WagonScalesID = db.WagonScales
                .Where(m => m.Location.ID == model.LocationID)
                .FirstOrDefault()?
                .ID ?? 1;
            var wagonScaleModel = GetWagonScaleModel(model.WagonScales.WagonScalesID, db, fromDate, toDate);
            model.WagonScales.TotalWagons = wagonScaleModel.WagonTransfers.Count();
            model.WagonScales.TotalTonns = (int)wagonScaleModel.WagonTransfers.Select(t => t.Brutto).Sum();
            model.WagonScales.LastTrainDirection = wagonScaleModel.LastTrainDirection;
            model.WagonScales.LastTrainItem = wagonScaleModel.WagonTransfers.FirstOrDefault()?.Item?.Name;
            model.WagonScales.LastTrainNumber = wagonScaleModel.WagonTransfers.FirstOrDefault()?.LotName;
            model.WagonScales.LastTrainTime = wagonScaleModel.WagonTransfers.FirstOrDefault()?.TransferTimeStamp ?? new DateTime();
            model.WagonScales.LastTrainWagonCount = (int)wagonScaleModel.LastTrainVagonCount;
        }

        private void GetGeneralStationData(Station model, DateTime fromDate, DateTime toDate, CtsDbContext db)
        {
            var location = db.Locations.Find(model.LocationID);
            model.StationName = location.LocationName;
            if (getUserLang(Request.Cookies["lang"]) == "en")
            {
                model.StationName = location.LocationNameEng;
            }

            if (getUserLang(Request.Cookies["lang"]) == "kk")
            {
                model.StationName = location.LocationNameKZ;
            }

            model.TotalIncome = db.WagonTransfers
                .Where(t => t.TransferTimeStamp >= fromDate && t.TransferTimeStamp <= toDate)
                .Where(d => d.ToDest == model.LocationID && d.IsValid == true)
                .Select(t => t.Brutto)
                .DefaultIfEmpty()
                .Sum();
            model.TotalOutcome = db.WagonTransfers
                .Where(t => t.TransferTimeStamp >= fromDate && t.TransferTimeStamp <= toDate)
                .Where(d => d.Equip.Location.ID == model.LocationID && d.IsValid == true)
                .Select(t => t.Brutto)
                .DefaultIfEmpty()
                .Sum();
            model.FromAbay = GetFromMineData(model, fromDate, toDate, db, "abay");
            model.FromCof = GetFromMineData(model, fromDate, toDate, db, "cofv");
            model.FromKaz = GetFromMineData(model, fromDate, toDate, db, "kaz");
            model.FromKost = GetFromMineData(model, fromDate, toDate, db, "kost");
            model.FromKuz = GetFromMineData(model, fromDate, toDate, db, "kuz");
            model.FromLen = GetFromMineData(model, fromDate, toDate, db, "len");
            model.FromSar = GetFromMineData(model, fromDate, toDate, db, "sar1");
            model.FromSar.WagonTransfers.AddRange(db.WagonTransfers
                .Where(t => t.Equip.Location.ID == "sar3" && t.ToDest == model.LocationID && t.IsValid == true)
                .Where(t => t.TransferTimeStamp >= fromDate && t.TransferTimeStamp <= toDate));
            model.FromShah = GetFromMineData(model, fromDate, toDate, db, "shah");
            model.FromTent = GetFromMineData(model, fromDate, toDate, db, "tent");
            GetStationWagonScalesData(model, fromDate, toDate, db);
        }

        private Station.FromStationData GetFromMineData (Station model, DateTime fromDate, DateTime toDate, CtsDbContext db, string mineId)
        {
            var fromStationData = new Station.FromStationData();
            fromStationData.WagonTransfers = db.WagonTransfers
                .Where(t => t.Equip.Location.ID == mineId && t.ToDest == model.LocationID && t.IsValid == true)
                .Where(t => t.TransferTimeStamp >= fromDate && t.TransferTimeStamp <= toDate).ToList();
            var location = db.Locations.Find(mineId);
            fromStationData.MineID = location.ID;
            fromStationData.MineName = location.LocationName;
            if (getUserLang(Request.Cookies["lang"]) == "en")
            {
                fromStationData.MineName = location.LocationNameEng;
            }

            if (getUserLang(Request.Cookies["lang"]) == "kk")
            {
                fromStationData.MineName = location.LocationNameKZ;
            }
            
            return fromStationData;
        }

        private string GetLocationNameOnCurrentLanguate(string locationID)
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
    }
}