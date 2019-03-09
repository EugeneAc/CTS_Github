using CTS_Analytics.Models.Mnemonic;
using CTS_Analytics.Services;
using CTS_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CTS_Analytics.Factories
{
    public class MnemonicModelBuilder
    {
        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly string userLanguage;
        private readonly CentralDBService _cdbService;
        private readonly WagonDbService _wagDbService;
        private readonly DateTime fromDate;
        private readonly DateTime toDate;

        public MnemonicModelBuilder(string userLanguage, DateTime fromDate, DateTime toDate)
        {
            this.userLanguage = userLanguage;
            this._cdbService = new CentralDBService();
            this._wagDbService = new WagonDbService();
            this.fromDate = fromDate;
            this.toDate = toDate;
        }

        public void GetGeneralMineData(MineGeneral model)
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

        public Mine_skip GetSkipModel(int skipID)
        {
            var skip = _cdbService.FindEquipmentById<Skip>(skipID);
            if (skip == null)
            {
                return new Mine_skip("Unknown");
            }

            var model = new Mine_skip(skip.LocationID);
            model.SkipID = skipID;
            model.SkipName = GetEquipNameOnCurrentLanguate(skip);
            model.MineName = GetLocationNameOnCurrentLanguate(skip.LocationID);

            model.SkipTransfers = _cdbService.GetTransfers<SkipTransfer>(skipID, fromDate, toDate).AsEnumerable();
            model.TotalSkipsPerTimeInterval = model.SkipTransfers.Sum(s => int.Parse(s.LiftingID));
            model.TotalTonnsPerTimeInterval = model.SkipTransfers.Select(t => t.SkipWeight * int.Parse(t.LiftingID)).Sum();
            model.LastSkipLiftingTime = model.SkipTransfers.Any() ? model.SkipTransfers.First().TransferTimeStamp : new DateTime?();

            var fromShiftDate = _cdbService.GetStartShiftTime(skip.LocationID);
            var toShiftDate = _cdbService.GetEndShiftTime(skip.LocationID, fromShiftDate);
            var shiftskiptransfers = _cdbService.GetTransfers<SkipTransfer>(skipID, fromShiftDate, toShiftDate).AsEnumerable();
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

        public Mine_konv GetBeltScaleModel(int beltID)
        {
            var belt = _cdbService.FindEquipmentById<BeltScale>(beltID);
            var model = new Mine_konv(belt.LocationID);
            model.BeltID = beltID;
            model.KonvName = GetEquipNameOnCurrentLanguate(belt);
            model.MineName = GetLocationNameOnCurrentLanguate(belt.LocationID);

            model.BeltTransfers = _cdbService.GetTransfers<BeltTransfer>(beltID, fromDate, toDate);
            model.ProductionPerTimeInterval = model.BeltTransfers.Sum(t => t.LotQuantity).GetValueOrDefault();
            var fromShiftDate = _cdbService.GetStartShiftTime(belt.LocationID);
            var toShiftDate = _cdbService.GetEndShiftTime(belt.LocationID, fromShiftDate);
            var shiftransfers = _cdbService.GetTransfers<BeltTransfer>(beltID, fromShiftDate, toShiftDate);
            model.ProductionPerShift = shiftransfers.Sum(t => t.LotQuantity).GetValueOrDefault();
            model.HasManualValues = model
                .BeltTransfers
                .Any(v => v.OperatorName != ProjectConstants.SystemPlarformOperatorName)
                ||
                shiftransfers
                .Any(v => v.OperatorName != ProjectConstants.SystemPlarformOperatorName);
            return model;
        }

        public Mine_vagon GetWagonScaleModel(int wagonScaleID)
        {
            var wagonScale = _cdbService.FindEquipmentById<WagonScale>(wagonScaleID);
            var model = new Mine_vagon(wagonScale.LocationID);
            model.WagonScaleID = wagonScaleID;
            model.WagonScaleName = GetEquipNameOnCurrentLanguate(wagonScale);
            model.MineName = GetLocationNameOnCurrentLanguate(wagonScale.LocationID);

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

            if (lastTrainTransfers.Any())
            {
                lastTrainTransfers = model.WagonTransfers.GroupBy(l => l.LotName)
                                    .First()
                                    .ToList();
                model.LastTrainDirection = lastTrainTransfers.Select(x => x.Direction).FirstOrDefault();
                model.ShippedToLastTrainDateTime = lastTrainTransfers.Select(x => x.TransferTimeStamp).FirstOrDefault();
                model.LastTrainVagonCount = lastTrainTransfers.Count();
                float? sum1 = 0;
                foreach (var t in lastTrainTransfers)
                {
                    var f = t.Netto;
                    if (f.HasValue) sum1 += f.Value;
                }

                model.ShippedToLastTrainTonns = (float)sum1;
            }

            model.HasManualValues = model
                .WagonTransfers
                .Any(v => v.OperatorName != ProjectConstants.DbSyncOperatorName)
                ||
                shiftransfers
                .Any(v => v.OperatorName != ProjectConstants.DbSyncOperatorName);
            return model;
        }

        public Mine_sklad GetWarehouseModel(int warehouseID)
        {
            var warehouse = _cdbService.FindEquipmentById<Warehouse>(warehouseID);
            var model = new Mine_sklad(warehouse.LocationID);
            model.WarehouseID = warehouseID;
            model.MineName = GetLocationNameOnCurrentLanguate(warehouse.LocationID);
            model.WarehouseName = GetEquipNameOnCurrentLanguate(warehouse);
            model.WarehouseTransfers = _cdbService.GetWarehouseTransfers(warehouseID, fromDate, toDate);

            if (model.WarehouseTransfers != null && model.WarehouseTransfers.Any())
            {
                model.AtStockPile = (int)model.WarehouseTransfers?.OrderByDescending(t => t.TransferTimeStamp)
                    .Select(f => f.TotalQuantity)
                    .FirstOrDefault();
                model.IncomePerTimeInterval = (int)model.WarehouseTransfers?.Select(t => t.LotQuantity).Sum();
            }

            model.StockPileCapacity = (int)warehouse.TotalCapacity;
            var fromShiftDate = _cdbService.GetStartShiftTime(warehouse.LocationID);
            var toShiftDate = _cdbService.GetEndShiftTime(warehouse.LocationID, fromShiftDate);
            model.IncomePerShift = (int)_cdbService.GetWarehouseTransfers(warehouseID, fromShiftDate, toShiftDate)
                .Sum(t => t.LotQuantity).GetValueOrDefault();

            return model;
        }

        public Mine_rockUtil GetRockUtilModel(int rockUtilID)
        {
            var rockUtil = _cdbService.FindEquipmentById<RockUtil>(rockUtilID);
            if (rockUtil == null)
            {
                return new Mine_rockUtil("Unknown");
            }

            var model = new Mine_rockUtil(rockUtil.LocationID);
            model.MineName = GetLocationNameOnCurrentLanguate(rockUtil.LocationID);
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
            var shiftTransfers = _cdbService.GetTransfers<RockUtilTransfer>(rockUtilID, fromShiftDate, toShiftDate);
            model.RocksPerShift = shiftTransfers.Sum(s => s.LotQuantity).GetValueOrDefault();

            model.HasManualValues = rockTransfers
                .Any(v => v.OperatorName != ProjectConstants.SystemPlarformOperatorName)
                ||
                shiftTransfers
                .Any(v => v.OperatorName != ProjectConstants.SystemPlarformOperatorName);
            return model;
        }

        public RaspoznModel GetRaspoznModel(int recognID, int takeCount = int.MaxValue, int offset = 0)
        {
            var model = new RaspoznModel();
            model.RaspoznID = recognID;
            model.RaspoznList = _wagDbService.GetWagonNumsAndDates(recognID, fromDate, toDate).ToList();
            for (int i = 0; i < model.RaspoznList.Count(); i++)
            {
                if (i >= offset && i <= offset + takeCount)
                {
                    var idSostav = model.RaspoznList[i].IdSostav; // this is PAIN
                    model.RaspoznList[i] = _wagDbService.GetWagonPhoto(model.RaspoznList[i].WagonNumber, model.RaspoznList[i].Date, model.RaspoznList[i].ManualRecogn);
                    model.RaspoznList[i].IdSostav = idSostav;
                }
            }

            return model;
        }

        public void GetGeneralStationData(Station model)
        {
            model.StationName = GetLocationNameOnCurrentLanguate(model.LocationID);

            model.TotalIncome = _cdbService.GetLocationIncome(model.LocationID, fromDate, toDate);

            model.TotalOutcome = _cdbService.GetLocationOutcome(model.LocationID, fromDate, toDate);
            model.FromAbay = GetFromMineData(model.LocationID, "abay");
            model.FromCof = GetFromMineData(model.LocationID, "cofv");
            model.FromKaz = GetFromMineData(model.LocationID, "kaz");
            model.FromKost = GetFromMineData(model.LocationID, "kost");
            model.FromKuz = GetFromMineData(model.LocationID, "kuz");
            model.FromLen = GetFromMineData(model.LocationID, "len");
            model.FromSar = GetFromMineData(model.LocationID, "sar");
            model.FromShah = GetFromMineData(model.LocationID, "shah");
            model.FromTent = GetFromMineData(model.LocationID, "tent");
            GetStationWagonScalesData(model);
        }

        public Station.FromStationData GetFromMineData(string destLocationId, string fromMineId, bool arrived = false)
        {
            var fromStationData = new Station.FromStationData();
            if (arrived)
            {
                fromStationData.WagonTransfers = _cdbService.GetWagonTransfersArrivedFromMine(fromMineId, fromDate, toDate, destLocationId);
            }
            else
            {
                fromStationData.WagonTransfers = _cdbService.GetWagonTransfersSendFromMine(_cdbService.FindLocationByLocationID(destLocationId), fromDate, toDate, fromMineId);
            }

            var location = _cdbService.FindLocationByLocationID(fromMineId);
            fromStationData.MineID = location.ID;
            fromStationData.MineName = GetLocationNameOnCurrentLanguate(location.ID);

            return fromStationData;
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

        private string GetEquipNameOnCurrentLanguate(IEquip entity)
        {
            var name = entity.Name;
            if (userLanguage == "en")
            {
                name = entity.NameEng;
            }

            if (userLanguage == "kk")
            {
                name = entity.NameKZ;
            }

            return name;
        }

        private void GetStationWagonScalesData(Station model)
        {
            var location = _cdbService.FindLocationByLocationID(model.LocationID);
            model.WagonScales = new Station.Station_wagon();
            model.WagonScales.WagonScalesID = _cdbService.GetWagonScalesIdOnLocation(model.LocationID);

            var wagonScaleModel = GetWagonScaleModel(model.WagonScales.WagonScalesID);
            model.WagonScales.TotalWagons = wagonScaleModel.WagonTransfers.Count();
            model.WagonScales.TotalTonns = (int)wagonScaleModel.WagonTransfers.Select(t => t.Brutto).Sum();
            model.WagonScales.LastTrainDirection = wagonScaleModel.LastTrainDirection;
            model.WagonScales.LastTrainItem = wagonScaleModel.WagonTransfers.FirstOrDefault()?.Item?.Name;
            model.WagonScales.LastTrainNumber = wagonScaleModel.WagonTransfers.FirstOrDefault()?.LotName;
            model.WagonScales.LastTrainTime = wagonScaleModel.WagonTransfers.FirstOrDefault()?.TransferTimeStamp ?? new DateTime();
            model.WagonScales.LastTrainWagonCount = (int)wagonScaleModel.LastTrainVagonCount;
        }
    }
}