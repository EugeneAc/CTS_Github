using CTS_Analytics.Helpers;
using System.Collections.Generic;

namespace CTS_Analytics.Models.Mnemonic
{
	public class MnemonicMain
	{
		public MnemonicMain()
		{
			Shah = new MineGeneral("shah");
			Tent = new MineGeneral("tent");
			Len = new MineGeneral("len");
			Kaz = new MineGeneral("kaz");
			Abay = new MineGeneral("abay");
			Sar1 = new MineGeneral("sar1");
			Sar3 = new MineGeneral("sar3");
			Kuz = new MineGeneral("kuz");
			Kost = new MineGeneral("kost");

			StAbay = new StationGeneral();
			StDubov = new StationGeneral();
			StRaspor = new StationGeneral();
			StUgler = new StationGeneral();

			COF = new COF();
        }

		public MineGeneral Shah { get; set; }
		public MineGeneral Tent { get; set; }
		public MineGeneral Len { get; set; }
		public MineGeneral Kaz { get; set; }
		public MineGeneral Abay { get; set; }
		public MineGeneral Sar1 { get; set; }
		public MineGeneral Sar3 { get; set; }
		public MineGeneral Kuz { get; set; }
		public MineGeneral Kost { get; set; }

		public StationGeneral StAbay { get; set; }
		public StationGeneral StDubov { get; set; }
		public StationGeneral StRaspor { get; set; }
		public StationGeneral StUgler { get; set; }

		public COF COF { get; set; }

        public virtual decimal TotalPlan { get { return Shah.ProdPlan + Tent.ProdPlan + Len.ProdPlan + Kaz.ProdPlan + Abay.ProdPlan + Sar1.ProdPlan + Sar3.ProdPlan + Kuz.ProdPlan + Kost.ProdPlan; } }
		public virtual decimal TotalFact { get { return Shah.ProdFact + Tent.ProdFact + Len.ProdFact + Kaz.ProdFact + Abay.ProdFact + Sar1.ProdFact + Sar3.ProdFact + Kuz.ProdFact + Kost.ProdFact; } }
	}

    public class MnemonicMainMap : MnemonicMain
    {

        public Dictionary<string,AlarmCountModel> AlarmDict { get; set; }
    }


    public class StationGeneral
	{
		public float Income { get; set; }
		public float Outcome { get; set; }
        public string DivId { get; set; }
        public string StationName { get; set; }
        public string StationID { get; set; }

        public bool CoalAlarm { get; set; }
        public bool HSEAlarm { get; set; }
        public bool CoalWarning { get; set; }
        public bool HSEWarning { get; set; }
    }

	public class COF
	{
		public float ProdPlan { get; set; }
		public float ProdFact { get; set; }
		public float Shipped { get; set; }
		public float AtStockpile { get; set; }
        public string DivId { get; set; }

        public bool CoalAlarm { get; set; }
        public bool CoalWarning { get; set; }
    }

}