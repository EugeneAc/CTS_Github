using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTS_Analytics.Models.Mnemonic
{
    public class shahModel : MineGeneral
    {
        public shahModel(string locationID = "shah") : base(locationID)
        {
            Skip = new Mine_skip(locationID);
            Belt = new Mine_konv(locationID);
            Sklad = new Mine_sklad(locationID);
            Vagon = new Mine_vagon(locationID);
            Crusher = new Mine_crusher(locationID);
            RockUtil = new Mine_rockUtil(locationID);
            Kotel = new Mine_Kotel(locationID);
        }

        public Mine_skip Skip { get; set; }
        public Mine_konv Belt { get; set; }
        public Mine_sklad Sklad { get; set; }
        public Mine_vagon Vagon { get; set; }
        public Mine_crusher Crusher { get; set; }
        public Mine_rockUtil RockUtil { get; set; }
        public Mine_Kotel Kotel { get; set; }

        public override decimal ProdFact { get { return (decimal)Skip.TotalTonnsPerTimeInterval; } }
    }
}