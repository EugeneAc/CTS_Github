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

        public void GetGeneralData(MineGeneral model)
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
            model.LastSkipLiftingTime = model.SkipTransfers.Any() ? model.SkipTransfers.First().TransferTimeStamp : new DateTime?();

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

        public Mine_konv GetBeltScaleModel(int beltID)
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
            float? sum = 0;
            foreach (var f in shiftransfers.Select(t =>
            {
                if (t.Netto != null)
                    return t.Netto;
                else
                    return 0;
            }))
            {
                sum += f.Value;
            }

            model.ShippedPerShiftTonns = (float)sum;
            var lastTrainTransfers = model.WagonTransfers.OrderByDescending(t => t.TransferTimeStamp).ToList();

            if (lastTrainTransfers.Any())
            {
                lastTrainTransfers = model.WagonTransfers.GroupBy(l => l.LotName)
                                    .First()
                                    .ToList();
                model.LastTrainDirection = lastTrainTransfers.Select(x => x.ToDest).FirstOrDefault();
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

            if (model.WarehouseTransfers != null && model.WarehouseTransfers.Any())
            {
                double? first = null;
                foreach (var f in model.WarehouseTransfers?.OrderByDescending(t => t.TransferTimeStamp))
                {
                    first = f.TotalQuantity;
                    break;
                }

                if (first != null) model.AtStockPile = (int) first;
                float? sum = 0;
                foreach (var f in model.WarehouseTransfers?.Select(t =>
                {
                    if (t.LotQuantity != null)
                        return t.LotQuantity;
                    else
                        return 0;
                }))
                {
                    sum += f.Value;
                }

                model.IncomePerTimeInterval = (int)sum;
            }

            model.StockPileCapacity = (int)warehouse.TotalCapacity;
            var fromShiftDate = _cdbService.GetStartShiftTime(warehouse.LocationID);
            var toShiftDate = _cdbService.GetEndShiftTime(warehouse.LocationID, fromShiftDate);
            var shiftransfers = _cdbService.GetWarehouseTransfers(warehouseID, fromShiftDate, toShiftDate)
                .Select(t => t.LotQuantity);
            model.IncomePerShift = int.Parse(shiftransfers?.Sum().ToString());

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
            model.RaspoznList = _wagDbService.GetWagonNumsAndDates(recognID, fromDate, toDate)
                .Select(s => new RaspoznItem
            {
                Date = s.Date,
                WagonNumber = s.WagonNum,
                ManualRecogn = s.ManualFlag,
                IdSostav = s.IdSostav
            })
            .ToList();
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