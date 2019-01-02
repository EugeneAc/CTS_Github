using CTS_Analytics.Models.Mnemonic;
using CTS_Analytics.Services;
using CTS_Models;
using System;
using System.Linq;

namespace CTS_Analytics.Factories
{
    public class MnemonicModelBuilder
    {
        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly string userLanguage;
        private readonly CentralDBService _cdbService;

        public MnemonicModelBuilder(string userLanguage)
        {
            this.userLanguage = userLanguage;
            this._cdbService = new CentralDBService();
        }

        public void GetGeneralData(MineGeneral model, DateTime fromDate, DateTime toDate)
        {
            model.ProdPlan = _cdbService.GetPlanData(model.LocationID, fromDate, toDate);
            var location = _cdbService.FindLocationByLocationID(model.LocationID);
            var oracleShop = (OracleShop)Enum.Parse(typeof(OracleShop), model.LocationID);
            try
            {
                var staffnum = _cdbService.GetStuffCount((int)oracleShop, fromDate, toDate);
                model.Productivity = (float)model.ProdFact / staffnum;
            }
            catch (Exception e)
            {
                _logger.Error(e, "Exception getting data for GetGeneralData");
            }

            model.MineName = GetLocationNameOnCurrentLanguate(model.LocationID);
        }

        public Mine_skip GetSkipModel(int skipID, DateTime fromDate, DateTime toDate)
        {
            var skip = _cdbService.FindEquipmentById<Skip>(skipID);
            if (skip == null)
            {
                return new Mine_skip("Unknown");
            }

            var model = new Mine_skip(skip.LocationID);
            model.SkipID = skipID;
            model.SkipName = skip.Name;
            model.MineName = skip.Location.LocationName;
            if (userLanguage == "en")
            {
                model.SkipName = skip.NameEng;
                model.MineName = skip.Location.LocationNameEng;
            }

            if (userLanguage == "kk")
            {
                model.SkipName = skip.NameKZ;
                model.MineName = skip.Location.LocationNameKZ;
            }

            model.SkipTransfers = _cdbService.GetTransfers<SkipTransfer>(skipID, fromDate, toDate);
            model.TotalSkipsPerTimeInterval = model.SkipTransfers.Sum(s => int.Parse(s.LiftingID));
            model.TotalTonnsPerTimeInterval = model.SkipTransfers.Select(t => t.SkipWeight * int.Parse(t.LiftingID)).Sum();
            model.LastSkipLiftingTime = model.SkipTransfers.Count() > 0 ? model.SkipTransfers.First().TransferTimeStamp : new DateTime?();

            var fromShiftDate = _cdbService.GetStartShiftTime(skip.LocationID);
            var toShiftDate = _cdbService.GetEndShiftTime(skip.LocationID, fromShiftDate);
            var shiftskiptransfers = _cdbService.GetTransfers<SkipTransfer>(skipID, fromShiftDate, toShiftDate);
            model.TotalSkipsPerThisShift = shiftskiptransfers.Sum(s => int.Parse(s.LiftingID));
            model.TotalTonnsPerThisShift = shiftskiptransfers.Select(t => t.SkipWeight * int.Parse(t.LiftingID)).Sum();

            model.HasManualValues = model
                .SkipTransfers
                .Where(v => v.OperatorName != ProjectConstants.SystemPlarformOperatorName)
                .Any()
                ||
                shiftskiptransfers
                .Where(v => v.OperatorName != ProjectConstants.SystemPlarformOperatorName)
                .Any();
            return model;
        }

