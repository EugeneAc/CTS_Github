using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTS_Analytics.Models.Mnemonic
{
    public class kostModel : MineGeneral
    {
        public kostModel(string locationID = "kost") : base(locationID)
        {
            Skip6t = new Mine_skip(locationID);
            Skip9t = new Mine_skip(locationID);
            BeltFromSkip6t = new Mine_konv(locationID);
            BeltFromSkip9t = new Mine_konv(locationID);
            BeltToSklad = new Mine_konv(locationID);
            BeltToVagon = new Mine_konv(locationID);
            BeltToBoiler = new Mine_konv(locationID);
            Sklad = new Mine_sklad(locationID);
            Vagon = new Mine_vagon(locationID);
            Crusher = new Mine_crusher(locationID);
            Crusher2 = new Mine_crusher(locationID);
            RockUtil38 = new Mine_rockUtil(locationID);
            RockUtil39 = new Mine_rockUtil(locationID);
            Kotel = new Mine_Kotel(locationID);
            Raspozn1 = new RaspoznModel();
            Raspozn2 = new RaspoznModel();
        }

        public Mine_skip Skip6t { get; set; }
        public Mine_skip Skip9t { get; set; }
        public Mine_konv BeltFromSkip6t { get; set; }
        public Mine_konv BeltFromSkip9t { get; set; }
        public Mine_konv BeltToSklad { get; set; }
        public Mine_konv BeltToVagon { get; set; }
        public Mine_konv BeltToBoiler { get; set; }
        public Mine_sklad Sklad { get; set; }
        public Mine_vagon Vagon { get; set; }
        public Mine_crusher Crusher { get; set; }
        public Mine_crusher Crusher2 { get; set; }
        public Mine_rockUtil RockUtil38 { get; set; }
        public Mine_rockUtil RockUtil39 { get; set; }
        public Mine_Kotel Kotel { get; set; }
        public RaspoznModel Raspozn1 { get; set; }
        public RaspoznModel Raspozn2 { get; set; }

        public override decimal ProdFact { get { return (decimal)Skip6t.TotalTonnsPerTimeInterval + (decimal)Skip9t.TotalTonnsPerTimeInterval; } }
    }
}