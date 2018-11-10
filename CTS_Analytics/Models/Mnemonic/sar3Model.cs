namespace CTS_Analytics.Models.Mnemonic
{
    public class sar3Model : MineGeneral
    {
        public sar3Model(string locationID = "sar3") : base(locationID)
        {
            Skip = new Mine_skip(locationID);
            BeltToTech1 = new Mine_konv(locationID);
            BeltToTech2 = new Mine_konv(locationID);
            BeltToBoiler = new Mine_konv(locationID);
            Sklad = new Mine_sklad(locationID);
            VagonObogatitel = new Mine_vagon(locationID);
            VagonSaburkhan = new Mine_vagon(locationID);
            Crusher = new Mine_crusher(locationID);
            RockUtil = new Mine_rockUtil(locationID);
            Kotel = new Mine_Kotel(locationID);
        }

        public Mine_skip Skip { get; set; }
        public Mine_skip Skip2 { get; set; }
        public Mine_konv BeltToTech1 { get; set; }
        public Mine_konv BeltToTech2 { get; set; }
        public Mine_konv BeltToBoiler { get; set; }
        public Mine_sklad Sklad { get; set; }
        public Mine_vagon VagonObogatitel { get; set; }
        public Mine_vagon VagonSaburkhan { get; set; }
        public RaspoznModel Raspozn1 { get; set; }
        public RaspoznModel Raspozn2 { get; set; }
        public Mine_crusher Crusher { get; set; }
        public Mine_rockUtil RockUtil { get; set; }
        public Mine_Kotel Kotel { get; set; }

        public override decimal ProdFact { get { return (decimal)Skip.TotalTonnsPerTimeInterval; } }
    }
}