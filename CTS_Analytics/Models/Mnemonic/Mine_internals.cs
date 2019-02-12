using CTS_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTS_Analytics.Models.Mnemonic
{
	public class Mine_internals: iMine
    {
		public string ReturnID { get; set; }
        public string MineName { get; set; }
        public string DivID { get; set; }
        public bool HasManualValues { get; set; }
        public string DetailsViewName { get; set; }
        public int ManualValue { get; set; }
        public int Automatic { get; set; }
        public bool FilterManualInput { get; set; }
        public bool OrderByTransferTimeStampAsc { get; set; }

        public Mine_internals(string locaitonID)
        {
            ReturnID = locaitonID;
        }
    }

	public class Mine_skip : Mine_internals
	{
        public Mine_skip(string locationID) : base(locationID)
        {
            
        }

        
        public float TotalSkipsPerThisShift { get; set; }
		public float TotalTonnsPerThisShift { get; set; }
		public float TotalSkipsPerTimeInterval { get; set; }
		public float TotalTonnsPerTimeInterval { get; set; }
		public DateTime? LastSkipLiftingTime { get; set; }
        public IEnumerable<SkipTransfer> SkipTransfers { get; set; }
        public PagedList.IPagedList<SkipTransfer> PagedkipTransfers { get; set; }
		public string SkipName { get; set; }
        public int SkipID { get; set; }
    }

	public class Mine_konv : Mine_internals
	{
        public Mine_konv(string locationID):base(locationID)
        {

        }

		public string KonvName { get; set; }
        public int BeltID { get; set; }
        public float ProductionPerShift { get; set; }
		public float ProductionPerTimeInterval { get; set; }
		public IEnumerable<BeltTransfer> BeltTransfers { get; set; }
        public PagedList.IPagedList<BeltTransfer> PagedBeltTransfers { get; set; }
    }

	public class Mine_sklad : Mine_internals
    {
        public Mine_sklad(string locationID) : base(locationID)
        { }

        public int WarehouseID { get; set; }
        public string WarehouseName { get; set; }
        public int IncomePerShift { get; set; }
		public int IncomePerTimeInterval { get; set; }
		public int StockPileCapacity { get; set; }
		public int AtStockPile { get; set; }
        public IEnumerable<WarehouseTransfer> WarehouseTransfers { get; set; }
        public PagedList.IPagedList<WarehouseTransfer> PagedWarehouseTransfers { get; set; }
    }

    public class Mine_crusher : Mine_internals
    {
        public Mine_crusher(string locationID) : base(locationID)
        {
        }

        public float CrushedPerShift { get; set; }
        public float CoalPerShift { get; set; }
        public float RocksPerShift { get; set; }
        public bool CoalAlarm { get; set; }
    }

    public class Mine_rockUtil : Mine_internals
    {
        public Mine_rockUtil(string locationID) : base(locationID)
        {
        }

        public float RocksPerPeriod { get; set; }
        public float RocksPerShift { get; set; }
        public bool MinHeight { get; set; }
        public string RockUtilName { get; set; }
    }

    public class Mine_Kotel : Mine_internals
    {
        public Mine_Kotel(string locationID) : base(locationID)
        {
        }

        public float ConsumptionNorm { get; set; }
        public float ConsumptionFact { get; set; }
        public bool CoalAlarm { get; set; }
    }

    public class Mine_vagon : Mine_internals
	{
        public Mine_vagon(string locationID):base(locationID)
        {
        }

        public int WagonScaleID { get; set; }
        public string WagonScaleName { get; set; }
        public float ShippedPerShiftTonns { get; set; }
		public float ShippedToLastTrainTonns { get; set; }
		public DateTime? ShippedToLastTrainDateTime { get; set; }
		public float LastTrainVagonCount { get; set; }
        public string LastTrainDirection { get; set; }
        public IEnumerable<WagonTransfer> WagonTransfers { get; set; }
        public PagedList.StaticPagedList<WagonTransfersAndPhoto> PagedWagonTrasnfersAndPhotos { get; set; }
        public List<RaspoznItem> WagonPictureList { get; set; }
        public string WagonNumberFilter { get; set; }
    }

    public class WagonTransfersAndPhoto
    {
        public WagonTransfer WagonTransfer { get; set; }
        public RaspoznItem Photo { get; set; }
    }

    public class Mine_raspozn : Mine_internals
    {
        public Mine_raspozn(string locationID) : base(locationID)
        {
        }

        public int RaspoznID { get; set; }
        public string RaspoznName { get; set; }
        public int WagonsPassed  { get; set; }
        public int RecognPercent { get; set; }
        public DateTime? LastTrainDateTime { get; set; }
        public int LastTrainWagonCount { get; set; }
        public RaspoznModel RaspoznTable { get; set; }
        public PagedList.IPagedList<RaspoznItem> PagedRaspoznTable { get; set; }
        public string WagonNumberFilter { get; set; }
    }
}