using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTS_Analytics.Models.Mnemonic
{
	public class kuzModel : MineGeneral
	{
		public kuzModel(string locationID = "kuz") : base(locationID)
		{
			SkipPos1 = new Mine_skip(locationID);
			SkipPos2 = new Mine_skip(locationID);
			BeltPos44 = new Mine_konv(locationID);
            BeltPos1L80_1 = new Mine_konv(locationID);
            BeltPos1L80_2 = new Mine_konv(locationID);
            kuzSklad = new Mine_sklad(locationID);
            kuzSklad = new Mine_sklad(locationID);
			Vagon = new Mine_vagon(locationID);
            Crusher = new Mine_crusher(locationID);
            RockUtil = new Mine_rockUtil(locationID);
            Kotel = new Mine_Kotel(locationID);
            Sklad = new Mine_sklad(locationID);
            Raspozn = new RaspoznModel();
        }
		
		public Mine_skip SkipPos1 { get; set; }
		public Mine_skip SkipPos2 { get; set; }
		public Mine_konv BeltPos44 { get; set; }
        public Mine_konv BeltPos1L80_1 { get; set; }
        public Mine_konv BeltPos1L80_2 { get; set; }
        public Mine_sklad kuzSklad { get; set; }
		public Mine_vagon Vagon { get; set; }
        public Mine_crusher Crusher { get; set; }
        public Mine_rockUtil RockUtil { get; set; }
        public Mine_Kotel Kotel { get; set; }
        public Mine_sklad Sklad { get; set; }
        public RaspoznModel Raspozn { get; set; }

        public override decimal ProdFact { get { return (decimal)SkipPos1.TotalTonnsPerTimeInterval + (decimal)SkipPos2.TotalTonnsPerTimeInterval; } }
    }
}