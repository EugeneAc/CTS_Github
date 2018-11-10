using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTS_Analytics.Models.Mnemonic
{
    public class tentModel : MineGeneral
    {
        public tentModel(string locationID = "tent") : base(locationID)
        {
            SkipPos1 = new Mine_skip(locationID);
            SkipPos2 = new Mine_skip(locationID);
            BeltToTechComplex = new Mine_konv(locationID);
            Sklad = new Mine_sklad(locationID);
            Vagon = new Mine_vagon(locationID);
            RockUtil = new Mine_rockUtil(locationID);
            Kotel = new Mine_Kotel(locationID);
        }

        public Mine_skip SkipPos1 { get; set; }
        public Mine_skip SkipPos2 { get; set; }
        public Mine_konv BeltToTechComplex { get; set; }
        public Mine_sklad Sklad { get; set; }
        public Mine_vagon Vagon { get; set; }
        public Mine_rockUtil RockUtil { get; set; }
        public Mine_Kotel Kotel { get; set; }

        public override decimal ProdFact { get { return (decimal)BeltToTechComplex.ProductionPerTimeInterval; } }
    }
}