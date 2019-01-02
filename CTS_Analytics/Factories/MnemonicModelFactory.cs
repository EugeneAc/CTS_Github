using CTS_Analytics.Models.Mnemonic;
using CTS_Analytics.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
            var skip = _cdbService.FindSkipBySkipId(skipID);
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

            model.SkipTransfers = _cdbService.GetSkipTransfers(skipID, fromDate, toDate);
            model.TotalSkipsPerTimeInterval = model.SkipTransfers.Sum(s => int.Parse(s.LiftingID));
            model.TotalTonnsPerTimeInterval = model.SkipTransfers.Select(t => t.SkipWeight * int.Parse(t.LiftingID)).Sum();
            model.LastSkipLiftingTime = model.SkipTransfers.Count() > 0 ? model.SkipTransfers.First().TransferTimeStamp : new DateTime?();

            var fromShiftDate = _cdbService.GetStartShiftTime(skip.LocationID);
            var toShiftDate = _cdbService.GetEndShiftTime(skip.LocationID, fromShiftDate);
            var shiftskiptransfers = _cdbService.GetSkipTransfers(skipID, fromShiftDate, toShiftDate);
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