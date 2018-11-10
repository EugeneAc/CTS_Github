namespace CTS_Analytics.Models.Mnemonic
{
    public class abayModel : MineGeneral
    {
        public abayModel(string locationID = "abay") : base(locationID)
        {
            Skip1 = new Mine_skip(locationID);
            Skip2 = new Mine_skip(locationID);
            BeltPos1 = new Mine_konv(locationID);
            BeltPos9 = new Mine_konv(locationID);
            BeltBoiler = new Mine_konv(locationID);
            Sklad = new Mine_sklad(locationID);
            Vagon = new Mine_vagon(locationID);
            Crusher = new Mine_crusher(locationID);
            RockUtil = new Mine_rockUtil(locationID);
            Kotel = new Mine_Kotel(locationID);
        }

        public Mine_skip Skip1 { get; set; }
        public Mine_skip Skip2 { get; set; }
        public Mine_konv BeltPos1 { get; set; }
        public Mine_konv BeltPos9 { get; set; }
        public Mine_konv BeltBoiler { get; set; }
        public Mine_sklad Sklad { get; set; }
        public Mine_vagon Vagon { get; set; }
        public RaspoznModel Raspozn { get; set; }
        public Mine_crusher Crusher { get; set; }
        public Mine_rockUtil RockUtil { get; set; }
        public Mine_Kotel Kotel { get; set; }

        public override decimal ProdFact { get { return (decimal)Skip1.TotalTonnsPerTimeInterval + (decimal)Skip2.TotalTonnsPerTimeInterval; } }
    }
}