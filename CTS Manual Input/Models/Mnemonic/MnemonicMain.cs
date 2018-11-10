using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTS_Manual_Input.Models.Mnemonic
{
	public class MnemonicMain
	{
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

		public COF COF { get; set; }
	}

	public class MineGeneral
	{
		public float ProdPlan { get; set; }
		public float ProdFact { get; set; }
		public float Shipped { get; set; }
		public float AtStockpile { get; set; }
		public bool CoalWarning { get; set; }
	}

	public class StationGeneral
	{
		public float Income { get; set; }
		public float Outcome { get; set; }
	}

	public class COF
	{
		public float ProdPlan { get; set; }
		public float ProdFact { get; set; }
		public float Shipped { get; set; }
		public float AtStockpile { get; set; }
		public bool CoalWarning { get; set; }
	}
}