        public Mine_konv GetBeltScaleModel(int beltID, DateTime fromDate, DateTime toDate)
        {
            var belt = _cdbService.FindEquipmentById<BeltScale>(beltID);
            var model = new Mine_konv(belt.LocationID);
            model.BeltID = beltID;
            model.KonvName = belt.Name;
            model.MineName = belt.Location.LocationName;
            if (userLanguage == "en")
            {
                model.KonvName = belt.NameEng;
                model.MineName = belt.Location.LocationNameEng;
            }

            if (userLanguage == "kk")
            {
                model.KonvName = belt.NameKZ;
                model.MineName = belt.Location.LocationNameKZ;
            }

            model.BeltTransfers = _cdbService.GetTransfers<BeltTransfer>(beltID, fromDate, toDate);
            model.ProductionPerTimeInterval = model.BeltTransfers.Sum(t => t.LotQuantity).GetValueOrDefault();
            var fromShiftDate = _cdbService.GetStartShiftTime(belt.LocationID);
            var toShiftDate = _cdbService.GetEndShiftTime(belt.LocationID, fromShiftDate);
            var shiftransfers = _cdbService.GetTransfers<BeltTransfer>(beltID, fromShiftDate, toShiftDate);
            model.ProductionPerShift = shiftransfers.Sum(t => t.LotQuantity).GetValueOrDefault();
            model.HasManualValues = model
                .BeltTransfers
                .Where(v => v.OperatorName != ProjectConstants.SystemPlarformOperatorName)
                .Any()
                ||
                shiftransfers
                .Where(v => v.OperatorName != ProjectConstants.SystemPlarformOperatorName)
                .Any();
            return model;
        }

        public Mine_vagon GetWagonScaleModel(int wagonScaleID, DateTime fromDate, DateTime toDate)
        {
            var wagonScale = _cdbService.FindEquipmentById<WagonScale>(wagonScaleID);
            var model = new Mine_vagon(wagonScale.LocationID);
            model.WagonScaleID = wagonScaleID;
            model.WagonScaleName = wagonScale.Name;
            model.MineName = wagonScale.Location.LocationName;

            model.WagonTransfers = _cdbService.GetTransfers<WagonTransfer>(wagonScaleID, fromDate, toDate);
            //model.WagonTransfers.AddRange(GetDataFromWagonDB(fromDate, toDate, wagonScale.LocationID)); // To get data from wagonDB
            var fromShiftDate = _cdbService.GetStartShiftTime(wagonScale.LocationID);
            var toShiftDate = _cdbService.GetEndShiftTime(wagonScale.LocationID, fromShiftDate);
            var shiftransfers = _cdbService.GetTransfers<WagonTransfer>(wagonScaleID, fromShiftDate, toShiftDate)
                .Where(tr => tr.Direction == CTS_Core.ProjectConstants.WagonDirection_FromObject)
                .ToArray();
            //var shiftransfers = GetDataFromWagonDB(fromShiftDate, toShiftDate, wagonScale.LocationID); // To get data from wagonDB
            model.ShippedPerShiftTonns = (float)shiftransfers.Sum(t => t.Netto);
            var lastTrainTransfers = model.WagonTransfers.OrderByDescending(t => t.TransferTimeStamp).ToList();

            if (lastTrainTransfers != null && lastTrainTransfers.Count > 0)
            {
                lastTrainTransfers = model.WagonTransfers.GroupBy(l => l.LotName)
                                    .First()
                                    .ToList();
                model.LastTrainDirection = lastTrainTransfers.FirstOrDefault().ToDest;
                model.ShippedToLastTrainDateTime = lastTrainTransfers.FirstOrDefault().TransferTimeStamp;
                model.LastTrainVagonCount = lastTrainTransfers.Count();
                model.ShippedToLastTrainTonns = (float)lastTrainTransfers.Sum(t => t.Netto);
            }

            if (userLanguage == "en")
            {
                model.WagonScaleName = wagonScale.NameEng;
                model.MineName = wagonScale.Location.LocationNameEng;
            }

            if (userLanguage == "kk")
            {
                model.WagonScaleName = wagonScale.NameKZ;
                model.MineName = wagonScale.Location.LocationNameKZ;
            }

            model.HasManualValues = model
                .WagonTransfers
                .Where(v => v.OperatorName != ProjectConstants.DbSyncOperatorName)
                .Any()
                ||
                shiftransfers
                .Where(v => v.OperatorName != ProjectConstants.DbSyncOperatorName)
                .Any();
            return model;
        }

