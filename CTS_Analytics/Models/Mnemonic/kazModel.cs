using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTS_Analytics.Models.Mnemonic
{
    public class kazModel : MineGeneral
    {
        public kazModel(string locationID = "kaz") : base(locationID)
        {
            SkipPos1 = new Mine_skip(locationID);
            SkipPos2 = new Mine_skip(locationID);
            Belt1 = new Mine_konv(locationID);
            Belt2 = new Mine_konv(locationID);
            Sklad = new Mine_sklad(locationID);
            Vagon = new Mine_vagon(locationID);
            Crusher = new Mine_crusher(locationID);
            Crusher2 = new Mine_crusher(locationID);
            RockUtil = new Mine_rockUtil(locationID);
            Kotel = new Mine_Kotel(locationID);
        }

        public Mine_skip SkipPos1 { get; set; }
        public Mine_skip SkipPos2 { get; set; }
        public Mine_konv Belt1 { get; set; }
        public Mine_konv Belt2 { get; set; }
        public Mine_sklad Sklad { get; set; }
        public Mine_vagon Vagon { get; set; }
        public RaspoznModel Raspozn { get; set; }
        public Mine_crusher Crusher { get; set; }
        public Mine_crusher Crusher2 { get; set; }
        public Mine_rockUtil RockUtil { get; set; }
        public Mine_Kotel Kotel { get; set; }

        public override decimal ProdFact { get { return (decimal)SkipPos1.TotalTonnsPerTimeInterval + (decimal)SkipPos2.TotalTonnsPerTimeInterval; } }
    }
}