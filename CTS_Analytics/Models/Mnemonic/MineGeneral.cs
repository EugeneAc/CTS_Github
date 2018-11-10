using CTS_Models;
using System;
using System.Collections.Generic;

namespace CTS_Analytics.Models.Mnemonic
{
	public class MineGeneral : iMine
	{
        public class HandLegend
        {
            public int ManualValue { get; set; }
            public int AutomaticValue { get; set; }
            public string DivID { get; set; }
        }

		public string MineName { get; set; }
		public string DetailsViewName { get; set; }

		public readonly string LocationID;
		public decimal Productivity { get; set; }
		public decimal ProdPlan { get; set; }
		public virtual decimal ProdFact { get; set; }
		public float Shipped { get; set; }
		public int AtStockpile { get; set; }
		public bool CoalWarning { get; set; }
		public bool HSEWarning { get; set; }
		public bool CoalAlarm { get; set; }
		public bool HSEAlarm { get; set; }
		public List<WagonTransfer> WahonTransfersJournal { get; set; }
        public string DivId { get; set; }
        public HandLegend HandLeg { get; set; }

        public MineGeneral(string locationID)
		{
			LocationID = locationID;
            HandLeg = new HandLegend();
		}
	}
}