        public Mine_sklad GetWarehouseModel(int warehouseID,  DateTime fromDate, DateTime toDate)
        {
            var warehouse = _cdbService.FindEquipmentById<Warehouse>(warehouseID);
            var model = new Mine_sklad(warehouse.LocationID);
            model.WarehouseID = warehouseID;
            model.MineName = GetLocationNameOnCurrentLanguate(warehouse.LocationID);
            model.WarehouseName = warehouse.Name;
            if (userLanguage == "en")
            {
                model.WarehouseName = warehouse.NameEng;
            }

            if (userLanguage == "kk")
            {
                model.WarehouseName = warehouse.NameKZ;
            }
            model.WarehouseTransfers = _cdbService.GetWarehouseTransfers(warehouseID, fromDate, toDate);

            if (model.WarehouseTransfers != null && model.WarehouseTransfers.Count() > 0)
            {
                model.AtStockPile = (int)model.WarehouseTransfers?.OrderByDescending(t => t.TransferTimeStamp)
                    .Select(f => f.TotalQuantity)
                    .FirstOrDefault();
                model.IncomePerTimeInterval = (int)model.WarehouseTransfers?.Select(t => t.LotQuantity).Sum();
            }

            model.StockPileCapacity = (int)warehouse.TotalCapacity;
            var fromShiftDate = _cdbService.GetStartShiftTime(warehouse.LocationID);
            var toShiftDate = _cdbService.GetEndShiftTime(warehouse.LocationID, fromShiftDate);
            var shiftransfers = _cdbService.GetWarehouseTransfers(warehouseID, fromShiftDate, toShiftDate)
                .Select(t => t.LotQuantity);
            model.IncomePerShift = (int)shiftransfers?.Sum();

            return model;
        }

        public Mine_rockUtil GetRockUtilModel(int rockUtilID, DateTime fromDate, DateTime toDate)
        {
            var rockUtil = _cdbService.FindEquipmentById<RockUtil>(rockUtilID);
            if (rockUtil == null)
            {
                return new Mine_rockUtil("Unknown");
            }

            var model = new Mine_rockUtil(rockUtil.LocationID);
            model.MineName = rockUtil.Location.LocationName;
            model.RockUtilName = rockUtil.Name;
            if (userLanguage == "en")
            {
                model.MineName = rockUtil.Location.LocationNameEng;
                model.RockUtilName = rockUtil.NameEng;
            }

            if (userLanguage == "kk")
            {
                model.MineName = rockUtil.Location.LocationNameKZ;
                model.RockUtilName = rockUtil.NameKZ;
            }
            var rockTransfers = _cdbService.GetTransfers<RockUtilTransfer>(rockUtilID, fromDate, toDate);

            model.RocksPerPeriod = rockTransfers.Sum(v => v.LotQuantity) ?? 0;
            var fromShiftDate = _cdbService.GetStartShiftTime(rockUtil.LocationID);
            var toShiftDate = _cdbService.GetEndShiftTime(rockUtil.LocationID, fromShiftDate);
            var shiftTransfers = _cdbService.GetTransfers<RockUtilTransfer>(rockUtilID, fromShiftDate, toShiftDate)
                .ToArray();
            model.RocksPerShift = shiftTransfers.Sum(s => s.LotQuantity) ?? 0;

            model.HasManualValues = rockTransfers
                .Where(v => v.OperatorName != ProjectConstants.SystemPlarformOperatorName)
                .Any()
                ||
                shiftTransfers
                .Where(v => v.OperatorName != ProjectConstants.SystemPlarformOperatorName)
                .Any();
            return model;
        }

        private string GetLocationNameOnCurrentLanguate(string locationID)
        {
            var location = _cdbService.FindLocationByLocationID(locationID);
            var name = location.LocationName;
            if (userLanguage == "en")
            {
                name = location.LocationNameEng;
            }

            if (userLanguage == "kk")
            {
                name = location.LocationNameKZ;
            }

            return name;
        }
    }
}