using CTS_Models;
using CTS_Models.DBContext;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using StackExchange.Profiling;

namespace CTS_Analytics.Services
{
    public class CentralDBService
    {
        private readonly CtsDbContext cdb;


        public CentralDBService()
        {
            this.cdb = new CtsDbContext();
           
        }

        public decimal GetPlanData(string location, DateTime fromDate, DateTime toDate)
        {
            var planSum = cdb.LocalPlansBWithLocationID
                .Where(l => location.Contains(l.LocationID))
                .Where(d => d.Date >= fromDate && d.Date <= toDate)
                .Select(p => p.Plan)
                .DefaultIfEmpty()
                .Sum();

            if (location == "sar1" || location == "sar3")
                planSum = planSum / 2;

            return planSum;
        }

        public int GetStuffCount(int location, DateTime fromDate, DateTime toDate)
        {
            var staffnum = cdb.LocalStaffs
                    .Where(s => s.ShopID == location)
                    .Where(d => d.Date <= toDate && d.Date >= fromDate)
                    .Select(v => v.CNT)
                    .DefaultIfEmpty()
                    .Select(int.Parse)
                    .ToArray()
                    .Sum();

            return staffnum;
        }

        public Location FindLocationByLocationID(string locationId)
        {
            return cdb.Locations.Find(locationId); ;
        }

        public TEquip FindEquipmentById<TEquip>(int Id) where TEquip : class, IEquip
        {
            var bd = new CtsEquipContext<TEquip>();
            return bd.DbSet.Find(Id);
        }

        public DateTime GetStartShiftTime(string locationId)
        {
            var nowTime = TimeSpan.Parse(System.DateTime.Now.ToString("HH:mm:ss"));
            var times = cdb.Shifts
                .Where(l => l.LocationID == locationId)
                .ToArray();
            if (times.Length == 0)
                return DateTime.Today;

            var startTime = times
                .Where(t => t.TimeStart < nowTime)
                .OrderByDescending(c => c.TimeStart)
                .Select(f => f.TimeStart)
                .FirstOrDefault();
            if (startTime == default(TimeSpan))
            {
                nowTime = new TimeSpan(23, 59, 59);
                startTime = times
                .Where(t => t.TimeStart < nowTime)
                .OrderByDescending(c => c.TimeStart)
                .Select(f => f.TimeStart)
                .First();
                return DateTime.Today.AddDays(-1).Add(startTime);
            }

            return DateTime.Today.Add(startTime);
        }

        public DateTime GetEndShiftTime(string locationId, DateTime startShiftTime)
        {
            var nowTime = TimeSpan.Parse(startShiftTime.ToString("HH:mm:ss"));
            var times = cdb.Shifts
                .Where(l => l.LocationID == locationId)
                .ToArray();
            if (times.Length == 0)
                return startShiftTime.AddHours(24);

            var endTime = times
                .Where(t => t.TimeStart > nowTime)
                .OrderByDescending(c => c.TimeStart)
                .Select(f => f.TimeStart)
                .LastOrDefault();

            if (endTime == default(TimeSpan))
            {
                nowTime = new TimeSpan(0, 00, 1);
                endTime = times
                .Where(t => t.TimeStart > nowTime)
                .OrderByDescending(c => c.TimeStart)
                .Select(f => f.TimeStart)
                .Last();
                return DateTime.Today.AddDays(1).Add(endTime);
            }

            return System.DateTime.Today.Add(endTime);
        }

        public IQueryable<TTransfer> GetTransfers<TTransfer>(int Id, DateTime fromDate, DateTime toDate) where TTransfer : class, ITransfer
        {
            CtsTransferContext<TTransfer> db = null;
            using (MiniProfiler.Current.Step("EF Stuff"))
            {
                db = new CtsTransferContext<TTransfer>();
                return db.DbSet
                   .Where(s => s.EquipID == Id)
                   .Where(d => d.TransferTimeStamp >= fromDate && d.TransferTimeStamp <= toDate)
                   .Where(v => v.IsValid == true)
                   .OrderByDescending(t => t.TransferTimeStamp)
                   .AsNoTracking();
            }
        }

        public IQueryable<WagonTransfer> GetWagonTransfersIncludeLocations(int? wagonScaleID, DateTime fromDate, DateTime toDate)
        {
            return cdb.WagonTransfers.Include(w => w.Equip.Location)
                .Where(s => wagonScaleID != null ? s.EquipID == wagonScaleID : true)
                .Where(d => d.TransferTimeStamp >= fromDate && d.TransferTimeStamp <= toDate)
                .Where(v => v.IsValid == true)
                .OrderByDescending(t => t.TransferTimeStamp)
                .AsNoTracking();
        }

        public IQueryable<WarehouseTransfer> GetWarehouseTransfers(int Id, DateTime fromDate, DateTime toDate)
        {
            return cdb.WarehouseTransfers
               .Where(s => s.WarehouseID == Id)
               .Where(d => d.TransferTimeStamp >= fromDate && d.TransferTimeStamp <= toDate)
               .OrderByDescending(t => t.TransferTimeStamp)
               .AsNoTracking();
        }

        public IQueryable<Location> GetAlllocaitons()
        {
            return cdb.Locations;
        }
    }
}